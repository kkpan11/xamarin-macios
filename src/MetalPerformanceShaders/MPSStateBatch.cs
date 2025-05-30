//
// MPSStateBatch.cs
//
// Authors:
//	Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2019 Microsoft Corporation.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using Metal;

namespace MetalPerformanceShaders {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static partial class MPSStateBatch {

		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern nuint MPSStateBatchIncrementReadCount (IntPtr batch, nint amount);

		// Using 'NSArray<MPSState>' instead of `MPSState[]` because array 'Handle' matters.
		public static nuint IncrementReadCount (NSArray<MPSState> stateBatch, nint amount)
		{
			if (stateBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (stateBatch));

			nuint count = MPSStateBatchIncrementReadCount (stateBatch.Handle, amount);
			GC.KeepAlive (stateBatch);
			return count;
		}

		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern void MPSStateBatchSynchronize (IntPtr batch, IntPtr /* id<MTLCommandBuffer> */ cmdBuf);

		// Using 'NSArray<MPSState>' instead of `MPSState[]` because array 'Handle' matters.
		/// <param name="stateBatch">To be added.</param>
		///         <param name="commandBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void Synchronize (NSArray<MPSState> stateBatch, IMTLCommandBuffer commandBuffer)
		{
			if (stateBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (stateBatch));
			if (commandBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (commandBuffer));

			MPSStateBatchSynchronize (stateBatch.Handle, commandBuffer.Handle);
			GC.KeepAlive (stateBatch);
			GC.KeepAlive (commandBuffer);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalPerformanceShadersLibrary)]
		static extern nuint MPSStateBatchResourceSize (IntPtr batch);

		// Using 'NSArray<MPSState>' instead of `MPSState[]` because array 'Handle' matters.
		/// <param name="stateBatch">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static nuint GetResourceSize (NSArray<MPSState> stateBatch)
		{
			if (stateBatch is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (stateBatch));

			nuint size = MPSStateBatchResourceSize (stateBatch.Handle);
			GC.KeepAlive (stateBatch);
			return size;
		}
	}
}
