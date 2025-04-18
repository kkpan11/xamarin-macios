using System;
using System.ComponentModel;
using CoreGraphics;
using ObjCRuntime;
using Foundation;

#if !MONOMAC
using UIKit;
using NSViewController = UIKit.UIViewController;
#else
using AppKit;
using UIEdgeInsets = AppKit.NSEdgeInsets;
using UIVibrancyEffect = Foundation.NSObject;
using UIVibrancyEffectStyle = Foundation.NSObject;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace NotificationCenter {
	/// <summary>Coordinates the display of a widget's content with its containing app.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NotificationCenter/Reference/NCWidgetController_Class/index.html">Apple documentation for <c>NCWidgetController</c></related>
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // not meant to be user created
	[Deprecated (PlatformName.iOS, 14, 0)]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	interface NCWidgetController {

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("widgetController")]
		NCWidgetController GetWidgetController ();

		/// <param name="flag">To be added.</param>
		///         <param name="bundleID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setHasContent:forWidgetWithBundleIdentifier:")]
		void SetHasContent (bool flag, string bundleID);
	}

	/// <summary>Customizes the appearance and behavior of a widget.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NotificationCenter/Reference/NCWidgetProviding_Protocol/index.html">Apple documentation for <c>NCWidgetProviding</c></related>
	[Deprecated (PlatformName.iOS, 14, 0)]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface NCWidgetProviding {

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("widgetPerformUpdateWithCompletionHandler:")]
		void WidgetPerformUpdate (Action<NCUpdateResult> completionHandler);

		/// <param name="defaultMarginInsets">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("widgetMarginInsetsForProposedMarginInsets:"), DelegateName ("NCWidgetProvidingMarginInsets"), DefaultValueFromArgument ("defaultMarginInsets")]
		[Deprecated (PlatformName.iOS, 10, 0)]
		UIEdgeInsets GetWidgetMarginInsets (UIEdgeInsets defaultMarginInsets);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[Export ("widgetAllowsEditing")]
		bool WidgetAllowsEditing {
			get;
#if !NET
			[NotImplemented]
			set;
#endif
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[Export ("widgetDidBeginEditing")]
		void WidgetDidBeginEditing ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[Export ("widgetDidEndEditing")]
		void WidgetDidEndEditing ();

		/// <param name="activeDisplayMode">To be added.</param>
		///         <param name="maxSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("widgetActiveDisplayModeDidChange:withMaximumSize:")]
		void WidgetActiveDisplayModeDidChange (NCWidgetDisplayMode activeDisplayMode, CGSize maxSize);
	}

	/// <summary>Defines the appropriate vibrancy effect for widgets (extensions) displayed in the Today view.</summary>
	[NoMac]
	[BaseType (typeof (UIVibrancyEffect))]
#if NET
	[Internal]
	[Category]
#else
#pragma warning disable 0618 // warning CS0618: 'CategoryAttribute.CategoryAttribute(bool)' is obsolete: 'Inline the static members in this category in the category's class (and remove this obsolete once fixed)'
	[Category (allowStaticMembers: true)] // Classic isn't internal so we need this
