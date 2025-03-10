using CoreFoundation;
using ObjCRuntime;
using Foundation;
using System;

#nullable enable

namespace CloudKit {
	// NSInteger -> CKContainer.h
	/// <summary>Enumerates values that indicate whether a user's iCloud account is available.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKAccountStatus : long {
		/// <summary>An error occured when the application tried to determine if the user's account is available.</summary>
		CouldNotDetermine = 0,
		/// <summary>The user's account is available.</summary>
		Available = 1,
		/// <summary>The user has an account, but a parental control or mobile restriction prevents its use.</summary>
		Restricted = 2,
		/// <summary>The user has no iCloud account.</summary>
		NoAccount = 3,
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		TemporarilyUnavailable = 4,
	}

	// NSUInteger -> CKContainer.h
	/// <summary>Enumerates a value that indicates that other app users can discover the current user by email address.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum CKApplicationPermissions : ulong {
		/// <summary>Other app users can discover the current user by email address.</summary>
		UserDiscoverability = 1 << 0,
	}

	// NSInteger -> CKContainer.h
	/// <summary>Enumerates the states that an application can have when attempting to obtain a permission.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKApplicationPermissionStatus : long {
		/// <summary>The application has not yet requested the permission.</summary>
		InitialState = 0,
		/// <summary>An error occured while attempting to obtain the permission.</summary>
		CouldNotComplete = 1,
		/// <summary>The user denied the permission request.</summary>
		Denied = 2,
		/// <summary>The user granted the permission request.</summary>
		Granted = 3,
	}

	// NSInteger -> CKError.h
	/// <summary>Enumerates CloudKit error conditions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("CKErrorDomain")]
	public enum CKErrorCode : long {
		None,
		InternalError = 1,
		PartialFailure = 2,
		NetworkUnavailable = 3,
		NetworkFailure = 4,
		BadContainer = 5,
		ServiceUnavailable = 6,
		RequestRateLimited = 7,
		MissingEntitlement = 8,
		NotAuthenticated = 9,
		PermissionFailure = 10,
		UnknownItem = 11,
		InvalidArguments = 12,
		ResultsTruncated = 13,
		ServerRecordChanged = 14,
		ServerRejectedRequest = 15,
		AssetFileNotFound = 16,
		AssetFileModified = 17,
		IncompatibleVersion = 18,
		ConstraintViolation = 19,
		OperationCancelled = 20,
		ChangeTokenExpired = 21,
		BatchRequestFailed = 22,
		ZoneBusy = 23,
		BadDatabase = 24,
		QuotaExceeded = 25,
		ZoneNotFound = 26,
		LimitExceeded = 27,
		UserDeletedZone = 28,
		TooManyParticipants = 29,
		AlreadyShared = 30,
		ReferenceViolation = 31,
		ManagedAccountRestricted = 32,
		ParticipantMayNeedVerification = 33,
		ResponseLost = 34,
		AssetNotAvailable = 35,
		TemporarilyUnavailable = 36,
	}

	// NSInteger -> CKModifyRecordsOperation.h
	/// <summary>Enumerates policies that control when or if a record should be saved.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKRecordSavePolicy : long {
		SaveIfServerRecordUnchanged = 0,
		SaveChangedKeys = 1,
		SaveAllKeys = 2,
	}

	// NSInteger -> CKNotification.h
	/// <summary>Enumerates the events that can generate a push notification.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKNotificationType : long {
		Query = 1,
		RecordZone = 2,
		ReadNotification = 3,
		[MacCatalyst (13, 1)]
		Database = 4,
	}

	// NSInteger -> CKNotification.h
	/// <summary>Enumerates the persistent storage events that can trigger data lifecycle notifications.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKQueryNotificationReason : long {
		RecordCreated = 1,
		RecordUpdated,
		RecordDeleted,
	}

	// NSUInteger -> CKRecordZone.h
	/// <summary>Enumerates the special operations that a zone is capable of.</summary>
	[MacCatalyst (13, 1)]
	[Flags]
	[Native]
	public enum CKRecordZoneCapabilities : ulong {
		FetchChanges = 1 << 0,
		Atomic = 1 << 1,
		[MacCatalyst (13, 1)]
		Sharing = 1 << 2,
		[iOS (15, 0), TV (15, 0)]
		[MacCatalyst (15, 0)]
		ZoneWideSharing = 1 << 3,

	}

	// NSUInteger -> CKReference.h
	/// <summary>Enumerates values that control whether a reference should delete itself when its target record is deleted.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKReferenceAction : ulong {
		None = 0,
		DeleteSelf = 1,
	}

	// NSInteger -> CKSubscription.h
	/// <summary>Enumerates subscription types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKSubscriptionType : long {
		Query = 1,
		RecordZone = 2,
		[MacCatalyst (13, 1)]
		Database = 3,
	}

	// NSInteger -> CKSubscription.h

