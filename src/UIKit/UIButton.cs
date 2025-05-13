// 
// UIButton.cs: Extension method for buttons
//
// Authors:
//   Miguel de Icaza
//     
// Copyright 2012, 2015 Xamarin Inc
//

using System;
using ObjCRuntime;

namespace UIKit {
	public partial class UIButton {

		/// <include file="../../docs/api/UIKit/UIButton.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIButton.#ctor(UIKit.UIButtonType)']/*" />
		public UIButton (UIButtonType type)
		: base (ObjCRuntime.Messaging.NativeHandle_objc_msgSend_int (class_ptr, Selector.GetHandle ("buttonWithType:"), (int) type))
		{
			VerifyIsUIButton ();
		}

		// do NOT change this signature without updating the linker's RemoveCode step
		// this is being removed from non-debug (release) builds
		// https://trello.com/c/Nf2B8mIM/484-remove-debug-code-in-the-linker
		private void VerifyIsUIButton ()
		{
			if (GetType () == typeof (UIButton))
				return;

			Runtime.NSLog ($"The UIButton subclass {GetType ()} called the (UIButtonType) constructor, but this is not allowed. Please use the default UIButton constructor from subclasses.\n{Environment.StackTrace}");
		}
	}
}
