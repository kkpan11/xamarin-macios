//
// Compat.cs: Stuff we won't provide in Xamarin.iOS.dll or newer XAMCORE_* profiles
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013, 2015 Xamarin, Inc.
// Copyright 2019 Microsoft Corporation
//

using System;
using System.ComponentModel;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace UIKit {
#if !NET
	public partial class UIPresentationController {

		[Obsolete ("Removed in iOS10. Use '.ctor (UIViewController,UIViewController)'.")]
		public UIPresentationController ()
		{
		}
	}

#if !TVOS
	public partial class UIPreviewInteraction {
		[Obsolete ("Use overload accepting a 'IUICoordinateSpace'.")]
		public virtual CGPoint GetLocationInCoordinateSpace (UICoordinateSpace coordinateSpace)
		{
			return GetLocationInCoordinateSpace ((IUICoordinateSpace) coordinateSpace);
		}
	}

	public partial class UIMarkupTextPrintFormatter {

		[Obsolete ("Use '.ctor(string)' instead.")]
		public UIMarkupTextPrintFormatter ()
		{
		}
	}
#endif

#endif

#if !NET
	public partial class UICollectionViewFocusUpdateContext {
		[Obsolete ("This cannot be directly created.")]
		public UICollectionViewFocusUpdateContext () { }
	}

	public partial class UIFocusUpdateContext {
		[Obsolete ("This cannot be directly created.")]
		public UIFocusUpdateContext () { }
	}

	public partial class NSLayoutManager {

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public void AddTemporaryAttribute (NSString attributeName, NSObject value, NSRange characterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual void AddTemporaryAttribute (string attributeName, NSObject value, NSRange characterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual void AddTemporaryAttributes (NSDictionary<NSString, NSObject> attributes, NSRange characterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual nfloat GetDefaultBaselineOffset (UIFont font)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual nfloat GetDefaultLineHeight (UIFont font)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		protected virtual NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, IntPtr effectiveRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, out NSRange effectiveRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		protected virtual NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, IntPtr longestEffectiveRange, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, out NSRange longestEffectiveRange, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		protected virtual NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, IntPtr effectiveCharacterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, out NSRange effectiveCharacterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		protected virtual NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, IntPtr longestEffectiveRange, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, out NSRange longestEffectiveRange, NSRange rangeLimit)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual void RemoveTemporaryAttribute (NSString attributeName, NSRange characterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public void RemoveTemporaryAttribute (string attributeName, NSRange characterRange)
		{
			throw new NotSupportedException ();
		}

		[Obsolete ("Always throw a 'NotSupportedException' (only available on macOS).")]
		public virtual void TextContainerChangedTextView (NSTextContainer container)
		{
			throw new NotSupportedException ();
		}
	}
#endif

#if !XAMCORE_5_0 && IOS
	public partial class UIDocViewController {
		[Obsolete ("Do not use; this constructor doesn't work.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public UIDocViewController ()
			: base (ThrowInvalidOperationException ())
		{
		}
		static NSObjectFlag ThrowInvalidOperationException ()
		{
			throw new InvalidOperationException ("Do not call this constructor, it may crash the app.");
		}
	}
#endif
}
