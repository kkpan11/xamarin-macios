using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreAnimation {
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CAFrameRateRange {
		public float Minimum;

		public float Maximum;

		public float Preferred;

		[DllImport (Constants.QuartzLibrary, EntryPoint = "CAFrameRateRangeIsEqualToRange")]
		static extern byte IsEqualTo (CAFrameRateRange range, CAFrameRateRange other);

		[DllImport (Constants.QuartzLibrary, EntryPoint = "CAFrameRateRangeMake")]
		public static extern CAFrameRateRange Create (float minimum, float maximum, float preferred);

		public bool IsEqualTo (CAFrameRateRange other)
			=> IsEqualTo (this, other) != 0;

#if !COREBUILD
		[Field ("CAFrameRateRangeDefault", "CoreAnimation")]
		public static CAFrameRateRange Default => Marshal.PtrToStructure<CAFrameRateRange> (Dlfcn.GetIndirect (Libraries.CoreAnimation.Handle, "CAFrameRateRangeDefault"))!;
#endif

	}

}
