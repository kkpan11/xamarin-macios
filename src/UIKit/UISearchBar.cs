//
// UIKit/UISearchBar.cs: Extensions to UISearchBar
//
// Copyright 2011, Xamarin, Inc.
//
// Author:
//   Miguel de Icaza
//

using System;

using TextAttributes = UIKit.UIStringAttributes;

namespace UIKit {
	public partial class UISearchBar {
		public void SetScopeBarButtonTitle (TextAttributes attributes, UIControlState state)
		{
			if (attributes is null)
				throw new ArgumentNullException ("attributes");

			var dict = attributes.Dictionary;
			_SetScopeBarButtonTitle (dict, state);
		}

		public TextAttributes GetScopeBarButtonTitleTextAttributes (UIControlState state)
		{
			using (var d = _GetScopeBarButtonTitleTextAttributes (state)) {
				return new TextAttributes (d);
			}
		}

		public partial class UISearchBarAppearance {
			public void SetScopeBarButtonTitle (TextAttributes attributes, UIControlState state)
			{
				if (attributes is null)
					throw new ArgumentNullException ("attributes");

				var dict = attributes.Dictionary;
				_SetScopeBarButtonTitle (dict, state);
			}

			public TextAttributes GetScopeBarButtonTitleTextAttributes (UIControlState state)
			{
				using (var d = _GetScopeBarButtonTitleTextAttributes (state)) {
					return new TextAttributes (d);
				}
			}
		}
	}
}
