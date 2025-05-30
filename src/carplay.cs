//
// CarPlay bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2018-2019 Microsoft Corporation. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using CoreGraphics;
using MapKit;

namespace CarPlay {

	// Just to please the generator that at this point does not know the hierarchy
	interface NSUnitLength : NSUnit { }
	interface NSUnitAngle : NSUnit { }

	/// <summary>Enumerates the styles for a <see cref="CarPlay.CPAlert" /> object's action button.</summary>
	[NoTV, NoMac]
	[Native]
	enum CPAlertActionStyle : ulong {
		/// <summary>The default style.</summary>
		Default = 0,
		/// <summary>The style used when providing a cancel action for the alert.</summary>
		Cancel,
		/// <summary>The style used when the action is destructive, such as deleting data.</summary>
		Destructive,
	}

	/// <summary>Enumerates the kinds of <see cref="CarPlay.CPBarButton" />.</summary>
	[NoTV, NoMac]
	[Native]
	enum CPBarButtonType : ulong {
		/// <summary>The button displays text.</summary>
		Text,
		/// <summary>The button displays an image.</summary>
		Image,
	}

	/// <summary>Enumerates the directions of panning the navigation map.</summary>
	[Flags, NoTV, NoMac]
	[Native]
	enum CPPanDirection : long {
		/// <summary>No pan.</summary>
		None = 0,
		/// <summary>Towards the left side of the map.</summary>
		Left = 1L << 0,
		/// <summary>Towards the right side of the map.</summary>
		Right = 1L << 1,
		/// <summary>Towards the top of the map.</summary>
		Up = 1L << 2,
		/// <summary>Towards the bottom of the map.</summary>
		Down = 1L << 3,
	}

	/// <summary>Enumerates the reasons why a navigation alert was dismissed.</summary>
	[NoTV, NoMac]
	[Native]
	enum CPNavigationAlertDismissalContext : ulong {
		/// <summary>The alert had expired.</summary>
		Timeout = 0,
		/// <summary>The user dismissed the alert.</summary>
		UserDismissed,
		/// <summary>The system was responsible for dismissing the alert.</summary>
		SystemDismissed,
	}

	/// <summary>Enumerates the reasons why the current trip has been paused.</summary>
	[NoTV, NoMac]
	[Native]
	enum CPTripPauseReason : ulong {
		/// <summary>The car has arrived at the trip destinatioj.</summary>
		Arrived = 1,
		/// <summary>The data for the trip is still loading.</summary>
		Loading = 2,
		/// <summary>The system is trying to locate the car.</summary>
		Locating = 3,
		/// <summary>The system is currently rerouting the trip.</summary>
		Rerouting = 4,
		/// <summary>The car is not on the trip route.</summary>
		ProceedToRoute = 5,
	}

	/// <summary>Flagging enumeration that describes how the UI might be limited.</summary>
	[NoTV, NoMac]
	[Flags]
	[Native]
	enum CPLimitableUserInterface : ulong {
		/// <summary>The keyboard may be limited.</summary>
		Keyboard = 1uL << 0,
		/// <summary>The length of lists may be limited.</summary>
		Lists = 1uL << 1,
	}

	[NoTV, NoMac, iOS (13, 0)]
	[Flags]
	[Native]
	enum CPContentStyle : ulong {
		Light = 1uL << 0,
		Dark = 1uL << 1,
	}

