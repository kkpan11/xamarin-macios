// 
// VTFrameSilo.cs: VideoToolbox VTFrameSilo class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace VideoToolbox {

#if NET
	/// <summary>Sample buffers storage object, used in conjuction of a multi pass compression session</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	public class VTFrameSilo : NativeObject {
#if !NET
		protected internal VTFrameSilo (NativeHandle handle)
			: base (handle, false)
		{
		}
#endif

		[Preserve (Conditional = true)]
		internal VTFrameSilo (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static /* OSStatus */ VTStatus VTFrameSiloCreate (
			/* CFAllocatorRef */ IntPtr allocator, /* can be null */
			/* CFURLRef */ IntPtr fileUrl, /* can be null */
			/* CMTimeRange */ CMTimeRange timeRange, /* can be kCMTimeRangeInvalid */
			/* CFDictionaryRef */ IntPtr options, /* Reserved, always null */
			/* VTFrameSiloRef */ IntPtr* siloOut);

		/// <param name="fileUrl">To be added.</param>
		///         <param name="timeRange">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VTFrameSilo? Create (NSUrl? fileUrl = null, CMTimeRange? timeRange = null)
		{
			VTStatus status;
			IntPtr ret;
			unsafe {
				status = VTFrameSiloCreate (
				IntPtr.Zero,
				fileUrl.GetHandle (),
				timeRange ?? CMTimeRange.InvalidRange,
				IntPtr.Zero,
				&ret);
				GC.KeepAlive (fileUrl);
			}

			if (status != VTStatus.Ok)
				return null;

			return new VTFrameSilo (ret, true);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* OSStatus */ VTStatus VTFrameSiloAddSampleBuffer (
			/* VTFrameSiloRef */ IntPtr silo,
			/* CMSampleBufferRef */ IntPtr sampleBuffer);

		/// <param name="sampleBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus AddSampleBuffer (CMSampleBuffer sampleBuffer)
		{
			if (sampleBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sampleBuffer));

			VTStatus status = VTFrameSiloAddSampleBuffer (Handle, sampleBuffer.Handle);
			GC.KeepAlive (sampleBuffer);
			return status;
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* OSStatus */ VTStatus VTFrameSiloSetTimeRangesForNextPass (
			/* VTFrameSiloRef */ IntPtr silo,
			/* CMItemCount */ nint timeRangeCount,
			/* const CMTimeRange * */ IntPtr timeRangeArray);

		/// <param name="ranges">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe VTStatus SetTimeRangesForNextPass (CMTimeRange [] ranges)
		{
			if (ranges is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ranges));

			if (ranges.Length > 0)
				fixed (CMTimeRange* first = ranges) {
					return VTFrameSiloSetTimeRangesForNextPass (Handle, ranges.Length, (IntPtr) first);
				}
			else
				return VTFrameSiloSetTimeRangesForNextPass (Handle, ranges.Length, IntPtr.Zero);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static /* OSStatus */ VTStatus VTFrameSiloGetProgressOfCurrentPass (
			/* VTFrameSiloRef */ IntPtr silo,
			/* Float32* */ float* progressOut);

		/// <param name="progress">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus GetProgressOfCurrentPass (out float progress)
		{
			progress = default;
			unsafe {
				return VTFrameSiloGetProgressOfCurrentPass (Handle, (float*) Unsafe.AsPointer<float> (ref progress));
			}
		}

#if !NET
		delegate VTStatus EachSampleBufferCallback (/* void* */ IntPtr callbackInfo, /* CMSampleBufferRef */ IntPtr sampleBufferPtr);

		static EachSampleBufferCallback static_EachSampleBufferCallback = new EachSampleBufferCallback (BufferCallback);
#endif

#if NET
		[UnmanagedCallersOnly]
#else
#if !MONOMAC
		[MonoPInvokeCallback (typeof (EachSampleBufferCallback))]
#endif
#endif
		static VTStatus BufferCallback (IntPtr callbackInfo, IntPtr sampleBufferPtr)
		{
			var gch = GCHandle.FromIntPtr (callbackInfo);
			var func = gch.Target as Func<CMSampleBuffer, VTStatus>;
			if (func is null)
				return (VTStatus) 1; // return non-zero to abort iteration early.
			var sampleBuffer = new CMSampleBuffer (sampleBufferPtr, false);
			return func (sampleBuffer);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static /* OSStatus */ VTStatus VTFrameSiloCallFunctionForEachSampleBuffer (
			/* VTFrameSiloRef */ IntPtr silo,
			/* CMTimeRange */ CMTimeRange timeRange, // CMTimeRange.Invalid retrieves all sample buffers
			/* void* */ IntPtr callbackInfo,
#if NET
			/* */ delegate* unmanaged<IntPtr, IntPtr, VTStatus> callback);
#else
			/* */ EachSampleBufferCallback callback);
#endif

		/// <param name="callback">To be added.</param>
		///         <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe VTStatus ForEach (Func<CMSampleBuffer, VTStatus> callback, CMTimeRange? range = null)
		{
			var callbackHandle = GCHandle.Alloc (callback);
#if NET
			var foreachResult = VTFrameSiloCallFunctionForEachSampleBuffer (Handle, range ?? CMTimeRange.InvalidRange, GCHandle.ToIntPtr (callbackHandle), &BufferCallback);
#else
			var foreachResult = VTFrameSiloCallFunctionForEachSampleBuffer (Handle, range ?? CMTimeRange.InvalidRange, GCHandle.ToIntPtr (callbackHandle), static_EachSampleBufferCallback);
#endif
			callbackHandle.Free ();
			return foreachResult;
		}
	}
}
