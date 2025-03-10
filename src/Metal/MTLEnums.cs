//
// API for the Metal framework
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2014, Xamarin Inc.
//
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ModelIO;
using ObjCRuntime;

#nullable enable

namespace Metal {

	/// <summary>Argument values for use with <see cref="T:Metal.MTLBlendOperation" />.</summary>
	[Native]
	public enum MTLBlendFactor : ulong {
		/// <summary>To be added.</summary>
		Zero = 0,
		/// <summary>To be added.</summary>
		One = 1,
		/// <summary>To be added.</summary>
		SourceColor = 2,
		/// <summary>To be added.</summary>
		OneMinusSourceColor = 3,
		/// <summary>To be added.</summary>
		SourceAlpha = 4,
		/// <summary>To be added.</summary>
		OneMinusSourceAlpha = 5,
		/// <summary>To be added.</summary>
		DestinationColor = 6,
		/// <summary>To be added.</summary>
		OneMinusDestinationColor = 7,
		/// <summary>To be added.</summary>
		DestinationAlpha = 8,
		/// <summary>To be added.</summary>
		OneMinusDestinationAlpha = 9,
		/// <summary>To be added.</summary>
		SourceAlphaSaturated = 10,
		/// <summary>To be added.</summary>
		BlendColor = 11,
		/// <summary>To be added.</summary>
		OneMinusBlendColor = 12,
		/// <summary>To be added.</summary>
		BlendAlpha = 13,
		/// <summary>To be added.</summary>
		OneMinusBlendAlpha = 14,
		[MacCatalyst (13, 1)]
		Source1Color = 15,
		[MacCatalyst (13, 1)]
		OneMinusSource1Color = 16,
		[MacCatalyst (13, 1)]
		Source1Alpha = 17,
		[MacCatalyst (13, 1)]
		OneMinusSource1Alpha = 18,
	}

	/// <summary>How the source and destination values are combined.</summary>
	[Native]
	public enum MTLBlendOperation : ulong {
		/// <summary>To be added.</summary>
		Add = 0,
		/// <summary>To be added.</summary>
		Subtract = 1,
		/// <summary>To be added.</summary>
		ReverseSubtract = 2,
		/// <summary>To be added.</summary>
		Min = 3,
		/// <summary>To be added.</summary>
		Max = 4,
	}

	/// <summary>Permits or restricts writing to specific color channels.</summary>
	[Native]
	[Flags]
	public enum MTLColorWriteMask : ulong {
		None = 0,
		Red = 0x1 << 3,
		Green = 0x1 << 2,
		Blue = 0x1 << 1,
		Alpha = 0x1 << 0,
		All = 0xf,
	}

	/// <summary>The state of the command-buffer state-machine. Enqueued-&gt;Committed-&gt;Scheduled-&gt;Completed.</summary>
	[Native]
	public enum MTLCommandBufferStatus : ulong {
		NotEnqueued,
		Enqueued,
		Committed,
		Scheduled,
		Completed,
		Error,
	}

	/// <summary>Enumeration of error codes describing why a command buffer has failed.</summary>
	[Native]
	[ErrorDomain ("MTLCommandBufferErrorDomain")]
	public enum MTLCommandBufferError : ulong {
		None = 0,
		Internal = 1,
		Timeout = 2,
		PageFault = 3,
		Blacklisted = 4,
		NotPermitted = 7,
		OutOfMemory = 8,
		InvalidResource = 9,
		Memoryless = 10,
		DeviceRemoved = 11,
		StackOverflow = 12,
	}

	/// <summary>An action performed at the start of a rendering pass.</summary>
	[Native]
	public enum MTLLoadAction : ulong {
		DontCare,
		Load,
		Clear,
	}

	/// <summary>The action to be performed at the end of a rendering pass.</summary>
	[Native]
	public enum MTLStoreAction : ulong {
		DontCare,
		Store,
		MultisampleResolve,
		[MacCatalyst (13, 1)]
		StoreAndMultisampleResolve,
		[MacCatalyst (13, 1)]
		Unknown,
		[MacCatalyst (13, 1)]
		CustomSampleDepthStore,
	}

	/// <summary>The dimensionality of each texture and how multiple images are arranged.</summary>
	[Native]
	public enum MTLTextureType : ulong {
		k1D = 0,
		k1DArray = 1,
		k2D = 2,
		k2DArray = 3,
		k2DMultisample = 4,
		kCube = 5,
		[MacCatalyst (13, 1)]
		kCubeArray = 6,
		k3D = 7,
		[MacCatalyst (13, 1)]
		k2DMultisampleArray = 8,
		[MacCatalyst (13, 1)]
		kTextureBuffer = 9,
	}

	/// <summary>Options for calculating the pixel value returned within a mipmap level.</summary>
	[Native]
	public enum MTLSamplerMinMagFilter : ulong {
		Nearest,
		Linear,
	}

	/// <summary>Options for calculating the pixel value returned with multiple mipmap levels.</summary>
	[Native]
	public enum MTLSamplerMipFilter : ulong {
		NotMipmapped,
		Nearest,
		Linear,
	}

	/// <summary>The wrapping mode. Used when a fetch falls outside the bounds of a texture.</summary>
	[Native]
	public enum MTLSamplerAddressMode : ulong {
		ClampToEdge = 0,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		MirrorClampToEdge = 1,
		Repeat = 2,
		MirrorRepeat = 3,
		ClampToZero = 4,

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		ClampToBorderColor = 5,
	}

	/// <summary>Enumerates vertex data formats (type and number).</summary>
	[Native]
	public enum MTLVertexFormat : ulong {
		Invalid = 0,

		UChar2 = 1,
		UChar3 = 2,
		UChar4 = 3,

		Char2 = 4,
		Char3 = 5,
		Char4 = 6,

