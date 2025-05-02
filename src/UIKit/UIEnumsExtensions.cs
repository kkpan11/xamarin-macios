//
// UIEnums.cs:
//
// Copyright 2009-2011 Novell, Inc.
// Copyright 2011-2012, Xamarin Inc.
//
// Author:
//  Miguel de Icaza
//

using System;
using Foundation;
using ObjCRuntime;

namespace UIKit {

#if IOS
	/// <summary>Extension methods for the <see cref="UIKit.UIDeviceOrientation" /> class.</summary>
	///     <remarks>To be added.</remarks>
	public static class UIDeviceOrientationExtensions {
		/// <param name="orientation">To be added.</param>
		///         <summary>Returns <see langword="true" /> if one of the short edges is lowest.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool IsPortrait (this UIDeviceOrientation orientation)
		{
			return orientation == UIDeviceOrientation.PortraitUpsideDown ||
				orientation == UIDeviceOrientation.Portrait;
		}

		/// <param name="orientation">To be added.</param>
		///         <summary>Returns <see langword="true" /> if one of the long edges of the device is lowest.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool IsLandscape (this UIDeviceOrientation orientation)
		{
			return orientation == UIDeviceOrientation.LandscapeRight || orientation == UIDeviceOrientation.LandscapeLeft;
		}

		/// <param name="orientation">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the device is lying on either its back or face.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool IsFlat (this UIDeviceOrientation orientation)
		{
			return orientation == UIDeviceOrientation.FaceUp || orientation == UIDeviceOrientation.FaceDown;
		}
	}

	/// <summary>Extension methods for the UIInterfaceOrientation enumeration.</summary>
	///     <remarks>
	///       <para>The extension methods in this class provide some
	///       convenience methods to use with enumerations of the
	///       UIInterfaceOrientation type.
	///       </para>
	///     </remarks>
	public static class UIInterfaceOrientationExtensions {
		/// <param name="orientation">The value to operate on.</param>
		///         <summary>Determines if the orientation is one of the portrait orientations.</summary>
		///         <returns>true if this is a portrait orientation.</returns>
		///         <remarks>
		///         </remarks>
		public static bool IsPortrait (this UIInterfaceOrientation orientation)
		{
			return orientation == UIInterfaceOrientation.PortraitUpsideDown ||
				orientation == UIInterfaceOrientation.Portrait;
		}
		/// <param name="orientation">The value to operate on.</param>
		///         <summary>Determines if the origination is one of the landscape
		///         values.</summary>
		///         <returns>true if this is a landscape orientation.</returns>
		///         <remarks>
		///         </remarks>
		public static bool IsLandscape (this UIInterfaceOrientation orientation)
		{
			return orientation == UIInterfaceOrientation.LandscapeRight ||
				orientation == UIInterfaceOrientation.LandscapeLeft;
		}
	}
#endif // IOS

#if __MACCATALYST__
	public static class UIImageResizingModeExtensions {
		public static nint ToNative (UIImageResizingMode value)
		{
			// The values we have in managed code corresponds with the ARM64 values.
			if (!Runtime.IsARM64CallingConvention) {
				// Stretch and Tile are switched on x64 on Mac Catalyst
				switch (value) {
				case (UIImageResizingMode) 0:
					return 1;
				case (UIImageResizingMode) 1:
					return 0;
				}
			}
			return (nint) (long) value;
		}

		public static UIImageResizingMode ToManaged (nint value)
		{
			// The values we have in managed code corresponds with the ARM64 values.
			if (!Runtime.IsARM64CallingConvention) {
				// Stretch and Tile are switched on x64 on Mac Catalyst
				switch (value) {
				case 0:
					return (UIImageResizingMode) 1;
				case 1:
					return (UIImageResizingMode) 0;
				}
			}
			return (UIImageResizingMode) (long) value;
		}
	}

	public static class UITextAlignmentExtensions {
		public static nint ToNative (UITextAlignment value)
		{
			// The values we have in managed code corresponds with the ARM64 values.
			if (!Runtime.IsARM64CallingConvention) {
				// Center and Right are switched on x64 on Mac Catalyst
				switch (value) {
				case (UITextAlignment) 1:
					value = (UITextAlignment) 2;
					break;
				case (UITextAlignment) 2:
					value = (UITextAlignment) 1;
					break;
				}
			}
			return (nint) (long) value;
		}

		public static UITextAlignment ToManaged (nint value)
		{
			// The values we have in managed code corresponds with the ARM64 values.
			if (!Runtime.IsARM64CallingConvention) {
				// Center and Right are switched on x64 on Mac Catalyst
				switch (value) {
				case 1:
					value = (nint) 2;
					break;
				case 2:
					value = (nint) 1;
					break;
				}
			}
			return (UITextAlignment) (long) value;
		}
	}
#endif // __MACCATALYST__
}

