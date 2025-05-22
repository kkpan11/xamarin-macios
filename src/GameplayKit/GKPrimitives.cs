//
// GKPrimitives.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace GameplayKit {
	/// <summary>An axis-aligned rectangular three-dimensional box.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct GKBox {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Min;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector3 Max;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct GKQuad {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector2 Min;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public Vector2 Max;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct GKTriangle {
		Vector3 point1;
		Vector3 point2;
		Vector3 point3;
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Vector3 [] Points {
			get {
				return new Vector3 [] { point1, point2, point3 };
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				if (value.Length != 3)
					throw new ArgumentOutOfRangeException (nameof (value), "The length of the Value array must be 3");
				point1 = value [0];
				point2 = value [1];
				point3 = value [2];
			}
		}
	}
}
