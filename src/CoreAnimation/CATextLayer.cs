// 
// CATextLayer: Support for CATextLayer
//
// Authors:
//   Miguel de Icaza.
//     
// Copyright 2010 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc
//
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

using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreFoundation;
using CoreText;
#if MONOMAC
using AppKit;
#endif

#nullable enable

namespace CoreAnimation {

	public partial class CATextLayer {

		/// <summary>Sets the attributed string to display.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    This sets the attributed string to display on the layer.   The attributed string should contain CoreText attributes.
		/// 	  </para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// void DrawHelloWorld (CATextLayer myTextLayer)
		/// {
		///     var hello = new NSAttributedString ("Hello, world",
		///            new CTStringAttributes () {
		///                   ForegroundColorFromContext =  true,
		///                   Font = new CTFont ("Arial", 24)
		///            });
		///
		///     myTextLayer.AttributedString = hello;
		/// }    
		/// ]]></code>
		///           </example>
		///         </remarks>
		public NSAttributedString? AttributedString {
			get {
				return Runtime.GetNSObject (_AttributedString) as NSAttributedString;
			}
			set {
				_AttributedString = value.GetHandle ();
				GC.KeepAlive (value);
			}
		}

		/// <param name="fontName">To be added.</param>
		///         <summary>Sets the font.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFont (string fontName)
		{
			if (fontName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fontName));
			using (var nss = new NSString (fontName)) {
				_Font = nss.Handle;
			}
		}

		/// <param name="font">To be added.</param>
		///         <summary>Sets the font.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFont (CGFont font)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			_Font = font.Handle;
			GC.KeepAlive (font);
		}

		/// <param name="font">To be added.</param>
		///         <summary>Sets the font.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFont (CTFont font)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			_Font = font.Handle;
			GC.KeepAlive (font);
		}

#if MONOMAC
		/// <param name="font">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetFont (NSFont font)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			_Font = font.Handle;
			GC.KeepAlive (font);
		}
#endif

		/// <summary>Gets or sets a weak reference to the font.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public object? WeakFont {
			get {
				var handle = _Font;
				nint type = CFType.GetTypeID (handle);
				if (type == CTFont.GetTypeID ())
					return new CTFont (handle, false);
				else if (type == CGFont.GetTypeID ())
					return new CGFont (handle, false);
				else if (type == CFString.GetTypeID ())
					return CFString.FromHandle (handle);
#if MONOMAC
				else return Runtime.GetNSObject<NSFont> (handle);
#else
				return null;
#endif
			}

			// Allows CTFont, CGFont, string and in OSX NSFont settings
			set {
#if MONOMAC
				var ns = value as NSFont;
				if (ns is not null) {
					_Font = ns.Handle;
					GC.KeepAlive (ns);
					return;
				}
#endif
				var ct = value as CTFont;
				if (ct is not null) {
					_Font = ct.Handle;
					GC.KeepAlive (ct);
					return;
				}
				var cg = value as CGFont;
				if (cg is not null) {
					_Font = cg.Handle;
					GC.KeepAlive (cg);
					return;
				}
				var nss = value as NSString;
				if (nss is not null) {
					_Font = nss.Handle;
					GC.KeepAlive (nss);
					return;
				}
				var str = value as string;
				if (str is not null) {
					nss = new NSString (str);
					_Font = nss.Handle;
					GC.KeepAlive (nss);
				}
			}
		}

		/// <summary>Gets or sets a value that controls how text will be truncated, if necessary, for display.</summary>
		public CATextLayerTruncationMode TextTruncationMode {
			get { return CATextLayerTruncationModeExtensions.GetValue (WeakTruncationMode); }
			set { WeakTruncationMode = value.GetConstant ()!; }
		}

		/// <summary>Gets or sets the text alignment mode.</summary>
		public CATextLayerAlignmentMode TextAlignmentMode {
			get { return CATextLayerAlignmentModeExtensions.GetValue (WeakAlignmentMode); }
			set { WeakAlignmentMode = value.GetConstant ()!; }
		}
	}
}
