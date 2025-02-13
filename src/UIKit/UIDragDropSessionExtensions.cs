//
// UIDragDropSessionExtensions.cs
//
// Authors:
//   Vincent Dondain  <vidondai@microsoft.com>
//
// Copyright 2017 Microsoft
//

#if !TVOS

using System;
using ObjCRuntime;
using Foundation;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	public static class UIDragDropSessionExtensions {

		public static NSProgress LoadObjects<T> (this IUIDropSession session, Action<T []> completion) where T : NSObject, INSItemProviderReading
		{
			return session.LoadObjects (new Class (typeof (T)), (v) => {
				var arr = v as T [];
				if (arr is null && v is not null) {
					arr = new T [v.Length];
					for (int i = 0; i < arr.Length; i++) {
						if (v [i] is not null)
							arr [i] = Runtime.ConstructNSObject<T> (v [i].Handle);
					}
				}

				completion (arr);
			});
		}

		public static bool CanLoadObjects (this IUIDragDropSession session, Type type)
		{
			return session.CanLoadObjects (new Class (type));
		}
	}
}

#endif
