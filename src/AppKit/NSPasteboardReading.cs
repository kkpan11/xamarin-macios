#if !__MACCATALYST__

using System;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial interface INSPasteboardReading {
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static T? CreateInstance<T> (NSObject propertyList, NSPasteboardType type) where T : NSObject, INSPasteboardReading
		{
			return CreateInstance<T> (propertyList, type.GetConstant ()!);
		}
	}
}
#endif // !__MACCATALYST__
