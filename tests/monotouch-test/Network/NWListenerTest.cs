using System;

using Foundation;
using Network;

using NUnit.Framework;

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWListenerTest {

		NWListener listener;

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);

		[SetUp]
		public void SetUp ()
		{
			using (var tcpOptions = new NWProtocolTcpOptions ())
			using (var tlsOptions = new NWProtocolTlsOptions ())
			using (var parameters = NWParameters.CreateTcp ()) {
				parameters.ProtocolStack.PrependApplicationProtocol (tlsOptions);
				parameters.ProtocolStack.PrependApplicationProtocol (tcpOptions);
				parameters.IncludePeerToPeer = true;
				listener = NWListener.Create ("1234", parameters);
			}
		}

		[TearDown]
		public void TearDown ()
		{
			listener?.Dispose ();
		}

		[Test]
		public void TestConnectionLimit ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			var defaultValue = 4294967295; // got it from running the code, if changes we will have an error.
			Assert.AreEqual (defaultValue, listener.ConnectionLimit);
			listener.ConnectionLimit = 10;
			Assert.AreEqual (10, listener.ConnectionLimit, "New value was not stored.");
		}

		[Test]
		public void SetNewConnectionGroupHandlerTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			Assert.DoesNotThrow (() => {
				listener.SetNewConnectionHandler ((c) => {
					Console.WriteLine ("New connection");
				});
			});
		}

#if __MACOS__
		[Test]
		public void TestCreateLaunchd ()
		{
			using var parameters = NWParameters.CreateTcp ();
			using var instance = NWListener.Create (parameters, "xamarinlaunchdkey");
			Assert.IsNotNull (instance, "Create");
		}
#endif
	}
}
