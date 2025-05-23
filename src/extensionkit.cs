//
// ExtensionKit.cs: This file describes the API that the generator will produce for ExtensionKit
//
// Copyright 2022 Microsoft Corp. All rights reserved
//

using ObjCRuntime;
using Foundation;

#if MONOMAC
using AppKit;
using UIView = AppKit.NSView;
using UIViewController = AppKit.NSViewController;
#else
using UIKit;
#endif

namespace ExtensionKit {
	[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
	[BaseType (typeof (UIViewController))]
	interface EXAppExtensionBrowserViewController {
		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);
	}

	interface IEXHostViewControllerDelegate { }

	[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface EXHostViewControllerDelegate {
		[Export ("hostViewControllerDidActivate:")]
		void DidActivate (EXHostViewController viewController);

		[Export ("hostViewControllerWillDeactivate:error:")]
		void WillDeactivate (EXHostViewController viewController, [NullAllowed] NSError error);

#if !XAMCORE_5_0
		[Obsoleted (PlatformName.MacOSX, 14, 0, message: "No longer required.")]
		[Export ("shouldAcceptXPCConnection:")]
		bool ShouldAcceptXpcConnection (NSXpcConnection connection);
#endif
	}

	// @interface EXHostViewController : NSViewController
	[Mac (13, 0), NoiOS, NoMacCatalyst, NoTV]
	[BaseType (typeof (UIViewController))]
	interface EXHostViewController {
		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[NullAllowed, Wrap ("WeakDelegate")]
		IEXHostViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("placeholderView", ArgumentSemantic.Strong)]
		UIView PlaceholderView { get; set; }

		[Export ("makeXPCConnectionWithError:")]
		[return: NullAllowed]
		NSXpcConnection MakeXpcConnection ([NullAllowed] out NSError error);
	}
}
