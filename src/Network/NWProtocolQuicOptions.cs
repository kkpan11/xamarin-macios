using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using Security;

using OS_nw_protocol_options = System.IntPtr;
using OS_nw_protocol_metadata = System.IntPtr;
using SecProtocolOptionsRef = System.IntPtr;

#nullable enable

namespace Network {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWProtocolQuicOptions : NWProtocolOptions {

		[Preserve (Conditional = true)]
		internal NWProtocolQuicOptions (NativeHandle handle, bool owns) : base (handle, owns) { }

		public NWProtocolQuicOptions () : this (nw_quic_create_options (), owns: true) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_add_tls_application_protocol (OS_nw_protocol_options options, IntPtr applicationProtocol);

		static void nw_quic_add_tls_application_protocol (OS_nw_protocol_options options, string applicationProtocol)
		{
			using var applicationProtocolPtr = new TransientString (applicationProtocol);
			nw_quic_add_tls_application_protocol (options, applicationProtocolPtr);
		}
		public void AddTlsApplicationProtocol (string applicationProtocol)
			=> nw_quic_add_tls_application_protocol (GetCheckedHandle (), applicationProtocol);

		[DllImport (Constants.NetworkLibrary)]
		static extern SecProtocolOptionsRef nw_quic_copy_sec_protocol_options (OS_nw_protocol_options options);

		public SecProtocolOptions SecProtocolOptions
			=> new SecProtocolOptions (nw_quic_copy_sec_protocol_options (GetCheckedHandle ()), true);

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_quic_get_stream_is_unidirectional (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_stream_is_unidirectional (OS_nw_protocol_options options, byte isUnidirectional);

		public bool StreamIsUnidirectional {
			get => nw_quic_get_stream_is_unidirectional (GetCheckedHandle ()) != 0;
			set => nw_quic_set_stream_is_unidirectional (GetCheckedHandle (), value.AsByte ());
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_data (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_data (OS_nw_protocol_options options, ulong initial_max_data);

		public ulong InitialMaxData {
			get => nw_quic_get_initial_max_data (GetCheckedHandle ());
			set => nw_quic_set_initial_max_data (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_quic_get_max_udp_payload_size (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_max_udp_payload_size (OS_nw_protocol_options options, ushort max_udp_payload_size);

		public ushort MaxUdpPayloadSize {
			get => nw_quic_get_max_udp_payload_size (GetCheckedHandle ());
			set => nw_quic_set_max_udp_payload_size (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern uint nw_quic_get_idle_timeout (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_idle_timeout (OS_nw_protocol_options options, uint idle_timeout);

		public uint IdleTimeout {
			get => nw_quic_get_idle_timeout (GetCheckedHandle ());
			set => nw_quic_set_idle_timeout (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_streams_bidirectional (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_streams_bidirectional (OS_nw_protocol_options options, ulong initial_max_streams_bidirectional);

		public ulong InitialMaxStreamsBidirectional {
			get => nw_quic_get_initial_max_streams_bidirectional (GetCheckedHandle ());
			set => nw_quic_set_initial_max_streams_bidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_streams_unidirectional (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_streams_unidirectional (OS_nw_protocol_options options, ulong initial_max_streams_unidirectional);

		public ulong InitialMaxStreamsUnidirectional {
			get => nw_quic_get_initial_max_streams_unidirectional (GetCheckedHandle ());
			set => nw_quic_set_initial_max_streams_unidirectional (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_stream_data_bidirectional_local (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_stream_data_bidirectional_local (OS_nw_protocol_options options, ulong initial_max_stream_data_bidirectional_local);

		public ulong InitialMaxStreamDataBidirectionalLocal {
			get => nw_quic_get_initial_max_stream_data_bidirectional_local (GetCheckedHandle ());
			set => nw_quic_set_initial_max_stream_data_bidirectional_local (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_stream_data_bidirectional_remote (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_stream_data_bidirectional_remote (OS_nw_protocol_options options, ulong initial_max_stream_data_bidirectional_remote);

		public ulong InitialMaxStreamDataBidirectionalRemote {
			get => nw_quic_get_initial_max_stream_data_bidirectional_remote (GetCheckedHandle ());
			set => nw_quic_set_initial_max_stream_data_bidirectional_remote (GetCheckedHandle (), value);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_quic_get_initial_max_stream_data_unidirectional (OS_nw_protocol_options options);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_initial_max_stream_data_unidirectional (OS_nw_protocol_options options, ulong initial_max_stream_data_unidirectional);

		public ulong InitialMaxStreamDataUnidirectional {
			get => nw_quic_get_initial_max_stream_data_unidirectional (GetCheckedHandle ());
			set => nw_quic_set_initial_max_stream_data_unidirectional (GetCheckedHandle (), value);
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_quic_get_max_datagram_frame_size (OS_nw_protocol_options options);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_max_datagram_frame_size (OS_nw_protocol_options options, ushort max_datagram_frame_size);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public ushort DatagramFrameSize {
			get => nw_quic_get_max_datagram_frame_size (GetCheckedHandle ());
			set => nw_quic_set_max_datagram_frame_size (GetCheckedHandle (), value);
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_quic_get_stream_is_datagram (OS_nw_protocol_options options);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_quic_set_stream_is_datagram (OS_nw_protocol_options options, byte is_datagram);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public bool StreamIsDatagram {
			get => nw_quic_get_stream_is_datagram (GetCheckedHandle ()) != 0;
			set => nw_quic_set_stream_is_datagram (GetCheckedHandle (), value.AsByte ());
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern ushort nw_quic_get_stream_usable_datagram_frame_size (OS_nw_protocol_metadata metadata);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public ushort StreamUsableDatagramFrameSize => nw_quic_get_stream_usable_datagram_frame_size (GetCheckedHandle ());

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_quic_get_stream_type (OS_nw_protocol_metadata stream_metadata);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public NWQuicStreamType StreamType => (NWQuicStreamType) nw_quic_get_stream_type (GetCheckedHandle ());

	}
}
