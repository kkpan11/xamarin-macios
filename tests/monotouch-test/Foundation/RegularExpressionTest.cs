//
// Unit tests for NSRegularExpression
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2019 Microsoft Corp. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RegularExpressionTest {
		[Test]
		public void GetMatches ()
		{
			var text = "some text https://microsoft.com text text";

			var range = new NSRange (0, text.Length);
			var detector = NSDataDetector.Create (NSTextCheckingType.Link, out NSError error);

			var matches = detector.GetMatches (new NSString (text), 0, range);

			Assert.AreEqual ((nint) 10, matches [0].Range.Location, "Range.Location");
			Assert.AreEqual ((nint) 21, matches [0].Range.Length, "Range.Length");
			Assert.AreEqual ("https://microsoft.com", matches [0].Url.AbsoluteString, "Url");
		}
	}
}
