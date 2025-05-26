//
// MDLLight Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc.
//

using System;
using CoreGraphics;
using Foundation;
#if !MONOMAC
using UIKit;
#endif
#if !__TVOS__
using MultipeerConnectivity;
#endif
using ModelIO;
using ObjCRuntime;
using NUnit.Framework;

using System.Numerics;

namespace MonoTouchFixtures.ModelIO {

	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLLightTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Ignore ("Requires iOS 9.0+ or macOS 10.11+");
		}

		[Test]
		public void IrradianceAtPointTest ()
		{
			using (var obj = new MDLLight ()) {
				var color = obj.GetIrradiance (new Vector3 (1, 2, 3));
				Assert.IsNotNull (color, "color 1");
			}

			using (var obj = new MDLLight ()) {
				var color = obj.GetIrradiance (new Vector3 (1, 2, 3), CGColorSpace.CreateGenericRgb ());
				Assert.IsNotNull (color, "color 2");
			}
		}
	}
}
