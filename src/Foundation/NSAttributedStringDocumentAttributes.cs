//
// NSAttributedStringDocumentAttributes.cs
//
// Authors:
//   Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2022 Microsoft Corp

#nullable enable

using System;
using System.ComponentModel;

#if HAS_APPKIT
using AppKit;
#endif
using CoreGraphics;
using Foundation;
#if HAS_UIKIT
using UIKit;
#endif
#if !COREBUILD && HAS_WEBKIT
using WebKit;
#endif
using ObjCRuntime;

#if !COREBUILD
#if __MACOS__
using XColor = AppKit.NSColor;
#else
using XColor = UIKit.UIColor;
#endif
#endif

namespace Foundation {
	/// <summary>A <see cref="Foundation.DictionaryContainer" /> that provides document attributes for <see cref="Foundation.NSAttributedString" />s.</summary>
	///     <remarks>To be added.</remarks>
	public partial class NSAttributedStringDocumentAttributes : DictionaryContainer {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSString? WeakDocumentType {
			get {
				return GetNSStringValue (NSAttributedStringDocumentAttributeKey.DocumentTypeDocumentAttribute);
			}
			set {
				SetStringValue (NSAttributedStringDocumentAttributeKey.DocumentTypeDocumentAttribute, value);
			}
		}

#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'CharacterEncoding' instead.")]
		public NSStringEncoding? StringEncoding {
			get {
				return CharacterEncoding;
			}
			set {
				CharacterEncoding = value;
			}
		}
#endif // !XAMCORE_5_0

#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDocumentType DocumentType {
			get {

				return (NSDocumentType) NSAttributedStringDocumentTypeExtensions.GetValue (WeakDocumentType);
			}
			set {
				WeakDocumentType = ((NSAttributedStringDocumentType) value).GetConstant ();
			}
		}
#endif // !XAMCORE_5_0

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary? WeakDefaultAttributes {
			get {
				return GetNativeValue<NSDictionary> (NSAttributedStringDocumentAttributeKey.DefaultAttributesDocumentAttribute);
			}
			set {
				SetNativeValue (NSAttributedStringDocumentAttributeKey.DefaultAttributesDocumentAttribute, value);
			}
		}

#if XAMCORE_5_0 || __MACOS__
		public bool? ReadOnly {
			get {
				var value = GetInt32Value (NSAttributedStringDocumentAttributeKey.ReadOnlyDocumentAttribute);
				if (value is null)
					return null;
				return value.Value == 1;
			}
			set {
				SetNumberValue (NSAttributedStringDocumentAttributeKey.ReadOnlyDocumentAttribute, value is null ? null : (value.Value ? 1 : 0));
			}
		}
#else
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool ReadOnly {
			get {
				var value = GetInt32Value (NSAttributedStringDocumentAttributeKey.ReadOnlyDocumentAttribute);
				if (value is null || value.Value != 1)
					return false;
				return true;
			}
			set {
				SetNumberValue (NSAttributedStringDocumentAttributeKey.ReadOnlyDocumentAttribute, value ? 1 : 0);
			}
		}
#endif // XAMCORE_5_0 || __MACOS__

#if !TVOS
		// documentation is unclear if an NSString or an NSUrl should be used...
		// but providing an `NSString` throws a `NSInvalidArgumentException Reason: (null) is not a file URL`
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public NSUrl? ReadAccessUrl {
			get {
				// The warning is because NSAttributedStringDocumentReadingOptionKey is in AppKit for macOS, and UIKit for other platforms, so it's not possible to get the availability attributes correct here.
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 13.0 and later, 'maccatalyst' 13.0 and later, 'macOS/OSX' 12.0 and later. 'NSAttributedStringDocumentReadingOptionKey.ReadAccessUrlDocumentOption' is only supported on: 'macOS/OSX' 12.0 and later.
				return GetNativeValue<NSUrl> (NSAttributedStringDocumentReadingOptionKey.ReadAccessUrlDocumentOption);
#pragma warning restore CA1416
			}
			set {
				// The warning is because NSAttributedStringDocumentReadingOptionKey is in AppKit for macOS, and UIKit for other platforms, so it's not possible to get the availability attributes correct here.
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 13.0 and later, 'maccatalyst' 13.0 and later, 'macOS/OSX' 12.0 and later. 'NSAttributedStringDocumentReadingOptionKey.ReadAccessUrlDocumentOption' is only supported on: 'macOS/OSX' 12.0 and later.
				SetNativeValue (NSAttributedStringDocumentReadingOptionKey.ReadAccessUrlDocumentOption, value);
#pragma warning restore CA1416
			}
		}
#endif // !TVOS

#if __MACOS__
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public WebPreferences? WebPreferences {
			get {
				return GetNativeValue<WebPreferences> (NSAttributedStringDocumentReadingOptionKey.WebPreferencesDocumentOption);
			}
			set {
				SetNativeValue (NSAttributedStringDocumentReadingOptionKey.WebPreferencesDocumentOption, value);
			}
		}
#endif // !__MACOS__

#if __MACOS__
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public NSObject? WebResourceLoadDelegate {
			get {
				return GetNativeValue<NSObject> (NSAttributedStringDocumentReadingOptionKey.WebResourceLoadDelegateDocumentOption);
			}
			set {
				SetNativeValue (NSAttributedStringDocumentReadingOptionKey.WebResourceLoadDelegateDocumentOption, value);
			}
		}
#endif // !__MACOS__

#if __MACOS__
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public NSUrl? BaseUrl {
			get {
				return GetNativeValue<NSUrl> (NSAttributedStringDocumentReadingOptionKey.BaseUrlDocumentOption);
			}
			set {
				SetNativeValue (NSAttributedStringDocumentReadingOptionKey.BaseUrlDocumentOption, value);
			}
		}
#endif // !__MACOS__

#if __MACOS__
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public float? TextSizeMultiplier {
			get {
				return GetFloatValue (NSAttributedStringDocumentReadingOptionKey.TextSizeMultiplierDocumentOption);
			}
			set {
				SetNumberValue (NSAttributedStringDocumentReadingOptionKey.TextSizeMultiplierDocumentOption, value);
			}
		}
#endif // !__MACOS__

#if __MACOS__
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public float? Timeout {
			get {
				return GetFloatValue (NSAttributedStringDocumentReadingOptionKey.TimeoutDocumentOption);
			}
			set {
				SetNumberValue (NSAttributedStringDocumentReadingOptionKey.TimeoutDocumentOption, value);
			}
		}
#endif // !__MACOS__

#endif // !COREBUILD
	}
}
