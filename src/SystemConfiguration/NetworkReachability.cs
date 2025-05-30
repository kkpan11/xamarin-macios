//
// NetworkReachability.cs: NetworkReachability binding
//
// Authors:
//    Miguel de Icaza (miguel@novell.com)
//    Marek Safar (marek.safar@gmail.com)
//    Aaron Bockover (abock@xamarin.com)
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;
using System.Net;
using System.Net.Sockets;

namespace SystemConfiguration {

	// SCNetworkReachabilityFlags -> uint32_t -> SCNetworkReachability.h
	/// <summary>The reachability status.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum NetworkReachabilityFlags {
		/// <summary>The host is reachable using a transient connection (PPP for example).</summary>
		TransientConnection = 1 << 0,
		/// <summary>The host is reachable.</summary>
		Reachable = 1 << 1,
		/// <summary>Reachable, but a connection must first be established.</summary>
		ConnectionRequired = 1 << 2,
		/// <summary>Reachable, but a connection must be initiated.   The connection will be initiated on any traffic to the target detected.</summary>
		ConnectionOnTraffic = 1 << 3,
		/// <summary>The host is reachable, but it will require user interaction.</summary>
		InterventionRequired = 1 << 4,
		/// <summary>Reachable, but a connection must be initiated. The connection will be initiated if you use any of the CFSocketStream APIs, but will not be initiated automatically.</summary>
		ConnectionOnDemand = 1 << 5,
		/// <summary>The specified address is the device local name or local device.</summary>
		IsLocalAddress = 1 << 16,
		/// <summary>Connection to the host is direct, and will not go through a gateway.</summary>
		IsDirect = 1 << 17,
		/// <summary>Reachable over the cellular connection (GPRS, EDGE or 3G).</summary>
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		IsWWAN = 1 << 18,
		/// <summary>The connection will happen automatically (alias for ConnectionOnTraffic).</summary>
		ConnectionAutomatic = ConnectionOnTraffic,
	}

	// http://developer.apple.com/library/ios/#documentation/SystemConfiguration/Reference/SCNetworkReachabilityRef/Reference/reference.html
	/// <include file="../../docs/api/SystemConfiguration/NetworkReachability.xml" path="/Documentation/Docs[@DocId='T:SystemConfiguration.NetworkReachability']/*" />
	public class NetworkReachability : NativeObject {
		// netinet/in.h
		[StructLayout (LayoutKind.Sequential)]
		struct sockaddr_in {
			// We're defining fields to make the struct the correct size (expected size = 28, so 7 * 4 bytes = 28),
			// and then we're defining properties that accesses these fields to get and set field values.
			// This looks a bit convoluted, but the purpose is to avoid the runtime's built-in marshaling support,
			// so that we're able to trim away the corresponding marshalling code in the runtime to minimize app size.
			uint value1;
			uint value2;
			uint value3;
			uint value4;
			uint value5;
			uint value6;
			uint value7;

			unsafe byte sin_len {
				get {
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						return self [0];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						self [0] = value;
					}
				}
			}

			unsafe byte sin_family {
				get {
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						return self [1];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						self [1] = value;
					}
				}
			}

			unsafe short sin_port {
				get {
					fixed (sockaddr_in* myself = &this) {
						short* self = (short*) myself;
						return self [1];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						short* self = (short*) myself;
						self [1] = value;
					}
				}
			}

			unsafe int sin_addr {
				get {
					fixed (sockaddr_in* myself = &this) {
						int* self = (int*) myself;
						return self [1];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						int* self = (int*) myself;
						self [1] = value;
					}
				}
			}

			// IPv6
			unsafe uint sin6_flowinfo {
				get {
					fixed (sockaddr_in* myself = &this) {
						uint* self = (uint*) myself;
						return self [1];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						uint* self = (uint*) myself;
						self [1] = value;
					}
				}
			}

			unsafe byte [] sin6_addr8 {
				get {
					var rv = new byte [16];
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						Marshal.Copy ((IntPtr) (self + 8), rv, 0, 16);
					}
					return rv;
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						byte* self = (byte*) myself;
						Marshal.Copy (value, 0, (IntPtr) (self + 8), 16);
					}
				}
			}

			unsafe uint sin6_scope_id {
				get {
					fixed (sockaddr_in* myself = &this) {
						uint* self = (uint*) myself;
						return self [6];
					}
				}
				set {
					fixed (sockaddr_in* myself = &this) {
						uint* self = (uint*) myself;
						self [6] = value;
					}
				}
			}

			public sockaddr_in (IPAddress address)
				: this ()
			{
				sin_addr = 0;
				sin_len = 28;
				sin6_flowinfo = 0;
				sin6_scope_id = 0;
				sin6_addr8 = new byte [16];

				switch (address.AddressFamily) {
				case AddressFamily.InterNetwork:
					sin_family = 2;  // Address for IPv4
#pragma warning disable CS0618 // Type or member is obsolete
					sin_addr = (int) address.Address;
#pragma warning restore CS0618 // Type or member is obsolete
					break;
				case AddressFamily.InterNetworkV6:
					sin_family = 30; // Address for IPv6
					sin6_addr8 = address.GetAddressBytes ();
					sin6_scope_id = (uint) address.ScopeId;
					break;
				default:
					throw new NotSupportedException (address.AddressFamily.ToString ());
				}

				sin_port = 0;
			}
		}

		// SCNetworkReachability.h
		[StructLayout (LayoutKind.Sequential)]
		struct SCNetworkReachabilityContext {
			public /* CFIndex */ IntPtr version;
			public /* void* */ IntPtr info;
			public IntPtr retain;
			public IntPtr release;
			public IntPtr copyDescription;

			public SCNetworkReachabilityContext (IntPtr val)
			{
				info = val;
				version = retain = release = copyDescription = IntPtr.Zero;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static /* SCNetworkReachabilityRef __nullable */ IntPtr SCNetworkReachabilityCreateWithName (
			/* CFAllocatorRef __nullable */ IntPtr allocator, /* const char* __nonnull */ IntPtr address);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe extern static /* SCNetworkReachabilityRef __nullable */ IntPtr SCNetworkReachabilityCreateWithAddress (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* const struct sockaddr * __nonnull */ sockaddr_in* address);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe extern static /* SCNetworkReachabilityRef __nullable */ IntPtr SCNetworkReachabilityCreateWithAddressPair (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* const struct sockaddr * __nullable */ sockaddr_in* localAddress,
			/* const struct sockaddr * __nullable */ sockaddr_in* remoteAddress);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe extern static /* SCNetworkReachabilityRef __nullable */ IntPtr SCNetworkReachabilityCreateWithAddressPair (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* const struct sockaddr * __nullable */ IntPtr localAddress,
			/* const struct sockaddr * __nullable */ sockaddr_in* remoteAddress);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe extern static /* SCNetworkReachabilityRef __nullable */ IntPtr SCNetworkReachabilityCreateWithAddressPair (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* const struct sockaddr * __nullable */ sockaddr_in* localAddress,
			/* const struct sockaddr * __nullable */ IntPtr remoteAddress);

		static IntPtr CheckFailure (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw SystemConfigurationException.FromMostRecentCall ();
			return handle;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		static IntPtr Create (IPAddress ip)
		{
			if (ip is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ip));

			var s = new sockaddr_in (ip);
			unsafe {
				return CheckFailure (SCNetworkReachabilityCreateWithAddress (IntPtr.Zero, &s));
			}
		}

		/// <summary>Creates a network reachability class based on an IP address.</summary>
		/// <param name="ip">The IP address. Only IPV4 is supported.</param>
		/// <remarks>In addition to probing general hosts on the Internet, you can detect the ad-hoc WiFi network using the IP address 169.254.0.0 and the general network availability with 0.0.0.0.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public NetworkReachability (IPAddress ip)
			: base (Create (ip), true)
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		static IntPtr Create (string address)
		{
			if (address is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (address));

			using var addressStr = new TransientString (address);
			return CheckFailure (SCNetworkReachabilityCreateWithName (IntPtr.Zero, addressStr));
		}

		/// <summary>Creates a network reachability object from a hostname.</summary>
		/// <param name="address">A host name.</param>
		/// <remarks>The hostname is resolved using the current DNS settings.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public NetworkReachability (string address)
			: base (Create (address), true)
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		static IntPtr Create (IPAddress localAddress, IPAddress remoteAddress)
		{
			if (localAddress is null && remoteAddress is null)
				throw new ArgumentException ("At least one address is required");

			IntPtr handle;
			if (localAddress is null) {
				var remote = new sockaddr_in (remoteAddress);

				unsafe {
					handle = SCNetworkReachabilityCreateWithAddressPair (IntPtr.Zero, IntPtr.Zero, &remote);
				}
			} else if (remoteAddress is null) {
				var local = new sockaddr_in (localAddress);

				unsafe {
					handle = SCNetworkReachabilityCreateWithAddressPair (IntPtr.Zero, &local, IntPtr.Zero);
				}
			} else {
				var local = new sockaddr_in (localAddress);
				var remote = new sockaddr_in (remoteAddress);

				unsafe {
					handle = SCNetworkReachabilityCreateWithAddressPair (IntPtr.Zero, &local, &remote);
				}
			}

			return CheckFailure (handle);
		}

		/// <summary>Creates a network reachability object from a local IP address and a remote one.</summary>
		/// <param name="localAddress">Local address to monitor, this can be null if you are not interested in the local changes.</param>
		/// <param name="remoteAddress">Remote address to monitor, this can be null if you are not interested in the remote changes.</param>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public NetworkReachability (IPAddress localAddress, IPAddress remoteAddress)
			: base (Create (localAddress, remoteAddress), true)
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe static extern int SCNetworkReachabilityGetFlags (/* SCNetworkReachabilityRef __nonnull */ IntPtr target,
			/* SCNetworkReachabilityFlags* __nonnull */ NetworkReachabilityFlags* flags);

		/// <summary>Method used to get the current reachability flags for this host.</summary>
		/// <param name="flags">Returned value of the current reachability for the specified host.</param>
		/// <returns>If flags were successfully fetched.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool TryGetFlags (out NetworkReachabilityFlags flags)
		{
			return GetFlags (out flags) == StatusCode.OK;
		}

		/// <summary>Method used to get the current reachability flags for this host.</summary>
		/// <param name="flags">Returned value of the current reachability for the specified host.</param>
		/// <returns>Returned value of the current reachability for the specified host.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public StatusCode GetFlags (out NetworkReachabilityFlags flags)
		{
			flags = default;
			int rv;
			unsafe {
				rv = SCNetworkReachabilityGetFlags (Handle, (NetworkReachabilityFlags*) Unsafe.AsPointer<NetworkReachabilityFlags> (ref flags));
			}
			return rv == 0 ? StatusCodeError.SCError () : StatusCode.OK;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		unsafe static extern /* Boolean */ byte SCNetworkReachabilitySetCallback (
			/* SCNetworkReachabilityRef __nonnull */ IntPtr handle,
			/* __nullable SCNetworkReachabilityCallBack */ delegate* unmanaged<IntPtr, NetworkReachabilityFlags, IntPtr, void> callout,
			/* __nullable */ SCNetworkReachabilityContext* context);

		/// <summary>Signature for the <see cref="SetNotification" /> method on NetworkReachability.</summary>
		/// <param name="flags">The current reachability flags for the NetworkReachability object.</param>
		/// <remarks>Methods with this signature are invoked in response to changes in the <see cref="NetworkReachability" /> state.</remarks>
		public delegate void Notification (NetworkReachabilityFlags flags);

		Notification? notification;
		GCHandle gch;

		[UnmanagedCallersOnly]
		static void Callback (IntPtr handle, NetworkReachabilityFlags flags, IntPtr info)
		{
			GCHandle gch = GCHandle.FromIntPtr (info);
			var r = gch.Target as NetworkReachability;
			if (r?.notification is null)
				return;
			r.notification (flags);
		}

		/// <summary>Configures the method to be invoked when network reachability changes.</summary>
		/// <param name="callback">The method to invoke on a network reachability change. Pass <see langword="null" /> to disable.</param>
		/// <returns>True if the operation succeeded, false otherwise.</returns>
		/// <remarks>The notification is invoked on either the runloop configured in the call to <see cref="Schedule(CoreFoundation.CFRunLoop,System.String)" />, or dispatched on the queue specified with <see cref="SetDispatchQueue(CoreFoundation.DispatchQueue)" />.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public StatusCode SetNotification (Notification? callback)
		{
			bool rv;
			if (notification is null) {
				if (callback is null)
					return StatusCode.OK;

				gch = GCHandle.Alloc (this);
				var ctx = new SCNetworkReachabilityContext (GCHandle.ToIntPtr (gch));

				unsafe {
					rv = SCNetworkReachabilitySetCallback (Handle, &Callback, &ctx) != 0;
				}
			} else {
				if (callback is null) {
					this.notification = null;
					unsafe {
						rv = SCNetworkReachabilitySetCallback (Handle, null, null) != 0;
					}
					if (!rv)
						return StatusCodeError.SCError ();

					return StatusCode.OK;
				}
			}

			this.notification = callback;
			return StatusCode.OK;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static /* Boolean */ byte SCNetworkReachabilityScheduleWithRunLoop (
			/* SCNetworkReachabilityRef __nonnull */ IntPtr target, /* CFRunLoopRef __nonnull */ IntPtr runloop,
			/* CFStringRef __nonnull */ IntPtr runLoopMode);

		/// <summary>Schedules the delivery of the events (what is set with SetCallback) on the given run loop.</summary>
		/// <param name="runLoop">The run loop where the reachability callback is invoked.</param>
		/// <param name="mode">The run loop mode.</param>
		/// <returns>True if the operation succeeded, false otherwise.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool Schedule (CFRunLoop runLoop, string mode)
		{
			if (runLoop is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (runLoop));

			if (mode is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (mode));

			using var modeHandle = new TransientCFString (mode);
			bool result = SCNetworkReachabilityScheduleWithRunLoop (Handle, runLoop.GetCheckedHandle (), modeHandle) != 0;
			GC.KeepAlive (runLoop);
			return result;
		}

		/// <summary>Schedules the delivery of the events (what is set with SetCallback) on the current loop.</summary>
		/// <returns>True if the operation succeeded, false otherwise.</returns>
		/// <remarks>This schedules using the <see cref="CoreFoundation.CFRunLoop.Current" /> and the <see cref="CoreFoundation.CFRunLoop.ModeDefault" />.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool Schedule ()
		{
			return Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static int SCNetworkReachabilityUnscheduleFromRunLoop (/* SCNetworkReachabilityRef */ IntPtr target, /* CFRunLoopRef */ IntPtr runloop, /* CFStringRef */ IntPtr runLoopMode);

		/// <summary>Removes the NetworkRechability from the given run loop.</summary>
		/// <param name="runLoop">The run loop where the object is currently scheduled.</param>
		/// <param name="mode">The mode used.</param>
		/// <returns>True on success.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool Unschedule (CFRunLoop runLoop, string mode)
		{
			if (runLoop is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (runLoop));

			if (mode is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (mode));

			using var modeHandle = new TransientCFString (mode);
			bool result = SCNetworkReachabilityUnscheduleFromRunLoop (Handle, runLoop.GetCheckedHandle (), modeHandle) != 0;
			GC.KeepAlive (runLoop);
			return result;
		}

		/// <summary>Removes the NetworkRechability from the current run loop.</summary>
		/// <returns>True if the operation succeeded, false otherwise.</returns>
		/// <remarks>This unschedules the notifications from the <see cref="CoreFoundation.CFRunLoop.Current" /> and the <see cref="CoreFoundation.CFRunLoop.ModeDefault" />.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool Unschedule ()
		{
			return Unschedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[DllImport (Constants.SystemConfigurationLibrary)]
		extern static /* Boolean */ byte SCNetworkReachabilitySetDispatchQueue (
			/* SCNetworkReachabilityRef __nonnull */ IntPtr target,
			/* dispatch_queue_t __nullable */ IntPtr queue);

		/// <summary>Specifies the <see cref="CoreFoundation.DispatchQueue" /> to be used for callbacks.</summary>
		/// <param name="queue">The queue on which the notification will be posted. Pass <see langword="null" /> to disable notifications on the specified queue.</param>
		/// <returns>True on success, false on failure.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("maccatalyst17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("tvos17.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		[ObsoletedOSPlatform ("macos14.4", "Use 'NSUrlSession' or 'NWConnection' instead.")]
		public bool SetDispatchQueue (DispatchQueue? queue)
		{
			bool result = SCNetworkReachabilitySetDispatchQueue (GetCheckedHandle (), queue.GetHandle ()) != 0;
			GC.KeepAlive (queue);
			return result;
		}
	}
}
