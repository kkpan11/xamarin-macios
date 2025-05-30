//
// gamecontroller.cs: binding for iOS (7+) GameController framework
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//   TJ Lambert (antlambe@microsoft.com)
//   Whitney Schmidt (whschm@microsoft.com)
//
// Copyright 2013, 2015 Xamarin Inc.
// Copyright 2019, 2020 Microsoft Corporation

using System;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
#if MONOMAC
using AppKit;
using UIViewController = AppKit.NSViewController;
using CHHapticEngine = Foundation.NSObject;
using BezierPath = AppKit.NSBezierPath;
using UIScene = Foundation.NSObject;
using UISceneConnectionOptions = Foundation.NSObject;
using UIInteraction = Foundation.NSObject;
#else
using CoreHaptics;
using UIKit;
using BezierPath = UIKit.UIBezierPath;
#endif

namespace GameController {

	[Flags]
	[Native]
	public enum GCPhysicalInputSourceDirection : ulong {
		NotApplicable = 0x0,
		Up = (1uL << 0),
		Right = (1uL << 1),
		Down = (1uL << 2),
		Left = (1uL << 3),
	}

	/// <summary>The base class for input elements of a game controller.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCControllerElement_Ref/index.html">Apple documentation for <c>GCControllerElement</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // The GCControllerElement class is never instantiated directly.
	partial interface GCControllerElement {

		// NOTE: ArgumentSemantic.Weak if ARC, ArgumentSemantic.Assign otherwise;
		// currently MonoTouch is not ARC, neither is Xammac, so go with assign.
		/// <summary>The <see cref="GameController.GCControllerElement" /> that <c>this</c> is a part of.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>If <c>this</c> is an element of another <see cref="GameController.GCControllerElement" />, this will hold the "parent" <see cref="GameController.GCControllerElement" />. (The D-Pad can be read as either a pair of <see cref="GameController.GCControllerAxisInput" /> elements or as four <see cref="GameController.GCControllerButtonInput" /> elements.)</para>
		///         </remarks>
		[NullAllowed]
		[Export ("collection", ArgumentSemantic.Assign)]
		GCControllerElement Collection { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("analog")]
		bool IsAnalog { [Bind ("isAnalog")] get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("sfSymbolsName", ArgumentSemantic.Strong)]
		string SfSymbolsName { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("localizedName", ArgumentSemantic.Strong)]
		string LocalizedName { get; set; }

		[TV (14, 2), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("unmappedSfSymbolsName", ArgumentSemantic.Strong)]
		string UnmappedSfSymbolsName { get; set; }

		[TV (14, 2), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("unmappedLocalizedName", ArgumentSemantic.Strong)]
		string UnmappedLocalizedName { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("aliases")]
		NSSet<NSString> Aliases { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("boundToSystemGesture")]
		bool IsBoundToSystemGesture { [Bind ("isBoundToSystemGesture")] get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("preferredSystemGestureState", ArgumentSemantic.Assign)]
		GCSystemGestureState PreferredSystemGestureState { get; set; }
	}

	/// <summary>The delegate used as the value-changed handler for <see cref="GameController.GCControllerAxisInput.ValueChangedHandler" />.</summary>
	delegate void GCControllerAxisValueChangedHandler (GCControllerAxisInput axis, float /* float, not CGFloat */ value);

	/// <summary>A <see cref="GameController.GCControllerElement" /> representing a joystick.</summary>
	///     
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCControllerAxisInput_Ref/index.html">Apple documentation for <c>GCControllerAxisInput</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (GCControllerElement))]
	[DisableDefaultCtor] // return nil handle -> only exposed as getter
	partial interface GCControllerAxisInput {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCControllerAxisValueChangedHandler ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value")]
		float Value {  /* float, not CGFloat */
			get;
			[iOS (13, 0)]
			[TV (13, 0)]
			[MacCatalyst (13, 1)]
			set;
		}
	}

	/// <summary>Handler that can be passed to the <see cref="GameController.GCControllerButtonInput.SetPressedChangedHandler(GameController.GCControllerButtonValueChanged)" /> method to respond to changes to button states.</summary>
	delegate void GCControllerButtonValueChanged (GCControllerButtonInput button, float /* float, not CGFloat */ buttonValue, bool pressed);
	delegate void GCControllerButtonTouchedChanged (GCControllerButtonInput button, float value, bool pressed, bool touched);

	/// <summary>A <see cref="GameController.GCControllerElement" /> representing a game-controller button.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCControllerButtonInput_Ref/index.html">Apple documentation for <c>GCControllerButtonInput</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (GCControllerElement))]
	[DisableDefaultCtor] // return nil handle -> only exposed as getter
	partial interface GCControllerButtonInput {
		/// <summary>Handler that is called when the button pressure changes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCControllerButtonValueChanged ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value")]
		float Value {  /* float, not CGFloat */
			get;
			[iOS (13, 0)]
			[TV (13, 0)]
			[MacCatalyst (13, 1)]
			set;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pressed")]
		bool IsPressed { [Bind ("isPressed")] get; }

		/// <summary>Handler that is called when the button press state changes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("pressedChangedHandler", ArgumentSemantic.Copy)]
		GCControllerButtonValueChanged PressedChangedHandler { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("touchedChangedHandler", ArgumentSemantic.Copy)]
		GCControllerButtonTouchedChanged TouchedChangedHandler { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("touched")]
		bool Touched { [Bind ("isTouched")] get; }
	}

	/// <summary>The delegate used as the value-changed handler for <see cref="GameController.GCControllerDirectionPad.ValueChangedHandler" />.</summary>
	delegate void GCControllerDirectionPadValueChangedHandler (GCControllerDirectionPad dpad, float /* float, not CGFloat */ xValue, float /* float, not CGFloat */ yValue);

	/// <summary>A <see cref="GameController.GCControllerElement" /> representing a direction-pad.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCControllerDirectionPad_Ref/index.html">Apple documentation for <c>GCControllerDirectionPad</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (GCControllerElement))]
	[DisableDefaultCtor] // return nil handle -> only exposed as getter
	partial interface GCControllerDirectionPad {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCControllerDirectionPadValueChangedHandler ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("xAxis")]
		GCControllerAxisInput XAxis { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("yAxis")]
		GCControllerAxisInput YAxis { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("up")]
		GCControllerButtonInput Up { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("down")]
		GCControllerButtonInput Down { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("left")]
		GCControllerButtonInput Left { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("right")]
		GCControllerButtonInput Right { get; }

		[iOS (13, 0)]
		[TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setValueForXAxis:yAxis:")]
		void SetValue (float xAxis, float yAxis);
	}

	/// <summary>The delegate used as the value-changed handler for <see cref="GameController.GCGamepad.ValueChangedHandler" />.</summary>
	delegate void GCGamepadValueChangedHandler (GCGamepad gamepad, GCControllerElement element);

