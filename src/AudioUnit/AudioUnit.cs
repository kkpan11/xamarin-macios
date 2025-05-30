//
// AudioUnit.cs: AudioUnit wrapper class
//
// Authors:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2011-2013 Xamarin Inc
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using AudioToolbox;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace AudioUnit {
#if !COREBUILD
	/// <summary>An exception relating to functions in the MonoTouch.AudioUnit namespace.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioUnitException : Exception {
		static string Lookup (int k)
		{
			switch ((AudioUnitStatus) k) {
			case AudioUnitStatus.InvalidProperty:
				return "Invalid Property";

			case AudioUnitStatus.InvalidParameter:
				return "Invalid Parameter";

			case AudioUnitStatus.InvalidElement:
				return "Invalid Element";

			case AudioUnitStatus.NoConnection:
				return "No Connection";

			case AudioUnitStatus.FailedInitialization:
				return "Failed Initialization";

			case AudioUnitStatus.TooManyFramesToProcess:
				return "Too Many Frames To Process";

			case AudioUnitStatus.InvalidFile:
				return "Invalid File";

			case AudioUnitStatus.FormatNotSupported:
				return "Format Not Supported";

			case AudioUnitStatus.Uninitialized:
				return "Uninitialized";

			case AudioUnitStatus.InvalidScope:
				return "Invalid Scope";

			case AudioUnitStatus.PropertyNotWritable:
				return "Property Not Writable";

			case AudioUnitStatus.CannotDoInCurrentContext:
				return "Cannot Do In Current Context";

			case AudioUnitStatus.InvalidPropertyValue:
				return "Invalid Property Value";

			case AudioUnitStatus.PropertyNotInUse:
				return "Property Not In Use";

			case AudioUnitStatus.Initialized:
				return "Initialized";

			case AudioUnitStatus.InvalidOfflineRender:
				return "Invalid Offline Render";

			case AudioUnitStatus.Unauthorized:
				return "Unauthorized";

			}
			return String.Format ("Unknown error code: 0x{0:x}", k);
		}

		internal AudioUnitException (AudioUnitStatus status)
			: this ((int) status)
		{
		}

		internal AudioUnitException (int k) : base (Lookup (k))
		{
		}
	}

	/// <include file="../../docs/api/AudioUnit/RenderDelegate.xml" path="/Documentation/Docs[@DocId='T:AudioUnit.RenderDelegate']/*" />
	public delegate AudioUnitStatus RenderDelegate (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data);
	/// <param name="actionFlags">To be added.</param>
	///     <param name="timeStamp">To be added.</param>
	///     <param name="busNumber">To be added.</param>
	///     <param name="numberFrames">To be added.</param>
	///     <param name="audioUnit">To be added.</param>
	///     <summary>Callback used with <see cref="AudioUnit.SetInputCallback(InputDelegate,AudioUnitScopeType,System.UInt32)" />.</summary>
	///     <returns>To be added.</returns>
	///     <remarks>To be added.</remarks>
	public delegate AudioUnitStatus InputDelegate (AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioUnit audioUnit);

	delegate AudioUnitStatus CallbackShared (IntPtr /* void* */ clientData, ref AudioUnitRenderActionFlags /* AudioUnitRenderActionFlags* */ actionFlags, ref AudioTimeStamp /* AudioTimeStamp* */ timeStamp, uint /* UInt32 */ busNumber, uint /* UInt32 */ numberFrames, IntPtr /* AudioBufferList* */ data);
