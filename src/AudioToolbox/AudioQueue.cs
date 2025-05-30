// 
// AudioQueue.cs:
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)
//    Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2009 Novell, Inc
// Copyright 2011-2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

using AudioQueueParameterValue = System.Single;
using AudioQueueRef = System.IntPtr;
using AudioQueueTimelineRef = System.IntPtr;

namespace AudioToolbox {

	/// <summary>An enumeration whose values specify the status of an audio queue.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioQueueStatus { // Implictly cast to OSType 
		/// <summary>To be added.</summary>
		Ok = 0,
		/// <summary>To be added.</summary>
		InvalidBuffer = -66687,
		/// <summary>To be added.</summary>
		BufferEmpty = -66686,
		/// <summary>To be added.</summary>
		DisposalPending = -66685,
		/// <summary>To be added.</summary>
		InvalidProperty = -66684,
		/// <summary>To be added.</summary>
		InvalidPropertySize = -66683,
		/// <summary>To be added.</summary>
		InvalidParameter = -66682,
		/// <summary>To be added.</summary>
		CannotStart = -66681,
		/// <summary>To be added.</summary>
		InvalidDevice = -66680,
		/// <summary>To be added.</summary>
		BufferInQueue = -66679,
		/// <summary>To be added.</summary>
		InvalidRunState = -66678,
		/// <summary>To be added.</summary>
		InvalidQueueType = -66677,
		/// <summary>To be added.</summary>
		Permissions = -66676,
		/// <summary>To be added.</summary>
		InvalidPropertyValue = -66675,
		/// <summary>To be added.</summary>
		PrimeTimedOut = -66674,
		/// <summary>To be added.</summary>
		CodecNotFound = -66673,
		/// <summary>To be added.</summary>
		InvalidCodecAccess = -66672,
		/// <summary>To be added.</summary>
		QueueInvalidated = -66671,
		/// <summary>To be added.</summary>
		TooManyTaps = -66670,
		/// <summary>To be added.</summary>
		InvalidTapContext = -66669,
		/// <summary>To be added.</summary>
		RecordUnderrun = -66668,
		/// <summary>To be added.</summary>
		InvalidTapType = -66667,
		/// <summary>To be added.</summary>
		EnqueueDuringReset = -66632,
		/// <summary>To be added.</summary>
		InvalidOfflineMode = -66626,
		/// <summary>To be added.</summary>
		BufferEnqueuedTwice = -66666,
		/// <summary>To be added.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		CannotStartYet = -66665,

		// There is countless of not well documented error codes returned
		/// <summary>To be added.</summary>
		QueueStopped = 0x73746f70,  // 'stop'
		/// <summary>To be added.</summary>
		DataFormatError = 0x666d743f,   // 'fmt?'
		/// <summary>To be added.</summary>
		UnsupportedProperty = 0x70726F70,  // 'prop'

		// From kAudio_
		/// <summary>To be added.</summary>
		GeneralParamError = -50,
	}

	/// <summary>An exception thrown by the AudioQueue class if there is a problem with the configuration parameters.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioQueueException : Exception {
		static string Lookup (int k)
		{
			var status = (AudioQueueStatus) k;
			switch (status) {
			case AudioQueueStatus.InvalidBuffer:
				return "The specified audio queue buffer does not belong to this audio queue";

			case AudioQueueStatus.BufferEmpty:
				return "The audio buffer is empty (AudioDataByteSize is zero)";

			case AudioQueueStatus.DisposalPending:
				return "This audio queue is being asynchronously disposed";

			case AudioQueueStatus.InvalidProperty:
				return "Invalid property specified";

			case AudioQueueStatus.InvalidPropertySize:
				return "Invalid property size";

			case AudioQueueStatus.InvalidParameter:
				return "The specified parameter is invalid";

			case AudioQueueStatus.CannotStart:
				return "The queue has encountered a problem and can not start";

			case AudioQueueStatus.InvalidDevice:
				return "The specified hardware device can not be located";

			case AudioQueueStatus.BufferInQueue:
				return "The specified buffer can not be disposed while it is on an active queue";

			case AudioQueueStatus.InvalidRunState:
				return "The operation can not be performed in the current queue state (running or stopped)";

			case AudioQueueStatus.InvalidQueueType:
				return "Invalid queue type (input operation attempted on output or output operation on input";

			case AudioQueueStatus.Permissions:
				return "No permissions to access that function";

			case AudioQueueStatus.InvalidPropertyValue:
				return "The property value is invalid";

			case AudioQueueStatus.PrimeTimedOut:
				return "Timeout during Prime operation";

			case AudioQueueStatus.CodecNotFound:
				return "The requested codec was not found";

			case AudioQueueStatus.InvalidCodecAccess:
				return "The codec could not be accessed";

			case AudioQueueStatus.QueueInvalidated:
				return "The audio server has terminated, the queue has been invalidated";

			case AudioQueueStatus.RecordUnderrun:
				return "Recording lost data because enqueued buffer was not available";

			case AudioQueueStatus.EnqueueDuringReset:
				return "You tried to enqueue a buffer during a Reset, Stop or Dispose methods";

			case AudioQueueStatus.InvalidOfflineMode:
				return "Offline mode is either required or not required for the operation";

			case AudioQueueStatus.GeneralParamError:
				return "Error in user parameter list";

			default:
				return String.Format ("Error code: {0}", status.ToString ());
			}
		}

		internal AudioQueueException (AudioQueueStatus k) : base (Lookup ((int) k))
		{
			ErrorCode = k;
		}

		internal AudioQueueException (int k) : base (Lookup (k))
		{
			ErrorCode = (AudioQueueStatus) k;
		}

		/// <summary>The underlying AudioToolbox error code.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus ErrorCode { get; private set; }
	}

	/// <summary>An enumeration whose values specify properties of audio queues.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioQueueProperty : uint // UInt32 AudioQueuePropertyID
	{
		/// <summary>To be added.</summary>
		IsRunning = 0x6171726e,
		/// <summary>To be added.</summary>
		DeviceSampleRate = 0x61717372,          // 'aqsr'
		/// <summary>To be added.</summary>
		DeviceNumberChannels = 0x61716463,
		/// <summary>To be added.</summary>
		CurrentDevice = 0x61716364,
		/// <summary>To be added.</summary>
		MagicCookie = 0x61716d63,
		/// <summary>To be added.</summary>
		MaximumOutputPacketSize = 0x786f7073,   // 'xops'
		/// <summary>To be added.</summary>
		StreamDescription = 0x61716674,         // 'aqft'
		/// <summary>To be added.</summary>
		ChannelLayout = 0x6171636c,             // 'aqcl'
		/// <summary>To be added.</summary>
		EnableLevelMetering = 0x61716d65,
		/// <summary>To be added.</summary>
		CurrentLevelMeter = 0x61716d76,
		/// <summary>To be added.</summary>
		CurrentLevelMeterDB = 0x61716d64,
		/// <summary>To be added.</summary>
		DecodeBufferSizeFrames = 0x64636266,
		/// <summary>To be added.</summary>
		ConverterError = 0x71637665,            // 'qcve'
		/// <summary>To be added.</summary>
		EnableTimePitch = 0x715f7470,   // 'q_tp'
		/// <summary>To be added.</summary>
		TimePitchAlgorithm = 0x71747061,    // 'qtpa'
		/// <summary>To be added.</summary>
		TimePitchBypass = 0x71747062,   // 'qtpb'
#if !MONOMAC
		/// <summary>To be added.</summary>
		HardwareCodecPolicy = 0x61716370,   // 'aqcp'
		/// <summary>To be added.</summary>
		ChannelAssignments = 0x61716361,    // 'aqca'
#endif
	}

	/// <summary>An enumeration whose values specify the Time Pitch algorithm. Used with <see cref="AudioToolbox.AudioQueueProperty.TimePitchAlgorithm" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioQueueTimePitchAlgorithm : uint {
		/// <summary>To be added.</summary>
		Spectral = 0x73706563,                  // spec
		/// <summary>To be added.</summary>
		TimeDomain = 0x7469646f,                // tido
#if !MONOMAC
		/// <summary>To be added.</summary>
		LowQualityZeroLatency = 0x6c717a6c,     // lqzl
#endif
		/// <summary>To be added.</summary>
		Varispeed = 0x76737064,                 // vspd
	}

#if !(XAMCORE_5_0 && __MACOS__)
#if !__MACOS__
	/// <summary>An enumeration whose values are used for the <see cref="AudioToolbox.AudioQueue.HardwareCodecPolicy" /> property.</summary>
#endif
#if XAMCORE_5_0
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("macos")]
#endif // XAMCORE_5_0
	public enum AudioQueueHardwareCodecPolicy { // A AudioQueuePropertyID (UInt32)
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		UseSoftwareOnly = 1,
		/// <summary>To be added.</summary>
		UseHardwareOnly = 2,
		/// <summary>To be added.</summary>
		PreferSoftware = 3,
		/// <summary>To be added.</summary>
		PreferHardware = 4,
	}
