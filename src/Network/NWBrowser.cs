//
// NWBrowser.cs: Bindings the Network nw_browser_t API.
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyrigh 2019 Microsoft Inc
//
#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_browser = System.IntPtr;
using OS_nw_browse_descriptor = System.IntPtr;
using OS_nw_parameters = System.IntPtr;
using dispatch_queue_t = System.IntPtr;

namespace Network {

	public delegate void NWBrowserChangesDelegate (NWBrowseResult? oldResult, NWBrowseResult? newResult, bool completed);

	public delegate void NWBrowserCompleteChangesDelegate (List<(NWBrowseResult? result, NWBrowseResultChange change)>? changes);

	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWBrowser : NativeObject {

		bool started = false;
		bool queueSet = false;
		object startLock = new Object ();

		[Preserve (Conditional = true)]
		internal NWBrowser (NativeHandle handle, bool owns) : base (handle, owns)
		{
			SetChangesHandler (InternalChangesHandler);
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_browser nw_browser_create (OS_nw_browse_descriptor descriptor, OS_nw_parameters parameters);

		public NWBrowser (NWBrowserDescriptor descriptor, NWParameters? parameters)
		{
			if (descriptor is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (descriptor));

			InitializeHandle (nw_browser_create (descriptor.Handle, parameters.GetHandle ()));
			GC.KeepAlive (descriptor);
			GC.KeepAlive (parameters);
			SetChangesHandler (InternalChangesHandler);
		}

		public NWBrowser (NWBrowserDescriptor descriptor) : this (descriptor, null) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_browser_set_queue (OS_nw_browser browser, dispatch_queue_t queue);

		public void SetDispatchQueue (DispatchQueue queue)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			lock (startLock) {
				nw_browser_set_queue (GetCheckedHandle (), queue.Handle);
				GC.KeepAlive (queue);
				queueSet = true;
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_browser_start (OS_nw_browser browser);

		public void Start ()
		{
			lock (startLock) {
				if (!queueSet) {
					throw new InvalidOperationException ("Cannot start the browser without a DispatchQueue.");
				}
				nw_browser_start (GetCheckedHandle ());
				started = true;
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_browser_cancel (OS_nw_browser browser);

		public void Cancel ()
		{
			lock (startLock) {
				try {
					nw_browser_cancel (GetCheckedHandle ());
				} finally {
					started = false;
				}
			}
		}

		public bool IsActive {
			get {
				lock (startLock)
					return started;
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_browse_descriptor nw_browser_copy_browse_descriptor (OS_nw_browser browser);

		public NWBrowserDescriptor Descriptor
			=> new NWBrowserDescriptor (nw_browser_copy_browse_descriptor (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_parameters nw_browser_copy_parameters (OS_nw_browser browser);

		public NWParameters Parameters
			=> new NWParameters (nw_browser_copy_parameters (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_browser_set_browse_results_changed_handler (OS_nw_browser browser, BlockLiteral* handler);

		[UnmanagedCallersOnly]
		static void TrampolineChangesHandler (IntPtr block, IntPtr oldResult, IntPtr newResult, byte completed)
		{
			var del = BlockLiteral.GetTarget<NWBrowserChangesDelegate> (block);
			if (del is not null) {
				// we do the cleanup of the objs in the internal handlers
				NWBrowseResult? nwOldResult = (oldResult == IntPtr.Zero) ? null : new NWBrowseResult (oldResult, owns: false);
				NWBrowseResult? nwNewResult = (newResult == IntPtr.Zero) ? null : new NWBrowseResult (newResult, owns: false);
				del (nwOldResult, nwNewResult, completed != 0);
			}
		}

		public Action<NWBrowseResult?, NWBrowseResult?>? IndividualChangesDelegate { get; set; }

		// syntactic sugar for the user, nicer to get all the changes at once
		public NWBrowserCompleteChangesDelegate? CompleteChangesDelegate { get; set; }
		object changesLock = new object ();
		List<(NWBrowseResult? result, NWBrowseResultChange change)> changes = new List<(NWBrowseResult? result, NWBrowseResultChange change)> ();

		void InternalChangesHandler (NWBrowseResult? oldResult, NWBrowseResult? newResult, bool completed)
		{
			// we allow the user to listen to both, individual changes AND complete ones, individual is simple, just
			// call the cb, completed, we need to get a collection and call the cb when completed
			var individualCb = IndividualChangesDelegate;
			individualCb?.Invoke (oldResult, newResult);
			var completeCb = CompleteChangesDelegate;
			if (completeCb is null) {
				// we do not want to keep a list of the new results if the user does not care, dispose and move on
				// results can be null, since we could have a not old one
				oldResult?.Dispose ();
				newResult?.Dispose ();
				return;
			}
			// get the change, add it to the list
			var change = NWBrowseResult.GetChanges (oldResult, newResult);
			var result = (result: newResult, change: change);
			// at this point, we do not longer need the old result
			// results can be null
			oldResult?.Dispose ();
			List<(NWBrowseResult? result, NWBrowseResultChange change)>? tmp_changes = null;
			lock (changesLock) {
				changes.Add (result);
				// only call when we know we are done
				if (completed) {
					tmp_changes = changes;
					changes = new List<(NWBrowseResult? result, NWBrowseResultChange change)> ();
				}
			}
			if (completed) {
				completeCb?.Invoke (tmp_changes);
				if (tmp_changes is not null)
					foreach (var c in tmp_changes)
						c.result?.Dispose ();
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		void SetChangesHandler (NWBrowserChangesDelegate? handler)
		{
			unsafe {
				if (handler is null) {
					nw_browser_set_browse_results_changed_handler (GetCheckedHandle (), null);
					return;
				}

				delegate* unmanaged<IntPtr, IntPtr, IntPtr, byte, void> trampoline = &TrampolineChangesHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWBrowser), nameof (TrampolineChangesHandler));
				nw_browser_set_browse_results_changed_handler (GetCheckedHandle (), &block);
			}
		}

		[DllImport (Constants.NetworkLibrary)]
		unsafe static extern void nw_browser_set_state_changed_handler (OS_nw_browser browser, BlockLiteral* state_changed_handler);

		[UnmanagedCallersOnly]
		static void TrampolineStateChangesHandler (IntPtr block, NWBrowserState state, IntPtr error)
		{
			var del = BlockLiteral.GetTarget<Action<NWBrowserState, NWError?>> (block);
			if (del is not null) {
				var nwError = (error == IntPtr.Zero) ? null : new NWError (error, owns: false);
				del (state, nwError);
			}
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		public void SetStateChangesHandler (Action<NWBrowserState, NWError?> handler)
		{
			unsafe {
				if (handler is null) {
					nw_browser_set_state_changed_handler (GetCheckedHandle (), null);
					return;
				}
				delegate* unmanaged<IntPtr, NWBrowserState, IntPtr, void> trampoline = &TrampolineStateChangesHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (NWBrowser), nameof (TrampolineStateChangesHandler));
				nw_browser_set_state_changed_handler (GetCheckedHandle (), &block);
			}
		}
	}
}
