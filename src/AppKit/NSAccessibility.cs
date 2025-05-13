//
// NSAccessibility.cs
//
// Author:
//  Chris Hamons <chris.hamons@xamarin.com>
//
// Copyright 2016 Xamarin Inc. (http://xamarin.com)

#if !__MACCATALYST__

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial interface INSAccessibility { }

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public partial class NSAccessibility {
#if !COREBUILD
		[DllImport (Constants.AppKitLibrary)]
		static extern CGRect NSAccessibilityFrameInView (NativeHandle parentView, CGRect frame);

		/// <param name="parentView">To be added.</param>
		///         <param name="frame">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGRect GetFrameInView (NSView parentView, CGRect frame)
		{
			CGRect result = NSAccessibilityFrameInView (parentView.GetHandle (), frame);
			GC.KeepAlive (parentView);
			return result;
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern CGPoint NSAccessibilityPointInView (NativeHandle parentView, CGPoint point);

		/// <param name="parentView">To be added.</param>
		///         <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CGPoint GetPointInView (NSView parentView, CGPoint point)
		{
			CGPoint result = NSAccessibilityPointInView (parentView.GetHandle (), point);
			GC.KeepAlive (parentView);
			return result;
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern void NSAccessibilityPostNotificationWithUserInfo (IntPtr element, IntPtr notification, IntPtr userInfo);

		/// <param name="element">To be added.</param>
		///         <param name="notification">To be added.</param>
		///         <param name="userInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void PostNotification (NSObject element, NSString notification, NSDictionary? userInfo)
		{
			if (element is null)
				throw new ArgumentNullException ("element");

			if (notification is null)
				throw new ArgumentNullException ("notification");

			NSAccessibilityPostNotificationWithUserInfo (element.Handle, notification.Handle, userInfo.GetHandle ());
			GC.KeepAlive (element);
			GC.KeepAlive (notification);
			GC.KeepAlive (userInfo);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern void NSAccessibilityPostNotification (IntPtr element, IntPtr notification);

		/// <param name="element">To be added.</param>
		///         <param name="notification">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void PostNotification (NSObject element, NSString notification)
		{
			if (element is null)
				throw new ArgumentNullException ("element");

			if (notification is null)
				throw new ArgumentNullException ("notification");

			NSAccessibilityPostNotification (element.Handle, notification.Handle);
			GC.KeepAlive (element);
			GC.KeepAlive (notification);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityRoleDescription (IntPtr role, IntPtr subrole);

		/// <param name="role">To be added.</param>
		///         <param name="subrole">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? GetRoleDescription (NSString role, NSString? subrole)
		{
			if (role is null)
				throw new ArgumentNullException ("role");

			IntPtr handle = NSAccessibilityRoleDescription (role.Handle, subrole.GetHandle ());
			GC.KeepAlive (role);
			GC.KeepAlive (subrole);
			return CFString.FromHandle (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityRoleDescriptionForUIElement (IntPtr element);

		/// <param name="element">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? GetRoleDescription (NSObject element)
		{
			if (element is null)
				throw new ArgumentNullException ("element");

			IntPtr handle = NSAccessibilityRoleDescriptionForUIElement (element.Handle);
			GC.KeepAlive (element);
			return CFString.FromHandle (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityActionDescription (IntPtr action);

		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? GetActionDescription (NSString action)
		{
			if (action is null)
				throw new ArgumentNullException ("action");

			IntPtr handle = NSAccessibilityActionDescription (action.Handle);
			GC.KeepAlive (action);
			return CFString.FromHandle (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityUnignoredAncestor (IntPtr element);

		/// <param name="element">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject? GetUnignoredAncestor (NSObject element)
		{
			if (element is null)
				throw new ArgumentNullException ("element");

			var handle = NSAccessibilityUnignoredAncestor (element.Handle);
			GC.KeepAlive (element);
			return Runtime.GetNSObject (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityUnignoredDescendant (IntPtr element);

		/// <param name="element">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject? GetUnignoredDescendant (NSObject element)
		{
			if (element is null)
				throw new ArgumentNullException ("element");

			var handle = NSAccessibilityUnignoredDescendant (element.Handle);
			GC.KeepAlive (element);
			return Runtime.GetNSObject (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityUnignoredChildren (IntPtr originalChildren);

		/// <param name="originalChildren">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject []? GetUnignoredChildren (NSArray originalChildren)
		{
			if (originalChildren is null)
				throw new ArgumentNullException ("originalChildren");

			var handle = NSAccessibilityUnignoredChildren (originalChildren.Handle);
			GC.KeepAlive (originalChildren);
			return NSArray.ArrayFromHandle<NSObject> (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern IntPtr NSAccessibilityUnignoredChildrenForOnlyChild (IntPtr originalChild);

		/// <param name="originalChild">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSObject []? GetUnignoredChildren (NSObject originalChild)
		{
			if (originalChild is null)
				throw new ArgumentNullException ("originalChild");

			var handle = NSAccessibilityUnignoredChildrenForOnlyChild (originalChild.Handle);
			GC.KeepAlive (originalChild);
			return NSArray.ArrayFromHandle<NSObject> (handle);
		}

		[DllImport (Constants.AppKitLibrary)]
		static extern byte NSAccessibilitySetMayContainProtectedContent (byte flag);

		/// <param name="flag">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool SetMayContainProtectedContent (bool flag)
		{
			return NSAccessibilitySetMayContainProtectedContent (flag ? (byte) 1 : (byte) 0) != 0;
		}
#endif
	}
}
#endif // !__MACCATALYST__
