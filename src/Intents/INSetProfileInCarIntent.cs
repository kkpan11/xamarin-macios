#if IOS
using System;
using Foundation;
using Intents;
using ObjCRuntime;
using UIKit;

#nullable enable

namespace Intents {

	public partial class INSetProfileInCarIntent {
		/// <param name="profileNumber">To be added.</param>
		///         <param name="profileLabel">To be added.</param>
		///         <param name="defaultProfile">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios12.0", "Use the overload that takes 'INSpeakableString carName'.")]
		[ObsoletedOSPlatform ("maccatalyst13.1", "Use the overload that takes 'INSpeakableString carName'.")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public INSetProfileInCarIntent (NSNumber profileNumber, string profileLabel, bool? defaultProfile) :
			this (profileNumber, profileLabel, defaultProfile.HasValue ? new NSNumber (defaultProfile.Value) : null)
		{
		}
	}
}

#endif