		UChar2Normalized = 7,
		UChar3Normalized = 8,
		UChar4Normalized = 9,

		Char2Normalized = 10,
		Char3Normalized = 11,
		Char4Normalized = 12,

		UShort2 = 13,
		UShort3 = 14,
		UShort4 = 15,

		Short2 = 16,
		Short3 = 17,
		Short4 = 18,

		UShort2Normalized = 19,
		UShort3Normalized = 20,
		UShort4Normalized = 21,

		Short2Normalized = 22,
		Short3Normalized = 23,
		Short4Normalized = 24,

		Half2 = 25,
		Half3 = 26,
		Half4 = 27,

		Float = 28,
		Float2 = 29,
		Float3 = 30,
		Float4 = 31,
		Int = 32,
		Int2 = 33,
		Int3 = 34,
		Int4 = 35,

		UInt = 36,
		UInt2 = 37,
		UInt3 = 38,
		UInt4 = 39,

		Int1010102Normalized = 40,
		UInt1010102Normalized = 41,

		[MacCatalyst (13, 1)]
		UChar4NormalizedBgra = 42,
		[MacCatalyst (13, 1)]
		UChar = 45,
		[MacCatalyst (13, 1)]
		Char = 46,
		[MacCatalyst (13, 1)]
		UCharNormalized = 47,

		[MacCatalyst (13, 1)]
		CharNormalized = 48,
		[MacCatalyst (13, 1)]
		UShort = 49,
		[MacCatalyst (13, 1)]
		Short = 50,
		[MacCatalyst (13, 1)]
		UShortNormalized = 51,
		[MacCatalyst (13, 1)]
		ShortNormalized = 52,

		[MacCatalyst (13, 1)]
		Half = 53,

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRG11B10 = 54,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRgb9E5 = 55,
	}

	/// <summary>The format used by pixels, including color, depth, compression technigue, and data type for components.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPixelFormat : ulong {
		Invalid = 0,
		A8Unorm = 1,
		R8Unorm = 10,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		R8Unorm_sRGB = 11,
		R8Snorm = 12,
		R8Uint = 13,
		R8Sint = 14,
		R16Unorm = 20,
		R16Snorm = 22,
		R16Uint = 23,
		R16Sint = 24,
		R16Float = 25,
		RG8Unorm = 30,
		[MacCatalyst (13, 1)]
		RG8Unorm_sRGB = 31,
		RG8Snorm = 32,
		RG8Uint = 33,
		RG8Sint = 34,
		[MacCatalyst (13, 1)]
		B5G6R5Unorm = 40,
		[MacCatalyst (13, 1)]
		A1BGR5Unorm = 41,
		[MacCatalyst (13, 1)]
		ABGR4Unorm = 42,
		[MacCatalyst (13, 1)]
		BGR5A1Unorm = 43,
		R32Uint = 53,
		R32Sint = 54,
		R32Float = 55,
		RG16Unorm = 60,
		RG16Snorm = 62,
		RG16Uint = 63,
		RG16Sint = 64,
		RG16Float = 65,
		RGBA8Unorm = 70,
		RGBA8Unorm_sRGB = 71,
		RGBA8Snorm = 72,
		RGBA8Uint = 73,
		RGBA8Sint = 74,
		BGRA8Unorm = 80,
		BGRA8Unorm_sRGB = 81,
		RGB10A2Unorm = 90,
		RGB10A2Uint = 91,
		RG11B10Float = 92,
		RGB9E5Float = 93,
		[MacCatalyst (13, 1)]
		BGR10A2Unorm = 94,
		RG32Uint = 103,
		RG32Sint = 104,
		RG32Float = 105,
		RGBA16Unorm = 110,
		RGBA16Snorm = 112,
		RGBA16Uint = 113,
		RGBA16Sint = 114,
		RGBA16Float = 115,
		RGBA32Uint = 123,
		RGBA32Sint = 124,
		RGBA32Float = 125,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC1RGBA = 130,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC1_RGBA_sRGB = 131,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC2RGBA = 132,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC2_RGBA_sRGB = 133,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC3RGBA = 134,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC3_RGBA_sRGB = 135,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC4_RUnorm = 140,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC4_RSnorm = 141,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC5_RGUnorm = 142,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC5_RGSnorm = 143,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC6H_RGBFloat = 150,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC6H_RGBUFloat = 151,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC7_RGBAUnorm = 152,
		[NoTV]
		[NoiOS]
		[NoMacCatalyst]
		BC7_RGBAUnorm_sRGB = 153,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_2BPP = 160,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_2BPP_sRGB = 161,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_4BPP = 162,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGB_4BPP_sRGB = 163,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_2BPP = 164,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_2BPP_sRGB = 165,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_4BPP = 166,

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use ASTC/ETC2/BC formats instead.")]
		PVRTC_RGBA_4BPP_sRGB = 167,

		EAC_R11Unorm = 170,
		EAC_R11Snorm = 172,
		EAC_RG11Unorm = 174,
		EAC_RG11Snorm = 176,
		EAC_RGBA8 = 178,
		EAC_RGBA8_sRGB = 179,
		ETC2_RGB8 = 180,
		ETC2_RGB8_sRGB = 181,
		ETC2_RGB8A1 = 182,
		ETC2_RGB8A1_sRGB = 183,


		ASTC_4x4_sRGB = 186,
		ASTC_5x4_sRGB = 187,
		ASTC_5x5_sRGB = 188,
		ASTC_6x5_sRGB = 189,
		ASTC_6x6_sRGB = 190,
		ASTC_8x5_sRGB = 192,
		ASTC_8x6_sRGB = 193,
		ASTC_8x8_sRGB = 194,
		ASTC_10x5_sRGB = 195,
		ASTC_10x6_sRGB = 196,
		ASTC_10x8_sRGB = 197,
		ASTC_10x10_sRGB = 198,
		ASTC_12x10_sRGB = 199,
		ASTC_12x12_sRGB = 200,

