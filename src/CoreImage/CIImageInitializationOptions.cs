// 
// CIImageInitializationOptions.cs: Implements settings for CIImage initialization
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, Xamarin Inc.
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
using System.Runtime.Versioning;

using Foundation;
using CoreFoundation;
using ObjCRuntime;
using CoreGraphics;

#nullable enable

namespace CoreImage {

	public partial class CIImageInitializationOptions {
#if !COREBUILD
		/// <summary>Gets or sets the color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGColorSpace? ColorSpace {
			get {
				return GetNativeValue<CGColorSpace> (CIImageInitializationOptionsKeys.ColorSpaceKey);
			}
			set {
				SetNativeValue (CIImageInitializationOptionsKeys.ColorSpaceKey, value);
			}
		}
#endif
	}

	/// <summary>A type of <see cref="CoreImage.CIImageInitializationOptions" /> that has additional metadata properties.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// Keeping 'CIImageInitializationOptionsWithMetadata' to avoid breaking change
	public class CIImageInitializationOptionsWithMetadata : CIImageInitializationOptions {
#if !COREBUILD
		/// <summary>Creates a new CIImageInitializationOptionsWithMetadata with default values.</summary>
		///         <remarks>To be added.</remarks>
		public CIImageInitializationOptionsWithMetadata ()
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>Creates a new CIImageInitializationOptionsWithMetadata by using the specified dictionary of options.</summary>
		///         <remarks>To be added.</remarks>
		public CIImageInitializationOptionsWithMetadata (NSDictionary dictionary)
			: base (dictionary)
		{
		}
#endif
	}
}
