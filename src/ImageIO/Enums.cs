//
// Enums.cs
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2016, Xamarin Inc.
// Copyright 2020, Microsoft Corp.
//

using System;
using ObjCRuntime;

namespace ImageIO {

	// untyped enum -> CGImageMetadata.h
	// note: not used in any API
	/// <summary>Enumeration of errors relating to metadata manipulation.</summary>
	[ErrorDomain ("kCFErrorDomainCGImageMetadata")]
	public enum CGImageMetadataErrors {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		UnsupportedFormat = 1,
		/// <summary>To be added.</summary>
		BadArgument = 2,
		/// <summary>To be added.</summary>
		ConflictingArguments = 3,
		/// <summary>To be added.</summary>
		PrefixConflict = 4,
	}

	// untyped enum -> CGImageMetadata.h
	/// <summary>Enumerates the type-forms of image metadata.</summary>
	public enum CGImageMetadataType {
		/// <summary>To be added.</summary>
		Invalid = -1,
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		String = 1,
		/// <summary>To be added.</summary>
		ArrayUnordered = 2,
		/// <summary>To be added.</summary>
		ArrayOrdered = 3,
		/// <summary>To be added.</summary>
		AlternateArray = 4,
		/// <summary>To be added.</summary>
		AlternateText = 5,
		/// <summary>To be added.</summary>
		Structure = 6,
	}

	/// <summary>Enumerates orientation values.</summary>
	public enum CGImagePropertyOrientation {
		/// <summary>To be added.</summary>
		Up = 1,
		/// <summary>To be added.</summary>
		UpMirrored,
		/// <summary>To be added.</summary>
		Down,
		/// <summary>To be added.</summary>
		DownMirrored,
		/// <summary>To be added.</summary>
		LeftMirrored,
		/// <summary>To be added.</summary>
		Right,
		/// <summary>To be added.</summary>
		RightMirrored,
		/// <summary>To be added.</summary>
		Left,
	}

	// untyped enum / #defines
	// used with kCGImagePropertyPNGCompressionFilter
	/// <summary>Enumerates the style of a PNG compression filter.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	public enum CGImagePropertyPngFilters {
		/// <summary>To be added.</summary>
		No = 0,
		/// <summary>To be added.</summary>
		None = 0x08,
		/// <summary>To be added.</summary>
		Sub = 0x10,
		/// <summary>To be added.</summary>
		Up = 0x20,
		/// <summary>To be added.</summary>
		Average = 0x40,
		/// <summary>To be added.</summary>
		Paeth = 0x80,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	public enum CGImageAnimationStatus {
		Ok = 0,
		ParameterError = -22140,
		CorruptInputImage = -22141,
		UnsupportedFormat = -22142,
		IncompleteInputImage = -22143,
		AllocationFailure = -22144,
	}

	// Yes, no [Native] here
	[iOS (14, 1), TV (14, 2)]
	[MacCatalyst (14, 1)]
	public enum CGImagePropertyTgaCompression : uint {
		None = 0,
		Rle,
	}
}
