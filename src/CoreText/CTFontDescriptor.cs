// 
// CTFontDescriptor.cs: Implements the managed CTFontDescriptor
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//          Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2010 Novell, Inc
// Copyright 2011 - 2014 Xamarin Inc
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using CoreGraphics;
using Foundation;

namespace CoreText {

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontDescriptor.h
	/// <summary>An enumeration whose values specify the orientation of a <see cref="CoreText.CTFont" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum CTFontOrientation : uint {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Horizontal = 1,
		/// <summary>To be added.</summary>
		Vertical = 2,
	}

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontDescriptor.h
	/// <summary>Font format.</summary>
	///     <remarks>
	///     </remarks>
	public enum CTFontFormat : uint {
		/// <summary>An unrecognized font format.</summary>
		Unrecognized = 0,
		/// <summary>OpenType PostScript font.</summary>
		OpenTypePostScript = 1,
		/// <summary>OpenType TrueType font.</summary>
		OpenTypeTrueType = 2,
		/// <summary>TrueType font.</summary>
		TrueType = 3,
		/// <summary>PostScript font.</summary>
		PostScript = 4,
		/// <summary>Bitmap font.</summary>
		Bitmap = 5,
	}

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontDescriptor.h
	/// <summary>An enumeration whose values specify the priority of a <see cref="CoreText.CTFont" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum CTFontPriority : uint {
		/// <summary>To be added.</summary>
		System = 10000,
		/// <summary>To be added.</summary>
		Network = 20000,
		/// <summary>To be added.</summary>
		Computer = 30000,
		/// <summary>To be added.</summary>
		User = 40000,
		/// <summary>To be added.</summary>
		Dynamic = 50000,
		/// <summary>To be added.</summary>
		Process = 60000,
	}

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontDescriptor.h
	/// <summary>An enumeration whose values can be used as parameters for the <see cref="CoreText.CTFontDescriptor.MatchFontDescriptors(CoreText.CTFontDescriptor[],Foundation.NSSet,System.Func{CoreText.CTFontDescriptorMatchingState,System.IntPtr,System.Boolean})" /> method.</summary>
	///     <remarks>To be added.</remarks>
	public enum CTFontDescriptorMatchingState : uint {
		/// <summary>To be added.</summary>
		Started,
		/// <summary>To be added.</summary>
		Finished,
		/// <summary>To be added.</summary>
		WillBeginQuerying,
		/// <summary>To be added.</summary>
		Stalled,
		/// <summary>To be added.</summary>
		WillBeginDownloading,
		/// <summary>To be added.</summary>
		Downloading,
		/// <summary>To be added.</summary>
		DownloadingFinished,
		/// <summary>To be added.</summary>
		Matched,
		/// <summary>To be added.</summary>
		FailedWithError,
	}

	/// <summary>Strongly typed class that contains font attributes.</summary>
	///     <remarks>
	///       <para>
	/// 	This is a class that allows developers to easily consume and configure font descriptor attributes.
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// var attributes = new CTFontDescriptorAttributes () {
	/// 	FamilyName = "Times New Roman"
	/// };
	///
	/// var fontDescriptor = new CTFontDescriptor (attributes);
	/// 	]]></code>
	///       </example>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTFontDescriptorAttributes {

		/// <summary>Default constructor.</summary>
		///         <remarks>
		///           <para>Typically used to initialize objects with the C# initializer syntax.</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var attributes = new CTFontDescriptorAttributes () {
		/// 	FamilyName = "Times New Roman"
		/// };
		///
		/// var fontDescriptor = new CTFontDescriptor (attributes);
		/// ]]></code>
		///           </example>
		///         </remarks>
		public CTFontDescriptorAttributes ()
			: this (new NSMutableDictionary ())
		{
		}

		/// <param name="dictionary">An NSDictionary containing CTFontDescriptorAttributes keys and values.</param>
		///         <summary>Creates a strongly typed CTFontDescriptorAttributes from a weakly typed NSDictionary.</summary>
		///         <remarks>
		///         </remarks>
		public CTFontDescriptorAttributes (NSDictionary dictionary)
		{
			if (dictionary is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dictionary));
			Dictionary = dictionary;
		}

		/// <summary>The NSDictionary that reflects the current values in the strongly typed CTFontDescriptorAttributes.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDictionary Dictionary { get; private set; }

		/// <summary>Used to access the URL for the font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSUrl? Url {
			get { return (NSUrl?) Dictionary [CTFontDescriptorAttributeKey.Url]; }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Url!, value); }
		}

		/// <summary>The PostScript name for the font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? Name {
			get { return Adapter.GetStringValue (Dictionary, CTFontDescriptorAttributeKey.Name); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Name!, value); }
		}

		/// <summary>The font display name</summary>
		///         <value>
		///         </value>
		///         <remarks>For example the display name for the PostScript name "HelveticaNeue" is "Helvetica Neue".</remarks>
		public string? DisplayName {
			get { return Adapter.GetStringValue (Dictionary, CTFontDescriptorAttributeKey.DisplayName); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.DisplayName!, value); }
		}

		/// <summary>The font family name.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var attributes = new CTFontDescriptorAttributes () {
		/// 	FamilyName = "Times New Roman"
		/// };
		///
		/// var fontDescriptor = new CTFontDescriptor (attributes);
		/// 	]]></code>
		///           </example>
		///         </remarks>
		public string? FamilyName {
			get { return Adapter.GetStringValue (Dictionary, CTFontDescriptorAttributeKey.FamilyName); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.FamilyName!, value); }
		}

		/// <summary>The font stylename</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? StyleName {
			get { return Adapter.GetStringValue (Dictionary, CTFontDescriptorAttributeKey.StyleName); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.StyleName!, value); }
		}

		/// <summary>The font traits.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontTraits? Traits {
			get {
				if (CTFontDescriptorAttributeKey.Traits is NSString traitsKey && Dictionary [traitsKey] is NSDictionary traits)
					return new CTFontTraits (traits);
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Traits!, value?.Dictionary);
			}
		}

		/// <summary>Font variation.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontVariation? Variation {
			get {
				if (CTFontDescriptorAttributeKey.Variation is NSString variationKey && Dictionary [variationKey] is NSDictionary variation)
					return new CTFontVariation (variation);
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Variation!, value?.Dictionary);
			}
		}

		// CFNumber
		/// <summary>The font point size, if not specified, the default 12.0 is used.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public float? Size {
			get { return Adapter.GetSingleValue (Dictionary, CTFontDescriptorAttributeKey.Size); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Size!, value); }
		}

		/// <summary>If set, the transformation matrix used when creating the font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public unsafe CGAffineTransform? Matrix {
			get {
				if (CTFontDescriptorAttributeKey.Matrix is NSString matrixKey && Dictionary [matrixKey] is NSData d)
					return Marshal.PtrToStructure<CGAffineTransform> (d.Bytes);
				return null;
			}
			set {
				if (CTFontDescriptorAttributeKey.Matrix is null)
					throw new ArgumentOutOfRangeException (nameof (CTFontDescriptorAttributeKey.Matrix));
				if (!value.HasValue)
					Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Matrix!, (NSObject?) null);
				else {
					byte [] data = new byte [Marshal.SizeOf<CGAffineTransform> ()];
					fixed (byte* p = data) {
						Marshal.StructureToPtr<CGAffineTransform> (value.Value, (IntPtr) p, false);
					}
					Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Matrix!, NSData.FromArray (data));
				}
			}
		}

		/// <summary>Cascade list of font descriptors.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public IEnumerable<CTFontDescriptor>? CascadeList {
			get {
				if (CTFontDescriptorAttributeKey.CascadeList is NSString cascadeList)
					return Adapter.GetNativeArray (Dictionary, cascadeList, d => new CTFontDescriptor (d, false));
				return null;
			}
			set { Adapter.SetNativeValue (Dictionary, CTFontDescriptorAttributeKey.CascadeList!, value); }
		}

		/// <summary>The character set.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSCharacterSet? CharacterSet {
			get { return (NSCharacterSet) Dictionary [CTFontDescriptorAttributeKey.CharacterSet]; }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.CharacterSet!, value); }
		}

		/// <summary>List of supported languages by the font, must be encoded in RFC3066bis standard.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public IEnumerable<string>? Languages {
			get { return Adapter.GetStringArray (Dictionary, CTFontDescriptorAttributeKey.Languages); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Languages!, value); }
		}

		// float represented as a CFNumber
		/// <summary>The Baseline Adjustment.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public float? BaselineAdjust {
			get { return Adapter.GetSingleValue (Dictionary, CTFontDescriptorAttributeKey.BaselineAdjust); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.BaselineAdjust!, value); }
		}

		/// <summary>If set, the Macintosh encodings for this font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public float? MacintoshEncodings {
			get { return Adapter.GetSingleValue (Dictionary, CTFontDescriptorAttributeKey.MacintoshEncodings); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.MacintoshEncodings!, value); }
		}

		/// <summary>The font features for a font reference.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public IEnumerable<CTFontFeatures>? Features {
			get {
				if (CTFontDescriptorAttributeKey.Features is NSString features) {
					return Adapter.GetNativeArray (Dictionary, features,
						d => new CTFontFeatures ((NSDictionary) Runtime.GetNSObject (d)!));
				}
				return null;
			}
			set {
				if (CTFontDescriptorAttributeKey.Features is null)
					throw new ArgumentOutOfRangeException (nameof (CTFontDescriptorAttributeKey.Features));

				List<CTFontFeatures> v;
				if (value is null || (v = new List<CTFontFeatures> (value)).Count == 0) {
					Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Features!, (NSObject?) null);
					return;
				}
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Features!,
						NSArray.FromNSObjects ((IList<NSObject>) v.ConvertAll (e => (NSObject) e.Dictionary)));
			}
		}

		/// <summary>Feature settings for the font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public IEnumerable<CTFontFeatureSettings>? FeatureSettings {
			get {
				if (CTFontDescriptorAttributeKey.Features is NSString features) {
					return Adapter.GetNativeArray (Dictionary, CTFontDescriptorAttributeKey.Features,
						d => new CTFontFeatureSettings ((NSDictionary) Runtime.GetNSObject (d)!));
				}
				return null;
			}
			set {
				if (CTFontDescriptorAttributeKey.Features is null)
					throw new ArgumentOutOfRangeException (nameof (CTFontDescriptorAttributeKey.Features));
				List<CTFontFeatureSettings> v;
				if (value is null || (v = new List<CTFontFeatureSettings> (value)).Count == 0) {
					Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Features!, (NSObject?) null);
					return;
				}

				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.FeatureSettings!,
						NSArray.FromNSObjects ((IList<NSObject>) v.ConvertAll (e => (NSObject) e.Dictionary)));
			}
		}

		// CFNumber
		/// <summary>If set, overrides the font built-in constant advance.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public float? FixedAdvance {
			get { return Adapter.GetSingleValue (Dictionary, CTFontDescriptorAttributeKey.FixedAdvance); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.FixedAdvance!, value); }
		}

		/// <summary>The font orientation.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontOrientation? FontOrientation {
			get {
				if (CTFontDescriptorAttributeKey.FontOrientation is NSString fontOrientation) {
					var value = Adapter.GetUInt32Value (Dictionary, fontOrientation);
					return !value.HasValue ? null : (CTFontOrientation?) value.Value;
				}
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.FontOrientation!,
						value.HasValue ? (uint?) value.Value : null);
			}
		}

		/// <summary>Font format.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontFormat? FontFormat {
			get {
				if (CTFontDescriptorAttributeKey.FontFormat is NSString fontFormat) {
					var value = Adapter.GetUInt32Value (Dictionary, fontFormat);
					return !value.HasValue ? null : (CTFontFormat?) value.Value;
				}
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.FontFormat!,
						value.HasValue ? (uint?) value.Value : null);
			}
		}

		/// <summary>The font registration scope.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontManagerScope? RegistrationScope {
			get {
				if (CTFontDescriptorAttributeKey.RegistrationScope is NSString registrationScope) {
					var value = Adapter.GetUnsignedIntegerValue (Dictionary, registrationScope);
					return !value.HasValue ? null : (CTFontManagerScope?) (ulong) value.Value;
				}
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.RegistrationScope!,
				value.HasValue ? (nuint?) (ulong) value.Value : null);
			}
		}

		/// <summary>The font priority used when resolving font duplicates.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CTFontPriority? Priority {
			get {
				if (CTFontDescriptorAttributeKey.Priority is NSString priority) {
					var value = Adapter.GetUInt32Value (Dictionary, priority);
					return !value.HasValue ? null : (CTFontPriority?) value.Value;
				}
				return null;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Priority!,
						value.HasValue ? (uint?) value.Value : null);
			}
		}

