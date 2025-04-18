// 
// CGPDFDictionary.cs: Implements the managed CGPDFDictionary binding
//
// Authors:
//	Miguel de Icaza <miguel@xamarin.com>
//	Sebastien Pouliot <sebastien@xamarin.com>
// 
// Copyright 2010 Novell, Inc
// Copyright 2011-2014 Xamarin Inc. All rights reserved
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

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	/// <summary>Represents a PDF Dictionary.</summary>
	///     <remarks>Dictionaries are used extensively in the PDF file format.
	///     Instances of this class represent dictionaries in your documents
	///     and the methods in this class can be used to look up the values in
	///     the dictionary or iterate over all of the elements of
	///     it.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPDFDictionary.h
	public class CGPDFDictionary : CGPDFObject {
		// The lifetime management of CGPDFObject (and CGPDFArray, CGPDFDictionary and CGPDFStream) are tied to
		// the containing CGPDFDocument, and not possible to handle independently, which is why this class
		// does not subclass NativeObject (there's no way to retain/release CGPDFObject instances). It's
		// also why this constructor doesn't have a 'bool owns' parameter: it's always owned by the containing CGPDFDocument.
		internal CGPDFDictionary (NativeHandle handle)
			: base (handle)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGPDFDictionaryGetCount (/* CGPDFDictionaryRef */ IntPtr dict);

		/// <summary>The number of items on this dictionary.</summary>
		///         <value>
		///         </value>
		///         <remarks>You can iterate over the dictionary elements by using the Apply method.</remarks>
		public int Count {
			get {
				return (int) CGPDFDictionaryGetCount (Handle);
			}
		}

		// CGPDFBoolean -> unsigned char -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetBoolean (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFBoolean* */ byte* value);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="result">The boolean value, if the function returns true.</param>
		///         <summary>Looks up a boolean value by name on the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetBoolean (string key, out bool result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			using var keyPtr = new TransientString (key);
			byte byteresult;
			unsafe {
				var rv = CGPDFDictionaryGetBoolean (Handle, keyPtr, &byteresult) != 0;
				result = byteresult != 0;
				return rv;
			}
		}

		// CGPDFInteger -> long int so 32/64 bits -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetInteger (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFInteger* */ nint* value);

		public bool GetInt (string key, out nint result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			using var keyPtr = new TransientString (key);
			result = default;
			unsafe {
				return CGPDFDictionaryGetInteger (Handle, keyPtr, (nint*) Unsafe.AsPointer<nint> (ref result)) != 0;
			}
		}

		// CGPDFReal -> CGFloat -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetNumber (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFReal* */ nfloat* value);

		public bool GetFloat (string key, out nfloat result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			using var keyPtr = new TransientString (key);
			result = default;
			unsafe {
				return CGPDFDictionaryGetNumber (Handle, keyPtr, (nfloat*) Unsafe.AsPointer<nfloat> (ref result)) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetName (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* const char ** */ IntPtr* value);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="result">The name, if the function returns true.</param>
		///         <summary>Looks up a name in the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetName (string key, out string? result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));
			using var keyPtr = new TransientString (key);
			bool r;
			IntPtr res;
			unsafe {
				r = CGPDFDictionaryGetName (Handle, keyPtr, &res) != 0;
			}
			result = r ? Marshal.PtrToStringAnsi (res) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetDictionary (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFDictionaryRef* */ IntPtr* result);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="result">The dictionary, if the function returns true.</param>
		///         <summary>Looks up a dictionary value by name on the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetDictionary (string key, out CGPDFDictionary? result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			using var keyPtr = new TransientString (key);
			IntPtr res;
			bool r;
			unsafe {
				r = CGPDFDictionaryGetDictionary (Handle, keyPtr, &res) != 0;
			}
			result = r ? new CGPDFDictionary (res) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetStream (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFStreamRef* */ IntPtr* value);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="result">The stream, if the function returns true.</param>
		///         <summary>Looks up a CGPDFStream in the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetStream (string key, out CGPDFStream? result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			using var keyPtr = new TransientString (key);
			bool r;
			IntPtr ptr;
			unsafe {
				r = CGPDFDictionaryGetStream (Handle, keyPtr, &ptr) != 0;
			}
			result = r ? new CGPDFStream (ptr) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetArray (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFArrayRef* */ IntPtr* value);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="array">The array, if the function returns true.</param>
		///         <summary>Looks up an array value by name on the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetArray (string key, out CGPDFArray? array)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			using var keyPtr = new TransientString (key);
			bool r;
			IntPtr ptr;
			unsafe {
				r = CGPDFDictionaryGetArray (Handle, keyPtr, &ptr) != 0;
			}
			array = r ? new CGPDFArray (ptr) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static void CGPDFDictionaryApplyFunction (/* CGPDFDictionaryRef */ IntPtr dic, delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> function, /* void* */ IntPtr info);

		/// <param name="key">To be added.</param>
		///     <param name="value">To be added.</param>
		///     <param name="info">To be added.</param>
		///     <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void ApplyCallback (string? key, object? value, object? info);

		[UnmanagedCallersOnly]
		static void ApplyBridge (IntPtr key, IntPtr pdfObject, IntPtr info)
		{
			var data = GCHandle.FromIntPtr (info).Target as Tuple<ApplyCallback, object?>;
			if (data is null)
				return;

			var callback = data.Item1;
			if (callback is not null)
				callback (Marshal.PtrToStringUTF8 (key), CGPDFObject.FromHandle (pdfObject), data.Item2);
		}

		/// <param name="callback">To be added.</param>
		///         <param name="info">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Apply (ApplyCallback callback, object? info = null)
		{
			var data = new Tuple<ApplyCallback, object?> (callback, info);
			var gch = GCHandle.Alloc (data);
			try {
				unsafe {
					CGPDFDictionaryApplyFunction (Handle, &ApplyBridge, GCHandle.ToIntPtr (gch));
				}
			} finally {
				gch.Free ();
			}
		}

		[UnmanagedCallersOnly]
		static void ApplyBridge2 (IntPtr key, IntPtr pdfObject, IntPtr info)
		{
			var callback = GCHandle.FromIntPtr (info).Target as Action<string?, CGPDFObject>;
			if (callback is not null)
				callback (Marshal.PtrToStringUTF8 (key), new CGPDFObject (pdfObject));
		}

		/// <param name="callback">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Apply (Action<string?, CGPDFObject> callback)
		{
			GCHandle gch = GCHandle.Alloc (callback);
			unsafe {
				CGPDFDictionaryApplyFunction (Handle, &ApplyBridge2, GCHandle.ToIntPtr (gch));
			}
			gch.Free ();
		}

		// CGPDFDictionary.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFDictionaryGetString (/* CGPDFDictionaryRef */ IntPtr dict, /* const char* */ IntPtr key, /* CGPDFStringRef* */ IntPtr* value);

		/// <param name="key">The name of the element to get out of the dictionary.</param>
		///         <param name="result">The string, if the function returns true.</param>
		///         <summary>Looks up a string in the dictionary.</summary>
		///         <returns>true if the value was found on the dictionary and the out parameter set to the value.   If the value is false, the result of the out parameter is undefined.</returns>
		///         <remarks>
		///         </remarks>
		public bool GetString (string key, out string? result)
		{
			if (key is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (key));

			using var keyPtr = new TransientString (key);
			bool r;
			IntPtr res;
			unsafe {
				r = CGPDFDictionaryGetString (Handle, keyPtr, &res) != 0;
			}
			result = r ? CGPDFString.ToString (res) : null;
			return r;
		}
	}
}
