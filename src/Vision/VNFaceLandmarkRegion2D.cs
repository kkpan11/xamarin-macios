//
// VNFaceLandmarkRegion2D.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using CoreGraphics;

namespace Vision {
	public partial class VNFaceLandmarkRegion2D {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual CGPoint []? NormalizedPoints {
			get {
				var ret = _GetNormalizedPoints ();
				if (ret == IntPtr.Zero)
					return null;

				unsafe {
					var count = (int) PointCount;
					var rv = new CGPoint [count];
					var ptr = (CGPoint*) ret;
					for (int i = 0; i < count; i++)
						rv [i] = *ptr++;
					return rv;
				}
			}
		}

		/// <param name="imageSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual CGPoint []? GetPointsInImage (CGSize imageSize)
		{
			// return the address of the array of pointCount points
			// or NULL if the conversion could not take place.
			var ret = _GetPointsInImage (imageSize);
			if (ret == IntPtr.Zero)
				return null;

			unsafe {
				var count = (int) PointCount;
				var rv = new CGPoint [count];
				var ptr = (CGPoint*) ret;
				for (int i = 0; i < count; i++)
					rv [i] = *ptr++;
				return rv;
			}
		}
	}
}
