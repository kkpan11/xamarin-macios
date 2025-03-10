// Copyright 2014 Xamarin Inc. All rights reserved.

using System;
using ObjCRuntime;

namespace Security {

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates SSL protocols.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsProtocolVersion' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'TlsProtocolVersion' instead.")]
	public enum SslProtocol {
		Unknown = 0,
		Ssl_3_0 = 2,
		Tls_1_0 = 4,
		Tls_1_1 = 7,
		Tls_1_2 = 8,
		Dtls_1_0 = 9,
		[MacCatalyst (13, 1)]
		Tls_1_3 = 10,
		Dtls_1_2 = 11,

		/* Obsolete on iOS */
		Ssl_2_0 = 1,
		Ssl_3_0_only = 3,
		Tls_1_0_only = 5,
		All = 6,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_protocol_version_t)
	[NativeName ("tls_protocol_version_t")]
	public enum TlsProtocolVersion : ushort {
		Tls10 = 769,
		Tls11 = 770,
		Tls12 = 771,
		Tls13 = 772,
		Dtls10 = 65279,
		Dtls12 = 65277,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_ciphersuite_t)
	[NativeName ("tls_ciphersuite_t")]
	public enum TlsCipherSuite : ushort {
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		RsaWith3desEdeCbcSha = 10,
		RsaWithAes128CbcSha = 47,
		RsaWithAes256CbcSha = 53,
		RsaWithAes128GcmSha256 = 156,
		RsaWithAes256GcmSha384 = 157,
		RsaWithAes128CbcSha256 = 60,
		RsaWithAes256CbcSha256 = 61,
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		EcdheEcdsaWith3desEdeCbcSha = 49160,
		EcdheEcdsaWithAes128CbcSha = 49161,
		EcdheEcdsaWithAes256CbcSha = 49162,
		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		EcdheRsaWith3desEdeCbcSha = 49170,
		EcdheRsaWithAes128CbcSha = 49171,
		EcdheRsaWithAes256CbcSha = 49172,
		EcdheEcdsaWithAes128CbcSha256 = 49187,
		EcdheEcdsaWithAes256CbcSha384 = 49188,
		EcdheRsaWithAes128CbcSha256 = 49191,
		EcdheRsaWithAes256CbcSha384 = 49192,
		EcdheEcdsaWithAes128GcmSha256 = 49195,
		EcdheEcdsaWithAes256GcmSha384 = 49196,
		EcdheRsaWithAes128GcmSha256 = 49199,
		EcdheRsaWithAes256GcmSha384 = 49200,
		EcdheRsaWithChacha20Poly1305Sha256 = 52392,
		EcdheEcdsaWithChacha20Poly1305Sha256 = 52393,
		Aes128GcmSha256 = 4865,
		Aes256GcmSha384 = 4866,
		Chacha20Poly1305Sha256 = 4867,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	// CF_ENUM(uint16_t, tls_ciphersuite_group_t)
	[NativeName ("tls_ciphersuite_group_t")]
	public enum TlsCipherSuiteGroup : ushort {
		Default,
		Compatibility,
		Legacy,
		Ats,
		AtsCompatibility,
	}

	// subset of OSStatus (int)
	/// <summary>Enumerates SSL connection status.</summary>
	public enum SslStatus {
		Success = 0,        // errSecSuccess in SecBase.h
		Protocol = -9800,
		Negotiation = -9801,
		FatalAlert = -9802,
		WouldBlock = -9803,
		SessionNotFound = -9804,
		ClosedGraceful = -9805,
		ClosedAbort = -9806,
		XCertChainInvalid = -9807,
		BadCert = -9808,
		Crypto = -9809,
		Internal = -9810,
		ModuleAttach = -9811,
		UnknownRootCert = -9812,
		NoRootCert = -9813,
		CertExpired = -9814,
		CertNotYetValid = -9815,
		ClosedNotNotified = -9816,
		BufferOverflow = -9817,
		BadCipherSuite = -9818,
		PeerUnexpectedMsg = -9819,
		PeerBadRecordMac = -9820,
		PeerDecryptionFail = -9821,
		PeerRecordOverflow = -9822,
		PeerDecompressFail = -9823,
		PeerHandshakeFail = -9824,
		PeerBadCert = -9825,
		PeerUnsupportedCert = -9826,
		PeerCertRevoked = -9827,
		PeerCertExpired = -9828,
		PeerCertUnknown = -9829,
		IllegalParam = -9830,
		PeerUnknownCA = -9831,
		PeerAccessDenied = -9832,
		PeerDecodeError = -9833,
		PeerDecryptError = -9834,
		PeerExportRestriction = -9835,
		PeerProtocolVersion = -9836,
		PeerInsufficientSecurity = -9837,
		PeerInternalError = -9838,
		PeerUserCancelled = -9839,
		PeerNoRenegotiation = -9840,
		PeerAuthCompleted = -9841, // non fatal
		PeerClientCertRequested = -9842, // non fatal
		HostNameMismatch = -9843,
		ConnectionRefused = -9844,
		DecryptionFail = -9845,
		BadRecordMac = -9846,
		RecordOverflow = -9847,
		BadConfiguration = -9848,
		UnexpectedRecord = -9849,
		SSLWeakPeerEphemeralDHKey = -9850,
		SSLClientHelloReceived = -9851, // non falta
		SSLTransportReset = -9852,
		SSLNetworkTimeout = -9853,
		SSLConfigurationFailed = -9854,
		SSLUnsupportedExtension = -9855,
		SSLUnexpectedMessage = -9856,
		SSLDecompressFail = -9857,
		SSLHandshakeFail = -9858,
		SSLDecodeError = -9859,
		SSLInappropriateFallback = -9860,
		SSLMissingExtension = -9861,
		SSLBadCertificateStatusResponse = -9862,
		SSLCertificateRequired = -9863,
		SSLUnknownPskIdentity = -9864,
		SSLUnrecognizedName = -9865,

