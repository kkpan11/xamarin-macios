// Copyright 2011-2015 Xamarin Inc. All rights reserved.
using System;

using Foundation;
using ObjCRuntime;

namespace Security {

	// this is a subset of OSStatus -> SInt32 -> signed int - see CoreFoundation.framework/Headers/CFBase.h
	// values are defined in Security.framework/Headers/SecBase.h 
	/// <summary>Status return from the SecKeyChain operations.</summary>
	public enum SecStatusCode {
		/// <summary>Success, there was no error.</summary>
		Success = 0,
		/// <summary>The specified feature is not implemented.</summary>
		Unimplemented = -4,
		/// <summary>Indicates that the disk was full.</summary>
		DiskFull = -34,
		/// <summary>Indicates an IO error.</summary>
		IO = -36,
		/// <summary>Indicates that a file is already open with read and write privileges.</summary>
		OpWr = -49,
		/// <summary>Invalid or incomplete parameters passed.</summary>
		Param = -50,
		WritePermissions = -61,
		Allocate = -108,
		/// <summary>Indicates that the user cancelled the operation.</summary>
		UserCanceled = -128,
		/// <summary>Indicates that authentication failed.</summary>
		BadReq = -909,
		/// <summary>Indicates an unspecified error in an internal component.</summary>
		InternalComponent = -2070,
		/// <summary>Indicates that an unknown Core Foundation error occurred.</summary>
		CoreFoundationUnknown = -4960,
		/// <summary>Indicates that a trust results were not available.</summary>
		NotAvailable = -25291,
		/// <summary>The keychain is read only.</summary>
		ReadOnly = -25292,
		/// <summary>Authentication failed.</summary>
		AuthFailed = -25293,
		/// <summary>The keychain specified does not exist.</summary>
		NoSuchKeyChain = -25294,
		/// <summary>The keychain provided is invalid.</summary>
		InvalidKeyChain = -25295,
		/// <summary>Duplicated key chain.</summary>
		DuplicateKeyChain = -25296,
		/// <summary>The item is duplicated.</summary>
		DuplicateItem = -25299,
		/// <summary>The item was not found.</summary>
		ItemNotFound = -25300,
		/// <summary>Indicates that the interaction with the Security Server was not allowed.</summary>
		InteractionNotAllowed = -25308,
		Decode = -26275,
		/// <summary>Indicates that a callback was duplicated</summary>
		DuplicateCallback = -25297,
		/// <summary>Indicates that a callback was not valid.</summary>
		InvalidCallback = -25298,
		/// <summary>Indicates that a buffer was too small.</summary>
		BufferTooSmall = -25301,
		/// <summary>Indicates that data were too large.</summary>
		DataTooLarge = -25302,
		/// <summary>Indicates that no such attribute was found.</summary>
		NoSuchAttribute = -25303,
		/// <summary>Indicates an invalid item reference.</summary>
		InvalidItemRef = -25304,
		/// <summary>Indicates an invalid search reference.</summary>
		InvalidSearchRef = -25305,
		NoSuchClass = -25306,
		/// <summary>Indicates that no default key chain was found.</summary>
		NoDefaultKeychain = -25307,
		ReadOnlyAttribute = -25309,
		/// <summary>Indicates that the wrong security version was encountered.</summary>
		WrongSecVersion = -25310,
		/// <summary>Indicates that a key's size was not supported.</summary>
		KeySizeNotAllowed = -25311,
		/// <summary>Indicates that no storage module was found.</summary>
		NoStorageModule = -25312,
		/// <summary>Indicates that no certificate module was found.</summary>
		NoCertificateModule = -25313,
		/// <summary>Indicates that no policy module was found.</summary>
		NoPolicyModule = -25314,
		/// <summary>Indicates that user interaction was required.</summary>
		InteractionRequired = -25315,
		/// <summary>Indicates that certain data were not available.</summary>
		DataNotAvailable = -25316,
		/// <summary>Indicates that data could not be modified.</summary>
		DataNotModifiable = -25317,
		/// <summary>Indicates that the certificate chain failed.</summary>
		CreateChainFailed = -25318,
		/// <summary>Indicates an invalid preferences domain.</summary>
		InvalidPrefsDomain = -25319,
		/// <summary>Indicates that a dark wake state prevented a UI from being displayed.</summary>
		InDarkWake = -25320,
		ACLNotSimple = -25240,
		PolicyNotFound = -25241,
		InvalidTrustSetting = -25242,
		NoAccessForItem = -25243,
		InvalidOwnerEdit = -25244,
		TrustNotAvailable = -25245,
		UnsupportedFormat = -25256,
		UnknownFormat = -25257,
		KeyIsSensitive = -25258,
		MultiplePrivateKeys = -25259,
		PassphraseRequired = -25260,
		InvalidPasswordRef = -25261,
		InvalidTrustSettings = -25262,
		NoTrustSettings = -25263,
		Pkcs12VerifyFailure = -25264,
		NotSigner = -26267,
		MissingEntitlement = -34018,
		RestrictedApi = -34020,
		ServiceNotAvailable = -67585,
		InsufficientClientID = -67586,
		DeviceReset = -67587,
		DeviceFailed = -67588,
		AppleAddAppACLSubject = -67589,
		ApplePublicKeyIncomplete = -67590,
		AppleSignatureMismatch = -67591,
		AppleInvalidKeyStartDate = -67592,
		AppleInvalidKeyEndDate = -67593,
		ConversionError = -67594,
		AppleSSLv2Rollback = -67595,
		QuotaExceeded = -67596,
		FileTooBig = -67597,
		InvalidDatabaseBlob = -67598,
		InvalidKeyBlob = -67599,
		IncompatibleDatabaseBlob = -67600,
		IncompatibleKeyBlob = -67601,
		HostNameMismatch = -67602,
		UnknownCriticalExtensionFlag = -67603,
		NoBasicConstraints = -67604,
		NoBasicConstraintsCA = -67605,
		InvalidAuthorityKeyID = -67606,
		InvalidSubjectKeyID = -67607,   /* The subject key ID is not valid. */
		InvalidKeyUsageForPolicy = -67608,  /* The key usage is not valid for the specified policy. */
		InvalidExtendedKeyUsage = -67609,   /* The extended key usage is not valid. */
		InvalidIDLinkage = -67610,  /* The ID linkage is not valid. */
		PathLengthConstraintExceeded = -67611,  /* The path length constraint was exceeded. */
		InvalidRoot = -67612,   /* The root or anchor certificate is not valid. */
		CRLExpired = -67613,    /* The CRL has expired. */
		CRLNotValidYet = -67614,    /* The CRL is not yet valid. */
		CRLNotFound = -67615,   /* The CRL was not found. */
		CRLServerDown = -67616, /* The CRL server is down. */
		CRLBadURI = -67617, /* The CRL has a bad Uniform Resource Identifier. */
		UnknownCertExtension = -67618,  /* An unknown certificate extension was encountered. */
		UnknownCRLExtension = -67619,   /* An unknown CRL extension was encountered. */
		CRLNotTrusted = -67620, /* The CRL is not trusted. */
		CRLPolicyFailed = -67621,   /* The CRL policy failed. */
		IDPFailure = -67622,    /* The issuing distribution point was not valid. */
		SMIMEEmailAddressesNotFound = -67623,   /* An email address mismatch was encountered. */
		SMIMEBadExtendedKeyUsage = -67624,  /* The appropriate extended key usage for SMIME was not found. */
		SMIMEBadKeyUsage = -67625,  /* The key usage is not compatible with SMIME. */
		SMIMEKeyUsageNotCritical = -67626,  /* The key usage extension is not marked as critical. */
		SMIMENoEmailAddress = -67627,   /* No email address was found in the certificate. */
		SMIMESubjAltNameNotCritical = -67628,   /* The subject alternative name extension is not marked as critical. */
		SSLBadExtendedKeyUsage = -67629,    /* The appropriate extended key usage for SSL was not found. */
		OCSPBadResponse = -67630,   /* The OCSP response was incorrect or could not be parsed. */
		OCSPBadRequest = -67631,    /* The OCSP request was incorrect or could not be parsed. */
		OCSPUnavailable = -67632,   /* OCSP service is unavailable. */
		OCSPStatusUnrecognized = -67633,    /* The OCSP server did not recognize this certificate. */
		EndOfData = -67634, /* An end-of-data was detected. */
		IncompleteCertRevocationCheck = -67635, /* An incomplete certificate revocation check occurred. */
		NetworkFailure = -67636,    /* A network failure occurred. */
		OCSPNotTrustedToAnchor = -67637,    /* The OCSP response was not trusted to a root or anchor certificate. */
		RecordModified = -67638,    /* The record was modified. */
		OCSPSignatureError = -67639,    /* The OCSP response had an invalid signature. */
		OCSPNoSigner = -67640,  /* The OCSP response had no signer. */
		OCSPResponderMalformedReq = -67641, /* The OCSP responder was given a malformed request. */
		OCSPResponderInternalError = -67642,    /* The OCSP responder encountered an internal error. */
		OCSPResponderTryLater = -67643, /* The OCSP responder is busy, try again later. */
		OCSPResponderSignatureRequired = -67644,    /* The OCSP responder requires a signature. */
		OCSPResponderUnauthorized = -67645, /* The OCSP responder rejected this request as unauthorized. */
		OCSPResponseNonceMismatch = -67646, /* The OCSP response nonce did not match the request. */
		CodeSigningBadCertChainLength = -67647, /* Code signing encountered an incorrect certificate chain length. */
		CodeSigningNoBasicConstraints = -67648, /* Code signing found no basic constraints. */
		CodeSigningBadPathLengthConstraint = -67649,    /* Code signing encountered an incorrect path length constraint. */
		CodeSigningNoExtendedKeyUsage = -67650, /* Code signing found no extended key usage. */
		CodeSigningDevelopment = -67651,    /* Code signing indicated use of a development-only certificate. */
		ResourceSignBadCertChainLength = -67652,    /* Resource signing has encountered an incorrect certificate chain length. */
		ResourceSignBadExtKeyUsage = -67653,    /* Resource signing has encountered an error in the extended key usage. */
		TrustSettingDeny = -67654,  /* The trust setting for this policy was set to Deny. */
		InvalidSubjectName = -67655,    /* An invalid certificate subject name was encountered. */
		UnknownQualifiedCertStatement = -67656, /* An unknown qualified certificate statement was encountered. */
		MobileMeRequestQueued = -67657, /* The MobileMe request will be sent during the next connection. */
		MobileMeRequestRedirected = -67658, /* The MobileMe request was redirected. */
		MobileMeServerError = -67659,   /* A MobileMe server error occurred. */
		MobileMeServerNotAvailable = -67660,    /* The MobileMe server is not available. */
		MobileMeServerAlreadyExists = -67661,   /* The MobileMe server reported that the item already exists. */
		MobileMeServerServiceErr = -67662,  /* A MobileMe service error has occurred. */
		MobileMeRequestAlreadyPending = -67663, /* A MobileMe request is already pending. */
		MobileMeNoRequestPending = -67664,  /* MobileMe has no request pending. */
		MobileMeCSRVerifyFailure = -67665,  /* A MobileMe CSR verification failure has occurred. */
		MobileMeFailedConsistencyCheck = -67666,    /* MobileMe has found a failed consistency check. */
		NotInitialized = -67667,    /* A function was called without initializing CSSM. */
		InvalidHandleUsage = -67668,    /* The CSSM handle does not match with the service type. */
		PVCReferentNotFound = -67669,   /* A reference to the calling module was not found in the list of authorized callers. */
		FunctionIntegrityFail = -67670, /* A function address was not within the verified module. */
		InternalError = -67671, /* An internal error has occurred. */
		MemoryError = -67672,   /* A memory error has occurred. */
		InvalidData = -67673,   /* Invalid data was encountered. */
		MDSError = -67674,  /* A Module Directory Service error has occurred. */
		InvalidPointer = -67675,    /* An invalid pointer was encountered. */
		SelfCheckFailed = -67676,   /* Self-check has failed. */
		FunctionFailed = -67677,    /* A function has failed. */
		ModuleManifestVerifyFailed = -67678,    /* A module manifest verification failure has occurred. */
		InvalidGUID = -67679,   /* An invalid GUID was encountered. */
		InvalidHandle = -67680, /* An invalid handle was encountered. */
		InvalidDBList = -67681, /* An invalid DB list was encountered. */
		InvalidPassthroughID = -67682,  /* An invalid passthrough ID was encountered. */
		InvalidNetworkAddress = -67683, /* An invalid network address was encountered. */
		CRLAlreadySigned = -67684,  /* The certificate revocation list is already signed. */
		InvalidNumberOfFields = -67685, /* An invalid number of fields were encountered. */
		VerificationFailure = -67686,   /* A verification failure occurred. */
		UnknownTag = -67687,    /* An unknown tag was encountered. */
		InvalidSignature = -67688,  /* An invalid signature was encountered. */
		InvalidName = -67689,   /* An invalid name was encountered. */
		InvalidCertificateRef = -67690, /* An invalid certificate reference was encountered. */
		InvalidCertificateGroup = -67691,   /* An invalid certificate group was encountered. */
		TagNotFound = -67692,   /* The specified tag was not found. */
		InvalidQuery = -67693,  /* The specified query was not valid. */
		InvalidValue = -67694,  /* An invalid value was detected. */
		CallbackFailed = -67695,    /* A callback has failed. */
		ACLDeleteFailed = -67696,   /* An ACL delete operation has failed. */
		ACLReplaceFailed = -67697,  /* An ACL replace operation has failed. */
		ACLAddFailed = -67698,  /* An ACL add operation has failed. */
		ACLChangeFailed = -67699,   /* An ACL change operation has failed. */
		InvalidAccessCredentials = -67700,  /* Invalid access credentials were encountered. */
		InvalidRecord = -67701, /* An invalid record was encountered. */
		InvalidACL = -67702,    /* An invalid ACL was encountered. */
		InvalidSampleValue = -67703,    /* An invalid sample value was encountered. */
		IncompatibleVersion = -67704,   /* An incompatible version was encountered. */
		PrivilegeNotGranted = -67705,   /* The privilege was not granted. */
		InvalidScope = -67706,  /* An invalid scope was encountered. */
		PVCAlreadyConfigured = -67707,  /* The PVC is already configured. */
		InvalidPVC = -67708,    /* An invalid PVC was encountered. */
		EMMLoadFailed = -67709, /* The EMM load has failed. */
		EMMUnloadFailed = -67710,   /* The EMM unload has failed. */
		AddinLoadFailed = -67711,   /* The add-in load operation has failed. */
		InvalidKeyRef = -67712, /* An invalid key was encountered. */
		InvalidKeyHierarchy = -67713,   /* An invalid key hierarchy was encountered. */
		AddinUnloadFailed = -67714, /* The add-in unload operation has failed. */
		LibraryReferenceNotFound = -67715,  /* A library reference was not found. */
		InvalidAddinFunctionTable = -67716, /* An invalid add-in function table was encountered. */
		InvalidServiceMask = -67717,    /* An invalid service mask was encountered. */
		ModuleNotLoaded = -67718,   /* A module was not loaded. */
		InvalidSubServiceID = -67719,   /* An invalid subservice ID was encountered. */
		AttributeNotInContext = -67720, /* An attribute was not in the context. */
		ModuleManagerInitializeFailed = -67721, /* A module failed to initialize. */
		ModuleManagerNotFound = -67722, /* A module was not found. */
		EventNotificationCallbackNotFound = -67723, /* An event notification callback was not found. */
		InputLengthError = -67724,  /* An input length error was encountered. */
		OutputLengthError = -67725, /* An output length error was encountered. */
		PrivilegeNotSupported = -67726, /* The privilege is not supported. */
		DeviceError = -67727,   /* A device error was encountered. */
		AttachHandleBusy = -67728,  /* The CSP handle was busy. */
		NotLoggedIn = -67729,   /* You are not logged in. */
		AlgorithmMismatch = -67730, /* An algorithm mismatch was encountered. */
		KeyUsageIncorrect = -67731, /* The key usage is incorrect. */
		KeyBlobTypeIncorrect = -67732,  /* The key blob type is incorrect. */
		KeyHeaderInconsistent = -67733, /* The key header is inconsistent. */
		UnsupportedKeyFormat = -67734,  /* The key header format is not supported. */
		UnsupportedKeySize = -67735,    /* The key size is not supported. */
		InvalidKeyUsageMask = -67736,   /* The key usage mask is not valid. */
		UnsupportedKeyUsageMask = -67737,   /* The key usage mask is not supported. */
		InvalidKeyAttributeMask = -67738,   /* The key attribute mask is not valid. */
		UnsupportedKeyAttributeMask = -67739,   /* The key attribute mask is not supported. */
		InvalidKeyLabel = -67740,   /* The key label is not valid. */
		UnsupportedKeyLabel = -67741,   /* The key label is not supported. */
		InvalidKeyFormat = -67742,  /* The key format is not valid. */
		UnsupportedVectorOfBuffers = -67743,    /* The vector of buffers is not supported. */
		InvalidInputVector = -67744,    /* The input vector is not valid. */
		InvalidOutputVector = -67745,   /* The output vector is not valid. */
		InvalidContext = -67746,    /* An invalid context was encountered. */
		InvalidAlgorithm = -67747,  /* An invalid algorithm was encountered. */
		InvalidAttributeKey = -67748,   /* A key attribute was not valid. */
		MissingAttributeKey = -67749,   /* A key attribute was missing. */
		InvalidAttributeInitVector = -67750,    /* An init vector attribute was not valid. */
		MissingAttributeInitVector = -67751,    /* An init vector attribute was missing. */
		InvalidAttributeSalt = -67752,  /* A salt attribute was not valid. */
		MissingAttributeSalt = -67753,  /* A salt attribute was missing. */
		InvalidAttributePadding = -67754,   /* A padding attribute was not valid. */
		MissingAttributePadding = -67755,   /* A padding attribute was missing. */
		InvalidAttributeRandom = -67756,    /* A random number attribute was not valid. */
		MissingAttributeRandom = -67757,    /* A random number attribute was missing. */
		InvalidAttributeSeed = -67758,  /* A seed attribute was not valid. */
		MissingAttributeSeed = -67759,  /* A seed attribute was missing. */
		InvalidAttributePassphrase = -67760,    /* A passphrase attribute was not valid. */
		MissingAttributePassphrase = -67761,    /* A passphrase attribute was missing. */
		InvalidAttributeKeyLength = -67762, /* A key length attribute was not valid. */
		MissingAttributeKeyLength = -67763, /* A key length attribute was missing. */
		InvalidAttributeBlockSize = -67764, /* A block size attribute was not valid. */
		MissingAttributeBlockSize = -67765, /* A block size attribute was missing. */
		InvalidAttributeOutputSize = -67766,    /* An output size attribute was not valid. */
		MissingAttributeOutputSize = -67767,    /* An output size attribute was missing. */
		InvalidAttributeRounds = -67768,    /* The number of rounds attribute was not valid. */
		MissingAttributeRounds = -67769,    /* The number of rounds attribute was missing. */
		InvalidAlgorithmParms = -67770, /* An algorithm parameters attribute was not valid. */
		MissingAlgorithmParms = -67771, /* An algorithm parameters attribute was missing. */
		InvalidAttributeLabel = -67772, /* A label attribute was not valid. */
		MissingAttributeLabel = -67773, /* A label attribute was missing. */
		InvalidAttributeKeyType = -67774,   /* A key type attribute was not valid. */
		MissingAttributeKeyType = -67775,   /* A key type attribute was missing. */
		InvalidAttributeMode = -67776,  /* A mode attribute was not valid. */
		MissingAttributeMode = -67777,  /* A mode attribute was missing. */
		InvalidAttributeEffectiveBits = -67778, /* An effective bits attribute was not valid. */
		MissingAttributeEffectiveBits = -67779, /* An effective bits attribute was missing. */
		InvalidAttributeStartDate = -67780, /* A start date attribute was not valid. */
		MissingAttributeStartDate = -67781, /* A start date attribute was missing. */
		InvalidAttributeEndDate = -67782,   /* An end date attribute was not valid. */
		MissingAttributeEndDate = -67783,   /* An end date attribute was missing. */
		InvalidAttributeVersion = -67784,   /* A version attribute was not valid. */
		MissingAttributeVersion = -67785,   /* A version attribute was missing. */
		InvalidAttributePrime = -67786, /* A prime attribute was not valid. */
		MissingAttributePrime = -67787, /* A prime attribute was missing. */
		InvalidAttributeBase = -67788,  /* A base attribute was not valid. */
		MissingAttributeBase = -67789,  /* A base attribute was missing. */
		InvalidAttributeSubprime = -67790,  /* A subprime attribute was not valid. */
		MissingAttributeSubprime = -67791,  /* A subprime attribute was missing. */
		InvalidAttributeIterationCount = -67792,    /* An iteration count attribute was not valid. */
		MissingAttributeIterationCount = -67793,    /* An iteration count attribute was missing. */
		InvalidAttributeDLDBHandle = -67794,    /* A database handle attribute was not valid. */
		MissingAttributeDLDBHandle = -67795,    /* A database handle attribute was missing. */
		InvalidAttributeAccessCredentials = -67796, /* An access credentials attribute was not valid. */
		MissingAttributeAccessCredentials = -67797, /* An access credentials attribute was missing. */
		InvalidAttributePublicKeyFormat = -67798,   /* A public key format attribute was not valid. */
		MissingAttributePublicKeyFormat = -67799,   /* A public key format attribute was missing. */
		InvalidAttributePrivateKeyFormat = -67800,  /* A private key format attribute was not valid. */
		MissingAttributePrivateKeyFormat = -67801,  /* A private key format attribute was missing. */
		InvalidAttributeSymmetricKeyFormat = -67802,    /* A symmetric key format attribute was not valid. */
		MissingAttributeSymmetricKeyFormat = -67803,    /* A symmetric key format attribute was missing. */
		InvalidAttributeWrappedKeyFormat = -67804,  /* A wrapped key format attribute was not valid. */
		MissingAttributeWrappedKeyFormat = -67805,  /* A wrapped key format attribute was missing. */
		StagedOperationInProgress = -67806, /* A staged operation is in progress. */
		StagedOperationNotStarted = -67807, /* A staged operation was not started. */
		VerifyFailed = -67808,  /* A cryptographic verification failure has occurred. */
		QuerySizeUnknown = -67809,  /* The query size is unknown. */
		BlockSizeMismatch = -67810, /* A block size mismatch occurred. */
		PublicKeyInconsistent = -67811, /* The public key was inconsistent. */
		DeviceVerifyFailed = -67812,    /* A device verification failure has occurred. */
		InvalidLoginName = -67813,  /* An invalid login name was detected. */
		AlreadyLoggedIn = -67814,   /* The user is already logged in. */
		InvalidDigestAlgorithm = -67815,    /* An invalid digest algorithm was detected. */
		InvalidCRLGroup = -67816,   /* An invalid CRL group was detected. */
		CertificateCannotOperate = -67817,  /* The certificate cannot operate. */
		CertificateExpired = -67818,    /* An expired certificate was detected. */
		CertificateNotValidYet = -67819,    /* The certificate is not yet valid. */
		CertificateRevoked = -67820,    /* The certificate was revoked. */
		CertificateSuspended = -67821,  /* The certificate was suspended. */
		InsufficientCredentials = -67822,   /* Insufficient credentials were detected. */
		InvalidAction = -67823, /* The action was not valid. */
		InvalidAuthority = -67824,  /* The authority was not valid. */
		VerifyActionFailed = -67825,    /* A verify action has failed. */
		InvalidCertAuthority = -67826,  /* The certificate authority was not valid. */
		InvalidCRLAuthority = -67827,   /* The CRL authority was not valid. */
#if MONOMAC
		[Obsolete ("Use InvalidCRLAuthority.")]
		InvaldCRLAuthority = InvalidCRLAuthority,
#endif
		InvalidCRLEncoding = -67828,    /* The CRL encoding was not valid. */
		InvalidCRLType = -67829,    /* The CRL type was not valid. */
		InvalidCRL = -67830,    /* The CRL was not valid. */
		InvalidFormType = -67831,   /* The form type was not valid. */
		InvalidID = -67832, /* The ID was not valid. */
		InvalidIdentifier = -67833, /* The identifier was not valid. */
		InvalidIndex = -67834,  /* The index was not valid. */
		InvalidPolicyIdentifiers = -67835,  /* The policy identifiers are not valid. */
		InvalidTimeString = -67836, /* The time specified was not valid. */
		InvalidReason = -67837, /* The trust policy reason was not valid. */
		InvalidRequestInputs = -67838,  /* The request inputs are not valid. */
		InvalidResponseVector = -67839, /* The response vector was not valid. */
		InvalidStopOnPolicy = -67840,   /* The stop-on policy was not valid. */
		InvalidTuple = -67841,  /* The tuple was not valid. */
		MultipleValuesUnsupported = -67842, /* Multiple values are not supported. */
		NotTrusted = -67843,    /* The trust policy was not trusted. */
		NoDefaultAuthority = -67844,    /* No default authority was detected. */
		RejectedForm = -67845,  /* The trust policy had a rejected form. */
		RequestLost = -67846,   /* The request was lost. */
		RequestRejected = -67847,   /* The request was rejected. */
		UnsupportedAddressType = -67848,    /* The address type is not supported. */
		UnsupportedService = -67849,    /* The service is not supported. */
		InvalidTupleGroup = -67850, /* The tuple group was not valid. */
		InvalidBaseACLs = -67851,   /* The base ACLs are not valid. */
		InvalidTupleCredentials = -67852,   /* The tuple credentials are not valid. */
#if MONOMAC
		[Obsolete ("Use InvalidTupleCredentials.")]
		InvalidTupleCredendtials = InvalidTupleCredentials,
#endif
		InvalidEncoding = -67853,   /* The encoding was not valid. */
		InvalidValidityPeriod = -67854, /* The validity period was not valid. */
		InvalidRequestor = -67855,  /* The requestor was not valid. */
		RequestDescriptor = -67856, /* The request descriptor was not valid. */
		InvalidBundleInfo = -67857, /* The bundle information was not valid. */
		InvalidCRLIndex = -67858,   /* The CRL index was not valid. */
		NoFieldValues = -67859, /* No field values were detected. */
		UnsupportedFieldFormat = -67860,    /* The field format is not supported. */
		UnsupportedIndexInfo = -67861,  /* The index information is not supported. */
		UnsupportedLocality = -67862,   /* The locality is not supported. */
		UnsupportedNumAttributes = -67863,  /* The number of attributes is not supported. */
		UnsupportedNumIndexes = -67864, /* The number of indexes is not supported. */
		UnsupportedNumRecordTypes = -67865, /* The number of record types is not supported. */
		FieldSpecifiedMultiple = -67866,    /* Too many fields were specified. */
		IncompatibleFieldFormat = -67867,   /* The field format was incompatible. */
		InvalidParsingModule = -67868,  /* The parsing module was not valid. */
		DatabaseLocked = -67869,    /* The database is locked. */
		DatastoreIsOpen = -67870,   /* The data store is open. */
		MissingValue = -67871,  /* A missing value was detected. */
		UnsupportedQueryLimits = -67872,    /* The query limits are not supported. */
		UnsupportedNumSelectionPreds = -67873,  /* The number of selection predicates is not supported. */
		UnsupportedOperator = -67874,   /* The operator is not supported. */
		InvalidDBLocation = -67875, /* The database location is not valid. */
		InvalidAccessRequest = -67876,  /* The access request is not valid. */
		InvalidIndexInfo = -67877,  /* The index information is not valid. */
		InvalidNewOwner = -67878,   /* The new owner is not valid. */
		InvalidModifyMode = -67879, /* The modify mode is not valid. */
		MissingRequiredExtension = -67880,  /* A required certificate extension is missing. */
		ExtendedKeyUsageNotCritical = -67881,   /* The extended key usage extension was not marked critical. */
		TimestampMissing = -67882,  /* A timestamp was expected but was not found. */
		TimestampInvalid = -67883,  /* The timestamp was not valid. */
		TimestampNotTrusted = -67884,   /* The timestamp was not trusted. */
		TimestampServiceNotAvailable = -67885,  /* The timestamp service is not available. */
		TimestampBadAlg = -67886,   /* An unrecognized or unsupported Algorithm Identifier in timestamp. */
		TimestampBadRequest = -67887,   /* The timestamp transaction is not permitted or supported. */
		TimestampBadDataFormat = -67888,    /* The timestamp data submitted has the wrong format. */
		TimestampTimeNotAvailable = -67889, /* The time source for the Timestamp Authority is not available. */
		TimestampUnacceptedPolicy = -67890, /* The requested policy is not supported by the Timestamp Authority. */
		TimestampUnacceptedExtension = -67891,  /* The requested extension is not supported by the Timestamp Authority. */
		TimestampAddInfoNotAvailable = -67892,  /* The additional information requested is not available. */
		TimestampSystemFailure = -67893,    /* The timestamp request cannot be handled due to system failure. */
		SigningTimeMissing = -67894,    /* A signing time was expected but was not found. */
		TimestampRejection = -67895,    /* A timestamp transaction was rejected. */
		TimestampWaiting = -67896,  /* A timestamp transaction is waiting. */
		TimestampRevocationWarning = -67897,    /* A timestamp authority revocation warning was issued. */
		TimestampRevocationNotification = -67898,   /* A timestamp authority revocation notification was issued. */
		CertificatePolicyNotAllowed = -67899,
		CertificateNameNotAllowed = -67900,
		CertificateValidityPeriodTooLong = -67901,
		CertificateIsCA = -67902,
	}

