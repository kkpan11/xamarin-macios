//
// NWIPMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an IP.
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
	public class NWIPMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWIPMetadata (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWIPMetadata () : this (nw_ip_create_metadata (), owns: true) { }

		public NWIPEcnFlag EcnFlag {
			get => nw_ip_metadata_get_ecn_flag (GetCheckedHandle ());
			set => nw_ip_metadata_set_ecn_flag (GetCheckedHandle (), value);
		}

		// A single tick represents one hundred nanoseconds, API returns: the time at which a packet was received, in nanoseconds
		// so we get nanoseconds, divide by 100 and use from ticks
		public TimeSpan ReceiveTime {
			get {
				var time = nw_ip_metadata_get_receive_time (GetCheckedHandle ());
				return TimeSpan.FromTicks ((long) time / 100);
			}
		}

		public NWServiceClass ServiceClass {
			get => nw_ip_metadata_get_service_class (GetCheckedHandle ());
			set => nw_ip_metadata_set_service_class (GetCheckedHandle (), value);
		}
	}
}
