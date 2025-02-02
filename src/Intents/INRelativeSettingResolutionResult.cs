//
// INRelativeSettingResolutionResult.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#if !MONOMAC && !TVOS
using System;
using Foundation;
using ObjCRuntime;

namespace Intents {
	public partial class INRelativeSettingResolutionResult {

		public static INRelativeSettingResolutionResult GetSuccess (INRelativeSetting resolvedValue)
		{
#if __IOS__
			if (SystemVersion.CheckiOS (11, 0))
				return SuccessWithResolvedRelativeSetting (resolvedValue);
			else
				return SuccessWithResolvedValue (resolvedValue);
#endif
		}

		public static INRelativeSettingResolutionResult GetConfirmationRequired (INRelativeSetting valueToConfirm)
		{
#if __IOS__
			if (SystemVersion.CheckiOS (11, 0))
				return ConfirmationRequiredWithRelativeSettingToConfirm (valueToConfirm);
			else
				return ConfirmationRequiredWithValueToConfirm (valueToConfirm);
#endif
		}
	}
}
#endif
