//
// SCNPhysicsShape.cs: extensions to SCNPhysicsShape
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

#nullable enable

namespace SceneKit {
	public partial class SCNPhysicsShape {
		/// <param name="shapes">A list of shapes to transform.</param>
		///         <param name="transforms">A list of transforms to apply..</param>
		///         <summary>Creates and returns a new physics shape by applying the specified <paramref name="transforms" /> to the specified shapes <paramref name="shapes" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SCNPhysicsShape Create (SCNPhysicsShape [] shapes, SCNMatrix4 [] transforms)
		{
			if (shapes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (shapes));

			if (transforms is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (nameof (transforms));

			var t = new NSValue [transforms.Length];
			for (var i = 0; i < t.Length; i++)
				t [i] = NSValue.FromSCNMatrix4 (transforms [i]);

			return Create (shapes, t);
		}

		/// <param name="geometry">To be added.</param>
		///         <param name="shapeType">To be added.</param>
		///         <param name="keepAsCompound">To be added.</param>
		///         <param name="scale">To be added.</param>
		///         <summary>Creates and returns a new physics shape from the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SCNPhysicsShape Create (SCNGeometry geometry,
			SCNPhysicsShapeType? shapeType = null,
			bool? keepAsCompound = null,
			SCNVector3? scale = null)
		{
			return Create (geometry, new SCNPhysicsShapeOptions {
				ShapeType = shapeType,
				KeepAsCompound = keepAsCompound,
				Scale = scale,
			}.ToDictionary ());
		}

		/// <param name="geometry">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>Creates and returns a new physics shape from the specified <paramref name="geometry" /> and <paramref name="options" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SCNPhysicsShape Create (SCNGeometry geometry, SCNPhysicsShapeOptions? options)
		{
			return Create (geometry, options?.ToDictionary ());
		}

		/// <param name="node">To be added.</param>
		///         <param name="shapeType">To be added.</param>
		///         <param name="keepAsCompound">To be added.</param>
		///         <param name="scale">To be added.</param>
		///         <summary>Creates and returns a new physics shape from the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SCNPhysicsShape Create (SCNNode node,
			SCNPhysicsShapeType? shapeType = null,
			bool? keepAsCompound = null,
			SCNVector3? scale = null)
		{
			return Create (node, new SCNPhysicsShapeOptions {
				ShapeType = shapeType,
				KeepAsCompound = keepAsCompound,
				Scale = scale,
			}.ToDictionary ());
		}

		/// <param name="node">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>Creates and returns a new physics shape from the specified <paramref name="node" /> and <paramref name="options" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SCNPhysicsShape Create (SCNNode node, SCNPhysicsShapeOptions? options)
		{
			return Create (node, options?.ToDictionary ());
		}

		/// <summary>Gets the options that were used when creating the physics shape.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNPhysicsShapeOptions? Options {
			get {
				var o = _Options;
				if (o is null)
					return null;
				return new SCNPhysicsShapeOptions (o);
			}
		}
	}

	/// <summary>Valid keys for the <c>options</c> creating new <see cref="SceneKit.SCNPhysicsShape" /> instances.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public class SCNPhysicsShapeOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNPhysicsShapeType? ShapeType { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? KeepAsCompound { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SCNVector3? Scale { get; set; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SCNPhysicsShapeOptions () { }

		internal SCNPhysicsShapeOptions (NSDictionary source)
		{
			var ret = source [SCNPhysicsShapeOptionsKeys.Type] as NSString;
			if (ret is not null) {
				if (ret == SCNPhysicsShapeOptionsTypes.BoundingBox)
					ShapeType = SCNPhysicsShapeType.BoundingBox;
				else if (ret == SCNPhysicsShapeOptionsTypes.ConcavePolyhedron)
					ShapeType = SCNPhysicsShapeType.ConcavePolyhedron;
				else if (ret == SCNPhysicsShapeOptionsTypes.ConvexHull)
					ShapeType = SCNPhysicsShapeType.ConvexHull;
			}
			var bret = source [SCNPhysicsShapeOptionsKeys.KeepAsCompound] as NSNumber;
			if (bret is not null)
				KeepAsCompound = bret.Int32Value != 0;
			var nret = source [SCNPhysicsShapeOptionsKeys.Scale] as NSValue;
			if (nret is not null)
				Scale = nret.Vector3Value;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary? ToDictionary ()
		{
			var n = 0;
			if (ShapeType.HasValue) n++;
			if (KeepAsCompound.HasValue) n++;
			if (Scale.HasValue) n++;

			if (n == 0)
				return null;

			var i = 0;
			var keys = new NSString [n];
			var values = new NSObject [n];

			if (ShapeType.HasValue) {
				keys [i] = SCNPhysicsShapeOptionsKeys.Type;
				switch (ShapeType.Value) {
				case SCNPhysicsShapeType.BoundingBox:
					values [i] = SCNPhysicsShapeOptionsTypes.BoundingBox;
					break;
				case SCNPhysicsShapeType.ConcavePolyhedron:
					values [i] = SCNPhysicsShapeOptionsTypes.ConcavePolyhedron;
					break;
				case SCNPhysicsShapeType.ConvexHull:
				default:
					values [i] = SCNPhysicsShapeOptionsTypes.ConvexHull;
					break;
				}
			}

			if (KeepAsCompound.HasValue) {
				keys [i] = SCNPhysicsShapeOptionsKeys.KeepAsCompound;
				values [i] = new NSNumber (KeepAsCompound.Value);
			}

			if (Scale.HasValue) {
				keys [i] = SCNPhysicsShapeOptionsKeys.Scale;
				values [i] = NSValue.FromVector (Scale.Value);
			}

			return NSDictionary.FromObjectsAndKeys (values, keys);
		}
	}
}