		ASTC_4x4_LDR = 204,
		ASTC_5x4_LDR = 205,
		ASTC_5x5_LDR = 206,
		ASTC_6x5_LDR = 207,
		ASTC_6x6_LDR = 208,
		ASTC_8x5_LDR = 210,
		ASTC_8x6_LDR = 211,
		ASTC_8x8_LDR = 212,
		ASTC_10x5_LDR = 213,
		ASTC_10x6_LDR = 214,
		ASTC_10x8_LDR = 215,
		ASTC_10x10_LDR = 216,
		ASTC_12x10_LDR = 217,
		ASTC_12x12_LDR = 218,

		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_4x4_HDR = 222,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_5x4_HDR = 223,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_5x5_HDR = 224,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_6x5_HDR = 225,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_6x6_HDR = 226,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x5_HDR = 228,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x6_HDR = 229,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_8x8_HDR = 230,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x5_HDR = 231,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x6_HDR = 232,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x8_HDR = 233,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_10x10_HDR = 234,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_12x10_HDR = 235,
		[iOS (13, 0)]
		[NoTV]
		[MacCatalyst (13, 1)]
		ASTC_12x12_HDR = 236,

		GBGR422 = 240,
		BGRG422 = 241,

		Depth16Unorm = 250,

		Depth32Float = 252,
		Stencil8 = 253,

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Depth24Unorm_Stencil8 = 255,

		[MacCatalyst (13, 1)]
		Depth32Float_Stencil8 = 260,

		[MacCatalyst (13, 1)]
		X32_Stencil8 = 261,

		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		X24_Stencil8 = 262,

		[MacCatalyst (13, 1)]
		BGRA10_XR = 552,
		[MacCatalyst (13, 1)]
		BGRA10_XR_sRGB = 553,
		[MacCatalyst (13, 1)]
		BGR10_XR = 554,
		[MacCatalyst (13, 1)]
		BGR10_XR_sRGB = 555,
	}

	/// <summary>The type of a Metal function.</summary>
	[Native]
	public enum MTLFunctionType : ulong {
		Vertex = 1,
		Fragment = 2,
		Kernel = 3,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		Visible = 5,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		Intersection = 6,
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		Mesh = 7,
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		Object = 8,
	}

	/// <summary>Enumerates errors possible in the creation of an MTLLibrary or MTLFunction object.</summary>
	[Native]
	[ErrorDomain ("MTLLibraryErrorDomain")]
	public enum MTLLibraryError : ulong {
		Unsupported = 1,
		Internal,
		CompileFailure,
		CompileWarning,
		FunctionNotFound,
		FileNotFound,
	}

#if !NET // this enum/error was removed from the headers a few years ago (the macOS 10.12 SDK has it, the 10.13 SDK doesn't)
	[Native]
	[ErrorDomain ("MTLRenderPipelineErrorDomain")]
	public enum MTLRenderPipelineError : ulong {
		Internal = 1,
		Unsupported,
		InvalidInput,
	}
#endif

	/// <summary>Holds a comparison test. When the comparison test passes, the incoming fragment is compared to the stored data at the specified location.</summary>
	[Native]
	public enum MTLCompareFunction : ulong {
		Never = 0,
		Less = 1,
		Equal = 2,
		LessEqual = 3,
		Greater = 4,
		NotEqual = 5,
		GreaterEqual = 6,
		Always = 7,
	}


	/// <summary>What to do if a stencil value passes or fails a comparison test.</summary>
	[Native]
	public enum MTLStencilOperation : ulong {
		Keep = 0,
		Zero = 1,
		Replace = 2,
		IncrementClamp = 3,
		DecrementClamp = 4,
		Invert = 5,
		IncrementWrap = 6,
		DecrementWrap = 7,
	}

	/// <summary>Enumerates the primitive geometry used for drawing commands.</summary>
	[Native]
	public enum MTLPrimitiveType : ulong {
		Point = 0,
		Line = 1,
		LineStrip = 2,
		Triangle = 3,
		TriangleStrip = 4,
	}

	/// <summary>The index type for an index buffer.</summary>
	[Native]
	public enum MTLIndexType : ulong {
		UInt16,
		UInt32,
	}

	/// <summary>Enumerates values that control how and whether to monitor samples that pass depth and stencil tests.</summary>
	[Native]
	public enum MTLVisibilityResultMode : ulong {
		Disabled = 0,
		Boolean = 1,
		Counting = 2,
	}

	/// <summary>Enumerates modes for culling and which types of primitives are culled.</summary>
	[Native]
	public enum MTLCullMode : ulong {
		None = 0,
		Front = 1,
		Back = 2,
	}

	/// <summary>Vertex winding rule for front-facing primitives.</summary>
	[Native]
	public enum MTLWinding : ulong {
		Clockwise = 0,
		CounterClockwise = 1,
	}

	/// <summary>How to rasterize triangle and triangle-strip primitives.</summary>
	[Native]
	public enum MTLTriangleFillMode : ulong {
		Fill,
		Lines,
	}

	/// <summary>The purgeable state of the resource.</summary>
	[Native]
	public enum MTLPurgeableState : ulong {
		KeepCurrent = 1,
		NonVolatile = 2,
		Volatile = 3,
		Empty = 4,
	}

	/// <summary>Enumerates what cache mode is used for CPU mapping of a resource.</summary>
	[Native]
	public enum MTLCpuCacheMode : ulong {
		DefaultCache,
		WriteCombined,
	}

	/// <summary>Enumerates the ways in which a <see cref="T:Metal.IMTLTexture" /> may be used.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLTextureUsage : ulong {
		Unknown = 0x0000,
		ShaderRead = 0x0001,
		ShaderWrite = 0x0002,
		RenderTarget = 0x0004,
