//
// Unit tests for UIDocument
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__TVOS__ && !MONOMAC

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Foundation;
using UIKit;
using ObjCRuntime;
using MonoTouchException = ObjCRuntime.RuntimeException;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.UIKit {

	class MyUrl : NSUrl {

		public MyUrl (string url, string annotation) : base (url)
		{
			Annotation = annotation;
		}

		public string Annotation { get; private set; }
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DocumentTest {

		class MyDocument : UIDocument {

			private string content = "content";

			public MyDocument (NSUrl url) : base (url)
			{
			}

			public override NSObject ContentsForType (string typeName, out NSError outError)
			{
				outError = null;
				return NSData.FromString (content);
			}

			public override bool LoadFromContents (NSObject contents, string typeName, out NSError outError)
			{
				outError = null;
				content = (contents as NSData).ToString ();
				return true;
			}
		}

		MyDocument doc;

		string GetFileName ()
		{
			string file = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), "mydocument.txt");
			if (File.Exists (file))
				File.Delete (file);
			return file;
		}

		[Test]
		public void Save ()
		{
			// This test may fail in the simulator, if the architecture of the simulator isn't the native one (say running x86_64 on an M1 machine),
			// so just skip this test for the simulator.
			TestRuntime.AssertIfSimulatorThenARM64 ();

			using (NSUrl url = NSUrl.FromFilename (GetFileName ())) {
				doc = new MyDocument (url);
				doc.Save (url, UIDocumentSaveOperation.ForCreating, OperationHandler);
			}
		}

		void OperationHandler (bool success)
		{
			Assert.True (success);
		}

		[Test]
		public void PerformAsynchronousFileAccess_Null ()
		{
			using (NSUrl url = NSUrl.FromFilename (GetFileName ()))
			using (var doc = new MyDocument (url)) {
				// NULL value is not documented by Apple but adding a
				// [NullAllowed] would throw an Objective-C exception (bad)
				Assert.Throws<ArgumentNullException> (() => doc.PerformAsynchronousFileAccess (null));
			}
		}

		[Test]
		public void Fields ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			// just to confirm it's not an NSUrl but an NSString
			Assert.That (UIDocument.UserActivityDocumentUrlKey.ToString (), Is.EqualTo ("NSUserActivityDocumentURL"), "NSUserActivityDocumentURLKey");
		}
	}
}

#endif // !__TVOS__ && !MONOMAC
