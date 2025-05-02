//
// Copyright 2014 Xamarin Inc
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//

// 'AVAudioChannelLayout' defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable 0660
// 'AVAudioChannelLayout' defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable 0661
// In both of these cases, the NSObject Equals/GetHashCode implementation works fine, so we can ignore these warnings.

using Foundation;
using ObjCRuntime;
using AudioToolbox;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable

namespace AVFoundation {
	/// <summary>Corresponds to a <see cref="AudioToolbox.AudioChannelLayout " /> channel layout.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AVFoundation/Reference/AVAudioChannelLayout_Class/index.html">Apple documentation for <c>AVAudioChannelLayout</c></related>
	public partial class AVAudioChannelLayout {
		static IntPtr CreateLayoutPtr (AudioChannelLayout layout, out IntPtr handleToLayout)
		{
			int size;
			handleToLayout = layout.ToBlock (out size);
			return handleToLayout;
		}

		/// <param name="layout">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DesignatedInitializer]
		public AVAudioChannelLayout (AudioChannelLayout layout)
			: this (CreateLayoutPtr (layout, out var handleToLayout))
		{
			Marshal.FreeHGlobal (handleToLayout);
		}

		/// <summary>The underlying <see cref="AudioToolbox.AudioChannelLayout" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AudioChannelLayout? Layout {
			get {
				return AudioChannelLayout.FromHandle (_Layout);
			}
		}

		public static bool operator == (AVAudioChannelLayout a, AVAudioChannelLayout b)
		{
			return a.Equals (b);
		}

		public static bool operator != (AVAudioChannelLayout a, AVAudioChannelLayout b)
		{
			return !a.Equals (b);
		}
	}
}
