using System;
using System.Threading;

using Foundation;
using ObjCRuntime;

using NUnit.Framework;

using Bindings.Test;

namespace Xamarin.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RuntimeTest {
		[Test]
		public void GlobalStringTest ()
		{
			Assert.AreEqual ("There's nothing cruvus here!", (string) Globals.GlobalString, "Global string");
		}

		[Test]
		public void WrapperTypeLookupTest ()
		{
			using (var assigner = new MyProtocolAssigner ()) {
				assigner.SetProtocol ();
			}
		}

		class MyProtocolAssigner : ProtocolAssigner {
			public bool Called;
			public override void CompletedSetProtocol (IProtocolAssignerProtocol value)
			{
				Called = true;
			}
		}

		public void EvilDeallocatorTest ()
		{
			// Create a few toggle-ref objects
			for (var i = 0; i < 10; i++) {
				var ed = new EvilDeallocator ();
				ed.MarkMeDirty ();
			}
			// Now create an object that will call a managed callback in its destructor
			using (var evil = new EvilDeallocator ()) {
				evil.EvilCallback += (int obj) => {
					// Running the GC will cause the GC to check the toggle-ref status
					// of the objects we created above.
					var t = new Thread (() => {
						GC.Collect ();
					});
					t.Start ();
					t.Join (); // If the test fails, this will deadlock.
				};
			}
		}

		[Test]
		public void MainThreadDeallocationTest ()
		{
#if OPTIMIZEALL
			if (!TestRuntime.IsLinkAll)
				Assert.Ignore ("This test must be processed by the linker if all optimizations are turned on.");
#endif

			ObjCBlockTester.CallAssertMainThreadBlockRelease ((callback) => {
				callback (42);
			});

			using (var main_thread_tester = new MainThreadTest ()) {
				main_thread_tester.CallAssertMainThreadBlockReleaseCallback ();
			}
		}

		[Test]
		public void MainThreadDeallocationTestQOS ()
		{
#if OPTIMIZEALL
			if (!TestRuntime.IsLinkAll)
				Assert.Ignore ("This test must be processed by the linker if all optimizations are turned on.");
#endif

			ObjCBlockTester.CallAssertMainThreadBlockReleaseQOS ((callback) => {
				callback (42);
			});

			using (var main_thread_tester = new MainThreadTest ()) {
				main_thread_tester.CallAssertMainThreadBlockReleaseCallbackQOS ();
			}
		}

		class MainThreadTest : ObjCBlockTester {
			public override void AssertMainThreadBlockReleaseCallback (InnerBlock completionHandler)
			{
				completionHandler (42);
			}
		}

		[Test]
		public void SwiftTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using var obj = new SwiftTestClass ();
			Assert.AreEqual ("Hello from Swift", obj.SayHello (), "Hello");
		}

		[Test]
		public void SwiftTypeEncodings ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);

			using var obj = new SwiftTestClass ();

			Assert.AreEqual ("42", obj.DoSomething ("42"), "DoSomething");

			string asyncResult = null;
			obj.DoSomethingAsync ("dolphins", (v) => asyncResult = v);
			var done = TestRuntime.RunAsync (TimeSpan.FromSeconds (5), () => asyncResult is not null);
			Assert.AreEqual ("dolphins", asyncResult, "DoSomethingAsync");
			Assert.IsTrue (done, "Done");

			obj.DoSomethingComplexAsync ("fish", IntPtr.Zero, (v) => asyncResult = v);
			done = TestRuntime.RunAsync (TimeSpan.FromSeconds (5), () => asyncResult is not null);
			Assert.AreEqual ("fish", asyncResult, "DoSomethingComplexAsync");
			Assert.IsTrue (done, "Done 2");
		}

		[Test]
		public void SwiftTestClass2 ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using var obj = new SwiftTestClass2 ();
			Assert.AreEqual ("Hello from Swift 2", obj.SayHello2 (), "Hello");
		}
	}
}
