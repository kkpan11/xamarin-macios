//
// CFHTTPMessage.cs:
//
// Authors:
//      Martin Baulig (martin.baulig@gmail.com)
//      Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012-2014 Xamarin Inc. (http://www.xamarin.com)
//

#nullable enable

using System;
using System.Net;
using System.Security.Authentication;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CFNetwork {
	/// <summary>An HTTP message.</summary>
	///     <remarks>To be added.</remarks>
	public partial class CFHTTPMessage : CFType {
		[Preserve (Conditional = true)]
		internal CFHTTPMessage (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CFNetworkLibrary, EntryPoint = "CFHTTPMessageGetTypeID")]
		public extern static /* CFTypeID */ nint GetTypeID ();

		static IntPtr GetVersion (Version? version)
		{
			if ((version is null) || version.Equals (HttpVersion.Version11))
				return _HTTPVersion1_1;

			if (version.Equals (HttpVersion.Version10))
				return _HTTPVersion1_0;

			if (version.Major == 3 && version.Minor == 0) {
				// HTTP 3.0 requires OS X 10.16 or later.
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'CFHTTPMessage._HTTPVersion3_0.get' is only supported on: 'ios' 14.0 and later, 'tvos' 14.0 and later.
				if (_HTTPVersion3_0 != IntPtr.Zero)
					return _HTTPVersion3_0;
#pragma warning restore CA1416
				else if (_HTTPVersion2_0 != IntPtr.Zero)
					return _HTTPVersion2_0;
				else
					return _HTTPVersion1_1;
			}

			if (version.Major == 2 && version.Minor == 0) {
				// HTTP 2.0 requires OS X 10.11 or later.
				if (_HTTPVersion2_0 != IntPtr.Zero)
					return _HTTPVersion2_0;
				else
					return _HTTPVersion1_1;
			}

			if (_HTTPVersion1_1 != IntPtr.Zero)
				return _HTTPVersion1_1;
			// not supporting version 1.1 is something to worry about
			throw new ArgumentException ();
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHTTPMessageRef __nonnull */ IntPtr CFHTTPMessageCreateEmpty (
			/* CFAllocatorRef __nullable */ IntPtr alloc, /* Boolean */ byte isRequest);

		public static CFHTTPMessage CreateEmpty (bool request)
		{
			var handle = CFHTTPMessageCreateEmpty (IntPtr.Zero, request ? (byte) 1 : (byte) 0);
			return new CFHTTPMessage (handle, true);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHTTPMessageRef __nonnull */ IntPtr CFHTTPMessageCreateRequest (
			/* CFAllocatorRef __nullable */ IntPtr alloc, /* CFStringRef __nonnull*/ IntPtr requestMethod,
			/* CFUrlRef __nonnull */ IntPtr url, /* CFStringRef __nonnull */ IntPtr httpVersion);

		public static CFHTTPMessage CreateRequest (CFUrl url, NSString method, Version? version)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			if (method is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (method));

			var handle = CFHTTPMessageCreateRequest (
				IntPtr.Zero, method.Handle, url.Handle, GetVersion (version));
			GC.KeepAlive (method);
			GC.KeepAlive (url);
			return new CFHTTPMessage (handle, true);
		}

		public static CFHTTPMessage CreateRequest (Uri uri, string method)
		{
			return CreateRequest (uri, method, null);
		}

		public static CFHTTPMessage CreateRequest (Uri uri, string method, Version? version)
		{
			if (uri is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (uri));

			// the method is obsolete, but EscapeDataString does not work the same way. We could get the components
			// of the Uri and then EscapeDataString, but this might introduce bugs, so for now we will ignore the warning
#pragma warning disable SYSLIB0013
			var escaped = Uri.EscapeUriString (uri.ToString ());
#pragma warning restore SYSLIB0013

			using var urlRef = CFUrl.FromUrlString (escaped, null);
			if (urlRef is null)
				throw new ArgumentException ("Invalid URL.");
			using var methodRef = new NSString (method);

			return CreateRequest (urlRef, methodRef, version);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPMessageIsRequest (/* CFHTTPMessageRef __nonnull */ IntPtr message);

		public bool IsRequest {
			get {
				return CFHTTPMessageIsRequest (GetCheckedHandle ()) != 0;
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFIndex */ nint CFHTTPMessageGetResponseStatusCode (
			/* CFHTTPMessageRef __nonnull */ IntPtr response);

		public HttpStatusCode ResponseStatusCode {
			get {
				if (IsRequest)
					throw new InvalidOperationException ();
				int status = (int) CFHTTPMessageGetResponseStatusCode (Handle);
				return (HttpStatusCode) status;
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFStringRef __nullable */ IntPtr CFHTTPMessageCopyResponseStatusLine (
			/* CFHTTPMessageRef __nonnull */ IntPtr response);

		public string? ResponseStatusLine {
			get {
				if (IsRequest)
					throw new InvalidOperationException ();
				var ptr = CFHTTPMessageCopyResponseStatusLine (Handle);
				if (ptr == IntPtr.Zero)
					return null;
				return CFString.FromHandle (ptr, true);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFStringRef __nonnull */ IntPtr CFHTTPMessageCopyVersion (
			/* CFHTTPMessageRef __nonnull */ IntPtr message);

		public Version Version {
			get {
				var ptr = CFHTTPMessageCopyVersion (GetCheckedHandle ());
				try {
					// FIXME: .NET HttpVersion does not include (yet) Version20, so Version11 is returned
					if (ptr == _HTTPVersion1_0)
						return HttpVersion.Version10;
					else
						return HttpVersion.Version11;
				} finally {
					if (ptr != IntPtr.Zero)
						CFObject.CFRelease (ptr);
				}
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPMessageIsHeaderComplete (
			/* CFHTTPMessageRef __nonnull */ IntPtr message);

		public bool IsHeaderComplete {
			get {
				return CFHTTPMessageIsHeaderComplete (GetCheckedHandle ()) != 0;
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		unsafe extern static /* Boolean */ byte CFHTTPMessageAppendBytes (
			/* CFHTTPMessageRef __nonnull */ IntPtr message,
			/* const UInt8* __nonnull */ byte* newBytes, /* CFIndex */ nint numBytes);

		public bool AppendBytes (byte [] bytes)
		{
			if (bytes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bytes));

			unsafe {
				fixed (byte* byteptr = bytes) {
					return CFHTTPMessageAppendBytes (Handle, byteptr, bytes.Length) != 0;
				}
			}
		}

		public bool AppendBytes (byte [] bytes, nint count)
		{
			if (bytes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bytes));

			if (bytes.Length < count)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (count), count, "Must not be longer than array length");

			unsafe {
				fixed (byte* byteptr = bytes) {
					return CFHTTPMessageAppendBytes (Handle, byteptr, count) != 0;
				}
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFDictionaryRef __nullable */ IntPtr CFHTTPMessageCopyAllHeaderFields (
			/* CFHTTPMessageRef __nonnull */ IntPtr message);

		public NSDictionary? GetAllHeaderFields ()
		{
			return Runtime.GetNSObject<NSDictionary> (CFHTTPMessageCopyAllHeaderFields (GetCheckedHandle ()));
		}

		#region Authentication

		// CFStream.h
		struct CFStreamError {
			public /* CFIndex (CFStreamErrorDomain) */ nint domain;
			public /* SInt32 */ int code;
		}

		// untyped enum -> CFHTTPAuthentication.h
		enum CFStreamErrorHTTPAuthentication {
			TypeUnsupported = -1000,
			BadUserName = -1001,
			BadPassword = -1002,
		}

		AuthenticationException GetException (CFStreamErrorHTTPAuthentication code)
		{
			switch (code) {
			case CFStreamErrorHTTPAuthentication.BadUserName:
				throw new InvalidCredentialException ("Bad username.");
			case CFStreamErrorHTTPAuthentication.BadPassword:
				throw new InvalidCredentialException ("Bad password.");
			case CFStreamErrorHTTPAuthentication.TypeUnsupported:
				throw new AuthenticationException ("Authentication type not supported.");
			default:
				throw new AuthenticationException ("Unknown error.");
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		unsafe extern static /* Boolean */ byte CFHTTPMessageApplyCredentials (/* CFHTTPMessageRef */ IntPtr request,
			/* CFHTTPAuthenticationRef */ IntPtr auth, /* CFString */ IntPtr username, /* CFString */ IntPtr password,
			/* CFStreamError* */ CFStreamError* error);

		public void ApplyCredentials (CFHTTPAuthentication? auth, NetworkCredential credential)
		{
			if (auth is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (auth));
			if (credential is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (credential));

			if (auth.RequiresAccountDomain) {
				ApplyCredentialDictionary (auth, credential);
				return;
			}

			var username = CFString.CreateNative (credential.UserName);
			var password = CFString.CreateNative (credential.Password);
			try {
				byte ok;
				CFStreamError error;
				unsafe {
					ok = CFHTTPMessageApplyCredentials (Handle, auth.Handle, username, password, &error);
					GC.KeepAlive (auth);
				}
				if (ok == 0)
					throw GetException ((CFStreamErrorHTTPAuthentication) error.code);
			} finally {
				CFString.ReleaseNative (username);
				CFString.ReleaseNative (password);
			}
		}

		// convenience enum on top of kCFHTTPAuthenticationScheme* fields
		/// <summary>An enumeration whose values specify HTTP authentication schemes.</summary>
		///     <remarks>To be added.</remarks>
		public enum AuthenticationScheme {
			Default,
			Basic,
			Negotiate,
			NTLM,
			Digest,
#if !XAMCORE_5_0
			[SupportedOSPlatform ("macos")]
			[SupportedOSPlatform ("ios")]
			[SupportedOSPlatform ("tvos")]
			[SupportedOSPlatform ("maccatalyst")]
			[ObsoletedOSPlatform ("tvos12.0", "Not available anymore.")]
			[ObsoletedOSPlatform ("macos10.14", "Not available anymore.")]
			[ObsoletedOSPlatform ("ios12.0", "Not available anymore.")]
			[ObsoletedOSPlatform ("maccatalyst13.1", "Not available anymore.")]
			OAuth1,
#endif
		}

		internal static IntPtr GetAuthScheme (AuthenticationScheme scheme)
		{
			switch (scheme) {
			case AuthenticationScheme.Default:
				return IntPtr.Zero;
			case AuthenticationScheme.Basic:
				return _AuthenticationSchemeBasic;
			case AuthenticationScheme.Negotiate:
				return _AuthenticationSchemeNegotiate;
			case AuthenticationScheme.NTLM:
				return _AuthenticationSchemeNTLM;
			case AuthenticationScheme.Digest:
				return _AuthenticationSchemeDigest;
			default:
				throw new ArgumentException ();
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* Boolean */ byte CFHTTPMessageAddAuthentication (
			/* CFHTTPMessageRef __nonnull */ IntPtr request,
			/* CFHTTPMessageRef __nullable */ IntPtr authenticationFailureResponse,
			/* CFStringRef __nonnull */ IntPtr username,
			/* CFStringRef __nonnull */ IntPtr password,
			/* CFStringRef __nullable */ IntPtr authenticationScheme,
			/* Boolean */ byte forProxy);

		public bool AddAuthentication (CFHTTPMessage failureResponse, NSString username,
									   NSString password, AuthenticationScheme scheme,
									   bool forProxy)
		{
			if (username is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (username));
			if (password is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (password));

			bool result = CFHTTPMessageAddAuthentication (
				Handle, failureResponse.GetHandle (), username.Handle,
				password.Handle, GetAuthScheme (scheme), forProxy ? (byte) 1 : (byte) 0) != 0;
			GC.KeepAlive (failureResponse);
			GC.KeepAlive (username);
			GC.KeepAlive (password);
			return result;
		}

		[DllImport (Constants.CFNetworkLibrary)]
		unsafe extern static /* Boolean */ byte CFHTTPMessageApplyCredentialDictionary (/* CFHTTPMessageRef */ IntPtr request,
			/* CFHTTPAuthenticationRef */ IntPtr auth, /* CFDictionaryRef */ IntPtr dict, /* CFStreamError* */ CFStreamError* error);

		public void ApplyCredentialDictionary (CFHTTPAuthentication auth, NetworkCredential credential)
		{
			if (auth is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (auth));
			if (credential is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (credential));

			var length = credential.Domain is null ? 2 : 3;
			var keys = new NSString [length];
			var values = new CFString [length];
			keys [0] = _AuthenticationUsername;
			keys [1] = _AuthenticationPassword;
			values [0] = credential.UserName!;
			values [1] = credential.Password!;
			if (credential.Domain is not null) {
				keys [2] = _AuthenticationAccountDomain;
				values [2] = credential.Domain;
			}

			var dict = CFDictionary.FromObjectsAndKeys (values, keys);

			try {
				CFStreamError error;
				byte ok;
				unsafe {
					ok = CFHTTPMessageApplyCredentialDictionary (
						Handle, auth.Handle, dict.Handle, &error);
					GC.KeepAlive (auth);
					GC.KeepAlive (dict);
				}
				if (ok != 0)
					return;
				throw GetException ((CFStreamErrorHTTPAuthentication) error.code);
			} finally {
				dict.Dispose ();
				values [0]?.Dispose ();
				values [1]?.Dispose ();
				values [2]?.Dispose ();
			}
		}

		#endregion

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFHTTPMessageSetHeaderFieldValue (/* CFHTTPMessageRef __nonnull */IntPtr message,
			/* CFStringRef __nonnull */ IntPtr headerField, /* CFStringRef __nullable */ IntPtr value);

		public void SetHeaderFieldValue (string name, string value)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var nameHandle = CFString.CreateNative (name);
			var valueHandle = CFString.CreateNative (value);
			try {
				CFHTTPMessageSetHeaderFieldValue (Handle, nameHandle, valueHandle);
			} finally {
				CFString.ReleaseNative (nameHandle);
				CFString.ReleaseNative (valueHandle);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static void CFHTTPMessageSetBody (/* CFHTTPMessageRef __nonnull */ IntPtr message,
			/* CFDataRef__nonnull  */ IntPtr data);

		public void SetBody (byte [] buffer)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));

			using (var data = new CFDataBuffer (buffer))
				CFHTTPMessageSetBody (Handle, data.Handle);
		}
	}
}
