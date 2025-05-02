//
// coregraphics.cs: Definitions for CoreGraphics
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;

namespace CoreGraphics {

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum CGToneMapping : uint {
		Default = 0,
		ImageSpecificLumaScaling,
		ReferenceWhiteBased,
		IturRecommended,
		ExrGamma,
		None,
	}

	/// <summary>Specifies various boxes for the <see cref="CoreGraphics.CGContextPDF.BeginPage(CoreGraphics.CGPDFPageInfo)" /> method.</summary>
	[Partial]
	interface CGPDFPageInfo {

		[Internal]
		[Field ("kCGPDFContextMediaBox")]
		IntPtr kCGPDFContextMediaBox { get; }

		[Internal]
		[Field ("kCGPDFContextCropBox")]
		IntPtr kCGPDFContextCropBox { get; }

		[Internal]
		[Field ("kCGPDFContextBleedBox")]
		IntPtr kCGPDFContextBleedBox { get; }

		[Internal]
		[Field ("kCGPDFContextTrimBox")]
		IntPtr kCGPDFContextTrimBox { get; }

		[Internal]
		[Field ("kCGPDFContextArtBox")]
		IntPtr kCGPDFContextArtBox { get; }
	}

	/// <summary>Auxiliary parameters for constructing a <see cref="CoreGraphics.CGContextPDF" />.</summary>
	[Partial]
	interface CGPDFInfo {

		[Internal]
		[Field ("kCGPDFContextTitle")]
		IntPtr kCGPDFContextTitle { get; }

		[Internal]
		[Field ("kCGPDFContextAuthor")]
		IntPtr kCGPDFContextAuthor { get; }

		[Internal]
		[Field ("kCGPDFContextSubject")]
		IntPtr kCGPDFContextSubject { get; }

		[Internal]
		[Field ("kCGPDFContextKeywords")]
		IntPtr kCGPDFContextKeywords { get; }

		[Internal]
		[Field ("kCGPDFContextCreator")]
		IntPtr kCGPDFContextCreator { get; }

		[Internal]
		[Field ("kCGPDFContextOwnerPassword")]
		IntPtr kCGPDFContextOwnerPassword { get; }

		[Internal]
		[Field ("kCGPDFContextUserPassword")]
		IntPtr kCGPDFContextUserPassword { get; }

		[Internal]
		[Field ("kCGPDFContextEncryptionKeyLength")]
		IntPtr kCGPDFContextEncryptionKeyLength { get; }

		[Internal]
		[Field ("kCGPDFContextAllowsPrinting")]
		IntPtr kCGPDFContextAllowsPrinting { get; }

		[Internal]
		[Field ("kCGPDFContextAllowsCopying")]
		IntPtr kCGPDFContextAllowsCopying { get; }

#if false
		kCGPDFContextOutputIntent;
		kCGPDFXOutputIntentSubtype;
		kCGPDFXOutputConditionIdentifier;
		kCGPDFXOutputCondition;
		kCGPDFXRegistryName;
		kCGPDFXInfo;
		kCGPDFXDestinationOutputProfile;
		kCGPDFContextOutputIntents;
#endif

