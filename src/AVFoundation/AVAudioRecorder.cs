// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2014 Xamarin Inc
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

using Foundation;
using CoreFoundation;
using AudioToolbox;
using ObjCRuntime;
using System;

#nullable enable

namespace AVFoundation {
	public partial class AVAudioRecorder {
		/// <summary>Create a new <see cref="AVAudioRecorder" /> instance.</summary>
		/// <param name="url">The url for the new <see cref="AVAudioRecorder" /> instance.</param>
		/// <param name="settings">The settings for the new <see cref="AVAudioRecorder" /> instance.</param>
		/// <param name="error">Returns the error if creating a new <see cref="AVAudioRecorder" /> instance fails.</param>
		/// <returns>A newly created <see cref="AVAudioRecorder" /> instance if successful, null otherwise.</returns>
		public static AVAudioRecorder? Create (NSUrl url, AudioSettings settings, out NSError? error)
		{
			if (settings is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (settings));
			var rv = new AVAudioRecorder (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithUrl (url, settings.Dictionary, out error), string.Empty, false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create a new <see cref="AVAudioRecorder" /> instance.</summary>
		/// <param name="url">The url for the new <see cref="AVAudioRecorder" /> instance.</param>
		/// <param name="format">The format for the new <see cref="AVAudioRecorder" /> instance.</param>
		/// <param name="error">Returns the error if creating a new <see cref="AVAudioRecorder" /> instance fails.</param>
		/// <returns>A newly created <see cref="AVAudioRecorder" /> instance if successful, null otherwise.</returns>
		public static AVAudioRecorder? Create (NSUrl url, AVAudioFormat? format, out NSError? error)
		{
			if (format is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (format));
			var rv = new AVAudioRecorder (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithUrl (url, format, out error), string.Empty, false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
