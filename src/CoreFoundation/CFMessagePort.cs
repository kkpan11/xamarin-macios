//
// CFMessagePort.cs: CFMessagePort is a wrapper around two native Mach ports with bidirectional communication support
//
// Authors:
//   Oleg Demchenko (oleg.demchenko@xamarin.com)
//
// Copyright 2015 Xamarin Inc
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

using dispatch_queue_t = System.IntPtr;

namespace CoreFoundation {

	// untyped enum from CFMessagePort.h
	// used as a return value of type SInt32 (always 4 bytes)
	/// <summary>This enumeration contains status codes for <see cref="CoreFoundation.CFMessagePort.SendRequest(System.Int32,Foundation.NSData,System.Double,System.Double,Foundation.NSString,out Foundation.NSData)" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum CFMessagePortSendRequestStatus {
		/// <summary>The message was sent, and any expected reply was received.</summary>
		Success = 0,
		/// <summary>The port timed out before the message could be sent.</summary>
		SendTimeout = -1,
		/// <summary>The port timed out before the response was received.</summary>
		ReceiveTimeout = -2,
		/// <summary>The port became invalid before the message was sent.</summary>
		IsInvalid = -3,
		/// <summary>An error occurred.</summary>
		TransportError = -4,
		/// <summary>The port became invalid after the message was sent, but before a response was received.</summary>
		BecameInvalidError = -5,
	}

	internal class CFMessagePortContext {

		public Func<INativeObject>? Retain { get; set; }

		public Action? Release { get; set; }

		public Func<NSString>? CopyDescription { get; set; }
	}

	/// <summary>A communication channel between multiple threads on the local device.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFMessagePort : NativeObject {

		// CFMessagePortContext
		[StructLayout (LayoutKind.Sequential)]
		unsafe struct ContextProxy {
			/* CFIndex */
			nint version; // must be 0
			public /* void * */ IntPtr info;
			public delegate* unmanaged<IntPtr, IntPtr> retain;
			public delegate* unmanaged<IntPtr, void> release;
			public delegate* unmanaged<IntPtr, IntPtr> copyDescription;
		}

		/// <param name="type">To be added.</param>
		///     <param name="data">To be added.</param>
		///     <summary>To be added.</summary>
		///     <returns>To be added.</returns>
		///     <remarks>To be added.</remarks>
		public delegate NSData CFMessagePortCallBack (int type, NSData data);

		static Dictionary<IntPtr, CFMessagePortCallBack> outputHandles = new Dictionary<IntPtr, CFMessagePortCallBack> (Runtime.IntPtrEqualityComparer);

		static Dictionary<IntPtr, Action?> invalidationHandles = new Dictionary<IntPtr, Action?> (Runtime.IntPtrEqualityComparer);

		static Dictionary<IntPtr, CFMessagePortContext?> messagePortContexts = new Dictionary<IntPtr, CFMessagePortContext?> (Runtime.IntPtrEqualityComparer);

		IntPtr contextHandle;

		/// <summary>Returns a Boolean value that indicates whether a current instance of CFMessagePort object represents a remote port.</summary>
		///         <value>Boolean value.</value>
		///         <remarks>Property returns true if CFMessagePort is remote.</remarks>
		public bool IsRemote {
			get {
				return CFMessagePortIsRemote (GetCheckedHandle ()) != 0;
			}
		}

		/// <summary>The registered name of message port.</summary>
		///         <value>String representation of message port's name.</value>
		///         <remarks> Property returns null if port have no name.</remarks>
		public string? Name {
			get {
				return CFString.FromHandle (CFMessagePortGetName (GetCheckedHandle ()));
			}
			set {
				var n = CFString.CreateNative (value);
				try {
					CFMessagePortSetName (GetCheckedHandle (), n);
				} finally {
					CFString.ReleaseNative (n);
				}
			}
		}

		/// <summary>Returns a boolean value that indicates whether a CFMessagePort object is valid.</summary>
		///         <value>Boolean value.</value>
		///         <remarks>Property indicates whether message port can send or receive messages.</remarks>
		public bool IsValid {
			get {
				return CFMessagePortIsValid (GetCheckedHandle ()) != 0;
			}
		}

		internal CFMessagePortContext? Context {
			get {
				CFMessagePortContext? result;
				ContextProxy context = new ContextProxy ();
				unsafe {
					CFMessagePortGetContext (GetCheckedHandle (), &context);
				}

				if (context.info == IntPtr.Zero)
					return null;

				lock (messagePortContexts)
					messagePortContexts.TryGetValue (context.info, out result);

				return result;
			}
		}

		/// <summary>Gets or sets the invalidation callback method for a CFMessagePort object.</summary>
		///         <value>Delegate</value>
		///         <remarks>Set null value to remove callback. Callback will be fired on message on port invalidation.</remarks>
		public Action? InvalidationCallback {
			get {
				lock (invalidationHandles) {
					invalidationHandles.TryGetValue (GetCheckedHandle (), out var result);
					return result;
				}
			}
			set {
				lock (invalidationHandles) {
					if (value is null)
						invalidationHandles [GetCheckedHandle ()] = null;
					else
						invalidationHandles.Add (GetCheckedHandle (), value);
				}

				unsafe {
					CFMessagePortSetInvalidationCallBack (Handle, &MessagePortInvalidationCallback);
				}
			}
		}

		[Preserve (Conditional = true)]
		internal CFMessagePort (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <include file="../../docs/api/CoreFoundation/CFMessagePort.xml" path="/Documentation/Docs[@DocId='M:CoreFoundation.CFMessagePort.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero) {

				lock (outputHandles)
					outputHandles.Remove (Handle);

				lock (invalidationHandles) {
					if (invalidationHandles.ContainsKey (Handle))
						invalidationHandles.Remove (Handle);
				}

				lock (messagePortContexts) {
					if (messagePortContexts.ContainsKey (contextHandle))
						messagePortContexts.Remove (contextHandle);
				}

				contextHandle = IntPtr.Zero;
			}

			base.Dispose (disposing);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static unsafe extern /* CFMessagePortRef */ IntPtr CFMessagePortCreateLocal (/* CFAllocatorRef */ IntPtr allocator, /* CFStringRef */ IntPtr name, delegate* unmanaged<IntPtr, int, IntPtr, IntPtr, IntPtr> callout, /*  CFMessagePortContext */ ContextProxy* context, byte* shouldFreeInfo);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* CFMessagePortRef */ IntPtr CFMessagePortCreateRemote (/* CFAllocatorRef */ IntPtr allocator, /* CFStringRef */ IntPtr name);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFMessagePortInvalidate (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFMessagePortCreateRunLoopSource (/* CFAllocatorRef */ IntPtr allocator, /* CFMessagePortRef */ IntPtr local, /* CFIndex */ nint order);

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe static extern /* SInt32 */ CFMessagePortSendRequestStatus CFMessagePortSendRequest (/* CFMessagePortRef */ IntPtr remote, /* SInt32 */ int msgid, /* CFDataRef */ IntPtr data, /* CFTiemInterval */ double sendTimeout, /* CFTiemInterval */ double rcvTimeout, /* CFStringRef */ IntPtr replyMode, /* CFDataRef* */ IntPtr* returnData);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortIsRemote (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortSetName (/* CFMessagePortRef */ IntPtr ms, /* CFStringRef */ IntPtr newName);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* CFStringRef */ IntPtr CFMessagePortGetName (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe static extern void CFMessagePortGetContext (/* CFMessagePortRef */ IntPtr ms, /* CFMessagePortContext* */ ContextProxy* context);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* Boolean */ byte CFMessagePortIsValid (/* CFMessagePortRef */ IntPtr ms);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFMessagePortSetDispatchQueue (/* CFMessagePortRef */ IntPtr ms, dispatch_queue_t queue);

		[DllImport (Constants.CoreFoundationLibrary)]
		static unsafe extern void CFMessagePortSetInvalidationCallBack (/* CFMessagePortRef */ IntPtr ms, delegate* unmanaged<IntPtr, IntPtr, void> callout);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFMessagePortGetInvalidationCallBack (/* CFMessagePortRef */ IntPtr ms);

		/// <param name="name">To be added.</param>
		///         <param name="callback">To be added.</param>
		///         <param name="allocator">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFMessagePort? CreateLocalPort (string? name, CFMessagePortCallBack callback, CFAllocator? allocator = null)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			return CreateLocalPort (allocator, name, callback, context: null);
		}

		internal static CFMessagePort? CreateLocalPort (CFAllocator? allocator, string? name, CFMessagePortCallBack callback, CFMessagePortContext? context)
		{
			var n = CFString.CreateNative (name);
			byte shouldFreeInfo = 0;
			var contextProxy = new ContextProxy ();

			// a GCHandle is needed because we do not have an handle before calling CFMessagePortCreateLocal
			// and that will call the RetainProxy. So using this (short-lived) GCHandle allow us to find back the
			// original context defined by developer
			var shortHandle = GCHandle.Alloc (contextProxy);

			if (context is not null) {
				unsafe {
					if (context.Retain is not null)
						contextProxy.retain = &RetainProxy;
					if (context.Release is not null)
						contextProxy.release = &ReleaseProxy;
					if (context.CopyDescription is not null)
						contextProxy.copyDescription = &CopyDescriptionProxy;
				}
				contextProxy.info = (IntPtr) shortHandle;
				lock (messagePortContexts)
					messagePortContexts.Add (contextProxy.info, context);
			}

			try {
				IntPtr portHandle;
				unsafe {
					portHandle = CFMessagePortCreateLocal (allocator.GetHandle (), n, &MessagePortCallback, &contextProxy, &shouldFreeInfo);
					GC.KeepAlive (allocator);
				}

				// TODO handle should free info
				if (portHandle == IntPtr.Zero)
					return null;

				var result = new CFMessagePort (portHandle, true);

				lock (outputHandles)
					outputHandles.Add (portHandle, callback);

				if (context is not null) {
					lock (messagePortContexts) {
						messagePortContexts.Remove (contextProxy.info);
						unsafe {
							CFMessagePortGetContext (portHandle, &contextProxy);
						}
						messagePortContexts.Add (contextProxy.info, context);
					}

					result.contextHandle = contextProxy.info;
				}

				return result;
			} finally {
				CFString.ReleaseNative (n);

				// we won't need short GCHandle after the Create call
				shortHandle.Free ();
			}
		}

		//
		// Proxy callbacks
		//
		[UnmanagedCallersOnly]
		static IntPtr RetainProxy (IntPtr info)
		{
			INativeObject? result = null;
			CFMessagePortContext? context;

			lock (messagePortContexts) {
				messagePortContexts.TryGetValue (info, out context);
			}

			if (context?.Retain is not null)
				result = context.Retain ();

			return result.GetHandle ();
		}

		[UnmanagedCallersOnly]
		static void ReleaseProxy (IntPtr info)
		{
			CFMessagePortContext? context;

			lock (messagePortContexts)
				messagePortContexts.TryGetValue (info, out context);

			if (context?.Release is not null)
				context.Release ();
		}

		[UnmanagedCallersOnly]
		static IntPtr CopyDescriptionProxy (IntPtr info)
		{
			NSString? result = null;
			CFMessagePortContext? context;

			lock (messagePortContexts)
				messagePortContexts.TryGetValue (info, out context);

			if (context?.CopyDescription is not null)
				result = context.CopyDescription ();

#pragma warning disable RBI0014
			return result.GetHandle ();
#pragma warning restore RBI0014
		}

		[UnmanagedCallersOnly]
		static IntPtr MessagePortCallback (IntPtr local, int msgid, IntPtr data, IntPtr info)
		{
			CFMessagePortCallBack callback;

			lock (outputHandles)
				callback = outputHandles [local];

			if (callback is null)
				return IntPtr.Zero;

			using (var managedData = Runtime.GetNSObject<NSData> (data)!) {
				var result = callback.Invoke (msgid, managedData);
				// System will release returned CFData
				result?.DangerousRetain ();
#pragma warning disable RBI0014
				return result.GetHandle ();
#pragma warning restore RBI0014
			}
		}

		[UnmanagedCallersOnly]
		static void MessagePortInvalidationCallback (IntPtr messagePort, IntPtr info)
		{
			Action? callback;

			lock (invalidationHandles)
				invalidationHandles.TryGetValue (messagePort, out callback);

			if (callback is not null)
				callback.Invoke ();
		}

		/// <param name="allocator">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <summary>Deprecated.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFMessagePort? CreateRemotePort (CFAllocator? allocator, string name)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var n = CFString.CreateNative (name);
			try {
				var portHandle = CFMessagePortCreateRemote (allocator.GetHandle (), n);
				GC.KeepAlive (allocator);
				return portHandle == IntPtr.Zero ? null : new CFMessagePort (portHandle, true);
			} finally {
				CFString.ReleaseNative (n);
			}
		}

		/// <summary>Invalidating a message port prevents the port from ever sending or receiving any more messages. </summary>
		///         <remarks>The message port is not deallocated after invalidation, however <see cref="CoreFoundation.CFMessagePort.IsValid" /> property is set to be true.</remarks>
		public void Invalidate ()
		{
			CFMessagePortInvalidate (GetCheckedHandle ());
		}

		/// <param name="msgid">To be added.</param>
		///         <param name="data">To be added.</param>
		///         <param name="sendTimeout">To be added.</param>
		///         <param name="rcvTimeout">To be added.</param>
		///         <param name="replyMode">To be added.</param>
		///         <param name="returnData">To be added.</param>
		///         <summary>Sends a message to the port.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CFMessagePortSendRequestStatus SendRequest (int msgid, NSData? data, double sendTimeout, double rcvTimeout, NSString? replyMode, out NSData? returnData)
		{
			CFMessagePortSendRequestStatus result;
			IntPtr returnDataHandle;
			unsafe {
				result = CFMessagePortSendRequest (GetCheckedHandle (), msgid, data.GetHandle (), sendTimeout, rcvTimeout, replyMode.GetHandle (), &returnDataHandle);
				GC.KeepAlive (data);
				GC.KeepAlive (replyMode);
			}

			returnData = Runtime.GetINativeObject<NSData> (returnDataHandle, false);

			return result;
		}

		/// <summary>Creates a CFRunLoopSource object for a CFMessagePort object.</summary>
		///         <returns>The new CFRunLoopSource object for listening port</returns>
		///         <remarks>Method returns loop which is not added to any run loop. Use <see cref="CoreFoundation.CFRunLoop.AddSource(CoreFoundation.CFRunLoopSource,Foundation.NSString)" /> to activate the loop.</remarks>
		public CFRunLoopSource CreateRunLoopSource ()
		{
			// note: order is currently ignored by CFMessagePort object run loop sources. Pass 0 for this value.
			var runLoopHandle = CFMessagePortCreateRunLoopSource (IntPtr.Zero, Handle, 0);
			return new CFRunLoopSource (runLoopHandle, false);
		}

		/// <param name="queue">To be added.</param>
		///         <summary>Schedules message port’s callbacks on the specified dispatch queue.</summary>
		///         <remarks>To be added.</remarks>
		public void SetDispatchQueue (DispatchQueue? queue)
		{
			CFMessagePortSetDispatchQueue (GetCheckedHandle (), queue.GetHandle ());
			GC.KeepAlive (queue);
		}
	}
}
