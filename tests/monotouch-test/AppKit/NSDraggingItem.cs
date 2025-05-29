#if __MACOS__
using System;
using NUnit.Framework;

using AppKit;
using ObjCRuntime;
using Foundation;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSDraggingItemTests {
		[Test]
		public void NSDraggingItemConstructorTests ()
		{
#pragma warning disable 0219
			NSDraggingItem item = new NSDraggingItem ((NSString) "Testing");
			item = new NSDraggingItem (new MyPasteboard ());
#pragma warning restore 0219
		}

		class MyPasteboard : NSObject, INSPasteboardWriting
		{
			NSObject INSPasteboardWriting.GetPasteboardPropertyListForType (string type)
			{
				return new NSObject ();
			}

			string [] INSPasteboardWriting.GetWritableTypesForPasteboard (NSPasteboard pasteboard)
			{
				return new string [] { };
			}

			[Export ("writingOptionsForType:pasteboard:")]
			public NSPasteboardWritingOptions GetWritingOptionsForType (string type, NSPasteboard pasteboard)
			{
				return NSPasteboardWritingOptions.WritingPromised;
			}
		}
	}
}
#endif // __MACOS__
