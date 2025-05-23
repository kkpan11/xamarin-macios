// 
// CTGlyphInfo.cs: Implements the managed CTGlyphInfo
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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

using ObjCRuntime;
using Foundation;
using CoreFoundation;

using CGGlyph = System.UInt16;
using CGFontIndex = System.UInt16;

namespace CoreText {

	#region Glyph Info Values
	/// <summary>A class whose static fields specify character collections.</summary>
	///     <remarks>To be added.</remarks>
	public enum CTCharacterCollection : ushort {
		/// <summary>The character identifier is the same as the glyph index.</summary>
		IdentityMapping = 0,
		/// <summary>The Adobe-CNS1 character collection.</summary>
		AdobeCNS1 = 1,
		/// <summary>The Adobe-GB1 character collection.</summary>
		AdobeGB1 = 2,
		/// <summary>The Adobe-Japan1 character collection.</summary>
		AdobeJapan1 = 3,
		/// <summary>The Adobe-Japan2 character collection.</summary>
		AdobeJapan2 = 4,
		/// <summary>The Adobe-Korea1 mapping.</summary>
		AdobeKorea1 = 5,
	}
	#endregion

	/// <summary>Provides the ability to override the Unicode-to-glyph mapping for a <see cref="CoreText.CTFont" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTGlyphInfo : NativeObject {
		[Preserve (Conditional = true)]
		internal CTGlyphInfo (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		#region Glyph Info Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTGlyphInfoCreateWithGlyphName (IntPtr glyphName, IntPtr font, IntPtr baseString);

		static IntPtr Create (string glyphName, CTFont font, string baseString)
		{
			if (glyphName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (glyphName));
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			if (baseString is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (baseString));

			var gnHandle = CFString.CreateNative (glyphName);
			var bsHandle = CFString.CreateNative (baseString);
			try {
				IntPtr result = CTGlyphInfoCreateWithGlyphName (gnHandle, font.Handle, bsHandle);
				GC.KeepAlive (font);
				return result;
			} finally {
				CFString.ReleaseNative (gnHandle);
				CFString.ReleaseNative (bsHandle);
			}
		}

		/// <param name="glyphName">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="baseString">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CTGlyphInfo (string glyphName, CTFont font, string baseString)
			: base (Create (glyphName, font, baseString), true, verify: true)
		{
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTGlyphInfoCreateWithGlyph (CGGlyph glyph, IntPtr font, IntPtr baseString);

		static IntPtr Create (CGGlyph glyph, CTFont font, string baseString)
		{
			if (font is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (font));
			if (baseString is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (baseString));

			var bsHandle = CFString.CreateNative (baseString);
			try {
				IntPtr result = CTGlyphInfoCreateWithGlyph (glyph, font.Handle, bsHandle);
				GC.KeepAlive (font);
				return result;
			} finally {
				CFString.ReleaseNative (bsHandle);
			}
		}

		/// <param name="glyph">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="baseString">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CTGlyphInfo (CGGlyph glyph, CTFont font, string baseString)
			: base (Create (glyph, font, baseString), true, verify: true)
		{
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTGlyphInfoCreateWithCharacterIdentifier (CGFontIndex cid, CTCharacterCollection collection, IntPtr baseString);

		static IntPtr Create (CGFontIndex cid, CTCharacterCollection collection, string baseString)
		{
			if (baseString is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (baseString));

			var bsHandle = CFString.CreateNative (baseString);
			try {
				return CTGlyphInfoCreateWithCharacterIdentifier (cid, collection, bsHandle);
			} finally {
				CFString.ReleaseNative (bsHandle);
			}
		}

		/// <param name="cid">To be added.</param>
		///         <param name="collection">To be added.</param>
		///         <param name="baseString">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CTGlyphInfo (CGFontIndex cid, CTCharacterCollection collection, string baseString)
			: base (Create (cid, collection, baseString), true, true)
		{
		}
		#endregion

		#region Glyph Info Access
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTGlyphInfoGetGlyphName (IntPtr glyphInfo);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? GlyphName {
			get {
				var cfStringRef = CTGlyphInfoGetGlyphName (Handle);
				return CFString.FromHandle (cfStringRef);
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern CGFontIndex CTGlyphInfoGetCharacterIdentifier (IntPtr glyphInfo);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CGFontIndex CharacterIdentifier {
			get { return CTGlyphInfoGetCharacterIdentifier (Handle); }
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern CTCharacterCollection CTGlyphInfoGetCharacterCollection (IntPtr glyphInfo);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CTCharacterCollection CharacterCollection {
			get { return CTGlyphInfoGetCharacterCollection (Handle); }
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern ushort /* CGGlyph */ CTGlyphInfoGetGlyph (IntPtr /* CTGlyphInfoRef */ glyphInfo);

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CGGlyph GetGlyph ()
		{
			return CTGlyphInfoGetGlyph (Handle);
		}
		#endregion

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string? ToString ()
		{
			return GlyphName;
		}
	}
}
