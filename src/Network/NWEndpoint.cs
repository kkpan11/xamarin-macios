//
// NWEndpoint.cs: Bindings the Netowrk nw_endpoint_t API.
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_endpoint = System.IntPtr;
using OS_nw_txt_record = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]

	public class NWEndpoint : NativeObject {
		[Preserve (Conditional = true)]
		internal NWEndpoint (NativeHandle handle, bool owns) : base (handle, owns) { }

#if !COREBUILD
		[DllImport (Constants.NetworkLibrary)]
		extern static NWEndpointType nw_endpoint_get_type (OS_nw_endpoint handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NWEndpointType Type => nw_endpoint_get_type (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		extern static OS_nw_endpoint nw_endpoint_create_host (IntPtr hostname, IntPtr port);

		/// <param name="hostname">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NWEndpoint? Create (string hostname, string port)
		{
			if (hostname is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (hostname));
			if (port is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (port));
			using var hostnamePtr = new TransientString (hostname);
			using var portPtr = new TransientString (port);
			var handle = nw_endpoint_create_host (hostnamePtr, portPtr);
			if (handle == IntPtr.Zero)
				return null;
			return new NWEndpoint (handle, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_endpoint_get_hostname (OS_nw_endpoint endpoint);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Hostname => Marshal.PtrToStringAnsi (nw_endpoint_get_hostname (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary, EntryPoint = "nw_endpoint_copy_port_string")]
		static extern IntPtr nw_endpoint_copy_port_string_ptr (OS_nw_endpoint endpoint);

		static string nw_endpoint_copy_port_string (OS_nw_endpoint endpoint)
		{
			var ptr = nw_endpoint_copy_port_string_ptr (endpoint);
			return TransientString.ToStringAndFree (ptr)!;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Port => nw_endpoint_copy_port_string (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_endpoint_get_port (OS_nw_endpoint endpoint);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ushort PortNumber => nw_endpoint_get_port (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_endpoint nw_endpoint_create_address (IntPtr sockaddrPtr);

		// TODO: .NET has a SocketAddress type, we could use it for simplicity, but the
		// address family would have to be mapped, and it does not look like a very useful
		// type to begin with.

		[DllImport (Constants.NetworkLibrary, EntryPoint = "nw_endpoint_copy_address_string")]
		static extern IntPtr nw_endpoint_copy_address_string_ptr (OS_nw_endpoint endpoint);

		static string nw_endpoint_copy_address_string (OS_nw_endpoint endpoint)
		{
			var ptr = nw_endpoint_copy_address_string_ptr (endpoint);
			return TransientString.ToStringAndFree (ptr)!;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string Address => nw_endpoint_copy_address_string (GetCheckedHandle ());

#if false
	// need to sort out sockaddr binding.
		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr /* struct sockaddr* */ nw_endpoint_get_address (OS_nw_endpoint endpoint);
#endif

		// TODO: same
		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe OS_nw_endpoint nw_endpoint_create_bonjour_service (IntPtr name, IntPtr type, IntPtr domain);

		/// <param name="name">To be added.</param>
		///         <param name="serviceType">To be added.</param>
		///         <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NWEndpoint? CreateBonjourService (string name, string serviceType, string domain)
		{
			if (serviceType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (serviceType));
			using var namePtr = new TransientString (name);
			using var serviceTypePtr = new TransientString (serviceType);
			using var domainPtr = new TransientString (domain);
			var x = nw_endpoint_create_bonjour_service (namePtr, serviceTypePtr, domainPtr);
			if (x == IntPtr.Zero)
				return null;
			return new NWEndpoint (x, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe IntPtr nw_endpoint_get_bonjour_service_name (OS_nw_endpoint endpoint);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? BonjourServiceName => Marshal.PtrToStringAnsi (nw_endpoint_get_bonjour_service_name (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_endpoint_get_bonjour_service_type (OS_nw_endpoint endpoint);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? BonjourServiceType => Marshal.PtrToStringAnsi (nw_endpoint_get_bonjour_service_type (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_endpoint_get_bonjour_service_domain (OS_nw_endpoint endpoint);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? BonjourServiceDomain => Marshal.PtrToStringAnsi (nw_endpoint_get_bonjour_service_domain (GetCheckedHandle ()));

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		static extern OS_nw_endpoint nw_endpoint_create_url (IntPtr url);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static NWEndpoint? Create (string url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			using var urlPtr = new TransientString (url);
			var handle = nw_endpoint_create_url (urlPtr);
			if (handle == IntPtr.Zero)
				return null;
			return new NWEndpoint (handle, owns: true);
		}

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr nw_endpoint_get_url (OS_nw_endpoint endpoint);

		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public string? Url => Marshal.PtrToStringAnsi (nw_endpoint_get_url (GetCheckedHandle ()));

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern unsafe byte* nw_endpoint_get_signature (OS_nw_endpoint endpoint, nuint* out_signature_length);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public ReadOnlySpan<byte> Signature {
			get {
				unsafe {
					nuint length;
					var data = nw_endpoint_get_signature (GetCheckedHandle (), &length);
					var mValue = new ReadOnlySpan<byte> (data, (int) length);
					// we do not know who manages the byte array, so we return a copy, is more expensive but
					// safer until we know what is the mem management.
					return new ReadOnlySpan<byte> (mValue.ToArray ());
				}
			}
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_txt_record nw_endpoint_copy_txt_record (OS_nw_endpoint endpoint);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public NWTxtRecord? TxtRecord {
			get {
				var record = nw_endpoint_copy_txt_record (GetCheckedHandle ());
				if (record == IntPtr.Zero)
					return null;
				return new NWTxtRecord (record, owns: true);
			}
		}

		internal NWEndpoint []? FromNSArrayHandle (IntPtr handle)
		{
			return NSArray.ArrayFromHandle<NWEndpoint> (handle);
		}

		/// <summary>Returns an autoreleased NSArray handle.</summary>
		internal IntPtr ToNSArrayHandle (NWEndpoint [] array)
		{
			using var rv = NSArray.FromNSObjects (array);
			if (rv is null)
				return IntPtr.Zero;
#pragma warning disable RBI0014
			rv.DangerousRetain ();
			rv.DangerousAutorelease ();
			return rv.Handle;
#pragma warning restore RBI0014
		}
#endif // !COREBUILD

	}
}
