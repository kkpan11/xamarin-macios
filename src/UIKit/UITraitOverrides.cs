// 
// UITraitOverrides.cs: support for UITraitOverrides
//
// Authors:
//   Rolf Bjarne Kvinge
//
// Copyright 2023 Microsoft Corp. All rights reserved.
//

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace UIKit {
	public partial interface IUITraitOverrides {
		/// <summary>
		/// Check whether the specified trait is overridden.
		/// </summary>
		/// <typeparam name="T">The trait to check for.</typeparam>
		/// <returns>True if the specified trait is overridden.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed bool ContainsTrait<T> () where T : IUITraitDefinition
		{
			return ContainsTrait (typeof (T));
		}

		/// <summary>
		/// Removes the specified trait override.
		/// </summary>
		/// <typeparam name="T">The trait to remove.</typeparam>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed void RemoveTrait<T> () where T : IUITraitDefinition
		{
			RemoveTrait (typeof (T));
		}

		/// <summary>
		/// Check whether the specified trait is overridden.
		/// </summary>
		/// <param name="trait">The trait to check for.</param>
		/// <returns>True if the specified trait is overridden.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed bool ContainsTrait (Type trait)
		{
			return ContainsTrait (new Class (trait));
		}

		/// <summary>
		/// Removes the specified trait override.
		/// </summary>
		/// <param name="trait">The trait to remove.</param>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed void RemoveTrait (Type trait)
		{
			RemoveTrait (new Class (trait));
		}

#if !XAMCORE_5_0
		/// <summary>
		/// Check whether the specified trait is overridden.
		/// </summary>
		/// <param name="trait">The trait to check for.</param>
		/// <returns>True if the specified trait is overridden.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed bool ContainsTrait (Class trait)
		{
			global::UIKit.UIApplication.EnsureUIThread ();
			var trait__handle__ = trait!.GetNonNullHandle (nameof (trait));
			var ret = global::ObjCRuntime.Messaging.bool_objc_msgSend_NativeHandle (this.Handle, Selector.GetHandle ("containsTrait:"), trait__handle__);
			GC.KeepAlive (trait);
			return ret != 0;
		}

		/// <summary>
		/// Removes the specified trait override.
		/// </summary>
		/// <param name="trait">The trait to remove.</param>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public sealed void RemoveTrait (Class trait)
		{
			global::UIKit.UIApplication.EnsureUIThread ();
			var trait__handle__ = trait!.GetNonNullHandle (nameof (trait));
			global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, Selector.GetHandle ("removeTrait:"), trait__handle__);
			GC.KeepAlive (trait);
		}
#endif // !XAMCORE_5_0
	}
}
