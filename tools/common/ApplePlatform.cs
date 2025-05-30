//
// ApplePlatform.cs
//
// Copyright 2020 Microsoft Corp. All Rights Reserved.

#nullable enable

namespace Xamarin.Utils {
	public enum ApplePlatform {
		None,
		MacOSX,
		iOS,
		[System.Obsolete ("Do not use")]
		WatchOS,
		TVOS,
		MacCatalyst,
	}

	public static class ApplePlatformExtensions {
		public static string AsString (this ApplePlatform @this)
		{
			switch (@this) {
			case ApplePlatform.iOS:
				return "iOS";
			case ApplePlatform.MacOSX:
				return "macOS";
			case ApplePlatform.TVOS:
				return "tvOS";
			case ApplePlatform.MacCatalyst:
				return "MacCatalyst";
			case ApplePlatform.None:
				return "None";
			default:
				return "Unknown";
			}
		}

		public static string ToFramework (this ApplePlatform @this, string? netVersion = null)
		{
			if (netVersion is null)
				netVersion = Xamarin.DotNetVersions.Tfm;

			switch (@this) {
			case ApplePlatform.iOS:
				return netVersion + "-ios";
			case ApplePlatform.MacOSX:
				return netVersion + "-macos";
			case ApplePlatform.TVOS:
				return netVersion + "-tvos";
			case ApplePlatform.MacCatalyst:
				return netVersion + "-maccatalyst";
			default:
				return "Unknown";
			}
		}

		public static ApplePlatform Parse (string platform)
		{
			switch (platform.ToLowerInvariant ()) {
			case "ios":
				return ApplePlatform.iOS;
			case "tvos":
				return ApplePlatform.TVOS;
			case "macos":
				return ApplePlatform.MacOSX;
			case "maccatalyst":
				return ApplePlatform.MacCatalyst;
			default:
				throw new System.InvalidOperationException ($"Unknown platform: {platform}");
			}
		}

		public static bool IsDesktop (this ApplePlatform @this)
		{
			switch (@this) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				return false;
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				return true;
			default:
				throw new System.InvalidOperationException ($"Unknown platform: {@this}");
			}
		}
	}
}
