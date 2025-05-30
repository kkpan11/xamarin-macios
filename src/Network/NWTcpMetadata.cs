//
// NWTcpMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an Tcp.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace Network {
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWTcpMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWTcpMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		public uint AvailableReceiveBuffer => nw_tcp_get_available_receive_buffer (GetCheckedHandle ());

		public uint AvailableSendBuffer => nw_tcp_get_available_send_buffer (GetCheckedHandle ());
	}
}
