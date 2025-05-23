//
// SharedWithYou C# bindings
//
// Authors:
//	Manuel de la Pena Saenz <mandel@microsoft.com>
//
// Copyright 2022 Microsoft Corporation All rights reserved.
//

using System;

using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using SharedWithYouCore;
using UniformTypeIdentifiers;
using ObjCRuntime;

#if MONOMAC
using AppKit;
using UIViewController = AppKit.NSViewController;
using UIView = AppKit.NSView;
using UIMenu = AppKit.NSMenu;
using UIImage = AppKit.NSImage;
using ICloudSharingControllerDelegate = AppKit.INSCloudSharingServiceDelegate;
using UIWindow = AppKit.NSWindow;
#else
using UIKit;
using ICloudSharingControllerDelegate = UIKit.IUICloudSharingControllerDelegate;
using NSMenuItem = Foundation.NSObject;
#endif

namespace SharedWithYou {

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWAttributionViewBackgroundStyle : long {
		Default = 0,
		Color,
		Material,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWAttributionViewDisplayContext : long {
		Summary = 0,
		Detail,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWAttributionViewHorizontalAlignment : long {
		Default = 0,
		Leading,
		Center,
		Trailing,
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0)]
	[MacCatalyst (16, 0)]
	[Native]
	public enum SWHighlightCenterErrorCode : long {
		NoError = 0,
		InternalError,
		InvalidURL,
		AccessDenied,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWHighlightChangeEventTrigger : long {
		Edit = 1,
		Comment = 2,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWHighlightMembershipEventTrigger : long {
		AddedCollaborator = 1,
		RemovedCollaborator = 2,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native]
	public enum SWHighlightPersistenceEventTrigger : long {
		Created = 1,
		Deleted = 2,
		Renamed = 3,
		Moved = 4,
	}

	[Mac (13, 0), TV (16, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (UIView))]
	interface SWAttributionView {

		[DesignatedInitializer]
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[NullAllowed, Export ("highlight", ArgumentSemantic.Strong)]
		SWHighlight Highlight { get; set; }

		[Export ("displayContext", ArgumentSemantic.Assign)]
		SWAttributionViewDisplayContext DisplayContext { get; set; }

		[Export ("horizontalAlignment", ArgumentSemantic.Assign)]
		SWAttributionViewHorizontalAlignment HorizontalAlignment { get; set; }

		[Export ("backgroundStyle", ArgumentSemantic.Assign)]
		SWAttributionViewBackgroundStyle BackgroundStyle { get; set; }

		[Export ("preferredMaxLayoutWidth")]
		nfloat PreferredMaxLayoutWidth { get; set; }

		[Export ("highlightMenu", ArgumentSemantic.Strong)]
		UIMenu HighlightMenu { get; }

		[NullAllowed, Export ("menuTitleForHideAction", ArgumentSemantic.Strong)]
		string MenuTitleForHideAction { get; set; }

		[NullAllowed, Export ("supplementalMenu", ArgumentSemantic.Strong)]
		UIMenu SupplementalMenu { get; set; }

		[NoMac, NoiOS, NoMacCatalyst]
		[Export ("enablesMarquee")]
		bool EnablesMarquee { get; set; }
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWHighlight : NSSecureCoding, NSCopying {

		[NoTV]
		[MacCatalyst (13, 1)]
		[Field ("SWCollaborationMetadataTypeIdentifier")]
		NSString MetadataTypeIdentifier { get; }

		[Export ("identifier", ArgumentSemantic.Copy)]
		NSObject/*<NSSecureCoding, NSCopying>*/ Identifier { get; }

		[Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; }
	}

	interface ISWHighlightCenterDelegate { }

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SWHighlightCenterDelegate {
		[Abstract]
		[Export ("highlightCenterHighlightsDidChange:")]
		void HighlightsDidChange (SWHighlightCenter highlightCenter);
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface SWHighlightCenter {
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISWHighlightCenterDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("highlights", ArgumentSemantic.Copy)]
		SWHighlight [] Highlights { get; }

		[Static]
		[Export ("highlightCollectionTitle")]
		string HighlightCollectionTitle { get; }

		[Static]
		[Export ("systemCollaborationSupportAvailable")]
		bool SystemCollaborationSupportAvailable { [Bind ("isSystemCollaborationSupportAvailable")] get; }

		[Async]
		[Export ("getHighlightForURL:completionHandler:")]
		void GetHighlight (NSUrl urL, Action<SWHighlight, NSError> completionHandler);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("collaborationHighlightForIdentifier:error:")]
		[return: NullAllowed]
		SWCollaborationHighlight GetCollaborationHighlight (string collaborationIdentifier, [NullAllowed] out NSError error);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("getCollaborationHighlightForURL:completionHandler:")]
		void GetCollaborationHighlight (NSUrl url, Action<SWCollaborationHighlight, NSError> completionHandler);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("postNoticeForHighlightEvent:")]
		void PostNotice (ISWHighlightEvent @event);

		[iOS (16, 1), NoTV]
		[MacCatalyst (16, 1)]
		[Export ("clearNoticesForHighlight:")]
		void ClearNotices (SWCollaborationHighlight highlight);

		[NoTV]
		[MacCatalyst (13, 1)]
		[Async]
		[Export ("getSignedIdentityProofForCollaborationHighlight:usingData:completionHandler:")]
		void GetSignedIdentityProof (SWCollaborationHighlight collaborationHighlight, NSData data, Action<SWSignedPersonIdentityProof, NSError> completionHandler);
	}

	[Mac (13, 0), NoTV, NoiOS, NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWRemoveParticipantAlert {
		[Static]
		[Export ("showAlertWithParticipant:highlight:inWindow:")]
		void ShowAlert (SWPerson participant, SWCollaborationHighlight highlight, [NullAllowed] UIWindow window);
	}

	[NoMac, NoTV, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface SWRemoveParticipantAlertController {
		[Static]
		[Export ("alertControllerWithParticipant:highlight:")]
		SWRemoveParticipantAlertController Create (SWPerson participant, SWCollaborationHighlight highlight);
	}

	interface ISWCollaborationViewDelegate { }

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SWCollaborationViewDelegate {

		// [DesignatedInitializer]
		// [Export ("initWithFrame:")]
		// NativeHandle Constructor (CGRect frame);

		[Export ("collaborationViewShouldPresentPopover:")]
		bool ShouldPresentPopover (SWCollaborationView collaborationView);

		[Export ("collaborationViewWillPresentPopover:")]
		void WillPresentPopover (SWCollaborationView collaborationView);

		[Export ("collaborationViewDidDismissPopover:")]
		void DidDismissPopover (SWCollaborationView collaborationView);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (UIView))]
	interface SWCollaborationView {

		[DesignatedInitializer]
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Wrap ("WeakCloudSharingDelegate")]
		[NullAllowed]
		ICloudSharingControllerDelegate CloudSharingDelegate { get; set; }

		[NullAllowed, Export ("cloudSharingDelegate", ArgumentSemantic.Weak)]
		NSObject WeakCloudSharingDelegate { get; set; }

		[Export ("setContentView:")]
		void SetContentView (UIView detailViewListContentView);

		[Export ("initWithItemProvider:")]
		NativeHandle Constructor (NSItemProvider itemProvider);

		[Export ("activeParticipantCount")]
		nuint ActiveParticipantCount { get; set; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		ISWCollaborationViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("headerTitle")]
		string HeaderTitle { get; set; }

		[Export ("headerSubtitle")]
		string HeaderSubtitle { get; set; }

		[Export ("headerImage", ArgumentSemantic.Strong)]
		UIImage HeaderImage { get; set; }

		[NoiOS, NoMacCatalyst]
		[Mac (13, 1)]
		[Export ("menuFormRepresentation")]
		NSMenuItem MenuFormRepresentation { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Wrap ("WeakCloudSharingControllerDelegate")]
		[NullAllowed]
		ICloudSharingControllerDelegate CloudSharingControllerDelegate { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("cloudSharingControllerDelegate", ArgumentSemantic.Weak)]
		NSObject WeakCloudSharingControllerDelegate { get; set; }

		[NoiOS, NoMacCatalyst]
		[Wrap ("WeakCloudSharingServiceDelegate")]
		[NullAllowed]
		ICloudSharingControllerDelegate CloudSharingServiceDelegate { get; set; }

		[NoiOS, NoMacCatalyst]
		[NullAllowed, Export ("cloudSharingServiceDelegate", ArgumentSemantic.Weak)]
		NSObject WeakCloudSharingServiceDelegate { get; set; }

		// should not be Async
		[Export ("dismissPopover:")]
		void DismissPopover ([NullAllowed] Action callback);

		[Export ("manageButtonTitle")]
		string ManageButtonTitle { get; set; }

		[Export ("setShowManageButton:")]
		void SetShowManageButton (bool showManageButton);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (SWHighlight))]
	interface SWCollaborationHighlight : NSSecureCoding, NSCopying {
		[Export ("collaborationIdentifier")]
		string CollaborationIdentifier { get; }