#endif // !(XAMCORE_5_0 && __MACOS__)

	/// <summary>An enumeration whose values specify various parameters of an audio queue.</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioQueueParameter : uint // UInt32 AudioQueueParameterID
	{
		/// <summary>To be added.</summary>
		Volume = 1,
		/// <summary>To be added.</summary>
		PlayRate = 2,
		/// <summary>To be added.</summary>
		Pitch = 3,
		/// <summary>To be added.</summary>
		VolumeRampTime = 4,
		/// <summary>To be added.</summary>
		Pan = 13,
	}

	/// <summary>An enumeration whose values specify properties of an audio queue device (number of channels and sample rate).</summary>
	///     <remarks>To be added.</remarks>
	public enum AudioQueueDeviceProperty { // UInt32 AudioQueueParameterID
		/// <summary>To be added.</summary>
		SampleRate = 0x61717372,
		/// <summary>To be added.</summary>
		NumberChannels = 0x61716463,
	}

	/// <summary>Flags used when an AudioQueue tap is created, and used by the tap processor callback.</summary>
	///     <remarks>
	///       <para>
	/// 	The PostEffects, PreEffects, Siphon values are used both when
	/// 	creating a audio queue tap (using <see cref="AudioToolbox.AudioQueue.CreateProcessingTap(AudioToolbox.AudioQueueProcessingTapDelegate,AudioToolbox.AudioQueueProcessingTapFlags,out AudioToolbox.AudioQueueStatus)" />)
	/// 	and are provided to the tap callback (of type <see cref="AudioToolbox.AudioQueueProcessingTapDelegate" />).
	///
	///       </para>
	///       <para>
	/// 	The StartOfStream and EndOfStream are returned by <see cref="AudioToolbox.AudioQueueProcessingTap" />'s
	/// 	GetSourceAudio method.
	///
	///       </para>
	///     </remarks>
	[Flags]
	public enum AudioQueueProcessingTapFlags : uint // UInt32 in AudioQueueProcessingTapNew
	{
		/// <summary>The tap is executed before any effects have run.</summary>
		PreEffects = (1 << 0),
		/// <summary>The tap is executed after any effects have run.</summary>
		PostEffects = (1 << 1),
		/// <summary>The tap is a siphon tap, it can only examine the AudioBuffers provided to the callback, but should not modify its contents.</summary>
		Siphon = (1 << 2),

		/// <summary>Indicates the start of audio and is returned by GetSourceAudio.  As a flag passed to a tap processor, this indicates a discontinuity in the audio.   Either because it is starting, or because there is a playback gap.  For the tap processor this means  that the data being requested should correspond to the first frame in the audio source.   This should reset any internal state in the tap processor that might have been saved from previous invocations to the tap handler.</summary>
		StartOfStream = (1 << 8),
		/// <summary>Indicates the end of the audio stream, it happens when the queue is being stopped asynchronosuly and is returned by a call to GetSourceAudio.  You must propagate this value to the caller.</summary>
		EndOfStream = (1 << 9),
	}

	/// <summary>Represents an audio queue buffer.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioQueueBuffer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint AudioDataBytesCapacity;
		/// <summary>Pointer to the audio data.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr AudioData;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint AudioDataByteSize;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr UserData;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint PacketDescriptionCapacity;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr IntPtrPacketDescriptions;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public int PacketDescriptionCount;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? PacketDescriptions {
			get {
				return AudioFile.PacketDescriptionFrom (PacketDescriptionCount, IntPtrPacketDescriptions);
			}
		}

		/// <param name="source">Pointer to the data to copy.</param>
		///         <param name="size">Number of bytes to copy.</param>
		///         <summary>Copies the specified buffer AudioQueue's AudioData buffer.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe void CopyToAudioData (IntPtr source, int size)
		{
			Buffer.MemoryCopy ((void*) source, (void*) AudioData, AudioDataByteSize, size);
		}
	}

	/// <summary>A class that encapsulates values used as <c>parameterEvents</c> in calls to the <see cref="AudioToolbox.InputAudioQueue.EnqueueBuffer(AudioToolbox.AudioQueueBuffer*)" /> method. </summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Explicit)]
	public struct AudioQueueParameterEvent {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		[Advice ("Use Parameter.")]
		public uint ID;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public AudioQueueParameter Parameter;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (4)]
		public float Value;

		/// <param name="parameter">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioQueueParameterEvent (AudioQueueParameter parameter, float value)
		{
			this.ID = (uint) parameter;
			this.Parameter = parameter;
			this.Value = value;
		}
	}

	/// <summary>Represents the level meter information on an audio channel.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioQueueLevelMeterState {
		/// <summary>The audio channel average root mean square power.</summary>
		///         <remarks>
		///         </remarks>
		public float AveragePower;
		/// <summary>The audio channel peak root mean square power.</summary>
		///         <remarks>
		///         </remarks>
		public float PeakPower;
	}

#if !(XAMCORE_5_0 && __MACOS__)
#if !__MACOS__
	/// <summary>Channel assignments used as a parameter to the <see cref="AudioToolbox.AudioQueue.SetChannelAssignments(AudioToolbox.AudioQueueChannelAssignment[])" /> method.</summary>
#endif
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
#if XAMCORE_5_0
	[UnsupportedOSPlatform ("macos")]
#else
	[SupportedOSPlatform ("macos")]
#endif
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioQueueChannelAssignment {
		IntPtr deviceUID; // CFString
		uint channelNumber;

		/// <param name="deviceUID">To be added.</param>
		///         <param name="channelNumber">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioQueueChannelAssignment (CFString deviceUID, uint channelNumber)
		{
			this.deviceUID = deviceUID.Handle;
			this.channelNumber = channelNumber;
			GC.KeepAlive (deviceUID);
		}
	}
