//
// MDLStereoscopicCamera Unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc.
//

#if !MONOMAC

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

using MatrixFloat2x2 = global::CoreGraphics.NMatrix2;
using MatrixFloat3x3 = global::CoreGraphics.NMatrix3;
using MatrixFloat4x4 = global::CoreGraphics.NMatrix4;
using VectorFloat3 = global::CoreGraphics.NVector3;
using Matrix4 = global::System.Numerics.Matrix4x4;
using Bindings.Test;
using NUnit.Framework;

namespace MonoTouchFixtures.ModelIO {
	[TestFixture]
	// we want the test to be available if we use the linker
	[Preserve (AllMembers = true)]
	public class MDLStereoscopicCameraTest {
		[OneTimeSetUp]
		public void Setup ()
		{
			TestRuntime.AssertXcodeVersion (7, 0);
		}

		[Test]
		public void Properties ()
		{
			using (var obj = new MDLStereoscopicCamera ()) {
				Assert.AreEqual (63f, obj.InterPupillaryDistance, "InterPupillaryDistance");
				Assert.AreEqual (0f, obj.LeftVergence, "LeftVergence");
				Assert.AreEqual (0f, obj.RightVergence, "RightVergence");
				Assert.AreEqual (0f, obj.Overlap, "Overlap");

				var mat1 = new Matrix4 (
					1, 0, 0, -0.63f,
					0, 1, 0, 0,
					0, 0, 1, 0,
					0, 0, 0, 1);
				Asserts.AreEqual (mat1, obj.LeftViewMatrix, "LeftViewMatrix");
				Asserts.AreEqual (mat1, CFunctions.GetMatrixFloat4x4 (obj, "leftViewMatrix"), "LeftViewMatrix4x4 native");

				var mat2 = new NMatrix4 (
					1, 0, 0, 0.63f,
					0, 1, 0, 0,
					0, 0, 1, 0,
					0, 0, 0, 1);
				Asserts.AreEqual (mat2, obj.RightViewMatrix, "RightViewMatrix");
				Asserts.AreEqual (mat2, CFunctions.GetMatrixFloat4x4 (obj, "rightViewMatrix"), "RightViewMatrix4x4 native");

				var mat3 = new Matrix4 (
					1.308407f, 0, 0, 0,
					0, 1.962611f, 0, 0,
					0, 0, -1.0002f, -0.20002f,
					0, 0, -1, 0);
				Asserts.AreEqual (mat3, obj.LeftProjectionMatrix, 0.0001f, "LeftProjectionMatrix");
				Asserts.AreEqual (mat3, CFunctions.GetMatrixFloat4x4 (obj, "leftProjectionMatrix"), 0.0001f, "LeftProjectionMatrix4x4 native");

				Asserts.AreEqual (mat3, obj.RightProjectionMatrix, 0.0001f, "RightProjectionMatrix");
				Asserts.AreEqual (mat3, CFunctions.GetMatrixFloat4x4 (obj, "rightProjectionMatrix"), 0.0001f, "RightProjectionMatrix4x4 native");
			}
		}
	}
}

#endif // !MONOMAC