#endif // !COREBUILD

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AURenderCallbackStruct {
#if COREBUILD
		public delegate* unmanaged<IntPtr, int*, AudioTimeStamp*, uint, uint, IntPtr, int> Proc;
#else
		public delegate* unmanaged<IntPtr, AudioUnitRenderActionFlags*, AudioTimeStamp*, uint, uint, IntPtr, AudioUnitStatus> Proc;
#endif
		public IntPtr ProcRefCon;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AudioUnitConnection {
		public IntPtr SourceAudioUnit;
		public uint /* UInt32 */ SourceOutputNumber;
		public uint /* UInt32 */ DestInputNumber;
	}

	/// <summary>Describes a sampler instrument. Used with <see cref="AudioUnit.LoadInstrument(SamplerInstrumentData,AudioUnitScopeType,System.UInt32)" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class SamplerInstrumentData {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public const byte DefaultPercussionBankMSB = 0x78;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public const byte DefaultMelodicBankMSB = 0x79;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public const byte DefaultBankLSB = 0x00;

		/// <param name="fileUrl">To be added.</param>
		///         <param name="instrumentType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SamplerInstrumentData (CFUrl fileUrl, InstrumentType instrumentType)
		{
			if (fileUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fileUrl));

			this.FileUrl = fileUrl;
			this.InstrumentType = instrumentType;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CFUrl FileUrl { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public InstrumentType InstrumentType { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public byte BankMSB { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public byte BankLSB { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public byte PresetID { get; set; }

		internal AUSamplerInstrumentData ToStruct ()
		{
			var data = new AUSamplerInstrumentData ();
			data.FileUrl = FileUrl.Handle;
			data.InstrumentType = InstrumentType;
			data.BankMSB = BankMSB;
			data.BankLSB = BankLSB;
			data.PresetID = PresetID;
			return data;
		}
#endif // !COREBUILD
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AUSamplerInstrumentData {
		public IntPtr FileUrl;
#if COREBUILD
		// keep structure size identical across builds
		public byte InstrumentType;
#else
		public InstrumentType InstrumentType;
#endif // !COREBUILD
		public byte BankMSB;
		public byte BankLSB;
		public byte PresetID;
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AudioUnitParameterInfoNative // AudioUnitParameterInfo in Obj-C
	{
		fixed byte /* char[52] */ name [52]; // unused
		public IntPtr /* CFStringRef */ UnitName;
#if COREBUILD
		// keep structure size identical across builds
		public uint ClumpID;
#else
		public AudioUnitClumpID /* UInt32 */ ClumpID;
#endif // !COREBUILD
		public IntPtr /* CFStringRef */ NameString;

#if COREBUILD
		// keep structure size identical across builds
		public uint Unit;
#else
		public AudioUnitParameterUnit /* AudioUnitParameterUnit */ Unit;
#endif // !COREBUILD
		public float /* AudioUnitParameterValue = Float32 */ MinValue;
		public float /* AudioUnitParameterValue = Float32 */ MaxValue;
		public float /* AudioUnitParameterValue = Float32 */ DefaultValue;
#if COREBUILD
		// keep structure size identical across builds
		public uint Flags;
#else
		public AudioUnitParameterFlag /* UInt32 */ Flags;
#endif // !COREBUILD
	}

	/// <summary>Holds information regarding an audio unit parameter.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioUnitParameterInfo {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? UnitName { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioUnitClumpID ClumpID { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioUnitParameterUnit Unit { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float MinValue { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float MaxValue { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float DefaultValue { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioUnitParameterFlag Flags { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioUnitParameterType Type { get; private set; }

		internal static AudioUnitParameterInfo Create (AudioUnitParameterInfoNative native, AudioUnitParameterType type)
		{
			var info = new AudioUnitParameterInfo ();

			// Remove obj-c noise
			info.Flags = native.Flags & ~(AudioUnitParameterFlag.HasCFNameString | AudioUnitParameterFlag.CFNameRelease);
			info.Unit = native.Unit;
			info.MinValue = native.MinValue;
			info.MaxValue = native.MaxValue;
			info.DefaultValue = native.DefaultValue;
			info.ClumpID = native.ClumpID;
			info.Type = type;

			if ((native.Flags & AudioUnitParameterFlag.HasCFNameString) != 0) {
				info.Name = CFString.FromHandle (native.NameString);

				if ((native.Flags & AudioUnitParameterFlag.CFNameRelease) != 0)
					CFObject.CFRelease (native.NameString);
			}

			if (native.Unit == AudioUnitParameterUnit.CustomUnit) {
				info.UnitName = CFString.FromHandle (native.UnitName);
			}

			return info;
		}
#endif // !COREBUILD
	}

	/// <summary>Enumerates types of audio unit parameter events.</summary>
	///     <remarks>To be added.</remarks>
	public enum AUParameterEventType : uint {
		/// <summary>Indicates an instantaneous, or step, change in a value.</summary>
		Immediate = 1,
		/// <summary>Indicates a linear ramp change in a value over time.</summary>
		Ramped = 2,
	}

	/// <summary>A change for an audio unit parameter.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AudioUnitParameterEvent {
		/// <summary>The changed parameter's audio unit scope.</summary>
		///         <remarks>To be added.</remarks>
		public uint Scope;
		/// <summary>The audio element whose parameter changed.</summary>
		///         <remarks>To be added.</remarks>
		public uint Element;
		/// <summary>The identifier for the changed parameter.</summary>
		///         <remarks>To be added.</remarks>
		public uint Parameter;
		/// <summary>Whether the change was a step change (immediate) or gradual linear change (ramped).</summary>
		///         <remarks>To be added.</remarks>
		public AUParameterEventType EventType;

		/// <summary>Contains structs for different types parameter change events.</summary>
		///     <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[StructLayout (LayoutKind.Explicit)]
		public struct EventValuesStruct {
			/// <summary>Contains values that describe a linear ramp change in a parameter value.</summary>
			///     <remarks>To be added.</remarks>
			[StructLayout (LayoutKind.Sequential)]
			public struct RampStruct {
				/// <summary>The offset into the frame buffer at which the change begins.</summary>
				///         <remarks>To be added.</remarks>
				public int StartBufferOffset;
				/// <summary>The number of frames over which the change occurs.</summary>
				///         <remarks>To be added.</remarks>
				public uint DurationInFrames;
				/// <summary>The start value of the parameter.</summary>
				///         <remarks>To be added.</remarks>
				public float StartValue;
				/// <summary>The final value of the parameter.</summary>
				///         <remarks>To be added.</remarks>
				public float EndValue;
			}


			/// <summary>A struct for describing events for linear ramp changes in parameters.</summary>
			///         <remarks>To be added.</remarks>
			[FieldOffset (0)]
			public RampStruct Ramp;

			/// <summary>Contains values that describe a step change in a parameter value.</summary>
			///     <remarks>To be added.</remarks>
			[StructLayout (LayoutKind.Sequential)]
			public struct ImmediateStruct {
				/// <summary>The offset into the frame buffer at which the change occurs.</summary>
				///         <remarks>To be added.</remarks>
				public uint BufferOffset;
				/// <summary>The new value of the parameter.</summary>
				///         <remarks>To be added.</remarks>
				public float Value;
			}

			/// <summary>A struct for describing events for step changes in parameters.</summary>
			///         <remarks>To be added.</remarks>
			[FieldOffset (0)]
			public ImmediateStruct Immediate;
		}

		/// <summary>The values that describe the change event for the parameter.</summary>
		///         <remarks>To be added.</remarks>
		public EventValuesStruct EventValues;
	}

	/// <summary>A plug-in component that processes or generates audio data.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioUnit : DisposableObject {
#if !COREBUILD
		GCHandle gcHandle;
		bool _isPlaying;

		Dictionary<uint, RenderDelegate>? renderer;
		Dictionary<uint, InputDelegate>? inputs;

		[Preserve (Conditional = true)]
		internal AudioUnit (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		static IntPtr Create (AudioComponent component)
		{
			if (component is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (component));

			IntPtr handle;
			AudioUnitStatus err;
			unsafe {
				err = AudioComponentInstanceNew (component.GetCheckedHandle (), &handle);
				GC.KeepAlive (component);
			}
			if (err != 0)
				throw new AudioUnitException (err);

			return handle;
		}

		/// <param name="component">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioUnit (AudioComponent component)
			: this (Create (component), true)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioComponent Component {
			get {
				return new AudioComponent (AudioComponentInstanceGetComponent (Handle), false);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPlaying { get { return _isPlaying; } }

		/// <param name="audioFormat">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioUnitStatus SetFormat (AudioToolbox.AudioStreamBasicDescription audioFormat, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			return (AudioUnitStatus) AudioUnitSetProperty (Handle,
							   AudioUnitPropertyIDType.StreamFormat,
							   scope,
							   audioUnitElement,
							   &audioFormat,
							   (uint) Marshal.SizeOf<AudioToolbox.AudioStreamBasicDescription> ());
		}

		/// <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public uint GetCurrentDevice (AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			uint device = 0;
			int size = sizeof (uint);
			AudioUnitStatus err;
			unsafe {
				err = AudioUnitGetProperty (Handle,
						AudioUnitPropertyIDType.CurrentDevice,
						scope,
						audioUnitElement,
						&device,
						&size);
			}
			if (err != 0)
				throw new AudioUnitException ((int) err);
			return device;
		}

#if MONOMAC || __MACCATALYST__
#if !MONOMAC && !__MACCATALYST__
		[Obsolete ("This API is not available on iOS.")]
#endif
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		public static uint GetCurrentInputDevice ()
		{
#if MONOMAC || __MACCATALYST__
			// We need to replace AudioHardwareGetProperty since it has been deprecated since OS X 10.6 and iOS 2.0
			// Replacing with the following implementation recommended in the following doc
			// See Listing 4  New - Getting the default input device.
			// https://developer.apple.com/library/mac/technotes/tn2223/_index.html

			uint inputDevice = 0;
			uint size = (uint) sizeof (uint);
			var theAddress = new AudioObjectPropertyAddress (
				AudioObjectPropertySelector.DefaultInputDevice,
				AudioObjectPropertyScope.Global,
				AudioObjectPropertyElement.Main);
			uint inQualifierDataSize = 0;
			IntPtr inQualifierData = IntPtr.Zero;

			int err;
			unsafe {
				err = AudioObjectGetPropertyData (1, &theAddress, &inQualifierDataSize, &inQualifierData, &size, &inputDevice);
			}

			if (err != 0)
				throw new AudioUnitException ((int) err);
			return inputDevice;
#endif
		}
#endif
		/// <param name="inputDevice">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioUnitStatus SetCurrentDevice (uint inputDevice, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			return AudioUnitSetProperty (Handle,
						AudioUnitPropertyIDType.CurrentDevice,
						scope,
						audioUnitElement,
						&inputDevice,
						(uint) sizeof (uint));
		}

		/// <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioStreamBasicDescription GetAudioFormat (AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			var audioFormat = new AudioStreamBasicDescription ();
			uint size = (uint) Marshal.SizeOf<AudioStreamBasicDescription> ();

			AudioUnitStatus err;
			unsafe {
				err = AudioUnitGetProperty (Handle,
							   AudioUnitPropertyIDType.StreamFormat,
							   scope,
							   audioUnitElement,
							   &audioFormat,
							   &size);
			}
			if (err != 0)
				throw new AudioUnitException ((int) err);

			return audioFormat;
		}

		/// <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public ClassInfoDictionary? GetClassInfo (AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			IntPtr ptr = new IntPtr ();
			int size = IntPtr.Size;
			AudioUnitStatus res;
			unsafe {
				res = AudioUnitGetProperty (Handle,
						AudioUnitPropertyIDType.ClassInfo,
						scope,
						audioUnitElement,
						&ptr,
						&size);
			}

			if (res != 0)
				return null;

			return new ClassInfoDictionary (Runtime.GetNSObject<NSDictionary> (ptr, true));
		}

		/// <param name="preset">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetClassInfo (ClassInfoDictionary preset, AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			var ptr = preset.Dictionary.Handle;
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.ClassInfo, scope, audioUnitElement,
					&ptr, IntPtr.Size);
			}
		}

		/// <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioUnitParameterInfo []? GetParameterList (AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			uint size;
			byte writable;
			if (AudioUnitGetPropertyInfo (Handle, AudioUnitPropertyIDType.ParameterList, scope, audioUnitElement, &size, &writable) != 0)
				return null;

			// Array of AudioUnitParameterID = UInt32
			var data = new uint [size / sizeof (uint)];
			fixed (uint* ptr = data) {
				if (AudioUnitGetProperty (Handle, AudioUnitPropertyIDType.ParameterList, scope, audioUnitElement, ptr, &size) != 0)
					return null;
			}

			var info = new AudioUnitParameterInfo [data.Length];
			size = (uint) sizeof (AudioUnitParameterInfoNative);

			for (int i = 0; i < data.Length; ++i) {
				var native = new AudioUnitParameterInfoNative ();
				if (AudioUnitGetProperty (Handle, AudioUnitPropertyIDType.ParameterInfo, scope, data [i], &native, &size) != 0)
					return null;

				info [i] = AudioUnitParameterInfo.Create (native, (AudioUnitParameterType) data [i]);
			}

			return info;
		}

		/// <param name="instrumentData">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus LoadInstrument (SamplerInstrumentData instrumentData, AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			if (instrumentData is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (instrumentData));

			var data = instrumentData.ToStruct ();
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.LoadInstrument, scope, audioUnitElement,
					&data, Marshal.SizeOf<AUSamplerInstrumentData> ());
			}
		}

		/// <param name="sourceAudioUnit">To be added.</param>
		///         <param name="sourceOutputNumber">To be added.</param>
		///         <param name="destInputNumber">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus MakeConnection (AudioUnit sourceAudioUnit, uint sourceOutputNumber, uint destInputNumber)
		{
			var auc = new AudioUnitConnection {
				SourceAudioUnit = sourceAudioUnit.GetHandle (),
				SourceOutputNumber = sourceOutputNumber,
				DestInputNumber = destInputNumber,
			};

			unsafe {
				AudioUnitStatus status = AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.MakeConnection, AudioUnitScopeType.Input, 0, &auc, Marshal.SizeOf<AudioUnitConnection> ());
				GC.KeepAlive (sourceAudioUnit);
				return status;
			}
		}

		/// <param name="enableIO">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetEnableIO (bool enableIO, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			// EnableIO: UInt32          
			uint flag = enableIO ? (uint) 1 : 0;
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.EnableIO, scope, audioUnitElement, &flag, sizeof (uint));
			}
		}

		/// <param name="value">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetMaximumFramesPerSlice (uint value, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			// MaximumFramesPerSlice: UInt32
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.MaximumFramesPerSlice, scope, audioUnitElement, &value, sizeof (uint));
			}
		}

		/// <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public uint GetMaximumFramesPerSlice (AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			// MaximumFramesPerSlice: UInt32
			uint value = 0;
			uint size = sizeof (uint);
			AudioUnitStatus res;
			unsafe {
				res = AudioUnitGetProperty (Handle,
						AudioUnitPropertyIDType.MaximumFramesPerSlice,
						scope,
						audioUnitElement,
						&value,
						&size);
			}

			if (res != 0)
				throw new AudioUnitException ((int) res);

			return value;
		}

		/// <param name="scope">To be added.</param>
		///         <param name="count">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetElementCount (AudioUnitScopeType scope, uint count)
		{
			// ElementCount: UInt32
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.ElementCount, scope, 0, &count, sizeof (uint));
			}
		}

		/// <param name="scope">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public uint GetElementCount (AudioUnitScopeType scope)
		{
			// ElementCount: UInt32
			uint value = 0;
			uint size = sizeof (uint);
			AudioUnitStatus res;
			unsafe {
				res = AudioUnitGetProperty (Handle,
						AudioUnitPropertyIDType.ElementCount,
						scope,
						0,
						&value,
						&size);
			}

			if (res != 0)
				throw new AudioUnitException ((int) res);

			return value;
		}

		/// <param name="sampleRate">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetSampleRate (double sampleRate, AudioUnitScopeType scope = AudioUnitScopeType.Output, uint audioUnitElement = 0)
		{
			// ElementCount: Float64
			unsafe {
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.SampleRate, scope, 0, &sampleRate, sizeof (double));
			}
		}

		/// <param name="status">To be added.</param>
		///         <param name="data1">To be added.</param>
		///         <param name="data2">To be added.</param>
		///         <param name="offsetSampleFrame">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus MusicDeviceMIDIEvent (uint status, uint data1, uint data2, uint offsetSampleFrame = 0)
		{
			return MusicDeviceMIDIEvent (Handle, status, data1, data2, offsetSampleFrame);
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement, double* outData, uint* ioDataSize);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public double GetLatency ()
		{
			uint size = sizeof (double);
			double latency = 0;
			AudioUnitStatus err;
			unsafe {
				err = AudioUnitGetProperty (Handle, AudioUnitPropertyIDType.Latency, AudioUnitScopeType.Global, 0, &latency, &size);
			}
			if (err != 0)
				throw new AudioUnitException ((int) err);
			return latency;
		}

		#region SetRenderCallback

		/// <param name="renderDelegate">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetRenderCallback (RenderDelegate renderDelegate, AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			if (renderer is null)
				Interlocked.CompareExchange (ref renderer, new Dictionary<uint, RenderDelegate> (), null);

			renderer [audioUnitElement] = renderDelegate;

			if (!gcHandle.IsAllocated)
				gcHandle = GCHandle.Alloc (this);

			var cb = new AURenderCallbackStruct ();
			unsafe {
				cb.Proc = &RenderCallbackImpl;
				cb.ProcRefCon = GCHandle.ToIntPtr (gcHandle);
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.SetRenderCallback, scope, audioUnitElement, &cb, Marshal.SizeOf<AURenderCallbackStruct> ());
			}
		}

		[UnmanagedCallersOnly]
		static unsafe AudioUnitStatus RenderCallbackImpl (IntPtr clientData, AudioUnitRenderActionFlags* actionFlags, AudioTimeStamp* timeStamp, uint busNumber, uint numberFrames, IntPtr data)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var au = (AudioUnit?) gch.Target;
			var renderer = au?.renderer;
			if (renderer is null)
				return AudioUnitStatus.Uninitialized;

			if (!renderer.TryGetValue (busNumber, out var render))
				return AudioUnitStatus.Uninitialized;

			using (var buffers = new AudioBuffers (data)) {
				unsafe {
					return render (*actionFlags, *timeStamp, busNumber, numberFrames, buffers);
				}
			}
		}

		#endregion

		#region SetInputCallback

		/// <param name="inputDelegate">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetInputCallback (InputDelegate inputDelegate, AudioUnitScopeType scope = AudioUnitScopeType.Global, uint audioUnitElement = 0)
		{
			if (inputs is null)
				Interlocked.CompareExchange (ref inputs, new Dictionary<uint, InputDelegate> (), null);

			inputs [audioUnitElement] = inputDelegate;

			if (!gcHandle.IsAllocated)
				gcHandle = GCHandle.Alloc (this);

			var cb = new AURenderCallbackStruct ();
			unsafe {
				cb.Proc = &InputCallbackImpl;
				cb.ProcRefCon = GCHandle.ToIntPtr (gcHandle);
				return AudioUnitSetProperty (Handle, AudioUnitPropertyIDType.SetInputCallback, scope, audioUnitElement, &cb, Marshal.SizeOf<AURenderCallbackStruct> ());
			}
		}
		[UnmanagedCallersOnly]
		static unsafe AudioUnitStatus InputCallbackImpl (IntPtr clientData, AudioUnitRenderActionFlags* actionFlags, AudioTimeStamp* timeStamp, uint busNumber, uint numberFrames, IntPtr data)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var au = gch.Target as AudioUnit;
			if (au is null)
				return AudioUnitStatus.Uninitialized;

			var inputs = au.inputs;
			if (inputs is null)
				return AudioUnitStatus.Uninitialized;

			if (!inputs.TryGetValue (busNumber, out var input))
				return AudioUnitStatus.Uninitialized;
			unsafe {
				return input (*actionFlags, *timeStamp, busNumber, numberFrames, au);
			}
		}

		#endregion

