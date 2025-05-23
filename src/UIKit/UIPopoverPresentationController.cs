// Copyright 2014 Xamarin Inc. All rights reserved.

#if IOS

using System;
using CoreGraphics;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public partial class UIPopoverPresentationController {

		// cute helper to avoid using `Class` in the public API
		/// <summary>Gets or sets the type that is used to display background content for the popover.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public virtual Type PopoverBackgroundViewType {
			get {
				IntPtr p = PopoverBackgroundViewClass;
				if (p == IntPtr.Zero)
					return null;
				return Class.Lookup (p);
			}
			set {
				PopoverBackgroundViewClass = (value is null) ? IntPtr.Zero :
					Class.GetHandle (value);
			}
		}
	}
}

#endif // IOS
