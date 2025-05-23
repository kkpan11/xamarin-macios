
//
// NWDataTransferReport.cs: Bindings the Network nw_data_transfer_report_t API.
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyright 2019 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_establishment_report = System.IntPtr;
using nw_endpoint_t = System.IntPtr;
using nw_report_protocol_enumerator_t = System.IntPtr;
using nw_protocol_definition_t = System.IntPtr;
using nw_resolution_report_t = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWEstablishmentReport : NativeObject {

		[Preserve (Conditional = true)]
		internal NWEstablishmentReport (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_establishment_report_get_used_proxy (OS_nw_establishment_report report);

		public bool UsedProxy => nw_establishment_report_get_used_proxy (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_establishment_report_get_proxy_configured (OS_nw_establishment_report report);

		public bool ProxyConfigured => nw_establishment_report_get_proxy_configured (GetCheckedHandle ()) != 0;

		[DllImport (Constants.NetworkLibrary)]
		static extern uint nw_establishment_report_get_previous_attempt_count (OS_nw_establishment_report report);

		public uint PreviousAttemptCount => nw_establishment_report_get_previous_attempt_count (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_establishment_report_get_duration_milliseconds (OS_nw_establishment_report report);

		public TimeSpan Duration => TimeSpan.FromMilliseconds (nw_establishment_report_get_duration_milliseconds (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary)]
		static extern ulong nw_establishment_report_get_attempt_started_after_milliseconds (OS_nw_establishment_report report);

		public TimeSpan ConnectionSetupTime => TimeSpan.FromMilliseconds (nw_establishment_report_get_attempt_started_after_milliseconds (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_establishment_report_enumerate_resolutions (OS_nw_establishment_report report, BlockLiteral* enumerate_block);

		[UnmanagedCallersOnly]
		static void TrampolineResolutionEnumeratorHandler (IntPtr block, NWReportResolutionSource source, nuint milliseconds, int endpoint_count, nw_endpoint_t successful_endpoint, nw_endpoint_t preferred_endpoint)
		{
			var del = BlockLiteral.GetTarget<Action<NWReportResolutionSource, TimeSpan, int, NWEndpoint, NWEndpoint>> (block);
			if (del is not null) {
				using (var nwSuccesfulEndpoint = new NWEndpoint (successful_endpoint, owns: false))
				using (var nwPreferredEndpoint = new NWEndpoint (preferred_endpoint, owns: false))
					del (source, TimeSpan.FromMilliseconds (milliseconds), endpoint_count, nwSuccesfulEndpoint, nwPreferredEndpoint);
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void EnumerateResolutions (Action<NWReportResolutionSource, TimeSpan, int, NWEndpoint, NWEndpoint> handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, NWReportResolutionSource, nuint, int, nw_endpoint_t, nw_endpoint_t, void> trampoline = &TrampolineResolutionEnumeratorHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWEstablishmentReport), nameof (TrampolineResolutionEnumeratorHandler));
				nw_establishment_report_enumerate_resolutions (GetCheckedHandle (), &block);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_establishment_report_enumerate_protocols (OS_nw_establishment_report report, BlockLiteral* enumerate_block);

		[UnmanagedCallersOnly]
		static void TrampolineEnumerateProtocolsHandler (IntPtr block, nw_protocol_definition_t protocol, nuint handshake_milliseconds, nuint handshake_rtt_milliseconds)
		{
			var del = BlockLiteral.GetTarget<Action<NWProtocolDefinition, TimeSpan, TimeSpan>> (block);
			if (del is not null) {
				using (var nwProtocolDefinition = new NWProtocolDefinition (protocol, owns: false))
					del (nwProtocolDefinition, TimeSpan.FromMilliseconds (handshake_milliseconds), TimeSpan.FromMilliseconds (handshake_rtt_milliseconds));
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void EnumerateProtocols (Action<NWProtocolDefinition, TimeSpan, TimeSpan> handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, nw_protocol_definition_t, nuint, nuint, void> trampoline = &TrampolineEnumerateProtocolsHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWEstablishmentReport), nameof (TrampolineEnumerateProtocolsHandler));
				nw_establishment_report_enumerate_protocols (GetCheckedHandle (), &block);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern nw_endpoint_t nw_establishment_report_copy_proxy_endpoint (OS_nw_establishment_report report);

		public NWEndpoint? ProxyEndpoint {
			get {
				var ptr = nw_establishment_report_copy_proxy_endpoint (GetCheckedHandle ());
				return (ptr == IntPtr.Zero) ? null : new NWEndpoint (ptr, owns: true);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_establishment_report_enumerate_resolution_reports (OS_nw_establishment_report report, BlockLiteral* enumerateBlock);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnmanagedCallersOnly]
		static void TrampolineEnumerateResolutionReport (IntPtr block, nw_resolution_report_t report)
		{
			var del = BlockLiteral.GetTarget<Action<NWResolutionReport>> (block);
			if (del is null)
				return;
			using var nwReport = new NWResolutionReport (report, owns: false);
			del (nwReport);
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void EnumerateResolutionReports (Action<NWResolutionReport> handler)
		{
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
				delegate* unmanaged<IntPtr, nw_resolution_report_t, void> trampoline = &TrampolineEnumerateResolutionReport;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWEstablishmentReport), nameof (TrampolineEnumerateResolutionReport));
				nw_establishment_report_enumerate_protocols (GetCheckedHandle (), &block);
			}
		}
	}
}
