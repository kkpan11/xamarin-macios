//
// AutomaticAssessmentConfiguration C# bindings
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//	TJ Lambert <TJ.Lambert@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

using System;
using Foundation;
using ObjCRuntime;

namespace AutomaticAssessmentConfiguration {

	[ErrorDomain ("AEAssessmentErrorDomain")]
	[iOS (13, 4)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum AEAssessmentErrorCode : long {
		Unknown = 1,
		UnsupportedPlatform = 2,
		MultipleParticipantsNotSupported = 3,
		ConfigurationUpdatesNotSupported = 4,
	}

	[iOS (14, 0)]
	[MacCatalyst (18, 0)]
	[Mac (15, 0)]
	[Native]
	enum AEAutocorrectMode : long {
		None = 0,
		Spelling = 1 << 0,
		Punctuation = 1 << 1,
	}

	[iOS (13, 4)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface AEAssessmentConfiguration : NSCopying {

		[Mac (15, 0), iOS (14, 0)]
		[MacCatalyst (18, 0)]
		[Export ("autocorrectMode")]
		AEAutocorrectMode AutocorrectMode { get; set; }

		[Mac (15, 0), iOS (14, 0)]
		[MacCatalyst (18, 0)]
		[Export ("allowsSpellCheck")]
		bool AllowsSpellCheck { get; set; }

		[Mac (15, 0), iOS (14, 0)]
		[MacCatalyst (18, 0)]
		[Export ("allowsPredictiveKeyboard")]
		bool AllowsPredictiveKeyboard { get; set; }

		[Mac (15, 0), iOS (14, 0)]
		[MacCatalyst (18, 0)]
		[Export ("allowsKeyboardShortcuts")]
		bool AllowsKeyboardShortcuts { get; set; }

		[NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("allowsActivityContinuation")]
		bool AllowsActivityContinuation { get; set; }

		[NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("allowsDictation")]
		bool AllowsDictation { get; set; }

		[NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("allowsAccessibilitySpeech")]
		bool AllowsAccessibilitySpeech { get; set; }

		[NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("allowsPasswordAutoFill")]
		bool AllowsPasswordAutoFill { get; set; }

		[NoMac, iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("allowsContinuousPathKeyboard")]
		bool AllowsContinuousPathKeyboard { get; set; }

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("configurationsByApplication", ArgumentSemantic.Copy)]
		NSDictionary<AEAssessmentApplication, AEAssessmentParticipantConfiguration> ConfigurationsByApplication { get; }

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("mainParticipantConfiguration", ArgumentSemantic.Strong)]
		AEAssessmentParticipantConfiguration MainParticipantConfiguration { get; }

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("removeApplication:")]
		void Remove (AEAssessmentApplication application);

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("setConfiguration:forApplication:")]
		void SetConfiguration (AEAssessmentParticipantConfiguration configuration, AEAssessmentApplication application);
	}

	[iOS (13, 4)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AEAssessmentSession {

		[MacCatalyst (17, 5), Mac (14, 5), iOS (17, 5)]
		[Static]
		[Export ("supportsMultipleParticipants")]
		bool SupportsMultipleParticipants { get; }

		[MacCatalyst (17, 5), Mac (14, 5), iOS (17, 5)]
		[Static]
		[Export ("supportsConfigurationUpdates")]
		bool SupportsConfigurationUpdates { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IAEAssessmentSessionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[Export ("initWithConfiguration:")]
		NativeHandle Constructor (AEAssessmentConfiguration configuration);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("configuration", ArgumentSemantic.Copy)]
		AEAssessmentConfiguration Configuration { get; }

		[Mac (12, 0), iOS (17, 5), MacCatalyst (15, 0)]
		[Export ("updateToConfiguration:")]
		void Update (AEAssessmentConfiguration configuration);

		[Export ("begin")]
		void Begin ();

		[Export ("end")]
		void End ();
	}

	interface IAEAssessmentSessionDelegate { }

	[iOS (13, 4)]
	[MacCatalyst (14, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface AEAssessmentSessionDelegate {

		[Export ("assessmentSessionDidBegin:")]
		void DidBegin (AEAssessmentSession session);

		[Export ("assessmentSession:failedToBeginWithError:")]
		void FailedToBegin (AEAssessmentSession session, NSError error);

		[Export ("assessmentSession:wasInterruptedWithError:")]
		void WasInterrupted (AEAssessmentSession session, NSError error);

		[Export ("assessmentSessionDidEnd:")]
		void DidEnd (AEAssessmentSession session);

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("assessmentSessionDidUpdate:")]
		void DidUpdate (AEAssessmentSession session);

		[iOS (17, 5), Mac (12, 0), MacCatalyst (15, 0)]
		[Export ("assessmentSession:failedToUpdateToConfiguration:error:")]
		void FailedToUpdate (AEAssessmentSession session, AEAssessmentConfiguration configuration, NSError error);
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
#if XAMCORE_5_0
	[DisableDefaultCtor]
#endif
	interface AEAssessmentApplication : NSCopying {
		[iOS (17, 5)]
		[MacCatalyst (15, 0)]
		[Export ("initWithBundleIdentifier:")]
		NativeHandle Constructor (string bundleIdentifier);

		[NoiOS]
		[NoMacCatalyst] // header says it's available in Mac Catalyst, Apple's documentation + xtro says it's not, so don't add it for now.
		[Export ("initWithBundleIdentifier:teamIdentifier:")]
		NativeHandle Constructor (string bundleIdentifier, [NullAllowed] string TeamIdentifier);

		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[NullAllowed, Export ("teamIdentifier")]
		string TeamIdentifier { get; }

		[Export ("requiresSignatureValidation")]
		bool RequiresSignatureValidation { get; set; }
	}

	[Mac (12, 0), iOS (17, 5), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface AEAssessmentParticipantConfiguration : NSCopying {
		[Export ("allowsNetworkAccess")]
		bool AllowsNetworkAccess { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("configurationInfo", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> ConfigurationInfo { get; set; }
	}
}
