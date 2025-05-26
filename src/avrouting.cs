//
// AVRouting bindings
//
// Authors:
//	TJ Lambert  <TJ.Lambert@microsoft.com>
//
// Copyright 2022 Microsoft Corp. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;
using UniformTypeIdentifiers;
using AVKit;

using OS_nw_endpoint = ObjCRuntime.NativeHandle;

namespace AVRouting {

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[Native]
	public enum AVCustomRoutingEventReason : long {
		Activate = 0,
		Deactivate,
		Reactivate,
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface AVCustomDeviceRoute {
		[Internal]
		[Export ("networkEndpoint")]
		OS_nw_endpoint _NetworkEndpoint { get; }

		[NullAllowed, Export ("bluetoothIdentifier")]
		NSUuid BluetoothIdentifier { get; }
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface AVCustomRoutingActionItem {
		[Export ("type", ArgumentSemantic.Copy)]
		UTType Type { get; set; }

		[NullAllowed, Export ("overrideTitle")]
		string OverrideTitle { get; set; }
	}

	[NoTV, NoMac, iOS (16, 4), MacCatalyst (16, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AVCustomRoutingPartialIP {

		[Export ("address", ArgumentSemantic.Copy)]
		NSData Address { get; }

		[Export ("mask", ArgumentSemantic.Copy)]
		NSData Mask { get; }

		[Export ("initWithAddress:mask:")]
		NativeHandle Constructor (NSData address, NSData mask);
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface AVCustomRoutingController {
		[Wrap ("WeakDelegate")]
		IAVCustomRoutingControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("authorizedRoutes")]
		AVCustomDeviceRoute [] AuthorizedRoutes { get; }

		[NoTV, NoMac, iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("knownRouteIPs", ArgumentSemantic.Strong)]
		AVCustomRoutingPartialIP [] KnownRouteIPs { get; set; }

		[Export ("customActionItems", ArgumentSemantic.Strong)]
		AVCustomRoutingActionItem [] CustomActionItems { get; set; }

		[Export ("invalidateAuthorizationForRoute:")]
		void InvalidateAuthorization (AVCustomDeviceRoute route);

		[Export ("setActive:forRoute:")]
		void SetActive (bool active, AVCustomDeviceRoute route);

		[Export ("isRouteActive:")]
		bool IsRouteActive (AVCustomDeviceRoute route);

		[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
		[Notification, Field ("AVCustomRoutingControllerAuthorizedRoutesDidChangeNotification")]
		NSString AuthorizedRoutesDidChangeNotification { get; }
	}

	// The AVCustomRoutingControllerDelegate type was incorrectly placed in the AVKit framework.
#if !XAMCORE_5_0
}
namespace AVKit {
	using AVRouting;
#endif

	delegate void AVCustomRoutingControllerDelegateCompletionHandler (bool success);

	interface IAVCustomRoutingControllerDelegate { }

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AVCustomRoutingControllerDelegate {
		[Abstract]
		[Export ("customRoutingController:handleEvent:completionHandler:")]
		void HandleEvent (AVCustomRoutingController controller, AVCustomRoutingEvent @event, AVCustomRoutingControllerDelegateCompletionHandler completionHandler);

		[Export ("customRoutingController:eventDidTimeOut:")]
		void EventDidTimeOut (AVCustomRoutingController controller, AVCustomRoutingEvent @event);

		[Export ("customRoutingController:didSelectItem:")]
		void DidSelectItem (AVCustomRoutingController controller, AVCustomRoutingActionItem customActionItem);
	}

#if !XAMCORE_5_0
}
namespace AVRouting {
#endif


	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface AVCustomRoutingEvent {
		[Export ("reason")]
		AVCustomRoutingEventReason Reason { get; }

		[Export ("route")]
		AVCustomDeviceRoute Route { get; }
	}
}
