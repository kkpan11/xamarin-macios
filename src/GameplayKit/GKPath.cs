//
// GKPath.cs: Implements some nicer methods for GKPath
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Numerics;
using Foundation;
using ObjCRuntime;

using System.Runtime.InteropServices;

namespace GameplayKit {
	public partial class GKPath {

		public static GKPath FromPoints (Vector2 [] points, float radius, bool cyclical)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));

			var buffer = IntPtr.Zero;
			try {
				PrepareBuffer (out buffer, ref points);

				return FromPoints (buffer, (nuint) points.Length, radius, cyclical);
			} finally {
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
			}
		}

		[DesignatedInitializer]
		public GKPath (Vector2 [] points, float radius, bool cyclical)
			: base (NSObjectFlag.Empty)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));

			var buffer = IntPtr.Zero;
			try {
				PrepareBuffer (out buffer, ref points);

				InitializeHandle (_InitWithPoints (buffer, (nuint) points.Length, radius, cyclical), "initWithPoints:count:radius:cyclical:");
			} finally {
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static GKPath FromPoints (Vector3 [] points, float radius, bool cyclical)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));

			var buffer = IntPtr.Zero;
			try {
				PrepareBuffer (out buffer, ref points);

				return FromFloat3Points (buffer, (nuint) points.Length, radius, cyclical);
			} finally {
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public GKPath (Vector3 [] points, float radius, bool cyclical)
			: base (NSObjectFlag.Empty)
		{
			if (points is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (points));

			var buffer = IntPtr.Zero;
			try {
				PrepareBuffer (out buffer, ref points);

				InitializeHandle (_InitWithFloat3Points (buffer, (nuint) points.Length, radius, cyclical), "initWithFloat3Points:count:radius:cyclical:");
			} finally {
				if (buffer != IntPtr.Zero)
					Marshal.FreeHGlobal (buffer);
			}
		}

		static void PrepareBuffer<T> (out IntPtr buffer, ref T [] points) where T : struct
		{
			var type = typeof (T);
			// Vector3 is 12 bytes but vector_float3 is 16
			var size = type == typeof (Vector3) ? 16 : Marshal.SizeOf<T> ();
			var length = points.Length * size;
			buffer = Marshal.AllocHGlobal (length);

			for (int i = 0; i < points.Length; i++)
				Marshal.StructureToPtr<T> (points [i], IntPtr.Add (buffer, i * size), false);
		}
	}
}