	[NoTV, NoMac]
	[Flags]
	[Native]
	enum CPManeuverDisplayStyle : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		LeadingSymbol,
		/// <summary>To be added.</summary>
		TrailingSymbol,
		/// <summary>To be added.</summary>
		SymbolOnly,
		/// <summary>To be added.</summary>
		InstructionOnly,
	}

	[NoTV, NoMac]
	[Native]
	enum CPTimeRemainingColor : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Green,
		/// <summary>To be added.</summary>
		Orange,
		/// <summary>To be added.</summary>
		Red,
	}

	[NoTV, NoMac]
	[Native]
	enum CPTripEstimateStyle : ulong {
		/// <summary>To be added.</summary>
		Light = 0,
		/// <summary>To be added.</summary>
		Dark,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPBarButtonStyle : long {
		None,
		Rounded,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPInformationTemplateLayout : long {
		Leading = 0,
		TwoColumn,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPListItemAccessoryType : long {
		None = 0,
		DisclosureIndicator,
		Cloud,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPListItemPlayingIndicatorLocation : long {
		Leading = 0,
		Trailing,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPMessageLeadingItem : long {
		None = 0,
		Pin,
		Star,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPMessageTrailingItem : long {
		None,
		Mute,
	}

	[NoTV, NoMac, iOS (14, 0)]
	[Native]
	public enum CPTextButtonStyle : long {
		Normal = 0,
		Cancel,
		Confirm,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum CPAssistantCellPosition : long {
		Top = 0,
		Bottom,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum CPAssistantCellVisibility : long {
		Off = 0,
		WhileLimitedUIActive,
		Always,
	}

	[iOS (15, 0), MacCatalyst (15, 0), NoMac, NoTV]
	[Native]
	public enum CPAssistantCellActionType : long {
		PlayMedia = 0,
		StartCall,
	}

	[NoTV, NoMac, iOS (15, 4), MacCatalyst (15, 4)]
	[Native]
	public enum CPInstrumentClusterSetting : ulong {
		Unspecified,
		Enabled,
		Disabled,
		UserPreference,
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum CPLaneStatus : long {
		NotGood = 0,
		Good,
		Preferred,
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum CPManeuverType : ulong {
		NoTurn = 0,
		LeftTurn = 1,
		RightTurn = 2,
		StraightAhead = 3,
		UTurn = 4,
		FollowRoad = 5,
		EnterRoundabout = 6,
		ExitRoundabout = 7,
		OffRamp = 8,
		OnRamp = 9,
		ArriveEndOfNavigation = 10,
		StartRoute = 11,
		ArriveAtDestination = 12,
		KeepLeft = 13,
		KeepRight = 14,
		EnterFerry = 15,
		ExitFerry = 16,
		ChangeFerry = 17,
		StartRouteWithUTurn = 18,
		UTurnAtRoundabout = 19,
		LeftTurnAtEnd = 20,
		RightTurnAtEnd = 21,
		HighwayOffRampLeft = 22,
		HighwayOffRampRight = 23,
		ArriveAtDestinationLeft = 24,
		ArriveAtDestinationRight = 25,
		UTurnWhenPossible = 26,
		ArriveEndOfDirections = 27,
		RoundaboutExit1 = 28,
		RoundaboutExit2 = 29,
		RoundaboutExit3 = 30,
		RoundaboutExit4 = 31,
		RoundaboutExit5 = 32,
		RoundaboutExit6 = 33,
		RoundaboutExit7 = 34,
		RoundaboutExit8 = 35,
		RoundaboutExit9 = 36,
		RoundaboutExit10 = 37,
		RoundaboutExit11 = 38,
		RoundaboutExit12 = 39,
		RoundaboutExit13 = 40,
		RoundaboutExit14 = 41,
		RoundaboutExit15 = 42,
		RoundaboutExit16 = 43,
		RoundaboutExit17 = 44,
		RoundaboutExit18 = 45,
		RoundaboutExit19 = 46,
		SharpLeftTurn = 47,
		SharpRightTurn = 48,
		SlightLeftTurn = 49,
		SlightRightTurn = 50,
		ChangeHighway = 51,
		ChangeHighwayLeft = 52,
		ChangeHighwayRight = 53,
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum CPJunctionType : ulong {
		Intersection = 0,
		Roundabout = 1,
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum CPTrafficSide : ulong {
		Right = 0,
		Left = 1,
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[Native]
	public enum CPManeuverState : long {
		Continue = 0,
		Initial,
		Prepare,
		Execute,
	}

	/// <summary>An action that is displayed on a button in an alert.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPAlertAction : NSSecureCoding {

		[Export ("initWithTitle:style:handler:")]
		NativeHandle Constructor (string title, CPAlertActionStyle style, Action<CPAlertAction> handler);

		[NoTV, NoMac, iOS (16, 0)]
		[Export ("initWithTitle:color:handler:")]
		NativeHandle Constructor (string title, UIColor color, Action<CPAlertAction> handler);

		[Export ("title")]
		string Title { get; }

		[Export ("style", ArgumentSemantic.Assign)]
		CPAlertActionStyle Style { get; }

		[Export ("handler", ArgumentSemantic.Copy)]
		Action<CPAlertAction> Handler { get; }

		[NullAllowed]
		[NoTV, NoMac, iOS (16, 0)]
		[Export ("color", ArgumentSemantic.Copy)]
		UIColor Color { get; }
	}

	delegate void CPBarButtonHandler (CPBarButton button);

	/// <summary>A button in the navigation bar.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPBarButton : NSSecureCoding {

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("initWithType:handler:")]
		NativeHandle Constructor (CPBarButtonType type, [NullAllowed] Action<CPBarButton> handler);

		/// <summary>Gets or sets whether the button is enabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("buttonType", ArgumentSemantic.Assign)]
		CPBarButtonType ButtonType { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; set; }

		[NullAllowed, Export ("title")]
		string Title { get; set; }

		[iOS (14, 0)]
		[Export ("initWithImage:handler:")]
		NativeHandle Constructor (UIImage image, [NullAllowed] CPBarButtonHandler handler);

		[iOS (14, 0)]
		[Export ("initWithTitle:handler:")]
		NativeHandle Constructor (string title, [NullAllowed] CPBarButtonHandler handler);

		[iOS (14, 0)]
		[Export ("buttonStyle", ArgumentSemantic.Assign)]
		CPBarButtonStyle ButtonStyle { get; set; }
	}

	/// <summary>Interface defining necessary methods for the <see cref="CarPlay.ICPBarButtonProviding" /> protocol.</summary>
	interface ICPBarButtonProviding { }

	/// <summary>Interface defining necessary methods for the <see cref="CarPlay.ICPBarButtonProviding" /> protocol.</summary>
	/// <remarks>To be added.</remarks>
	[NoTV, NoMac]
	[Protocol]
	interface CPBarButtonProviding {

		/// <summary>Developers must override this with the array of <see cref="CarPlay.CPBarButton" /> objects on the leading part of the navigation bar.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("leadingNavigationBarButtons", ArgumentSemantic.Strong)]
		CPBarButton [] LeadingNavigationBarButtons { get; set; }

		/// <summary>Developers must override this with the array of <see cref="CarPlay.CPBarButton" /> objects on the trailing part of the navigation bar..</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("trailingNavigationBarButtons", ArgumentSemantic.Strong)]
		CPBarButton [] TrailingNavigationBarButtons { get; set; }

		[Abstract]
		[NullAllowed, Export ("backButton", ArgumentSemantic.Strong)]
		CPBarButton BackButton { get; set; }
	}

	/// <summary>A menu item displayed in a <see cref="CarPlay.CPGridTemplate" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPGridButton : NSSecureCoding {

		[Export ("initWithTitleVariants:image:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string [] titleVariants, UIImage image, [NullAllowed] Action<CPGridButton> handler);

		/// <summary>Gets or sets whether the button is enabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("image")]
		UIImage Image { get; }

		[Export ("titleVariants")]
		string [] TitleVariants { get; }
	}

	/// <summary>
	///       <see cref="CarPlay.CPTemplate" /> subclass that displays a menu in grid form.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPGridTemplate : CPBarButtonProviding {

		[Export ("initWithTitle:gridButtons:")]
		NativeHandle Constructor ([NullAllowed] string title, CPGridButton [] gridButtons);

		[Export ("gridButtons")]
		CPGridButton [] GridButtons { get; }

		[Export ("title")]
		string Title { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("updateGridButtons:")]
		void UpdateGridButtons (CPGridButton [] gridButtons);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("updateTitle:")]
		void UpdateTitle (string title);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CPGridTemplateMaximumItems")]
		nuint MaximumItems { get; }
	}

	/// <summary>A system-created controller object (similar, but not derived from, <see cref="UIKit.UIViewController" />).</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPInterfaceController {

		/// <summary>An instance of the CarPlay.ICPInterfaceControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CarPlay.ICPInterfaceControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPInterfaceControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[iOS (13, 0)]
		[Export ("prefersDarkUserInterfaceStyle")]
		bool PrefersDarkUserInterfaceStyle { get; set; }

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("setRootTemplate:animated:")]
		void SetRootTemplate (CPTemplate rootTemplate, bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("pushTemplate:animated:")]
		void PushTemplate (CPTemplate templateToPush, bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("popTemplateAnimated:")]
		void PopTemplate (bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("popToRootTemplateAnimated:")]
		void PopToRootTemplate (bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("popToTemplate:animated:")]
		void PopToTemplate (CPTemplate targetTemplate, bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("presentTemplate:animated:")]
		void PresentTemplate (CPTemplate templateToPresent, bool animated);

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("dismissTemplateAnimated:")]
		void DismissTemplate (bool animated);

		[Export ("presentedTemplate")]
		[NullAllowed]
		CPTemplate PresentedTemplate { get; }

		[Export ("rootTemplate")]
		CPTemplate RootTemplate { get; }

		[NullAllowed, Export ("topTemplate", ArgumentSemantic.Strong)]
		CPTemplate TopTemplate { get; }

		[Export ("templates", ArgumentSemantic.Strong)]
		CPTemplate [] Templates { get; }

		[iOS (14, 0)]
		[Async]
		[Export ("setRootTemplate:animated:completion:")]
		void SetRootTemplate (CPTemplate rootTemplate, bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("pushTemplate:animated:completion:")]
		void PushTemplate (CPTemplate templateToPush, bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("popTemplateAnimated:completion:")]
		void PopTemplate (bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("popToRootTemplateAnimated:completion:")]
		void PopToRootTemplate (bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("popToTemplate:animated:completion:")]
		void PopToTemplate (CPTemplate targetTemplate, bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("presentTemplate:animated:completion:")]
		void PresentTemplate (CPTemplate templateToPresent, bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Async]
		[Export ("dismissTemplateAnimated:completion:")]
		void DismissTemplate (bool animated, [NullAllowed] Action<bool, NSError> completion);

		[iOS (14, 0)]
		[Export ("carTraitCollection", ArgumentSemantic.Strong)]
		UITraitCollection CarTraitCollection { get; }
	}

	/// <summary>Delegate object for <see cref="CarPlay.CPInterfaceController" /> objects.</summary>
	interface ICPInterfaceControllerDelegate { }

	/// <summary>Default implementation of <see cref="CarPlay.ICPInterfaceControllerDelegate" />, the delegate object for <see cref="CarPlay.CPInterfaceController" /> objects.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPInterfaceControllerDelegate {

		/// <param name="aTemplate">The template that will appear.</param>
		/// <param name="animated">
		///           <see langword="true" /> if the transition is automated. Otherwise, <see langword="false" />.</param>
		/// <summary>Method that is called when a template is about to appear.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("templateWillAppear:animated:")]
		void TemplateWillAppear (CPTemplate aTemplate, bool animated);

		/// <param name="aTemplate">The template that appeared.</param>
		/// <param name="animated">
		///           <see langword="true" /> if the transition is automated. Otherwise, <see langword="false" />.</param>
		/// <summary>Method that is called when a template appears.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("templateDidAppear:animated:")]
		void TemplateDidAppear (CPTemplate aTemplate, bool animated);

		/// <param name="aTemplate">The template that will disappear.</param>
		/// <param name="animated">
		///           <see langword="true" /> if the transition is automated. Otherwise, <see langword="false" />.</param>
		/// <summary>Method that is called when a template is about to disappear.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("templateWillDisappear:animated:")]
		void TemplateWillDisappear (CPTemplate aTemplate, bool animated);

		/// <param name="aTemplate">The template that disappeared.</param>
		/// <param name="animated">
		///           <see langword="true" /> if the transition is automated. Otherwise, <see langword="false" />.</param>
		/// <summary>Method that is called when a template disappears.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("templateDidDisappear:animated:")]
		void TemplateDidDisappear (CPTemplate aTemplate, bool animated);
	}

	/// <summary>Application delegate for the required methods of the <see cref="CarPlay.CPApplicationDelegate" /> protocol.</summary>
	interface ICPApplicationDelegate { }

	/// <summary>The application delegate for CarPlay applications.</summary>
	[Introduced (PlatformName.iOS, 12, 0)]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'CPTemplateApplicationSceneDelegate' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CPTemplateApplicationSceneDelegate' instead.")]
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPApplicationDelegate : UIApplicationDelegate {

		/// <param name="application">To be added.</param>
		/// <param name="interfaceController">To be added.</param>
		/// <param name="window">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("application:didConnectCarInterfaceController:toWindow:")]
		void DidConnectCarInterfaceController (UIApplication application, CPInterfaceController interfaceController, CPWindow window);

		/// <param name="application">To be added.</param>
		/// <param name="interfaceController">To be added.</param>
		/// <param name="window">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("application:didDisconnectCarInterfaceController:fromWindow:")]
		void DidDisconnectCarInterfaceController (UIApplication application, CPInterfaceController interfaceController, CPWindow window);

		/// <param name="application">The application in which a navigation alert was selected.</param>
		/// <param name="navigationAlert">The selected navigation alert.</param>
		/// <summary>Method that is called when the user selects a navigation alert.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("application:didSelectNavigationAlert:")]
		void DidSelectNavigationAlert (UIApplication application, CPNavigationAlert navigationAlert);

		/// <param name="application">The application in which a maneuver was selected.</param>
		/// <param name="maneuver">The selected maneuver.</param>
		/// <summary>Method that is called when the user selects a maneuver.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("application:didSelectManeuver:")]
		void DidSelectManeuver (UIApplication application, CPManeuver maneuver);
	}

	/// <summary>A line in a <see cref="CarPlay.CPListTemplate" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPListItem : CPSelectableListItem, NSSecureCoding {
		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("initWithText:detailText:image:showsDisclosureIndicator:")]
		NativeHandle Constructor ([NullAllowed] string text, [NullAllowed] string detailText, [NullAllowed] UIImage image, bool showsDisclosureIndicator);

		[Export ("initWithText:detailText:image:")]
		NativeHandle Constructor ([NullAllowed] string text, [NullAllowed] string detailText, [NullAllowed] UIImage image);

		[Export ("initWithText:detailText:")]
		NativeHandle Constructor ([NullAllowed] string text, [NullAllowed] string detailText);

		[iOS (14, 0)]
		[Export ("initWithText:detailText:image:accessoryImage:accessoryType:")]
		NativeHandle Constructor ([NullAllowed] string text, [NullAllowed] string detailText, [NullAllowed] UIImage image, [NullAllowed] UIImage accessoryImage, CPListItemAccessoryType accessoryType);

		[NullAllowed, Export ("detailText")]
		string DetailText { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; }

		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacCatalyst, 14, 0)]
		[Export ("showsDisclosureIndicator")]
		bool ShowsDisclosureIndicator { get; }

		[iOS (14, 0)]
		[Export ("explicitContent")]
		bool IsExplicitContent { [Bind ("isExplicitContent")] get; set; }

		[iOS (14, 0)]
		[Export ("playbackProgress")]
		nfloat PlaybackProgress { get; set; }

		[iOS (14, 0)]
		[Export ("playing")]
		bool IsPlaying { [Bind ("isPlaying")] get; set; }

		[iOS (14, 0)]
		[Export ("playingIndicatorLocation", ArgumentSemantic.Assign)]
		CPListItemPlayingIndicatorLocation PlayingIndicatorLocation { get; set; }

		[iOS (14, 0)]
		[Static]
		[Export ("maximumImageSize")]
		CGSize MaximumImageSize { get; }

		[iOS (14, 0)]
		[Export ("setDetailText:")]
		void SetDetailText ([NullAllowed] string detailText);

		[iOS (14, 0)]
		[Export ("setImage:")]
		void SetImage ([NullAllowed] UIImage image);

		[iOS (14, 0)]
		[Export ("setAccessoryImage:")]
		void SetAccessoryImage ([NullAllowed] UIImage accessoryImage);

		[iOS (14, 0)]
		[Export ("accessoryType", ArgumentSemantic.Assign)]
		CPListItemAccessoryType AccessoryType { get; set; }

		[iOS (14, 0)]
		[NullAllowed, Export ("accessoryImage", ArgumentSemantic.Strong)]
		UIImage AccessoryImage { get; }

		[iOS (14, 0)]
		[Export ("setText:")]
		void SetText (string text);
	}

	/// <summary>Organizational element within a <see cref="CarPlay.CPListTemplate" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPListSection : NSSecureCoding {

#if !XAMCORE_5_0
		/// <param name="items">To be added.</param>
		/// <param name="header">To be added.</param>
		/// <param name="sectionIndexTitle">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Wrap ("base (true ? throw new InvalidOperationException (Constants.BrokenBinding) : NSObjectFlag.Empty)")]
		[Obsolete ("Use '.ctor (ICPListTemplateItem [], string, string)' constructor instead. Warning: this will throw InvalidOperationException at runtime.")]
		NativeHandle Constructor (CPListItem [] items, [NullAllowed] string header, [NullAllowed] string sectionIndexTitle);
#endif

#if !XAMCORE_5_0
		/// <param name="items">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Wrap ("base (true ? throw new InvalidOperationException (Constants.BrokenBinding) : NSObjectFlag.Empty)")]
		[Obsolete ("Use '.ctor (ICPListTemplateItem [], string, string)' constructor instead. Warning: this will throw InvalidOperationException at runtime.")]
		NativeHandle Constructor (CPListItem [] items);
#endif

		[Export ("initWithItems:header:sectionIndexTitle:")]
		NativeHandle Constructor (ICPListTemplateItem [] items, [NullAllowed] string header, [NullAllowed] string sectionIndexTitle);

		[Export ("initWithItems:")]
		NativeHandle Constructor (ICPListTemplateItem [] items);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithItems:header:headerSubtitle:headerImage:headerButton:sectionIndexTitle:")]
		NativeHandle Constructor (ICPListTemplateItem [] items, string header, [NullAllowed] string headerSubtitle, [NullAllowed] UIImage headerImage, [NullAllowed] CPButton headerButton, [NullAllowed] string sectionIndexTitle);

		[NullAllowed, Export ("header")]
		string Header { get; }

		[NullAllowed, Export ("sectionIndexTitle")]
		string SectionIndexTitle { get; }

#if !XAMCORE_5_0
		/// <summary>The contents of the section.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("true ? throw new InvalidOperationException (Constants.BrokenBinding) : new NSArray ()", IsVirtual = true)]
		[Obsolete ("Use 'Items2 : ICPListTemplateItem []' instead.")]
		CPListItem [] Items { get; }
#endif

		[Export ("items", ArgumentSemantic.Copy)]
#if !XAMCORE_5_0
		ICPListTemplateItem [] Items2 { get; }
#else
		ICPListTemplateItem [] Items { get; }
#endif

		[iOS (14, 0)]
		[Export ("indexOfItem:")]
		nuint GetIndex (ICPListTemplateItem item);

		[iOS (14, 0)]
		[Export ("itemAtIndex:")]
		ICPListTemplateItem GetItem (nuint index);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed]
		[Export ("headerSubtitle")]
		string HeaderSubtitle { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed]
		[Export ("headerImage", ArgumentSemantic.Copy)]
		UIImage HeaderImage { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed]
		[Export ("headerButton", ArgumentSemantic.Copy)]
		CPButton HeaderButton { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("CPMaximumListSectionImageSize")]
		CGSize MaximumImageSize { get; }
	}

	/// <summary>
	///       <see cref="CarPlay.CPTemplate" /> that presents a hierarchical menu of choices.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPListTemplate : CPBarButtonProviding {

		[Export ("initWithTitle:sections:")]
		NativeHandle Constructor ([NullAllowed] string title, CPListSection [] sections);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithTitle:sections:assistantCellConfiguration:")]
		NativeHandle Constructor ([NullAllowed] string title, CPListSection [] sections, [NullAllowed] CPAssistantCellConfiguration assistantCellConfiguration);

		/// <summary>An instance of the CarPlay.ICPListTemplateDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CarPlay.ICPListTemplateDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPListTemplateDelegate Delegate { get; set; }

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("sections", ArgumentSemantic.Copy)]
		CPListSection [] Sections { get; }

		[NullAllowed, Export ("title")]
		string Title { get; }

		[Export ("updateSections:")]
		void UpdateSections (CPListSection [] sections);

		[iOS (14, 0)]
		[Static]
		[Export ("maximumItemCount")]
		nuint MaximumItemCount { get; }

		[iOS (14, 0)]
		[Static]
		[Export ("maximumSectionCount")]
		nuint MaximumSectionCount { get; }

		[iOS (14, 0)]
		[Export ("sectionCount")]
		nuint SectionCount { get; }

		[iOS (14, 0)]
		[Export ("itemCount")]
		nuint ItemCount { get; }

		[iOS (14, 0)]
		[Export ("indexPathForItem:")]
		[return: NullAllowed]
		NSIndexPath GetIndexPath (ICPListTemplateItem item);

		[iOS (14, 0)]
		[Export ("emptyViewTitleVariants", ArgumentSemantic.Copy)]
		string [] EmptyViewTitleVariants { get; set; }

		[iOS (14, 0)]
		[Export ("emptyViewSubtitleVariants", ArgumentSemantic.Copy)]
		string [] EmptyViewSubtitleVariants { get; set; }

		[NullAllowed]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("assistantCellConfiguration", ArgumentSemantic.Strong)]
		CPAssistantCellConfiguration AssistantCellConfiguration { get; set; }

		[NullAllowed]
		[iOS (18, 4), MacCatalyst (18, 4)]
		[Export ("showsSpinnerWhileEmpty", ArgumentSemantic.Assign)]
		bool ShowsSpinnerWhileEmpty { get; set; }
	}

	/// <summary>Delegate object for <see cref="CarPlay.CPListTemplate" /> objects.</summary>
	interface ICPListTemplateDelegate { }

	/// <summary>Abstract implementation of <see cref="CarPlay.ICPListTemplateDelegate" />, the delegate object for <see cref="CarPlay.CPListTemplate" /> objects.</summary>
	[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
	[NoTV, NoMac]
	[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'CPListItem.Handler' instead.")]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPListTemplateDelegate {

		/// <param name="listTemplate">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>Developers must override this method to react to the selection of a list item.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("listTemplate:didSelectListItem:completionHandler:")]
		void DidSelectListItem (CPListTemplate listTemplate, CPListItem item, Action completionHandler);
	}

	/// <summary>A step in a <see cref="CarPlay.CPTrip" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface CPManeuver : NSCopying, NSSecureCoding {

		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'CPManeuver.SymbolImage' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 0, message: "Use 'CPManeuver.SymbolImage' instead.")]
		[NullAllowed, Export ("symbolSet", ArgumentSemantic.Strong)]
		CPImageSet SymbolSet { get; set; }

		[NullAllowed]
		[iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("cardBackgroundColor", ArgumentSemantic.Strong)]
		UIColor CardBackgroundColor { get; set; }

		[iOS (13, 0)]
		[NullAllowed, Export ("symbolImage", ArgumentSemantic.Strong)]
		UIImage SymbolImage { get; set; }

		[Export ("instructionVariants", ArgumentSemantic.Copy)]
		string [] InstructionVariants { get; set; }

		[NullAllowed, Export ("initialTravelEstimates", ArgumentSemantic.Strong)]
		CPTravelEstimates InitialTravelEstimates { get; set; }

		[Export ("attributedInstructionVariants", ArgumentSemantic.Copy)]
		NSAttributedString [] AttributedInstructionVariants { get; set; }

		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }

		[NullAllowed, Export ("junctionImage", ArgumentSemantic.Strong)]
		UIImage JunctionImage { get; set; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("dashboardSymbolImage", ArgumentSemantic.Strong)]
		UIImage DashboardSymbolImage { get; set; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("dashboardJunctionImage", ArgumentSemantic.Strong)]
		UIImage DashboardJunctionImage { get; set; }

		[iOS (14, 0)]
		[Export ("dashboardInstructionVariants", ArgumentSemantic.Copy)]
		string [] DashboardInstructionVariants { get; set; }

		[iOS (14, 0)]
		[Export ("dashboardAttributedInstructionVariants", ArgumentSemantic.Copy)]
		NSAttributedString [] DashboardAttributedInstructionVariants { get; set; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("notificationSymbolImage", ArgumentSemantic.Strong)]
		UIImage NotificationSymbolImage { get; set; }

		[iOS (14, 0)]
		[Export ("notificationInstructionVariants", ArgumentSemantic.Copy)]
		string [] NotificationInstructionVariants { get; set; }

		[iOS (14, 0)]
		[Export ("notificationAttributedInstructionVariants", ArgumentSemantic.Copy)]
		NSAttributedString [] NotificationAttributedInstructionVariants { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("maneuverType", ArgumentSemantic.Assign)]
		CPManeuverType ManeuverType { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[NullAllowed]
		[Export ("roadFollowingManeuverVariants", ArgumentSemantic.Copy)]
		string [] RoadFollowingManeuverVariants { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("trafficSide", ArgumentSemantic.Assign)]
		CPTrafficSide TrafficSide { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("junctionType", ArgumentSemantic.Assign)]
		CPJunctionType JunctionType { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[NullAllowed]
		[Export ("junctionExitAngle", ArgumentSemantic.Copy)]
		NSMeasurement<NSUnitAngle> JunctionExitAngle { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[NullAllowed]
		[Export ("junctionElementAngles", ArgumentSemantic.Copy)]
		NSSet<NSMeasurement<NSUnitAngle>> JunctionElementAngles { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("linkedLaneGuidance", ArgumentSemantic.Assign)]
		CPLaneGuidance LinkedLaneGuidance { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("highwayExitLabel")]
		string HighwayExitLabel { get; set; }
	}

	/// <summary>A button displayed on the <see cref="CarPlay.CPMapTemplate" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPMapButton : NSSecureCoding {

		[Export ("initWithHandler:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] Action<CPMapButton> handler);

		/// <summary>Gets or sets whether the button is enabled.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Gets or sets whether the button is visible.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get; set; }

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; set; }

		[NullAllowed, Export ("focusedImage", ArgumentSemantic.Strong)]
		UIImage FocusedImage { get; set; }
	}

	/// <summary>
	///       <see cref="CarPlay.CPTemplate" /> subclass that displays a map.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPMapTemplate : CPBarButtonProviding {

		[Export ("guidanceBackgroundColor", ArgumentSemantic.Strong)]
		UIColor GuidanceBackgroundColor { get; set; }

		[Export ("tripEstimateStyle", ArgumentSemantic.Assign)]
		CPTripEstimateStyle TripEstimateStyle { get; set; }

		[Export ("mapButtons", ArgumentSemantic.Strong)]
		CPMapButton [] MapButtons { get; set; }

		[Export ("showTripPreviews:textConfiguration:")]
		void ShowTripPreviews (CPTrip [] tripPreviews, [NullAllowed] CPTripPreviewTextConfiguration textConfiguration);

		[Export ("showRouteChoicesPreviewForTrip:textConfiguration:")]
		void ShowRouteChoicesPreview (CPTrip tripPreview, [NullAllowed] CPTripPreviewTextConfiguration textConfiguration);

		[Export ("hideTripPreviews")]
		void HideTripPreviews ();

		[Export ("updateTravelEstimates:forTrip:")]
		void UpdateTravelEstimates (CPTravelEstimates estimates, CPTrip trip);

		[Export ("updateTravelEstimates:forTrip:withTimeRemainingColor:")]
		void UpdateTravelEstimates (CPTravelEstimates estimates, CPTrip trip, CPTimeRemainingColor timeRemainingColor);

		[Export ("startNavigationSessionForTrip:")]
		CPNavigationSession StartNavigationSession (CPTrip trip);

		[Export ("automaticallyHidesNavigationBar")]
		bool AutomaticallyHidesNavigationBar { get; set; }

		[Export ("hidesButtonsWithNavigationBar")]
		bool HidesButtonsWithNavigationBar { get; set; }

		/// <summary>Gets or sets the delegate object for the map.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakMapDelegate")]
		[NullAllowed]
		ICPMapTemplateDelegate MapDelegate { get; set; }

		[NullAllowed, Export ("mapDelegate", ArgumentSemantic.Weak)]
		NSObject WeakMapDelegate { get; set; }

		[Export ("showPanningInterfaceAnimated:")]
		void ShowPanningInterface (bool animated);

		[Export ("dismissPanningInterfaceAnimated:")]
		void DismissPanningInterface (bool animated);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("panningInterfaceVisible")]
		bool PanningInterfaceVisible { [Bind ("isPanningInterfaceVisible")] get; }

		[NullAllowed, Export ("currentNavigationAlert", ArgumentSemantic.Strong)]
		CPNavigationAlert CurrentNavigationAlert { get; }

		[Export ("presentNavigationAlert:animated:")]
		void PresentNavigationAlert (CPNavigationAlert navigationAlert, bool animated);

		[Async (XmlDocs = """
			<param name="animated">To be added.</param>
			<summary>Asynchronously dismisses the animation.</summary>
			<returns>To be added.</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("dismissNavigationAlertAnimated:completion:")]
		void DismissNavigationAlert (bool animated, Action<bool> completion);

		[iOS (14, 0)]
		[Export ("showTripPreviews:selectedTrip:textConfiguration:")]
		void ShowTripPreviews (CPTrip [] tripPreviews, [NullAllowed] CPTrip selectedTrip, [NullAllowed] CPTripPreviewTextConfiguration textConfiguration);
	}

	/// <summary>Delegate object for <see cref="CarPlay.CPMapTemplate" /> objects.</summary>
	interface ICPMapTemplateDelegate { }

	/// <summary>Default implementation of <see cref="CarPlay.ICPMapTemplateDelegate" />, providing the delegate object for <see cref="CarPlay.CPMapTemplate" /> objects.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPMapTemplateDelegate {

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("mapTemplateShouldProvideNavigationMetadata:")]
		bool ShouldProvideNavigationMetadata (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map to query.</param>
		/// <param name="maneuver">The maneuver about which to query.</param>
		/// <summary>Method that is called to determine whether a navigation maneuver notification should be shown when the app is running in the background.</summary>
		/// <returns>
		///           <see langword="true" /> if the notification should be shown. Otherwise, <see langword="false" />.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:shouldShowNotificationForManeuver:")]
		bool ShouldShowNotificationForManeuver (CPMapTemplate mapTemplate, CPManeuver maneuver);

		/// <param name="mapTemplate">The template for the map to query.</param>
		/// <param name="maneuver">To be added.</param>
		/// <param name="travelEstimates">The travel estimates about which to query.</param>
		/// <summary>Method that is called to determine whether the specified travel estimate updates should be shown when the app is running in the background.</summary>
		/// <returns>
		///           <see langword="true" /> if the specified travel estimate updates should be shown. Otherwise, <see langword="false" />.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:shouldUpdateNotificationForManeuver:withTravelEstimates:")]
		bool ShouldUpdateNotificationForManeuver (CPMapTemplate mapTemplate, CPManeuver maneuver, CPTravelEstimates travelEstimates);

		/// <param name="mapTemplate">The template for the map to query.</param>
		/// <param name="navigationAlert">The navigation alert about which to query.</param>
		/// <summary>Method that is called to determine whether a navigation alert should be shown when the app is running in the background.</summary>
		/// <returns>
		///           <see langword="true" /> if the navigation alert should be shown. Otherwise, <see langword="false" />.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:shouldShowNotificationForNavigationAlert:")]
		bool ShouldShowNotificationForNavigationAlert (CPMapTemplate mapTemplate, CPNavigationAlert navigationAlert);

		/// <param name="mapTemplate">The template for the map for which a panning interface was shown.</param>
		/// <summary>Method that is called when a panning interface is shown.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplateDidShowPanningInterface:")]
		void DidShowPanningInterface (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map on which a panning interface will be dismissed.</param>
		/// <summary>Method that is called just before a panning interface is dismissed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplateWillDismissPanningInterface:")]
		void WillDismissPanningInterface (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map whose panning interface was dismissed.</param>
		/// <summary>Method that is called when a panning interface is dismissed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplateDidDismissPanningInterface:")]
		void DidDismissPanningInterface (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map for which a pan was started.</param>
		/// <param name="direction">The direction of the pan.</param>
		/// <summary>Method that is called when a pan begins.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:panBeganWithDirection:")]
		void PanBegan (CPMapTemplate mapTemplate, CPPanDirection direction);

		/// <param name="mapTemplate">The template for the map for which a pan was ended.</param>
		/// <param name="direction">The direction of the pan.</param>
		/// <summary>Method that is called when a pan ends.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:panEndedWithDirection:")]
		void PanEnded (CPMapTemplate mapTemplate, CPPanDirection direction);

		/// <param name="mapTemplate">The template for the map to pan.</param>
		/// <param name="direction">The direction to pan.</param>
		/// <summary>Pans the map.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:panWithDirection:")]
		void Pan (CPMapTemplate mapTemplate, CPPanDirection direction);

		/// <param name="mapTemplate">The template for the map that is panning.</param>
		/// <summary>Method that is called when a pan gesture starts.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplateDidBeginPanGesture:")]
		void DidBeginPanGesture (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map whose pan gesture was updated.</param>
		/// <param name="translation">To be added.</param>
		/// <param name="velocity">The pan velocity.</param>
		/// <summary>Method that is called when a pan gesture is updated.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:didUpdatePanGestureWithTranslation:velocity:")]
		void DidUpdatePanGesture (CPMapTemplate mapTemplate, CGPoint translation, CGPoint velocity);

		/// <param name="mapTemplate">The template for the map whose pan gesture ended.</param>
		/// <param name="velocity">To be added.</param>
		/// <summary>Method that is called when a panning interface ends.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:didEndPanGestureWithVelocity:")]
		void DidEndPanGesture (CPMapTemplate mapTemplate, CGPoint velocity);

		/// <param name="mapTemplate">The template for the map for which a navigation alert will be shown.</param>
		/// <param name="navigationAlert">The navigation alert that will be shown.</param>
		/// <summary>Method that is called just before a navigation alert is shown.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:willShowNavigationAlert:")]
		void WillShowNavigationAlert (CPMapTemplate mapTemplate, CPNavigationAlert navigationAlert);

		/// <param name="mapTemplate">The template for the map for which a navigation alert was shown.</param>
		/// <param name="navigationAlert">The alert that was shown.</param>
		/// <summary>Method that is called when a navigation alert is shown.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:didShowNavigationAlert:")]
		void DidShowNavigationAlert (CPMapTemplate mapTemplate, CPNavigationAlert navigationAlert);

		/// <param name="mapTemplate">The template for the map for which a navigation alert will be dismissed.</param>
		/// <param name="navigationAlert">The alert that will be dismissed.</param>
		/// <param name="dismissalContext">The reason the alert will be dismissed.</param>
		/// <summary>Method that is called just before a navigation alert is dismissed.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:willDismissNavigationAlert:dismissalContext:")]
		void WillDismissNavigationAlert (CPMapTemplate mapTemplate, CPNavigationAlert navigationAlert, CPNavigationAlertDismissalContext dismissalContext);

		/// <param name="mapTemplate">The template for the map whose navigation alert was canceled.</param>
		/// <param name="navigationAlert">The alert that was canceled.</param>
		/// <param name="dismissalContext">The reason the alert was dismissed.</param>
		/// <summary>Method that is called when a navigation alert is canceled.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:didDismissNavigationAlert:dismissalContext:")]
		void DidDismissNavigationAlert (CPMapTemplate mapTemplate, CPNavigationAlert navigationAlert, CPNavigationAlertDismissalContext dismissalContext);

		/// <param name="mapTemplate">To be added</param>
		/// <param name="trip">To be added.</param>
		/// <param name="routeChoice">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:selectedPreviewForTrip:usingRouteChoice:")]
		void SelectedPreview (CPMapTemplate mapTemplate, CPTrip trip, CPRouteChoice routeChoice);

		/// <param name="mapTemplate">The template for the map on which a trip was started.</param>
		/// <param name="trip">The trip that started.</param>
		/// <param name="routeChoice">The route choice for the trip that started.</param>
		/// <summary>Method that is called when a trip starts.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:startedTrip:usingRouteChoice:")]
		void StartedTrip (CPMapTemplate mapTemplate, CPTrip trip, CPRouteChoice routeChoice);

		/// <param name="mapTemplate">The template for the map whose navigation was canceled.</param>
		/// <summary>Method that is called when navigation is canceled.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplateDidCancelNavigation:")]
		void DidCancelNavigation (CPMapTemplate mapTemplate);

		/// <param name="mapTemplate">The template for the map that .</param>
		/// <param name="maneuver">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("mapTemplate:displayStyleForManeuver:")]
		CPManeuverDisplayStyle GetDisplayStyle (CPMapTemplate mapTemplate, CPManeuver maneuver);
	}

	/// <summary>A banner displayed with high-priority.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNavigationAlert : NSSecureCoding {


		[Deprecated (PlatformName.iOS, 13, 0, message: "Use constructor that takes in 'UIImage' instead of 'CPImageSet'.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use constructor that takes in 'UIImage' instead of 'CPImageSet'.")]
		[Export ("initWithTitleVariants:subtitleVariants:imageSet:primaryAction:secondaryAction:duration:")]
		NativeHandle Constructor (string [] titleVariants, [NullAllowed] string [] subtitleVariants, [NullAllowed] CPImageSet imageSet, CPAlertAction primaryAction, [NullAllowed] CPAlertAction secondaryAction, double duration);

		[iOS (13, 0)]
		[Export ("initWithTitleVariants:subtitleVariants:image:primaryAction:secondaryAction:duration:")]
		NativeHandle Constructor (string [] titleVariants, [NullAllowed] string [] subtitleVariants, [NullAllowed] UIImage image, CPAlertAction primaryAction, [NullAllowed] CPAlertAction secondaryAction, double duration);

		[Export ("updateTitleVariants:subtitleVariants:")]
		void UpdateTitleVariants (string [] newTitleVariants, string [] newSubtitleVariants);

		[Export ("titleVariants", ArgumentSemantic.Copy)]
		string [] TitleVariants { get; }

		[Export ("subtitleVariants", ArgumentSemantic.Copy)]
		string [] SubtitleVariants { get; }

		[NullAllowed, Export ("imageSet", ArgumentSemantic.Copy)]
		CPImageSet ImageSet { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("image", ArgumentSemantic.Copy)]
		UIImage Image { get; }

		[Export ("primaryAction", ArgumentSemantic.Strong)]
		CPAlertAction PrimaryAction { get; }

		[NullAllowed, Export ("secondaryAction", ArgumentSemantic.Strong)]
		CPAlertAction SecondaryAction { get; }

		[Export ("duration")]
		double Duration { get; }
	}

	/// <summary>A session that may involve planning, updating, and executing a trip.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNavigationSession {

		[Export ("pauseTripForReason:description:")]
		void PauseTrip (CPTripPauseReason reason, [NullAllowed] string description);

		[iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("pauseTripForReason:description:turnCardColor:")]
		void PauseTrip (CPTripPauseReason reason, [NullAllowed] string description, [NullAllowed] UIColor turnCardColor);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("resumeTripWithUpdatedRouteInformation:")]
		void ResumeTrip (CPRouteInformation routeInformation);

		[Export ("finishTrip")]
		void FinishTrip ();

		[Export ("cancelTrip")]
		void CancelTrip ();

		[Export ("upcomingManeuvers", ArgumentSemantic.Copy)]
		CPManeuver [] UpcomingManeuvers { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[NullAllowed, Export ("currentLaneGuidance", ArgumentSemantic.Copy)]
		CPLaneGuidance CurrentLaneGuidance { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("addManeuvers:")]
		void AddManeuvers (CPManeuver [] maneuvers);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("addLaneGuidances:")]
		void AddLaneGuidances (CPLaneGuidance [] laneGuidances);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("currentRoadNameVariants", ArgumentSemantic.Copy)]
		string [] CurrentRoadNameVariants { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("maneuverState", ArgumentSemantic.Assign)]
		CPManeuverState ManeuverState { get; set; }

		[Export ("trip", ArgumentSemantic.Strong)]
		CPTrip Trip { get; }

		[Export ("updateTravelEstimates:forManeuver:")]
		void UpdateTravelEstimates (CPTravelEstimates estimates, CPManeuver maneuver);
	}

	/// <summary>
	///       <see cref="CarPlay.CPTemplate" /> subclass showing the destination search results.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	interface CPSearchTemplate {

		/// <summary>An instance of the CarPlay.ICPSearchTemplateDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CarPlay.ICPSearchTemplateDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPSearchTemplateDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>Delegate object used by <see cref="CarPlay.CPSearchTemplate" />.</summary>
	interface ICPSearchTemplateDelegate { }
	delegate void CPSearchTemplateDelegateUpdateHandler (CPListItem [] searchResults);

	/// <summary>Delegate object for the <see cref="CarPlay.CPSearchTemplate" /> class.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPSearchTemplateDelegate {

		/// <param name="searchTemplate">To be added.</param>
		/// <param name="searchText">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>Developers must override this method to respond to a change in the search text.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("searchTemplate:updatedSearchText:completionHandler:")]
		void UpdatedSearchText (CPSearchTemplate searchTemplate, string searchText, CPSearchTemplateDelegateUpdateHandler completionHandler);

		/// <param name="searchTemplate">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>Developers must overrride this method to respond to a search selection.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("searchTemplate:selectedResult:completionHandler:")]
		void SelectedResult (CPSearchTemplate searchTemplate, CPListItem item, Action completionHandler);

		/// <param name="searchTemplate">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("searchTemplateSearchButtonPressed:")]
		void SearchButtonPressed (CPSearchTemplate searchTemplate);
	}

	/// <summary>Class that responds to user-interface configuration changes.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPSessionConfiguration {

		[Export ("initWithDelegate:")]
		[DesignatedInitializer]
		NativeHandle Constructor (ICPSessionConfigurationDelegate @delegate);

		[Export ("limitedUserInterfaces")]
		CPLimitableUserInterface LimitedUserInterfaces { get; }

		[iOS (13, 0)]
		[Export ("contentStyle")]
		CPContentStyle ContentStyle { get; }

		/// <summary>An instance of the CarPlay.ICPSessionConfigurationDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the CarPlay.ICPSessionConfigurationDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPSessionConfigurationDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>Delegate object used by <see cref="CarPlay.CPSessionConfiguration" />.</summary>
	interface ICPSessionConfigurationDelegate { }

	/// <summary>Abstract implementation of <see cref="Carlay.ICPSessionConfigurationDelegate" />.</summary>
	[NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPSessionConfigurationDelegate {
		/// <param name="sessionConfiguration">To be added.</param>
		/// <param name="limitedUserInterfaces">To be added.</param>
		/// <summary>Called when the system changes keyboards or modifies list limits.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("sessionConfiguration:limitedUserInterfacesChanged:")]
		void LimitedUserInterfacesChanged (CPSessionConfiguration sessionConfiguration, CPLimitableUserInterface limitedUserInterfaces);

		[iOS (13, 0)]
		[Export ("sessionConfiguration:contentStyleChanged:")]
		void ContentStyleChanged (CPSessionConfiguration sessionConfiguration, CPContentStyle contentStyle);
	}

	/// <summary>Abstract base class for CarPlay user interface templates.</summary>
	[Abstract]
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface CPTemplate : NSSecureCoding {
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("tabTitle")]
		string TabTitle { get; set; }

		[iOS (14, 0)]
		[NullAllowed]
		[Export ("tabImage", ArgumentSemantic.Strong)]
		UIImage TabImage { get; set; }

		[iOS (14, 0)]
		[Export ("tabSystemItem", ArgumentSemantic.Assign)]
		UITabBarSystemItem TabSystemItem { get; set; }

		[iOS (14, 0)]
		[Export ("showsTabBadge")]
		bool ShowsTabBadge { get; set; }
	}

	/// <summary>A possible route for the trip.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPRouteChoice : NSCopying, NSSecureCoding {

		[Export ("initWithSummaryVariants:additionalInformationVariants:selectionSummaryVariants:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string [] summaryVariants, string [] additionalInformationVariants, string [] selectionSummaryVariants);

		[Export ("summaryVariants", ArgumentSemantic.Copy)]
		string [] SummaryVariants { get; }

		[Export ("selectionSummaryVariants", ArgumentSemantic.Copy)]
		[NullAllowed]
		string [] SelectionSummaryVariants { get; }

		[Export ("additionalInformationVariants", ArgumentSemantic.Copy)]
		[NullAllowed]
		string [] AdditionalInformationVariants { get; }

		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }
	}

	/// <summary>A journey from <see cref="CarPlay.CPTrip.Origin" /> to <see cref="CarPlay.CPTrip.Destination" />.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPTrip : NSSecureCoding {

		[Export ("initWithOrigin:destination:routeChoices:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapItem origin, MKMapItem destination, CPRouteChoice [] routeChoices);

		[Export ("origin", ArgumentSemantic.Strong)]
		MKMapItem Origin { get; }

		[Export ("destination", ArgumentSemantic.Strong)]
		MKMapItem Destination { get; }

		[Export ("routeChoices", ArgumentSemantic.Copy)]
		CPRouteChoice [] RouteChoices { get; }

		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }

		[iOS (17, 4), MacCatalyst (17, 4)]
		[NullAllowed, Export ("destinationNameVariants", ArgumentSemantic.Copy)]
		string [] DestinationNameVariants { get; set; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface CPVoiceControlState : NSSecureCoding {

		[Export ("initWithIdentifier:titleVariants:image:repeats:")]
		NativeHandle Constructor (string identifier, [NullAllowed] string [] titleVariants, [NullAllowed] UIImage image, bool repeats);

		[NullAllowed, Export ("titleVariants", ArgumentSemantic.Copy)]
		string [] TitleVariants { get; }

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("repeats")]
		bool Repeats { get; }
	}

	/// <summary>
	///       <see cref="CarPlay.CPTemplate" /> subclass for displaying the voice control indicator.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPVoiceControlTemplate {

		[Export ("initWithVoiceControlStates:")]
		NativeHandle Constructor (CPVoiceControlState [] voiceControlStates);

		[Export ("voiceControlStates", ArgumentSemantic.Copy)]
		CPVoiceControlState [] VoiceControlStates { get; }

		[Export ("activateVoiceControlStateWithIdentifier:")]
		void ActivateVoiceControlState (string identifier);

		[NullAllowed, Export ("activeStateIdentifier")]
		string ActiveStateIdentifier { get; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPImageSet : NSSecureCoding {

		[Export ("initWithLightContentImage:darkContentImage:")]
		NativeHandle Constructor (UIImage lightImage, UIImage darkImage);

		[Export ("lightContentImage")]
		UIImage LightContentImage { get; }

		[Export ("darkContentImage")]
		UIImage DarkContentImage { get; }
	}

	interface ICPTemplateApplicationSceneDelegate { }

	[NoTV, NoMac, iOS (13, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPTemplateApplicationSceneDelegate : UISceneDelegate {
		[Export ("templateApplicationScene:didConnectInterfaceController:toWindow:")]
		void DidConnect (CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController, CPWindow window);

		[Export ("templateApplicationScene:didDisconnectInterfaceController:fromWindow:")]
		void DidDisconnect (CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController, CPWindow window);

		[Export ("templateApplicationScene:didSelectNavigationAlert:")]
		void DidSelect (CPTemplateApplicationScene templateApplicationScene, CPNavigationAlert navigationAlert);

		[Export ("templateApplicationScene:didSelectManeuver:")]
		void DidSelect (CPTemplateApplicationScene templateApplicationScene, CPManeuver maneuver);

		[iOS (14, 0)]
		[Export ("templateApplicationScene:didConnectInterfaceController:")]
		void DidConnect (CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController);

		[iOS (14, 0)]
		[Export ("templateApplicationScene:didDisconnectInterfaceController:")]
		void DidDisconnect (CPTemplateApplicationScene templateApplicationScene, CPInterfaceController interfaceController);

		[iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("contentStyleDidChange:")]
		void ContentStyleDidChange (UIUserInterfaceStyle contentStyle);
	}

	[NoTV, NoMac, iOS (13, 0)]
	[BaseType (typeof (UIScene))]
	interface CPTemplateApplicationScene {
		[Export ("initWithSession:connectionOptions:")]
		NativeHandle Constructor (UISceneSession session, UISceneConnectionOptions connectionOptions);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPTemplateApplicationSceneDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Strong)]
		NSObject WeakDelegate { get; set; }

		[Export ("interfaceController", ArgumentSemantic.Strong)]
		CPInterfaceController InterfaceController { get; }

		[Export ("carWindow", ArgumentSemantic.Strong)]
		CPWindow CarWindow { get; }

		[iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("contentStyle")]
		UIUserInterfaceStyle ContentStyle { get; }

		[Field ("CPTemplateApplicationSceneSessionRoleApplication")]
		[Advice ("Use 'UIWindowSceneSessionRole.CarTemplateApplication' instead.")]
		NSString SessionRoleApplication { get; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (UIWindow))]
	interface CPWindow {

		/// <param name="frame">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("mapButtonSafeAreaLayoutGuide")]
		UILayoutGuide MapButtonSafeAreaLayoutGuide { get; }

		[iOS (13, 0)]
		[NullAllowed, Export ("templateApplicationScene", ArgumentSemantic.Weak)]
		CPTemplateApplicationScene TemplateApplicationScene { get; set; }
	}

	/// <summary>Estimates of time and distance requirements for requested navigation.</summary>
	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPTravelEstimates : NSSecureCoding {

		[Export ("initWithDistanceRemaining:timeRemaining:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSMeasurement<NSUnitLength> distance, double time);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("initWithDistanceRemaining:distanceRemainingToDisplay:timeRemaining:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSMeasurement<NSUnitLength> distanceRemaining, NSMeasurement<NSUnitLength> distanceRemainingToDisplay, double time);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("distanceRemainingToDisplay", ArgumentSemantic.Copy)]
		NSMeasurement<NSUnitLength> DistanceRemainingToDisplay { get; }

		[Export ("distanceRemaining", ArgumentSemantic.Copy)]
		NSMeasurement<NSUnitLength> DistanceRemaining { get; }

		[Export ("timeRemaining")]
		double TimeRemaining { get; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	interface CPTripPreviewTextConfiguration : NSSecureCoding {

		[Export ("initWithStartButtonTitle:additionalRoutesButtonTitle:overviewButtonTitle:")]
		NativeHandle Constructor ([NullAllowed] string startButtonTitle, [NullAllowed] string additionalRoutesButtonTitle, [NullAllowed] string overviewButtonTitle);

		[NullAllowed, Export ("startButtonTitle")]
		string StartButtonTitle { get; }

		[NullAllowed, Export ("additionalRoutesButtonTitle")]
		string AdditionalRoutesButtonTitle { get; }

		[NullAllowed, Export ("overviewButtonTitle")]
		string OverviewButtonTitle { get; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPActionSheetTemplate {

		[Export ("initWithTitle:message:actions:")]
		NativeHandle Constructor ([NullAllowed] string title, [NullAllowed] string message, CPAlertAction [] actions);

		[NullAllowed, Export ("title")]
		string Title { get; }

		[NullAllowed, Export ("message")]
		string Message { get; }

		[Export ("actions", ArgumentSemantic.Strong)]
		CPAlertAction [] Actions { get; }
	}

	[NoTV, NoMac]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPAlertTemplate {

		[Export ("initWithTitleVariants:actions:")]
		NativeHandle Constructor (string [] titleVariants, CPAlertAction [] actions);

		[Export ("titleVariants", ArgumentSemantic.Copy)]
		string [] TitleVariants { get; }

		[Export ("actions", ArgumentSemantic.Strong)]
		CPAlertAction [] Actions { get; }

		[iOS (14, 0)]
		[Static]
		[Export ("maximumActionCount")]
		nuint MaximumActionCount { get; }
	}

	[NoTV, NoMac, iOS (13, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPDashboardButton : NSSecureCoding {

		[Export ("initWithTitleVariants:subtitleVariants:image:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string [] titleVariants, string [] subtitleVariants, UIImage image, [NullAllowed] Action<CPDashboardButton> handler);

		[Export ("image")]
		UIImage Image { get; }

		[Export ("titleVariants")]
		string [] TitleVariants { get; }

		[Export ("subtitleVariants")]
		string [] SubtitleVariants { get; }
	}

	[NoTV, NoMac, iOS (13, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPDashboardController {

		[Export ("shortcutButtons", ArgumentSemantic.Copy)]
		CPDashboardButton [] ShortcutButtons { get; set; }
	}

	interface ICPTemplateApplicationDashboardSceneDelegate { }

	[NoTV, NoMac, iOS (13, 4)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPTemplateApplicationDashboardSceneDelegate : UISceneDelegate {

		[Export ("templateApplicationDashboardScene:didConnectDashboardController:toWindow:")]
		void DidConnectDashboardController (CPTemplateApplicationDashboardScene templateApplicationDashboardScene, CPDashboardController dashboardController, UIWindow window);

		[Export ("templateApplicationDashboardScene:didDisconnectDashboardController:fromWindow:")]
		void DidDisconnectDashboardController (CPTemplateApplicationDashboardScene templateApplicationDashboardScene, CPDashboardController dashboardController, UIWindow window);
	}

	[NoTV, NoMac, iOS (13, 4)]
	[BaseType (typeof (UIScene))]
	interface CPTemplateApplicationDashboardScene {

		[Field ("CPTemplateApplicationDashboardSceneSessionRoleApplication")]
		NSString SessionRoleApplication { get; }

		[Export ("initWithSession:connectionOptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UISceneSession session, UISceneConnectionOptions connectionOptions);

		[Wrap ("WeakDelegate")]
		[NullAllowed, New]
		ICPTemplateApplicationDashboardSceneDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Strong), New]
		NSObject WeakDelegate { get; set; }

		[Export ("dashboardController", ArgumentSemantic.Strong)]
		CPDashboardController DashboardController { get; }

		[Export ("dashboardWindow", ArgumentSemantic.Strong)]
		UIWindow DashboardWindow { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPButton {
		[Export ("initWithImage:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UIImage image, [NullAllowed] Action<CPButton> handler);

		[NullAllowed, Export ("image", ArgumentSemantic.Copy)]
		UIImage Image { get; }

		[NullAllowed, Export ("title")]
		string Title { get; set; }

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Field ("CPButtonMaximumImageSize")]
		CGSize MaximumImageSize { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	interface CPContact : NSSecureCoding {
		[Export ("initWithName:image:")]
		NativeHandle Constructor (string name, UIImage image);

		[Export ("name")]
		string Name { get; set; }

		[Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; set; }

		[NullAllowed, Export ("actions", ArgumentSemantic.Copy)]
		CPButton [] Actions { get; set; }

		[NullAllowed, Export ("subtitle")]
		string Subtitle { get; set; }

		[NullAllowed, Export ("informativeText")]
		string InformativeText { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPButton))]
	[DisableDefaultCtor]
	interface CPContactCallButton {
		[Export ("initWithImage:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UIImage image, [NullAllowed] Action<CPButton> handler);

		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPButton))]
	[DisableDefaultCtor]
	interface CPContactDirectionsButton {
		[Export ("initWithImage:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UIImage image, [NullAllowed] Action<CPButton> handler);

		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPContactTemplate : CPBarButtonProviding {
		[Export ("initWithContact:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CPContact contact);

		[Export ("contact", ArgumentSemantic.Strong)]
		CPContact Contact { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPInformationItem : NSSecureCoding {
		[Export ("initWithTitle:detail:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] string title, [NullAllowed] string detail);

		[NullAllowed, Export ("title")]
		string Title { get; }

		[NullAllowed, Export ("detail")]
		string Detail { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPInformationTemplate : CPBarButtonProviding {
		[Export ("initWithTitle:layout:items:actions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string title, CPInformationTemplateLayout layout, CPInformationItem [] items, CPTextButton [] actions);

		[Export ("layout")]
		CPInformationTemplateLayout Layout { get; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("items", ArgumentSemantic.Copy)]
		CPInformationItem [] Items { get; set; }

		[Export ("actions", ArgumentSemantic.Copy)]
		CPTextButton [] Actions { get; set; }
	}

	delegate void CPListImageRowItemHandler (CPListImageRowItem item, nint index, [BlockCallback] Action completionBlock);

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPListImageRowItem : CPSelectableListItem {
		[Export ("initWithText:images:")]
		NativeHandle Constructor (string text, UIImage [] images);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("initWithText:images:imageTitles:")]
		NativeHandle Constructor (string text, UIImage [] images, string [] imageTitles);

		[Export ("gridImages", ArgumentSemantic.Strong)]
		UIImage [] GridImages { get; }

		[Export ("updateImages:")]
		void UpdateImages (UIImage [] gridImages);

		[iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("imageTitles", ArgumentSemantic.Copy)]
		string [] ImageTitles { get; set; }

		[NullAllowed, Export ("listImageRowHandler", ArgumentSemantic.Copy)]
		CPListImageRowItemHandler ListImageRowHandler { get; set; }

		[Static]
		[Export ("maximumImageSize")]
		CGSize MaximumImageSize { get; }

		[Field ("CPMaximumNumberOfGridImages")]
		nuint MaximumNumberOfGridImages { get; }

		[NullAllowed, Export ("text")]
		new string Text { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPTextButton {
		[Export ("initWithTitle:textStyle:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string title, CPTextButtonStyle textStyle, [NullAllowed] Action<CPTextButton> handler);

		[Export ("title")]
		string Title { get; set; }

		[Export ("textStyle", ArgumentSemantic.Assign)]
		CPTextButtonStyle TextStyle { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPBarButton))]
	interface CPMessageComposeBarButton {
		[Static]
		[Export ("new")]
		[return: Release]
		CPMessageComposeBarButton Create ();

		[Export ("initWithImage:")]
		NativeHandle Constructor (UIImage image);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	interface CPMessageListItem : CPListTemplateItem {
		[Internal]
		[Export ("initWithConversationIdentifier:text:leadingConfiguration:trailingConfiguration:detailText:trailingText:")]
		IntPtr InitWithConversationIdentifier (string conversationIdentifier, string text, CPMessageListItemLeadingConfiguration leadingConfiguration, [NullAllowed] CPMessageListItemTrailingConfiguration trailingConfiguration, [NullAllowed] string detailText, [NullAllowed] string trailingText);

		[Internal]
		[Export ("initWithFullName:phoneOrEmailAddress:leadingConfiguration:trailingConfiguration:detailText:trailingText:")]
		IntPtr InitWithFullName (string fullName, string phoneOrEmailAddress, CPMessageListItemLeadingConfiguration leadingConfiguration, [NullAllowed] CPMessageListItemTrailingConfiguration trailingConfiguration, [NullAllowed] string detailText, [NullAllowed] string trailingText);

		[NullAllowed, Export ("conversationIdentifier")]
		string ConversationIdentifier { get; set; }

		[NullAllowed, Export ("phoneOrEmailAddress")]
		string PhoneOrEmailAddress { get; set; }

		[Export ("leadingConfiguration", ArgumentSemantic.Strong)]
		CPMessageListItemLeadingConfiguration LeadingConfiguration { get; set; }

		[NullAllowed, Export ("trailingConfiguration", ArgumentSemantic.Strong)]
		CPMessageListItemTrailingConfiguration TrailingConfiguration { get; set; }

		[NullAllowed, Export ("detailText")]
		string DetailText { get; set; }

		[NullAllowed, Export ("trailingText")]
		string TrailingText { get; set; }

		[Field ("CPMaximumMessageItemImageSize")]
		CGSize MaximumMessageItemImageSize { get; }

		[NullAllowed, Export ("text")]
		new string Text { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPMessageListItemLeadingConfiguration {
		[Export ("unread")]
		bool Unread { [Bind ("isUnread")] get; }

		[Export ("leadingItem")]
		CPMessageLeadingItem LeadingItem { get; }

		[NullAllowed, Export ("leadingImage")]
		UIImage LeadingImage { get; }

		[Export ("initWithLeadingItem:leadingImage:unread:")]
		NativeHandle Constructor (CPMessageLeadingItem leadingItem, [NullAllowed] UIImage leadingImage, bool unread);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPMessageListItemTrailingConfiguration {
		[Export ("trailingItem")]
		CPMessageTrailingItem TrailingItem { get; }

		[NullAllowed, Export ("trailingImage")]
		UIImage TrailingImage { get; }

		[Export ("initWithTrailingItem:trailingImage:")]
		NativeHandle Constructor (CPMessageTrailingItem trailingItem, [NullAllowed] UIImage trailingImage);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingButton : NSSecureCoding {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("selected")]
		bool Selected { [Bind ("isSelected")] get; set; }

		[Field ("CPNowPlayingButtonMaximumImageSize")]
		CGSize MaximumImageSize { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	[DisableDefaultCtor]
	interface CPNowPlayingImageButton {
		[Export ("initWithImage:handler:")]
		NativeHandle Constructor (UIImage image, [NullAllowed] Action<CPNowPlayingButton> handler);

		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);

		[NullAllowed, Export ("image", ArgumentSemantic.Strong)]
		UIImage Image { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPPointOfInterest : NSSecureCoding {
		[Export ("initWithLocation:title:subtitle:summary:detailTitle:detailSubtitle:detailSummary:pinImage:")]
		NativeHandle Constructor (MKMapItem location, string title, [NullAllowed] string subtitle, [NullAllowed] string summary, [NullAllowed] string detailTitle, [NullAllowed] string detailSubtitle, [NullAllowed] string detailSummary, [NullAllowed] UIImage pinImage);

		[iOS (16, 0)]
		[Export ("initWithLocation:title:subtitle:summary:detailTitle:detailSubtitle:detailSummary:pinImage:selectedPinImage:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MKMapItem location, string title, [NullAllowed] string subtitle, [NullAllowed] string summary, [NullAllowed] string detailTitle, [NullAllowed] string detailSubtitle, [NullAllowed] string detailSummary, [NullAllowed] UIImage pinImage, [NullAllowed] UIImage selectedPinImage);

		[Export ("location", ArgumentSemantic.Strong)]
		MKMapItem Location { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[NullAllowed, Export ("subtitle")]
		string Subtitle { get; set; }

		[NullAllowed, Export ("summary")]
		string Summary { get; set; }

		[NullAllowed, Export ("detailTitle")]
		string DetailTitle { get; set; }

		[NullAllowed, Export ("detailSubtitle")]
		string DetailSubtitle { get; set; }

		[NullAllowed, Export ("detailSummary")]
		string DetailSummary { get; set; }

		[NullAllowed, Export ("pinImage", ArgumentSemantic.Strong)]
		UIImage PinImage { get; set; }

		[NullAllowed, Export ("primaryButton", ArgumentSemantic.Strong)]
		CPTextButton PrimaryButton { get; set; }

		[NullAllowed, Export ("secondaryButton", ArgumentSemantic.Strong)]
		CPTextButton SecondaryButton { get; set; }

		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }

		[iOS (16, 0)]
		[Static]
		[Export ("pinImageSize")]
		CGSize PinImageSize { get; }

		[iOS (16, 0)]
		[Static]
		[Export ("selectedPinImageSize")]
		CGSize SelectedPinImageSize { get; }

		[iOS (16, 0)]
		[NullAllowed, Export ("selectedPinImage", ArgumentSemantic.Strong)]
		UIImage SelectedPinImage { get; set; }
	}

	interface ICPPointOfInterestTemplateDelegate { }

	[NoTV, NoMac, iOS (14, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPPointOfInterestTemplateDelegate {
		[Abstract]
		[Export ("pointOfInterestTemplate:didChangeMapRegion:")]
		void DidChangeMapRegion (CPPointOfInterestTemplate pointOfInterestTemplate, MKCoordinateRegion region);

		[Export ("pointOfInterestTemplate:didSelectPointOfInterest:")]
		void DidSelectPointOfInterest (CPPointOfInterestTemplate pointOfInterestTemplate, CPPointOfInterest pointOfInterest);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPPointOfInterestTemplate : CPBarButtonProviding {
		[Export ("initWithTitle:pointsOfInterest:selectedIndex:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string title, CPPointOfInterest [] pointsOfInterest, nint selectedIndex);

		[Export ("title")]
		string Title { get; set; }

		[Export ("setPointsOfInterest:selectedIndex:")]
		void SetPointsOfInterest (CPPointOfInterest [] pointsOfInterest, nint selectedIndex);

		[Export ("pointsOfInterest")]
		CPPointOfInterest [] PointsOfInterest { get; }

		[Export ("selectedIndex")]
		nint SelectedIndex { get; set; }

		[Wrap ("WeakPointOfInterestDelegate")]
		[NullAllowed]
		ICPPointOfInterestTemplateDelegate PointOfInterestDelegate { get; set; }

		[NullAllowed, Export ("pointOfInterestDelegate", ArgumentSemantic.Weak)]
		NSObject WeakPointOfInterestDelegate { get; set; }
	}

	interface ICPTabBarTemplateDelegate { }

	[NoTV, NoMac, iOS (14, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPTabBarTemplateDelegate {
		[Abstract]
		[Export ("tabBarTemplate:didSelectTemplate:")]
		void DidSelectTemplate (CPTabBarTemplate tabBarTemplate, CPTemplate selectedTemplate);
	}


	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPTabBarTemplate {
		[Export ("initWithTemplates:")]
		NativeHandle Constructor (CPTemplate [] templates);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPTabBarTemplateDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Static]
		[Export ("maximumTabCount")]
		nuint MaximumTabCount { get; }

		[Export ("templates", ArgumentSemantic.Strong)]
		CPTemplate [] Templates { get; }

		[NullAllowed, Export ("selectedTemplate", ArgumentSemantic.Strong)]
		CPTemplate SelectedTemplate { get; }

		[Export ("updateTemplates:")]
		void UpdateTemplates (CPTemplate [] newTemplates);

		[iOS (17, 0)]
		[Export ("selectTemplate:")]
		void SelectTemplate (CPTemplate newTemplate);

		[iOS (17, 0)]
		[Export ("selectTemplateAtIndex:")]
		void SelectTemplate (nint index);
	}

	interface ICPNowPlayingTemplateObserver { }

	[NoTV, NoMac, iOS (14, 0)]
	[Protocol]
	interface CPNowPlayingTemplateObserver {
		[Export ("nowPlayingTemplateUpNextButtonTapped:")]
		void UpNextButtonTapped (CPNowPlayingTemplate nowPlayingTemplate);

		[Export ("nowPlayingTemplateAlbumArtistButtonTapped:")]
		void AlbumArtistButtonTapped (CPNowPlayingTemplate nowPlayingTemplate);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPTemplate))]
	[DisableDefaultCtor]
	interface CPNowPlayingTemplate {
		[Static]
		[Export ("sharedTemplate", ArgumentSemantic.Strong)]
		CPNowPlayingTemplate SharedTemplate { get; }

		[Export ("addObserver:")]
		void AddObserver (ICPNowPlayingTemplateObserver observer);

		[Export ("removeObserver:")]
		void RemoveObserver (ICPNowPlayingTemplateObserver observer);

		[Export ("nowPlayingButtons", ArgumentSemantic.Strong)]
		CPNowPlayingButton [] NowPlayingButtons { get; }

		[Export ("upNextButtonEnabled")]
		bool IsUpNextButtonEnabled { [Bind ("isUpNextButtonEnabled")] get; set; }

		[Export ("upNextTitle")]
		string UpNextTitle { get; set; }

		[Export ("albumArtistButtonEnabled")]
		bool IsAlbumArtistButtonEnabled { [Bind ("isAlbumArtistButtonEnabled")] get; set; }

		[Export ("updateNowPlayingButtons:")]
		void UpdateNowPlayingButtons (CPNowPlayingButton [] nowPlayingButtons);

		[iOS (18, 4), MacCatalyst (18, 4)]
		[Export ("nowPlayingMode", ArgumentSemantic.Strong), NullAllowed]
		CPNowPlayingMode NowPlayingMode { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPButton))]
	[DisableDefaultCtor]
	interface CPContactMessageButton {
		[Export ("initWithImage:handler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UIImage image, [NullAllowed] Action<CPButton> handler);

		[Export ("initWithPhoneOrEmail:")]
		NativeHandle Constructor (string phoneOrEmail);

		[Export ("phoneOrEmail")]
		string PhoneOrEmail { get; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	interface CPNowPlayingShuffleButton {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	interface CPNowPlayingAddToLibraryButton {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	interface CPNowPlayingMoreButton {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	interface CPNowPlayingPlaybackRateButton {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPNowPlayingButton))]
	interface CPNowPlayingRepeatButton {
		[Export ("initWithHandler:")]
		NativeHandle Constructor ([NullAllowed] Action<CPNowPlayingButton> handler);
	}

	interface ICPListTemplateItem { }

	[NoTV, NoMac, iOS (14, 0)]
	[Protocol]
	interface CPListTemplateItem {
		[Abstract]
		[NullAllowed, Export ("text")]
		string Text { get; }

		[Abstract]
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Strong)]
		NSObject UserInfo { get; set; }

		[Abstract]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }
	}

	interface ICPSelectableListItem { }

	delegate void CPSelectableListItemHandler (ICPSelectableListItem item, [BlockCallback] Action completionBlock);

	[NoTV, NoMac, iOS (14, 0)]
	[Protocol]
	interface CPSelectableListItem : CPListTemplateItem {
		[Abstract]
		[NullAllowed, Export ("handler", ArgumentSemantic.Copy)]
		CPSelectableListItemHandler Handler { get; set; }
	}

	[NoTV, NoMac, iOS (14, 0)]
	[BaseType (typeof (CPInformationItem))]
	[DisableDefaultCtor]
	interface CPInformationRatingItem {
		[Export ("initWithRating:maximumRating:title:detail:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSNumber rating, [NullAllowed] NSNumber maximumRating, [NullAllowed] string title, [NullAllowed] string detail);

		[NullAllowed, Export ("rating")]
		NSNumber Rating { get; }

		[NullAllowed, Export ("maximumRating")]
		NSNumber MaximumRating { get; }
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPAssistantCellConfiguration : NSSecureCoding {
		[Export ("initWithPosition:visibility:assistantAction:")]
		NativeHandle Constructor (CPAssistantCellPosition position, CPAssistantCellVisibility visibility, CPAssistantCellActionType assistantAction);

		[Export ("position")]
		CPAssistantCellPosition Position { get; }

		[Export ("visibility")]
		CPAssistantCellVisibility Visibility { get; }

		[Export ("assistantAction")]
		CPAssistantCellActionType AssistantAction { get; }
	}

	[NoTV, NoMac, iOS (15, 4), MacCatalyst (15, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPInstrumentClusterController {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPInstrumentClusterControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("instrumentClusterWindow", ArgumentSemantic.Strong)]
		UIWindow InstrumentClusterWindow { get; }

		[Export ("speedLimitSetting")]
		CPInstrumentClusterSetting SpeedLimitSetting { get; }

		[Export ("compassSetting")]
		CPInstrumentClusterSetting CompassSetting { get; }

		[Export ("inactiveDescriptionVariants", ArgumentSemantic.Copy)]
		string [] InactiveDescriptionVariants { get; set; }

		[Export ("attributedInactiveDescriptionVariants", ArgumentSemantic.Copy)]
		NSAttributedString [] AttributedInactiveDescriptionVariants { get; set; }
	}

	interface ICPInstrumentClusterControllerDelegate { }

	[NoTV, NoMac, iOS (15, 4), MacCatalyst (15, 4)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPInstrumentClusterControllerDelegate {

		[Abstract]
		[Export ("instrumentClusterControllerDidConnectWindow:")]
		void DidConnectWindow (UIWindow instrumentClusterWindow);

		[Abstract]
		[Export ("instrumentClusterControllerDidDisconnectWindow:")]
		void DidDisconnectWindow (UIWindow instrumentClusterWindow);

		[Export ("instrumentClusterControllerDidZoomIn:")]
		void DidZoomIn (CPInstrumentClusterController instrumentClusterController);

		[Export ("instrumentClusterControllerDidZoomOut:")]
		void DidZoomOut (CPInstrumentClusterController instrumentClusterController);

		[Export ("instrumentClusterController:didChangeCompassSetting:")]
		void DidChangeCompassSetting (CPInstrumentClusterController instrumentClusterController, CPInstrumentClusterSetting compassSetting);

		[Export ("instrumentClusterController:didChangeSpeedLimitSetting:")]
		void DidChangeSpeedLimitSetting (CPInstrumentClusterController instrumentClusterController, CPInstrumentClusterSetting speedLimitSetting);
	}

	interface ICPTemplateApplicationInstrumentClusterSceneDelegate { }

	[NoTV, NoMac, iOS (15, 4), MacCatalyst (15, 4)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface CPTemplateApplicationInstrumentClusterSceneDelegate : UISceneDelegate {

		[Export ("templateApplicationInstrumentClusterScene:didConnectInstrumentClusterController:")]
		void DidConnectInstrumentClusterController (CPTemplateApplicationInstrumentClusterScene templateApplicationInstrumentClusterScene, CPInstrumentClusterController instrumentClusterController);

		[Export ("templateApplicationInstrumentClusterScene:didDisconnectInstrumentClusterController:")]
		void DidDisconnectInstrumentClusterController (CPTemplateApplicationInstrumentClusterScene templateApplicationInstrumentClusterScene, CPInstrumentClusterController instrumentClusterController);

		[Export ("contentStyleDidChange:")]
		void ContentStyleDidChange (UIUserInterfaceStyle contentStyle);
	}

	[NoTV, NoMac, iOS (15, 4), MacCatalyst (15, 4)]
	[BaseType (typeof (UIScene))]
	[DisableDefaultCtor]
	interface CPTemplateApplicationInstrumentClusterScene {

		[Field ("CPTemplateApplicationInstrumentClusterSceneSessionRoleApplication")]
		NSString SessionRoleApplication { get; }

		[Export ("initWithSession:connectionOptions:")]
		[DesignatedInitializer]
		NativeHandle Constructor (UISceneSession session, UISceneConnectionOptions connectionOptions);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ICPTemplateApplicationInstrumentClusterSceneDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Strong)]
		NSObject WeakDelegate { get; set; }

		[Export ("instrumentClusterController", ArgumentSemantic.Strong)]
		CPInstrumentClusterController InstrumentClusterController { get; }

		[Export ("contentStyle")]
		UIUserInterfaceStyle ContentStyle { get; }
	}

	[NoTV, NoMac, iOS (17, 4)]
	[NoMacCatalyst] // We don't expose CarPlay on Mac Catalyst for the moment // [MacCatalyst (17, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPLane : NSCopying, NSSecureCoding {
		[Export ("status", ArgumentSemantic.Assign)]
		CPLaneStatus Status {
			get;
			[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'NSMeasurement<NSUnitAngle>' constructor to create a CPLane with CPLaneStatus.NotGood, or use the 'NSMeasurement<NSUnitAngle>[], NSMeasurement<NSUnitAngle>[], bool' constructor to create a CPLane with status CPLaneStatus.Good or CPLaneStatus.Preferred.")]
			set;
		}

		[Export ("primaryAngle", ArgumentSemantic.Strong)]
		NSMeasurement<NSUnitAngle> PrimaryAngle {
			[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'HighlightedAngle' property instead.")]
			get;
			[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'NSMeasurement<NSUnitAngle>[], NSMeasurement<NSUnitAngle>[], bool' to create a CPLane with a HighlightedAngle.")]
			set;
		}

		[Export ("secondaryAngles", ArgumentSemantic.Strong)]
		NSMeasurement<NSUnitAngle> [] SecondaryAngles {
			[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'Angles' property instead.")]
			get;
			[Deprecated (PlatformName.iOS, 18, 0, message: "Use the 'NSMeasurement<NSUnitAngle>' constructor or the 'NSMeasurement<NSUnitAngle>[], NSMeasurement<NSUnitAngle>[], bool' constructor to create a CPLane with angles.")]
			set;
		}

		[Export ("init")]
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use either of the other two constructors instead.")]
		NativeHandle Constructor ();

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("initWithAngles:")]
		NativeHandle Constructor (NSMeasurement<NSUnitAngle> [] angles);

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("initWithAngles:highlightedAngle:isPreferred:")]
		NativeHandle Constructor (NSMeasurement<NSUnitAngle> [] angles, NSMeasurement<NSUnitAngle> [] highlightedAngle, bool isPreferred);


		[NullAllowed]
		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("highlightedAngle", ArgumentSemantic.Strong)]
		NSMeasurement<NSUnitAngle> HighlightedAngle { get; }


		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("angles", ArgumentSemantic.Copy)]
		NSMeasurement<NSUnitAngle> [] Angles { get; }
	}

	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[BaseType (typeof (NSObject))]
	interface CPLaneGuidance : NSCopying, NSSecureCoding {

		[Export ("lanes", ArgumentSemantic.Copy)]
		CPLane [] Lanes { get; set; }

		[Export ("instructionVariants", ArgumentSemantic.Copy)]
		string [] InstructionVariants { get; set; }
	}

	// @interface CPRouteInformation : NSObject
	[NoTV, NoMac, iOS (17, 4), MacCatalyst (17, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPRouteInformation {

		[Export ("initWithManeuvers:laneGuidances:currentManeuvers:currentLaneGuidance:tripTravelEstimates:maneuverTravelEstimates:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CPManeuver [] maneuvers, CPLaneGuidance [] laneGuidances, CPManeuver [] currentManeuvers, CPLaneGuidance currentLaneGuidance, CPTravelEstimates tripTravelEstimates, CPTravelEstimates maneuverTravelEstimates);

		[Export ("maneuvers", ArgumentSemantic.Copy)]
		CPManeuver [] Maneuvers { get; }

		[Export ("laneGuidances", ArgumentSemantic.Copy)]
		CPLaneGuidance [] LaneGuidances { get; }

		[Export ("currentManeuvers", ArgumentSemantic.Copy)]
		CPManeuver [] CurrentManeuvers { get; }

		[Export ("currentLaneGuidance", ArgumentSemantic.Copy)]
		CPLaneGuidance CurrentLaneGuidance { get; }

		[Export ("tripTravelEstimates", ArgumentSemantic.Copy)]
		CPTravelEstimates TripTravelEstimates { get; }

		[Export ("maneuverTravelEstimates", ArgumentSemantic.Copy)]
		CPTravelEstimates ManeuverTravelEstimates { get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingMode : NSSecureCoding {
		[Static]
		[Export ("defaultNowPlayingMode")]
		CPNowPlayingMode DefaultNowPlayingMode { get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (CPNowPlayingMode))]
	interface CPNowPlayingModeSports : NSSecureCoding {
		[Export ("initWithLeftTeam:rightTeam:eventStatus:backgroundArtwork:")]
		NativeHandle Constructor (CPNowPlayingSportsTeam leftTeam, CPNowPlayingSportsTeam rightTeam, [NullAllowed] CPNowPlayingSportsEventStatus eventStatus, [NullAllowed] UIImage backgroundArtwork);

		[Export ("leftTeam", ArgumentSemantic.Strong)]
		CPNowPlayingSportsTeam LeftTeam { get; }

		[Export ("rightTeam", ArgumentSemantic.Strong)]
		CPNowPlayingSportsTeam RightTeam { get; }

		[NullAllowed, Export ("eventStatus", ArgumentSemantic.Strong)]
		CPNowPlayingSportsEventStatus EventStatus { get; }

		[NullAllowed, Export ("backgroundArtwork", ArgumentSemantic.Copy)]
		UIImage BackgroundArtwork { get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingSportsTeam : NSSecureCoding {
		[Export ("initWithName:logo:teamStandings:eventScore:possessionIndicator:favorite:")]
		NativeHandle Constructor (string name, CPNowPlayingSportsTeamLogo logo, [NullAllowed] string teamStandings, string eventScore, [NullAllowed] UIImage possessionIndicator, bool favorite);

		[Export ("name")]
		string Name { get; }

		[Export ("logo", ArgumentSemantic.Copy)]
		CPNowPlayingSportsTeamLogo Logo { get; }

		[NullAllowed, Export ("teamStandings")]
		string TeamStandings { get; }

		[Export ("eventScore")]
		string EventScore { get; }

		[NullAllowed, Export ("possessionIndicator", ArgumentSemantic.Copy)]
		UIImage PossessionIndicator { get; }

		[Export ("favorite")]
		bool Favorite { [Bind ("isFavorite")] get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingSportsEventStatus : NSSecureCoding {
		[Export ("initWithEventStatusText:eventStatusImage:eventClock:")]
		NativeHandle Constructor ([NullAllowed] string [] eventStatusText, [NullAllowed] UIImage eventStatusImage, [NullAllowed] CPNowPlayingSportsClock eventClock);

		[NullAllowed, Export ("eventStatusText", ArgumentSemantic.Copy)]
		string [] EventStatusText { get; }

		[NullAllowed, Export ("eventClock", ArgumentSemantic.Copy)]
		CPNowPlayingSportsClock EventClock { get; }

		[NullAllowed, Export ("eventStatusImage", ArgumentSemantic.Copy)]
		UIImage EventStatusImage { get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingSportsClock : INSSecureCoding {
		[Internal]
		[Export ("initWithElapsedTime:paused:")]
		NativeHandle _InitWithElapsedTime (double elapsedTime, bool paused);

		[Internal]
		[Export ("initWithTimeRemaining:paused:")]
		NativeHandle _InitWithRemainingTime (double timeRemaining, bool paused);

		[Export ("timeValue")]
		double TimeValue { get; }

		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; }

		[Export ("countsUp")]
		bool CountsUp { get; }
	}

	[NoTV, NoMac, iOS (18, 4), MacCatalyst (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CPNowPlayingSportsTeamLogo : INSSecureCoding {
		[Export ("initWithTeamLogo:")]
		NativeHandle Constructor (UIImage teamLogo);

		[Export ("initWithTeamInitials:")]
		NativeHandle Constructor (string teamInitials);

		[NullAllowed, Export ("logo", ArgumentSemantic.Copy)]
		UIImage Logo { get; }

		[NullAllowed, Export ("initials")]
		string Initials { get; }
	}
}
