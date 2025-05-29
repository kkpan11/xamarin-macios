//
// NEVpnManager.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

#if MONOMAC
using System;
using Foundation;
using ObjCRuntime;
using Security;

namespace NetworkExtension {
	/// <summary>Manages and controls VPN configurations and connections.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkExtension/Reference/NEVPNManagerClassRef/index.html">Apple documentation for <c>NEVPNManager</c></related>
	public partial class NEVpnManager {
		/// <param name="authorization">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public void SetAuthorization (Authorization authorization)
		{
			if (authorization is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (authorization));

			_SetAuthorization (authorization.Handle);
			GC.KeepAlive (authorization);
		}
	}
}
#endif
