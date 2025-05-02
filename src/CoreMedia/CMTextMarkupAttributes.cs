// 
// CMTextMarkupAttributes.cs: Implements CMTextMarkup Attributes
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012-2014 Xamarin Inc
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
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CoreMedia {

	// Convenience structure
	/// <summary>A color to be used with <see cref="CoreMedia.CMTextMarkupAttributes.BackgroundColor" /> and <see cref="CoreMedia.CMTextMarkupAttributes.ForegroundColor" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct TextMarkupColor {
		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public TextMarkupColor (float red, float green, float blue, float alpha)
			: this ()
		{
			if (red < 0 || red > 1.0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (red), "Not between 0.0 and 1.0");
			if (green < 0 || green > 1.0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (green), "Not between 0.0 and 1.0");
			if (blue < 0 || blue > 1.0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (blue), "Not between 0.0 and 1.0");
			if (alpha < 0 || alpha > 1.0)
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (alpha), "Not between 0.0 and 1.0");

			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float Red { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float Green { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float Blue { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float Alpha { get; private set; }
	}

	/// <summary>Manages the attributes used by <see cref="AVFoundation.AVTextStyleRule" />.</summary>
	///     <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMTextMarkupAttributes : DictionaryContainer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTextMarkupAttributes ()
		{
		}

#if !COREBUILD
		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMTextMarkupAttributes (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>The foreground color for text.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_ForegroundColorARGB value to access the underlying dictionary.</remarks>
		public TextMarkupColor? ForegroundColor {
			get {
				var array = GetArray<NSNumber> (CMTextMarkupAttributesKeys.ForegroundColorARGB);
				if (array is null)
					return null;

				return new TextMarkupColor (array [1].FloatValue, array [2].FloatValue, array [3].FloatValue, array [0].FloatValue);
			}
			set {
				if (value is not null) {
					var v = value.Value;
					SetArrayValue (CMTextMarkupAttributesKeys.ForegroundColorARGB, new [] {
						NSNumber.FromFloat (v.Alpha),
						NSNumber.FromFloat (v.Red),
						NSNumber.FromFloat (v.Green),
						NSNumber.FromFloat (v.Blue),
					});
				} else {
					RemoveValue (CMTextMarkupAttributesKeys.ForegroundColorARGB);
				}
			}
		}

		/// <summary>The background color for the shape holding the text</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_BackgroundColorARGB value to access the underlying dictionary.</remarks>
		public TextMarkupColor? BackgroundColor {
			get {
				var array = GetArray<NSNumber> (CMTextMarkupAttributesKeys.BackgroundColorARGB);
				if (array is null)
					return null;

				return new TextMarkupColor (array [1].FloatValue, array [2].FloatValue, array [3].FloatValue, array [0].FloatValue);
			}
			set {
				if (value is not null) {
					var v = value.Value;
					SetArrayValue (CMTextMarkupAttributesKeys.BackgroundColorARGB, new [] {
						NSNumber.FromFloat (v.Alpha),
						NSNumber.FromFloat (v.Red),
						NSNumber.FromFloat (v.Green),
						NSNumber.FromFloat (v.Blue),
					});
				} else {
					RemoveValue (CMTextMarkupAttributesKeys.BackgroundColorARGB);
				}
			}
		}

		/// <summary>Causes content to render with a bold font weight.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_BoldStyle value to access the underlying dictionary.</remarks>
		public bool? Bold {
			get {
				return GetBoolValue (CMTextMarkupAttributesKeys.BoldStyle);
			}
			set {
				SetBooleanValue (CMTextMarkupAttributesKeys.BoldStyle, value);
			}
		}

		/// <summary>Causes content to render with an italic font style.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_ItalicStyle value to access the underlying dictionary.</remarks>
		public bool? Italic {
			get {
				return GetBoolValue (CMTextMarkupAttributesKeys.ItalicStyle);
			}
			set {
				SetBooleanValue (CMTextMarkupAttributesKeys.ItalicStyle, value);
			}
		}

		/// <summary>Indicates whether an underline will be applied.</summary>
		///         <value>
		///         </value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_UnderlineStyle value to access the underlying dictionary.</remarks>
		public bool? Underline {
			get {
				return GetBoolValue (CMTextMarkupAttributesKeys.UnderlineStyle);
			}
			set {
				SetBooleanValue (CMTextMarkupAttributesKeys.UnderlineStyle, value);
			}
		}

		/// <summary>The name of the font.</summary>
		///         <value>Value must be the family name of an installed font that is used to render and/or measure text.</value>
		///         <remarks>The property uses constant kCMTextMarkupAttribute_FontFamilyName value to access the underlying dictionary.</remarks>
		public string? FontFamilyName {
			get {
				return GetStringValue (CMTextMarkupAttributesKeys.FontFamilyName);
			}
			set {
				SetStringValue (CMTextMarkupAttributesKeys.FontFamilyName, value);
			}
		}

		/// <summary>The font size expressed as a percentage of the current default font size.</summary>
		///         <value>Value must be positive.</value>
		///         <remarks>
		///           <para>The property uses constant kCMTextMarkupAttribute_RelativeFontSize value to access the underlying dictionary.</para>
		///           <para>This is a number holding a percentage of the size of the calculated default font size. The value 100 indicates no size
		///                 difference and is the default.</para>
		///         </remarks>
		public int? RelativeFontSize {
			get {
				return GetInt32Value (CMTextMarkupAttributesKeys.RelativeFontSize);
			}
			set {
				if (value < 0)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Negative");

				SetNumberValue (CMTextMarkupAttributesKeys.RelativeFontSize, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? BaseFontSizePercentageRelativeToVideoHeight {
			get {
				return GetFloatValue (CMTextMarkupAttributesKeys.BaseFontSizePercentageRelativeToVideoHeight);
			}
			set {
				if (value < 0)
					ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (value), "Negative");

				SetNumberValue (CMTextMarkupAttributesKeys.BaseFontSizePercentageRelativeToVideoHeight, value);
			}
		}
#endif
	}
}