		[NullAllowed, Export ("title")]
		string Title { get; }

		[Export ("creationDate", ArgumentSemantic.Copy)]
		NSDate CreationDate { get; }

		[Export ("contentType", ArgumentSemantic.Copy)]
		UTType ContentType { get; }
	}

	interface ISWHighlightEvent { }

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface SWHighlightEvent : NSSecureCoding, NSCopying {
		[Abstract]
		[Export ("highlightURL", ArgumentSemantic.Copy)]
		NSUrl HighlightUrl { get; }
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWHighlightChangeEvent : SWHighlightEvent {
		[Export ("changeEventTrigger", ArgumentSemantic.Assign)]
		SWHighlightChangeEventTrigger ChangeEventTrigger { get; }

		[Export ("initWithHighlight:trigger:")]
		NativeHandle Constructor (SWHighlight highlight, SWHighlightChangeEventTrigger trigger);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWHighlightMembershipEvent : SWHighlightEvent {
		[Export ("membershipEventTrigger", ArgumentSemantic.Assign)]
		SWHighlightMembershipEventTrigger MembershipEventTrigger { get; }

		[Export ("initWithHighlight:trigger:")]
		NativeHandle Constructor (SWHighlight highlight, SWHighlightMembershipEventTrigger trigger);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWHighlightMentionEvent : SWHighlightEvent {
		[Export ("mentionedPersonHandle", ArgumentSemantic.Strong)]
		string MentionedPersonHandle { get; }

		[Export ("initWithHighlight:mentionedPersonCloudKitShareHandle:")]
		NativeHandle Constructor (SWHighlight highlight, string handle);

		[Export ("initWithHighlight:mentionedPersonIdentity:")]
		NativeHandle Constructor (SWHighlight highlight, SWPersonIdentity identity);
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface SWHighlightPersistenceEvent : SWHighlightEvent {
		[Export ("persistenceEventTrigger", ArgumentSemantic.Assign)]
		SWHighlightPersistenceEventTrigger PersistenceEventTrigger { get; }

		[Export ("initWithHighlight:trigger:")]
		NativeHandle Constructor (SWHighlight highlight, SWHighlightPersistenceEventTrigger trigger);
	}


}
