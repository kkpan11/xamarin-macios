using System;
using ObjCRuntime;
using Foundation;

namespace LocalAuthentication {

	/// <summary>Authentication policies.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LAPolicy : long {
		[NoWatch]
		[MacCatalyst (13, 1)]
		DeviceOwnerAuthenticationWithBiometrics = 1,
		DeviceOwnerAuthentication = 2,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'DeviceOwnerAuthenticationWithCompanion' instead.")]
		[NoiOS]
		[NoWatch]
		[NoMacCatalyst]
		DeviceOwnerAuthenticationWithWatch = 3,
		[NoWatch, NoTV, MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		DeviceOwnerAuthenticationWithCompanion = 3,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'DeviceOwnerAuthenticationWithBiometricsOrCompanion' instead.")]
		[NoiOS]
		[NoWatch]
		[NoMacCatalyst]
		DeviceOwnerAuthenticationWithBiometricsOrWatch = 4,
		[NoWatch, NoTV, MacCatalyst (18, 0), Mac (15, 0), iOS (18, 0)]
		DeviceOwnerAuthenticationWithBiometricsOrCompanion = 4,
		[Obsolete ("Use DeviceOwnerAuthenticationWithBiometricsOrWatch enum value instead.")]
		[NoiOS]
		[NoWatch]
		[NoMacCatalyst]
		OwnerAuthenticationWithBiometricsOrWatch = DeviceOwnerAuthenticationWithBiometricsOrWatch,
		[NoMac, NoiOS, NoMacCatalyst, Watch (9, 0)]
		DeviceOwnerAuthenticationWithWristDetection = 5,
	}

	/// <summary>Status and error codes returned by methods in LocalAuthentication.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native ("LAError")]
	[ErrorDomain ("LAErrorDomain")]
	public enum LAStatus : long {
		Success = 0,
		/// Authentication was not successful, because user failed to provide valid credentials.
		AuthenticationFailed = -1,
		/// Authentication was canceled by user (e.g. tapped Cancel button).
		UserCancel = -2,
		/// Authentication was canceled, because the user tapped the fallback button (Enter Password).
		UserFallback = -3,
		/// Authentication was canceled by system (e.g. another application went to foreground).
		SystemCancel = -4,
		/// Authentication could not start, because passcode is not set on the device.
		PasscodeNotSet = -5,

#if !NET
		/// Authentication could not start, because Touch ID is not available on the device.
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryNotAvailable' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryNotAvailable' instead.")]
		TouchIDNotAvailable = BiometryNotAvailable,

		/// Authentication could not start, because Touch ID has no enrolled fingers.
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryNotEnrolled' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryNotEnrolled' instead.")]
		TouchIDNotEnrolled = BiometryNotEnrolled,

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'BiometryLockout' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'BiometryLockout' instead.")]
		TouchIDLockout = BiometryLockout,
#endif
		AppCancel = -9,
		InvalidContext = -10,
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'CompanionNotAvailable' instead.")]
		[NoiOS, NoWatch, NoMacCatalyst]
		WatchNotAvailable = -11,
		[NoiOS, NoWatch, NoMacCatalyst]
		BiometryNotPaired = -12,
		[NoiOS, NoWatch, NoMacCatalyst]
		BiometryDisconnected = -13,
		[NoiOS, NoWatch, NoMacCatalyst]
		InvalidDimension = -14,
		[NoWatch]
		[MacCatalyst (13, 1)]
		BiometryNotAvailable = -6,
		[NoWatch]
		[MacCatalyst (13, 1)]
		BiometryNotEnrolled = -7,
		[NoWatch]
		[MacCatalyst (13, 1)]
		BiometryLockout = -8,
		NotInteractive = -1004,
		[NoWatch]
		CompanionNotAvailable = -11,
	}

	/// <summary>Enumerates local authentication credential types.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LACredentialType : long {
		ApplicationPassword = 0,
		[iOS (13, 4), NoWatch, NoTV]
		[MacCatalyst (13, 1)]
		SmartCardPin = -3,
	}

	/// <summary>Enumerates access control operations for the <see cref="M:LocalAuthentication.LAContext.EvaluateAccessControl(Security.SecAccessControl,LocalAuthentication.LAAccessControlOperation,System.String,System.Action{System.Boolean,Foundation.NSError})" /> method.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LAAccessControlOperation : long {
		CreateItem,
		UseItem,
		CreateKey,
		UseKeySign,
		[MacCatalyst (13, 1)]
		UseKeyDecrypt,
		[MacCatalyst (13, 1)]
		UseKeyKeyExchange,
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoWatch, NoTV]
	[Native]
	public enum LARightState : long {
		Unknown = 0,
		Authorizing = 1,
		Authorized = 2,
		NotAuthorized = 3,
	}
}