		// xcode 11
		SslAtsViolation = -9880,
		SslAtsMinimumVersionViolation = -9881,
		SslAtsCiphersuiteViolation = -9882,
		SslAtsMinimumKeySizeViolation = -9883,
		SslAtsLeafCertificateHashAlgorithmViolation = -9884,
		SslAtsCertificateHashAlgorithmViolation = -9885,
		SslAtsCertificateTrustViolation = -9886,
		// xcode 12
		SslEarlyDataRejected = -9890,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates SSL session behavior options.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionOption {
		BreakOnServerAuth,
		BreakOnCertRequested,
		BreakOnClientAuth,

		[MacCatalyst (13, 1)]
		FalseStart,

		SendOneByteRecord,

		[MacCatalyst (13, 1)]
		AllowServerIdentityChange = 5,

		[MacCatalyst (13, 1)]
		Fallback = 6,

		[MacCatalyst (13, 1)]
		BreakOnClientHello = 7,

		[MacCatalyst (13, 1)]
		AllowRenegotiation = 8,

		[MacCatalyst (13, 1)]
		EnableSessionTickets = 9,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates values that control when to use SSL.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslAuthenticate {
		Never,
		Always,
		Try,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates values that indicate whether a server side or client side <see cref="T:Security.SslContext" /> should be created.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslProtocolSide {
		Server,
		Client,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates types of SSL connections.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslConnectionType {
		Stream,
		Datagram,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates stages in the SSL session life cycle.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionState {
		Invalid = -1,
		Idle,
		Handshake,
		Connected,
		Closed,
		Aborted,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslSessionStrengthPolicy {
		Default,
		ATSv1,
		ATSv1NoPFS,
	}

	// Security.framework/Headers/SecureTransport.h
	// untyped enum
	/// <summary>Enumerates stages in an SSL client certificate exchange.</summary>
	[Deprecated (PlatformName.MacOSX, 10, 15, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.iOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 13, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: Constants.UseNetworkInstead)]
	public enum SslClientCertificateState {
		None,
		Requested,
		Sent,
		Rejected,
	}

#if !NET
	// Security.framework/Headers/CipherSuite.h
	// 32 bits (uint32_t) on OSX, 16 bits (uint16_t) on iOS
	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsCipherSuite' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsCipherSuite' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsCipherSuite' instead.")]
#if MONOMAC || __MACCATALYST__
	public enum SslCipherSuite : uint {
#else
	public enum SslCipherSuite : ushort {
#endif
		// DO NOT RENAME VALUES - they don't look good but we need them to keep compatibility with our System.dll code
		// it's how it's defined across most SSL/TLS implementation (from RFC)

		SSL_NULL_WITH_NULL_NULL = 0x0000,   // value used before (not after) negotiation
		TLS_NULL_WITH_NULL_NULL = 0x0000,

		// Not the whole list (too much unneeed metadata) but only what's supported
		// FIXME needs to be expended with OSX 10.9

		SSL_RSA_WITH_NULL_MD5 = 0x0001,
		SSL_RSA_WITH_NULL_SHA = 0x0002,
		SSL_RSA_EXPORT_WITH_RC4_40_MD5 = 0x0003,    // iOS 5.1 only
		SSL_RSA_WITH_RC4_128_MD5 = 0x0004,
		SSL_RSA_WITH_RC4_128_SHA = 0x0005,
		SSL_RSA_WITH_3DES_EDE_CBC_SHA = 0x000A,
		SSL_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 0x0016,
		SSL_DH_anon_EXPORT_WITH_RC4_40_MD5 = 0x0017,    // iOS 5.1 only
		SSL_DH_anon_WITH_RC4_128_MD5 = 0x0018,
		SSL_DH_anon_WITH_3DES_EDE_CBC_SHA = 0x001B,

		// TLS - identical values to SSL (above)

		TLS_RSA_WITH_NULL_MD5 = 0x0001,
		TLS_RSA_WITH_NULL_SHA = 0x0002,
		TLS_RSA_WITH_RC4_128_MD5 = 0x0004,
		TLS_RSA_WITH_RC4_128_SHA = 0x0005,
		TLS_RSA_WITH_3DES_EDE_CBC_SHA = 0x000A,
		TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 0x0016,
		TLS_DH_anon_WITH_RC4_128_MD5 = 0x0018,
		TLS_DH_anon_WITH_3DES_EDE_CBC_SHA = 0x001B,

		// TLS specific

		TLS_PSK_WITH_NULL_SHA = 0x002C,
		TLS_RSA_WITH_AES_128_CBC_SHA = 0x002F,
		TLS_DHE_RSA_WITH_AES_128_CBC_SHA = 0x0033,
		TLS_DH_anon_WITH_AES_128_CBC_SHA = 0x0034,
		TLS_RSA_WITH_AES_256_CBC_SHA = 0x0035,
		TLS_DHE_RSA_WITH_AES_256_CBC_SHA = 0x0039,
		TLS_DH_anon_WITH_AES_256_CBC_SHA = 0x003A,
		TLS_RSA_WITH_NULL_SHA256 = 0x003B,
		TLS_RSA_WITH_AES_128_CBC_SHA256 = 0x003C,
		TLS_RSA_WITH_AES_256_CBC_SHA256 = 0x003D,
		TLS_DHE_RSA_WITH_AES_128_CBC_SHA256 = 0x0067,
		TLS_DHE_RSA_WITH_AES_256_CBC_SHA256 = 0x006B,
		TLS_DH_anon_WITH_AES_128_CBC_SHA256 = 0x006C,
		TLS_DH_anon_WITH_AES_256_CBC_SHA256 = 0x006D,
		TLS_PSK_WITH_RC4_128_SHA = 0x008A,
		TLS_PSK_WITH_3DES_EDE_CBC_SHA = 0x008B,
		TLS_PSK_WITH_AES_128_CBC_SHA = 0x008C,
		TLS_PSK_WITH_AES_256_CBC_SHA = 0x008D,

		TLS_RSA_WITH_AES_128_GCM_SHA256 = 0x009C,   // iOS 9+
		TLS_RSA_WITH_AES_256_GCM_SHA384 = 0x009D,   // iOS 9+
		TLS_DHE_RSA_WITH_AES_128_GCM_SHA256 = 0x009E,   // iOS 9+
		TLS_DHE_RSA_WITH_AES_256_GCM_SHA384 = 0x009F,   // iOS 9+

		TLS_DH_DSS_WITH_AES_256_GCM_SHA384 = 0x00A5,
		TLS_DH_anon_WITH_AES_128_GCM_SHA256 = 0x00A6,   // iOS 5.1 only
		TLS_DH_anon_WITH_AES_256_GCM_SHA384 = 0x00A7,   // iOS 5.1 only

		/* RFC 5487 - PSK with SHA-256/384 and AES GCM */
		TLS_PSK_WITH_AES_128_GCM_SHA256 = 0x00A8,
		TLS_PSK_WITH_AES_256_GCM_SHA384 = 0x00A9,
		TLS_DHE_PSK_WITH_AES_128_GCM_SHA256 = 0x00AA,
		TLS_DHE_PSK_WITH_AES_256_GCM_SHA384 = 0x00AB,
		TLS_RSA_PSK_WITH_AES_128_GCM_SHA256 = 0x00AC,
		TLS_RSA_PSK_WITH_AES_256_GCM_SHA384 = 0x00AD,

		TLS_PSK_WITH_AES_128_CBC_SHA256 = 0x00AE,
		TLS_PSK_WITH_AES_256_CBC_SHA384 = 0x00AF,
		TLS_PSK_WITH_NULL_SHA256 = 0x00B0,
		TLS_PSK_WITH_NULL_SHA384 = 0x00B1,

		TLS_DHE_PSK_WITH_AES_128_CBC_SHA256 = 0x00B2,
		TLS_DHE_PSK_WITH_AES_256_CBC_SHA384 = 0x00B3,
		TLS_DHE_PSK_WITH_NULL_SHA256 = 0x00B4,
		TLS_DHE_PSK_WITH_NULL_SHA384 = 0x00B5,

		TLS_RSA_PSK_WITH_AES_128_CBC_SHA256 = 0x00B6,
		TLS_RSA_PSK_WITH_AES_256_CBC_SHA384 = 0x00B7,
		TLS_RSA_PSK_WITH_NULL_SHA256 = 0x00B8,
		TLS_RSA_PSK_WITH_NULL_SHA384 = 0x00B9,

		TLS_ECDH_ECDSA_WITH_NULL_SHA = 0xC001,
		TLS_ECDH_ECDSA_WITH_RC4_128_SHA = 0xC002,
		TLS_ECDH_ECDSA_WITH_3DES_EDE_CBC_SHA = 0xC003,
		TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA = 0xC004,
		TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA = 0xC005,
		TLS_ECDHE_ECDSA_WITH_NULL_SHA = 0xC006,
		TLS_ECDHE_ECDSA_WITH_RC4_128_SHA = 0xC007,
		TLS_ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA = 0xC008,
		TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA = 0xC009,
		TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA = 0xC00A,
		TLS_ECDH_RSA_WITH_NULL_SHA = 0xC00B,
		TLS_ECDH_RSA_WITH_RC4_128_SHA = 0xC00C,
		TLS_ECDH_RSA_WITH_3DES_EDE_CBC_SHA = 0xC00D,
		TLS_ECDH_RSA_WITH_AES_128_CBC_SHA = 0xC00E,
		TLS_ECDH_RSA_WITH_AES_256_CBC_SHA = 0xC00F,
		TLS_ECDHE_RSA_WITH_NULL_SHA = 0xC010,
		TLS_ECDHE_RSA_WITH_RC4_128_SHA = 0xC011,
		TLS_ECDHE_RSA_WITH_3DES_EDE_CBC_SHA = 0xC012,
		TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA = 0xC013,
		TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA = 0xC014,

		TLS_ECDH_anon_WITH_NULL_SHA = 0xC015,
		TLS_ECDH_anon_WITH_RC4_128_SHA = 0xC016,
		TLS_ECDH_anon_WITH_3DES_EDE_CBC_SHA = 0xC017,
		TLS_ECDH_anon_WITH_AES_128_CBC_SHA = 0xC018,
		TLS_ECDH_anon_WITH_AES_256_CBC_SHA = 0xC019,

		TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 = 0xC023,
		TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 = 0xC024,
		TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256 = 0xC025,
		TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384 = 0xC026,
		TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256 = 0xC027,
		TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384 = 0xC028,
		TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256 = 0xC029,
		TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384 = 0xC02A,

		TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 = 0xC02B,   // iOS 9+
		TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 = 0xC02C,   // iOS 9+
		TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256 = 0xC02D,    // iOS 9+
		TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384 = 0xC02E,    // iOS 9+
		TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256 = 0xC02F, // iOS 9+
		TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384 = 0xC030, // iOS 9+
		TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256 = 0xC031,  // iOS 9+
		TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384 = 0xC032,  // iOS 9+

		// rfc 5489
		TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA = 0xC035,
		TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA = 0xC036,

		// https://tools.ietf.org/html/rfc7905
		TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 0xCCA8,   // Xcode 9+
		TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 = 0xCCA9, // Xcode 9+

		// rfc 7905
		TLS_PSK_WITH_CHACHA20_POLY1305_SHA256 = 0xCCAB,

		// https://tools.ietf.org/html/rfc5746 secure renegotiation
		TLS_EMPTY_RENEGOTIATION_INFO_SCSV = 0x00FF,

		/* TLS 1.3 */
		TLS_AES_128_GCM_SHA256 = 0x1301,    // iOS 11+
		TLS_AES_256_GCM_SHA384 = 0x1302,    // iOS 11+
		TLS_CHACHA20_POLY1305_SHA256 = 0x1303,  // iOS 11+
		TLS_AES_128_CCM_SHA256 = 0x1304,    // iOS 11+
		TLS_AES_128_CCM_8_SHA256 = 0x1305,  // iOS 11+

		SSL_RSA_WITH_RC2_CBC_MD5 = 0xFF80,
		SSL_RSA_WITH_IDEA_CBC_MD5 = 0xFF81,
		SSL_RSA_WITH_DES_CBC_MD5 = 0xFF82,
		SSL_RSA_WITH_3DES_EDE_CBC_MD5 = 0xFF83,
		SSL_NO_SUCH_CIPHERSUITE = 0xFFFF,

	}
#endif // !NET

	[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'TlsCipherSuiteGroup' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'TlsCipherSuiteGroup' instead.")]
	// typedef CF_ENUM(int, SSLCiphersuiteGroup)
	public enum SslCipherSuiteGroup {
		Default,
		Compatibility,
		Legacy,
		Ats,
		AtsCompatibility,
	}
}
