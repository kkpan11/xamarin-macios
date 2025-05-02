using System;
using ObjCRuntime;

namespace Foundation {

#if !XAMCORE_5_0
	// Utility enum, ObjC uses NSString
	/// <summary>An enumeration of known document types. Used with the <see cref="Foundation.NSAttributedStringDocumentAttributes.DocumentType" /> property.</summary>
	public enum NSDocumentType {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>To be added.</summary>
		PlainText,
		/// <summary>To be added.</summary>
		RTF,
		/// <summary>To be added.</summary>
		RTFD,
		/// <summary>To be added.</summary>
		HTML,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		MacSimpleText,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		DocFormat,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		WordML,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		OfficeOpenXml,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		WebArchive,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV, NoMacCatalyst]
		OpenDocument,
	}
#endif // !XAMCORE_5_0

	// Utility enum, ObjC uses NSString
	/// <summary>An enumeration that specifies how a document is being viewed. Used with the <see cref="Foundation.NSAttributedStringDocumentAttributes.ViewMode" /> property.</summary>
	public enum NSDocumentViewMode {
		/// <summary>To be added.</summary>
		Normal,
		/// <summary>To be added.</summary>
		PageLayout,

	}

	/// <summary>Run loop modes for <see cref="Foundation.NSRunLoop" />.</summary>
	public enum NSRunLoopMode {

		/// <summary>The default mode to handle input sources.   The most common run loop mode.</summary>
		[DefaultEnumValue]
		[Field ("NSDefaultRunLoopMode")]
		Default,

		/// <summary>Run loop mode constant used to run handlers in any of the declared “common” modes.</summary>
		[Field ("NSRunLoopCommonModes")]
		Common,

#if MONOMAC
		/// <summary>To be added.</summary>
		[Field ("NSConnectionReplyMode")]
		ConnectionReply = 2,

		/// <summary>To be added.</summary>
		[Field ("NSModalPanelRunLoopMode", "AppKit")]
		ModalPanel,

		/// <summary>To be added.</summary>
		[Field ("NSEventTrackingRunLoopMode", "AppKit")]
		EventTracking,
#else
		// iOS-specific Enums start in 100 to avoid conflicting with future extensions to MonoMac
		/// <summary>The NSRunLoop mode used when tracking controls. Use this to receive timers and events during UI tracking.</summary>
		[Field ("UITrackingRunLoopMode", "UIKit")]
		UITracking = 100,
#endif
		// If it is not part of these enumerations
		/// <summary>To be added.</summary>
		[Field (null)]
		Other = 1000,
	}

	/// <summary>Enumerates download status values.</summary>
	[MacCatalyst (13, 1)]
	public enum NSItemDownloadingStatus {
		/// <summary>To be added.</summary>
		[Field (null)]
		Unknown = -1,

		/// <summary>To be added.</summary>
		[Field ("NSMetadataUbiquitousItemDownloadingStatusCurrent")]
		Current,

		/// <summary>To be added.</summary>
		[Field ("NSMetadataUbiquitousItemDownloadingStatusDownloaded")]
		Downloaded,

		/// <summary>To be added.</summary>
		[Field ("NSMetadataUbiquitousItemDownloadingStatusNotDownloaded")]
		NotDownloaded,
	}

	[MacCatalyst (13, 1)]
	public enum NSStringTransform {
		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToKatakana")]
		LatinToKatakana,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToHiragana")]
		LatinToHiragana,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToHangul")]
		LatinToHangul,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToArabic")]
		LatinToArabic,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToHebrew")]
		LatinToHebrew,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToThai")]
		LatinToThai,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToCyrillic")]
		LatinToCyrillic,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformLatinToGreek")]
		LatinToGreek,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformToLatin")]
		ToLatin,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformMandarinToLatin")]
		MandarinToLatin,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformHiraganaToKatakana")]
		HiraganaToKatakana,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformFullwidthToHalfwidth")]
		FullwidthToHalfwidth,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformToXMLHex")]
		ToXmlHex,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformToUnicodeName")]
		ToUnicodeName,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformStripCombiningMarks")]
		StripCombiningMarks,

		/// <summary>To be added.</summary>
		[Field ("NSStringTransformStripDiacritics")]
		StripDiacritics,
	}

	[NoTV, NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSUrlSessionMultipathServiceType : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Handover = 1,
		/// <summary>To be added.</summary>
		Interactive = 2,
		/// <summary>To be added.</summary>
		Aggregate = 3,
	}

	/// <summary>Enumerates classes of tags that are returned from a text classifier.</summary>
	public enum NSLinguisticTagScheme {
		/// <summary>Indicates that tokens will be tagged with information about whether they are words, whitespace, or punctuation.</summary>
		[Field ("NSLinguisticTagSchemeTokenType")]
		Token,

		/// <summary>Indicates that tokens will be returned for whether they are part of speech or whitespace, or their punctuation type if they are punctuation.</summary>
		[Field ("NSLinguisticTagSchemeLexicalClass")]
		LexicalClass,

		/// <summary>Indicates that tokens will be tagged as names of which they are a part.</summary>
		[Field ("NSLinguisticTagSchemeNameType")]
		Name,

		/// <summary>Indicates that tokens will be tagged those tags indicated by <see cref="Foundation.NSLinguisticTagScheme.LexicalClass" /> and <see cref="Foundation.NSLinguisticTagScheme.Name" />.</summary>
		[Field ("NSLinguisticTagSchemeNameTypeOrLexicalClass")]
		NameOrLexicalClass,

		/// <summary>Indicates that tokens will be tagged with their stem, if known.</summary>
		[Field ("NSLinguisticTagSchemeLemma")]
		Lemma,

		/// <summary>Indicates that tokens will be tagged with their language, if known.</summary>
		[Field ("NSLinguisticTagSchemeLanguage")]
		Language,

		/// <summary>Indicates that tokens will be tagged with the script in which they were written.</summary>
		[Field ("NSLinguisticTagSchemeScript")]
		Script,
	}

