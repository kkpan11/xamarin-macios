//
// ARSkeleton2D.cs: Nicer code for ARSkeleton2D
//
// Authors:
//	Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2019 Microsoft Inc. All rights reserved.
//

using System;
using System.Numerics;
using System.Runtime.InteropServices;

#nullable enable

namespace ARKit {
	public partial class ARSkeleton2D {

		public unsafe Vector2 [] JointLandmarks {
			get {
				var count = (int) JointCount;
				var rv = new Vector2 [count];
				var ptr = (Vector2*) RawJointLandmarks;
				for (int i = 0; i < count; i++)
					rv [i] = *ptr++;
				return rv;
			}
		}
	}
}
