//
// UICollectionView.cs: Extensions to the UICollectionView class
//
// Copyright 2012 Xamarin Inc.
//
// Authors:
//   Miguel de Icaza
//

using System;
using ObjCRuntime;
using Foundation;
using System.Threading.Tasks;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {
	public partial class UICollectionView {

		public UICollectionReusableView DequeueReusableCell (string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableCell (str, indexPath);
		}

		public UICollectionReusableView DequeueReusableSupplementaryView (NSString kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (kind, str, indexPath);
		}

		public UICollectionReusableView DequeueReusableSupplementaryView (UICollectionElementKindSection kind, string reuseIdentifier, NSIndexPath indexPath)
		{
			using (var str = (NSString) reuseIdentifier)
				return (UICollectionReusableView) DequeueReusableSupplementaryView (KindToString (kind), str, indexPath);
		}

		public void RegisterNibForCell (UINib nib, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForCell (nib, str);
		}

		public void RegisterClassForCell (Type cellType, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForCell (cellType, str);
		}

		public void RegisterClassForCell (Type cellType, NSString reuseIdentifier)
		{
			if (cellType is null)
				throw new ArgumentNullException ("cellType");

			RegisterClassForCell (Class.GetHandle (cellType), reuseIdentifier);
		}

		public void RegisterClassForSupplementaryView (Type cellType, NSString kind, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, str);
		}

		public void RegisterClassForSupplementaryView (Type cellType, NSString kind, NSString reuseIdentifier)
		{
			RegisterClassForSupplementaryView (Class.GetHandle (cellType), kind, reuseIdentifier);
		}

		public void RegisterClassForSupplementaryView (Type cellType, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterClassForSupplementaryView (cellType, section, str);
		}

		public void RegisterClassForSupplementaryView (Type cellType, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			if (cellType is null)
				throw new ArgumentNullException ("cellType");

			RegisterClassForSupplementaryView (Class.GetHandle (cellType), KindToString (section), reuseIdentifier);
		}

		public void RegisterNibForSupplementaryView (UINib nib, UICollectionElementKindSection section, string reuseIdentifier)
		{
			using (var str = (NSString) reuseIdentifier)
				RegisterNibForSupplementaryView (nib, section, str);
		}

		public void RegisterNibForSupplementaryView (UINib nib, UICollectionElementKindSection section, NSString reuseIdentifier)
		{
			RegisterNibForSupplementaryView (nib, KindToString (section), reuseIdentifier);
		}

		public NSObject DequeueReusableSupplementaryView (UICollectionElementKindSection section, NSString reuseIdentifier, NSIndexPath indexPath)
		{
			return DequeueReusableSupplementaryView (KindToString (section), reuseIdentifier, indexPath);
		}

		static NSString KindToString (UICollectionElementKindSection section)
		{
			switch (section) {
			case UICollectionElementKindSection.Header:
				return UICollectionElementKindSectionKey.Header;
			case UICollectionElementKindSection.Footer:
				return UICollectionElementKindSectionKey.Footer;
			default:
				throw new ArgumentOutOfRangeException ("section");
			}
		}

		/// <summary>An optional property that can substitute for the <see cref="P:UIKit.UICollectionView.DataSource" /> and <see cref="P:UIKit.UICollectionView.Delegate" /> properties</summary>
		///         <value>The default value is <see langword="null" />.</value>
		///         <remarks>
		///           <para>Rather than specify separate classes and provide two objects for the  <see cref="P:UIKit.UICollectionView.DataSource" /> and <see cref="P:UIKit.UICollectionView.Delegate" /> properties, one can provide a single class of type <see cref="T:UIKit.UICollectionViewSource" /> (which itself is simply defined as ).</para>
		///         </remarks>
		public UICollectionViewSource Source {
			get {
				var d = WeakDelegate as UICollectionViewSource;
				if (d is not null)
					return d;
				return null;
			}

			set {
				WeakDelegate = value;
				WeakDataSource = value;
			}
		}
	}
}
