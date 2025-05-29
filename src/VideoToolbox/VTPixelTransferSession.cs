// 
// VTPixelTransferSession.cs: VideoTools Pixel Transfer Session class 
//
// Authors: 
//    Israel Soto (issoto@microsoft.com)
// 
// Copyright 2022 Microsoft Corporation.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

namespace VideoToolbox {
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios16.0")]
	[SupportedOSPlatform ("maccatalyst16.0")]
	[SupportedOSPlatform ("tvos16.0")]
	public class VTPixelTransferSession : VTSession {

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* CFTypeID */ nint VTPixelTransferSessionGetTypeID ();
		public static nint GetTypeID () => VTPixelTransferSessionGetTypeID ();

		[Preserve (Conditional = true)]
		internal VTPixelTransferSession (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static void VTPixelTransferSessionInvalidate (/* VTPixelTransferSessionRef */ IntPtr session);

		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				VTPixelTransferSessionInvalidate (Handle);

			base.Dispose (disposing);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static VTStatus VTPixelTransferSessionCreate (
			/* CFAllocatorRef */ IntPtr allocator, /* can be null */
			/* VTPixelTransferSessionRef* */ IntPtr* pixelTransferSessionOut);

		public static VTPixelTransferSession? Create () => Create (null);

		public static VTPixelTransferSession? Create (CFAllocator? allocator)
		{
			VTStatus result;
			IntPtr ret;
			unsafe {
				result = VTPixelTransferSessionCreate (allocator.GetHandle (), &ret);
				GC.KeepAlive (allocator);
			}

			if (result == VTStatus.Ok && ret != IntPtr.Zero)
				return new VTPixelTransferSession (ret, true);

			return null;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTPixelTransferSessionTransferImage (
			/* VTPixelTransferSessionRef */ IntPtr session,
			/* CVPixelBuffer */ IntPtr sourceBuffer,
			/* CVPixelBuffer */ IntPtr destinationBuffer);

		public VTStatus TransferImage (CVPixelBuffer sourceBuffer, CVPixelBuffer destinationBuffer)
		{
			if (sourceBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceBuffer));

			if (destinationBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (destinationBuffer));

			VTStatus status = VTPixelTransferSessionTransferImage (GetCheckedHandle (), sourceBuffer.Handle, destinationBuffer.Handle);
			GC.KeepAlive (sourceBuffer);
			GC.KeepAlive (destinationBuffer);
			return status;
		}

		public VTStatus SetTransferProperties (VTPixelTransferProperties options)
		{
			if (options is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (options));

			return VTSessionSetProperties (Handle, options.Dictionary.Handle);
		}
	}
}
