//
// GCExtendedGamepadSnapshot.cs: extensions to GCExtendedGamepadSnapshot iOS API
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//   TJ Lambert (t-anlamb@microsoft.com)
//
// Copyright 2013-2014 Xamarin Inc.
// Copyright 2019 Microsoft Corporation

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;

namespace GameController {
	// GCExtendedGamepadSnapshot.h
	// float_t are 4 bytes (at least for ARM64)
	/// <summary>The state of a <see cref="GameController.GCExtendedGamepad" />. Produced by <see cref="GameController.GCExtendedGamepadSnapshot.TryGetSnapShotData(Foundation.NSData,out GameController.GCExtendedGamepadSnapShotDataV100)" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("macos10.14.4", "Use 'GCExtendedGamepadSnapshotData' instead.")]
	[ObsoletedOSPlatform ("tvos12.2", "Use 'GCExtendedGamepadSnapshotData' instead.")]
	[ObsoletedOSPlatform ("ios12.2", "Use 'GCExtendedGamepadSnapshotData' instead.")]
	[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
	[StructLayout (LayoutKind.Sequential, Pack = 1)]

	public struct GCExtendedGamepadSnapShotDataV100 {

		// Standard information
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Version; // 0x0100
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Size;    // sizeof(GCExtendedGamepadSnapShotDataV100) or larger

		// Extended gamepad data
		// Axes in the range [-1.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonA;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonB;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftShoulder;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightShoulder;

		// Axes in the range [-1.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftThumbstickX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftThumbstickY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightThumbstickX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightThumbstickY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftTrigger;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightTrigger;

		// radar: https://trello.com/c/7FoGTORD (GCExtendedGamepadSnapShotDataV100 struct size / alignment not backward compatible)
		// public bool LeftThumbstickButton;
		// public bool RightThumbstickButton;

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern /* NSData * __nullable */ IntPtr NSDataFromGCExtendedGamepadSnapShotDataV100 (
			/* GCExtendedGamepadSnapShotDataV100 * __nullable */ GCExtendedGamepadSnapShotDataV100* snapshotData);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? ToNSData ()
		{
			unsafe {
				fixed (GCExtendedGamepadSnapShotDataV100* self = &this) {
					var p = NSDataFromGCExtendedGamepadSnapShotDataV100 (self);
					return p == IntPtr.Zero ? null : new NSData (p);
				}
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
	// float_t are 4 bytes (at least for ARM64)
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	public struct GCExtendedGamepadSnapshotData {

		// Standard information
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Version;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ushort /* uint16_t */ Size;

		// Extended gamepad data
		// Axes in the range [-1.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ DPadY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonA;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonB;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ ButtonY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftShoulder;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightShoulder;

		// Axes in the range [-1.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftThumbstickX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftThumbstickY;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightThumbstickX;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightThumbstickY;

		// Buttons in the range [0.0, 1.0]
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ LeftTrigger;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float /* float_t = float */ RightTrigger;

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
#if XAMCORE_5_0
		byte supportsClickableThumbsticks;
		public bool SupportsClickableThumbsticks {
			get => supportsClickableThumbsticks != 0;
			set => supportsClickableThumbsticks = value.AsByte ();
		}
#else
		[MarshalAs (UnmanagedType.I1)]
		public bool SupportsClickableThumbsticks;
#endif

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		byte LeftThumbstickButton;

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		byte RightThumbstickButton;

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern /* NSData * __nullable */ IntPtr NSDataFromGCExtendedGamepadSnapshotData (
#if XAMCORE_5_0
			/* GCExtendedGamepadSnapshotData * __nullable */ GCExtendedGamepadSnapshotData* snapshotData);
#else
			/* GCExtendedGamepadSnapshotData * __nullable */ GCExtendedGamepadSnapshotData_Blittable* snapshotData);
#endif

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		public NSData? ToNSData ()
		{
			unsafe {
#if !XAMCORE_5_0
				var blittable = ToBlittable ();
				GCExtendedGamepadSnapshotData_Blittable* self = &blittable;
				{
#else
				fixed (GCExtendedGamepadSnapshotData* self = &this) {
#endif
					var p = NSDataFromGCExtendedGamepadSnapshotData (self);
					return p == IntPtr.Zero ? null : new NSData (p);
				}
			}
		}

#if !XAMCORE_5_0
		internal GCExtendedGamepadSnapshotData_Blittable ToBlittable ()
		{
			var blittable = new GCExtendedGamepadSnapshotData_Blittable ();
			blittable.Version = Version;
			blittable.Size = Size;
			blittable.DPadX = DPadX;
			blittable.DPadY = DPadY;
			blittable.ButtonA = ButtonA;
			blittable.ButtonB = ButtonB;
			blittable.ButtonX = ButtonX;
			blittable.ButtonY = ButtonY;
			blittable.LeftShoulder = LeftShoulder;
			blittable.RightShoulder = RightShoulder;
			blittable.LeftThumbstickX = LeftThumbstickX;
			blittable.LeftThumbstickY = LeftThumbstickY;
			blittable.RightThumbstickX = RightThumbstickX;
			blittable.RightThumbstickY = RightThumbstickY;
			blittable.LeftTrigger = LeftTrigger;
			blittable.RightTrigger = RightTrigger;
			blittable.SupportsClickableThumbsticks = SupportsClickableThumbsticks.AsByte ();
			blittable.LeftThumbstickButton = LeftThumbstickButton;
			blittable.RightThumbstickButton = RightThumbstickButton;
			return blittable;
		}

		internal GCExtendedGamepadSnapshotData (GCExtendedGamepadSnapshotData_Blittable blittable)
		{
			Version = blittable.Version;
			Size = blittable.Size;
			DPadX = blittable.DPadX;
			DPadY = blittable.DPadY;
			ButtonA = blittable.ButtonA;
			ButtonB = blittable.ButtonB;
			ButtonX = blittable.ButtonX;
			ButtonY = blittable.ButtonY;
			LeftShoulder = blittable.LeftShoulder;
			RightShoulder = blittable.RightShoulder;
			LeftThumbstickX = blittable.LeftThumbstickX;
			LeftThumbstickY = blittable.LeftThumbstickY;
			RightThumbstickX = blittable.RightThumbstickX;
			RightThumbstickY = blittable.RightThumbstickY;
			LeftTrigger = blittable.LeftTrigger;
			RightTrigger = blittable.RightTrigger;
			SupportsClickableThumbsticks = blittable.SupportsClickableThumbsticks != 0;
			LeftThumbstickButton = blittable.LeftThumbstickButton;
			RightThumbstickButton = blittable.RightThumbstickButton;
		}
#endif
	}


#if !XAMCORE_5_0
	[StructLayout (LayoutKind.Sequential, Pack = 1)]
	struct GCExtendedGamepadSnapshotData_Blittable {
		public ushort Version;
		public ushort Size;
		public float DPadX;
		public float DPadY;
		public float ButtonA;
		public float ButtonB;
		public float ButtonX;
		public float ButtonY;
		public float LeftShoulder;
		public float RightShoulder;
		public float LeftThumbstickX;
		public float LeftThumbstickY;
		public float RightThumbstickX;
		public float RightThumbstickY;
		public float LeftTrigger;
		public float RightTrigger;
		public byte SupportsClickableThumbsticks;
		public byte LeftThumbstickButton;
		public byte RightThumbstickButton;
	}
#endif // !XAMORE_5_0

	public partial class GCExtendedGamepadSnapshot {

		// GCExtendedGamepadSnapshot.h
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern byte GCExtendedGamepadSnapShotDataV100FromNSData (
			/* GCExtendedGamepadSnapShotDataV100 * __nullable */ GCExtendedGamepadSnapShotDataV100* snapshotData,
			/* NSData * __nullable */ IntPtr data);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1")]
		[DllImport (Constants.GameControllerLibrary)]
		unsafe static extern byte GCExtendedGamepadSnapshotDataFromNSData (
#if XAMCORE_5_0
			/* GCExtendedGamepadSnapshotData * __nullable */ GCExtendedGamepadSnapshotData* snapshotData,
#else
			/* GCExtendedGamepadSnapshotData * __nullable */ GCExtendedGamepadSnapshotData_Blittable* snapshotData,
#endif
			/* NSData * __nullable */ IntPtr data);

		/// <param name="data">To be added.</param>
		/// <param name="snapshotData">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		[ObsoletedOSPlatform ("tvos", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		[ObsoletedOSPlatform ("ios", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'GCExtendedGamepadSnapshotData' instead.")]
		public static bool TryGetSnapShotData (NSData? data, out GCExtendedGamepadSnapShotDataV100 snapshotData)
		{
			snapshotData = default;
			unsafe {
				bool result = GCExtendedGamepadSnapShotDataV100FromNSData ((GCExtendedGamepadSnapShotDataV100*) Unsafe.AsPointer<GCExtendedGamepadSnapShotDataV100> (ref snapshotData), data.GetHandle ()) != 0;
				GC.KeepAlive (data);
				return result;
			}
		}

		/// <param name="data">To be added.</param>
		///         <param name="snapshotData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.1")]
		public static bool TryGetExtendedSnapShotData (NSData? data, out GCExtendedGamepadSnapshotData snapshotData)
		{
			snapshotData = default;
			unsafe {
#if XAMCORE_5_0
				return GCExtendedGamepadSnapshotDataFromNSData ((GCExtendedGamepadSnapshotData*) Unsafe.AsPointer<GCExtendedGamepadSnapshotData> (ref snapshotData), data.GetHandle ()) != 0;
#else
				GCExtendedGamepadSnapshotData_Blittable blittableData = snapshotData.ToBlittable ();
				var rv = GCExtendedGamepadSnapshotDataFromNSData (&blittableData, data.GetHandle ()) != 0;
				GC.KeepAlive (data);
				snapshotData = new GCExtendedGamepadSnapshotData (blittableData);
				return rv;
#endif
			}
		}
	}
}
