#nullable enable

using System;
using System.Runtime.InteropServices;
using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace ClockKit {
	public partial class CLKComplication {
		[DllImport (Constants.ClockKitLibrary)]
		static extern IntPtr CLKAllComplicationFamilies ();

		public static CLKComplicationFamily [] GetAllComplicationFamilies ()
		{
			using (var nsArray = new NSArray (CLKAllComplicationFamilies ())) {
				var families = new CLKComplicationFamily [(int) nsArray.Count];
				for (nuint i = 0; i < nsArray.Count; i++) {
					families [i] = (CLKComplicationFamily) nsArray.GetItem<NSNumber> (i).Int32Value;
				}
				return families;
			}
		}
	}
}
