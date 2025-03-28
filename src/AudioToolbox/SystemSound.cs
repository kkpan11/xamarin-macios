// 
// SystemSound.cs: AudioServices system sound
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2009 Novell, Inc
// Copyright 2012 Xamarin Inc.
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace AudioToolbox {

	enum SystemSoundId : uint { // UInt32 SystemSoundID
		Vibrate = 0x00000FFF,
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class SystemSound : IDisposable {
#if MONOMAC
		// TODO:
#else
		/// <summary>A special system sound that can be used to make the device vibrate.</summary>
		///         <remarks>This system sound does nothing on iPod touches.</remarks>
		public static readonly SystemSound Vibrate = new SystemSound ((uint) SystemSoundId.Vibrate, false);
#endif

		uint soundId;
		bool ownsHandle;

		Action? completionRoutine;
		GCHandle gc_handle;

		internal SystemSound (uint soundId, bool ownsHandle)
		{
			this.soundId = soundId;
			this.ownsHandle = ownsHandle;
		}

		public SystemSound (uint soundId) : this (soundId, false) { }


		~SystemSound ()
		{
			Dispose (false);
		}

		public uint SoundId {
			get {
				AssertNotDisposed ();
				return soundId;
			}
		}

		/// <summary>Gets or sets whether to override user preferences when calling <see cref="M:AudioToolbox.SystemSound.PlaySystemSound" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsUISound {
			get {
				uint out_size = sizeof (uint);
				uint data;
				AudioServicesError res;
				var soundId = this.soundId;

				unsafe {
					res = AudioServices.AudioServicesGetProperty (AudioServicesPropertyKey.IsUISound, sizeof (AudioServicesPropertyKey), &soundId, &out_size, &data);
				}
				if (res != AudioServicesError.None)
					throw new ArgumentException (res.ToString ());

				return data == 1;
			}

			set {
				uint data = value ? (uint) 1 : 0;
				AudioServicesError res;
				var soundId = this.soundId;

				unsafe {
					res = AudioServices.AudioServicesSetProperty (AudioServicesPropertyKey.IsUISound, sizeof (AudioServicesPropertyKey), &soundId, sizeof (uint), &data);
				}
				if (res != AudioServicesError.None)
					throw new ArgumentException (res.ToString ());
			}
		}

		/// <summary>Controls whether the sound will complete playing, even if the application dies.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool CompletePlaybackIfAppDies {
			get {
				uint out_size = sizeof (uint);
				uint data;
				AudioServicesError res;
				var soundId = this.soundId;
				unsafe {
					res = AudioServices.AudioServicesGetProperty (AudioServicesPropertyKey.CompletePlaybackIfAppDies, sizeof (AudioServicesPropertyKey), &soundId, &out_size, &data);
				}
				if (res != AudioServicesError.None)
					throw new ArgumentException (res.ToString ());

				return data == 1;
			}

			set {
				uint data = value ? (uint) 1 : 0;
				AudioServicesError res;
				var soundId = this.soundId;
				unsafe {
					res = AudioServices.AudioServicesSetProperty (AudioServicesPropertyKey.CompletePlaybackIfAppDies, sizeof (AudioServicesPropertyKey), &soundId, sizeof (uint), &data);
				}
				if (res != AudioServicesError.None)
					throw new ArgumentException (res.ToString ());
			}
		}

		void AssertNotDisposed ()
		{
			if (soundId == 0)
				throw new ObjectDisposedException ("SystemSound");
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			Cleanup (false);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern AudioServicesError AudioServicesDisposeSystemSoundID (uint soundId);

		void Cleanup (bool checkForError)
		{
			if (soundId == 0 || !ownsHandle)
				return;

			if (gc_handle.IsAllocated) {
				gc_handle.Free ();
			}

			if (completionRoutine is not null) {
				RemoveSystemSoundCompletion ();
			}

			var error = AudioServicesDisposeSystemSoundID (soundId);
			var oldId = soundId;
			soundId = 0;
			if (checkForError && error != AudioServicesError.None) {
				throw new InvalidOperationException (string.Format ("Error while disposing SystemSound with ID {0}: {1}",
							oldId, error.ToString ()));
			}
		}

		public void Close ()
		{
			Cleanup (true);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesPlayAlertSound (uint inSystemSoundID);
		public void PlayAlertSound ()
		{
			AssertNotDisposed ();
			AudioServicesPlayAlertSound (soundId);
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesPlaySystemSound (uint inSystemSoundID);
		public void PlaySystemSound ()
		{
			AssertNotDisposed ();
			AudioServicesPlaySystemSound (soundId);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void PlayAlertSound (Action onCompletion)
		{
			if (onCompletion is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (onCompletion));

			AssertNotDisposed ();

			unsafe {
				using var block = BlockStaticDispatchClass.CreateBlock (onCompletion);
				AudioServicesPlayAlertSoundWithCompletion (soundId, &block);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public Task PlayAlertSoundAsync ()
		{
			var tcs = new TaskCompletionSource<bool> ();
			PlayAlertSound (() => {
				tcs.SetResult (true);
			});
			return tcs.Task;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void PlaySystemSound (Action onCompletion)
		{
			if (onCompletion is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (onCompletion));

			AssertNotDisposed ();

			unsafe {
				using var block = BlockStaticDispatchClass.CreateBlock (onCompletion);
				AudioServicesPlaySystemSoundWithCompletion (soundId, &block);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public Task PlaySystemSoundAsync ()
		{
			var tcs = new TaskCompletionSource<bool> ();
			PlaySystemSound (() => {
				tcs.SetResult (true);
			});
			return tcs.Task;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern void AudioServicesPlayAlertSoundWithCompletion (uint inSystemSoundID, BlockLiteral* inCompletionBlock);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern void AudioServicesPlaySystemSoundWithCompletion (uint inSystemSoundID, BlockLiteral* inCompletionBlock);

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioServicesError AudioServicesCreateSystemSoundID (IntPtr fileUrl, uint* soundId);

		static uint Create (NSUrl fileUrl)
		{
			AudioServicesError error;
			uint soundId;

			if (fileUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fileUrl));

			unsafe {
				error = AudioServicesCreateSystemSoundID (fileUrl.Handle, &soundId);
				GC.KeepAlive (fileUrl);
			}

			if (error != AudioServicesError.None)
				throw new InvalidOperationException (string.Format ("Could not create system sound ID for url {0}; error={1}",
							fileUrl, error));
			return soundId;
		}

		public SystemSound (NSUrl fileUrl)
			: this (Create (fileUrl))
		{
		}

		public static SystemSound? FromFile (NSUrl fileUrl)
		{
			AudioServicesError error;
			uint soundId;

			if (fileUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (fileUrl));

			unsafe {
				error = AudioServicesCreateSystemSoundID (fileUrl.Handle, &soundId);
				GC.KeepAlive (fileUrl);
			}
			if (error != AudioServicesError.None)
				return null;
			return new SystemSound (soundId, true);
		}

		public static SystemSound? FromFile (string filename)
		{
			AudioServicesError error;
			uint soundId;

			if (filename is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (filename));

			using (var url = new NSUrl (filename)) {
				unsafe {
					error = AudioServicesCreateSystemSoundID (url.Handle, &soundId);
					GC.KeepAlive (url);
				}
				if (error != AudioServicesError.None)
					return null;
				return new SystemSound (soundId, true);
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		unsafe static extern AudioServicesError AudioServicesAddSystemSoundCompletion (uint soundId, IntPtr runLoop, IntPtr runLoopMode, delegate* unmanaged<SystemSoundId, IntPtr, void> completionRoutine, IntPtr clientData);

		[UnmanagedCallersOnly]
		static void SoundCompletionShared (SystemSoundId id, IntPtr clientData)
		{
			GCHandle gch = GCHandle.FromIntPtr (clientData);
			var ss = gch.Target as SystemSound;

			if (ss?.completionRoutine is not null)
				ss.completionRoutine ();
		}

		public AudioServicesError AddSystemSoundCompletion (Action routine, CFRunLoop? runLoop = null)
		{
			if (gc_handle.IsAllocated)
				throw new ArgumentException ("Only single completion routine is supported");

			gc_handle = GCHandle.Alloc (this);
			completionRoutine = routine;

			unsafe {
				AudioServicesError result = AudioServicesAddSystemSoundCompletion (
														  soundId,
														  runLoop.GetHandle (),
														  IntPtr.Zero, // runLoopMode should be enum runLoopMode.GetHandle (),
														  &SoundCompletionShared,
														  GCHandle.ToIntPtr (gc_handle));
				GC.KeepAlive (runLoop);
				return result;
			}
		}

		[DllImport (Constants.AudioToolboxLibrary)]
		static extern void AudioServicesRemoveSystemSoundCompletion (uint soundId);

		public void RemoveSystemSoundCompletion ()
		{
			completionRoutine = null;
			AudioServicesRemoveSystemSoundCompletion (soundId);
		}
	}
}