#if !NET
		[Obsolete ("This option is unavailable.")]
		Blit = 0x0008,
#endif
		PixelFormatView = 0x0010,

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		ShaderAtomic = 0x20,
	}

	/// <summary>Holds options used by buffer and texture objects.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLResourceOptions : ulong {
		CpuCacheModeDefault = MTLCpuCacheMode.DefaultCache << 0,
		CpuCacheModeWriteCombined = MTLCpuCacheMode.WriteCombined << 0,

		[MacCatalyst (13, 1)]
		StorageModeShared = MTLStorageMode.Shared << 4,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		StorageModeManaged = MTLStorageMode.Managed << 4,
		[MacCatalyst (13, 1)]
		StorageModePrivate = MTLStorageMode.Private << 4,

		[NoMac]
		[MacCatalyst (13, 1)]
		StorageModeMemoryless = MTLStorageMode.Memoryless << 4,

		[MacCatalyst (13, 1)]
		HazardTrackingModeUntracked = 1 << 8,

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		HazardTrackingModeTracked = 1 << 9,
	}

	// MTLVertexDescriptor.h
	/// <summary>The frequency at which the vertex shader function should fetch attribute data.</summary>
	[Native]
	public enum MTLVertexStepFunction : ulong {
		Constant,
		PerVertex,
		PerInstance,
		[MacCatalyst (13, 1)]
		PerPatch = 3,
		[MacCatalyst (13, 1)]
		PerPatchControlPoint = 4,
	}

	/// <summary>The data type of a function argument, as used in the shader language code.</summary>
	[Native]
	public enum MTLDataType : ulong {

		None = 0,

		Struct = 1,
		Array = 2,

		Float = 3,
		Float2 = 4,
		Float3 = 5,
		Float4 = 6,

		Float2x2 = 7,
		Float2x3 = 8,
		Float2x4 = 9,

		Float3x2 = 10,
		Float3x3 = 11,
		Float3x4 = 12,

		Float4x2 = 13,
		Float4x3 = 14,
		Float4x4 = 15,

		Half = 16,
		Half2 = 17,
		Half3 = 18,
		Half4 = 19,

		Half2x2 = 20,
		Half2x3 = 21,
		Half2x4 = 22,

		Half3x2 = 23,
		Half3x3 = 24,
		Half3x4 = 25,

		Half4x2 = 26,
		Half4x3 = 27,
		Half4x4 = 28,

		Int = 29,
		Int2 = 30,
		Int3 = 31,
		Int4 = 32,

		UInt = 33,
		UInt2 = 34,
		UInt3 = 35,
		UInt4 = 36,

		Short = 37,
		Short2 = 38,
		Short3 = 39,
		Short4 = 40,

		UShort = 41,
		UShort2 = 42,
		UShort3 = 43,
		UShort4 = 44,

		Char = 45,
		Char2 = 46,
		Char3 = 47,
		Char4 = 48,

		UChar = 49,
		UChar2 = 50,
		UChar3 = 51,
		UChar4 = 52,

		Bool = 53,
		Bool2 = 54,
		Bool3 = 55,
		Bool4 = 56,
		[MacCatalyst (13, 1)]

		Texture = 58,
		[MacCatalyst (13, 1)]
		Sampler = 59,
		[MacCatalyst (13, 1)]
		Pointer = 60,
		[MacCatalyst (13, 1)]

		[NoMac, TV (14, 5)] R8Unorm = 62,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R8Snorm = 63,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R16Unorm = 64,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] R16Snorm = 65,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg8Unorm = 66,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg8Snorm = 67,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg16Unorm = 68,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg16Snorm = 69,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Unorm = 70,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Unorm_sRgb = 71,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba8Snorm = 72,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba16Unorm = 73,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgba16Snorm = 74,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgb10A2Unorm = 75,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rg11B10Float = 76,
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Rgb9E5Float = 77,
		[MacCatalyst (13, 1)]

		RenderPipeline = 78,
		[MacCatalyst (13, 1)]
		[iOS (13, 0), TV (13, 0)] ComputePipeline = 79,
		[MacCatalyst (13, 1)]
		IndirectCommandBuffer = 80,

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long = 81,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long2 = 82,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long3 = 83,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] Long4 = 84,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong = 85,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong2 = 86,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong3 = 87,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)] ULong4 = 88,
		[MacCatalyst (14, 0)]

		[iOS (14, 0), NoTV] VisibleFunctionTable = 115,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] IntersectionFunctionTable = 116,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] PrimitiveAccelerationStructure = 117,
		[MacCatalyst (14, 0)]
		[iOS (14, 0), NoTV] InstanceAccelerationStructure = 118,

		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat = 121,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat2 = 122,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat3 = 123,
		[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		BFloat4 = 124,
	}

	/// <summary>The resource type of an argument of a function.</summary>
	[Native]
	public enum MTLArgumentType : ulong {
		/// <summary>To be added.</summary>
		Buffer = 0,
		/// <summary>To be added.</summary>
		ThreadgroupMemory = 1,
		/// <summary>To be added.</summary>
		Texture = 2,
		/// <summary>To be added.</summary>
		Sampler = 3,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]

		[NoMac, TV (14, 5)] ImageblockData = 16,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[NoMac, TV (14, 5)] Imageblock = 17,

		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		VisibleFunctionTable = 24,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		PrimitiveAccelerationStructure = 25,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		InstanceAccelerationStructure = 26,
		[iOS (14, 0)]
		[NoTV]
		[MacCatalyst (14, 0)]
		IntersectionFunctionTable = 27,
	}

