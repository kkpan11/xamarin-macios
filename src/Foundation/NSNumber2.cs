//
// Copyright 2010, Novell, Inc.
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	public partial class NSNumber : NSValue
#if COREBUILD
	{
#else
	, IComparable, IComparable<NSNumber>, IEquatable<NSNumber> {

		public static implicit operator NSNumber (float value)
		{
			return FromFloat (value);
		}

		public static implicit operator NSNumber (double value)
		{
			return FromDouble (value);
		}

		public static implicit operator NSNumber (bool value)
		{
			return FromBoolean (value);
		}

		public static implicit operator NSNumber (sbyte value)
		{
			return FromSByte (value);
		}

		public static implicit operator NSNumber (byte value)
		{
			return FromByte (value);
		}

		public static implicit operator NSNumber (short value)
		{
			return FromInt16 (value);
		}

		public static implicit operator NSNumber (ushort value)
		{
			return FromUInt16 (value);
		}

		public static implicit operator NSNumber (int value)
		{
			return FromInt32 (value);
		}

		public static implicit operator NSNumber (uint value)
		{
			return FromUInt32 (value);
		}

		public static implicit operator NSNumber (long value)
		{
			return FromInt64 (value);
		}

		public static implicit operator NSNumber (ulong value)
		{
			return FromUInt64 (value);
		}

		public static explicit operator byte (NSNumber source)
		{
			return source.ByteValue;
		}

		public static explicit operator sbyte (NSNumber source)
		{
			return source.SByteValue;
		}

		public static explicit operator short (NSNumber source)
		{
			return source.Int16Value;
		}

		public static explicit operator ushort (NSNumber source)
		{
			return source.UInt16Value;
		}

		public static explicit operator int (NSNumber source)
		{
			return source.Int32Value;
		}

		public static explicit operator uint (NSNumber source)
		{
			return source.UInt32Value;
		}

		public static explicit operator long (NSNumber source)
		{
			return source.Int64Value;
		}

		public static explicit operator ulong (NSNumber source)
		{
			return source.UInt64Value;
		}

		public static explicit operator float (NSNumber source)
		{
			return source.FloatValue;
		}

		public static explicit operator double (NSNumber source)
		{
			return source.DoubleValue;
		}

		public static explicit operator bool (NSNumber source)
		{
			return source.BoolValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a boolean.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static bool ToBool (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.BoolValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a byte.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static byte ToByte (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.ByteValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a sbyte.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static sbyte ToSByte (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.SByteValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a short.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static short ToInt16 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int16Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a ushort.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static ushort ToUInt16 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt16Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as an int.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static int ToInt32 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int32Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a uint.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static uint ToUInt32 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt32Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a long.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static long ToInt64 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.Int64Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a ulong.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static ulong ToUInt64 (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.UInt64Value;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a float.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static float ToFloat (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.FloatValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a nfloat.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static nfloat ToNFloat (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NFloatValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a double.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static double ToDouble (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.DoubleValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a nint.
		/// </summary>
		/// <param name="handle">The NativeHandle to convert.</param>
		public static nint ToNInt (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NIntValue;
		}

		/// <summary>
		/// Convert a NativeHandle to a NSNumber and return its value as a nuint.
		/// </summary>
		public static nuint ToNUInt (NativeHandle handle)
		{
			using var num = Runtime.GetNSObject<NSNumber> (handle)!;
			return num.NUIntValue;
		}

		/// <param name="value">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public NSNumber (nfloat value) :
			this ((double) value)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nfloat NFloatValue {
			get {
				return (nfloat) DoubleValue;
			}
		}

		/// <param name="value">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSNumber FromNFloat (nfloat value)
		{
			return (FromDouble ((double) value));
		}

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return StringValue;
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int CompareTo (object obj)
		{
			return CompareTo (obj as NSNumber);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int CompareTo (NSNumber other)
		{
			// value must not be `nil` to call the `compare:` selector
			// that match well with the not same type of .NET check
			if (other is null)
				throw new ArgumentException ("other");
			return (int) Compare (other);
		}

		// should be present when implementing IComparable
		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object other)
		{
			return Equals (other as NSNumber);
		}

		// IEquatable<NSNumber>
		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (NSNumber other)
		{
			if (other is null)
				return false;
			bool result = IsEqualTo (other.Handle);
			GC.KeepAlive (other);
			return result;
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		///         <returns>A int containing the hash code for this instance.</returns>
		///         <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			// this is heavy weight :( but it's the only way to follow .NET rule where:
			// "If two objects compare as equal, the GetHashCode method for each object must return the same value."
			// otherwise NSNumber (1) needs to be != from NSNumber (1d), a breaking change from classic and 
			// something that's really not obvious
			return StringValue.GetHashCode ();
		}

		public bool IsEqualTo (NSNumber number)
		{
			var result = IsEqualTo (number.GetHandle ());
			GC.KeepAlive (number);
			return result;
		}
#endif
	}
}
