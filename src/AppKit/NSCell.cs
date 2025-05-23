//
// Author:
//       Martin Baulig <martin.baulig@xamarin.com>
//
// Copyright 2010, Novell, Inc.
// Copyright (c) 2012 Xamarin Inc. (http://www.xamarin.com)
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

#if !__MACCATALYST__

using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace AppKit {
	public partial class NSCell {

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSDrawThreePartImage (CGRect rect,
			IntPtr /* NSImage* */ startCap, IntPtr /* NSImage* */ centerFill, IntPtr /* NSImage* */ endCap,
			byte vertial, nint op, nfloat alphaFraction, byte flipped);

		/// <param name="frame">To be added.</param>
		/// <param name="startCap">To be added.</param>
		/// <param name="centerFill">To be added.</param>
		/// <param name="endCap">To be added.</param>
		/// <param name="vertical">To be added.</param>
		/// <param name="op">To be added.</param>
		/// <param name="alphaFraction">To be added.</param>
		/// <param name="flipped">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void DrawThreePartImage (CGRect frame,
			NSImage startCap, NSImage centerFill, NSImage endCap,
			bool vertical, NSCompositingOperation op, nfloat alphaFraction, bool flipped)
		{
			NSDrawThreePartImage (
				frame, startCap.GetHandle (),
				centerFill.GetHandle (),
				endCap.GetHandle (),
				vertical ? (byte) 1 : (byte) 0,
				(nint) (long) op,
				alphaFraction,
				flipped ? (byte) 1 : (byte) 0);
			GC.KeepAlive (startCap);
			GC.KeepAlive (centerFill);
			GC.KeepAlive (endCap);
		}

		[DllImport (Constants.AppKitLibrary)]
		extern static void NSDrawNinePartImage (CGRect frame,
			IntPtr /* NSImage* */ topLeftCorner, IntPtr /* NSImage* */ topEdgeFill, IntPtr /* NSImage* */ topRightCorner,
			IntPtr /* NSImage* */ leftEdgeFill, IntPtr /* NSImage* */ centerFill, IntPtr /* NSImage* */ rightEdgeFill,
			IntPtr /* NSImage* */ bottomLeftCorner, IntPtr /* NSImage* */ bottomEdgeFill, IntPtr /* NSImage* */ bottomRightCnint,
			nint op, nfloat alphaFraction, byte flipped);

		/// <param name="frame">To be added.</param>
		/// <param name="topLeftCorner">To be added.</param>
		/// <param name="topEdgeFill">To be added.</param>
		/// <param name="topRightCorner">To be added.</param>
		/// <param name="leftEdgeFill">To be added.</param>
		/// <param name="centerFill">To be added.</param>
		/// <param name="rightEdgeFill">To be added.</param>
		/// <param name="bottomLeftCorner">To be added.</param>
		/// <param name="bottomEdgeFill">To be added.</param>
		/// <param name="bottomRightCorner">To be added.</param>
		/// <param name="op">To be added.</param>
		/// <param name="alphaFraction">To be added.</param>
		/// <param name="flipped">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void DrawNinePartImage (CGRect frame,
			NSImage topLeftCorner, NSImage topEdgeFill, NSImage topRightCorner,
			NSImage leftEdgeFill, NSImage centerFill, NSImage rightEdgeFill,
			NSImage bottomLeftCorner, NSImage bottomEdgeFill, NSImage bottomRightCorner,
			NSCompositingOperation op, nfloat alphaFraction, bool flipped)
		{
			NSDrawNinePartImage (
				frame, topLeftCorner.GetHandle (),
				topEdgeFill.GetHandle (),
				topRightCorner.GetHandle (),
				leftEdgeFill.GetHandle (),
				centerFill.GetHandle (),
				rightEdgeFill.GetHandle (),
				bottomLeftCorner.GetHandle (),
				bottomEdgeFill.GetHandle (),
				bottomRightCorner.GetHandle (),
				(nint) (long) op, alphaFraction, flipped ? (byte) 1 : (byte) 0);
			GC.KeepAlive (topLeftCorner);
			GC.KeepAlive (topEdgeFill);
			GC.KeepAlive (topRightCorner);
			GC.KeepAlive (leftEdgeFill);
			GC.KeepAlive (centerFill);
			GC.KeepAlive (rightEdgeFill);
			GC.KeepAlive (bottomLeftCorner);
			GC.KeepAlive (bottomEdgeFill);
			GC.KeepAlive (bottomRightCorner);
		}
	}
}
#endif // !__MACCATALYST__
