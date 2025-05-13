//
// UIViewControllerTransitionCoordinatorContext.cs: Helper methods to make the class more usable
//
// Authors: miguel de icaza
//
// Copyright 2014 Xamarin
//

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	/// <summary>Extension class that, together with the <see cref="UIKit.IUIViewControllerTransitionCoordinatorContext" /> interface, comprise the UIViewControllerTransitionCoordinatorContext protocol.</summary>
	///     <remarks>To be added.</remarks>
	public static partial class UIViewControllerTransitionCoordinatorContext_Extensions {
		/// <summary>Gets a view controller that controls a transition.</summary>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		public static UIView GetTransitionViewController (this IUIViewControllerTransitionCoordinatorContext This, UITransitionViewControllerKind kind)
		{
			switch (kind) {
			case UITransitionViewControllerKind.ToView:
				return This.GetTransitionViewControllerForKey (UITransitionContext.ToViewKey);
			case UITransitionViewControllerKind.FromView:
				return This.GetTransitionViewControllerForKey (UITransitionContext.FromViewKey);
			default:
				return null;
			}
		}
	}
}
