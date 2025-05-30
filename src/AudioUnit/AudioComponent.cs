//
// AudioComponent.cs: AudioComponent wrapper class
//
// Author:
//   AKIHIRO Uehara (u-akihiro@reinforce-lab.com)
//
// Copyright 2010 Reinforce Lab.
// Copyright 2011, 2012 Xamarin Inc
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
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ObjCRuntime;
using AudioToolbox;
using CoreFoundation;
using Foundation;
#if !MONOMAC
using UIKit;
#else
using AppKit;
#if !COREBUILD
using UIImage = AppKit.NSImage;
#endif
#endif

namespace AudioUnit {

#if !COREBUILD

#if !TVOS
	// keys are not constants and had to be found in AudioToolbox.framework/Headers/AudioComponent.h
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	public partial class ResourceUsageInfo : DictionaryContainer {
		static NSString userClientK = new NSString ("iokit.user-client");
		static NSString globalNameK = new NSString ("mach-lookup.global-name");
		static NSString networkClientK = new NSString ("network.client");
		static NSString exceptionK = new NSString ("temporary-exception.files.all.read-write");

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ResourceUsageInfo () : base () { }

		/// <param name="dic">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public ResourceUsageInfo (NSDictionary dic) : base (dic) { }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string []? IOKitUserClient {
			get {
				return GetStringArrayValue (userClientK);
			}
			set {
				if (value is null)
					RemoveValue (userClientK);
				else
					SetArrayValue (userClientK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string []? MachLookUpGlobalName {
			get {
				return GetStringArrayValue (globalNameK);
			}
			set {
				if (value is null)
					RemoveValue (globalNameK);
				else
					SetArrayValue (globalNameK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? NetworkClient {
			get {
				return GetBoolValue (networkClientK);
			}
			set {
				SetBooleanValue (networkClientK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? TemporaryExceptionReadWrite {
			get {
				return GetBoolValue (exceptionK);
			}
			set {
				SetBooleanValue (exceptionK, value);
			}
		}
	}

	// keys are not constants and had to be found in AudioToolbox.framework/Headers/AudioComponent.h
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	public partial class AudioComponentInfo : DictionaryContainer {
		static NSString typeK = new NSString ("type");
		static NSString subtypeK = new NSString ("subtype");
		static NSString manufacturerK = new NSString ("manufacturer");
		static NSString nameK = new NSString ("name");
		static NSString versionK = new NSString ("version");
		static NSString factoryFunctionK = new NSString ("factoryFunction");
		static NSString sandboxSafeK = new NSString ("sandboxSafe");
		static NSString resourceUsageK = new NSString ("resourceUsage");
		static NSString tagsK = new NSString ("tags");

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioComponentInfo () : base () { }

		/// <param name="dic">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AudioComponentInfo (NSDictionary dic) : base (dic) { }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Type {
			get {
				return GetStringValue (typeK);
			}
			set {
				SetStringValue (typeK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Subtype {
			get {
				return GetStringValue (subtypeK);
			}
			set {
				SetStringValue (subtypeK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Manufacturer {
			get {
				return GetStringValue (manufacturerK);
			}
			set {
				SetStringValue (manufacturerK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				return GetStringValue (nameK);
			}
			set {
				SetStringValue (nameK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nuint? Version {
			get {
				return GetNUIntValue (versionK);
			}
			set {
				SetNumberValue (versionK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? FactoryFunction {
			get {
				return GetStringValue (factoryFunctionK);
			}
			set {
				SetStringValue (factoryFunctionK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool? SandboxSafe {
			get {
				return GetBoolValue (sandboxSafeK);
			}
			set {
				SetBooleanValue (sandboxSafeK, value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ResourceUsageInfo? ResourceUsage {
			get {
				return GetStrongDictionary<ResourceUsageInfo> (resourceUsageK, (dict) => new ResourceUsageInfo (dict));
			}
			set {
				SetNativeValue (resourceUsageK, value?.Dictionary, true);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string []? Tags {
			get {
				return GetStringArrayValue (tagsK);
			}
			set {
				if (value is null)
					RemoveValue (tagsK);
				else
					SetArrayValue (tagsK, value);
			}
		}
	}
#endif // !TVOS

#endif // !COREBUILD


	/// <summary>An audio component.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class AudioComponent : DisposableObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal AudioComponent (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public AudioUnit CreateAudioUnit ()
		{
			return new AudioUnit (this);
		}

		/// <param name="cmp">To be added.</param>
		///         <param name="cd">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindNextComponent (AudioComponent? cmp, ref AudioComponentDescription cd)
		{
			var handle = cmp.GetHandle ();
			unsafe {
				handle = AudioComponentFindNext (handle, (AudioComponentDescription*) Unsafe.AsPointer<AudioComponentDescription> (ref cd));
			}
			AudioComponent? result = (handle != IntPtr.Zero) ? new AudioComponent (handle, false) : null;
			GC.KeepAlive (cmp);
			return result;
		}

		/// <param name="cd">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (ref AudioComponentDescription cd)
		{
			return FindNextComponent (null, ref cd);
		}

		/// <param name="output">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeOutput output)
		{
			var cd = AudioComponentDescription.CreateOutput (output);
			return FindComponent (ref cd);
		}

		/// <param name="musicDevice">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeMusicDevice musicDevice)
		{
			var cd = AudioComponentDescription.CreateMusicDevice (musicDevice);
			return FindComponent (ref cd);
		}

		/// <param name="conveter">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeConverter conveter)
		{
			var cd = AudioComponentDescription.CreateConverter (conveter);
			return FindComponent (ref cd);
		}

		/// <param name="effect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeEffect effect)
		{
			var cd = AudioComponentDescription.CreateEffect (effect);
			return FindComponent (ref cd);
		}

		/// <param name="mixer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeMixer mixer)
		{
			var cd = AudioComponentDescription.CreateMixer (mixer);
			return FindComponent (ref cd);
		}

		/// <param name="panner">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypePanner panner)
		{
			var cd = AudioComponentDescription.CreatePanner (panner);
			return FindComponent (ref cd);
		}

		/// <param name="generator">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AudioComponent? FindComponent (AudioTypeGenerator generator)
		{
			var cd = AudioComponentDescription.CreateGenerator (generator);
			return FindComponent (ref cd);
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern IntPtr AudioComponentFindNext (IntPtr inComponent, AudioComponentDescription* inDesc);

		[DllImport (Constants.AudioUnitLibrary, EntryPoint = "AudioComponentCopyName")]
		unsafe static extern int /* OSStatus */ AudioComponentCopyName (IntPtr component, IntPtr* cfstr);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				IntPtr r;
				unsafe {
					if (AudioComponentCopyName (Handle, &r) == 0)
						return CFString.FromHandle (r);
				}
				return null;
			}
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int /* OSStatus */ AudioComponentGetDescription (IntPtr component, AudioComponentDescription* desc);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioComponentDescription? Description {
			get {
				AudioComponentDescription desc;
				unsafe {
					if (AudioComponentGetDescription (Handle, &desc) == 0)
						return desc;
				}

				return null;
			}
		}

		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int /* OSStatus */ AudioComponentGetVersion (IntPtr component, int* /* UInt32* */ version);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public Version? Version {
			get {
				int ret;
				unsafe {
					if (AudioComponentGetVersion (Handle, &ret) == 0)
						return new Version (ret >> 16, (ret >> 8) & 0xff, ret & 0xff);
				}

				return null;
			}
		}

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern unsafe IntPtr AudioComponentCopyIcon (IntPtr comp);

		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public UIImage? CopyIcon ()
		{
			var ptr = AudioComponentCopyIcon (Handle);
			return Runtime.GetNSObject<UIImage> (ptr, owns: true);
		}

#if !MONOMAC
#if !__MACCATALYST__
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos14.0")]
		[ObsoletedOSPlatform ("ios14.0")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern IntPtr AudioComponentGetIcon (IntPtr comp, float /* float */ desiredPointSize);

		/// <param name="desiredPointSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos14.0", "Use 'CopyIcon' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'CopyIcon' instead.")]
		public UIKit.UIImage? GetIcon (float desiredPointSize)
		{
			return Runtime.GetNSObject<UIKit.UIImage> (AudioComponentGetIcon (Handle, desiredPointSize));
		}
#endif // !__MACCATALYST__

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
		[ObsoletedOSPlatform ("ios13.0")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern double AudioComponentGetLastActiveTime (IntPtr comp);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("maccatalyst14.0", "Use 'AudioUnit' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'AudioUnit' instead.")]
		public double LastActiveTime {
			get {
				return AudioComponentGetLastActiveTime (Handle);
			}
		}
#else
		// extern NSImage * __nullable AudioComponentGetIcon (AudioComponent __nonnull comp) __attribute__((availability(macosx, introduced=10.11)));
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos11.0")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern IntPtr AudioComponentGetIcon (IntPtr comp);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos11.0")]
		public AppKit.NSImage? GetIcon ()
		{
			return Runtime.GetNSObject<AppKit.NSImage> (AudioComponentGetIcon (Handle));
		}
#endif

#if IOS || MONOMAC
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int AudioComponentCopyConfigurationInfo (IntPtr /* AudioComponent */ inComponent, /* CFDictionaryRef** */ IntPtr* outConfigurationInfo);

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public NSDictionary? GetConfigurationInfo (out int resultCode)
		{
			IntPtr dictPtr;
			unsafe {
				resultCode = AudioComponentCopyConfigurationInfo (GetCheckedHandle (), &dictPtr);
			}
			if (resultCode == 0) {
				return Runtime.GetNSObject<NSDictionary> (dictPtr, owns: true);
			}
			return null;
		}

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public NSDictionary? GetConfigurationInfo () => GetConfigurationInfo (out var _);

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int AudioComponentValidate (IntPtr /* AudioComponent* */ inComponent, IntPtr /* CFDictionaryRef* */ inValidationParameters,
				AudioComponentValidationResult* outValidationResult);

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public AudioComponentValidationResult Validate (NSDictionary? validationParameters, out int resultCode)
		{
			AudioComponentValidationResult result;
			unsafe {
				resultCode = AudioComponentValidate (GetCheckedHandle (), validationParameters.GetHandle (), &result);
				GC.KeepAlive (validationParameters);
			}
			if (resultCode == 0)
				return result;
			return AudioComponentValidationResult.Unknown;
		}

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public AudioComponentValidationResult Validate (NSDictionary? validationParameters = null) => Validate (validationParameters, out var _);

		[UnmanagedCallersOnly]
		static void TrampolineAction (IntPtr blockPtr, AudioComponentValidationResult result, IntPtr dictionary)
		{
			var del = BlockLiteral.GetTarget<Action<AudioComponentValidationResult, NSDictionary?>> (blockPtr);
			if (del is not null)
				del (result, Runtime.GetNSObject<NSDictionary> (dictionary));
		}

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioUnitLibrary)]
		unsafe static extern int AudioComponentValidateWithResults (IntPtr /* AudioComponent* */ inComponent, IntPtr /* CFDictionaryRef* */ inValidationParameters, BlockLiteral* inCompletionHandler);

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public void ValidateAsync (NSDictionary? validationParameters,
				Action<AudioComponentValidationResult, NSDictionary?> onCompletion, out int resultCode)
		{
			if (onCompletion is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (onCompletion));

			unsafe {
				delegate* unmanaged<IntPtr, AudioComponentValidationResult, IntPtr, void> trampoline = &TrampolineAction;
				using var block = new BlockLiteral (trampoline, onCompletion, typeof (AudioComponent), nameof (TrampolineAction));
				resultCode = AudioComponentValidateWithResults (GetCheckedHandle (), validationParameters.GetHandle (), &block);
				GC.KeepAlive (validationParameters);
			}
		}

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void ValidateAsync (NSDictionary? validationParameters,
				Action<AudioComponentValidationResult, NSDictionary?> onCompletion) => ValidateAsync (validationParameters, onCompletion, out var _);

		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public void ValidateAsync (Action<AudioComponentValidationResult, NSDictionary?> onCompletion) => ValidateAsync (null, onCompletion, out var _);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern int /* OSStatus */ AudioUnitExtensionSetComponentList (IntPtr /* CFString */ extensionIdentifier, /* CFArrayRef */ IntPtr audioComponentInfo);

		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.AudioUnitLibrary)]
		static extern /* CFArrayRef */ IntPtr AudioUnitExtensionCopyComponentList (IntPtr /* CFString */ extensionIdentifier);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public AudioComponentInfo []? ComponentList {
			get {
				var nameHandle = CFString.CreateNative (Name);
				try {
					var cHandle = AudioUnitExtensionCopyComponentList (nameHandle);
					if (cHandle == IntPtr.Zero)
						return null;
					using (var nsArray = Runtime.GetNSObject<NSArray> (cHandle, owns: true)) {
						if (nsArray is null)
							return null;
						// make things easier for developers since we do not know how to have an implicit conversion from NSObject to AudioComponentInfo
						var dics = NSArray.FromArray<NSDictionary> (nsArray);
						var result = new AudioComponentInfo [dics.Length];
						for (var i = 0; i < result.Length; i++) {
							result [i] = new AudioComponentInfo (dics [i]);
						}
						return result;
					}
				} finally {
					CFString.ReleaseNative (nameHandle);
				}
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				var nameHandle = CFString.CreateNative (Name);
				try {
					var dics = new NSDictionary [value.Length];
					for (var i = 0; i < value.Length; i++) {
						dics [i] = value [i].Dictionary;
					}
					using (var array = NSArray.FromNSObjects (dics)) {
						var result = (AudioConverterError) AudioUnitExtensionSetComponentList (nameHandle, array.Handle);
						switch (result) {
						case AudioConverterError.None:
							return;
						default:
							throw new InvalidOperationException ($"ComponentList could not be set, error {result.ToString ()}");
						}
					}
				} finally {
					CFString.ReleaseNative (nameHandle);
				}
			}
		}

#endif // IOS || MONOMAC

#endif // !COREBUILD
	}

#if !COREBUILD
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class AudioComponentValidationParameter {
		//		#define kAudioComponentValidationParameter_ForceValidation		 "ForceValidation"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString ForceValidation = new NSString ("ForceValidation");

		//		#define kAudioComponentValidationParameter_LoadOutOfProcess		 "LoadOutOfProcess"
		[SupportedOSPlatform ("ios14.5")]
		[SupportedOSPlatform ("tvos14.5")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static NSString LoadOutOfProcess = new NSString ("LoadOutOfProcess");

		//		#define kAudioComponentValidationParameter_TimeOut				"TimeOut"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString TimeOut = new NSString ("TimeOut");
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static class AudioComponentConfigurationInfo {
		//		#define kAudioComponentConfigurationInfo_ValidationResult	"ValidationResult"
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static NSString ValidationResult = new NSString ("ValidationResult");
	}
#endif
}
