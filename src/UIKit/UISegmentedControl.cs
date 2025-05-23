// 
// UISegmentedControl.cs: Implements the managed UISegmentedControl
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
// Copyright 2011 Xamarin, Inc
//

using System;
using System.Collections;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using UIKit;

#nullable enable

namespace UIKit {
	public partial class UISegmentedControl {
		/// <param name="args">Array of strings or UIImage objects to use in the control.</param>
		///         <summary>Creates a UISegmentedControl by passing an array containing strings or <see cref="UIKit.UIImage" /> objects.</summary>
		///         <remarks>
		///         </remarks>
		public UISegmentedControl (params object [] args) : this (FromObjects (args))
		{
		}

		static NSArray FromObjects (object [] args)
		{
			if (args is null)
				throw new ArgumentNullException (nameof (args));

			NSObject [] nsargs = new NSObject [args.Length];

			for (int i = 0; i < args.Length; i++) {
				object a = args [i];
				if (a is null)
					throw new ArgumentNullException ($"Element {i} in args is null");

				var s = (a as string);
				if (s is not null) {
					nsargs [i] = new NSString (s);
					continue;
				}

				var ns = (a as NSString);
				if (ns is not null) {
					nsargs [i] = ns;
					continue;
				}

				var img = (a as UIImage);
				if (img is not null)
					nsargs [i] = img;
				else
					throw new ArgumentException ("Non-string or UIImage at position {i} with type {a.GetType ()}");
			}
			return NSArray.FromNSObjects (nsargs);
		}

		/// <param name="images">To be added.</param>
		///         <summary>Creates a new segmented control with the images in the provided array.</summary>
		///         <remarks>To be added.</remarks>
		public UISegmentedControl (params UIImage [] images) : this (FromNSObjects (images))
		{
		}

		/// <param name="strings">To be added.</param>
		///         <summary>Creates a new segmented control with the titles in the provided array.</summary>
		///         <remarks>To be added.</remarks>
		public UISegmentedControl (params NSString [] strings) : this (FromNSObjects (strings))
		{
		}

		static NSArray FromNSObjects (NSObject [] items)
		{
			// if the caller used an array [] then items can be null
			// if the caller used only null then we get an array with a null item
			if ((items is null) || ((items.Length == 1) && (items [0] is null)))
				throw new ArgumentNullException (nameof (items));

			return NSArray.FromNSObjects (items);
		}

		/// <param name="strings">To be added.</param>
		///         <summary>Creates a new segmented control with the titles in the provided array.</summary>
		///         <remarks>To be added.</remarks>
		public UISegmentedControl (params string [] strings) : this (FromStrings (strings))
		{
		}

		static NSArray FromStrings (string [] strings)
		{
			// if the caller used an array [] then items can be null
			// if the caller used only null then we get an array with a null item
			if ((strings is null) || ((strings.Length == 1) && (strings [0] is null)))
				throw new ArgumentNullException (nameof (strings));

			return NSArray.FromStrings (strings);
		}
	}
}
