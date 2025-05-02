/*
 * MachPort.cs: Bindings to the CFMachPort API
 * 
 * Authors:
 *    Miguel de Icaza
 * 
 * Copyright 2014 Xamarin Inc
 * All Rights Reserved
 */

#nullable enable

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace CoreFoundation {

#if false
	[StructLayout(LayoutKind.Sequential)]
	internal public struct CFMachPortContext {
		public delegate IntPtr CBRetain (IntPtr info);
		public delegate void CBRelease (IntPtr info);
		public delegate IntPtr CBCopyDescription (IntPtr _EventInfo);

		public IntPtr Version;
		public IntPtr Info;
		public CBRetain Retain;
		public CBRelease Release;
		public CBCopyDescription CopyDescription;
	}

	delegate void CFMachPortCallBack (IntPtr cfMachPort, IntPtr msg, IntPtr size, IntPtr info);
#endif

	/// <summary>Basic access to the underlying operating system Mach Port and integration with run loops.</summary>
	///     <remarks>
	///       <para>
	/// 	The main use is to integrate Mach Ports into a <see cref="CoreFoundation.CFRunLoop" />.  Use the <see cref="CoreFoundation.CFMachPort.CreateRunLoopSource" />
	/// 	to create a <see cref="CoreFoundation.CFRunLoopSource" /> that can
	/// 	then be added into the <see cref="CoreFoundation.CFRunLoop" />.
	///       </para>
	///     </remarks>
	public class CFMachPort : NativeObject {
		delegate void CFMachPortCallBack (IntPtr cfmachport, IntPtr msg, nint len, IntPtr context);

		[Preserve (Conditional = true)]
		internal CFMachPort (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFMachPortGetPort (IntPtr handle);

		/// <summary>Gets the pointer to the wrapped Mach port instance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr MachPort {
			get {
				return CFMachPortGetPort (Handle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFMachPortInvalidate (IntPtr handle);

		/// <summary>Stops the Mach port from sending or receiving messages, but does not destroy it.</summary>
		///         <remarks>To be added.</remarks>
		public void Invalidate ()
		{
			CFMachPortInvalidate (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFMachPortIsValid (IntPtr handle);
		/// <summary>Gets a value that tells whether the port can send and receive messages.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsValid {
			get {
				return CFMachPortIsValid (Handle) != 0;
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFMachPortCreateRunLoopSource (IntPtr allocator, IntPtr port, IntPtr order);

		/// <summary>Creates the run loop source for the Mach port.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CFRunLoopSource CreateRunLoopSource ()
		{
			// order is currently ignored, we must pass 0
			var source = CFMachPortCreateRunLoopSource (IntPtr.Zero, Handle, IntPtr.Zero);
			return new CFRunLoopSource (source, true);
		}

	}
}
