#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_relay_hop = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;
using OS_nw_protocol_options = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos17.0")]
	[SupportedOSPlatform ("macos14.0")]
	[SupportedOSPlatform ("ios17.0")]
	[SupportedOSPlatform ("maccatalyst17.0")]
	public class NWRelayHop : NativeObject {
		[Preserve (Conditional = true)]
		internal NWRelayHop (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_relay_hop nw_relay_hop_create (/*[NullAllowed]*/ OS_nw_endpoint http3_relay_endpoint, /*[NullAllowed]*/ OS_nw_endpoint http2_relay_endpoint, /* [NullAllowed]*/ OS_nw_protocol_options relay_tls_options);

		public static NWRelayHop? Create (NWEndpoint? http3RelayEndpoint, NWEndpoint? http2RelayEndpoint,
			NWProtocolOptions? relayTlsOptions)
		{
			var handle = nw_relay_hop_create (http3RelayEndpoint.GetHandle (), http2RelayEndpoint.GetHandle (), relayTlsOptions.GetHandle ());
			GC.KeepAlive (http3RelayEndpoint);
			GC.KeepAlive (http2RelayEndpoint);
			GC.KeepAlive (relayTlsOptions);
			if (handle == NativeHandle.Zero)
				return default;
			return new NWRelayHop (handle, owns: true);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_relay_hop_add_additional_http_header_field (OS_nw_relay_hop relay_hop, /* sbyte* */ IntPtr field_name, /* sbyte* */ IntPtr field_value);

		public void AddAdditionalHttpHeaderField (string fieldName, string fieldValue)
		{
			if (fieldName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fieldName));
			if (fieldValue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fieldValue));

			using var fieldNamePtr = new TransientString (fieldName);
			using var fieldValuePtr = new TransientString (fieldValue);
			nw_relay_hop_add_additional_http_header_field (GetCheckedHandle (), fieldNamePtr, fieldValuePtr);
		}

	}
}
