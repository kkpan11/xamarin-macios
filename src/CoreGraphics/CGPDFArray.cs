// 
// CGPDFArray.cs: Implements the managed CGPDFArray binding
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
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	/// <summary>Represents a PDF array</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPDFArray.h
	public class CGPDFArray : CGPDFObject {
		// The lifetime management of CGPDFObject (and CGPDFArray, CGPDFDictionary and CGPDFStream) are tied to
		// the containing CGPDFDocument, and not possible to handle independently, which is why this class
		// does not subclass NativeObject (there's no way to retain/release CGPDFObject instances). It's
		// also why this constructor doesn't have a 'bool owns' parameter: it's always owned by the containing CGPDFDocument.
		internal CGPDFArray (NativeHandle handle)
			: base (handle)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* size_t */ nint CGPDFArrayGetCount (/* CGPDFArrayRef */ IntPtr array);

		/// <summary>The number of elements in the PDF array.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public nint Count {
			get {
				return CGPDFArrayGetCount (Handle);
			}
		}

		// CGPDFBoolean -> unsigned char -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetBoolean (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFBoolean* */ byte* value);

		public unsafe bool GetBoolean (nint idx, out bool result)
		{
			byte res = 0;
			var rv = CGPDFArrayGetBoolean (Handle, idx, &res) != 0;
			result = res != 0;
			return rv;
		}

		// CGPDFInteger -> long int 32/64 bits -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetInteger (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFInteger* */ nint* value);

		public bool GetInt (nint idx, out nint result)
		{
			result = default;
			unsafe {
				return CGPDFArrayGetInteger (Handle, idx, (nint*) Unsafe.AsPointer<nint> (ref result)) != 0;
			}
		}

		// CGPDFReal -> CGFloat -> CGPDFObject.h

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetNumber (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFReal* */ nfloat* value);

		public bool GetFloat (nint idx, out nfloat result)
		{
			result = default;
			unsafe {
				return CGPDFArrayGetNumber (Handle, idx, (nfloat*) Unsafe.AsPointer<nfloat> (ref result)) != 0;
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetName (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* const char** */ IntPtr* value);

		public bool GetName (nint idx, out string? result)
		{
			IntPtr res;
			bool r;
			unsafe {
				r = CGPDFArrayGetName (Handle, idx, &res) != 0;
			}
			result = r ? Marshal.PtrToStringAnsi (res) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetDictionary (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFDictionaryRef* */ IntPtr* value);

		public bool GetDictionary (nint idx, out CGPDFDictionary? result)
		{
			IntPtr res;
			bool r;
			unsafe {
				r = CGPDFArrayGetDictionary (Handle, idx, &res) != 0;
			}
			result = r ? new CGPDFDictionary (res) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetStream (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFStreamRef* */ IntPtr* value);

		public bool GetStream (nint idx, out CGPDFStream? result)
		{
			IntPtr ptr;
			bool r;
			unsafe {
				r = CGPDFArrayGetStream (Handle, idx, &ptr) != 0;
			}
			result = r ? new CGPDFStream (ptr) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetArray (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFArrayRef* */ IntPtr* value);

		public bool GetArray (nint idx, out CGPDFArray? array)
		{
			bool r;
			IntPtr ptr;
			unsafe {
				r = CGPDFArrayGetArray (Handle, idx, &ptr) != 0;
			}
			array = r ? new CGPDFArray (ptr) : null;
			return r;
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayGetString (/* CGPDFArrayRef */ IntPtr array, /* size_t */ nint index, /* CGPDFStringRef* */ IntPtr* value);

		public bool GetString (nint idx, out string? result)
		{
			IntPtr res;
			bool r;
			unsafe {
				r = CGPDFArrayGetString (Handle, idx, &res) != 0;
			}
			result = r ? CGPDFString.ToString (res) : null;
			return r;
		}

		[UnmanagedCallersOnly]
		static byte ApplyBlockHandler (IntPtr block, nint index, IntPtr value, IntPtr info)
		{
			var del = BlockLiteral.GetTarget<ApplyCallback> (block);
			if (del is not null) {
				var context = info == IntPtr.Zero ? null : GCHandle.FromIntPtr (info).Target;
				return del (index, CGPDFObject.FromHandle (value), context) ? (byte) 1 : (byte) 0;
			}

			return 0;
		}

		/// <param name="index">To be added.</param>
		///     <param name="value">To be added.</param>
		///     <param name="info">To be added.</param>
		///     <summary>To be added.</summary>
		///     <returns>To be added.</returns>
		///     <remarks>To be added.</remarks>
		public delegate bool ApplyCallback (nint index, object? value, object? info);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static byte CGPDFArrayApplyBlock (/* CGPDFArrayRef */ IntPtr array, /* CGPDFArrayApplierBlock */ BlockLiteral* block, /* void* */ IntPtr info);

		/// <param name="callback">To be added.</param>
		///         <param name="info">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public bool Apply (ApplyCallback callback, object? info = null)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));

			unsafe {
				delegate* unmanaged<IntPtr, nint, IntPtr, IntPtr, byte> trampoline = &ApplyBlockHandler;
				using var block = new BlockLiteral (trampoline, callback, typeof (CGPDFArray), nameof (ApplyBlockHandler));
				var gc_handle = info is null ? default (GCHandle) : GCHandle.Alloc (info);
				try {
					return CGPDFArrayApplyBlock (Handle, &block, info is null ? IntPtr.Zero : GCHandle.ToIntPtr (gc_handle)) != 0;
				} finally {
					if (info is not null)
						gc_handle.Free ();
				}
			}
		}
	}
}
