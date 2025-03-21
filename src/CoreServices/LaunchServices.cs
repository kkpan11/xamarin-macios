//
// LaunchServices.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//
// NOTE: intentionally passing IntPtr.Zero to all
// 'out NSError' APIs since errors return NULL anyway,
// and the NSError objects are specified to be a
// constant error object (from the docs).
// 
// In other words, A NULL return value implies
// ApplicationNotFoundso we just drop the
// 'out NSError' parameter to make the API nicer.
//
// NOTE: only bound APIs not deprecated in 10.11
//
// NOTE: KEEP IN SYNC WITH TESTS!

#nullable enable

#if MONOMAC

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreServices {
	[Flags]
	public enum LSRoles/*Mask*/ : uint /* always 32-bit uint */
	{
		None = 1,
		Viewer = 2,
		Editor = 4,
		Shell = 8,
		All = 0xffffffff,
	}

	[Flags]
	public enum LSAcceptanceFlags : uint /* always 32-bit uint */
	{
		Default = 1,
		AllowLoginUI = 2,
	}

	public enum LSResult {
		Success = 0,
#if NET
		[SupportedOSPlatform ("macos13.0")]
#else
		[Mac (13,0)]
#endif
		MalformedLocErr = -10400,
		AppInTrash = -10660,
		ExecutableIncorrectFormat = -10661,
		AttributeNotFound = -10662,
		AttributeNotSettable = -10663,
		IncompatibleApplicationVersion = -10664,
		NoRosettaEnvironment = -10665,
		Unknown = -10810,
		NotAnApplication = -10811,
		NotInitialized = -10812,
		DataUnavailable = -10813,
		ApplicationNotFound = -10814,
		UnknownType = -10815,
		DataTooOld = -10816,
		Data = -10817,
		LaunchInProgress = -10818,
		NotRegistered = -10819,
		AppDoesNotClaimType = -10820,
		AppDoesNotSupportSchemeWarning = -10821,
		ServerCommunication = -10822,
		CannotSetInfo = -10823,
		NoRegistrationInfo = -10824,
		IncompatibleSystemVersion = -10825,
		NoLaunchPermission = -10826,
		NoExecutable = -10827,
		NoClassicEnvironment = -10828,
		MultipleSessionsNotSupported = -10829,
	}

#if NET
	[SupportedOSPlatform ("macos")]
#endif
	public static class LaunchServices {
		#region Locating an Application

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultApplicationURLForURL (IntPtr inUrl, LSRoles inRole, /*out*/ IntPtr outError);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static NSUrl? GetDefaultApplicationUrlForUrl (NSUrl url, LSRoles roles = LSRoles.All)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var result = Runtime.GetNSObject<NSUrl> (
				LSCopyDefaultApplicationURLForURL (url.Handle, roles, IntPtr.Zero)
			);
			GC.KeepAlive (url);
			return result;
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultApplicationURLForContentType (IntPtr inContentType, LSRoles inRole, /*out*/ IntPtr outError);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static NSUrl? GetDefaultApplicationUrlForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return Runtime.GetNSObject<NSUrl> (
					LSCopyDefaultApplicationURLForContentType (contentTypeHandle, roles, IntPtr.Zero)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyApplicationURLsForURL (IntPtr inUrl, LSRoles inRole);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static NSUrl [] GetApplicationUrlsForUrl (NSUrl url, LSRoles roles = LSRoles.All)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var result = NSArray.ArrayFromHandle<NSUrl> (
				LSCopyApplicationURLsForURL (url.Handle, roles)
			);
			GC.KeepAlive (url);
			return result;
		}

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern LSResult LSCanURLAcceptURL (IntPtr inItemUrl, IntPtr inTargetUrl,
			LSRoles inRole, LSAcceptanceFlags inFlags, byte* outAcceptsItem);

		// NOTE: intentionally inverting the status results (return bool, with an out
		// LSResult vs return LSResult with an out bool) to make the API nicer to use
		public static bool CanUrlAcceptUrl (NSUrl itemUrl, NSUrl targetUrl,
			LSRoles roles, LSAcceptanceFlags acceptanceFlags, out LSResult result)
		{
			if (itemUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (itemUrl));
			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			byte acceptsItem;
			unsafe {
				result = LSCanURLAcceptURL (itemUrl.Handle, targetUrl.Handle, roles, acceptanceFlags, &acceptsItem);
				GC.KeepAlive (itemUrl);
				GC.KeepAlive (targetUrl);
			}
			return acceptsItem != 0;
		}

		public static bool CanUrlAcceptUrl (NSUrl itemUrl, NSUrl targetUrl,
			LSRoles roles = LSRoles.All, LSAcceptanceFlags acceptanceFlags = LSAcceptanceFlags.Default)
		{
			LSResult result;
			return CanUrlAcceptUrl (itemUrl, targetUrl, roles, acceptanceFlags, out result);
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyApplicationURLsForBundleIdentifier (IntPtr inBundleIdentifier, /*out*/ IntPtr outError);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static NSUrl [] GetApplicationUrlsForBundleIdentifier (string bundleIdentifier)
		{
			if (bundleIdentifier is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundleIdentifier));

			var bundleIdentifierHandle = CFString.CreateNative (bundleIdentifier);
			try {
				return NSArray.ArrayFromHandle<NSUrl> (
					LSCopyApplicationURLsForBundleIdentifier (bundleIdentifierHandle, IntPtr.Zero)
				);
			}
			finally {
				CFString.ReleaseNative (bundleIdentifierHandle);
			}
		}

		#endregion

		#region Opening Items

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern LSResult LSOpenCFURLRef (IntPtr inUrl, void** outLaunchedUrl);

		public unsafe static LSResult Open (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			LSResult result = LSOpenCFURLRef (url.Handle, (void**) 0);
			GC.KeepAlive (url);
			return result;
		}

		public unsafe static LSResult Open (NSUrl url, out NSUrl? launchedUrl)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			void* launchedUrlHandle;
			var result = LSOpenCFURLRef (url.Handle, &launchedUrlHandle);
			GC.KeepAlive (url);
			launchedUrl = Runtime.GetNSObject<NSUrl> (new IntPtr (launchedUrlHandle));
			return result;
		}

		#endregion

		#region Registering an Application

		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSRegisterURL (IntPtr inUrl, byte inUpdate);

		public static LSResult Register (NSUrl url, bool update)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			LSResult result = LSRegisterURL (url.Handle, (byte) (update ? 1 : 0));
			GC.KeepAlive (url);
			return result;
		}

		#endregion

		#region Working with Role Handlers

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyAllRoleHandlersForContentType (IntPtr inContentType, LSRoles inRole);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static string? []? GetAllRoleHandlersForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return CFArray.StringArrayFromHandle (
					LSCopyAllRoleHandlersForContentType (contentTypeHandle, roles)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultRoleHandlerForContentType (IntPtr inContentType, LSRoles inRole);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static string GetDefaultRoleHandlerForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return (string) Runtime.GetNSObject<NSString> (
					LSCopyDefaultRoleHandlerForContentType (contentTypeHandle, roles)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSSetDefaultRoleHandlerForContentType (IntPtr inContentType,
			LSRoles inRole, IntPtr inHandlerBundleID);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		// NOTE: intentionally swapped handlerBundleId and roles parameters for a nicer API
		public static LSResult SetDefaultRoleHandlerForContentType (string contentType, string handlerBundleId,
			LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));
			if (handlerBundleId is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handlerBundleId));

			var contentTypeHandle = CFString.CreateNative (contentType);
			var handlerBundleIdHandle = CFString.CreateNative (handlerBundleId);
			try {
				return LSSetDefaultRoleHandlerForContentType (
					contentTypeHandle,
					roles,
					handlerBundleIdHandle
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
				CFString.ReleaseNative (handlerBundleIdHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15")]
#else
		[Deprecated (PlatformName.MacOSX, 10,15)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyAllHandlersForURLScheme (IntPtr inUrlScheme);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GetApplicationUrlsForUrl' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10,15, message: "Use 'GetApplicationUrlsForUrl' instead.")]
#endif
		public static string? []? GetAllHandlersForUrlScheme (string urlScheme)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			try {
				return CFArray.StringArrayFromHandle (
					LSCopyAllHandlersForURLScheme (urlSchemeHandle)
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15")]
#else
		[Deprecated (PlatformName.MacOSX, 10,15)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultHandlerForURLScheme (IntPtr inUrlScheme);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GetDefaultApplicationUrlForUrl' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10,15, message: "Use 'GetDefaultApplicationUrlForUrl' instead.")]
#endif
		public static string GetDefaultHandlerForUrlScheme (string urlScheme)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			try {
				return (string) Runtime.GetNSObject<NSString> (
					LSCopyDefaultHandlerForURLScheme (urlSchemeHandle)
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
			}
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSSetDefaultHandlerForURLScheme (IntPtr inUrlScheme, IntPtr inHandlerBundleId);

#if NET
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
#else
		[Deprecated (PlatformName.MacOSX, 14, 0)]
#endif
		public static LSResult SetDefaultHandlerForUrlScheme (string urlScheme, string handlerBundleId)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));
			if (handlerBundleId is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handlerBundleId));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			var handlerBundleIdHandle = CFString.CreateNative (handlerBundleId);
			try {
				return LSSetDefaultHandlerForURLScheme (
					urlSchemeHandle,
					handlerBundleIdHandle
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
				CFString.ReleaseNative (handlerBundleIdHandle);
			}
		}

		#endregion
	}
}

#endif // MONOMAC