#pragma warning restore
#endif
	interface UIVibrancyEffect_NotificationCenter {
		[Internal]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static, Export ("notificationCenterVibrancyEffect")]
		UIVibrancyEffect NotificationCenterVibrancyEffect ();
	}

	/// <summary>Extension context methods and properties for an NDWidget.</summary>
	[NoMac]
	[Deprecated (PlatformName.iOS, 14, 0)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_NCWidgetAdditions {
		[Export ("widgetLargestAvailableDisplayMode")]
		NCWidgetDisplayMode GetWidgetLargestAvailableDisplayMode ();

		[Export ("setWidgetLargestAvailableDisplayMode:")]
		void SetWidgetLargestAvailableDisplayMode (NCWidgetDisplayMode mode);

		[Export ("widgetActiveDisplayMode")]
		NCWidgetDisplayMode GetWidgetActiveDisplayMode ();

		[Export ("widgetMaximumSizeForDisplayMode:")]
		CGSize GetWidgetMaximumSize (NCWidgetDisplayMode displayMode);
	}

	[NoMac]
	[Category]
	[Internal] // only static methods, which are not _nice_ to use as extension methods
	[Deprecated (PlatformName.iOS, 14, 0)]
	[BaseType (typeof (UIVibrancyEffect))]
	interface UIVibrancyEffect_NCWidgetAdditions {
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static]
		[Export ("widgetPrimaryVibrancyEffect")]
		UIVibrancyEffect GetWidgetPrimaryVibrancyEffect ();

		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static]
		[Export ("widgetSecondaryVibrancyEffect")]
		UIVibrancyEffect GetWidgetSecondaryVibrancyEffect ();

		[iOS (13, 0)]
		[Static]
		[Export ("widgetEffectForVibrancyStyle:")]
		UIVibrancyEffect GetWidgetEffect (UIVibrancyEffectStyle vibrancyStyle);
	}

	[NoiOS]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[BaseType (typeof (NSViewController), Delegates = new string [] { "Delegate" }, Events = new Type [] { typeof (NCWidgetListViewDelegate) })]
	interface NCWidgetListViewController {
		/// <param name="nibNameOrNull">To be added.</param>
		/// <param name="nibBundleOrNull">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		INCWidgetListViewDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contents", ArgumentSemantic.Copy)]
		NSViewController [] Contents { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minimumVisibleRowCount", ArgumentSemantic.Assign)]
		nuint MinimumVisibleRowCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDividerLines")]
		bool HasDividerLines { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("editing")]
		bool Editing { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showsAddButtonWhenEditing")]
		bool ShowsAddButtonWhenEditing { get; set; }

		[Export ("viewControllerAtRow:makeIfNecessary:")]
		NSViewController GetViewController (nuint row, bool makeIfNecesary);

		/// <param name="viewController">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("rowForViewController:")]
		nuint GetRow (NSViewController viewController);
	}

	[NoiOS]
	interface INCWidgetListViewDelegate { }

	[NoiOS]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface NCWidgetListViewDelegate {
		[Abstract]
		[Export ("widgetList:viewControllerForRow:"), DelegateName ("NCWidgetListViewGetController"), DefaultValue (null)]
		NSViewController GetViewControllerForRow (NCWidgetListViewController list, nuint row);

		/// <param name="list">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("widgetListPerformAddAction:"), DelegateName ("NCWidgetListViewController")]
		void PerformAddAction (NCWidgetListViewController list);

		[Export ("widgetList:shouldReorderRow:"), DelegateName ("NCWidgetListViewControllerShouldReorderRow"), DefaultValue (false)]
		bool ShouldReorderRow (NCWidgetListViewController list, nuint row);

		[Export ("widgetList:didReorderRow:toRow:"), EventArgs ("NCWidgetListViewControllerDidReorder"), DefaultValue (false)]
		void DidReorderRow (NCWidgetListViewController list, nuint row, nuint newIndex);

		[Export ("widgetList:shouldRemoveRow:"), DelegateName ("NCWidgetListViewControllerShouldRemoveRow"), DefaultValue (false)]
		bool ShouldRemoveRow (NCWidgetListViewController list, nuint row);

		[Export ("widgetList:didRemoveRow:"), EventArgs ("NCWidgetListViewControllerDidRemoveRow"), DefaultValue (false)]
		void DidRemoveRow (NCWidgetListViewController list, nuint row);
	}

	[NoiOS]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[BaseType (typeof (NSViewController), Delegates = new string [] { "Delegate" }, Events = new Type [] { typeof (NCWidgetSearchViewDelegate) })]
	interface NCWidgetSearchViewController {
		/// <param name="nibNameOrNull">To be added.</param>
		/// <param name="nibBundleOrNull">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		INCWidgetSearchViewDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("searchResults", ArgumentSemantic.Copy)]
		NSObject [] SearchResults { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("searchDescription")]
		string SearchDescription { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("searchResultsPlaceholderString")]
		string SearchResultsPlaceholderString { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("searchResultKeyPath")]
		string SearchResultKeyPath { get; set; }
	}

	[NoiOS]
	interface INCWidgetSearchViewDelegate { }

	[NoiOS]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface NCWidgetSearchViewDelegate {
#if !NET
		[Abstract]
		[Export ("widgetSearch:searchForTerm:maxResults:"), EventArgs ("NSWidgetSearchForTerm"), DefaultValue (false)]
		void SearchForTearm (NCWidgetSearchViewController controller, string searchTerm, nuint max);
#else
		[Abstract]
		[Export ("widgetSearch:searchForTerm:maxResults:"), EventArgs ("NSWidgetSearchForTerm"), DefaultValue (false)]
		void SearchForTerm (NCWidgetSearchViewController controller, string searchTerm, nuint max);
#endif

		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("widgetSearchTermCleared:"), EventArgs ("NSWidgetSearchViewController"), DefaultValue (false)]
		void TermCleared (NCWidgetSearchViewController controller);

		/// <param name="controller">To be added.</param>
		///         <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("widgetSearch:resultSelected:"), EventArgs ("NSWidgetSearchResultSelected"), DefaultValue (false)]
		void ResultSelected (NCWidgetSearchViewController controller, NSObject obj);
	}
}
