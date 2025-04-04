//
// MonoMac.CoreServices.CFHost
//
// Authors:
//      Martin Baulig (martin.baulig@xamarin.com)
//
// Copyright 2012-2015 Xamarin Inc. (http://www.xamarin.com)
//

#nullable enable

using System;
using System.Net;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CFNetwork {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("tvos15.0", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("maccatalyst15.0", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("macos12.0", Constants.UseNetworkInstead)]
	[ObsoletedOSPlatform ("ios15.0", Constants.UseNetworkInstead)]
	class CFHost : NativeObject {
		[Preserve (Conditional = true)]
		internal CFHost (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHostRef __nonnull */ IntPtr CFHostCreateWithAddress (
			/* CFAllocatorRef __nullable */ IntPtr allocator, /* CFDataRef __nonnull */ IntPtr addr);

		public static CFHost Create (IPEndPoint endpoint)
		{
			// CFSocketAddress will throw the ANE
			using (var data = new CFSocketAddress (endpoint))
				return new CFHost (CFHostCreateWithAddress (IntPtr.Zero, data.Handle), true);
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHostRef __nonnull */ IntPtr CFHostCreateWithName (
			/* CFAllocatorRef __nullable */ IntPtr allocator, /* CFStringRef __nonnull */ IntPtr hostname);

		public static CFHost Create (string name)
		{
			// CFString will throw the ANE
			using (var ptr = new CFString (name))
				return new CFHost (CFHostCreateWithName (IntPtr.Zero, ptr.Handle), true);
		}
	}
}
