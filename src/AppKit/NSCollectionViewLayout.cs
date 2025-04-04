#if !__MACCATALYST__
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSCollectionViewLayout {
		/// <param name="itemClass">To be added.</param>
		///         <param name="elementKind">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void RegisterClassForDecorationView (Type itemClass, NSString elementKind)
		{
			_RegisterClassForDecorationView (itemClass is null ? IntPtr.Zero : Class.GetHandle (itemClass), elementKind);
		}
	}
}
#endif // !__MACCATALYST__
