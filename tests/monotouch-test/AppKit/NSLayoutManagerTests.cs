#if __MACOS__
using System;
using NUnit.Framework;

using AppKit;
using ObjCRuntime;
using Foundation;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSLayoutManagerTests {
		NSLayoutManager manager;

		[SetUp]
		public void CreateManager ()
		{
			// This sets up the global context so our drawing doesn't produce error messages
			NSBitmapImageRep bitmap = new NSBitmapImageRep (IntPtr.Zero, 1000, 1000, 16, 4, true, false, NSColorSpace.DeviceRGB, 0, 0);
			NSGraphicsContext.CurrentContext = NSGraphicsContext.FromBitmap (bitmap);

			NSTextStorage storage = new NSTextStorage ("Hello World");
			NSTextContainer container = new NSTextContainer ();
			manager = new NSLayoutManager ();

			manager.AddTextContainer (container);
			storage.AddLayoutManager (manager);
		}

		[Test]
		public void NSLayoutManager_DrawGlyphsForGlyphRange ()
		{
			manager.DrawGlyphs (new NSRange (0, 4), new CGPoint (10, 10));
		}

		[Test]
		public void NSLayoutManager_CharacterRangeForGlyphRange ()
		{
			NSRange pnt;
			NSRange range = manager.GetCharacterRange (new NSRange (0, 4), out pnt);
			Assert.IsNotNull (range);
		}

		[Test]
		public void NSLayoutManager_GlyphRangeForCharacterRange ()
		{
			NSRange pnt;
			NSRange range = manager.GetGlyphRange (new NSRange (0, 4), out pnt);
			Assert.IsNotNull (range);
		}
	}
}
#endif // __MACOS__