#if !NET
	[Obsoleted (PlatformName.iOS, 14, 0, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Obsoleted (PlatformName.MacOSX, 10, 16, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'CKQuerySubscriptionOptions' instead.")]
	[Flags]
	[Native]
	public enum CKSubscriptionOptions : ulong {
		FiresOnRecordCreation = 1 << 0,
		FiresOnRecordUpdate = 1 << 1,
		FiresOnRecordDeletion = 1 << 2,
		FiresOnce = 1 << 3,
	}
#endif

	/// <summary>Enumerates values that tell whether a database is private, shared, or public.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKDatabaseScope : long {
		Public = 1,
		Private,
		Shared,
	}

	/// <summary>Enumerates responses to share participation requests.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantAcceptanceStatus : long {
		Unknown,
		Pending,
		Accepted,
		Removed,
	}

	/// <summary>Enumerates user share permissions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantPermission : long {
		Unknown,
		None,
		ReadOnly,
		ReadWrite,
	}

	/// <summary>Enumerates share participant types.</summary>
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'CKShareParticipantRole' instead.")]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'CKShareParticipantRole' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'CKShareParticipantRole' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CKShareParticipantRole' instead.")]
	[Native]
	public enum CKShareParticipantType : long {
		Unknown = 0,
		Owner = 1,
		PrivateUser = 3,
		PublicUser = 4,
	}

	/// <summary>Enumerates the time or times when a <see cref="T:CloudKit.CKSubscription" /> fires a notification.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CKQuerySubscriptionOptions : ulong {
		RecordCreation = 1 << 0,
		RecordUpdate = 1 << 1,
		RecordDeletion = 1 << 2,
		FiresOnce = 1 << 3,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CKOperationGroupTransferSize : long {
		Unknown,
		Kilobytes,
		Megabytes,
		TensOfMegabytes,
		HundredsOfMegabytes,
		Gigabytes,
		TensOfGigabytes,
		HundredsOfGigabytes,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CKShareParticipantRole : long {
		Unknown = 0,
		Owner = 1,
		PrivateUser = 3,
		PublicUser = 4,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native, Flags]
	public enum CKSharingParticipantAccessOption : ulong {
		AnyoneWithLink = 1uL << 0,
		SpecifiedRecipientsOnly = 1uL << 1,
		Any = AnyoneWithLink | SpecifiedRecipientsOnly,
	}

	[NoTV, Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[Native, Flags]
	public enum CKSharingParticipantPermissionOption : ulong {
		ReadOnly = 1uL << 0,
		ReadWrite = 1uL << 1,
		Any = ReadOnly | ReadWrite,
	}


	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineAccountChangeType : long {
		SignIn,
		SignOut,
		SwitchAccounts,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineSyncReason : long {
		Scheduled,
		Manual,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineEventType : long {
		StateUpdate,
		AccountChange,
		FetchedDatabaseChanges,
		FetchedRecordZoneChanges,
		SentDatabaseChanges,
		SentRecordZoneChanges,
		WillFetchChanges,
		WillFetchRecordZoneChanges,
		DidFetchRecordZoneChanges,
		DidFetchChanges,
		WillSendChanges,
		DidSendChanges,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEnginePendingRecordZoneChangeType : long {
		SaveRecord,
		DeleteRecord,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEngineZoneDeletionReason : long {
		Deleted,
		Purged,
		EncryptedDataReset,
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Native]
	public enum CKSyncEnginePendingDatabaseChangeType : long {
		SaveZone,
		DeleteZone,
	}

}
