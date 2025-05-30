//
// Unit tests for AudioUnit
//

using System.Runtime.InteropServices;

using NUnit.Framework;

using Foundation;
using AudioUnit;
using AudioToolbox;
using ObjCRuntime;

namespace MonoTouchFixtures.AudioUnit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioUnitTest {
		[Test]
		public void DisposeMethodTest ()
		{
			// Test case from bxc #5410

			// Create instance of AudioUnit object
			AudioComponentDescription cd = new AudioComponentDescription () {
				ComponentType = AudioComponentType.Output,
#if MONOMAC
				ComponentSubType = AudioUnitSubType.VoiceProcessingIO,
#else
				ComponentSubType = (AudioUnitSubType) AudioTypeOutput.Remote,
#endif
				ComponentManufacturer = AudioComponentManufacturerType.Apple
			};
			AudioComponent component = AudioComponent.FindComponent (ref cd);
			var audioUnit = component.CreateAudioUnit ();

			audioUnit.Dispose ();
		}

		[Test]
		public void GetElementCount ()
		{
			var graph = new AUGraph ();
			var mixerNode = graph.AddNode (AudioComponentDescription.CreateMixer (AudioTypeMixer.MultiChannel));
			graph.Open ();
			var mixer = graph.GetNodeInfo (mixerNode);
			Assert.AreEqual (1, mixer.GetElementCount (AudioUnitScopeType.Global));
		}

		[Test]
		public void CopyIconTest ()
		{
			TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);
			AudioComponentDescription cd = new AudioComponentDescription () {
				ComponentType = AudioComponentType.Output,
#if MONOMAC
				ComponentSubType = AudioUnitSubType.VoiceProcessingIO,
#else
				ComponentSubType = (AudioUnitSubType) AudioTypeOutput.Remote,
#endif
				ComponentManufacturer = AudioComponentManufacturerType.Apple
			};
			AudioComponent component = AudioComponent.FindComponent (ref cd);
			Assert.DoesNotThrow (() => {
				var icon = component.CopyIcon (); // ensuring that the manual binding does not throw, we do not care about the result
			});
		}

		[Test]
		public unsafe void TestSizeOf ()
		{
			Assert.AreEqual (sizeof (AudioFormat), Marshal.SizeOf<AudioFormat> ());
			Assert.AreEqual (sizeof (AudioValueRange), Marshal.SizeOf<AudioValueRange> ());
			Assert.AreEqual (sizeof (AudioClassDescription), Marshal.SizeOf<AudioClassDescription> ());
		}
	}
}
