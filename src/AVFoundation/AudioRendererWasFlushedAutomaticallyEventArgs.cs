using Foundation;
using CoreMedia;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {
	/// <summary>Provides data for the  event.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public partial class AudioRendererWasFlushedAutomaticallyEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime AudioRendererFlushTime {
			get {
				return _AudioRendererFlushTime.CMTimeValue;
			}
		}
	}
}