#if XAMCORE_5_0
		public bool? Enabled {
#else
		public bool? WeakEnabled {
#endif
			get {
				var value = (NSNumber?) Dictionary [CTFontDescriptorAttributeKey.Enabled];
				if (value is null)
					return null;
				return value.Int32Value != 0;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Enabled!, value is null ? null : new NSNumber ((value.Value ? 1 : 0)));
			}
		}

#if !XAMCORE_5_0
		/// <summary>Enabled state.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public bool Enabled {
			get {
				var value = (NSNumber) Dictionary [CTFontDescriptorAttributeKey.Enabled];
				if (value is null)
					return false;
				return value.Int32Value != 0;
			}
			set {
				Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.Enabled!, value ? new NSNumber (1) : null);
			}
		}
#endif // !XAMCORE_5_0

#if __IOS__ || __MACCATALYST__
		[SupportedOSPlatform ("ios13.0")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public string? RegistrationUserInfo {
			get { return Adapter.GetStringValue (Dictionary, CTFontDescriptorAttributeKey.RegistrationUserInfo); }
			set { Adapter.SetValue (Dictionary, CTFontDescriptorAttributeKey.RegistrationUserInfo!, value); }
		}
#endif
	}

	/// <summary>Font Descriptors contain a description of font features that can identify a font.</summary>
	///     <remarks>
	///       <para>
	/// 	Font Descriptors contain a description of font features and can
	/// 	completely identify a font.  Sometimes the description is not
	/// 	complete enough, and the system will pick a font that matches
	/// 	the specified parameters.
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// var attributes = new CTFontDescriptorAttributes () {
	/// 	FamilyName = "Times New Roman",
	/// 	Size = 18
	/// };
	///
	/// var fontDescriptor = new CTFontDescriptor (attributes);
	/// ]]></code>
	///       </example>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CTFontDescriptor : NativeObject {
		[Preserve (Conditional = true)]
		internal CTFontDescriptor (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		#region Descriptor Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateWithNameAndSize (IntPtr name, nfloat size);
		static IntPtr Create (string name, nfloat size)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));
			var nameHandle = CFString.CreateNative (name);
			try {
				return CTFontDescriptorCreateWithNameAndSize (nameHandle, size);
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		public CTFontDescriptor (string name, nfloat size)
			: base (Create (name, size), true, true)
		{
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateWithAttributes (IntPtr attributes);
		static IntPtr Create (CTFontDescriptorAttributes attributes)
		{
			if (attributes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (attributes));
			var dict = attributes.Dictionary;
			IntPtr result = CTFontDescriptorCreateWithAttributes (dict.Handle);
			GC.KeepAlive (dict);
			return result;
		}

		/// <param name="attributes">Font attributes to use for the font descriptor.</param>
		///         <summary>Creates a font descriptor from a set of attributes.</summary>
		///         <remarks>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var attributes = new CTFontDescriptorAttributes () {
		/// 	FamilyName = "Times New Roman"
		/// };
		///
		/// var fontDescriptor = new CTFontDescriptor (attributes);
		/// ]]></code>
		///           </example>
		///         </remarks>
		public CTFontDescriptor (CTFontDescriptorAttributes attributes)
			: base (Create (attributes), true, true)
		{
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateCopyWithAttributes (IntPtr original, IntPtr attributes);
		/// <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? WithAttributes (NSDictionary attributes)
		{
			if (attributes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (attributes));
			CTFontDescriptor? descriptor = CreateDescriptor (CTFontDescriptorCreateCopyWithAttributes (Handle, attributes.Handle));
			GC.KeepAlive (attributes);
			return descriptor;
		}

		static CTFontDescriptor? CreateDescriptor (IntPtr h)
		{
			if (h == IntPtr.Zero)
				return null;
			return new CTFontDescriptor (h, true);
		}

		/// <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[
		/// var attributes = new CTFontDescriptorAttributes () {
		/// 	FamilyName = "Times New Roman"
		/// };
		///
		/// var newFontDescriptor = myDescriptor.WithAttributes (attributes);
		/// 	]]></code>
		///           </example>
		///         </remarks>
		public CTFontDescriptor? WithAttributes (CTFontDescriptorAttributes attributes)
		{
			if (attributes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (attributes));
			var attributesDictionary = attributes.Dictionary;
			CTFontDescriptor? descriptor = CreateDescriptor (CTFontDescriptorCreateCopyWithAttributes (Handle, attributesDictionary.Handle));
			GC.KeepAlive (attributesDictionary);
			return descriptor;
		}

		// TODO: is there a better type to use for variationIdentifier?  
		// uint perhaps?  "This is the four character code of the variation axis"
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateCopyWithVariation (IntPtr original, IntPtr variationIdentifier, nfloat variationValue);
		public CTFontDescriptor? WithVariation (uint variationIdentifier, nfloat variationValue)
		{
			using (var id = new NSNumber (variationIdentifier))
				return CreateDescriptor (CTFontDescriptorCreateCopyWithVariation (Handle,
							id.Handle, variationValue));
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateCopyWithFeature (IntPtr original, IntPtr featureTypeIdentifier, IntPtr featureSelectorIdentifier);

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureAllTypographicFeatures.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.AllTypographicFeatures, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureLigatures.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.Ligatures, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCursiveConnection.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CursiveConnection, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureVerticalSubstitutionConnection.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.VerticalSubstitution, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureLinguisticRearrangementConnection.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.LinguisticRearrangement, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureNumberSpacing.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.NumberSpacing, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureSmartSwash.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.SmartSwash, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureDiacritics.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.Diacritics, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureVerticalPosition.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.VerticalPosition, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureFractions.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.Fractions, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureOverlappingCharacters.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.OverlappingCharacters, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureTypographicExtras.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.TypographicExtras, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureMathematicalExtras.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.MathematicalExtras, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureOrnamentSets.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.OrnamentSets, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCharacterAlternatives.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CharacterAlternatives, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureDesignComplexity.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.DesignComplexity, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureStyleOptions.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.StyleOptions, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCharacterShape.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CharacterShape, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureNumberCase.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.NumberCase, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureTextSpacing.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.TextSpacing, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureTransliteration.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.Transliteration, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureAnnotation.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.Annotation, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureKanaSpacing.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.KanaSpacing, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureIdeographicSpacing.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.IdeographicSpacing, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureUnicodeDecomposition.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.UnicodeDecomposition, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureRubyKana.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.RubyKana, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCJKSymbolAlternatives.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CJKSymbolAlternatives, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureIdeographicAlternatives.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.IdeographicAlternatives, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCJKVerticalRomanPlacement.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CJKVerticalRomanPlacement, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureItalicCJKRoman.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.ItalicCJKRoman, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCaseSensitiveLayout.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CaseSensitiveLayout, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureAlternateKana.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.AlternateKana, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureStylisticAlternatives.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.StylisticAlternatives, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureContextualAlternates.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.ContextualAlternates, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureLowerCase.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.LowerCase, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureUpperCase.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.UpperCase, (int) featureSelector);
		}

		/// <param name="featureSelector">the feature to create.</param>
		///         <summary>Creates a font descriptor from this font descriptor, with the specified feature set.</summary>
		///         <returns>A new CTFontDescriptor representing the specified feature.</returns>
		///         <remarks>This is a convenience method that creates new CTFontDescriptors with a single feature altered.</remarks>
		public CTFontDescriptor? WithFeature (CTFontFeatureCJKRomanSpacing.Selector featureSelector)
		{
			return WithFeature (FontFeatureGroup.CJKRomanSpacing, (int) featureSelector);
		}

		CTFontDescriptor? WithFeature (FontFeatureGroup featureGroup, int featureSelector)
		{
			using (NSNumber t = new NSNumber ((int) featureGroup), f = new NSNumber (featureSelector)) {
				CTFontDescriptor? result = CreateDescriptor (CTFontDescriptorCreateCopyWithFeature (Handle, t.Handle, f.Handle));
				return result;
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateMatchingFontDescriptors (IntPtr descriptor, IntPtr mandatoryAttributes);
		/// <param name="mandatoryAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor [] GetMatchingFontDescriptors (NSSet? mandatoryAttributes)
		{
			var cfArrayRef = CTFontDescriptorCreateMatchingFontDescriptors (Handle, mandatoryAttributes.GetHandle ());
			GC.KeepAlive (mandatoryAttributes);
			if (cfArrayRef == IntPtr.Zero)
				return Array.Empty<CTFontDescriptor> ();
			return CFArray.ArrayFromHandleFunc (cfArrayRef, fd => new CTFontDescriptor (cfArrayRef, false), true)!;
		}

		/// <param name="mandatoryAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? []? GetMatchingFontDescriptors (params NSString [] mandatoryAttributes)
		{
			NSSet attrs = NSSet.MakeNSObjectSet (mandatoryAttributes);
			return GetMatchingFontDescriptors (attrs);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? []? GetMatchingFontDescriptors ()
		{
			NSSet? attrs = null;
			return GetMatchingFontDescriptors (attrs);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCreateMatchingFontDescriptor (IntPtr descriptor, IntPtr mandatoryAttributes);
		/// <param name="mandatoryAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? GetMatchingFontDescriptor (NSSet? mandatoryAttributes)
		{
			CTFontDescriptor? result = CreateDescriptor (CTFontDescriptorCreateMatchingFontDescriptors (Handle, mandatoryAttributes.GetHandle ()));
			GC.KeepAlive (mandatoryAttributes);
			return result;
		}

		/// <param name="mandatoryAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? GetMatchingFontDescriptor (params NSString [] mandatoryAttributes)
		{
			NSSet attrs = NSSet.MakeNSObjectSet (mandatoryAttributes);
			return GetMatchingFontDescriptor (attrs);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFontDescriptor? GetMatchingFontDescriptor ()
		{
			NSSet? attrs = null;
			return GetMatchingFontDescriptor (attrs);
		}
		#endregion

		#region Descriptor Accessors
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCopyAttributes (IntPtr descriptor);
		/// <summary>Retrieves the attributes from this CTFontDescriptor.</summary>
		///         <returns>Strongly typed CTFontDescriptorAttributes.</returns>
		///         <remarks>
		///         </remarks>
		public CTFontDescriptorAttributes? GetAttributes ()
		{
			var cfDictRef = CTFontDescriptorCopyAttributes (Handle);
			var dict = Runtime.GetNSObject<NSDictionary> (cfDictRef, true);
			if (dict is null)
				return null;
			return new CTFontDescriptorAttributes (dict);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFontDescriptorCopyAttribute (IntPtr descriptor, IntPtr attribute);
		/// <param name="attribute">An NSString representing a CTFontDescriptor attribute, one of the keys in <see cref="CoreText.CTFontDescriptorAttributeKey" />.</param>
		///         <summary>Fetches a CTFontDescriptorAttribute from the descriptor.</summary>
		///         <returns>The attribute as an NSObject.</returns>
		///         <remarks>You can use <see cref="CoreText.CTFontDescriptor.GetAttributes" /> method to get all the attributes at once with a strongly typed set of properties.</remarks>
		public NSObject? GetAttribute (NSString attribute)
		{
			if (attribute is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (attribute));
			NSObject? result = Runtime.GetNSObject<NSObject> (CTFontDescriptorCopyAttribute (Handle, attribute.Handle), true);
			GC.KeepAlive (attribute);
			return result;
		}

		/// <param name="attribute">An NSString representing a CTFontDescriptor attribute, one of the keys in <see cref="CoreText.CTFontDescriptorAttributeKey" />.</param>
		///         <summary>Returns an attribute that has been localized.</summary>
		///         <returns>The attribute as an NSObject, or null if not available.</returns>
		///         <remarks>
		///         </remarks>
		public NSObject? GetLocalizedAttribute (NSString attribute)
		{
			unsafe {
				NSObject? result = Runtime.GetNSObject<NSObject> (CTFontDescriptorCopyLocalizedAttribute (Handle, attribute.Handle, null), true);
				GC.KeepAlive (attribute);
				return result;
			}
		}

		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern IntPtr CTFontDescriptorCopyLocalizedAttribute (IntPtr descriptor, IntPtr attribute, IntPtr* language);
		/// <param name="attribute">An NSString representing a CTFontDescriptor attribute, one of the keys in <see cref="CoreText.CTFontDescriptorAttributeKey" />.</param>
		///         <param name="language">On output, the language code that matched (if available).</param>
		///         <summary>Returns an attribute that has been localized.</summary>
		///         <returns>The attribute as an NSObject or null if not available.</returns>
		///         <remarks>
		///         </remarks>
		public NSObject? GetLocalizedAttribute (NSString attribute, out NSString? language)
		{
			IntPtr handle;
			IntPtr lang;
			unsafe {
				handle = CTFontDescriptorCopyLocalizedAttribute (Handle, attribute.Handle, &lang);
				GC.KeepAlive (attribute);
			}
			language = Runtime.GetNSObject<NSString> (lang, true);
			return Runtime.GetNSObject<NSObject> (handle, true);
		}
		#endregion
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreTextLibrary)]
		static unsafe extern byte CTFontDescriptorMatchFontDescriptorsWithProgressHandler (IntPtr descriptors, IntPtr mandatoryAttributes, BlockLiteral* progressBlock);

		public delegate bool CTFontDescriptorProgressHandler (CTFontDescriptorMatchingState state, CTFontDescriptorMatchingProgress progress);

		[UnmanagedCallersOnly]
		static byte MatchFontDescriptorsHandler (IntPtr block, CTFontDescriptorMatchingState state, IntPtr progress)
		{
			var del = BlockLiteral.GetTarget<CTFontDescriptorProgressHandler> (block);
			if (del is not null) {
				var progressDictionary = Runtime.GetNSObject<NSDictionary> (progress)!;
				var strongDictionary = new CTFontDescriptorMatchingProgress (progressDictionary);
				var rv = del (state, strongDictionary);
				return rv ? (byte) 1 : (byte) 0;
			}
			return 0;
		}

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static bool MatchFontDescriptors (CTFontDescriptor [] descriptors, NSSet? mandatoryAttributes, CTFontDescriptorProgressHandler progressHandler)
		{
			if (descriptors is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (descriptors));

			if (progressHandler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (progressHandler));

			unsafe {
				delegate* unmanaged<IntPtr, CTFontDescriptorMatchingState, IntPtr, byte> trampoline = &MatchFontDescriptorsHandler;
				using var block = new BlockLiteral (trampoline, progressHandler, typeof (CTFontDescriptor), nameof (MatchFontDescriptorsHandler));
				using var descriptorsArray = NSArray.FromNSObjects (descriptors);
				var rv = CTFontDescriptorMatchFontDescriptorsWithProgressHandler (descriptorsArray.GetHandle (), mandatoryAttributes.GetHandle (), &block);
				GC.KeepAlive (descriptorsArray);
				GC.KeepAlive (mandatoryAttributes);
				return rv != 0;
			}
		}

#if !XAMCORE_5_0
		/// <param name="descriptors">To be added.</param>
		///         <param name="mandatoryAttributes">To be added.</param>
		///         <param name="progressHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'MatchFontDescriptors (CTFontDescriptor[], NSSet, CTFontDescriptorProgressHandler)' instead.")]
		public static bool MatchFontDescriptors (CTFontDescriptor [] descriptors, NSSet? mandatoryAttributes, Func<CTFontDescriptorMatchingState, IntPtr, bool> progressHandler)
		{
			throw new NotImplementedException ();
		}
#endif
	}
}
