using CoreGraphics;
using ObjCRuntime;
using Foundation;
using UIKit;

using System;

namespace CoreLocationUI {

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum CLLocationButtonIcon : long {
		None = 0,
		ArrowFilled,
		ArrowOutline,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum CLLocationButtonLabel : long {
		None = 0,
		CurrentLocation,
		SendCurrentLocation,
		SendMyCurrentLocation,
		ShareCurrentLocation,
		ShareMyCurrentLocation,
	}

	[NoTV, NoMac, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (UIControl))]
	interface CLLocationButton : NSSecureCoding {

		[Export ("initWithFrame:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGRect frame);

		[Export ("icon", ArgumentSemantic.Assign)]
		CLLocationButtonIcon Icon { get; set; }

		[Export ("label", ArgumentSemantic.Assign)]
		CLLocationButtonLabel Label { get; set; }

		[Export ("fontSize")]
		nfloat FontSize { get; set; }

		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }
	}
}
