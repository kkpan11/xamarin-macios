// 
// CFAllocator.cs
//
// Authors:
//    Rolf Bjarne Kvinge
//    Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
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
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

namespace CoreFoundation {

	// CFBase.h
	public partial class CFAllocator : NativeObject {
#if !COREBUILD
		static CFAllocator? Default_cf;
		static CFAllocator? SystemDefault_cf;
		static CFAllocator? Malloc_cf;
		static CFAllocator? MallocZone_cf;
		static CFAllocator? Null_cf;
#endif

		[Preserve (Conditional = true)]
		internal CFAllocator (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CFAllocator Default {
			get {
				return Default_cf ?? (Default_cf = new CFAllocator (default_ptr, false));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CFAllocator SystemDefault {
			get {
				return SystemDefault_cf ?? (SystemDefault_cf = new CFAllocator (system_default_ptr, false));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CFAllocator Malloc {
			get {
				return Malloc_cf ?? (Malloc_cf = new CFAllocator (malloc_ptr, false));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CFAllocator MallocZone {
			get {
				return MallocZone_cf ?? (MallocZone_cf = new CFAllocator (malloc_zone_ptr, false));
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CFAllocator Null {
			get {
				return Null_cf ?? (Null_cf = new CFAllocator (null_ptr, false));
			}
		}
#endif

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern /* void* */ IntPtr CFAllocatorAllocate (/* CFAllocatorRef*/ IntPtr allocator, /*CFIndex*/ nint size, /* CFOptionFlags */ nuint hint);

		/// <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public IntPtr Allocate (long size)
		{
			return CFAllocatorAllocate (Handle, (nint) size, 0);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFAllocatorDeallocate (/* CFAllocatorRef */ IntPtr allocator, /* void* */ IntPtr ptr);

		/// <param name="ptr">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Deallocate (IntPtr ptr)
		{
			CFAllocatorDeallocate (Handle, ptr);
		}

		/// <summary>Type identifier for the CoreFoundation.CFAllocator type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isCFAllocator = (CFType.GetTypeID (foo.Handle) == CFAllocator.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFAllocatorGetTypeID")]
		public extern static /* CFTypeID */ nint GetTypeID ();
	}
}
