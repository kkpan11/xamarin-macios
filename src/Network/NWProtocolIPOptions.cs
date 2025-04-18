//
// NWProtocolTls: Bindings the Netowrk nw_protocol_options API focus on TLS options.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using Security;
using OS_nw_protocol_definition = System.IntPtr;
using OS_nw_protocol_options = System.IntPtr;
using IntPtr = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolIPOptions : NWProtocolOptions {
		[Preserve (Conditional = true)]
		internal NWProtocolIPOptions (NativeHandle handle, bool owns) : base (handle, owns) { }

		public void SetVersion (NWIPVersion version)
			=> nw_ip_options_set_version (GetCheckedHandle (), version);

		public void SetHopLimit (nuint hopLimit)
			=> nw_ip_options_set_hop_limit (GetCheckedHandle (), (byte) hopLimit);

		public void SetUseMinimumMtu (bool useMinimumMtu)
			=> nw_ip_options_set_use_minimum_mtu (GetCheckedHandle (), useMinimumMtu.AsByte ());

		public void SetDisableFragmentation (bool disableFragmentation)
			=> nw_ip_options_set_disable_fragmentation (GetCheckedHandle (), disableFragmentation.AsByte ());

		public void SetCalculateReceiveTime (bool shouldCalculateReceiveTime)
			=> nw_ip_options_set_calculate_receive_time (GetCheckedHandle (), shouldCalculateReceiveTime.AsByte ());

		public void SetIPLocalAddressPreference (NWIPLocalAddressPreference localAddressPreference)
			=> nw_ip_options_set_local_address_preference (GetCheckedHandle (), localAddressPreference);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_ip_options_set_disable_multicast_loopback (OS_nw_protocol_options options, byte disableMulticastLoopback);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public void DisableMulticastLoopback (bool disable)
			=> nw_ip_options_set_disable_multicast_loopback (GetCheckedHandle (), disable.AsByte ());
	}
}