#if !MONOMAC
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
		[ObsoletedOSPlatform ("ios13.0")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioComponentStatus AudioOutputUnitPublish (AudioComponentDescription inDesc, IntPtr /* CFStringRef */ inName, uint /* UInt32 */ inVersion, IntPtr /* AudioUnit */ inOutputUnit);

		/// <param name="description">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <param name="version">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("maccatalyst14.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'AudioUnit' instead.")]
		public AudioComponentStatus AudioOutputUnitPublish (AudioComponentDescription description, string name, uint version = 1)
		{

			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			var nameHandle = CFString.CreateNative (name);
			try {
				return AudioOutputUnitPublish (description, nameHandle, version, Handle);
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
		[ObsoletedOSPlatform ("ios13.0")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern IntPtr AudioOutputUnitGetHostIcon (IntPtr /* AudioUnit */ au, float /* float */ desiredPointSize);

		/// <param name="desiredPointSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("maccatalyst14.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'AudioUnit' instead.")]
		public UIKit.UIImage? GetHostIcon (float desiredPointSize)
		{
			return Runtime.GetNSObject<UIKit.UIImage> (AudioOutputUnitGetHostIcon (Handle, desiredPointSize));
		}
#endif

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus Initialize ()
		{
			return AudioUnitInitialize (Handle);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus Uninitialize ()
		{
			return AudioUnitUninitialize (Handle);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus Start ()
		{
			AudioUnitStatus rv = 0;
			if (!_isPlaying) {
				rv = AudioOutputUnitStart (Handle);
				_isPlaying = true;
			}
			return rv;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus Stop ()
		{
			AudioUnitStatus rv = 0;
			if (_isPlaying) {
				rv = AudioOutputUnitStop (Handle);
				_isPlaying = false;
			}
			return rv;
		}

		#region Render

		/// <param name="actionFlags">To be added.</param>
		///         <param name="timeStamp">To be added.</param>
		///         <param name="busNumber">To be added.</param>
		///         <param name="numberFrames">To be added.</param>
		///         <param name="data">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus Render (ref AudioUnitRenderActionFlags actionFlags, AudioTimeStamp timeStamp, uint busNumber, uint numberFrames, AudioBuffers data)
		{
			if ((IntPtr) data == IntPtr.Zero)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			unsafe {
				return AudioUnitRender (
					Handle,
					(AudioUnitRenderActionFlags*) Unsafe.AsPointer<AudioUnitRenderActionFlags> (ref actionFlags),
					&timeStamp,
					busNumber,
					numberFrames,
					(IntPtr) data);
			}
		}

		#endregion

		/// <param name="type">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <param name="scope">To be added.</param>
		///         <param name="audioUnitElement">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetParameter (AudioUnitParameterType type, float value, AudioUnitScopeType scope, uint audioUnitElement = 0)
		{
			return AudioUnitSetParameter (Handle, type, scope, audioUnitElement, value, 0);
		}

		/// <param name="inParameterEvent">To be added.</param>
		///         <param name="inNumParamEvents">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus ScheduleParameter (AudioUnitParameterEvent inParameterEvent, uint inNumParamEvents)
		{
			return AudioUnitScheduleParameters (Handle, inParameterEvent, inNumParamEvents);
		}

		[DllImport (Constants.AudioUnitLibrary)]
		static extern int AudioComponentInstanceDispose (IntPtr inInstance);

		/// <include file="../../docs/api/AudioUnit/AudioUnit.xml" path="/Documentation/Docs[@DocId='M:AudioUnit.AudioUnit.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns) {
				Stop ();
				AudioUnitUninitialize (Handle);
				AudioComponentInstanceDispose (Handle);
			}
			if (gcHandle.IsAllocated)
				gcHandle.Free ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioComponentInstanceNew (IntPtr inComponent, IntPtr* inDesc);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern IntPtr AudioComponentInstanceGetComponent (IntPtr inComponent);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitInitialize (IntPtr inUnit);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitUninitialize (IntPtr inUnit);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioOutputUnitStart (IntPtr ci);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioOutputUnitStop (IntPtr ci);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitRender (IntPtr inUnit, AudioUnitRenderActionFlags* ioActionFlags, AudioTimeStamp* inTimeStamp,
						  uint inOutputBusNumber, uint inNumberFrames, IntPtr ioData);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AudioStreamBasicDescription* inData,
							   uint inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   uint* inData, uint inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   double* inData, uint inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   IntPtr* inData, int inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   NativeHandle* inData, int inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AURenderCallbackStruct* inData, int inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AudioUnitConnection* inData, int inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AUSamplerInstrumentData* inData, int inDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AudioStreamBasicDescription* outData,
							   uint* ioDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   IntPtr* outData,
							   int* ioDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
						uint* outData,
						int* ioDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern unsafe AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   uint* outData,
							   uint* ioDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern unsafe AudioUnitStatus AudioUnitGetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
							   AudioUnitParameterInfoNative* outData,
							   uint* ioDataSize);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitGetPropertyInfo (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
								uint* outDataSize, byte* outWritable);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitSetParameter (IntPtr inUnit, AudioUnitParameterType inID, AudioUnitScopeType inScope,
			uint inElement, float inValue, uint inBufferOffsetInFrames);

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitScheduleParameters (IntPtr inUnit, AudioUnitParameterEvent inParameterEvent, uint inNumParamEvents);

#if MONOMAC || __MACCATALYST__
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[DllImport (Constants.CoreAudioLibrary)]
		unsafe static extern int AudioObjectGetPropertyData (
			uint inObjectID,
			AudioObjectPropertyAddress* inAddress,
			uint* inQualifierDataSize,
			IntPtr* inQualifierData,
			uint* ioDataSize,
			uint* outData
		);
#endif // MONOMAC
		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus MusicDeviceMIDIEvent (IntPtr /* MusicDeviceComponent = void* */ inUnit, uint /* UInt32 */ inStatus, uint /* UInt32 */ inData1, uint /* UInt32 */ inData2, uint /* UInt32 */ inOffsetSampleFrame);

		// TODO: https://github.com/dotnet/macios/issues/12489
		// [TV (15,0), iOS (15,0), MacCatalyst (15,0)]
		// [DllImport (Constants.AudioUnitLibrary)]
		// static extern MusicDeviceMIDIEvent[] MusicDeviceMIDIEventList (IntPtr /* MusicDeviceComponent = void* */ inUnit, uint /* UInt32 */ inOffsetSampleFrame, MIDIEventList eventList);

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
			AUScheduledAudioFileRegion.ScheduledAudioFileRegion* inData, int inDataSize);

		/// <param name="region">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetScheduledFileRegion (AUScheduledAudioFileRegion region)
		{
			if (region is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (region));

			var safr = region.GetAudioFileRegion ();
			unsafe {
				return AudioUnitSetProperty (GetCheckedHandle (), AudioUnitPropertyIDType.ScheduledFileRegion, AudioUnitScopeType.Global, 0, &safr, Marshal.SizeOf<AUScheduledAudioFileRegion.ScheduledAudioFileRegion> ());
			}
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
			AudioTimeStamp* inData, int inDataSize);

		/// <param name="timeStamp">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetScheduleStartTimeStamp (AudioTimeStamp timeStamp)
		{
			unsafe {
				return AudioUnitSetProperty (GetCheckedHandle (), AudioUnitPropertyIDType.ScheduleStartTimeStamp, AudioUnitScopeType.Global, 0, &timeStamp, Marshal.SizeOf<AudioTimeStamp> ());
			}
		}

		/// <param name="audioFile">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnitStatus SetScheduledFiles (AudioFile audioFile)
		{
			if (audioFile is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioFile));

			var audioFilehandle = audioFile.Handle;
			unsafe {
				AudioUnitStatus status = AudioUnitSetProperty (GetCheckedHandle (), AudioUnitPropertyIDType.ScheduledFileIDs, AudioUnitScopeType.Global, 0, &audioFilehandle, Marshal.SizeOf<NativeHandle> ());
				GC.KeepAlive (audioFile);
				return status;
			}
		}

		[DllImport (Constants.AudioUnitLibrary)]
		static extern AudioUnitStatus AudioUnitSetProperty (IntPtr inUnit, AudioUnitPropertyIDType inID, AudioUnitScopeType inScope, uint inElement,
			IntPtr inData, int inDataSize);

		/// <param name="audioFiles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe AudioUnitStatus SetScheduledFiles (AudioFile [] audioFiles)
		{
			if (audioFiles is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (audioFiles));

			int count = audioFiles.Length;
			IntPtr [] handles = new IntPtr [count];
			for (int i = 0; i < count; i++)
				handles [i] = audioFiles [i].Handle;

			fixed (IntPtr* ptr = handles) {
				AudioUnitStatus status = AudioUnitSetProperty (GetCheckedHandle (), AudioUnitPropertyIDType.ScheduledFileIDs, AudioUnitScopeType.Global, 0, (IntPtr) ptr, IntPtr.Size * count);
				GC.KeepAlive (audioFiles);
				return status;
			}
		}

#endif // !COREBUILD
	}

#if MONOMAC || __MACCATALYST__
	[StructLayout (LayoutKind.Sequential)]
	struct AudioObjectPropertyAddress {
#if !COREBUILD
		public uint /* UInt32 */ Selector;
		public uint /* UInt32 */ Scope;
		public uint /* UInt32 */ Element;

		public AudioObjectPropertyAddress (uint selector, uint scope, uint element)
		{
			Selector = selector;
			Scope = scope;
			Element = element;
		}

		public AudioObjectPropertyAddress (AudioObjectPropertySelector selector, AudioObjectPropertyScope scope, AudioObjectPropertyElement element)
		{
			Selector = (uint) selector;
			Scope = (uint) scope;
			Element = (uint) element;
		}
#endif // !COREBUILD
	}
#endif // MONOMAC || __MACCATALYST__

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public unsafe class AURenderEventEnumerator : INativeObject
#if COREBUILD
	{ }
#else
	, IEnumerator<AURenderEvent> {
		AURenderEvent* current;

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public NativeHandle Handle { get; private set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsEmpty { get { return Handle == IntPtr.Zero; } }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsAtEnd { get { return current is null; } }

		public AURenderEventEnumerator (NativeHandle ptr)
			: this (ptr, false)
		{
		}

		[Preserve (Conditional = true)]
		internal AURenderEventEnumerator (NativeHandle handle, bool owns)
		{
			Handle = handle;
			current = (AURenderEvent*) (IntPtr) handle;
		}

		/// <summary>Releases the resources used by the AURenderEventEnumerator object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the AURenderEventEnumerator class.</para>
		///           <para>Calling the Dispose method when the application is finished using the AURenderEventEnumerator ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Handle = NativeHandle.Zero;
			current = null;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AURenderEvent* UnsafeFirst {
			get {
				return (AURenderEvent*) (IntPtr) Handle;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AURenderEvent First {
			get {
				if (IsEmpty)
					throw new InvalidOperationException ("Can not get First on AURenderEventEnumerator when empty");
				return *(AURenderEvent*) (IntPtr) Handle;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AURenderEvent Current {
			get {
				if (IsAtEnd)
					throw new InvalidOperationException ("Can not get Current on AURenderEventEnumerator when at end");
				return *current;
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		object IEnumerator.Current {
			get { return Current; }
		}

		bool IsAt (nint now)
		{
			return current is not null && (current->Head.EventSampleTime == now);
		}

		public IEnumerable<AURenderEvent> EnumeratorCurrentEvents (nint now)
		{
			if (IsAtEnd)
				throw new InvalidOperationException ("Can not enumerate events on AURenderEventEnumerator when at end");

			do {
				yield return Current;
				MoveNext ();
			} while (IsAt (now));
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool /*IEnumerator<AURenderEvent>.*/MoveNext ()
		{
			if (current is not null)
				current = ((AURenderEvent*) current)->Head.UnsafeNext;
			return current is not null;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void /*IEnumerator<AURenderEvent>.*/Reset ()
		{
			current = (AURenderEvent*) (IntPtr) Handle;
		}
	}
#endif // !COREBUILD

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum AURenderEventType : byte {
		/// <summary>To be added.</summary>
		Parameter = 1,
		/// <summary>To be added.</summary>
		ParameterRamp = 2,
		/// <summary>To be added.</summary>
		Midi = 8,
		/// <summary>To be added.</summary>
		MidiSysEx = 9,
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		MidiEventList = 10,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct AURenderEventHeader {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AURenderEvent* UnsafeNext;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AURenderEvent? Next {
			get {
				if (UnsafeNext is not null)
					return Marshal.PtrToStructure<AURenderEvent> ((IntPtr) UnsafeNext);
				return null;
			}
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public long EventSampleTime;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AURenderEventType EventType;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public byte Reserved;
	}

	/// <summary>Contains a token for an installed parameter observer delegate.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AUParameterObserverToken {
		/// <summary>The token that represents a parameter or parameter recording observer delegate.</summary>
		///         <remarks>To be added.</remarks>
		public IntPtr ObserverToken;
		/// <param name="observerToken">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AUParameterObserverToken (IntPtr observerToken)
		{
			ObserverToken = observerToken;
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct AUParameterEvent {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AURenderEvent* UnsafeNext;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AURenderEvent? Next {
			get {
				if (UnsafeNext is not null)
					return Marshal.PtrToStructure<AURenderEvent> ((IntPtr) UnsafeNext);
				return null;
			}
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public long EventSampleTime;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AURenderEventType EventType;

		internal byte reserved1;
		internal byte reserved2;
		internal byte reserved3;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public uint RampDurationSampleFrames;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ulong ParameterAddress;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Value;
	}

	// 	AUAudioTODO - We need testing for these bindings
	// 	[StructLayout (LayoutKind.Sequential)]
	// 	public unsafe struct AUMidiEvent
	// 	{
	//		public AURenderEvent * UnsafeNext;
	//
	//		public AURenderEvent? Next {
	//			get {
	//				if (UnsafeNext is not null)
	//					return Marshal.PtrToStructure<AURenderEvent> ((IntPtr)UnsafeNext);
	//				return null;
	//			}
	//		}
	//
	// 		public long EventSampleTime;
	//
	// 		public AURenderEventType EventType;
	//
	// 		public byte Reserved;
	//
	// 		public ushort Length;
	//
	// 		public byte Cable;
	//
	// 		public byte Data_1;
	// 		public byte Data_2;
	// 		public byte Data_3;
	// 	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Explicit)]
	public struct AURenderEvent {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public AURenderEventHeader Head;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[FieldOffset (0)]
		public AUParameterEvent Parameter;

		// 		[FieldOffset (0)]
		// 		public AUMidiEvent Midi;
	}

	/// <summary>An event that represents the change and time of change for a parameter value.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AURecordedParameterEvent {
		/// <summary>The host time at which the change occured.</summary>
		///         <remarks>To be added.</remarks>
		public ulong HostTime;

		/// <summary>The numeric identfier of the parameter.</summary>
		///         <remarks>To be added.</remarks>
		public ulong Address;

		/// <summary>The new value of the parameter.</summary>
		///         <remarks>To be added.</remarks>
		public float Value;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct AUParameterAutomationEvent {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ulong HostTime;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ulong Address;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public float Value;
#if COREBUILD
		// keep structure size identical across builds
		public uint EventType;
#else
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AUParameterAutomationEventType EventType;
#endif
		ulong Reserved;
	}

#if !COREBUILD
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	//	Configuration Info Keys
	public static class AudioUnitConfigurationInfo {
		//		#define kAudioUnitConfigurationInfo_HasCustomView	"HasCustomView"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString HasCustomView = new NSString ("HasCustomView");

		//		#define kAudioUnitConfigurationInfo_ChannelConfigurations	"ChannelConfigurations"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString ChannelConfigurations = new NSString ("ChannelConfigurations");

		//		#define kAudioUnitConfigurationInfo_InitialInputs	"InitialInputs"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString InitialInputs = new NSString ("InitialInputs");

		//		#define kAudioUnitConfigurationInfo_IconURL			"IconURL"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString IconUrl = new NSString ("IconURL");

		//		#define kAudioUnitConfigurationInfo_BusCountWritable	"BusCountWritable"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString BusCountWritable = new NSString ("BusCountWritable");

		//		#define kAudioUnitConfigurationInfo_SupportedChannelLayoutTags	"SupportedChannelLayoutTags"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString SupportedChannelLayoutTags = new NSString ("SupportedChannelLayoutTags");
	}
#endif
}