	/// <summary>A gamepad with two shoulder buttons, a D-Pad, and a directional button array..</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCGamePad_Ref/index.html">Apple documentation for <c>GCGamepad</c></related>
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'GCExtendedGamepad' instead.")]
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'GCExtendedGamepad' instead.")]
	[Deprecated (PlatformName.TvOS, 10, 0, message: "Use 'GCExtendedGamepad' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCExtendedGamepad' instead.")]
	[BaseType (typeof (GCPhysicalInputProfile))]
	[DisableDefaultCtor] // return nil handle -> only exposed as getter
	partial interface GCGamepad {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controller", ArgumentSemantic.Assign)]
		GCController Controller { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCGamepadValueChangedHandler ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("saveSnapshot")]
		GCGamepadSnapshot SaveSnapshot { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dpad")]
		GCControllerDirectionPad DPad { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonA")]
		GCControllerButtonInput ButtonA { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonB")]
		GCControllerButtonInput ButtonB { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonX")]
		GCControllerButtonInput ButtonX { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonY")]
		GCControllerButtonInput ButtonY { get; }

		/// <summary>To be added.</summary>
		///         <value>A gamepad with two shoulder buttons, a D-Pad, and a directional button array.</value>
		///         <remarks>To be added.</remarks>
		[Export ("leftShoulder")]
		GCControllerButtonInput LeftShoulder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rightShoulder")]
		GCControllerButtonInput RightShoulder { get; }
	}

	/// <summary>A serializable snapshot of the game controller's state.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCGamePadSnapshot_Ref/index.html">Apple documentation for <c>GCGamepadSnapshot</c></related>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCExtendedGamepad' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCExtendedGamepad' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCExtendedGamepad' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCExtendedGamepad' instead.")]
	[BaseType (typeof (GCGamepad))]
	[DisableDefaultCtor]
	partial interface GCGamepadSnapshot {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("snapshotData", ArgumentSemantic.Copy)]
		NSData SnapshotData { get; set; }

		/// <param name="data">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithSnapshotData:")]
		NativeHandle Constructor (NSData data);

		/// <param name="controller">To be added.</param>
		/// <param name="data">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithController:snapshotData:")]
		NativeHandle Constructor (GCController controller, NSData data);
	}

	/// <summary>The delegate used as the value-changed handler for <see cref="GameController.GCExtendedGamepad.ValueChangedHandler" />.</summary>
	delegate void GCExtendedGamepadValueChangedHandler (GCExtendedGamepad gamepad, GCControllerElement element);

	/// <summary>A gamepad with two shoulder buttons, two triggers, two thumbsticks, a D-Pad, and a directional button array.</summary>
	///     <remarks>
	///       <para>Application developers should not instantiate this class. Rather, they should use the instance read from the <see cref="GameController.GCController.ExtendedGamepad" /> property.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCExtendedGamePad_Ref/index.html">Apple documentation for <c>GCExtendedGamepad</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (GCPhysicalInputProfile))]
	[DisableDefaultCtor] // return nil handle -> only exposed as getter
	partial interface GCExtendedGamepad {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controller", ArgumentSemantic.Assign)]
		[NullAllowed]
		GCController Controller { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCExtendedGamepadValueChangedHandler ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.Capture()' instead.")]
		[Export ("saveSnapshot")]
		GCExtendedGamepadSnapshot SaveSnapshot ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dpad")]
		GCControllerDirectionPad DPad { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonA")]
		GCControllerButtonInput ButtonA { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonB")]
		GCControllerButtonInput ButtonB { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonX")]
		GCControllerButtonInput ButtonX { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonY")]
		GCControllerButtonInput ButtonY { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("leftThumbstick")]
		GCControllerDirectionPad LeftThumbstick { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rightThumbstick")]
		GCControllerDirectionPad RightThumbstick { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("leftShoulder")]
		GCControllerButtonInput LeftShoulder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rightShoulder")]
		GCControllerButtonInput RightShoulder { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("leftTrigger")]
		GCControllerButtonInput LeftTrigger { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rightTrigger")]
		GCControllerButtonInput RightTrigger { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("leftThumbstickButton")]
		GCControllerButtonInput LeftThumbstickButton { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("rightThumbstickButton")]
		GCControllerButtonInput RightThumbstickButton { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("buttonMenu")]
		GCControllerButtonInput ButtonMenu { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("buttonOptions")]
		GCControllerButtonInput ButtonOptions { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setStateFromExtendedGamepad:")]
		void SetState (GCExtendedGamepad extendedGamepad);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("buttonHome")]
		GCControllerButtonInput ButtonHome { get; }
	}

	/// <summary>A serializable snapshot of the game controller's state.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCExtendedGamePadSnapshot_Ref/index.html">Apple documentation for <c>GCExtendedGamepadSnapshot</c></related>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
	[BaseType (typeof (GCExtendedGamepad))]
	[DisableDefaultCtor]
	partial interface GCExtendedGamepadSnapshot {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("snapshotData", ArgumentSemantic.Copy)]
		NSData SnapshotData { get; set; }

		/// <param name="data">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithSnapshotData:")]
		NativeHandle Constructor (NSData data);

		/// <param name="controller">To be added.</param>
		/// <param name="data">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithController:snapshotData:")]
		NativeHandle Constructor (GCController controller, NSData data);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.GetExtendedGamepadController()' instead.")]
		[Field ("GCCurrentExtendedGamepadSnapshotDataVersion")]
		GCExtendedGamepadSnapshotDataVersion DataVersion { get; }
	}

	/// <summary>A game controller, either form-fitting or extended wireless.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCController_Ref/index.html">Apple documentation for <c>GCController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface GCController : GCDevice {

		/// <summary>Gets or sets a handler to run when the pause button is pressed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use the Menu button found on the controller's profile, if it exists.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use the Menu button found on the controller's profile, if it exists.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use the Menu button found on the controller's profile, if it exists.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the Menu button found on the controller's profile, if it exists.")]
		[NullAllowed]
		[Export ("controllerPausedHandler", ArgumentSemantic.Copy)]
		Action<GCController> ControllerPausedHandler { get; set; }

		/// <summary>Gets the manufacturer name for the controller.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("vendorName", ArgumentSemantic.Copy)]
		new string VendorName { get; }

		/// <summary>Gets a Boolean value that tells whether the controller is attached via a cabled or wireless connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attachedToDevice")]
		bool AttachedToDevice { [Bind ("isAttachedToDevice")] get; }

		/// <summary>Gets or sets the player index for the player who is assigned to the controller.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playerIndex")]
		GCControllerPlayerIndex PlayerIndex { get; set; }

		/// <summary>If not null, the <see cref="GameController.GCController" /> is a standard controller.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		///         <altmember cref="GameController.GCController.ExtendedGamepad" />
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[NullAllowed]
		[Export ("gamepad", ArgumentSemantic.Retain)]
		GCGamepad Gamepad { get; }

		/// <summary>If not null, the <see cref="GameController.GCController" /> is an extended controller.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		///         <altmember cref="GameController.GCController.Gamepad" />
		[NullAllowed]
		[Export ("extendedGamepad", ArgumentSemantic.Retain)]
		GCExtendedGamepad ExtendedGamepad { get; }

		/// <summary>Gets the micro gamepad for the controller.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("microGamepad", ArgumentSemantic.Retain)]
		GCMicroGamepad MicroGamepad { get; }

		/// <summary>Gets an array that contains all of the connected controllers.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("controllers")]
		GCController [] Controllers { get; }

		/// <param name="completionHandler">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Starts discovery of nearby wireless controllers, and runs the provided completion handler when all discoverable controllers are discovered.</summary>
		///         <remarks>To be added.</remarks>
		[Static, Export ("startWirelessControllerDiscoveryWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Starts discovery of nearby wireless controllers, and runs the provided completion handler when all discoverable controllers are discovered.</summary>
			<returns>A task that represents the asynchronous StartWirelessControllerDiscovery operation</returns>
			<remarks>
			          <para copied="true">The StartWirelessControllerDiscoveryAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void StartWirelessControllerDiscovery ([NullAllowed] Action completionHandler);

		/// <summary>Stops discovering nearby wireless controllers.</summary>
		///         <remarks>To be added.</remarks>
		[Static, Export ("stopWirelessControllerDiscovery")]
		void StopWirelessControllerDiscovery ();

		/// <include file="../docs/api/GameController/GCController.xml" path="/Documentation/Docs[@DocId='P:GameController.GCController.DidConnectNotification']/*" />
		[Notification, Field ("GCControllerDidConnectNotification")]
		NSString DidConnectNotification { get; }

		/// <include file="../docs/api/GameController/GCController.xml" path="/Documentation/Docs[@DocId='P:GameController.GCController.DidDisconnectNotification']/*" />
		[Notification, Field ("GCControllerDidDisconnectNotification")]
		NSString DidDisconnectNotification { get; }

		/// <summary>Gets the object that contains motion data, if the controller supports motion.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("motion", ArgumentSemantic.Retain)]
		GCMotion Motion { get; }

		/// <summary>Gets or sets the dispatch queue for game controller input changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("handlerQueue", ArgumentSemantic.Retain)]
		new DispatchQueue HandlerQueue { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("productCategory")]
		new string ProductCategory { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("snapshot")]
		bool Snapshot { [Bind ("isSnapshot")] get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("capture")]
		GCController Capture ();

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("controllerWithMicroGamepad")]
		GCController GetMicroGamepadController ();

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("controllerWithExtendedGamepad")]
		GCController GetExtendedGamepadController ();

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[NullAllowed, Export ("current", ArgumentSemantic.Strong)]
		GCController Current { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("light", ArgumentSemantic.Retain)]
		GCDeviceLight Light { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("haptics", ArgumentSemantic.Retain)]
		GCDeviceHaptics Haptics { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("battery", ArgumentSemantic.Copy)]
		GCDeviceBattery Battery { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Notification, Field ("GCControllerDidBecomeCurrentNotification")]
		NSString DidBecomeCurrentNotification { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Notification, Field ("GCControllerDidStopBeingCurrentNotification")]
		NSString DidStopBeingCurrentNotification { get; }

		[TV (14, 5)]
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Static]
		[Export ("shouldMonitorBackgroundEvents")]
		bool ShouldMonitorBackgroundEvents { get; set; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("input", ArgumentSemantic.Strong)]
		GCControllerLiveInput Input { get; }
	}

	/// <summary>Holds position data of a game controller.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GameController/Reference/GCMotion_Ref/index.html">Apple documentation for <c>GCMotion</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // access thru GCController.Motion - returns a nil Handle
	partial interface GCMotion {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("controller", ArgumentSemantic.Assign)]
		GCController Controller { get; }

		/// <summary>Handler that is called when a value changes.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		Action<GCMotion> ValueChangedHandler { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("gravity", ArgumentSemantic.Assign)]
		GCAcceleration Gravity { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userAcceleration", ArgumentSemantic.Assign)]
		GCAcceleration UserAcceleration { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("attitude", ArgumentSemantic.Assign)]
		GCQuaternion Attitude { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("rotationRate", ArgumentSemantic.Assign)]
		GCRotationRate RotationRate { get; }

		/// <summary>Gets a Boolean value that tells whether the controller  can return attitude and rotation data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'HasAttitude' and 'HasRotationRate' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'HasAttitude' and 'HasRotationRate' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'HasAttitude' and 'HasRotationRate' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'HasAttitude' and 'HasRotationRate' instead.")]
		[Export ("hasAttitudeAndRotationRate")]
		bool HasAttitudeAndRotationRate { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setGravity:")]
		void SetGravity (GCAcceleration gravity);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setUserAcceleration:")]
		void SetUserAcceleration (GCAcceleration userAcceleration);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setAttitude:")]
		void SetAttitude (GCQuaternion attitude);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setRotationRate:")]
		void SetRotationRate (GCRotationRate rotationRate);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setStateFromMotion:")]
		void SetState (GCMotion motion);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("hasAttitude")]
		bool HasAttitude { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("hasRotationRate")]
		bool HasRotationRate { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sensorsRequireManualActivation")]
		bool SensorsRequireManualActivation { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sensorsActive")]
		bool SensorsActive { get; set; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("hasGravityAndUserAcceleration")]
		bool HasGravityAndUserAcceleration { get; }

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("acceleration")]
		GCAcceleration Acceleration { get; set; }
	}

	/// <param name="gamepad">The profile that contains the changed element.</param>
	///     <param name="element">The element that changed.</param>
	///     <summary>A handler that is called whenever any single element of a controller changes.</summary>
	///     <remarks>
	///       <para>This handler is called once for each element change. It is only called for directly attached elements.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	delegate void GCMicroGamepadValueChangedHandler (GCMicroGamepad gamepad, GCControllerElement element);

	/// <summary>A logical mapping of hardware controller controls to a set of in-game elements.</summary>
	///     <remarks>
	///       <para>This type is implemented by the Apple TV remote. It maps an <c>A</c> button, an <c>X</c> button, and an analog D-pad as a touchpad.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/GameController/GCMicroGamepad">Apple documentation for <c>GCMicroGamepad</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (GCPhysicalInputProfile))]
	[DisableDefaultCtor]
	interface GCMicroGamepad {
		/// <summary>Gets the controller for this profile.</summary>
		///         <value>The controller for this profile.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controller", ArgumentSemantic.Assign)]
		[NullAllowed]
		GCController Controller { get; }

		/// <summary>Gets or sets a handler that is called whenever the state of any controller element changes</summary>
		///         <value>
		///           <para>A handler that is called whenever any immediate child element changes.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>
		///           <para>This handler is called once for each element change. It is only called for directly attached elements.</para>
		///         </remarks>
		[NullAllowed, Export ("valueChangedHandler", ArgumentSemantic.Copy)]
		GCMicroGamepadValueChangedHandler ValueChangedHandler { get; set; }

		/// <summary>Gest the current state of the controller.</summary>
		///         <value>The current state of the controller.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.Capture()' instead.")]
		[Export ("saveSnapshot")]
		GCMicroGamepadSnapshot SaveSnapshot { get; }

		/// <summary>Gets the D-pad.</summary>
		///         <value>The D-pad.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dpad", ArgumentSemantic.Retain)]
		GCControllerDirectionPad Dpad { get; }

		/// <summary>Gets the <c>A</c> button.</summary>
		///         <value>The <c>A</c> button.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonA", ArgumentSemantic.Retain)]
		GCControllerButtonInput ButtonA { get; }

		/// <summary>Gets the <c>X</c> button.</summary>
		///         <value>The <c>X</c> button.</value>
		///         <remarks>To be added.</remarks>
		[Export ("buttonX", ArgumentSemantic.Retain)]
		GCControllerButtonInput ButtonX { get; }

		/// <summary>Gets or sets a value that controls whether D-pad values are measured from the physical center of the touchpad or from the point that the user first touches. </summary>
		///         <value>A value that controls whether D-pad values are measured from the physical center of the touchpad or from the point that the user first touches. The default is <see langword="false" />, to indicate that inputs are measured from the place that the user first touches.</value>
		///         <remarks>To be added.</remarks>
		[Export ("reportsAbsoluteDpadValues")]
		bool ReportsAbsoluteDpadValues { get; set; }

		/// <summary>Gets or sets a value that controls whether the D-pad changes between portrait and landscape mode on the controller as its orientation changes.</summary>
		///         <value>
		///           <see langword="true" /> if the D-pad matches the device's orientation. Otherwise, <see langword="false" />. The default is <see langword="false" />.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsRotation")]
		bool AllowsRotation { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("buttonMenu")]
		GCControllerButtonInput ButtonMenu { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setStateFromMicroGamepad:")]
		void SetState (GCMicroGamepad microGamepad);
	}

	/// <summary>Gets snapshots of the state of a micro gamepad.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/GameController/GCMicroGamepadSnapshot">Apple documentation for <c>GCMicroGamepadSnapshot</c></related>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.Capture()' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCController.Capture()' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.Capture()' instead.")]
	[BaseType (typeof (GCMicroGamepad))]
	interface GCMicroGamepadSnapshot {
		/// <summary>Gets or sets the current snapshot data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("snapshotData", ArgumentSemantic.Copy)]
		NSData SnapshotData { get; set; }

		/// <param name="data">The data with which to initialize the snapshot.</param>
		/// <summary>Creates a new snapshot by using the data from another snapshot.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithSnapshotData:")]
		NativeHandle Constructor (NSData data);

		/// <param name="controller">The controller from which to get snapshots.</param>
		/// <param name="data">The data with which to initialize the snapshot.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithController:snapshotData:")]
		NativeHandle Constructor (GCController controller, NSData data);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'GCControler.GetMicroGamepadController()' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'GCController.GetMicroGamepadController()' instead.")]
		[Field ("GCCurrentMicroGamepadSnapshotDataVersion")]
		GCMicroGamepadSnapshotDataVersion DataVersion { get; }
	}

	/// <summary>View controller that can switch event delivery between the responder chain and the game controller.</summary>
	///     <remarks>Developers can use this class as the root view controller in order to smoothly respond to both game UI inputs and game control inputs.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/GameController/GCEventViewController">Apple documentation for <c>GCEventViewController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController))]
	interface GCEventViewController {

		// inlined ctor
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>Gets or sets a value that controls whether events are delivered through the responder chain.</summary>
		///         <value>A value that controls whether events are delivered through the responder chain.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controllerUserInteractionEnabled")]
		bool ControllerUserInteractionEnabled { get; set; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCColor : NSCopying, NSSecureCoding {
		[Export ("initWithRed:green:blue:")]
		NativeHandle Constructor (float red, float green, float blue);

		[Export ("red")]
		float Red { get; }

		[Export ("green")]
		float Green { get; }

		[Export ("blue")]
		float Blue { get; }
	}

	delegate void GCControllerTouchpadHandler (GCControllerTouchpad touchpad, float xValue, float yValue, float buttonValue, bool buttonPressed);

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCControllerElement))]
	interface GCControllerTouchpad {
		[Export ("button")]
		GCControllerButtonInput Button { get; }

		[NullAllowed, Export ("touchDown", ArgumentSemantic.Copy)]
		GCControllerTouchpadHandler TouchDown { get; set; }

		[NullAllowed, Export ("touchMoved", ArgumentSemantic.Copy)]
		GCControllerTouchpadHandler TouchMoved { get; set; }

		[NullAllowed, Export ("touchUp", ArgumentSemantic.Copy)]
		GCControllerTouchpadHandler TouchUp { get; set; }

		[Export ("touchSurface")]
		GCControllerDirectionPad TouchSurface { get; }

		[Export ("touchState")]
		GCTouchState TouchState { get; }

		[Export ("reportsAbsoluteTouchSurfaceValues")]
		bool ReportsAbsoluteTouchSurfaceValues { get; set; }

		[Export ("setValueForXAxis:yAxis:touchDown:buttonValue:")]
		void SetValue (float xAxis, float yAxis, bool touchDown, float buttonValue);
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCDeviceBattery : NSSecureCoding, NSCoding {
		[Export ("batteryLevel")]
		float BatteryLevel { get; }

		[Export ("batteryState")]
		GCDeviceBatteryState BatteryState { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCDeviceHaptics {
		[Export ("supportedLocalities", ArgumentSemantic.Strong)]
		NSSet<NSString> SupportedLocalities { get; }

		[MacCatalyst (13, 1)]
		[Export ("createEngineWithLocality:")]
		[return: NullAllowed]
		CHHapticEngine CreateEngine (string locality);

		[Field ("GCHapticDurationInfinite")]
		float HapticDurationInfinite { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Static]
	interface GCHapticsLocality {

		[Field ("GCHapticsLocalityDefault")]
		NSString Default { get; }

		[Field ("GCHapticsLocalityAll")]
		NSString All { get; }

		[Field ("GCHapticsLocalityHandles")]
		NSString Handles { get; }

		[Field ("GCHapticsLocalityLeftHandle")]
		NSString LeftHandle { get; }

		[Field ("GCHapticsLocalityRightHandle")]
		NSString RightHandle { get; }

		[Field ("GCHapticsLocalityTriggers")]
		NSString Triggers { get; }

		[Field ("GCHapticsLocalityLeftTrigger")]
		NSString LeftTrigger { get; }

		[Field ("GCHapticsLocalityRightTrigger")]
		NSString RightTrigger { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCDeviceLight : NSSecureCoding, NSCoding {
		[Export ("color", ArgumentSemantic.Copy)]
		GCColor Color { get; set; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCExtendedGamepad))]
	interface GCDualShockGamepad : NSSecureCoding, NSCoding {
		[Export ("touchpadButton")]
		GCControllerButtonInput TouchpadButton { get; }

		[Export ("touchpadPrimary")]
		GCControllerDirectionPad TouchpadPrimary { get; }

		[Export ("touchpadSecondary")]
		GCControllerDirectionPad TouchpadSecondary { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
#if !XAMCORE_5_0
	interface GCKeyboard : GCDevice, NSSecureCoding, NSCoding {
#else
	interface GCKeyboard : GCDevice {
#endif

		[NullAllowed, Export ("keyboardInput", ArgumentSemantic.Strong)]
		GCKeyboardInput KeyboardInput { get; }

		[Static]
		[NullAllowed, Export ("coalescedKeyboard", ArgumentSemantic.Strong)]
		GCKeyboard CoalescedKeyboard { get; }

		[Notification, Field ("GCKeyboardDidConnectNotification")]
		NSString DidConnectNotification { get; }

		[Notification, Field ("GCKeyboardDidDisconnectNotification")]
		NSString DidDisconnectNotification { get; }
	}

	delegate void GCKeyboardValueChangedHandler (GCKeyboardInput keyboard, GCControllerButtonInput key, nint keyCode, bool pressed);

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCPhysicalInputProfile))]
	interface GCKeyboardInput {
		[NullAllowed, Export ("keyChangedHandler", ArgumentSemantic.Copy)]
		GCKeyboardValueChangedHandler KeyChangedHandler { get; set; }

		[Export ("anyKeyPressed")]
		bool IsAnyKeyPressed { [Bind ("isAnyKeyPressed")] get; }

		[Export ("buttonForKeyCode:")]
		[return: NullAllowed]
		GCControllerButtonInput GetButton (nint code);
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface GCMouse : GCDevice {
		[NullAllowed, Export ("mouseInput", ArgumentSemantic.Strong)]
		GCMouseInput MouseInput { get; }

		[Static]
		[NullAllowed, Export ("current", ArgumentSemantic.Strong)]
		GCMouse Current { get; }

		[Static]
		[Export ("mice")]
		GCMouse [] Mice { get; }

		[Notification, Field ("GCMouseDidConnectNotification")]
		NSString DidConnectNotification { get; }

		[Notification, Field ("GCMouseDidDisconnectNotification")]
		NSString DidDisconnectNotification { get; }

		[Notification, Field ("GCMouseDidBecomeCurrentNotification")]
		NSString DidBecomeCurrentNotification { get; }

		[Notification, Field ("GCMouseDidStopBeingCurrentNotification")]
		NSString DidStopBeingCurrentNotification { get; }
	}

	delegate void GCMouseMoved (GCMouseInput mouse, float deltaX, float deltaY);

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCControllerDirectionPad))]
	interface GCDeviceCursor { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCPhysicalInputProfile))]
	interface GCMouseInput {
		[NullAllowed, Export ("mouseMovedHandler", ArgumentSemantic.Copy)]
		GCMouseMoved MouseMovedHandler { get; set; }

		[Export ("scroll")]
		GCDeviceCursor Scroll { get; }

		[Export ("leftButton")]
		GCControllerButtonInput LeftButton { get; }

		[NullAllowed, Export ("rightButton")]
		GCControllerButtonInput RightButton { get; }

		[NullAllowed, Export ("middleButton")]
		GCControllerButtonInput MiddleButton { get; }

		[NullAllowed, Export ("auxiliaryButtons")]
		GCControllerButtonInput [] AuxiliaryButtons { get; }
	}

	interface IGCDevice { }

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface GCDevice {
		[Abstract]
		[Export ("handlerQueue", ArgumentSemantic.Strong)]
		DispatchQueue HandlerQueue { get; set; }

		[Abstract]
		[NullAllowed, Export ("vendorName")]
		string VendorName { get; }

		[Abstract]
		[Export ("productCategory")]
		string ProductCategory { get; }

		[Abstract]
		[Export ("physicalInputProfile", ArgumentSemantic.Strong)]
		GCPhysicalInputProfile PhysicalInputProfile { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCPhysicalInputProfile {
		[NullAllowed, Export ("device", ArgumentSemantic.Weak)]
		IGCDevice Device { get; }

		[Export ("lastEventTimestamp")]
		double LastEventTimestamp { get; }

		[Export ("elements", ArgumentSemantic.Strong)]
		NSDictionary<NSString, GCControllerElement> Elements { get; }

		[Export ("buttons", ArgumentSemantic.Strong)]
		NSDictionary<NSString, GCControllerButtonInput> Buttons { get; }

		[Export ("axes", ArgumentSemantic.Strong)]
		NSDictionary<NSString, GCControllerAxisInput> Axes { get; }

		[Export ("dpads", ArgumentSemantic.Strong)]
		NSDictionary<NSString, GCControllerDirectionPad> Dpads { get; }

		[Export ("allElements", ArgumentSemantic.Strong)]
		NSSet<GCControllerElement> AllElements { get; }

		[Export ("allButtons", ArgumentSemantic.Strong)]
		NSSet<GCControllerButtonInput> AllButtons { get; }

		[Export ("allAxes", ArgumentSemantic.Strong)]
		NSSet<GCControllerAxisInput> AllAxes { get; }

		[Export ("allDpads", ArgumentSemantic.Strong)]
		NSSet<GCControllerDirectionPad> AllDpads { get; }

		[Export ("objectForKeyedSubscript:")]
		[return: NullAllowed]
		GCControllerElement GetObjectForKeyedSubscript (string key);

		[Export ("capture")]
		GCPhysicalInputProfile Capture ();

		[Export ("setStateFromPhysicalInput:")]
		void SetState (GCPhysicalInputProfile physicalInput);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("allTouchpads", ArgumentSemantic.Strong)]
		NSSet<GCControllerTouchpad> AllTouchpads { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("touchpads", ArgumentSemantic.Strong)]
		NSDictionary<NSString, GCControllerTouchpad> Touchpads { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("hasRemappedElements")]
		bool HasRemappedElements { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("mappedElementAliasForPhysicalInputName:")]
		string GetMappedElementAlias (string physicalInputName);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("mappedPhysicalInputNamesForElementAlias:")]
		NSSet<NSString> GetMappedPhysicalInputNames (string elementAlias);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("valueDidChangeHandler", ArgumentSemantic.Copy)]
		Action<GCPhysicalInputProfile, GCControllerElement> ValueDidChangeHandler { get; set; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Static]
	interface GCInputXbox {

		[Field ("GCInputXboxPaddleOne")]
		NSString PaddleOne { get; }

		[Field ("GCInputXboxPaddleTwo")]
		NSString PaddleTwo { get; }

		[Field ("GCInputXboxPaddleThree")]
		NSString PaddleThree { get; }

		[Field ("GCInputXboxPaddleFour")]
		NSString PaddleFour { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Static]
	interface GCInput {

		[Field ("GCInputButtonA")]
		NSString ButtonA { get; }

		[Field ("GCInputButtonB")]
		NSString ButtonB { get; }

		[Field ("GCInputButtonX")]
		NSString ButtonX { get; }

		[Field ("GCInputButtonY")]
		NSString ButtonY { get; }

		[Field ("GCInputDirectionPad")]
		NSString DirectionPad { get; }

		[Field ("GCInputLeftThumbstick")]
		NSString LeftThumbstick { get; }

		[Field ("GCInputRightThumbstick")]
		NSString RightThumbstick { get; }

		[Field ("GCInputLeftShoulder")]
		NSString LeftShoulder { get; }

		[Field ("GCInputRightShoulder")]
		NSString RightShoulder { get; }

		[Field ("GCInputLeftTrigger")]
		NSString LeftTrigger { get; }

		[Field ("GCInputRightTrigger")]
		NSString RightTrigger { get; }

		[Field ("GCInputLeftThumbstickButton")]
		NSString LeftThumbstickButton { get; }

		[Field ("GCInputRightThumbstickButton")]
		NSString RightThumbstickButton { get; }

		[Field ("GCInputButtonHome")]
		NSString ButtonHome { get; }

		[Field ("GCInputButtonMenu")]
		NSString ButtonMenu { get; }

		[Field ("GCInputButtonOptions")]
		NSString ButtonOptions { get; }

		[Field ("GCInputDualShockTouchpadOne")]
		NSString DualShockTouchpadOne { get; }

		[Field ("GCInputDualShockTouchpadTwo")]
		NSString DualShockTouchpadTwo { get; }

		[Field ("GCInputDualShockTouchpadButton")]
		NSString DualShockTouchpadButton { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCInputButtonShare")]
		NSString ButtonShare { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputLeftPaddle")]
		NSString /* IGCButtonElementName */ LeftPaddle { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalAccelerator")]
		NSString /* IGCButtonElementName */ PedalAccelerator { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalBrake")]
		NSString /* IGCButtonElementName */ PedalBrake { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalClutch")]
		NSString /* IGCButtonElementName */ PedalClutch { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputRightPaddle")]
		NSString /* IGCButtonElementName */ RightPaddle { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputShifter")]
		NSString /* IGCPhysicalInputElementName */ Shifter { get; }

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputSteeringWheel")]
		NSString /* IGCAxisElementName */ SteeringWheel { get; }

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Field ("GCInputLeftBumper")]
		NSString /* GCButtonElementName */ LeftBumper { get; }

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Field ("GCInputRightBumper")]
		NSString /* GCButtonElementName */ RightBumper { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (GCExtendedGamepad))]
	interface GCXboxGamepad : NSSecureCoding, NSCoding {
		[NullAllowed, Export ("paddleButton1")]
		GCControllerButtonInput PaddleButton1 { get; }

		[NullAllowed, Export ("paddleButton2")]
		GCControllerButtonInput PaddleButton2 { get; }

		[NullAllowed, Export ("paddleButton3")]
		GCControllerButtonInput PaddleButton3 { get; }

		[NullAllowed, Export ("paddleButton4")]
		GCControllerButtonInput PaddleButton4 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("buttonShare")]
		GCControllerButtonInput ButtonShare { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	public enum GCInputElementName {
		[Field ("GCInputShifter")]
		Shifter,
	}

	[TV (14, 0), iOS (14, 0), MacCatalyst (14, 0)]
	public enum GCInputButtonName {
		[Field ("GCInputButtonA")]
		ButtonA,

		[Field ("GCInputButtonB")]
		ButtonB,

		[Field ("GCInputButtonX")]
		ButtonX,

		[Field ("GCInputButtonY")]
		ButtonY,

		[Field ("GCInputLeftThumbstickButton")]
		LeftThumbstickButton,

		[Field ("GCInputRightThumbstickButton")]
		RightThumbstickButton,

		[Field ("GCInputLeftShoulder")]
		LeftShoulder,

		[Field ("GCInputRightShoulder")]
		RightShoulder,

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Field ("GCInputLeftBumper")]
		LeftBumper,

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Field ("GCInputRightBumper")]
		RightBumper,

		[Field ("GCInputLeftTrigger")]
		LeftTrigger,

		[Field ("GCInputRightTrigger")]
		RightTrigger,

		[Field ("GCInputButtonHome")]
		ButtonHome,

		[Field ("GCInputButtonMenu")]
		ButtonMenu,

		[Field ("GCInputButtonOptions")]
		ButtonOptions,

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCInputButtonShare")]
		ButtonShare,

		[Field ("GCInputXboxPaddleOne")]
		PaddleOne,

		[Field ("GCInputXboxPaddleTwo")]
		PaddleTwo,

		[Field ("GCInputXboxPaddleThree")]
		PaddleThree,

		[Field ("GCInputXboxPaddleFour")]
		PaddleFour,

		[Field ("GCInputDualShockTouchpadButton")]
		DualShockTouchpadButton,

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputLeftPaddle")]
		LeftPaddle,

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalAccelerator")]
		PedalAccelerator,

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalBrake")]
		PedalBrake,

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputPedalClutch")]
		PedalClutch,

		[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("GCInputRightPaddle")]
		RightPaddle,
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	public enum GCInputAxisName {
		[Field ("GCInputSteeringWheel")]
		SteeringWheel,
	}

	[TV (14, 0), iOS (14, 0), MacCatalyst (14, 0)]
	public enum GCInputDirectionPadName {
		[Field ("GCInputDirectionPad")]
		DirectionPad,

		[Field ("GCInputLeftThumbstick")]
		LeftThumbstick,

		[Field ("GCInputRightThumbstick")]
		RightThumbstick,

		[Field ("GCInputDualShockTouchpadOne")]
		DualShockTouchpadOne,

		[Field ("GCInputDualShockTouchpadTwo")]
		DualShockTouchpadTwo,
	}

	[Static]
	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	partial interface GCKey {
		[Field ("GCKeyA")]
		NSString A { get; }

		[Field ("GCKeyB")]
		NSString B { get; }

		[Field ("GCKeyC")]
		NSString C { get; }

		[Field ("GCKeyD")]
		NSString D { get; }

		[Field ("GCKeyE")]
		NSString E { get; }

		[Field ("GCKeyF")]
		NSString F { get; }

		[Field ("GCKeyG")]
		NSString G { get; }

		[Field ("GCKeyH")]
		NSString H { get; }

		[Field ("GCKeyI")]
		NSString I { get; }

		[Field ("GCKeyJ")]
		NSString J { get; }

		[Field ("GCKeyK")]
		NSString K { get; }

		[Field ("GCKeyL")]
		NSString L { get; }

		[Field ("GCKeyM")]
		NSString M { get; }

		[Field ("GCKeyN")]
		NSString N { get; }

		[Field ("GCKeyO")]
		NSString O { get; }

		[Field ("GCKeyP")]
		NSString P { get; }

		[Field ("GCKeyQ")]
		NSString Q { get; }

		[Field ("GCKeyR")]
		NSString R { get; }

		[Field ("GCKeyS")]
		NSString S { get; }

		[Field ("GCKeyT")]
		NSString T { get; }

		[Field ("GCKeyU")]
		NSString U { get; }

		[Field ("GCKeyV")]
		NSString V { get; }

		[Field ("GCKeyW")]
		NSString W { get; }

		[Field ("GCKeyX")]
		NSString X { get; }

		[Field ("GCKeyY")]
		NSString Y { get; }

		[Field ("GCKeyZ")]
		NSString Z { get; }

		[Field ("GCKeyOne")]
		NSString One { get; }

		[Field ("GCKeyTwo")]
		NSString Two { get; }

		[Field ("GCKeyThree")]
		NSString Three { get; }

		[Field ("GCKeyFour")]
		NSString Four { get; }

		[Field ("GCKeyFive")]
		NSString Five { get; }

		[Field ("GCKeySix")]
		NSString Six { get; }

		[Field ("GCKeySeven")]
		NSString Seven { get; }

		[Field ("GCKeyEight")]
		NSString Eight { get; }

		[Field ("GCKeyNine")]
		NSString Nine { get; }

		[Field ("GCKeyZero")]
		NSString Zero { get; }

		[Field ("GCKeyReturnOrEnter")]
		NSString ReturnOrEnter { get; }

		[Field ("GCKeyEscape")]
		NSString Escape { get; }

		[Field ("GCKeyDeleteOrBackspace")]
		NSString DeleteOrBackspace { get; }

		[Field ("GCKeyTab")]
		NSString Tab { get; }

		[Field ("GCKeySpacebar")]
		NSString Spacebar { get; }

		[Field ("GCKeyHyphen")]
		NSString Hyphen { get; }

		[Field ("GCKeyEqualSign")]
		NSString EqualSign { get; }

		[Field ("GCKeyOpenBracket")]
		NSString OpenBracket { get; }

		[Field ("GCKeyCloseBracket")]
		NSString CloseBracket { get; }

		[Field ("GCKeyBackslash")]
		NSString Backslash { get; }

		[Field ("GCKeyNonUSPound")]
		NSString NonUSPound { get; }

		[Field ("GCKeySemicolon")]
		NSString Semicolon { get; }

		[Field ("GCKeyQuote")]
		NSString Quote { get; }

		[Field ("GCKeyGraveAccentAndTilde")]
		NSString GraveAccentAndTilde { get; }

		[Field ("GCKeyComma")]
		NSString Comma { get; }

		[Field ("GCKeyPeriod")]
		NSString Period { get; }

		[Field ("GCKeySlash")]
		NSString Slash { get; }

		[Field ("GCKeyCapsLock")]
		NSString CapsLock { get; }

		[Field ("GCKeyF1")]
		NSString F1 { get; }

		[Field ("GCKeyF2")]
		NSString F2 { get; }

		[Field ("GCKeyF3")]
		NSString F3 { get; }

		[Field ("GCKeyF4")]
		NSString F4 { get; }

		[Field ("GCKeyF5")]
		NSString F5 { get; }

		[Field ("GCKeyF6")]
		NSString F6 { get; }

		[Field ("GCKeyF7")]
		NSString F7 { get; }

		[Field ("GCKeyF8")]
		NSString F8 { get; }

		[Field ("GCKeyF9")]
		NSString F9 { get; }

		[Field ("GCKeyF10")]
		NSString F10 { get; }

		[Field ("GCKeyF11")]
		NSString F11 { get; }

		[Field ("GCKeyF12")]
		NSString F12 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF13")]
		NSString F13 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF14")]
		NSString F14 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF15")]
		NSString F15 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF16")]
		NSString F16 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF17")]
		NSString F17 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF18")]
		NSString F18 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF19")]
		NSString F19 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyF20")]
		NSString F20 { get; }

		[Field ("GCKeyPrintScreen")]
		NSString PrintScreen { get; }

		[Field ("GCKeyScrollLock")]
		NSString ScrollLock { get; }

		[Field ("GCKeyPause")]
		NSString Pause { get; }

		[Field ("GCKeyInsert")]
		NSString Insert { get; }

		[Field ("GCKeyHome")]
		NSString Home { get; }

		[Field ("GCKeyPageUp")]
		NSString PageUp { get; }

		[Field ("GCKeyDeleteForward")]
		NSString DeleteForward { get; }

		[Field ("GCKeyEnd")]
		NSString End { get; }

		[Field ("GCKeyPageDown")]
		NSString PageDown { get; }

		[Field ("GCKeyRightArrow")]
		NSString RightArrow { get; }

		[Field ("GCKeyLeftArrow")]
		NSString LeftArrow { get; }

		[Field ("GCKeyDownArrow")]
		NSString DownArrow { get; }

		[Field ("GCKeyUpArrow")]
		NSString UpArrow { get; }

		[Field ("GCKeyKeypadNumLock")]
		NSString KeypadNumLock { get; }

		[Field ("GCKeyKeypadSlash")]
		NSString KeypadSlash { get; }

		[Field ("GCKeyKeypadAsterisk")]
		NSString KeypadAsterisk { get; }

		[Field ("GCKeyKeypadHyphen")]
		NSString KeypadHyphen { get; }

		[Field ("GCKeyKeypadPlus")]
		NSString KeypadPlus { get; }

		[Field ("GCKeyKeypadEnter")]
		NSString KeypadEnter { get; }

		[Field ("GCKeyKeypad1")]
		NSString Keypad1 { get; }

		[Field ("GCKeyKeypad2")]
		NSString Keypad2 { get; }

		[Field ("GCKeyKeypad3")]
		NSString Keypad3 { get; }

		[Field ("GCKeyKeypad4")]
		NSString Keypad4 { get; }

		[Field ("GCKeyKeypad5")]
		NSString Keypad5 { get; }

		[Field ("GCKeyKeypad6")]
		NSString Keypad6 { get; }

		[Field ("GCKeyKeypad7")]
		NSString Keypad7 { get; }

		[Field ("GCKeyKeypad8")]
		NSString Keypad8 { get; }

		[Field ("GCKeyKeypad9")]
		NSString Keypad9 { get; }

		[Field ("GCKeyKeypad0")]
		NSString Keypad0 { get; }

		[Field ("GCKeyKeypadPeriod")]
		NSString KeypadPeriod { get; }

		[Field ("GCKeyKeypadEqualSign")]
		NSString KeypadEqualSign { get; }

		[Field ("GCKeyNonUSBackslash")]
		NSString NonUSBackslash { get; }

		[Field ("GCKeyApplication")]
		NSString Application { get; }

		[Field ("GCKeyPower")]
		NSString Power { get; }

		[Field ("GCKeyInternational1")]
		NSString International1 { get; }

		[Field ("GCKeyInternational2")]
		NSString International2 { get; }

		[Field ("GCKeyInternational3")]
		NSString International3 { get; }

		[Field ("GCKeyInternational4")]
		NSString International4 { get; }

		[Field ("GCKeyInternational5")]
		NSString International5 { get; }

		[Field ("GCKeyInternational6")]
		NSString International6 { get; }

		[Field ("GCKeyInternational7")]
		NSString International7 { get; }

		[Field ("GCKeyInternational8")]
		NSString International8 { get; }

		[Field ("GCKeyInternational9")]
		NSString International9 { get; }

		[Field ("GCKeyLANG1")]
		NSString Lang1 { get; }

		[Field ("GCKeyLANG2")]
		NSString Lang2 { get; }

		[Field ("GCKeyLANG3")]
		NSString Lang3 { get; }

		[Field ("GCKeyLANG4")]
		NSString Lang4 { get; }

		[Field ("GCKeyLANG5")]
		NSString Lang5 { get; }

		[Field ("GCKeyLANG6")]
		NSString Lang6 { get; }

		[Field ("GCKeyLANG7")]
		NSString Lang7 { get; }

		[Field ("GCKeyLANG8")]
		NSString Lang8 { get; }

		[Field ("GCKeyLANG9")]
		NSString Lang9 { get; }

		[Field ("GCKeyLeftControl")]
		NSString LeftControl { get; }

		[Field ("GCKeyLeftShift")]
		NSString LeftShift { get; }

		[Field ("GCKeyLeftAlt")]
		NSString LeftAlt { get; }

		[Field ("GCKeyLeftGUI")]
		NSString LeftGui { get; }

		[Field ("GCKeyRightControl")]
		NSString RightControl { get; }

		[Field ("GCKeyRightShift")]
		NSString RightShift { get; }

		[Field ("GCKeyRightAlt")]
		NSString RightAlt { get; }

		[Field ("GCKeyRightGUI")]
		NSString RightGui { get; }
	}

	[TV (14, 0), iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Static]
	interface GCKeyCode {
		[Field ("GCKeyCodeKeyA")]
		nint KeyA { get; }

		[Field ("GCKeyCodeKeyB")]
		nint KeyB { get; }

		[Field ("GCKeyCodeKeyC")]
		nint KeyC { get; }

		[Field ("GCKeyCodeKeyD")]
		nint KeyD { get; }

		[Field ("GCKeyCodeKeyE")]
		nint KeyE { get; }

		[Field ("GCKeyCodeKeyF")]
		nint KeyF { get; }

		[Field ("GCKeyCodeKeyG")]
		nint KeyG { get; }

		[Field ("GCKeyCodeKeyH")]
		nint KeyH { get; }

		[Field ("GCKeyCodeKeyI")]
		nint KeyI { get; }

		[Field ("GCKeyCodeKeyJ")]
		nint KeyJ { get; }

		[Field ("GCKeyCodeKeyK")]
		nint KeyK { get; }

		[Field ("GCKeyCodeKeyL")]
		nint KeyL { get; }

		[Field ("GCKeyCodeKeyM")]
		nint KeyM { get; }

		[Field ("GCKeyCodeKeyN")]
		nint KeyN { get; }

		[Field ("GCKeyCodeKeyO")]
		nint KeyO { get; }

		[Field ("GCKeyCodeKeyP")]
		nint KeyP { get; }

		[Field ("GCKeyCodeKeyQ")]
		nint KeyQ { get; }

		[Field ("GCKeyCodeKeyR")]
		nint KeyR { get; }

		[Field ("GCKeyCodeKeyS")]
		nint KeyS { get; }

		[Field ("GCKeyCodeKeyT")]
		nint KeyT { get; }

		[Field ("GCKeyCodeKeyU")]
		nint KeyU { get; }

		[Field ("GCKeyCodeKeyV")]
		nint KeyV { get; }

		[Field ("GCKeyCodeKeyW")]
		nint KeyW { get; }

		[Field ("GCKeyCodeKeyX")]
		nint KeyX { get; }

		[Field ("GCKeyCodeKeyY")]
		nint KeyY { get; }

		[Field ("GCKeyCodeKeyZ")]
		nint KeyZ { get; }

		[Field ("GCKeyCodeOne")]
		nint One { get; }

		[Field ("GCKeyCodeTwo")]
		nint Two { get; }

		[Field ("GCKeyCodeThree")]
		nint Three { get; }

		[Field ("GCKeyCodeFour")]
		nint Four { get; }

		[Field ("GCKeyCodeFive")]
		nint Five { get; }

		[Field ("GCKeyCodeSix")]
		nint Six { get; }

		[Field ("GCKeyCodeSeven")]
		nint Seven { get; }

		[Field ("GCKeyCodeEight")]
		nint Eight { get; }

		[Field ("GCKeyCodeNine")]
		nint Nine { get; }

		[Field ("GCKeyCodeZero")]
		nint Zero { get; }

		[Field ("GCKeyCodeReturnOrEnter")]
		nint ReturnOrEnter { get; }

		[Field ("GCKeyCodeEscape")]
		nint Escape { get; }

		[Field ("GCKeyCodeDeleteOrBackspace")]
		nint DeleteOrBackspace { get; }

		[Field ("GCKeyCodeTab")]
		nint Tab { get; }

		[Field ("GCKeyCodeSpacebar")]
		nint Spacebar { get; }

		[Field ("GCKeyCodeHyphen")]
		nint Hyphen { get; }

		[Field ("GCKeyCodeEqualSign")]
		nint EqualSign { get; }

		[Field ("GCKeyCodeOpenBracket")]
		nint OpenBracket { get; }

		[Field ("GCKeyCodeCloseBracket")]
		nint CloseBracket { get; }

		[Field ("GCKeyCodeBackslash")]
		nint Backslash { get; }

		[Field ("GCKeyCodeNonUSPound")]
		nint NonUSPound { get; }

		[Field ("GCKeyCodeSemicolon")]
		nint Semicolon { get; }

		[Field ("GCKeyCodeQuote")]
		nint Quote { get; }

		[Field ("GCKeyCodeGraveAccentAndTilde")]
		nint GraveAccentAndTilde { get; }

		[Field ("GCKeyCodeComma")]
		nint Comma { get; }

		[Field ("GCKeyCodePeriod")]
		nint Period { get; }

		[Field ("GCKeyCodeSlash")]
		nint Slash { get; }

		[Field ("GCKeyCodeCapsLock")]
		nint CapsLock { get; }

		[Field ("GCKeyCodeF1")]
		nint F1 { get; }

		[Field ("GCKeyCodeF2")]
		nint F2 { get; }

		[Field ("GCKeyCodeF3")]
		nint F3 { get; }

		[Field ("GCKeyCodeF4")]
		nint F4 { get; }

		[Field ("GCKeyCodeF5")]
		nint F5 { get; }

		[Field ("GCKeyCodeF6")]
		nint F6 { get; }

		[Field ("GCKeyCodeF7")]
		nint F7 { get; }

		[Field ("GCKeyCodeF8")]
		nint F8 { get; }

		[Field ("GCKeyCodeF9")]
		nint F9 { get; }

		[Field ("GCKeyCodeF10")]
		nint F10 { get; }

		[Field ("GCKeyCodeF11")]
		nint F11 { get; }

		[Field ("GCKeyCodeF12")]
		nint F12 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF13")]
		nint F13 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF14")]
		nint F14 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF15")]
		nint F15 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF16")]
		nint F16 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF17")]
		nint F17 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF18")]
		nint F18 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF19")]
		nint F19 { get; }

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCKeyCodeF20")]
		nint F20 { get; }

		[Field ("GCKeyCodePrintScreen")]
		nint PrintScreen { get; }

		[Field ("GCKeyCodeScrollLock")]
		nint ScrollLock { get; }

		[Field ("GCKeyCodePause")]
		nint Pause { get; }

		[Field ("GCKeyCodeInsert")]
		nint Insert { get; }

		[Field ("GCKeyCodeHome")]
		nint Home { get; }

		[Field ("GCKeyCodePageUp")]
		nint PageUp { get; }

		[Field ("GCKeyCodeDeleteForward")]
		nint DeleteForward { get; }

		[Field ("GCKeyCodeEnd")]
		nint End { get; }

		[Field ("GCKeyCodePageDown")]
		nint PageDown { get; }

		[Field ("GCKeyCodeRightArrow")]
		nint RightArrow { get; }

		[Field ("GCKeyCodeLeftArrow")]
		nint LeftArrow { get; }

		[Field ("GCKeyCodeDownArrow")]
		nint DownArrow { get; }

		[Field ("GCKeyCodeUpArrow")]
		nint UpArrow { get; }

		[Field ("GCKeyCodeKeypadNumLock")]
		nint KeypadNumLock { get; }

		[Field ("GCKeyCodeKeypadSlash")]
		nint KeypadSlash { get; }

		[Field ("GCKeyCodeKeypadAsterisk")]
		nint KeypadAsterisk { get; }

		[Field ("GCKeyCodeKeypadHyphen")]
		nint KeypadHyphen { get; }

		[Field ("GCKeyCodeKeypadPlus")]
		nint KeypadPlus { get; }

		[Field ("GCKeyCodeKeypadEnter")]
		nint KeypadEnter { get; }

		[Field ("GCKeyCodeKeypad1")]
		nint Keypad1 { get; }

		[Field ("GCKeyCodeKeypad2")]
		nint Keypad2 { get; }

		[Field ("GCKeyCodeKeypad3")]
		nint Keypad3 { get; }

		[Field ("GCKeyCodeKeypad4")]
		nint Keypad4 { get; }

		[Field ("GCKeyCodeKeypad5")]
		nint Keypad5 { get; }

		[Field ("GCKeyCodeKeypad6")]
		nint Keypad6 { get; }

		[Field ("GCKeyCodeKeypad7")]
		nint Keypad7 { get; }

		[Field ("GCKeyCodeKeypad8")]
		nint Keypad8 { get; }

		[Field ("GCKeyCodeKeypad9")]
		nint Keypad9 { get; }

		[Field ("GCKeyCodeKeypad0")]
		nint Keypad0 { get; }

		[Field ("GCKeyCodeKeypadPeriod")]
		nint KeypadPeriod { get; }

		[Field ("GCKeyCodeKeypadEqualSign")]
		nint KeypadEqualSign { get; }

		[Field ("GCKeyCodeNonUSBackslash")]
		nint NonUSBackslash { get; }

		[Field ("GCKeyCodeApplication")]
		nint Application { get; }

		[Field ("GCKeyCodePower")]
		nint Power { get; }

		[Field ("GCKeyCodeInternational1")]
		nint International1 { get; }

		[Field ("GCKeyCodeInternational2")]
		nint International2 { get; }

		[Field ("GCKeyCodeInternational3")]
		nint International3 { get; }

		[Field ("GCKeyCodeInternational4")]
		nint International4 { get; }

		[Field ("GCKeyCodeInternational5")]
		nint International5 { get; }

		[Field ("GCKeyCodeInternational6")]
		nint International6 { get; }

		[Field ("GCKeyCodeInternational7")]
		nint International7 { get; }

		[Field ("GCKeyCodeInternational8")]
		nint International8 { get; }

		[Field ("GCKeyCodeInternational9")]
		nint International9 { get; }

		[Field ("GCKeyCodeLANG1")]
		nint Lang1 { get; }

		[Field ("GCKeyCodeLANG2")]
		nint Lang2 { get; }

		[Field ("GCKeyCodeLANG3")]
		nint Lang3 { get; }

		[Field ("GCKeyCodeLANG4")]
		nint Lang4 { get; }

		[Field ("GCKeyCodeLANG5")]
		nint Lang5 { get; }

		[Field ("GCKeyCodeLANG6")]
		nint Lang6 { get; }

		[Field ("GCKeyCodeLANG7")]
		nint Lang7 { get; }

		[Field ("GCKeyCodeLANG8")]
		nint Lang8 { get; }

		[Field ("GCKeyCodeLANG9")]
		nint Lang9 { get; }

		[Field ("GCKeyCodeLeftControl")]
		nint LeftControl { get; }

		[Field ("GCKeyCodeLeftShift")]
		nint LeftShift { get; }

		[Field ("GCKeyCodeLeftAlt")]
		nint LeftAlt { get; }

		[Field ("GCKeyCodeLeftGUI")]
		nint LeftGui { get; }

		[Field ("GCKeyCodeRightControl")]
		nint RightControl { get; }

		[Field ("GCKeyCodeRightShift")]
		nint RightShift { get; }

		[Field ("GCKeyCodeRightAlt")]
		nint RightAlt { get; }

		[Field ("GCKeyCodeRightGUI")]
		nint RightGui { get; }
	}

	[iOS (14, 3)]
	[TV (14, 3)]
	[MacCatalyst (14, 3)]
	[BaseType (typeof (GCMicroGamepad))]
	[DisableDefaultCtor]
	interface GCDirectionalGamepad {
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[Native]
	enum GCDualSenseAdaptiveTriggerMode : long {
		Off = 0,
		Feedback = 1,
		Weapon = 2,
		Vibration = 3,
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		SlopeFeedback = 4,
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[Native]
	enum GCDualSenseAdaptiveTriggerStatus : long {
		Unknown = -1,
		FeedbackNoLoad,
		FeedbackLoadApplied,
		WeaponReady,
		WeaponFiring,
		WeaponFired,
		VibrationNotVibrating,
		VibrationIsVibrating,
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		SlopeFeedbackReady,
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		SlopeFeedbackApplyingLoad,
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		SlopeFeedbackFinished,
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[BaseType (typeof (GCControllerButtonInput))]
	[DisableDefaultCtor]
	interface GCDualSenseAdaptiveTrigger {

		[Export ("mode")]
		GCDualSenseAdaptiveTriggerMode Mode { get; }

		[Export ("status")]
		GCDualSenseAdaptiveTriggerStatus Status { get; }

		[Export ("armPosition")]
		float ArmPosition { get; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("setModeSlopeFeedbackWithStartPosition:endPosition:startStrength:endStrength:")]
		void SetModeSlopeFeedback (float startPosition, float endPosition, float startStrength, float endStrength);

		[Export ("setModeFeedbackWithStartPosition:resistiveStrength:")]
		void SetModeFeedback (float startPosition, float resistiveStrength);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
#if XAMCORE_5_0
		[Export ("setModeFeedbackWithResistiveStrengths:")]
#else
		[Wrap ("_SetModeFeedback (positionalResistiveStrengths.ToBlittable ())", IsVirtual = true)]
#endif
		void SetModeFeedback (GCDualSenseAdaptiveTriggerPositionalResistiveStrengths positionalResistiveStrengths);

#if !XAMCORE_5_0
		[Internal]
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("setModeFeedbackWithResistiveStrengths:")]
		void _SetModeFeedback (GCDualSenseAdaptiveTriggerPositionalResistiveStrengths_Blittable positionalResistiveStrengths);
#endif

		[Export ("setModeWeaponWithStartPosition:endPosition:resistiveStrength:")]
		void SetModeWeapon (float startPosition, float endPosition, float resistiveStrength);

		[Export ("setModeVibrationWithStartPosition:amplitude:frequency:")]
		void SetModeVibration (float startPosition, float amplitude, float frequency);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
#if XAMCORE_5_0
		[Export ("setModeVibrationWithAmplitudes:frequency:")]
#else
		[Wrap ("_SetModeVibration (positionalAmplitudes.ToBlittable (), frequency)", IsVirtual = true)]
#endif
		void SetModeVibration (GCDualSenseAdaptiveTriggerPositionalAmplitudes positionalAmplitudes, float frequency);

#if !XAMCORE_5_0
		[Internal]
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("setModeVibrationWithAmplitudes:frequency:")]
		void _SetModeVibration (GCDualSenseAdaptiveTriggerPositionalAmplitudes_Blittable positionalAmplitudes, float frequency);
#endif

		[Export ("setModeOff")]
		void SetModeOff ();
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[BaseType (typeof (GCExtendedGamepad))]
	[DisableDefaultCtor] // Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: -[GCControllerButtonInput setIndex:]: unrecognized selector sent to instance 0x60000147eac0
	interface GCDualSenseGamepad {

		[Export ("touchpadButton")]
		GCControllerButtonInput TouchpadButton { get; }

		[Export ("touchpadPrimary")]
		GCControllerDirectionPad TouchpadPrimary { get; }

		[Export ("touchpadSecondary")]
		GCControllerDirectionPad TouchpadSecondary { get; }

		[Export ("leftTrigger")]
		GCDualSenseAdaptiveTrigger LeftTrigger { get; }

		[Export ("rightTrigger")]
		GCDualSenseAdaptiveTrigger RightTrigger { get; }
	}

	[TV (14, 5)]
	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	enum GCInputDirectional {
		[Field ("GCInputDirectionalDpad")]
		Dpad,

		[Field ("GCInputDirectionalCardinalDpad")]
		CardinalDpad,

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCInputDirectionalCenterButton")]
		CenterButton,

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("GCInputDirectionalTouchSurfaceButton")]
		TouchSurfaceButton,
	}

	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	enum GCInputMicroGamepad {
		[Field ("GCInputMicroGamepadDpad")]
		Dpad,

		[Field ("GCInputMicroGamepadButtonA")]
		ButtonA,

		[Field ("GCInputMicroGamepadButtonX")]
		ButtonX,

		[Field ("GCInputMicroGamepadButtonMenu")]
		ButtonMenu,
	}

	delegate GCVirtualControllerElementConfiguration GCVirtualControllerElementUpdateBlock (GCVirtualControllerElementConfiguration configuration);

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCVirtualController {
		[Static]
		[Export ("virtualControllerWithConfiguration:")]
		GCVirtualController Create (GCVirtualControllerConfiguration configuration);

		[Export ("initWithConfiguration:")]
		[DesignatedInitializer]
		NativeHandle Constructor (GCVirtualControllerConfiguration configuration);

		[Async]
		[Export ("connectWithReplyHandler:")]
		void Connect ([NullAllowed] Action<NSError> reply);

		[Export ("disconnect")]
		void Disconnect ();

		[NullAllowed, Export ("controller", ArgumentSemantic.Weak)]
		GCController Controller { get; }

		[Export ("updateConfigurationForElement:configuration:")]
		void UpdateConfiguration (string element, GCVirtualControllerElementUpdateBlock configuration);

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("setValue:forButtonElement:")]
		void SetValue (nfloat value, string element);

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("setPosition:forDirectionPadElement:")]
		void SetPosition (CGPoint position, string element);
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface GCVirtualControllerConfiguration {
		[Export ("elements", ArgumentSemantic.Strong)]
		NSSet<NSString> Elements { get; set; }

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get; set; }
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface GCVirtualControllerElementConfiguration {
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get; set; }

		[NullAllowed, Export ("path", ArgumentSemantic.Strong)]
		BezierPath Path { get; set; }

		[Export ("actsAsTouchpad")]
		bool ActsAsTouchpad { get; set; }
	}

	// Deliberate decision on not enum'ifying these
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Static]
	interface GCProductCategory {
		[Field ("GCProductCategoryDualSense")]
		NSString DualSense { get; }

		[Field ("GCProductCategoryDualShock4")]
		NSString DualShock4 { get; }

		[Field ("GCProductCategoryMFi")]
		NSString MFi { get; }

		[Field ("GCProductCategoryXboxOne")]
		NSString XboxOne { get; }

		[Field ("GCProductCategorySiriRemote1stGen")]
		NSString SiriRemote1stGen { get; }

		[Field ("GCProductCategorySiriRemote2ndGen")]
		NSString SiriRemote2ndGen { get; }

		[Field ("GCProductCategoryControlCenterRemote")]
		NSString ControlCenterRemote { get; }

		[Field ("GCProductCategoryUniversalElectronicsRemote")]
		NSString UniversalElectronicsRemote { get; }

		[Field ("GCProductCategoryCoalescedRemote")]
		NSString CoalescedRemote { get; }

		[Field ("GCProductCategoryMouse")]
		NSString Mouse { get; }

		[Field ("GCProductCategoryKeyboard")]
		NSString Keyboard { get; }

#if !XAMCORE_5_0
		[Obsolete ("Use 'Hid' instead.")]
		[iOS (16, 0), Mac (13, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Field ("GCProductCategoryHID")]
		NSString GCProductCategoryHid { get; }
#endif

		[iOS (16, 0), Mac (13, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Field ("GCProductCategoryHID")]
		NSString Hid { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("GCProductCategoryArcadeStick")]
		NSString ArcadeStick { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCRacingWheel : GCDevice {
		[Static]
		[Export ("connectedRacingWheels")]
		NSSet<GCRacingWheel> ConnectedRacingWheels { get; }

		[Export ("acquireDeviceWithError:")]
		bool AcquireDevice ([NullAllowed] out NSError error);

		[Export ("relinquishDevice")]
		void RelinquishDevice ();

		[Export ("acquired")]
		bool Acquired { [Bind ("isAcquired")] get; }

		[Export ("wheelInput", ArgumentSemantic.Strong)]
		GCRacingWheelInput WheelInput { get; }

		[Export ("snapshot")]
		bool Snapshot { [Bind ("isSnapshot")] get; }

		[Export ("capture")]
		GCRacingWheel Capture { get; }

		[Notification, Field ("GCRacingWheelDidConnectNotification")]
		NSString DidConnectNotification { get; }

		[Notification, Field ("GCRacingWheelDidDisconnectNotification")]
		NSString DidDisconnectNotification { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (GCRacingWheelInputState))]
	interface GCRacingWheelInput : GCDevicePhysicalInput {
		// Sealed since GCDevicePhysicalInput.Capture returns IGCDevicePhysicalInputState
		[Sealed, Export ("capture")]
		GCRacingWheelInputState WheelInputCapture { get; }

		// Sealed since GCDevicePhysicalInput.NextInputState returns NSObject
		[Sealed, NullAllowed, Export ("nextInputState")]
		IGCDevicePhysicalInputStateDiff WheelInputNextInputState { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface GCRacingWheelInputState : GCDevicePhysicalInputState {
		[Export ("wheel")]
		GCSteeringWheelElement Wheel { get; }

		[NullAllowed, Export ("acceleratorPedal")]
		IGCButtonElement AcceleratorPedal { get; }

		[NullAllowed, Export ("brakePedal")]
		IGCButtonElement BrakePedal { get; }

		[NullAllowed, Export ("clutchPedal")]
		IGCButtonElement ClutchPedal { get; }

		[NullAllowed, Export ("shifter")]
		GCGearShifterElement Shifter { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCSteeringWheelElement : GCAxisElement {
		[Export ("maximumDegreesOfRotation")]
		float MaximumDegreesOfRotation { get; }
	}

	[iOS (16, 0), Mac (13, 0), TV (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCPhysicalInputElementCollection<KeyIdentifierType, ElementIdentifierType> : INSFastEnumeration // # no generator support for FastEnumeration - https://github.com/dotnet/macios/issues/22516
		where KeyIdentifierType : NSString
		where ElementIdentifierType : IGCPhysicalInputElement /* id<GCPhysicalInputElement>> */
	{
		[Export ("count")]
		nuint Count { get; }

		[Export ("elementForAlias:")]
		[return: NullAllowed]
		IGCPhysicalInputElement GetElement (string alias);

		[Export ("objectForKeyedSubscript:")]
		[return: NullAllowed]
		IGCPhysicalInputElement GetObject (string key);

		[Export ("elementEnumerator")]
		NSEnumerator<ElementIdentifierType> ElementEnumerator { get; }
	}

	interface IGCDevicePhysicalInputState { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCDevicePhysicalInputState {
		[Abstract]
		[NullAllowed, Export ("device", ArgumentSemantic.Weak)]
		IGCDevice Device { get; }

		[Abstract]
		[Export ("lastEventTimestamp")]
		double LastEventTimestamp { get; }

		[Abstract]
		[Export ("lastEventLatency")]
		double LastEventLatency { get; }

		[Abstract]
		[Export ("elements")]
		GCPhysicalInputElementCollection<NSString, IGCPhysicalInputElement> Elements { get; }

		[Abstract]
		[Export ("buttons")]
		GCPhysicalInputElementCollection<NSString, IGCButtonElement> Buttons { get; }

		[Abstract]
		[Export ("axes")]
		GCPhysicalInputElementCollection<NSString, IGCAxisElement> Axes { get; }

		[Abstract]
		[Export ("switches")]
		GCPhysicalInputElementCollection<NSString, IGCSwitchElement> Switches { get; }

		[Abstract]
		[Export ("dpads")]
		GCPhysicalInputElementCollection<NSString, IGCDirectionPadElement> Dpads { get; }

		[Abstract]
		[Export ("objectForKeyedSubscript:")]
		[return: NullAllowed]
		IGCPhysicalInputElement GetObject (string key);
	}

	interface IGCAxisInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCAxisInput {
		[Abstract]
		[NullAllowed, Export ("valueDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCAxisInput, float> ValueDidChangeHandler { get; set; }

		[Abstract]
		[Export ("value")]
		float Value { get; }

		[Abstract]
		[Export ("analog")]
		bool Analog { [Bind ("isAnalog")] get; }

		[Abstract]
		[Export ("canWrap")]
		bool CanWrap { get; }

		[Abstract]
		[Export ("lastValueTimestamp")]
		double LastValueTimestamp { get; }

		[Abstract]
		[Export ("lastValueLatency")]
		double LastValueLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCAxisElement : IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCAxisElement : GCPhysicalInputElement {
		[Abstract]
		[NullAllowed, Export ("absoluteInput")]
		IGCAxisInput AbsoluteInput { get; }

		[Abstract]
		[Export ("relativeInput")]
		IGCRelativeInput RelativeInput { get; }
	}

	interface IGCButtonElement : IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCButtonElement : GCPhysicalInputElement {
		[Abstract]
		[Export ("pressedInput")]
		NSObject PressedInput { get; }

		[Abstract]
		[NullAllowed, Export ("touchedInput")]
		IGCTouchedStateInput TouchedInput { get; }
	}

	delegate void ElementValueDidChangeHandler (IGCDevicePhysicalInput physicalInput, IGCPhysicalInputElement element);
	delegate void InputStateAvailableHandler (IGCDevicePhysicalInput physicalInput);

	interface IGCDevicePhysicalInput : IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCDevicePhysicalInput : GCDevicePhysicalInputState {
#if !XAMCORE_5_0
#pragma warning disable 0108 // warning CS0108: 'GCDevicePhysicalInput.Device' hides inherited member 'GCDevicePhysicalInputState.Device'. Use the new keyword if hiding was intended.
		[Abstract]
		[NullAllowed, Export ("device", ArgumentSemantic.Weak)]
		IGCDevice Device { get; }
#pragma warning restore
#endif

		[Abstract]
		[NullAllowed, Export ("elementValueDidChangeHandler", ArgumentSemantic.Copy)]
		ElementValueDidChangeHandler ElementValueDidChangeHandler { get; set; }

		[Abstract]
		[Export ("capture")]
		IGCDevicePhysicalInputState Capture { get; }

		[Abstract]
		[NullAllowed, Export ("inputStateAvailableHandler", ArgumentSemantic.Copy)]
		InputStateAvailableHandler InputStateAvailableHandler { get; set; }

		[Abstract]
		[Export ("inputStateQueueDepth")]
		nint InputStateQueueDepth { get; set; }

		[Abstract]
		[NullAllowed, Export ("nextInputState")]
		NSObject NextInputState { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[NullAllowed, Export ("queue", ArgumentSemantic.Strong)]
		DispatchQueue Queue { get; set; }
	}

	interface IGCDevicePhysicalInputStateDiff { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCDevicePhysicalInputStateDiff {
		[Abstract]
		[Export ("changeForElement:")]
		GCDevicePhysicalInputElementChange GetChange (IGCPhysicalInputElement element);

		[Abstract]
		[NullAllowed, Export ("changedElements")]
		NSEnumerator<IGCPhysicalInputElement> ChangedElements { get; }
	}

	interface IGCDirectionPadElement : IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCDirectionPadElement : GCPhysicalInputElement {
		[Abstract]
		[Export ("xAxis")]
		IGCAxisInput XAxis { get; }

		[Abstract]
		[Export ("yAxis")]
		IGCAxisInput YAxis { get; }

		[Abstract]
		[Export ("up")]
		NSObject Up { get; }

		[Abstract]
		[Export ("down")]
		NSObject Down { get; }

		[Abstract]
		[Export ("left")]
		NSObject Left { get; }

		[Abstract]
		[Export ("right")]
		NSObject Right { get; }

		[TV (17, 4), Mac (14, 3), iOS (17, 4), MacCatalyst (17, 4)]
		[Abstract]
		[Export ("xyAxes")]
		IGCAxis2DInput XyAxes { get; }
	}

	interface IGCLinearInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCLinearInput {
		[Abstract]
		[NullAllowed, Export ("valueDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCLinearInput, float> ValueDidChangeHandler { get; set; }

		[Abstract]
		[Export ("value")]
		float Value { get; }

		[Abstract]
		[Export ("analog")]
		bool Analog { [Bind ("isAnalog")] get; }

		[Abstract]
		[Export ("canWrap")]
		bool CanWrap { get; }

		[Abstract]
		[Export ("lastValueTimestamp")]
		double LastValueTimestamp { get; }

		[Abstract]
		[Export ("lastValueLatency")]
		double LastValueLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCPhysicalInputElement {
		[Abstract]
		[NullAllowed, Export ("sfSymbolsName")]
		string SfSymbolsName { get; }

		[Abstract]
		[NullAllowed, Export ("localizedName")]
		string LocalizedName { get; }

		[Abstract]
		[Export ("aliases")]
		NSSet<NSString> Aliases { get; }
	}

	interface IGCPressedStateInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCPressedStateInput {
		[Abstract]
		[NullAllowed, Export ("pressedDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCPressedStateInput, bool> PressedDidChangeHandler { get; set; }

		[Abstract]
		[Export ("pressed")]
		bool Pressed { [Bind ("isPressed")] get; }

		[Abstract]
		[Export ("lastPressedStateTimestamp")]
		double LastPressedStateTimestamp { get; }

		[Abstract]
		[Export ("lastPressedStateLatency")]
		double LastPressedStateLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCRelativeInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCRelativeInput {
		[Abstract]
		[NullAllowed, Export ("deltaDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCRelativeInput, float> DeltaDidChangeHandler { get; set; }

		[Abstract]
		[Export ("delta")]
		float Delta { get; }

		[Abstract]
		[Export ("analog")]
		bool Analog { [Bind ("isAnalog")] get; }

		[Abstract]
		[Export ("lastDeltaTimestamp")]
		double LastDeltaTimestamp { get; }

		[Abstract]
		[Export ("lastDeltaLatency")]
		double LastDeltaLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 4)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCSwitchElement : IGCPhysicalInputElement { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCSwitchElement : GCPhysicalInputElement {
		[Abstract]
		[Export ("positionInput")]
		IGCSwitchPositionInput PositionInput { get; }
	}

	interface IGCSwitchPositionInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCSwitchPositionInput {
		[Abstract]
		[NullAllowed, Export ("positionDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCSwitchPositionInput, nint> PositionDidChangeHandler { get; set; }

		[Abstract]
		[Export ("position")]
		nint Position { get; }

		[Abstract]
		[Export ("positionRange")]
		NSRange PositionRange { get; }

		[Abstract]
		[Export ("sequential")]
		bool Sequential { [Bind ("isSequential")] get; }

		[Abstract]
		[Export ("canWrap")]
		bool CanWrap { get; }

		[Abstract]
		[Export ("lastPositionTimestamp")]
		double LastPositionTimestamp { get; }

		[Abstract]
		[Export ("lastPositionLatency")]
		double LastPositionLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCTouchedStateInput { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
	[Protocol]
	interface GCTouchedStateInput {
		[Abstract]
		[NullAllowed, Export ("touchedDidChangeHandler", ArgumentSemantic.Copy)]
		Action<IGCPhysicalInputElement, IGCTouchedStateInput, bool> TouchedDidChangeHandler { get; set; }

		[Abstract]
		[Export ("touched")]
		bool Touched { [Bind ("isTouched")] get; }

		[Abstract]
		[Export ("lastTouchedStateTimestamp")]
		double LastTouchedStateTimestamp { get; }

		[Abstract]
		[Export ("lastTouchedStateLatency")]
		double LastTouchedStateLatency { get; }

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	[NoiOS, Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCGearShifterElement : GCPhysicalInputElement {
		[NullAllowed, Export ("patternInput")]
		IGCSwitchPositionInput PatternInput { get; }

		[NullAllowed, Export ("sequentialInput")]
		IGCRelativeInput SequentialInput { get; }
	}

	[Static]
	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	interface GCControllerUserCustomizations {
		[Notification, Field ("GCControllerUserCustomizationsDidChangeNotification")]
		NSString DidChangeNotification { get; }
	}

#if !XAMCORE_5_0
	[TV (18, 0)]
#if __TVOS__
	[Obsolete ("This enum does not exist on this platform.")]
#endif
#endif
	[NoMac, iOS (18, 0), MacCatalyst (18, 0)]
	[Native]
	enum GCUIEventTypes : ulong {
		None = 0U,
		Gamepad = (1U << 0),
	}

#if IOS || MACCATALYST
	[NoTV, NoMac, iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCEventInteraction : UIInteraction {
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("handledEventTypes")]
		GCUIEventTypes HandledEventTypes { get; set; }
	}
#endif // IOS || MACCATALYST

	[NoTV, NoMac, iOS (18, 0), NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCGameControllerActivationContext {
		[Export ("previousApplicationBundleID"), NullAllowed]
		string PreviousApplicationBundleId { get; }
	}

	[NoTV, NoMac, iOS (18, 0), NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	interface GCGameControllerSceneDelegate {
		[Abstract]
		[Export ("scene:didActivateGameControllerWithContext:")]
		void DidActivateGameController (UIScene scene, GCGameControllerActivationContext context);
	}

	[NoTV, NoMac, iOS (18, 0), NoMacCatalyst]
	[Category]
	[BaseType (typeof (UISceneConnectionOptions))]
	interface UISceneConnectionOptions_GameController {
		[Export ("gameControllerActivationContext")]
		[return: NullAllowed]
		GCGameControllerActivationContext GetGameControllerActivationContext ();
	}

	delegate void GCAxis2DInputValueDidChangeCallback (IGCPhysicalInputElement element, IGCAxis2DInput input, GCPoint2 point);

	[TV (17, 4), Mac (14, 3), iOS (17, 4), MacCatalyst (17, 4)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface GCAxis2DInput {
		[Abstract]
		[NullAllowed, Export ("valueDidChangeHandler", ArgumentSemantic.Copy)]
		GCAxis2DInputValueDidChangeCallback ValueDidChangeHandler { get; set; }

		[Abstract]
		[Export ("value")]
		GCPoint2 Value { get; }

		[Abstract]
		[Export ("analog")]
		bool Analog { [Bind ("isAnalog")] get; }

		[Abstract]
		[Export ("canWrap")]
		bool CanWrap { get; }

		[Abstract]
		[Export ("lastValueTimestamp")]
		double LastValueTimestamp { get; }

		[Abstract]
		[Export ("lastValueLatency")]
		double LastValueLatency { get; }

		[Abstract]
		[Export ("sources", ArgumentSemantic.Copy)]
		NSSet<IGCPhysicalInputSource> Sources { get; }
	}

	interface IGCAxis2DInput { }

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface GCPhysicalInputSource {
		[Abstract]
		[Export ("elementAliases", ArgumentSemantic.Copy)]
		NSSet<NSString> ElementAliases { get; }

		[Abstract]
		[NullAllowed, Export ("elementLocalizedName")]
		string ElementLocalizedName { get; }

		[Abstract]
		[NullAllowed, Export ("sfSymbolsName")]
		string SfSymbolsName { get; }

		[Abstract]
		[Export ("direction")]
		GCPhysicalInputSourceDirection Direction { get; }
	}

	interface IGCPhysicalInputSource { }

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (GCControllerInputState))]
	[DisableDefaultCtor]
	interface GCControllerLiveInput : GCDevicePhysicalInput {
		[NullAllowed, Export ("unmappedInput")]
		GCControllerLiveInput UnmappedInput { get; }

		// 'new' because this is also implemented in GCDevicePhysicalInput, but with a less defined property type (IGCDevicePhysicalInputState)
		[Export ("capture")]
		new GCControllerInputState Capture { get; }

		[NullAllowed, Export ("nextInputState")]
		// The property type is both GCControllerInputState + implements the GCDevicePhysicalInputStateDiff protocol,
		// which can't be expressed in C#. Choosing to bind as GCControllerInputState.
		// 'new' because this is also implemented in GCDevicePhysicalInput, but with a less defined property type (NSObject)
		new GCControllerInputState NextInputState { get; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GCControllerInputState : GCDevicePhysicalInputState {
	}
}

namespace Foundation {
	using GameController;

	partial interface NSValue {
		[TV (17, 4), Mac (14, 3), iOS (17, 4), MacCatalyst (17, 4)]
		[Static]
		[Export ("valueWithGCPoint2:")]
		NSValue FromGCPoint2 (GCPoint2 point);

		[TV (17, 4), Mac (14, 3), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("GCPoint2Value")]
		GCPoint2 GCPoint2Value { get; }
	}
}
