using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using OS_nw_group_descriptor = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;

#nullable enable

namespace Network {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWMultiplexGroup : NWMulticastGroup {

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_group_descriptor nw_group_descriptor_create_multiplex (OS_nw_endpoint remoteEndpoint);

		[Preserve (Conditional = true)]
		internal NWMultiplexGroup (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWMultiplexGroup (NWEndpoint endpoint)
			: base (nw_group_descriptor_create_multiplex (endpoint.GetCheckedHandle ()), true)
		{
			GC.KeepAlive (endpoint);
		}
	}
}
