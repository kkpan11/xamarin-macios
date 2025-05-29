//
// NSString2.cs: Support code added after the generator has run for NSString
// 
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011-2014 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

#nullable enable

namespace Foundation {

	public partial class NSString : IComparable<NSString> {
		const string selDataUsingEncodingAllow = "dataUsingEncoding:allowLossyConversion:";

#if MONOMAC
		static IntPtr selDataUsingEncodingAllowHandle = Selector.GetHandle (selDataUsingEncodingAllow);
#endif

		/// <param name="enc">To be added.</param>
		///         <param name="allowLossyConversion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData Encode (NSStringEncoding enc, bool allowLossyConversion = false)
		{
			return new NSData (Messaging.NativeHandle_objc_msgSend_NativeHandle_bool (Handle, Selector.GetHandle (selDataUsingEncodingAllow), (IntPtr) (int) enc, allowLossyConversion ? (byte) 1 : (byte) 0));
		}

		/// <param name="data">The byte buffer.</param>
		///         <param name="encoding">Use this encoding to intepret the byte buffer.</param>
		///         <summary>Creates an NSString from an NSData source.</summary>
		///         <returns>An NSString created by parsing the byte buffer using the specified encoding.</returns>
		///         <remarks>To be added.</remarks>
		public static NSString? FromData (NSData data, NSStringEncoding encoding)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			NSString? ret = null;
			try {
				ret = new NSString (data, encoding);
			} catch (Exception) {

			}
			return ret;
		}

		public int CompareTo (NSString? other)
		{
			// `compare:` behavior is undefined if `nil` is provided, so we need to handle that case.
			if (other is null)
				return -1;
			return (int) Compare (other);
		}

		public char this [nint idx] {
			get {
				return _characterAtIndex (idx);
			}
		}
	}
}
