//
// BACompar.cs: Compatibility functions
//
// Copyright 2022 Microsoft Inc. All rights reserved.
using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using Foundation;

#nullable enable

#if !__TVOS__

namespace BackgroundAssets {

	public partial class BAAppExtensionInfo {

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual string ApplicationIdentifier => string.Empty;

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		bool DownloadSizeRestricted => false;

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual NSDate LastPeriodicCheckTime => NSDate.Now;

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual NSDate LastApplicationLaunchTime => NSDate.Now;
	}

	public partial class BADownloadManager {

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual void PerformWithExclusiveControl (NSDate date, Action<bool, NSError?> performHandler) => performHandler?.Invoke (false, null);
	}

	public partial class BADownload {

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("This property has been removed. It will always return string.Empty.")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public virtual NSError? Error => null;
	}

}

#endif // !__TVOS__