#if !XAMCORE_5_0
	/// <summary>Function access restrictions of argument data in the shader code.</summary>
	[Deprecated (PlatformName.MacOSX, 14, 0)]
	[Deprecated (PlatformName.iOS, 17, 0)]
	[Deprecated (PlatformName.TvOS, 17, 0)]
	[Deprecated (PlatformName.MacCatalyst, 17, 0)]
	[Native]
	public enum MTLArgumentAccess : ulong {
		/// <summary>To be added.</summary>
		ReadOnly,
		/// <summary>To be added.</summary>
		ReadWrite,
		/// <summary>To be added.</summary>
		WriteOnly,
	}
#endif

	/// <summary>Options relating to argument information in the creation of a pipeline.</summary>
	[Native]
	[Flags]
	public enum MTLPipelineOption : ulong {
		None,
#if !XAMCORE_5_0
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'BindingInfo' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'BindingInfo' instead.")]
		ArgumentInfo = 1 << 0,
#endif
		BindingInfo = 1 << 0,
		BufferTypeInfo,
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
		FailOnBinaryArchiveMiss = 4,
	}

	/// <summary>Enumerates the hardware feature sets that are available on a device.</summary>
	[Native]
	public enum MTLFeatureSet : ulong {
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily1_v1 = 0,
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily1_v2 = 2,
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily2_v1 = 1,
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily2_v2 = 3,
#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif
		[NoTV, NoMac]
		iOS_GPUFamily3_v1 = 4,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v3 = 5,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v3 = 6,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v2 = 7,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v4 = 8,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v4 = 9,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v3 = 10,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily4_v1 = 11,

		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily1_v5 = 12,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily2_v5 = 13,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily3_v4 = 14,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily4_v2 = 15,
		[NoTV, NoMac, NoMacCatalyst]
		iOS_GPUFamily5_v1 = 16,

		[NoiOS, NoTV, NoMacCatalyst]
		macOS_GPUFamily1_v1 = 10000,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v2 = 10001,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_ReadWriteTextureTier2 = 10002,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v3 = 10003,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily1_v4 = 10004,

		[NoiOS, NoTV]
		[NoMacCatalyst]
		macOS_GPUFamily2_v1 = 10005,

#if XAMCORE_5_0
		[NoMacCatalyst]
#elif __MACCATALYST__
		[Obsolete ("Not available on the current platform.")]
#endif

		[NoiOS, NoMac]
		tvOS_GPUFamily1_v1 = 30000,

		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v2 = 30001,

		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v3 = 30002,

		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily2_v1 = 30003,

		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily1_v4 = 30004,
		[NoiOS, NoMac]
		[NoMacCatalyst]
		tvOS_GPUFamily2_v2 = 30005,
	}

	/// <summary>Enumerates Metal shader programming language versions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLLanguageVersion : ulong {
		[NoMac]
		[NoMacCatalyst]
		v1_0 = (1 << 16),
		v1_1 = (1 << 16) + 1,
		[MacCatalyst (13, 1)]
		v1_2 = (1 << 16) + 2,
		[MacCatalyst (13, 1)]
		v2_0 = (2 << 16),
		[MacCatalyst (13, 1)]
		v2_1 = (2 << 16) + 1,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		v2_2 = (2 << 16) + 2,
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		v2_3 = (2 << 16) + 3,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		v2_4 = (2uL << 16) + 4,
		[iOS (16, 0), TV (16, 0), MacCatalyst (16, 0), Mac (13, 0)]
		v3_0 = (3uL << 16) + 0,
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		v3_1 = (3uL << 16) + 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		v3_2 = (3ul << 16) + 2,
	}

	/// <summary>Enumerates values that indicate whether to clip or clamp depth values.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLDepthClipMode : ulong {
		Clip = 0,
		Clamp = 1,
	}

	/// <summary>Enumerates blitting options.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum MTLBlitOption : ulong {
		None = 0,
		DepthFromDepthStencil = 1 << 0,
		StencilFromDepthStencil = 1 << 1,
		[NoMac]
		[MacCatalyst (13, 1)]
		RowLinearPvrtc = 1 << 2,
	}

	/// <summary>Enumerates CPU cache modes for resource mapping.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLStorageMode : ulong {
		Shared = 0,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Managed = 1,
		Private = 2,
		[NoMac]
		[MacCatalyst (13, 1)]
		Memoryless = 3,
	}

	/// <summary>Enumerates MSAA dept resolve filter operations.</summary>
	[Native]
	public enum MTLMultisampleDepthResolveFilter : ulong {
		Sample0,
		Min,
		Max,
	}

	[TV (16, 0)]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLSamplerBorderColor : ulong {
		TransparentBlack = 0,
		OpaqueBlack = 1,
		OpaqueWhite = 2,
	}

	[TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPrimitiveTopologyClass : ulong {
		Unspecified = 0,
		Point = 1,
		Line = 2,
		Triangle = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationPartitionMode : ulong {
		Pow2 = 0,
		Integer = 1,
		FractionalOdd = 2,
		FractionalEven = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationFactorFormat : ulong {
		Half = 0,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationControlPointIndexType : ulong {
		None = 0,
		UInt16 = 1,
		UInt32 = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLTessellationFactorStepFunction : ulong {
		Constant = 0,
		PerPatch = 1,
		PerInstance = 2,
		PerPatchAndPerInstance = 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLPatchType : ulong {
		None = 0,
		Triangle = 1,
		Quad = 2,
	}

	/// <summary>Enumerates formats for function attributes.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLAttributeFormat : ulong {
		/// <summary>Indicates an invalid format.</summary>
		Invalid = 0,
		/// <summary>Indicates two unsigned 8-bit characters.</summary>
		UChar2 = 1,
		/// <summary>Indicates three unsigned 8-bit characters.</summary>
		UChar3 = 2,
		/// <summary>Indicates four unsigned 8-bit characters..</summary>
		UChar4 = 3,
		/// <summary>Indicates two signed 8-bit characters.</summary>
		Char2 = 4,
		/// <summary>Indicates three signed 8-bit characters.</summary>
		Char3 = 5,
		/// <summary>Indicates four signed 8-bit characters.</summary>
		Char4 = 6,
		/// <summary>Indicates two normalized unsigned 8-bit characters.</summary>
		UChar2Normalized = 7,
		/// <summary>Indicates three normalized unsigned 8-bit characters.</summary>
		UChar3Normalized = 8,
		/// <summary>Indicates four normalized unsigned 8-bit characters.</summary>
		UChar4Normalized = 9,
		/// <summary>Indicates two normalized signed 8-bit characters.</summary>
		Char2Normalized = 10,
		/// <summary>Indicates three normalized signed 8-bit characters.</summary>
		Char3Normalized = 11,
		/// <summary>Indicates four normalized signed 8-bit characters.</summary>
		Char4Normalized = 12,
		/// <summary>Indicates two unsigned 16-bit two's complement values.</summary>
		UShort2 = 13,
		/// <summary>Indicates three unsigned 16-bit two's complement values.</summary>
		UShort3 = 14,
		/// <summary>Indicates four unsigned 16-bit two's complement values.</summary>
		UShort4 = 15,
		/// <summary>Indicates two signed 16-bit two's complement value.</summary>
		Short2 = 16,
		/// <summary>Indicates three signed 16-bit two's complement values.</summary>
		Short3 = 17,
		/// <summary>Indicates four signed 16-bit two's complement values.</summary>
		Short4 = 18,
		/// <summary>Indicates two normalized unsigned 16-bit two's complement values.</summary>
		UShort2Normalized = 19,
		/// <summary>Indicates three normalized unsigned 16-bit two's complement values.</summary>
		UShort3Normalized = 20,
		/// <summary>Indicates four normalized unsigned 16-bit two's complement values.</summary>
		UShort4Normalized = 21,
		/// <summary>Indicates two normalized signed 16-bit two's complement values.</summary>
		Short2Normalized = 22,
		/// <summary>Indicates three normalized signed 16-bit two's complement values.</summary>
		Short3Normalized = 23,
		/// <summary>Indicates four normalized signed 16-bit two's complement values.</summary>
		Short4Normalized = 24,
		/// <summary>Indicates two half-precision floating point values.</summary>
		Half2 = 25,
		/// <summary>Indicates three half-precision floating point values.</summary>
		Half3 = 26,
		/// <summary>Indicates four half-precision floating point values.</summary>
		Half4 = 27,
		/// <summary>Indicates one single-precision floating point value.</summary>
		Float = 28,
		/// <summary>Indicates two single-precision floating point values.</summary>
		Float2 = 29,
		/// <summary>Indicates three single-precision floating point values.</summary>
		Float3 = 30,
		/// <summary>Indicates four single-precision floating point values.</summary>
		Float4 = 31,
		/// <summary>Indicates a single signed 32-bit two's complement value.</summary>
		Int = 32,
		/// <summary>Indicates two signed 32-bit two's complement values.</summary>
		Int2 = 33,
		/// <summary>Indicates three signed 32-bit two's complement values.</summary>
		Int3 = 34,
		/// <summary>Indicates four signed 32-bit two's complement values.</summary>
		Int4 = 35,
		/// <summary>Indicates a single unsigned 32-bit two's complement value.</summary>
		UInt = 36,
		/// <summary>Indicates two unsigned 32-bit two's complement values.</summary>
		UInt2 = 37,
		/// <summary>Indicates three unsigned 32-bit two's complement values.</summary>
		UInt3 = 38,
		/// <summary>Indicates four unsigned 32-bit two's complement values.</summary>
		UInt4 = 39,
		/// <summary>Indicates a 10-10-10-2 scheme of packed normalized signed integers.</summary>
		Int1010102Normalized = 40,
		/// <summary>To be added.</summary>
		UInt1010102Normalized = 41,
		/// <summary>Indicates four unsigned 8-bit characters that describe BGRA channels.</summary>
		[MacCatalyst (13, 1)]
		UChar4Normalized_Bgra = 42,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UChar = 45,
		/// <summary>Indicates a single signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		Char = 46,
		/// <summary>Indicates a single unsigned 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		UCharNormalized = 47,
		/// <summary>Indicates a single normalized signed 8-bit character.</summary>
		[MacCatalyst (13, 1)]
		CharNormalized = 48,
		/// <summary>Indicates a single unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShort = 49,
		/// <summary>Indicates a single signed 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		Short = 50,
		/// <summary>Indicates a single normalized unsigned 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		UShortNormalized = 51,
		/// <summary>Indicates a single normalized signed 16-bit two's complement value.</summary>
		[MacCatalyst (13, 1)]
		ShortNormalized = 52,
		/// <summary>Indicates a single half-precision floating point value.</summary>
		[MacCatalyst (13, 1)]
		Half = 53,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRG11B10 = 54,
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		FloatRGB9E5 = 55,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLStepFunction : ulong {
		Constant = 0,
		PerVertex = 1,
		PerInstance = 2,
		PerPatch = 3,
		PerPatchControlPoint = 4,
		ThreadPositionInGridX = 5,
		ThreadPositionInGridY = 6,
		ThreadPositionInGridXIndexed = 7,
		ThreadPositionInGridYIndexed = 8,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLRenderStages : ulong {
		Vertex = (1 << 0),
		Fragment = (1 << 1),
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		Tile = (1uL << 2),

		[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		Object = (1uL << 3),
		[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		Mesh = (1uL << 4),
	}

	/// <summary>Enumerates resource usage patterns.</summary>
	[MacCatalyst (13, 1)]
	[Native, Flags]
	public enum MTLResourceUsage : ulong {
		Read = 1 << 0,
		Write = 1 << 1,
		Sample = 1 << 2,
	}

	/// <summary>Enumerates buffer mutability descriptions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLMutability : ulong {
		Default = 0,
		Mutable = 1,
		Immutable = 2,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLReadWriteTextureTier : ulong {
		None = 0,
		One = 1,
		Two = 2,
	}

	/// <summary>Enumerates argument buffer tiers.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLArgumentBuffersTier : ulong {
		/// <summary>Indicates tier one.</summary>
		One = 0,
		/// <summary>Indicates tier two.</summary>
		Two = 1,
	}

	/// <summary>Enumerates store action options.</summary>
	[MacCatalyst (13, 1)]
	[Native, Flags]
	public enum MTLStoreActionOptions : ulong {
		None = 0,
		CustomSamplePositions = 1 << 0,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLDispatchType : ulong {
		Serial,
		Concurrent,
	}

	[Flags]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLBarrierScope : ulong {
		/// <summary>To be added.</summary>
		Buffers = 1 << 0,
		/// <summary>To be added.</summary>
		Textures = 1 << 1,
		/// <summary>To be added.</summary>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		RenderTargets = 1 << 2,
	}

	[Flags]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLIndirectCommandType : ulong {
		Draw = 1 << 0,
		DrawIndexed = 1 << 1,
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		DrawPatches = 1 << 2,
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		DrawIndexedPatches = 1 << 3,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		ConcurrentDispatch = 1 << 5,
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		ConcurrentDispatchThreads = 1 << 6,
		[NoMac, iOS (17, 0), TV (18, 1), MacCatalyst (17, 0)]
		DrawMeshThreadgroups = (1uL << 7),
		[NoMac, iOS (17, 0), TV (18, 1), MacCatalyst (17, 0)]
		DrawMeshThreads = (1uL << 8),
	}

	[TV (14, 5)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLMultisampleStencilResolveFilter : ulong {
		Sample0 = 0,
		DepthResolvedSample = 1,
	}


	[Flags, TV (17, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLSparseTextureRegionAlignmentMode : ulong {
		Outward = 0x0,
		Inward = 0x1,
	}

	[Flags, TV (17, 0), iOS (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLSparseTextureMappingMode : ulong {
		Map = 0x0,
		Unmap = 0x1,
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLHazardTrackingMode : ulong {
		Default = 0,
		Untracked = 1,
		Tracked = 2,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("MTLCaptureErrorDomain")]
	public enum MTLCaptureError : long {
		NotSupported = 1,
		AlreadyCapturing,
		InvalidDescriptor,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLGpuFamily : long {
		Apple1 = 1001,
		Apple2 = 1002,
		Apple3 = 1003,
		Apple4 = 1004,
		Apple5 = 1005,
		[NoTV, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Apple6 = 1006,
		[NoTV, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Apple7 = 1007,
		Apple8 = 1008,
		[NoTV]
		Apple9 = 1009,
		Mac1 = 2001,
		Mac2 = 2002,
		Common1 = 3001,
		Common2 = 3002,
		Common3 = 3003,
		iOSMac1 = 4001,
		iOSMac2 = 4002,

		[iOS (16, 0), TV (16, 0), MacCatalyst (16, 0), Mac (13, 0)]
		Metal3 = 5001,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLHeapType : long {
		Automatic = 0,
		Placement = 1,
		[NoTV]
		[MacCatalyst (13, 1)]
		Sparse = 2,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum MTLCaptureDestination : long {
		DeveloperTools = 1,
		GpuTraceDocument,
	}

	[NoiOS, NoTV]
	[NoMacCatalyst]
	[Native]
	public enum MTLDeviceLocation : ulong {
		BuiltIn = 0,
		Slot = 1,
		External = 2,
		Unspecified = ulong.MaxValue,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	[ErrorDomain ("MTLCounterErrorDomain")]
	public enum MTLCounterSampleBufferError : long {
		OutOfMemory,
		Invalid = 1,
		Internal = 2,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	public enum MTLCommonCounter {
		[Field ("MTLCommonCounterTimestamp")]
		Timestamp,

		[Field ("MTLCommonCounterTessellationInputPatches")]
		TessellationInputPatches,

		[Field ("MTLCommonCounterVertexInvocations")]
		VertexInvocations,

		[Field ("MTLCommonCounterPostTessellationVertexInvocations")]
		PostTessellationVertexInvocations,

		[Field ("MTLCommonCounterClipperInvocations")]
		ClipperInvocations,

		[Field ("MTLCommonCounterClipperPrimitivesOut")]
		ClipperPrimitivesOut,

		[Field ("MTLCommonCounterFragmentInvocations")]
		FragmentInvocations,

		[Field ("MTLCommonCounterFragmentsPassed")]
		FragmentsPassed,

		[Field ("MTLCommonCounterComputeKernelInvocations")]
		ComputeKernelInvocations,

		[Field ("MTLCommonCounterTotalCycles")]
		TotalCycles,

		[Field ("MTLCommonCounterVertexCycles")]
		VertexCycles,

		[Field ("MTLCommonCounterTessellationCycles")]
		TessellationCycles,

		[Field ("MTLCommonCounterPostTessellationVertexCycles")]
		PostTessellationVertexCycles,

		[Field ("MTLCommonCounterFragmentCycles")]
		FragmentCycles,

		[Field ("MTLCommonCounterRenderTargetWriteCycles")]
		RenderTargetWriteCycles,

		[Field ("MTLCommonCounterSetTimestamp")]
		SetTimestamp,

		[Field ("MTLCommonCounterSetStageUtilization")]
		SetStageUtilization,

		[Field ("MTLCommonCounterSetStatistic")]
		SetStatistic,
	}

	[Flags, iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	public enum MTLAccelerationStructureInstanceOptions : uint {
		None = 0x0,
		DisableTriangleCulling = (1u << 0),
		TriangleFrontFacingWindingCounterClockwise = (1u << 1),
		Opaque = (1u << 2),
		NonOpaque = (1u << 3),
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Flags]
	[Native]
	public enum MTLAccelerationStructureUsage : ulong {
		None = 0x0,
		Refit = (1uL << 0),
		PreferFastBuild = (1uL << 1),
		[iOS (15, 0), MacCatalyst (15, 0)]
		ExtendedLimits = (1uL << 2),
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[ErrorDomain ("MTLBinaryArchiveDomain")]
	[Native]
	public enum MTLBinaryArchiveError : ulong {
		None = 0,
		InvalidFile = 1,
		UnexpectedElement = 2,
		CompilationFailure = 3,
		InternalError = 4,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Flags]
	[Native]
	public enum MTLCommandBufferErrorOption : ulong {
		None = 0x0,
		EncoderExecutionStatus = 1uL << 0,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLCommandEncoderErrorState : long {
		Unknown = 0,
		Completed = 1,
		Affected = 2,
		Pending = 3,
		Faulted = 4,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLCounterSamplingPoint : ulong {
		StageBoundary,
		DrawBoundary,
		DispatchBoundary,
		TileDispatchBoundary,
		BlitBoundary,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[ErrorDomain ("MTLDynamicLibraryDomain")]
	[Native]
	public enum MTLDynamicLibraryError : ulong {
		None = 0,
		InvalidFile = 1,
		CompilationFailure = 2,
		UnresolvedInstallName = 3,
		DependencyLoadFailure = 4,
		Unsupported = 5,
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLFunctionLogType : ulong {
		Validation = 0,
	}

	[Flags, iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLFunctionOptions : ulong {
		None = 0x0,
		[NoTV]
		[MacCatalyst (14, 0)]
		CompileToBinary = 1uL << 0,
		[iOS (17, 0), TV (17, 0), MacCatalyst (17, 0), Mac (14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'StoreFunctionInMetalPipelinesScript' instead.")]
		StoreFunctionInMetalScript = 1uL << 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		StoreFunctionInMetalPipelinesScript = 1 << 1,
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		FailOnBinaryArchiveMiss = 1 << 2,
	}

	[Flags, iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLIntersectionFunctionSignature : ulong {
		None = 0x0,
		Instancing = (1uL << 0),
		TriangleData = (1uL << 1),
		WorldSpaceData = (1uL << 2),
		[iOS (15, 0), MacCatalyst (15, 0)]
		InstanceMotion = (1uL << 3),
		[iOS (15, 0), MacCatalyst (15, 0)]
		PrimitiveMotion = (1uL << 4),
		[iOS (15, 0), MacCatalyst (15, 0)]
		ExtendedLimits = (1uL << 5),
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		MaxLevels = (1uL << 6),
		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		CurveData = (1uL << 7),
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum MTLLibraryType : long {
		Executable = 0,
		Dynamic = 1,
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	public enum MTLTextureSwizzle : byte {
		Zero = 0,
		One = 1,
		Red = 2,
		Green = 3,
		Blue = 4,
		Alpha = 5,
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
	public enum MTLMotionBorderMode : uint {
		Clamp = 0,
		Vanish = 1,
	}

	[iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
	[Native]
	public enum MTLAccelerationStructureInstanceDescriptorType : ulong {
		Default = 0,
		UserID = 1,
		Motion = 2,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
		Indirect = 3,
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
		IndirectMotion = 4,
	}

	[Mac (12, 5), iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
	[Native]
	public enum MTLTextureCompressionType : long {
		Lossless = 0,
		Lossy = 1,
	}

	[Flags, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLAccelerationStructureRefitOptions : ulong {
		VertexData = (1uL << 0),
		PerPrimitiveData = (1uL << 1),
	}

	[iOS (14, 0), MacCatalyst (14, 0), TV (14, 0)]
	[Native]
	public enum MTLBindingType : long {
		Buffer = 0,
		ThreadgroupMemory = 1,
		Texture = 2,
		Sampler = 3,
		ImageblockData = 16,
		Imageblock = 17,
		VisibleFunctionTable = 24,
		PrimitiveAccelerationStructure = 25,
		InstanceAccelerationStructure = 26,
		IntersectionFunctionTable = 27,
		ObjectPayload = 34,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCommandQueueType : long {
		Concurrent = 0,
		Serial = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCompressionMethod : long {
		Zlib = 0,
		Lzfse = 1,
		Lz4 = 2,
		Lzma = 3,
		LzBitmap = 4,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOCompressionStatus : long {
		Complete = 0,
		Error = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	[ErrorDomain ("MTLIOErrorDomain")]
	public enum MTLIOError : long {
		UrlInvalid = 1,
		Internal = 2,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOPriority : long {
		High = 0,
		Normal = 1,
		Low = 2,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLIOStatus : long {
		Pending = 0,
		Cancelled = 1,
		Error = 2,
		Complete = 3,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLLibraryOptimizationLevel : long {
		Default = 0,
		Size = 1,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLSparsePageSize : long {
		Size16 = 101,
		Size64 = 102,
		Size256 = 103,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Native]
	public enum MTLBindingAccess : long {
		ReadOnly = 0,
		ReadWrite = 1,
		WriteOnly = 2,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveType : long {
		Round = 0,
		Flat = 1,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveBasis : long {
		BSpline = 0,
		CatmullRom = 1,
		Linear = 2,
		Bezier = 3,
	}

	[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
	[Native]
	public enum MTLCurveEndCaps : long {
		None = 0,
		Disk = 1,
		Sphere = 2,
	}

	[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), TV (16, 4)]
	[Native]
	public enum MTLCompileSymbolVisibility : long {
		Default = 0,
		Hidden = 1,
	}

}
