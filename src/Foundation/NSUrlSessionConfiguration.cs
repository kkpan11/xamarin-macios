//
// NSUrlSessionHandlerConfiguration.cs:
//
// Authors:
//     Manuel de la Pena <mandel@microsoft.com>
using System;
using ObjCRuntime;
using Network;

using Foundation;

namespace Foundation {

	// the following was added to make the use of the configuration easier for the NUrlSessionHandler. 
	// Apple APIs do not give an easy way to know the type of configuration that was created, this is an 
	// issue when we want to interact with the cookie containers, since depending on the configuration type
	// the cookie container can be shared or not. This code should be transparent to the user, and is only used internaly.
	public partial class NSUrlSessionConfiguration {
		public enum SessionConfigurationType {
			Default,
			Background,
			Ephemeral,
		}

		public SessionConfigurationType SessionType { get; private set; } = SessionConfigurationType.Default;

		/// <summary>A copy of the default session configuration.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSUrlSessionConfiguration DefaultSessionConfiguration {
			get {
				var config = NSUrlSessionConfiguration._DefaultSessionConfiguration;
				config.SessionType = SessionConfigurationType.Default;
				return config;
			}
		}

		/// <summary>A session configuration that uses no persistent storage for caches, cookies, or credentials.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static NSUrlSessionConfiguration EphemeralSessionConfiguration {
			get {
				var config = NSUrlSessionConfiguration._EphemeralSessionConfiguration;
				config.SessionType = SessionConfigurationType.Ephemeral;
				return config;
			}
		}

		/// <param name="identifier">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Developers should use 'CreateBackgroundSessionConfiguration' instead.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.10", "Use 'CreateBackgroundSessionConfiguration' instead.")]
		[ObsoletedOSPlatform ("ios8.0", "Use 'CreateBackgroundSessionConfiguration' instead.")]
		public static NSUrlSessionConfiguration BackgroundSessionConfiguration (string identifier)
		{
			var config = NSUrlSessionConfiguration._BackgroundSessionConfiguration (identifier);
			config.SessionType = SessionConfigurationType.Background;
			return config;
		}

		/// <param name="identifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSUrlSessionConfiguration CreateBackgroundSessionConfiguration (string identifier)
		{
			var config = NSUrlSessionConfiguration._CreateBackgroundSessionConfiguration (identifier);
			config.SessionType = SessionConfigurationType.Background;
			return config;
		}

		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("tvos17.0")]
		public NWProxyConfig [] ProxyConfigurations {
			get => NSArray.ArrayFromHandleFunc (_ProxyConfigurations, handle => new NWProxyConfig (handle, owns: false));
			set {
				var arr = NSArray.FromNSObjects (value);
				_ProxyConfigurations = arr.Handle;
				GC.KeepAlive (arr);
			}
		}

	}
}