#endif // !(XAMCORE_5_0 && __MACOS__)

	delegate void AudioQueuePropertyListener (IntPtr userData, IntPtr AQ, AudioQueueProperty id);

	/// <summary>Provides data for the <see cref="AudioToolbox.OutputAudioQueue.BufferCompleted" /> event.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class BufferCompletedEventArgs : EventArgs {
		/// <param name="audioQueueBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public BufferCompletedEventArgs (IntPtr audioQueueBuffer)
		{
			IntPtrBuffer = audioQueueBuffer;
		}

		/// <param name="audioQueueBuffer">To be added.</param>
		///         <summary>Initializes a new instance of the BufferCompletedEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe BufferCompletedEventArgs (AudioQueueBuffer* audioQueueBuffer)
		{
			IntPtrBuffer = (IntPtr) audioQueueBuffer;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr IntPtrBuffer { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioQueueBuffer* UnsafeBuffer {
			get { return (AudioQueueBuffer*) IntPtrBuffer; }
			set { IntPtrBuffer = (IntPtr) value; }
		}
	}

	/// <summary>Provides data for the <see cref="AudioToolbox.InputAudioQueue.InputCompleted" /> event.</summary>
	///     <remarks>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class InputCompletedEventArgs : EventArgs {
		/// <param name="audioQueueBuffer">To be added.</param>
		///         <param name="timeStamp">To be added.</param>
		///         <param name="pdec">To be added.</param>
		///         <summary>Initializes a new instance of the InputCompletedEventArgs class.</summary>
		///         <remarks>
		///         </remarks>
		public unsafe InputCompletedEventArgs (IntPtr audioQueueBuffer, AudioTimeStamp timeStamp, AudioStreamPacketDescription []? pdec)
		{
			IntPtrBuffer = audioQueueBuffer;
			TimeStamp = timeStamp;
			PacketDescriptions = pdec;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public IntPtr IntPtrBuffer { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioQueueBuffer* UnsafeBuffer {
			get { return (AudioQueueBuffer*) IntPtrBuffer; }
			set { IntPtrBuffer = (IntPtr) value; }
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe AudioQueueBuffer Buffer {
			get { return *(AudioQueueBuffer*) IntPtrBuffer; }
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioTimeStamp TimeStamp { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioStreamPacketDescription []? PacketDescriptions { get; private set; }
	}

	/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='T:AudioToolbox.AudioQueue']/*" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public abstract class AudioQueue : IDisposable {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		internal protected IntPtr handle;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		internal protected GCHandle gch;

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public IntPtr Handle { get { return handle; } }

		internal AudioQueue ()
		{
		}

		~AudioQueue ()
		{
			Dispose (false, true);
		}

		/// <summary>Releases the resources used by the AudioQueue object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioQueue class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioQueue ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true, true);
			GC.SuppressFinalize (this);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void QueueDispose ()
		{
			Dispose (true, false);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioQueueDispose (IntPtr AQ, byte immediate);

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueue.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			Dispose (disposing, true);
		}

		void Dispose (bool disposing, bool immediate)
		{
			if (handle != IntPtr.Zero) {
				if (disposing) {
					if (listeners is not null) {
						foreach (AudioQueueProperty prop in listeners.Keys) {
							unsafe {
								AudioQueueRemovePropertyListener (handle, prop, &property_changed, GCHandle.ToIntPtr (gch));
							}
						}
					}
				}

				AudioQueueDispose (handle, immediate ? (byte) 1 : (byte) 0);
				handle = IntPtr.Zero;
			}

			if (gch.IsAllocated)
				gch.Free ();
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueStart (IntPtr AQ, AudioTimeStamp* startTime);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueStart (IntPtr AQ, IntPtr startTime);

		/// <param name="startTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Start (AudioTimeStamp startTime)
		{
			unsafe {
				return AudioQueueStart (handle, &startTime);
			}
		}

		/// <summary>Starts the audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Start ()
		{
			return AudioQueueStart (handle, IntPtr.Zero);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueuePrime (IntPtr AQ, int toPrepare, int* prepared);
		/// <param name="toPrepare">Number of frames to process.  If you pass zero, this will process all the frames.</param>
		///         <param name="prepared">Returns the number of frames actually processed</param>
		///         <summary>Used to prepare the audio buffers to play back and ensure that there is data ready to be played by the audio hardware.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus Prime (int toPrepare, out int prepared)
		{
			prepared = 0;
			unsafe {
				return AudioQueuePrime (handle, toPrepare, AsPointer<int> (ref prepared));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueFlush (IntPtr aq);
		/// <summary>To be added.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Flush ()
		{
			return AudioQueueFlush (handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueStop (IntPtr aq, byte immediate);
		/// <param name="immediate">If true, by the time the function returns, audio would have stopped playing.   Otherwise the pending buffers are flushed and audio continues to play or be recorded until then.</param>
		///         <summary>Stops the AudioQueue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Stop (bool immediate)
		{
			return AudioQueueStop (handle, immediate ? (byte) 1 : (byte) 0);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueuePause (IntPtr aq);
		/// <summary>To be added.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Pause ()
		{
			return AudioQueuePause (handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueReset (IntPtr aq);
		/// <summary>To be added.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus Reset ()
		{
			return AudioQueueReset (handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueAllocateBuffer (AudioQueueRef AQ, int bufferSize, IntPtr* audioQueueBuffer);
		/// <param name="bufferSize">The audio buffer size to allocate (in bytes).</param>
		///         <param name="audioQueueBuffer">Returns the pointer to the allocated buffer as an IntPtr.</param>
		///         <summary>Allocates an audio buffer associated with this AudioQueue, used for fixed bit rate buffers.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error. </returns>
		///         <remarks>
		///           <para>
		/// 	    Use the <see cref="AudioToolbox.AudioQueue.AllocateBufferWithPacketDescriptors(System.Int32,System.Int32,out System.IntPtr)" /> to allocate buffers that will be used with variable bit
		/// 	    rate encodings.
		/// 	  </para>
		///           <para>Use <see cref="AudioToolbox.AudioQueue.FreeBuffer(System.IntPtr)" /> to dispose the buffer.</para>
		///         </remarks>
		public AudioQueueStatus AllocateBuffer (int bufferSize, out IntPtr audioQueueBuffer)
		{
			audioQueueBuffer = default (IntPtr);
			unsafe {
				return AudioQueueAllocateBuffer (handle, bufferSize, AsPointer<IntPtr> (ref audioQueueBuffer));
			}
		}

		/// <param name="bufferSize">The audio buffer size to allocate (in bytes).</param>
		///         <param name="audioQueueBuffer">Returns the allocated buffer as an unsafe AudioQueueBuffer pointer.</param>
		///         <summary>Allocates an audio buffer associated with this AudioQueue</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error. </returns>
		///         <remarks>
		/// 	  Use <see cref="AudioToolbox.AudioQueue.FreeBuffer(System.IntPtr)" /> to dispose the buffer.
		///         </remarks>
		public unsafe AudioQueueStatus AllocateBuffer (int bufferSize, out AudioQueueBuffer* audioQueueBuffer)
		{
			IntPtr buf;
			AudioQueueStatus result;
			result = AudioQueueAllocateBuffer (handle, bufferSize, &buf);
			audioQueueBuffer = (AudioQueueBuffer*) buf;
			return result;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueAllocateBufferWithPacketDescriptions (IntPtr AQ, int bufferSize, int nPackets, IntPtr* audioQueueBuffer);
		/// <param name="bufferSize">Size of the buffer to allocate.</param>
		///         <param name="nPackets">Number of packets descriptors in the audio queue buffer.</param>
		///         <param name="audioQueueBuffer">The allocated buffer on return</param>
		///         <summary>Allocates an audio queue object for variable-bit-rate buffers.</summary>
		///         <returns>AudioQueueStatus.Ok on success and the audioQueueBuffer pointing to the buffer, otherwise the error.</returns>
		///         <remarks>
		///           <para>
		/// 	    Use the <see cref="AudioToolbox.AudioQueue.AllocateBuffer(System.Int32,out AudioToolbox.AudioQueueBuffer*)" /> to allocate buffers that will be used with fixed bit
		/// 	    rate encodings.
		/// 	  </para>
		///           <para>Use <see cref="AudioToolbox.AudioQueue.FreeBuffer(System.IntPtr)" /> to dispose the buffer.</para>
		///         </remarks>
		public AudioQueueStatus AllocateBufferWithPacketDescriptors (int bufferSize, int nPackets, out IntPtr audioQueueBuffer)
		{
			audioQueueBuffer = default (IntPtr);
			unsafe {
				return AudioQueueAllocateBufferWithPacketDescriptions (handle, bufferSize, nPackets, AsPointer<IntPtr> (ref audioQueueBuffer));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueFreeBuffer (IntPtr AQ, IntPtr audioQueueBuffer);
		/// <param name="audioQueueBuffer">AudioQueue buffer previously allocated with AllocateBuffer.</param>
		///         <summary>Releases an AudioQueue buffer.</summary>
		///         <remarks>
		///         </remarks>
		public void FreeBuffer (IntPtr audioQueueBuffer)
		{
			if (audioQueueBuffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));
			AudioQueueFreeBuffer (handle, audioQueueBuffer);
		}

		public static void FillAudioData (IntPtr audioQueueBuffer, int offset, IntPtr source, int sourceOffset, nint size)
		{
			IntPtr target = Marshal.ReadIntPtr (audioQueueBuffer, IntPtr.Size);
			unsafe {
				Buffer.MemoryCopy ((void*) (source + sourceOffset), (void*) (target + offset), size, size);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		internal extern unsafe static AudioQueueStatus AudioQueueEnqueueBuffer (IntPtr AQ, AudioQueueBuffer* audioQueueBuffer, int nPackets, AudioStreamPacketDescription* desc);

		/// <param name="audioQueueBuffer">The audio queue buffer to add to the buffer queue.</param>
		///         <param name="bytes">The number of bytes from the queue buffer to add to the buffer queue. The audioQueueBuffer parameter will be updated with this value.</param>
		///         <param name="desc">An array of packet descriptors for the packets that will be added to the queue.</param>
		///         <summary>Adds a buffer to the buffer queue of an audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus EnqueueBuffer (IntPtr audioQueueBuffer, int bytes, AudioStreamPacketDescription [] desc)
		{
			if (audioQueueBuffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			unsafe {
				AudioQueueBuffer* buffer = (AudioQueueBuffer*) audioQueueBuffer;
				buffer->AudioDataByteSize = (uint) bytes;
				return EnqueueBuffer (buffer, desc);
			}
		}

		/// <param name="audioQueueBuffer">The audio queue buffer to add to the buffer queue.</param>
		///         <param name="desc">An array of packet descriptors for the packets that will be added to the queue.</param>
		///         <summary>Adds a buffer to the buffer queue of an audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public unsafe AudioQueueStatus EnqueueBuffer (AudioQueueBuffer* audioQueueBuffer, AudioStreamPacketDescription [] desc)
		{
			if (audioQueueBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			fixed (AudioStreamPacketDescription* descPtr = desc) {
				return AudioQueueEnqueueBuffer (handle, audioQueueBuffer, desc?.Length ?? 0, descPtr);
			}
		}

		/// <param name="audioQueueBuffer">To be added.</param>
		///         <param name="desc">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioQueueStatus EnqueueBuffer (IntPtr audioQueueBuffer, AudioStreamPacketDescription [] desc)
		{
			if (audioQueueBuffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			fixed (AudioStreamPacketDescription* descPtr = desc) {
				return AudioQueueEnqueueBuffer (handle, (AudioQueueBuffer*) audioQueueBuffer, desc?.Length ?? 0, descPtr);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static AudioQueueStatus AudioQueueEnqueueBufferWithParameters (
			IntPtr AQ,
			AudioQueueBuffer* audioQueueBuffer,
			int nPackets,
			AudioStreamPacketDescription* desc,
			int trimFramesAtStart,
			int trimFramesAtEnd,
			int nParam,
			AudioQueueParameterEvent* parameterEvents,
			AudioTimeStamp* startTime,
			AudioTimeStamp* actualStartTime);

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueue.EnqueueBuffer(System.IntPtr,System.Int32,AudioToolbox.AudioStreamPacketDescription[],System.Int32,System.Int32,AudioToolbox.AudioQueueParameterEvent[],AudioToolbox.AudioTimeStamp@,AudioToolbox.AudioTimeStamp@)']/*" />
		public AudioQueueStatus EnqueueBuffer (IntPtr audioQueueBuffer, int bytes, AudioStreamPacketDescription [] desc,
								   int trimFramesAtStart, int trimFramesAtEnd, AudioQueueParameterEvent [] parameterEvents,
								   ref AudioTimeStamp startTime, out AudioTimeStamp actualStartTime)
		{
			if (audioQueueBuffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			actualStartTime = default (AudioTimeStamp);

			unsafe {
				AudioQueueBuffer* buffer = (AudioQueueBuffer*) audioQueueBuffer;
				buffer->AudioDataByteSize = (uint) bytes;

				fixed (AudioStreamPacketDescription* descPtr = desc)
				fixed (AudioQueueParameterEvent* parameterEventsPtr = parameterEvents) {
					return AudioQueueEnqueueBufferWithParameters (
						handle, buffer, desc?.Length ?? 0, descPtr,
						trimFramesAtStart, trimFramesAtEnd, parameterEvents?.Length ?? 0,
						parameterEventsPtr,
						AsPointer<AudioTimeStamp> (ref startTime),
						AsPointer<AudioTimeStamp> (ref actualStartTime));
				}
			}
		}
		/// <param name="audioQueueBuffer">The audio queue buffer to add to the buffer queue.</param>
		///         <param name="bytes">The number of bytes from the queue buffer to add to the buffer queue. The audioQueueBuffer parameter will be updated with this value.</param>
		///         <param name="desc">An array of packet descriptors for the packets that will be added to the queue.</param>
		///         <param name="trimFramesAtStart">The number of frames to skip at the start of the buffer. </param>
		///         <param name="trimFramesAtEnd">The number of frames to skip at the end of the buffer.</param>
		///         <param name="parameterEvents">An array of parameter events for the buffer.</param>
		///         <param name="actualStartTime">The time when the buffer will start playing.</param>
		///         <summary>Adds a buffer that should play as soon as possible to the buffer queue of a playback audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus EnqueueBuffer (IntPtr audioQueueBuffer, int bytes, AudioStreamPacketDescription [] desc,
							   int trimFramesAtStart, int trimFramesAtEnd, AudioQueueParameterEvent [] parameterEvents,
							   out AudioTimeStamp actualStartTime)
		{
			if (audioQueueBuffer == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			actualStartTime = default (AudioTimeStamp);

			unsafe {
				AudioQueueBuffer* buffer = (AudioQueueBuffer*) audioQueueBuffer;
				buffer->AudioDataByteSize = (uint) bytes;

				fixed (AudioStreamPacketDescription* descPtr = desc)
				fixed (AudioQueueParameterEvent* parameterEventsPtr = parameterEvents) {
					return AudioQueueEnqueueBufferWithParameters (
						handle, buffer, desc?.Length ?? 0, descPtr,
						trimFramesAtStart, trimFramesAtEnd, parameterEvents?.Length ?? 0,
						parameterEventsPtr,
						null,
						AsPointer<AudioTimeStamp> (ref actualStartTime));
				}
			}
		}

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueue.EnqueueBuffer(AudioToolbox.AudioQueueBuffer*,System.Int32,AudioToolbox.AudioStreamPacketDescription[],System.Int32,System.Int32,AudioToolbox.AudioQueueParameterEvent[],AudioToolbox.AudioTimeStamp@,AudioToolbox.AudioTimeStamp@)']/*" />
		public unsafe AudioQueueStatus EnqueueBuffer (AudioQueueBuffer* audioQueueBuffer, int bytes, AudioStreamPacketDescription [] desc,
								   int trimFramesAtStart, int trimFramesAtEnd, AudioQueueParameterEvent [] parameterEvents,
								   ref AudioTimeStamp startTime, out AudioTimeStamp actualStartTime)
		{
			if (audioQueueBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			fixed (AudioStreamPacketDescription* descPtr = desc)
			fixed (AudioQueueParameterEvent* parameterEventsPtr = parameterEvents) {
				startTime = default;
				actualStartTime = default;
				return AudioQueueEnqueueBufferWithParameters (
					handle, audioQueueBuffer, desc?.Length ?? 0, descPtr,
					trimFramesAtStart, trimFramesAtEnd, parameterEvents?.Length ?? 0,
					parameterEventsPtr,
					AsPointer<AudioTimeStamp> (ref startTime),
					AsPointer<AudioTimeStamp> (ref actualStartTime));
			}
		}

		/// <param name="audioQueueBuffer">The audio queue buffer to add to the buffer queue.</param>
		///         <param name="bytes">The number of bytes from the queue buffer to add to the buffer queue. The audioQueueBuffer parameter will be updated with this value.</param>
		///         <param name="desc">An array of packet descriptors for the packets that will be added to the queue.</param>
		///         <param name="trimFramesAtStart">The number of frames to skip at the start of the buffer. </param>
		///         <param name="trimFramesAtEnd">The number of frames to skip at the end of the buffer.</param>
		///         <param name="parameterEvents">An array of parameter events for the buffer.</param>
		///         <param name="actualStartTime">The time when the buffer will start playing.</param>
		///         <summary>Adds a buffer that should play as soon as possible to the buffer queue of a playback audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public unsafe AudioQueueStatus EnqueueBuffer (AudioQueueBuffer* audioQueueBuffer, int bytes, AudioStreamPacketDescription [] desc,
							   int trimFramesAtStart, int trimFramesAtEnd, AudioQueueParameterEvent [] parameterEvents,
							   out AudioTimeStamp actualStartTime)
		{
			if (audioQueueBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			fixed (AudioStreamPacketDescription* descPtr = desc)
			fixed (AudioQueueParameterEvent* parameterEventsPtr = parameterEvents) {
				actualStartTime = default;
				return AudioQueueEnqueueBufferWithParameters (
					handle, audioQueueBuffer, desc?.Length ?? 0, descPtr,
					trimFramesAtStart, trimFramesAtEnd, parameterEvents?.Length ?? 0,
					parameterEventsPtr,
					null,
					AsPointer<AudioTimeStamp> (ref actualStartTime));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueCreateTimeline (IntPtr AQ, IntPtr* timeline);

		/// <summary>Creates a timeline object that can be used to track discontinuities in the audio queue's audio.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueTimeline? CreateTimeline ()
		{
			IntPtr thandle;

			unsafe {
				if (AudioQueueCreateTimeline (handle, &thandle) == AudioQueueStatus.Ok)
					return new AudioQueueTimeline (handle, thandle);
			}
			return null;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueGetCurrentTime (IntPtr AQ, IntPtr timelineHandle, AudioTimeStamp* time, byte* discontinuty);

		/// <param name="timeline">Timeline object to track discontinuities, or null if you do not need it.</param>
		///         <param name="time">The time</param>
		///         <param name="timelineDiscontinuty">On return, if true, it means that there was an audio discontinuity.</param>
		///         <summary>Returns the current time for the audio queue.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error.</returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus GetCurrentTime (AudioQueueTimeline? timeline, ref AudioTimeStamp time, ref bool timelineDiscontinuty)
		{
			IntPtr arg;
			if (timeline is null)
				arg = IntPtr.Zero;
			else {
				arg = timeline.timelineHandle;
				if (arg == IntPtr.Zero)
					throw new ObjectDisposedException ("timeline");
			}

			byte timelineDiscontinuityPtr;
			AudioQueueStatus rv;
			unsafe {
				rv = AudioQueueGetCurrentTime (handle, arg, AsPointer<AudioTimeStamp> (ref time), &timelineDiscontinuityPtr);
			}
			timelineDiscontinuty = timelineDiscontinuityPtr != 0;
			return rv;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueDeviceGetCurrentTime (IntPtr AQ, AudioTimeStamp* time);

		/// <summary>Returns the current time for the hardware device.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AudioTimeStamp CurrentTime {
			get {
				AudioTimeStamp stamp = new AudioTimeStamp ();

				unsafe {
					if (AudioQueueDeviceGetCurrentTime (handle, &stamp) != AudioQueueStatus.Ok) {
						// Set no values as valid
						stamp.Flags = 0;
					}
				}

				return stamp;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueDeviceGetNearestStartTime (IntPtr AQ, AudioTimeStamp* data, int flags);

		/// <param name="requestedStartTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioTimeStamp GetNearestStartTime (AudioTimeStamp requestedStartTime)
		{
			unsafe {
				var k = AudioQueueDeviceGetNearestStartTime (handle, &requestedStartTime, 0);
				if (k != 0)
					throw new AudioQueueException (k);
			}

			return requestedStartTime;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueDeviceTranslateTime (IntPtr AQ, AudioTimeStamp* inTime, AudioTimeStamp* translatedTime);

		/// <param name="timeToTranslate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioTimeStamp TranslateTime (AudioTimeStamp timeToTranslate)
		{
			AudioTimeStamp ret;

			unsafe {
				AudioQueueDeviceTranslateTime (handle, &timeToTranslate, &ret);
			}
			return ret;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static OSStatus AudioQueueGetParameter (IntPtr AQ, AudioQueueParameter parameterId, float* result);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioQueueSetParameter (IntPtr AQ, AudioQueueParameter parameterId, float value);

		/// <summary>The volume</summary>
		///         <value>Value between 0.0 and 1.0.</value>
		///         <remarks>
		///         </remarks>
		public float Volume {
			get {
				float r;
				unsafe {
					var res = AudioQueueGetParameter (handle, AudioQueueParameter.Volume, &r);
					if (res != 0)
						throw new AudioQueueException (res);
				}

				return r;
			}

			set {
				var res = AudioQueueSetParameter (handle, AudioQueueParameter.Volume, value);
				if (res != 0)
					throw new AudioQueueException (res);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float VolumeRampTime {
			get {
				float r;
				unsafe {
					var res = AudioQueueGetParameter (handle, AudioQueueParameter.VolumeRampTime, &r);
					if (res != 0)
						throw new AudioQueueException (res);
				}

				return r;
			}

			set {
				var res = AudioQueueSetParameter (handle, AudioQueueParameter.VolumeRampTime, value);
				if (res != 0)
					throw new AudioQueueException (res);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float Pan {
			get {
				float r;
				unsafe {
					var res = AudioQueueGetParameter (handle, AudioQueueParameter.Pan, &r);
					if (res != 0)
						throw new AudioQueueException (res);
				}

				return r;
			}

			set {
				var res = AudioQueueSetParameter (handle, AudioQueueParameter.Pan, value);
				if (res != 0)
					throw new AudioQueueException (res);
			}
		}

		Hashtable? listeners;

		[UnmanagedCallersOnly]
		static void property_changed (IntPtr userData, IntPtr AQ, AudioQueueProperty id)
		{
			GCHandle gch = GCHandle.FromIntPtr (userData);
			var aq = gch.Target as AudioQueue;
			lock (aq!.listeners!) {
				ArrayList a = (ArrayList) aq.listeners [id]!;
				if (a is null)
					return;
				foreach (AudioQueuePropertyChanged cback in a) {
					cback (id);
				}
			}
		}

		/// <param name="property">To be added.</param>
		///     <summary>The delegate to be used with the <see cref="AudioToolbox.AudioQueue.AddListener(AudioToolbox.AudioQueueProperty,AudioToolbox.AudioQueue.AudioQueuePropertyChanged)" /> and <see cref="AudioToolbox.AudioQueue.RemoveListener(AudioToolbox.AudioQueueProperty,AudioToolbox.AudioQueue.AudioQueuePropertyChanged)" /> methods.</summary>
		///     <remarks>To be added.</remarks>
		public delegate void AudioQueuePropertyChanged (AudioQueueProperty property);

		/// <param name="property">ID of the property to listen to.</param>
		///         <param name="callback">The method to invoke when the specified AudioQueue property changes.</param>
		///         <summary>Use this method to track changes to the audio queue properties.</summary>
		///         <returns>Status code.</returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus AddListener (AudioQueueProperty property, AudioQueuePropertyChanged callback)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));
			if (listeners is null)
				listeners = new Hashtable ();

			AudioQueueStatus res = AudioQueueStatus.Ok;
			lock (listeners) {
				var a = (ArrayList) listeners [property]!;
				if (a is null) {
					unsafe {
						res = AudioQueueAddPropertyListener (handle, property, &property_changed, GCHandle.ToIntPtr (gch));
					}
					if (res != AudioQueueStatus.Ok)
						return res;

					listeners [property] = a = new ArrayList ();
				}
				a.Add (callback);
			}

			return res;
		}

		/// <param name="property">To be added.</param>
		///         <param name="callback">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void RemoveListener (AudioQueueProperty property, AudioQueuePropertyChanged callback)
		{
			if (callback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (callback));
			if (listeners is null)
				return;
			lock (listeners) {
				var a = (ArrayList) listeners [property]!;
				if (a is null)
					return;
				a.Remove (callback);
				if (a.Count == 0) {
					unsafe {
						AudioQueueRemovePropertyListener (handle, property, &property_changed, GCHandle.ToIntPtr (gch));
					}
				}
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static AudioQueueStatus AudioQueueAddPropertyListener (IntPtr AQ, AudioQueueProperty id, delegate* unmanaged<IntPtr, IntPtr, AudioQueueProperty, void> proc, IntPtr data);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static OSStatus AudioQueueRemovePropertyListener (IntPtr AQ, AudioQueueProperty id, delegate* unmanaged<IntPtr, IntPtr, AudioQueueProperty, void> proc, IntPtr data);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueGetProperty (IntPtr AQ, uint id, IntPtr outdata, int* dataSize);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueGetPropertySize (IntPtr AQ, uint id, int* size);

		unsafe static AudioQueueStatus AudioQueueGetPropertySize (IntPtr AQ, uint id, out int size)
		{
			size = 0;
			return AudioQueueGetPropertySize (AQ, id, AsPointer<int> (ref size));
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueSetProperty (
			IntPtr AQ, AudioQueueProperty id, IntPtr data, int size);

		// Should be private
		/// <param name="property">Property ID to retrieve.</param>
		///         <param name="dataSize">Expected size of the property.</param>
		///         <param name="outdata">Pointers to the data.</param>
		///         <summary>Low-level API to fetch AudioQueue properties. </summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		/// 	  MonoTouch provides a high-level interface to the AudioQueue
		/// 	  properties.  This API is here in case a new property is
		/// 	  added and you have not updated your code to the latest
		/// 	  version of MonoTouch.
		/// 	</remarks>
		public bool GetProperty (AudioQueueProperty property, ref int dataSize, IntPtr outdata)
		{
			if (outdata == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (outdata));
			unsafe {
				return AudioQueueGetProperty (handle, (uint) property, outdata, AsPointer<int> (ref dataSize)) == 0;
			}
		}

		// Should be private
		/// <param name="property">To be added.</param>
		///         <param name="dataSize">To be added.</param>
		///         <param name="propertyData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SetProperty (AudioQueueProperty property, int dataSize, IntPtr propertyData)
		{
			if (propertyData == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (propertyData));
			return AudioQueueSetProperty (handle, property, propertyData, dataSize) == 0;
		}

		// Should be private
		/// <param name="property">Property ID to retrieve.</param>
		///         <param name="size">Expected size of the property.</param>
		///         <summary>Low-level API to fetch AudioQueue properties. </summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		/// 	  MonoTouch provides a high-level interface to the AudioQueue
		/// 	  properties.  This API is here in case a new property is
		/// 	  added and you have not updated your code to the latest
		/// 	  version of MonoTouch.
		/// 	</remarks>
		public IntPtr GetProperty (AudioQueueProperty property, out int size)
		{
			var r = AudioQueueGetPropertySize (handle, (uint) property, out size);
			if (r != 0)
				throw new AudioQueueException (r);

			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return IntPtr.Zero;

			unsafe {
				r = AudioQueueGetProperty (handle, (uint) property, buffer, AsPointer<int> (ref size));
			}
			if (r == 0)
				return buffer;
			Marshal.FreeHGlobal (buffer);
			return IntPtr.Zero;
		}

		// Should be private
		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="property">Property ID to retrieve.</param>
		///         <summary>Low-level API to fetch AudioQueue properties. </summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    This version returns the value of the property based on the provided generic type.
		/// 	  </para>
		///           <para>
		/// 	    MonoTouch provides a high-level interface to the AudioQueue
		/// 	    properties.  This API is here in case a new property is
		/// 	    added and you have not updated your code to the latest
		/// 	    version of MonoTouch.
		/// 	  </para>
		///         </remarks>
		public unsafe T GetProperty<[DynamicallyAccessedMembers (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T> (AudioQueueProperty property) where T : struct
		{
			int size;

			var r = AudioQueueGetPropertySize (handle, (uint) property, out size);
			if (r != 0)
				throw new AudioQueueException (r);

			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return default (T);
			try {
				r = AudioQueueGetProperty (handle, (uint) property, buffer, &size);
				if (r == 0) {
					T result = Marshal.PtrToStructure<T> (buffer)!;
					return result;
				}

				throw new AudioQueueException (r);
			} finally {
				Marshal.FreeHGlobal (buffer);
			}
		}

		unsafe T GetProperty<[DynamicallyAccessedMembers (DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T> (AudioConverterPropertyID property) where T : struct
		{
			int size;

			var r = AudioQueueGetPropertySize (handle, (uint) property, out size);
			if (r != 0)
				throw new AudioQueueException (r);

			var buffer = Marshal.AllocHGlobal (size);
			if (buffer == IntPtr.Zero)
				return default (T);
			try {
				r = AudioQueueGetProperty (handle, (uint) property, buffer, &size);
				if (r == 0) {
					T result = Marshal.PtrToStructure<T> (buffer)!;
					return result;
				}

				throw new AudioQueueException (r);
			} finally {
				Marshal.FreeHGlobal (buffer);
			}
		}

		int GetInt (AudioQueueProperty property)
		{
			unsafe {
				int val = 0;
				int size = 4;
				var k = AudioQueueGetProperty (handle, (uint) property, (IntPtr) (&val), &size);
				if (k == 0)
					return val;
				throw new AudioQueueException (k);
			}
		}

		int SetInt (AudioQueueProperty property, int val)
		{
			unsafe {
				int size = 4;
				var k = AudioQueueSetProperty (handle, property, (IntPtr) (&val), size);
				if (k == 0)
					return val;
				throw new AudioQueueException (k);
			}
		}

		double GetDouble (AudioQueueProperty property)
		{
			unsafe {
				double val = 0;
				int size = 8;
				var k = AudioQueueGetProperty (handle, (uint) property, (IntPtr) (&val), &size);
				if (k == 0)
					return val;
				throw new AudioQueueException (k);

			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsRunning {
			get {
				return GetInt (AudioQueueProperty.IsRunning) != 0;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double SampleRate {
			get {
				return GetDouble (AudioQueueProperty.DeviceSampleRate);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int DeviceChannels {
			get {
				return GetInt (AudioQueueProperty.DeviceNumberChannels);
			}
		}

		/// <summary>Unique identifier for the device associated with this Audio Queue.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string? CurrentDevice {
			get {
				return CFString.FromHandle ((IntPtr) GetInt (AudioQueueProperty.CurrentDevice));
			}

			set {
				// TODO
				throw new NotImplementedException ();
			}
		}

#pragma warning disable 612

		/// <summary>Audio metadata required by certain formats.</summary>
		///         <value>Opaque byte array with a codec-specific token.</value>
		///         <remarks>
		///
		/// 	  Certain file format produce a MagicCookie that
		/// 	  contains audio metadata.  When playing back or recording, you need to copy
		/// 	  this magic cookie from the AudioQueue to the <see cref="AudioToolbox.AudioFileStream" /> by copying this property to the 
		/// 	  <see cref="AudioToolbox.AudioFileStream.MagicCookie" />
		/// 	  property.   
		/// 	</remarks>
		public byte [] MagicCookie {
			get {
				int size;
				var h = GetProperty (AudioQueueProperty.MagicCookie, out size);
				if (h == IntPtr.Zero)
					return Array.Empty<byte> ();

				byte [] cookie = new byte [size];
				Marshal.Copy (h, cookie, 0, size);
				Marshal.FreeHGlobal (h);

				return cookie;
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				if (value.Length == 0)
					return;

				unsafe {
					fixed (byte* bp = value) {
						SetProperty (AudioQueueProperty.MagicCookie, value.Length, (IntPtr) bp);
					}
				}
			}
		}

		/// <summary>The audio queue channel layout.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>
		/// 	    The ChannelLayout must match the number of channels in the
		/// 	    audio format. 
		/// 	  </para>
		///           <para>
		/// 	    This property is used to configure the channel layout in
		/// 	    complex scenarios like 5.1 surround sound.
		/// 	  </para>
		///         </remarks>
		public AudioChannelLayout? ChannelLayout {
			get {
				int size;
				var h = GetProperty (AudioQueueProperty.ChannelLayout, out size);
				if (h == IntPtr.Zero)
					return null;

				var layout = AudioChannelLayout.FromHandle (h);
				Marshal.FreeHGlobal (h);

				return layout;
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value)); // TODO: enable ?

				int size;
				var h = value.ToBlock (out size);
				SetProperty (AudioQueueProperty.ChannelLayout, size, h);
				Marshal.FreeHGlobal (h);
			}
		}

		/// <summary>Enables level metering on the audio queue.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		/// 	    Enable level metering in your audio queue if you want to read the level meters with the <see cref="AudioToolbox.AudioQueue.CurrentLevelMeter" /> or <see cref="AudioToolbox.AudioQueue.CurrentLevelMeterDB" /> properties.n
		/// 	  </para>
		///         </remarks>
		public bool EnableLevelMetering {
			get {
				return GetInt (AudioQueueProperty.EnableLevelMetering) != 0;
			}
			set {
				SetInt (AudioQueueProperty.EnableLevelMetering, value ? 1 : 0);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int MaximumOutputPacketSize {
			get {
				return GetInt (AudioQueueProperty.MaximumOutputPacketSize);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int DecodeBufferSizeFrames {
			get {
				return GetInt (AudioQueueProperty.DecodeBufferSizeFrames);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioStreamBasicDescription AudioStreamDescription {
			get {
#if !MONOMAC
				return GetProperty<AudioStreamBasicDescription> (AudioQueueProperty.StreamDescription);
#else
				return GetProperty<AudioStreamBasicDescription> (AudioConverterPropertyID.CurrentInputStreamDescription);
#endif
			}
		}

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='P:AudioToolbox.AudioQueue.CurrentLevelMeter']/*" />
		public AudioQueueLevelMeterState [] CurrentLevelMeter {
			get {
				unsafe {
					int size = DeviceChannels * sizeof (AudioQueueLevelMeterState);
					int n;
					var buffer = GetProperty (AudioQueueProperty.CurrentLevelMeter, out n);
					if (buffer == IntPtr.Zero)
						return new AudioQueueLevelMeterState [0];
					var ret = new AudioQueueLevelMeterState [n];
					AudioQueueLevelMeterState* ptr = (AudioQueueLevelMeterState*) buffer;
					for (int i = 0; i < n; i++)
						ret [i] = ptr [i];
					return ret;
				}
			}
		}

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='P:AudioToolbox.AudioQueue.CurrentLevelMeterDB']/*" />
		public AudioQueueLevelMeterState [] CurrentLevelMeterDB {
			get {
				unsafe {
					int size = DeviceChannels * sizeof (AudioQueueLevelMeterState);
					int n;
					var buffer = GetProperty (AudioQueueProperty.CurrentLevelMeterDB, out n);
					if (buffer == IntPtr.Zero)
						return new AudioQueueLevelMeterState [0];
					var ret = new AudioQueueLevelMeterState [n];
					AudioQueueLevelMeterState* ptr = (AudioQueueLevelMeterState*) buffer;
					for (int i = 0; i < n; i++)
						ret [i] = ptr [i];
					return ret;
				}
			}
		}

#pragma warning restore 612

		/// <summary>Contains the most recent error generated in the audio queue's encoding or decoding process.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public uint ConverterError {
			get {
				return (uint) GetInt (AudioQueueProperty.ConverterError);
			}
		}

#if !MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioQueueHardwareCodecPolicy HardwareCodecPolicy {
			get {
				return (AudioQueueHardwareCodecPolicy) GetInt (AudioQueueProperty.HardwareCodecPolicy);
			}
			set {
				SetInt (AudioQueueProperty.HardwareCodecPolicy, (int) value);
			}
		}

		/// <param name="channelAssignments">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioQueueStatus SetChannelAssignments (params AudioQueueChannelAssignment [] channelAssignments)
		{
			if (channelAssignments is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (channelAssignments));

			int length;
			var ptr = MarshalArray (ref channelAssignments, out length);
			try {
				return AudioQueueSetProperty (handle, AudioQueueProperty.ChannelAssignments, ptr, length);
			} finally {
				Marshal.FreeHGlobal (ptr);
			}
		}

		unsafe static IntPtr MarshalArray (ref AudioQueueChannelAssignment [] array, out int totalSize)
		{
			int elementSize = sizeof (AudioQueueChannelAssignment);
			totalSize = elementSize * array.Length;
			var array_ptr = (AudioQueueChannelAssignment*) Marshal.AllocHGlobal (totalSize);

			for (int i = 0; i < array.Length; i++)
				array_ptr [i] = array [i];

			return (IntPtr) array_ptr;
		}
#endif

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static AudioQueueStatus AudioQueueProcessingTapNew (IntPtr inAQ, delegate* unmanaged<IntPtr, IntPtr, uint, AudioTimeStamp*, AudioQueueProcessingTapFlags*, uint*, IntPtr, void> inCallback,
			IntPtr inClientData, AudioQueueProcessingTapFlags inFlags, uint* outMaxFrames,
			AudioStreamBasicDescription* outProcessingFormat, IntPtr* outAQTap);

		/// <include file="../../docs/api/AudioToolbox/AudioQueue.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueue.CreateProcessingTap(AudioToolbox.AudioQueueProcessingTapDelegate,AudioToolbox.AudioQueueProcessingTapFlags,AudioToolbox.AudioQueueStatus@)']/*" />
		public AudioQueueProcessingTap? CreateProcessingTap (AudioQueueProcessingTapDelegate processingCallback, AudioQueueProcessingTapFlags flags,
																out AudioQueueStatus status)
		{
			var aqpt = new AudioQueueProcessingTap (processingCallback);
			uint maxFrames;
			AudioStreamBasicDescription processingFormat;
			IntPtr tapHandle;

			unsafe {
				status = AudioQueueProcessingTapNew (handle, &AudioQueueProcessingTap.TapCallback, GCHandle.ToIntPtr (aqpt.Handle), flags, &maxFrames,
						 &processingFormat, &tapHandle);
			}

			if (status != AudioQueueStatus.Ok) {
				aqpt.Dispose ();
				return null;
			}

			aqpt.TapHandle = tapHandle;
			aqpt.MaxFrames = maxFrames;
			aqpt.ProcessingFormat = processingFormat;
			return aqpt;
		}

		internal unsafe static T* AsPointer<T> (ref T value) where T : unmanaged
		{
			return (T*) Unsafe.AsPointer<T> (ref value);
		}
	}

	/// <include file="../../docs/api/AudioToolbox/AudioQueueProcessingTapDelegate.xml" path="/Documentation/Docs[@DocId='T:AudioToolbox.AudioQueueProcessingTapDelegate']/*" />
	public delegate uint AudioQueueProcessingTapDelegate (AudioQueueProcessingTap audioQueueTap, uint numberOfFrames,
														  ref AudioTimeStamp timeStamp, ref AudioQueueProcessingTapFlags flags,
														  AudioBuffers data);

	/// <summary>Holds the state for an AudioQueue processing tap.</summary>
	///     <remarks>
	///       <para>
	/// 	Instances of this class are returned by the <see cref="AudioToolbox.AudioQueue.CreateProcessingTap(AudioToolbox.AudioQueueProcessingTapDelegate,AudioToolbox.AudioQueueProcessingTapFlags,out AudioToolbox.AudioQueueStatus)" />
	/// 	from AudioQueue and hold the state to the audio processing tap that was created as well as containing information like MaxFrames and the ProcessingFormat.
	///       </para>
	///       <para>
	/// 	You can terminate the processing tap by calling the Dispose
	/// 	method or by releasing the AudioQueue that created it.
	///       </para>
	///       <example>
	///         <code lang="c#">
	///         </code>
	///       </example>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioQueueProcessingTap : IDisposable {
		AudioQueueProcessingTapDelegate? callback;
		readonly GCHandle gc_handle;

		internal AudioQueueProcessingTap (AudioQueueProcessingTapDelegate callback)
		{
			this.callback = callback;
			gc_handle = GCHandle.Alloc (this);
		}

		~AudioQueueProcessingTap ()
		{
			Dispose (false);
		}

		internal GCHandle Handle {
			get {
				return gc_handle;
			}
		}

		internal IntPtr TapHandle { get; set; }
		/// <summary>Maximum number of sample frames per tap.</summary>
		///         <value>
		///         </value>
		///         <remarks>Apple lists this as being "50 msec of audio, 2048 samples at 44.1Khz".</remarks>
		public uint MaxFrames { get; internal set; }
		/// <summary>The format of the data that will be provided to the tap render function.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public AudioStreamBasicDescription ProcessingFormat { get; internal set; }

		/// <summary>Releases the resources used by the AudioQueueProcessingTap object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioQueueProcessingTap class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioQueueProcessingTap ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/AudioToolbox/AudioQueueProcessingTap.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueueProcessingTap.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (disposing) {
				callback = null;
			}

			if (gc_handle.IsAllocated) {
				gc_handle.Free ();
				AudioQueueProcessingTapDispose (TapHandle);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static OSStatus AudioQueueProcessingTapDispose (IntPtr inAQTap);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueProcessingTapGetSourceAudio (IntPtr inAQTap, uint inNumberFrames, AudioTimeStamp* ioTimeStamp,
																	   AudioQueueProcessingTapFlags* outFlags, uint* outNumberFrames,
																	   IntPtr ioData);

		/// <include file="../../docs/api/AudioToolbox/AudioQueueProcessingTap.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueueProcessingTap.GetSourceAudio(System.UInt32,AudioToolbox.AudioTimeStamp@,AudioToolbox.AudioQueueProcessingTapFlags@,System.UInt32@,AudioToolbox.AudioBuffers)']/*" />
		public AudioQueueStatus GetSourceAudio (uint numberOfFrames, ref AudioTimeStamp timeStamp,
													out AudioQueueProcessingTapFlags flags, out uint parentNumberOfFrames, AudioBuffers data)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));

			parentNumberOfFrames = 0;
			flags = default (AudioQueueProcessingTapFlags);
			unsafe {
				return AudioQueueProcessingTapGetSourceAudio (TapHandle, numberOfFrames,
															 AudioQueue.AsPointer<AudioTimeStamp> (ref timeStamp),
															 AudioQueue.AsPointer<AudioQueueProcessingTapFlags> (ref flags),
															 AudioQueue.AsPointer<uint> (ref parentNumberOfFrames),
															 (IntPtr) data);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueProcessingTapGetQueueTime (IntPtr inAQTap, double* outQueueSampleTime, uint* outQueueFrameCount);

		/// <param name="sampleTime">Returns the sample time for the output queue.</param>
		///         <param name="frameCount">Frame count for the audio being processed by the tap..</param>
		///         <summary>Get the current Queue Time.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		/// 	    This method should only be called from the AudioProcessingTap callback.
		/// 	  </para>
		///         </remarks>
		public AudioQueueStatus GetQueueTime (out double sampleTime, out uint frameCount)
		{
			sampleTime = 0;
			frameCount = 0;
			unsafe {
				return AudioQueueProcessingTapGetQueueTime (TapHandle, AudioQueue.AsPointer<double> (ref sampleTime), AudioQueue.AsPointer<uint> (ref frameCount));
			}
		}

		[UnmanagedCallersOnly]
		internal unsafe static void TapCallback (IntPtr clientData, IntPtr tap,
			uint numberFrames, AudioTimeStamp* timeStamp,
			AudioQueueProcessingTapFlags* flags, uint* outNumberFrames,
			IntPtr data)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var aqpt = (AudioQueueProcessingTap) gch.Target!;

			using (var buffers = new AudioBuffers (data)) {
				var localTimeStamp = *timeStamp;
				var localFlags = *flags;
				*outNumberFrames = aqpt.callback! (aqpt, numberFrames, ref localTimeStamp, ref localFlags, buffers);
				*timeStamp = localTimeStamp;
				*flags = localFlags;
			}
		}
	}

	/// <summary>The output AudioQueue.</summary>
	///     <remarks>
	///       <para>Use this class to playback audio.</para>
	///       <para>
	/// You will usually create an OutputAudioQueue instance and allocate a number of buffers that you will use to fill in with data.   Once a buffer is filled, the buffer is enqueued and when the OutputAudioQueue has finished playing it back, the OutputCompleted event will be raised.
	/// </para>
	///       <para>
	/// See the StreamingAudio sample program in monotouch-samples for an example program.
	/// </para>
	///     </remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/StreamingAudio/">StreamingAudio</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class OutputAudioQueue : AudioQueue {
		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static OSStatus AudioQueueNewOutput (AudioStreamBasicDescription* format, delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> callback,
			IntPtr userData, IntPtr cfrunLoop_callbackRunloop, IntPtr cfstr_runMode,
			uint flags, IntPtr* audioQueue);

		[UnmanagedCallersOnly]
		static void output_callback (IntPtr userData, IntPtr AQ, IntPtr audioQueueBuffer)
		{
			GCHandle gch = GCHandle.FromIntPtr (userData);
			var aq = gch.Target as OutputAudioQueue;
			aq!.OnBufferCompleted (audioQueueBuffer);
		}

		public event EventHandler<BufferCompletedEventArgs>? BufferCompleted;

		/// <param name="audioQueueBuffer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected virtual void OnBufferCompleted (IntPtr audioQueueBuffer)
		{
			var h = BufferCompleted;
			if (h is not null)
				h (this, new BufferCompletedEventArgs (audioQueueBuffer));
		}

		/// <param name="desc">Stream description.</param>
		///         <summary>Creates an OutputAudioQueue.</summary>
		///         <remarks>Usually the stream description is fetched from an AudioFile or an AudioStreamFile</remarks>
		public OutputAudioQueue (AudioStreamBasicDescription desc) : this (desc, null, (CFString) null!)
		{
		}

		/// <param name="desc">Stream description.</param>
		///         <param name="runLoop">The run loop in which the OnOutputCompleted method and the OutputCompleted event are raised, if you pass null, this uses an internal thread.</param>
		///         <param name="runMode">The run mode for the run loop.</param>
		///         <summary>Creates an OutputAudioQueue, specifying on which run loop events are delivered.</summary>
		///         <remarks>Usually the stream description is fetched from an AudioFile or an AudioStreamFile.</remarks>
		public OutputAudioQueue (AudioStreamBasicDescription desc, CFRunLoop runLoop, string runMode)
			: this (desc, runLoop, runMode is null ? null : new CFString (runMode))
		{
		}

		/// <param name="desc">Stream description.</param>
		///         <param name="runLoop">The run loop in which the OnOutputCompleted method and the OutputCompleted event are raised, if you pass null, this uses an internal thread.</param>
		///         <param name="runMode">The run mode for the run loop.</param>
		///         <summary>Creates an OutputAudioQueue, specifying on which run loop events are delivered.</summary>
		///         <remarks>Usually the stream description is fetched from an AudioFile or an AudioStreamFile.</remarks>
		public OutputAudioQueue (AudioStreamBasicDescription desc, CFRunLoop? runLoop, CFString? runMode)
		{
			IntPtr h;
			GCHandle gch = GCHandle.Alloc (this);

			OSStatus code = 0;
			unsafe {
				code = AudioQueueNewOutput (&desc,
					&output_callback,
					GCHandle.ToIntPtr (gch),
					runLoop.GetHandle (),
					runMode.GetHandle (), 0, &h);
				GC.KeepAlive (runLoop);
				GC.KeepAlive (runMode);
			}

			if (code != 0) {
				gch.Free ();
				throw new AudioQueueException (code);
			}

			this.gch = gch;
			handle = h;
		}

		[DllImport (Constants.AudioToolboxLibrary, EntryPoint = "AudioQueueSetOfflineRenderFormat")]
		extern static AudioQueueStatus AudioQueueSetOfflineRenderFormat2 (IntPtr aq, IntPtr format, IntPtr layout);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static AudioQueueStatus AudioQueueSetOfflineRenderFormat (IntPtr aq, AudioStreamBasicDescription* format, IntPtr layout);

		/// <param name="desc">The audio format to use for offline rendering.</param>
		///         <param name="layout">The channel layout to use for offline rendering. Optional.</param>
		///         <summary>Enables offline rendering by setting the audio format and optionally the channel layout to use when rendering.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error. </returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus SetOfflineRenderFormat (AudioStreamBasicDescription desc, AudioChannelLayout layout)
		{
			int size;
			var h = layout is null ? IntPtr.Zero : layout.ToBlock (out size);
			try {
				unsafe {
					return AudioQueueSetOfflineRenderFormat (handle, &desc, h);
				}
			} finally {
				Marshal.FreeHGlobal (h);
			}
		}

		/// <summary>Disables the offline renderer.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error. </returns>
		///         <remarks>
		///         </remarks>
		public AudioQueueStatus DisableOfflineRender ()
		{
			return AudioQueueSetOfflineRenderFormat2 (handle, IntPtr.Zero, IntPtr.Zero);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static AudioQueueStatus AudioQueueOfflineRender (IntPtr aq, AudioTimeStamp* stamp, AudioQueueBuffer* buffer, int frames);

		/// <param name="timeStamp">The timestamp of the first frame to render.</param>
		///         <param name="audioQueueBuffer">The audio queue buffer to render to.</param>
		///         <param name="frameCount">The number of frames to render.</param>
		///         <summary>Writes audio data to an audio buffer, instead of to a device.</summary>
		///         <returns>AudioQueueStatus.Ok on success, otherwise the error. </returns>
		///         <remarks>
		///         </remarks>
		public unsafe AudioQueueStatus RenderOffline (double timeStamp, AudioQueueBuffer* audioQueueBuffer, int frameCount)
		{
			if (audioQueueBuffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioQueueBuffer));

			var stamp = new AudioTimeStamp () {
				SampleTime = timeStamp,
				Flags = AudioTimeStamp.AtsFlags.SampleTimeValid,
			};
			return AudioQueueOfflineRender (handle, &stamp, audioQueueBuffer, frameCount);
		}
	}

	/// <summary>An Input Audio Queue, used for audio capturing and recording.</summary>
	///     <remarks>
	///
	///       To receive input completed notifications, you can either hook up
	///       to the C# event InputCompleted or you can subclass and override the 
	///       OnInputCompleted method.   They serve the same purpose.
	///
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class InputAudioQueue : AudioQueue {
		[UnmanagedCallersOnly]
		unsafe static void input_callback (IntPtr userData, IntPtr AQ, IntPtr audioQueueBuffer,
						AudioTimeStamp* startTime, int descriptors, IntPtr inPacketDesc)
		{
			GCHandle gch = GCHandle.FromIntPtr (userData);
			var aq = gch.Target as InputAudioQueue;

			aq!.OnInputCompleted (audioQueueBuffer, *startTime, AudioFile.PacketDescriptionFrom (descriptors, inPacketDesc));
		}

		public event EventHandler<InputCompletedEventArgs>? InputCompleted;
		/// <param name="audioQueueBuffer">.</param>
		///         <param name="timeStamp">To be added.</param>
		///         <param name="packetDescriptions">To be added.</param>
		///         <summary>Method invoked .</summary>
		///         <remarks>
		///           <para>
		/// 	    This method is invoked when the audio system has
		/// 	    completely filled one of the buffers with audio data.  You
		/// 	    would override this method to process the data, to either
		/// 	    save the raw bytes to disk, encode them using the <see cref="AudioToolbox.AudioFile" /> or do some
		/// 	    real-time processing with the audio packets.  
		/// 	  </para>
		///           <para>
		/// 	    If you override this method, you do not necessarily
		/// 	    need to call base.OnInputComplete (audioQueueBuffer,
		/// 	    timeStamp, packetDescriptions) unless you are interested in
		/// 	    raising the C# events to potential consumers of your class.
		/// 	  </para>
		///         </remarks>
		protected virtual void OnInputCompleted (IntPtr audioQueueBuffer, AudioTimeStamp timeStamp, AudioStreamPacketDescription []? packetDescriptions)
		{
			var h = InputCompleted;
			if (h is not null)
				h (this, new InputCompletedEventArgs (audioQueueBuffer, timeStamp, packetDescriptions));
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern unsafe static OSStatus AudioQueueNewInput (
			AudioStreamBasicDescription* format,
			delegate* unmanaged<IntPtr, IntPtr, IntPtr, AudioTimeStamp*, int, IntPtr, void> callback,
			IntPtr inUserData,
			IntPtr cfrunLoop_inCallbackRunLoop,
			IntPtr cfstringref_inCallbackRunLoopMode,
			UInt32 inFlags,
			IntPtr* audioQueue);

		/// <param name="desc">Audio stream description.</param>
		///         <summary>Creates an AudioQueue for recording, and invokes the notification callback on an internal AudioQueue thread.</summary>
		///         <remarks>
		///         </remarks>
		public InputAudioQueue (AudioStreamBasicDescription desc)
		 : this (desc, null, null)
		{
		}

		/// <param name="desc">Audio stream description.</param>
		///         <param name="runLoop">If you specify null, InputAudioQueue will invoke the callback on an internal thread.   Otherwise the callback will be invoked on the specified runLoop thread.</param>
		///         <param name="runMode">The run mode for the run loop.</param>
		///         <summary>Creates an AudioQueue for recording, specifying on which run loop events are delivered.</summary>
		///         <remarks>
		///         </remarks>
		public InputAudioQueue (AudioStreamBasicDescription desc, CFRunLoop? runLoop, string? runMode)
		{
			IntPtr h;
			GCHandle mygch = GCHandle.Alloc (this);
			CFString? s = runMode is null ? null : new CFString (runMode);

			OSStatus code = 0;
			unsafe {
				code = AudioQueueNewInput (&desc, &input_callback, GCHandle.ToIntPtr (mygch),
					runLoop.GetHandle (), s.GetHandle (),
					0, &h);
				GC.KeepAlive (runLoop);
				GC.KeepAlive (s);
			}
			if (s is not null)
				s.Dispose ();

			if (code == 0) {
				handle = h;
				gch = mygch;
				return;
			}
			mygch.Free ();
			throw new AudioQueueException (code);
		}

		/// <param name="buffer">The buffer to add.</param>
		///         <summary>Adds the specified buffer to the queue.</summary>
		///         <returns>Status code</returns>
		///         <remarks>This is equivalent to calling EnqueueBuffer with an empty AudioStreamPacketDescription (for example, for constant bit rates, or while recording).</remarks>
		public unsafe AudioQueueStatus EnqueueBuffer (AudioQueueBuffer* buffer)
		{
			return AudioQueueEnqueueBuffer (handle, buffer, 0, null);
		}
	}

	/// <summary>Objects used to track audio queue timelines</summary>
	///     <remarks>
	///       <para>
	/// 	This object is used to track discontinuities in the Audio Queue's audio.   
	///       </para>
	///       <para>
	/// 	You create these objects by calling <see cref="AudioToolbox.AudioQueue.CreateTimeline" />
	/// 	method and use them to probe audio discontinuities by calling
	/// 	the <see cref="AudioToolbox.AudioQueue.GetCurrentTime(AudioToolbox.AudioQueueTimeline,ref AudioToolbox.AudioTimeStamp,ref System.Boolean)" />
	/// 	method.
	///       </para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioQueueTimeline : IDisposable {
		/// <summary>Handle to the underlying C timeline object.</summary>
		///         <remarks>
		///         </remarks>
		internal protected IntPtr timelineHandle;
		/// <summary>The handle to the underlying C queue.</summary>
		///         <remarks>
		///         </remarks>
		internal protected IntPtr queueHandle;

		internal AudioQueueTimeline (IntPtr queueHandle, IntPtr timelineHandle)
		{
			this.queueHandle = queueHandle;
			this.timelineHandle = timelineHandle;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static AudioQueueStatus AudioQueueDisposeTimeline (IntPtr AQ, IntPtr timeline);

		/// <summary>Releases the resources used by the AudioQueueTimeline object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AudioQueueTimeline class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AudioQueueTimeline ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
		}

		/// <include file="../../docs/api/AudioToolbox/AudioQueueTimeline.xml" path="/Documentation/Docs[@DocId='M:AudioToolbox.AudioQueueTimeline.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (timelineHandle != IntPtr.Zero) {
				AudioQueueDisposeTimeline (queueHandle, timelineHandle);
				timelineHandle = IntPtr.Zero;
			}
			GC.SuppressFinalize (this);
		}

		~AudioQueueTimeline ()
		{
			Dispose (false);
		}
	}
}
