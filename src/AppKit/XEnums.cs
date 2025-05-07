//
// Enums.cs: enums for AppKit
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2012 Xamarin Inc

using System;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	[NoMacCatalyst]
	[Native]
	public enum NSPrintRenderingQuality : long {
		/// <summary>To be added.</summary>
		Best,
		/// <summary>To be added.</summary>
		Responsive,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCorrectionIndicatorType : long {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Reversion,
		/// <summary>To be added.</summary>
		Guesses,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCorrectionResponse : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Accepted,
		/// <summary>To be added.</summary>
		Rejected,
		/// <summary>To be added.</summary>
		Ignored,
		/// <summary>To be added.</summary>
		Edited,
		/// <summary>To be added.</summary>
		Reverted,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextFinderMatchingType : long {
		/// <summary>To be added.</summary>
		Contains = 0,
		/// <summary>To be added.</summary>
		StartsWith = 1,
		/// <summary>To be added.</summary>
		FullWord = 2,
		/// <summary>To be added.</summary>
		EndsWith = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCharacterCollection : ulong {
		/// <summary>Identity mapping (CID == NSGlyph)</summary>
		IdentityMapping = 0,

		/// <summary>Adobe-CNS1</summary>
		AdobeCns1 = 1,

		/// <summary>Adobe-GB1</summary>
		AdobeGb1 = 2,

		/// <summary>Adobe-Japan1</summary>
		AdobeJapan1 = 3,

		/// <summary>Adobe-Japan2</summary>
		AdobeJapan2 = 4,

		/// <summary>Adobe-Korea1</summary>
		AdobeKorea1 = 5,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSSpellingState : long {
		/// <summary>To be added.</summary>
		None = 0x0,
		/// <summary>To be added.</summary>
		Spelling = 0x1,
		/// <summary>To be added.</summary>
		Grammar = 0x2,
	}
}
