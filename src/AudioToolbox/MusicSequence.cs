//
// MusicPlayer.cs: Bindings to the AudioToolbox's MusicPlayers APIs
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012-2014 Xamarin Inc.
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;
#if !COREBUILD
using CoreAnimation;
#if !TVOS
using CoreMidi;
#endif
using AudioUnit;
#endif

using MidiEndpointRef = System.Int32;

namespace AudioToolbox {

#if !COREBUILD
	public delegate void MusicSequenceUserCallback (MusicTrack track, double inEventTime, MusicEventUserData inEventData, double inStartSliceBeat, double inEndSliceBeat);

	delegate void MusicSequenceUserCallbackProxy (/* void * */ IntPtr inClientData, /* MusicSequence* */ IntPtr inSequence, /* MusicTrack* */ IntPtr inTrack, /* MusicTimeStamp */ double inEventTime, /* MusicEventUserData* */ IntPtr inEventData, /* MusicTimeStamp */ double inStartSliceBeat, /* MusicTimeStamp */ double inEndSliceBeat);
#endif

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// MusicPlayer.h
	public class MusicSequence : DisposableObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal MusicSequence (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		static Dictionary<IntPtr, MusicSequenceUserCallback> userCallbackHandles = new Dictionary<IntPtr, MusicSequenceUserCallback> (Runtime.IntPtrEqualityComparer);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus NewMusicSequence (/* MusicSequence* */ IntPtr* outSequence);

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus DisposeMusicSequence (/* MusicSequence */ IntPtr inSequence);

		static IntPtr Create ()
		{
			IntPtr handle;
			unsafe {
				NewMusicSequence (&handle);
			}
			return handle;
		}

		public MusicSequence ()
			: base (Create (), true)
		{
			lock (sequenceMap)
				sequenceMap [Handle] = new WeakReference (this);
		}

		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns) {

				lock (userCallbackHandles)
					userCallbackHandles.Remove (Handle);

				// Remove native user callback
				unsafe {
					MusicSequenceSetUserCallback (Handle, null, IntPtr.Zero);
				}

				DisposeMusicSequence (Handle);
				lock (sequenceMap) {
					sequenceMap.Remove (Handle);
				}
			}
			base.Dispose (disposing);
		}

		static readonly Dictionary<IntPtr, WeakReference> sequenceMap = new Dictionary<IntPtr, WeakReference> (Runtime.IntPtrEqualityComparer);

		internal static MusicSequence Lookup (IntPtr handle)
		{
			return Lookup (handle, false);
		}

		internal static MusicSequence Lookup (IntPtr handle, bool owns)
		{
			lock (sequenceMap) {
				if (sequenceMap.TryGetValue (handle, out var weakRef)) {
					var target = weakRef.Target;
					if (target is not null) {
						return (MusicSequence) target;
					}
					sequenceMap.Remove (handle);
				}
				var ms = new MusicSequence (handle, owns);
				sequenceMap [handle] = new WeakReference (ms);
				return ms;
			}
		}


		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceSetAUGraph (/* MusicSequence */ IntPtr inSequence, /* AUGraph */ IntPtr inGraph);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetAUGraph (/* MusicSequence */ IntPtr inSequence, /* AUGraph* */ IntPtr* outGraph);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos14.0", "Use 'AVAudioEngine' instead.")]
		[ObsoletedOSPlatform ("macos11.0", "Use 'AVAudioEngine' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'AVAudioEngine' instead.")]
		[ObsoletedOSPlatform ("maccatalyst14.0", "Use 'AVAudioEngine' instead.")]
		// This API isn't obsoleted, but AUGraph is, so copy the obsolete attributes from AUGraph
		public AUGraph? AUGraph {
			get {
				IntPtr h;
				unsafe {
					if (MusicSequenceGetAUGraph (Handle, &h) != MusicPlayerStatus.Success)
						return null;
				}

				return new AUGraph (h, false);
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				MusicSequenceSetAUGraph (Handle, value.Handle);
				GC.KeepAlive (value);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceSetSequenceType (/* MusicSequence */ IntPtr inSequence, MusicSequenceType inType);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetSequenceType (/* MusicSequence */ IntPtr inSequence, MusicSequenceType* outType);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public MusicSequenceType SequenceType {
			get {
				MusicSequenceType type;
				unsafe {
					MusicSequenceGetSequenceType (Handle, &type);
				}
				return type;
			}
			set {
				MusicSequenceSetSequenceType (Handle, value);
			}
		}

		public void GetSmpteResolution (short resolution, out sbyte fps, out byte ticks)
		{
			// MusicSequenceGetSMPTEResolution is CF_INLINE -> can't be pinvoke'd (it's not part of the library)
			fps = (sbyte) ((resolution & 0xFF00) >> 8);
			ticks = (byte) (resolution & 0x007F);
		}

		public short SetSmpteResolution (sbyte fps, byte ticks)
		{
			// MusicSequenceSetSMPTEResolution is CF_INLINE -> can't be pinvoke'd (it's not part of the library)
			if (fps > 0)
				fps = (sbyte) -fps;
			return (short) ((fps << 8) + ticks);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* CFDictionaryRef */ IntPtr MusicSequenceGetInfoDictionary (/* MusicSequence */ IntPtr inSequence);

		public NSDictionary? GetInfoDictionary ()
		{
			return Runtime.GetNSObject<NSDictionary> (MusicSequenceGetInfoDictionary (Handle));
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceNewTrack (/* MusicSequence */ IntPtr inSequence, /* MusicTrack* */ IntPtr* outTrack);

		public MusicTrack? CreateTrack ()
		{
			IntPtr trackHandle;
			unsafe {
				if (MusicSequenceNewTrack (Handle, &trackHandle) == MusicPlayerStatus.Success)
					return new MusicTrack (this, trackHandle, owns: true);
			}

			return null;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetTrackCount (/* MusicSequence */ IntPtr inSequence, /* UInt32* */ int* outNumberOfTracks);

		// an `uint` but we keep `int` for compatibility (should be enough tracks)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int TrackCount {
			get {
				int count;
				unsafe {
					if (MusicSequenceGetTrackCount (Handle, &count) == MusicPlayerStatus.Success)
						return count;
				}
				return 0;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetIndTrack (/* MusicSequence */ IntPtr inSequence, /* Uint32 */ int inTrackIndex, /* MusicTrack* */ IntPtr* outTrack);

		public MusicTrack? GetTrack (int trackIndex)
		{
			IntPtr outTrack;
			unsafe {
				if (MusicSequenceGetIndTrack (Handle, trackIndex, &outTrack) == MusicPlayerStatus.Success)
					return new MusicTrack (this, outTrack, owns: false);
			}

			return null;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetTrackIndex (/* MusicSequence */ IntPtr inSequence, /* MusicTrack */ IntPtr inTrack, /* UInt32* */ int* outTrackIndex);

		public MusicPlayerStatus GetTrackIndex (MusicTrack track, out int index)
		{
			if (track is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (track));

			index = 0;
			unsafe {
				MusicPlayerStatus status = MusicSequenceGetTrackIndex (Handle, track.Handle, (int*) Unsafe.AsPointer<int> (ref index));
				GC.KeepAlive (track);
				return status;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetTempoTrack (/* MusicSequence */ IntPtr sequence, /* MusicTrack */ IntPtr* outTrack);

		public MusicTrack? GetTempoTrack ()
		{
			IntPtr outTrack;
			unsafe {
				if (MusicSequenceGetTempoTrack (Handle, &outTrack) == MusicPlayerStatus.Success)
					return new MusicTrack (this, outTrack, owns: false);
			}

			return null;
		}

#if IOS
		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceSetMIDIEndpoint (/* MusicSequence */ IntPtr inSequence, MidiEndpointRef inEndpoint);

		public MusicPlayerStatus SetMidiEndpoint (MidiEndpoint endpoint)
		{
			if (endpoint is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (endpoint));
			MusicPlayerStatus status = MusicSequenceSetMIDIEndpoint (Handle, endpoint.handle);
			GC.KeepAlive (endpoint);
			return status;
		}
#endif // IOS

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetSecondsForBeats (/* MusicSequence */ IntPtr inSequence, /* MusicTimeStamp */ double inBeats, /* Float64* */ double* outSeconds);

		public double GetSecondsForBeats (double beats)
		{
			double sec;
			unsafe {
				if (MusicSequenceGetSecondsForBeats (Handle, beats, &sec) == MusicPlayerStatus.Success)
					return sec;
			}
			return 0;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceGetBeatsForSeconds (/* MusicSequence */ IntPtr inSequence, /* Float64 */ double inSeconds, /* MusicTimeStamp* */ double* outBeats);

		public double GetBeatsForSeconds (double seconds)
		{
			double beats;
			unsafe {
				if (MusicSequenceGetBeatsForSeconds (Handle, seconds, &beats) == MusicPlayerStatus.Success)
					return beats;
			}
			return 0;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static unsafe /* OSStatus */ MusicPlayerStatus MusicSequenceSetUserCallback (/* MusicSequence */ IntPtr inSequence, delegate* unmanaged<IntPtr, IntPtr, IntPtr, double, IntPtr, double, double, void> inCallback, /* void * */ IntPtr inClientData);

		public void SetUserCallback (MusicSequenceUserCallback callback)
		{
			lock (userCallbackHandles)
				userCallbackHandles [Handle] = callback;

			unsafe {
				MusicSequenceSetUserCallback (Handle, &UserCallbackProxy, IntPtr.Zero);
			}
		}

		[UnmanagedCallersOnly]
		static void UserCallbackProxy (IntPtr inClientData, IntPtr inSequence, IntPtr inTrack, double inEventTime, IntPtr inEventData, double inStartSliceBeat, double inEndSliceBeat)
		{
			MusicSequenceUserCallback? userCallback;
			lock (userCallbackHandles)
				userCallbackHandles.TryGetValue (inSequence, out userCallback);

			if (userCallback is not null) {
				var userEventData = new MusicEventUserData (inEventData);
				var musicSequence = MusicSequence.Lookup (inSequence);
				var musicTrack = new MusicTrack (musicSequence, inTrack, false);

				userCallback (musicTrack, inEventTime, userEventData, inStartSliceBeat, inEndSliceBeat);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceBeatsToBarBeatTime (/* MusicSequence */ IntPtr inSequence, /* MusicTimeStamp */ double inBeats, /* UInt32 */ int inSubbeatDivisor, CABarBeatTime* outBarBeatTime);

		public MusicPlayerStatus BeatsToBarBeatTime (double beats, int subbeatDivisor, out CABarBeatTime barBeatTime)
		{
			barBeatTime = default (CABarBeatTime);
			unsafe {
				return MusicSequenceBeatsToBarBeatTime (Handle, beats, subbeatDivisor, (CABarBeatTime*) Unsafe.AsPointer<CABarBeatTime> (ref barBeatTime));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceBarBeatTimeToBeats (/* MusicSequence */ IntPtr inSequence, CABarBeatTime inBarBeatTime, /* MusicTimeStamp*/ double* outBeats);
		public MusicPlayerStatus BarBeatTimeToBeats (CABarBeatTime barBeatTime, out double beats)
		{
			beats = 0;
			unsafe {
				return MusicSequenceBarBeatTimeToBeats (Handle, barBeatTime, (double*) Unsafe.AsPointer<double> (ref beats));
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceReverse (/* MusicSequence */ IntPtr inSequence);

		public MusicPlayerStatus Reverse ()
		{
			return MusicSequenceReverse (Handle);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceFileLoad (/* MusicSequence */ IntPtr inSequence, /* CFURLRef */ IntPtr inFileRef, MusicSequenceFileTypeID inFileTypeHint, MusicSequenceLoadFlags inFlags);

		public MusicPlayerStatus LoadFile (NSUrl url, MusicSequenceFileTypeID fileTypeId, MusicSequenceLoadFlags loadFlags = 0)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			MusicPlayerStatus status = MusicSequenceFileLoad (Handle, url.Handle, fileTypeId, loadFlags);
			GC.KeepAlive (url);
			return status;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceFileLoadData (/* MusicSequence */ IntPtr inSequence, /* CFDataRef */ IntPtr inData, MusicSequenceFileTypeID inFileTypeHint, MusicSequenceLoadFlags inFlags);

		public MusicPlayerStatus LoadData (NSData data, MusicSequenceFileTypeID fileTypeId, MusicSequenceLoadFlags loadFlags = 0)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));

			MusicPlayerStatus status = MusicSequenceFileLoadData (Handle, data.Handle, fileTypeId, loadFlags);
			GC.KeepAlive (data);
			return status;
		}


		[DllImport (Constants.AudioToolboxLibrary)]
		extern static /* OSStatus */ MusicPlayerStatus MusicSequenceFileCreate (/* MusicSequence */ IntPtr inSequence, /* CFURLRef */ IntPtr inFileRef, MusicSequenceFileTypeID inFileType, MusicSequenceFileFlags inFlags, /* SInt16 */ ushort resolution);

		// note: resolution should be short instead of ushort
		public MusicPlayerStatus CreateFile (NSUrl url, MusicSequenceFileTypeID fileType, MusicSequenceFileFlags flags = 0, ushort resolution = 0)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			MusicPlayerStatus status = MusicSequenceFileCreate (Handle, url.Handle, fileType, flags, resolution);
			GC.KeepAlive (url);
			return status;
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe extern static /* OSStatus */ MusicPlayerStatus MusicSequenceFileCreateData (/* MusicSequence */ IntPtr inSequence, MusicSequenceFileTypeID inFileType, MusicSequenceFileFlags inFlags, /* SInt16 */ ushort resolution, /* CFDataRef* */ IntPtr* outData);

		// note: resolution should be short instead of ushort
		public NSData? CreateData (MusicSequenceFileTypeID fileType, MusicSequenceFileFlags flags = 0, ushort resolution = 0)
		{
			IntPtr theData;
			unsafe {
				if (MusicSequenceFileCreateData (Handle, fileType, flags, resolution, &theData) == MusicPlayerStatus.Success)
					return Runtime.GetNSObject<NSData> (theData);
			}
			return null;
		}
#endif // !COREBUILD
	}

	// typedef UInt32 -> MusicPlayer.h
	public enum MusicSequenceType : uint {
		/// <summary>A normal MIDI music sequence. The tempo track defines beats-per-second.</summary>
		Beats = 0x62656174,     // 'beat'
		/// <summary>A MIDI sequence saved with SMPTE timecode.</summary>
		Seconds = 0x73656373,   // 'secs'
		/// <summary>Indicates an audio sample. The tempo track defines samples-per-second.</summary>
		Samples = 0x73616d70,   // 'samp'
	}
}