#if !NET
	public enum NSLinguisticTagUnit {
#else
	/// <summary>Contains read-only static properties corresponding to the parts of speech recognized by a <see cref="Foundation.NSLinguisticTagger" />.</summary>
	/// <summary>Enumerates tag values for linguistic units.</summary>
	public enum NSLinguisticTag {
#endif
		[Field ("NSLinguisticTagWord")]
		Word,

		[Field ("NSLinguisticTagPunctuation")]
		Punctuation,

		[Field ("NSLinguisticTagWhitespace")]
		Whitespace,

		[Field ("NSLinguisticTagOther")]
		Other,

		[Field ("NSLinguisticTagNoun")]
		Noun,

		[Field ("NSLinguisticTagVerb")]
		Verb,

		[Field ("NSLinguisticTagAdjective")]
		Adjective,

		[Field ("NSLinguisticTagAdverb")]
		Adverb,

		[Field ("NSLinguisticTagPronoun")]
		Pronoun,

		[Field ("NSLinguisticTagDeterminer")]
		Determiner,

		[Field ("NSLinguisticTagParticle")]
		Particle,

		[Field ("NSLinguisticTagPreposition")]
		Preposition,

		[Field ("NSLinguisticTagNumber")]
		Number,

		[Field ("NSLinguisticTagConjunction")]
		Conjunction,

		[Field ("NSLinguisticTagInterjection")]
		Interjection,

		[Field ("NSLinguisticTagClassifier")]
		Classifier,

		[Field ("NSLinguisticTagIdiom")]
		Idiom,

		[Field ("NSLinguisticTagOtherWord")]
		OtherWord,

		[Field ("NSLinguisticTagSentenceTerminator")]
		Terminator,

		[Field ("NSLinguisticTagOpenQuote")]
		OpenQuote,

		[Field ("NSLinguisticTagCloseQuote")]
		CloseQuote,

		[Field ("NSLinguisticTagOpenParenthesis")]
		OpenParenthesis,

		[Field ("NSLinguisticTagCloseParenthesis")]
		CloseParenthesis,

		[Field ("NSLinguisticTagWordJoiner")]
		WordJoiner,

		[Field ("NSLinguisticTagDash")]
		Dash,

		[Field ("NSLinguisticTagOtherPunctuation")]
		OtherPunctuation,

		[Field ("NSLinguisticTagParagraphBreak")]
		ParagraphBreak,

		[Field ("NSLinguisticTagOtherWhitespace")]
		OtherWhitespace,

		[Field ("NSLinguisticTagPersonalName")]
		PersonalName,

		[Field ("NSLinguisticTagOrganizationName")]
		OrganizationName,

		[Field ("NSLinguisticTagPlaceName")]
		PlaceName,
	}

	[Flags]
	[Native]
	public enum NSStringEnumerationOptions : ulong {
		ByLines = 0x0,
		ByParagraphs = 0x1,
		ByComposedCharacterSequences = 0x2,
		ByWords = 0x3,
		BySentences = 0x4,
		ByCaretPositions = 0x5,
		ByDeletionClusters = 0x6,
		Reverse = 1uL << 8,
		SubstringNotRequired = 1uL << 9,
		Localized = 1uL << 10,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Flags]
	[Native]
	public enum NSAttributedStringFormattingOptions : ulong {
		InsertArgumentAttributesWithoutMerging = 1uL << 0,
		ApplyReplacementIndexAttribute = 1uL << 1,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSAttributedStringMarkdownInterpretedSyntax : long {
		Full = 0,
		InlineOnly = 1,
		InlineOnlyPreservingWhitespace = 2,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSAttributedStringMarkdownParsingFailurePolicy : long {
		Error = 0,
		PartiallyParsedIfPossible = 1,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSGrammaticalGender : long {
		NotSet = 0,
		Feminine,
		Masculine,
		Neuter,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSGrammaticalNumber : long {
		NotSet = 0,
		Singular,
		Zero,
		Plural,
		PluralTwo,
		PluralFew,
		PluralMany,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSGrammaticalPartOfSpeech : long {
		NotSet = 0,
		Determiner,
		Pronoun,
		Letter,
		Adverb,
		Particle,
		Adjective,
		Adposition,
		Verb,
		Noun,
		Conjunction,
		Numeral,
		Interjection,
		Preposition,
		Abbreviation,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSInlinePresentationIntent : ulong {
		Emphasized = 1uL << 0,
		StronglyEmphasized = 1uL << 1,
		Code = 1uL << 2,
		Strikethrough = 1uL << 5,
		SoftBreak = 1uL << 6,
		LineBreak = 1uL << 7,
		InlineHTML = 1uL << 8,
		BlockHTML = 1uL << 9,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSPresentationIntentKind : long {
		Paragraph,
		Header,
		OrderedList,
		UnorderedList,
		ListItem,
		CodeBlock,
		BlockQuote,
		ThematicBreak,
		Table,
		TableHeaderRow,
		TableRow,
		TableCell,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSPresentationIntentTableColumnAlignment : long {
		Left,
		Center,
		Right,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum NSURLRequestAttribution : ulong {
		Developer = 0,
		User = 1,
	}
}
