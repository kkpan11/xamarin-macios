//
// UIAccessibilityCustomAction.cs: Helpers for actions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin Inc
//

using System;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public partial class UIAccessibilityCustomAction {
		object action;

		/// <param name="name">To be added.</param>
		///         <param name="probe">To be added.</param>
		///         <summary>Creates a <see cref="UIKit.UIAccessibilityCustomAction" /> withthe specified <paramref name="name" />.</summary>
		///         <remarks>To be added.</remarks>
		public UIAccessibilityCustomAction (string name, Func<UIAccessibilityCustomAction, bool> probe) : this (name, FuncBoolDispatcher.Selector, new FuncBoolDispatcher (probe))
		{

		}

		internal UIAccessibilityCustomAction (string name, Selector sel, FuncBoolDispatcher disp) : this (name, disp, sel)
		{
			action = disp;
			MarkDirty ();
		}

		// Use this for synchronous operations
		[Register ("__MonoMac_FuncBoolDispatcher")]
		internal sealed class FuncBoolDispatcher : NSObject {
			public const string SelectorName = "xamarinApplySelectorFunc:";
			public static readonly Selector Selector = new Selector (SelectorName);

			readonly Func<UIAccessibilityCustomAction, bool> probe;

			public FuncBoolDispatcher (Func<UIAccessibilityCustomAction, bool> probe)
			{
				if (probe is null)
					throw new ArgumentNullException ("probe");

				this.probe = probe;
				IsDirectBinding = false;
			}

			[Export (SelectorName)]
			[Preserve (Conditional = true)]
			public bool Probe (UIAccessibilityCustomAction customAction)
			{
				return probe (customAction);
			}
		}

	}
}