	// typedef uint32_t SecPadding;
	// values are defined in Security.framework/Headers/SecKey.h 
	/// <summary>An enumeration whose values specify the type of padding to be used in encryption and decryption.</summary>
	public enum SecPadding {
		None = 0,
		PKCS1 = 1,

		[NoMac]
		[MacCatalyst (13, 1)]
		OAEP = 2,

		[MacCatalyst (13, 1)]
		Raw = 0x4000,

		[Obsolete ("Don't use hash algorithm.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		PKCS1MD2 = 0x8000,

		[Obsolete ("Don't use hash algorithm.")]
		[NoTV]
		[MacCatalyst (13, 1)]
		PKCS1MD5 = 0x8001,

		PKCS1SHA1 = 0x8002,

		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA224 = 0x8003,
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA256 = 0x8004,
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA384 = 0x8005,
		[NoMac]
		[MacCatalyst (13, 1)]
		PKCS1SHA512 = 0x8006,
	}

	// typedef uint32_t SecTrustResultType;
	// values are defined in Security.framework/Headers/SecTrust.h 
	/// <summary>Result code for SecTrust.Evaluate</summary>
	///     <remarks>In general both Proceed and Unspecified means you can trust the certificate, other values means it should not be trusted.</remarks>
	[NativeName ("SecTrustResultType")]
	public enum SecTrustResult {
		Invalid,
		Proceed,

		[Deprecated (PlatformName.iOS, 7, 0)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		Confirm,
		Deny,
		Unspecified,
		RecoverableTrustFailure,
		FatalTrustFailure,
		ResultOtherError,
	}

	// convenience enum mapping to kSecUseAuthenticationUI* fields
	/// <summary>Enumeration whose values represent valid options for <see cref="P:Security.SecRecord.AuthenticationUI" />.</summary>
	[MacCatalyst (13, 1)]
	public enum SecAuthenticationUI {
		/// <summary>To be added.</summary>
		NotSet = -1,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Field ("kSecUseAuthenticationUIAllow")]
		Allow,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'LAContext.InteractionNotAllowed' instead.")]
		[Field ("kSecUseAuthenticationUIFail")]
		Fail,

		/// <summary>To be added.</summary>
		[Field ("kSecUseAuthenticationUISkip")]
		Skip,
	}

	/// <summary>Enumeration defining valid options for <see cref="P:Security.SecRecord.TokenID" />.</summary>
	[MacCatalyst (13, 1)]
	public enum SecTokenID {
		None = 0,

		[Field ("kSecAttrTokenIDSecureEnclave")]
		SecureEnclave,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum SecKeyOperationType : long {
		Sign = 0,
		Verify = 1,
		Encrypt = 2,
		Decrypt = 3,
		KeyExchange = 4,
	}

	// untyped enum in Security.framework/Headers/SecPolicy.h but the API use CFOptionFlags
	// which is defined as in CFBase.h (do not trust Apple web documentation)
	/// <summary>Enumerates certificate revocation methods.</summary>
	[Flags]
	[Native]
	public enum SecRevocation : ulong {
		None,
		OCSPMethod = 1,
		CRLMethod = 2,
		PreferCRL = 4,
		RequirePositiveResponse = 8,
		NetworkAccessDisabled = 16,
		UseAnyAvailableMethod = OCSPMethod | CRLMethod,
	}
}
