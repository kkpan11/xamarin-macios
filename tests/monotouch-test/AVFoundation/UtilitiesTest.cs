//
// Unit tests for AVUtilities.h helpers
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using AVFoundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	[TestFixture]
	public class UtilitiesTest {

		[Test]
		public void AspectRatio ()
		{
			var r = CGRect.Empty.WithAspectRatio (CGSize.Empty);
			Assert.True (nfloat.IsNaN (r.Top), "Top");
			Assert.That (nfloat.IsNaN (r.Left), "Left");
			Assert.That (nfloat.IsNaN (r.Width), "Width");
			Assert.That (nfloat.IsNaN (r.Height), "Height");
		}
	}

#if __MACOS__
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVStructTest {

		[Test]
		public void StructSizeTest ()
		{
			if (!TestRuntime.CheckXcodeVersion (6, 1))
				Assert.Ignore ("Ignoring Tests: Requires Xcode 6.1+ API");

			Assert.That (Marshal.SizeOf<AVSampleCursorSyncInfo> (), Is.EqualTo (3), "AVSampleCursorSyncInfo Size");
			Assert.That (Marshal.SizeOf<AVSampleCursorDependencyInfo> (), Is.EqualTo (6), "AVSampleCursorDependencyInfo Size");
			Assert.That (Marshal.SizeOf<AVSampleCursorStorageRange> (), Is.EqualTo (16), "AVSampleCursorStorageRange Size");
			Assert.That (Marshal.SizeOf<AVSampleCursorChunkInfo> (), Is.EqualTo (IntPtr.Size == 8 ? 16 : 12), "AVSampleCursorChunkInfo Size");
		}
	}
#endif // __MACOS__
}