		[MacCatalyst (13, 1)]
		[Internal]
		[Field ("kCGPDFContextAccessPermissions")]
		IntPtr kCGPDFContextAccessPermissions { get; }

		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Internal]
		[Field ("kCGPDFContextCreateLinearizedPDF")]
		IntPtr kCGPDFContextCreateLinearizedPDF { get; }

		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Internal]
		[Field ("kCGPDFContextCreatePDFA")]
		IntPtr kCGPDFContextCreatePDFA { get; }
	}

	/// <summary>Provides string constants whose values are known color spaces.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface CGColorSpaceNames {
		/// <summary>Gets the name of the generic gray color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceGenericGray")]
		NSString GenericGray { get; }

		/// <summary>Gets a string constant that identifies the GenericRgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceGenericRGB")]
		NSString GenericRgb { get; }

		/// <summary>Gets a string constant that identifies the GenericCmyk color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceGenericCMYK")]
		NSString GenericCmyk { get; }

		/// <summary>Gets a string constant that identifies the DisplayP3 color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceDisplayP3")]
		NSString DisplayP3 { get; }

		/// <summary>Gets a string constant that identifies the GenericRgbLinear color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceGenericRGBLinear")]
		NSString GenericRgbLinear { get; }

		/// <summary>Gets a string constant that identifies the AdobeRgb1998 color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceAdobeRGB1998")]
		NSString AdobeRgb1998 { get; }

		/// <summary>Gets a string constant that identifies the Srgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceSRGB")]
		NSString Srgb { get; }

		/// <summary>Gets the name of the generic gray color space that has a gamma value of 2.2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCGColorSpaceGenericGrayGamma2_2")]
		NSString GenericGrayGamma2_2 { get; }

		/// <summary>Gets a string constant that identifies the GenericXyz color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceGenericXYZ")]
		NSString GenericXyz { get; }

		/// <summary>Gets a string constant that identifies the AcesCGLinear color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceACESCGLinear")]
		NSString AcesCGLinear { get; }

		/// <summary>Gets a string constant that identifies the ItuR_709 color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceITUR_709")]
		NSString ItuR_709 { get; }

		[Mac (12, 1), iOS (15, 2), TV (15, 2)]
		[MacCatalyst (15, 2)]
		[Field ("kCGColorSpaceITUR_709_PQ")]
		NSString ItuR_709_PQ { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Field ("kCGColorSpaceITUR_709_HLG")]
		NSString ItuR_709_Hlg { get; }

		/// <summary>Gets a string constant that identifies the ItuR_2020 color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceITUR_2020")]
		NSString ItuR_2020 { get; }

		[Mac (12, 1), iOS (15, 2), TV (15, 2)]
		[MacCatalyst (15, 2)]
		[Field ("kCGColorSpaceITUR_2020_sRGBGamma")]
		NSString ItuR_2020_sRgbGamma { get; }

		/// <summary>Gets a string constant that identifies the RommRgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceROMMRGB")]
		NSString RommRgb { get; }

		/// <summary>Gets a string constant that identifies the Dcip3 color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceDCIP3")]
		NSString Dcip3 { get; }

		/// <summary>Gets a string constant that identifies the ExtendedSrgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedSRGB")]
		NSString ExtendedSrgb { get; }

		/// <summary>Gets a string constant that identifies the LinearSrgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceLinearSRGB")]
		NSString LinearSrgb { get; }

		/// <summary>Gets a string constant that identifies the ExtendedLinearSrgb color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedLinearSRGB")]
		NSString ExtendedLinearSrgb { get; }

		/// <summary>Gets a string constant that identifies the ExtendedGray color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedGray")]
		NSString ExtendedGray { get; }

		/// <summary>Gets a string constant that identifies the LinearGray color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceLinearGray")]
		NSString LinearGray { get; }

		/// <summary>Gets a string constant that identifies the ExtendedLinearGray color space.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedLinearGray")]
		NSString ExtendedLinearGray { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Obsolete ("Now accessible as GenericCmyk.")]
		[Field ("kCGColorSpaceGenericCMYK")]
		NSString GenericCMYK { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Obsolete ("Now accessible as AdobeRgb1998.")]
		[Field ("kCGColorSpaceAdobeRGB1998")]
		NSString AdobeRGB1998 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Obsolete ("Now accessible as Srgb.")]
		[Field ("kCGColorSpaceSRGB")]
		NSString SRGB { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Obsolete ("Now accessible as GenericRgb.")]
		[Field ("kCGColorSpaceGenericRGB")]
		NSString GenericRGB { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Obsolete ("Now accessible as GenericRgb.")]
		[Field ("kCGColorSpaceGenericRGBLinear")]
		NSString GenericRGBLinear { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceGenericLab")]
		NSString GenericLab { get; }

		[iOS (12, 3)]
		[TV (12, 3)]
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedLinearITUR_2020")]
		NSString ExtendedLinearItur_2020 { get; }

		[iOS (14, 1), TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Field ("kCGColorSpaceExtendedITUR_2020")]
		NSString ExtendedItur_2020 { get; }

		[iOS (12, 3)]
		[TV (12, 3)]
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceExtendedLinearDisplayP3")]
		NSString ExtendedLinearDisplayP3 { get; }

		[iOS (14, 1), TV (14, 2)]
		[MacCatalyst (14, 1)]
		[Field ("kCGColorSpaceExtendedDisplayP3")]
		NSString ExtendedDisplayP3 { get; }

		[Deprecated (PlatformName.MacOSX, 10, 15, 4, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 4, message: "Use 'Itur_2100_PQ' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'Itur_2100_PQ' instead.")]
		[Field ("kCGColorSpaceITUR_2020_PQ_EOTF")]
		NSString Itur_2020_PQ_Eotf { get; }

		[iOS (13, 4), TV (13, 4)]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Field ("kCGColorSpaceITUR_2020_PQ")]
		NSString Itur_2020_PQ { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 15, 4)]
		[Deprecated (PlatformName.iOS, 13, 4)]
		[Deprecated (PlatformName.TvOS, 13, 4)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Field ("kCGColorSpaceDisplayP3_PQ_EOTF")]
		NSString DisplayP3_PQ_Eotf { get; }

		[iOS (13, 4), TV (13, 4)]
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceDisplayP3_PQ")]
		NSString DisplayP3_PQ { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCGColorSpaceDisplayP3_HLG")]
		NSString DisplayP3_Hlg { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'Itur_2100_PQ' instead.")]
		[Field ("kCGColorSpaceITUR_2020_HLG")]
		NSString Itur_2020_Hlg { get; }

		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("kCGColorSpaceITUR_2100_HLG")]
		NSString Itur_2100_Hlg { get; }

		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("kCGColorSpaceITUR_2100_PQ")]
		NSString Itur_2100_PQ { get; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Field ("kCGColorSpaceExtendedRange")]
		NSString ExtendedRange { get; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Field ("kCGColorSpaceLinearDisplayP3")]
		NSString LinearDisplayP3 { get; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Field ("kCGColorSpaceLinearITUR_2020")]
		NSString LinearItur_2020 { get; }

		[Mac (15, 0), iOS (18, 0), TV (18, 0), MacCatalyst (18, 0)]
		[Field ("kCGColorSpaceCoreMedia709")]
		NSString CoreMedia709 { get; }
	}

	[Partial]
	partial interface CGColorConversionInfo {

		[Internal]
		[Field ("kCGColorConversionBlackPointCompensation")]
		NSString BlackPointCompensationKey { get; }

		[Internal]
		[Field ("kCGColorConversionTRCSize")]
		[MacCatalyst (13, 1)]
		NSString TrcSizeKey { get; }
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("CGColorConversionInfo")]
	interface CGColorConversionOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool BlackPointCompensation { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CGSize TrcSize { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	public interface CGPDFOutlineKeys {
		[Internal]
		[Field ("kCGPDFOutlineTitle")]
		NSString OutlineTitleKey { get; }

		[Internal]
		[Field ("kCGPDFOutlineChildren")]
		NSString OutlineChildrenKey { get; }

		[Internal]
		[Field ("kCGPDFOutlineDestination")]
		NSString OutlineDestinationKey { get; }

		[Internal]
		[Field ("kCGPDFOutlineDestinationRect")]
		NSString DestinationRectKey { get; }

		[Internal]
		[Field ("kCGPDFContextAccessPermissions")]
		NSString AccessPermissionsKey { get; }
	}

	[MacCatalyst (13, 1)]
	[StrongDictionary ("CGPDFOutlineKeys")]
	interface CGPDFOutlineOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string OutlineTitle { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary [] OutlineChildren { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSObject OutlineDestination { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		CGRect DestinationRect { get; set; }
	}

	[iOS (13, 0)]
	[TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Static]
	[Internal]
	interface CGPdfTagPropertyKeys {
		[Field ("kCGPDFTagPropertyActualText")]
		NSString ActualTextKey { get; }

		[Field ("kCGPDFTagPropertyAlternativeText")]
		NSString AlternativeTextKey { get; }

		[Field ("kCGPDFTagPropertyTitleText")]
		NSString TitleTextKey { get; }

		[Field ("kCGPDFTagPropertyLanguageText")]
		NSString LanguageTextKey { get; }
	}

	[iOS (13, 0)]
	[TV (13, 0)]
	[MacCatalyst (13, 1)]
	[StrongDictionary ("CGPdfTagPropertyKeys")]
	interface CGPdfTagProperties {
		// <quote>The following CGPDFTagProperty keys are to be paired with CFStringRef values</quote>
		string ActualText { get; set; }
		string AlternativeText { get; set; }
		string TitleText { get; set; }
		string LanguageText { get; set; }
	}

	// macOS 10.5
	[iOS (14, 0)]
	[TV (14, 0)]
	[MacCatalyst (14, 0)]
	enum CGConstantColor {
		[Field ("kCGColorWhite")]
		White,
		[Field ("kCGColorBlack")]
		Black,
		[Field ("kCGColorClear")]
		Clear,
	}

	// Adding suffix *Keys to avoid possible name clash
	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ScreenCaptureKit instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ScreenCaptureKit instead.")]
	interface CGDisplayStreamKeys {

		[Field ("kCGDisplayStreamColorSpace")]
		NSString ColorSpace { get; }

		[Field ("kCGDisplayStreamDestinationRect")]
		NSString DestinationRect { get; }

		[Field ("kCGDisplayStreamMinimumFrameTime")]
		NSString MinimumFrameTime { get; }

		[Field ("kCGDisplayStreamPreserveAspectRatio")]
		NSString PreserveAspectRatio { get; }

		[Field ("kCGDisplayStreamQueueDepth")]
		NSString QueueDepth { get; }

		[Field ("kCGDisplayStreamShowCursor")]
		NSString ShowCursor { get; }

		[Field ("kCGDisplayStreamSourceRect")]
		NSString SourceRect { get; }

		[Field ("kCGDisplayStreamYCbCrMatrix")]
		NSString YCbCrMatrix { get; }
	}

	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[Static]
	interface CGDisplayStreamYCbCrMatrixOptionKeys {

		[Field ("kCGDisplayStreamYCbCrMatrix_ITU_R_601_4")]
		NSString Itu_R_601_4 { get; }

		[Field ("kCGDisplayStreamYCbCrMatrix_ITU_R_709_2")]
		NSString Itu_R_709_2 { get; }

		[Field ("kCGDisplayStreamYCbCrMatrix_SMPTE_240M_1995")]
		NSString Smpte_240M_1995 { get; }
	}

	[NoiOS, NoTV, MacCatalyst (13, 1)]
	[StrongDictionary ("CGSessionKeys")]
	interface CGSessionProperties {
		uint UserId { get; }
		string UserName { get; }
		uint ConsoleSet { get; }
		bool OnConsole { get; }
		bool LoginDone { get; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Partial]
	partial interface CGToneMappingOptionKeys {
		[Internal]
		[Field ("kCGUse100nitsHLGOOTF")]
		NSString Use100nitsHlgOotfKey { get; }

		[Internal]
		[Field ("kCGUseBT1886ForCoreVideoGamma")]
		NSString UseBT1886ForCoreVideoGammaKey { get; }

		[Internal]
		[Field ("kCGSkipBoostToHDR")]
		NSString SkipBoostToHdrKey { get; }

		[Internal]
		[Field ("kCGEXRToneMappingGammaDefog")]
		NSString ExrToneMappingGammaDefogKey { get; }

		[Internal]
		[Field ("kCGEXRToneMappingGammaExposure")]
		NSString ExrToneMappingGammaExposureKey { get; }

		[Internal]
		[Field ("kCGEXRToneMappingGammaKneeLow")]
		NSString ExrToneMappingGammaKneeLowKey { get; }

		[Internal]
		[Field ("kCGEXRToneMappingGammaKneeHigh")]
		NSString ExrToneMappingGammaKneeHighKey { get; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[StrongDictionary ("CGToneMappingOptionKeys")]
	interface CGToneMappingOptions {
		bool Use100nitsHlgOotf { get; set; }
		bool UseBT1886ForCoreVideoGamma { get; set; }
		bool SkipBoostToHdr { get; set; }
		float ExrToneMappingGammaDefog { get; set; }
		float ExrToneMappingGammaExposure { get; set; }
		float ExrToneMappingGammaKneeLow { get; set; }
		float ExrToneMappingGammaKneeHigh { get; set; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Partial]
	[Internal]
	interface CoreGraphicsFields {
		[Field ("kCGDefaultHDRImageContentHeadroom")]
		float DefaultHdrImageContentHeadroom { get; }
	}
}
