//
// CGLPixelFormat.cs: Implements the managed CGLPixelFormat
//
// Authors: Mono Team
//
// Copyright 2009-2010 Novell, Inc
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
using System.Collections.Generic;

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace OpenGL {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' Framework instead.")]
	public class CGLPixelFormat : NativeObject {
		protected internal override void Retain ()
		{
			CGLRetainPixelFormat (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGLReleasePixelFormat (GetCheckedHandle ());
		}

		[Preserve (Conditional = true)]
		internal CGLPixelFormat (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLRetainPixelFormat (IntPtr handle);

		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLReleasePixelFormat (IntPtr handle);

		[DllImport (Constants.OpenGLLibrary)]
		unsafe extern static CGLErrorCode CGLChoosePixelFormat (CGLPixelFormatAttribute* attributes, IntPtr* /* CGLPixelFormatObj* */ pix, int* /* GLint* */ npix);

#if !COREBUILD
		/// <param name="attributes">To be added.</param>
		///         <param name="npix">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGLPixelFormat (CGLPixelFormatAttribute [] attributes, out int npix)
			: base (Create (attributes, out npix), true)
		{
		}

		static IntPtr Create (CGLPixelFormatAttribute [] attributes, out int npix)
		{
			if (attributes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (attributes));
			IntPtr pixelFormatOut;
			var marshalAttribs = new CGLPixelFormatAttribute [attributes.Length + 1];

			Array.Copy (attributes, marshalAttribs, attributes.Length);

			npix = default;

			CGLErrorCode ret;
			unsafe {
				fixed (CGLPixelFormatAttribute* marshalAttribsPtr = marshalAttribs) {
					ret = CGLChoosePixelFormat (marshalAttribsPtr, &pixelFormatOut, (int*) Unsafe.AsPointer<int> (ref npix));
				}
			}

			if (ret != CGLErrorCode.NoError) {
				throw new Exception ("CGLChoosePixelFormat returned: " + ret);
			}

			return pixelFormatOut;
		}

		/// <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGLPixelFormat (params object [] attributes)
			: base (Create (ConvertToAttributes (attributes), out _), true)
		{
		}

		/// <param name="npix">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGLPixelFormat (out int npix, params object [] attributes) : this (ConvertToAttributes (attributes), out npix)
		{
		}

		static CGLPixelFormatAttribute [] ConvertToAttributes (object [] args)
		{
			var list = new List<CGLPixelFormatAttribute> ();
			for (int i = 0; i < args.Length; i++) {
				var v = (CGLPixelFormatAttribute) args [i];
				switch (v) {
				case CGLPixelFormatAttribute.AllRenderers:
				case CGLPixelFormatAttribute.DoubleBuffer:
				case CGLPixelFormatAttribute.Stereo:
				case CGLPixelFormatAttribute.MinimumPolicy:
				case CGLPixelFormatAttribute.MaximumPolicy:
				case CGLPixelFormatAttribute.OffScreen:
				case CGLPixelFormatAttribute.FullScreen:
				case CGLPixelFormatAttribute.SingleRenderer:
				case CGLPixelFormatAttribute.NoRecovery:
				case CGLPixelFormatAttribute.Accelerated:
				case CGLPixelFormatAttribute.ClosestPolicy:
				case CGLPixelFormatAttribute.Robust:
				case CGLPixelFormatAttribute.BackingStore:
				case CGLPixelFormatAttribute.Window:
				case CGLPixelFormatAttribute.MultiScreen:
				case CGLPixelFormatAttribute.Compliant:
				case CGLPixelFormatAttribute.PixelBuffer:

				// Not listed in the docs, but header file implies it
				case CGLPixelFormatAttribute.RemotePixelBuffer:
				case CGLPixelFormatAttribute.AuxDepthStencil:
				case CGLPixelFormatAttribute.ColorFloat:
				case CGLPixelFormatAttribute.Multisample:
				case CGLPixelFormatAttribute.Supersample:
				case CGLPixelFormatAttribute.SampleAlpha:
				case CGLPixelFormatAttribute.AllowOfflineRenderers:
				case CGLPixelFormatAttribute.AcceleratedCompute:
				case CGLPixelFormatAttribute.MPSafe:
					list.Add (v);
					break;

				case CGLPixelFormatAttribute.AuxBuffers:
				case CGLPixelFormatAttribute.ColorSize:
				case CGLPixelFormatAttribute.AlphaSize:
				case CGLPixelFormatAttribute.DepthSize:
				case CGLPixelFormatAttribute.StencilSize:
				case CGLPixelFormatAttribute.AccumSize:
				case CGLPixelFormatAttribute.RendererID:
				case CGLPixelFormatAttribute.ScreenMask:

				// not listed in the docs, but header file implies it
				case CGLPixelFormatAttribute.SampleBuffers:
				case CGLPixelFormatAttribute.Samples:
				case CGLPixelFormatAttribute.VirtualScreenCount:
					list.Add (v);
					i++;
					if (i >= args.Length)
						throw new ArgumentException ("Attribute " + v + " needs a value");
					CGLPixelFormatAttribute attr;
					object item = args [i];
					if (item is CGLPixelFormatAttribute) {
						attr = (CGLPixelFormatAttribute) item;
					} else {
						attr = (CGLPixelFormatAttribute) Convert.ChangeType (item, typeof (CGLPixelFormatAttribute).GetEnumUnderlyingType ());
					}
					list.Add (attr);

					break;
				}
			}
			return list.ToArray ();
		}
#endif // !COREBUILD

	}
}
