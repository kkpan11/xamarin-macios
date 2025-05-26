using System;
using CoreMedia;
using Foundation;
using ObjCRuntime;

#nullable enable

#if !__MACCATALYST__

namespace Cinematic {
	[SupportedOSPlatform ("tvos17.0")]
	[SupportedOSPlatform ("macos14.0")]
	[SupportedOSPlatform ("ios17.0")]
	[SupportedOSPlatform ("maccatalyst17.0")]
	public enum CNDecisionIdentifierType {
		Single,
		Group,
	}

	public partial class CNDecision {

		public CNDecision (CMTime time, long detectionId, bool isStrong, CNDecisionIdentifierType identifierType)
			: base (NSObjectFlag.Empty)
		{

			switch (identifierType) {
			case CNDecisionIdentifierType.Single:
				InitializeHandle (_InitWithSingleIdentifier (time, detectionId, isStrong), "initWithTime:detectionID:strong:");
				break;
			case CNDecisionIdentifierType.Group:
				InitializeHandle (_InitWithGroupIdentifier (time, detectionId, isStrong), "initWithTime:detectionGroupID:strong:");
				break;
			default:
				ObjCRuntime.ThrowHelper.ThrowArgumentOutOfRangeException (nameof (identifierType), $"Unknown identifier type: {identifierType}");
				break;
			}
		}
	}
}
#endif
