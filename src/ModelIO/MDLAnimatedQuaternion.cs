//
// MDLAnimatedQuaternion.cs
//
// Authors:
//	Rolf Bjarne Kvinge <rolf.kvinge@microsoft.com>
//
// Copyright 2019 Microsoft Corp. All rights reserved.
//

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

using Vector2d = global::CoreGraphics.NVector2d;
using Vector3 = global::CoreGraphics.NVector3;
using Vector3d = global::CoreGraphics.NVector3d;
using Vector4d = global::CoreGraphics.NVector4d;
using Matrix4 = global::CoreGraphics.NMatrix4;
using Matrix4d = global::CoreGraphics.NMatrix4d;
using Quaterniond = global::CoreGraphics.NQuaterniond;

#nullable enable

namespace ModelIO {
	public partial class MDLAnimatedQuaternion {
		public virtual void Reset (Quaternion [] values, double [] times)
		{
			if (values is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (values));
			if (times is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (times));
			if (values.Length != times.Length)
				throw new ArgumentOutOfRangeException ($"The '{nameof (values)}' and '{nameof (times)}' arrays must have the same length");
			int typeSize = Marshal.SizeOf<Quaternion> ();

			unsafe {
				fixed (Quaternion* valuesPtr = values)
					MDLMemoryHelper.Reset (typeSize, (IntPtr) valuesPtr, times, _ResetWithFloatQuaternionArray);
			}
		}

		public virtual void Reset (Quaterniond [] values, double [] times)
		{
			if (values is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (values));
			if (times is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (times));
			if (values.Length != times.Length)
				throw new ArgumentOutOfRangeException ($"The '{nameof (values)}' and '{nameof (times)}' arrays must have the same length");

			int typeSize = Marshal.SizeOf<Quaterniond> ();

			unsafe {
				fixed (Quaterniond* valuesPtr = values)
					MDLMemoryHelper.Reset (typeSize, (IntPtr) valuesPtr, times, _ResetWithDoubleQuaternionArray);
			}
		}

		public virtual Quaternion [] GetQuaternionValues (nuint maxCount)
		{
			var timesArr = new Quaternion [(int) maxCount];

			unsafe {
				int typeSize = sizeof (Quaternion);
				fixed (Quaternion* arrptr = timesArr) {
					var rv = MDLMemoryHelper.FetchValues (typeSize, (IntPtr) arrptr, maxCount, _GetFloatQuaternionArray);
					Array.Resize (ref timesArr, (int) rv);
				}
			}

			return timesArr;
		}

		public virtual Quaterniond [] GetQuaterniondValues (nuint maxCount)
		{
			var timesArr = new Quaterniond [(int) maxCount];

			unsafe {
				int typeSize = sizeof (Quaterniond);
				fixed (Quaterniond* arrptr = timesArr) {
					var rv = MDLMemoryHelper.FetchValues (typeSize, (IntPtr) arrptr, maxCount, _GetDoubleQuaternionArray);
					Array.Resize (ref timesArr, (int) rv);
				}
			}

			return timesArr;
		}
	}
}
