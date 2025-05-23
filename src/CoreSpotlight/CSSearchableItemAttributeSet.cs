// Copyright 2015 Xamarin Inc. All rights reserved.

#nullable enable

#if IOS || MONOMAC

using System;
using Foundation;
using ObjCRuntime;

namespace CoreSpotlight {

	public partial class CSSearchableItemAttributeSet {

		public INSSecureCoding? this [CSCustomAttributeKey key] {
			get {
				return ValueForCustomKey (key);
			}
			set {
				SetValue (value, key);
			}
		}

		// Manually deal with these properties until we get BindAs working
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public bool? IsUserCreated {
			get {
				return _IsUserCreated?.BoolValue;
			}
			set {
				_IsUserCreated = value.HasValue ? new NSNumber (value.Value) : null;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public bool? IsUserOwned {
			get {
				return _IsUserOwned?.BoolValue;
			}
			set {
				_IsUserOwned = value.HasValue ? new NSNumber (value.Value) : null;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public bool? IsUserCurated {
			get {
				return _IsUserCurated?.BoolValue;
			}
			set {
				_IsUserCurated = value.HasValue ? new NSNumber (value.Value) : null;
			}
		}
	}
}

#endif
