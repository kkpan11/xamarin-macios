//
// NSButton.cs: Support for the NSButton class
//
// Author:
//   Michael Hutchinson (mhutchinson@novell.com)
//
// Copyright 2010, Novell, Inc.
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

#if !__MACCATALYST__

using System;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace AppKit {

	public partial class NSSlider {
		NSActionDispatcher? dispatcher;

		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static NSSlider FromTarget (Action action)
		{
			var dispatcher = new NSActionDispatcher (action);
			var control = _FromTarget (dispatcher, NSActionDispatcher.Selector);
			control.dispatcher = dispatcher;
			return control;
		}

		/// <param name="value">To be added.</param>
		///         <param name="minValue">To be added.</param>
		///         <param name="maxValue">To be added.</param>
		///         <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static NSSlider FromValue (double value, double minValue, double maxValue, Action action)
		{
			var dispatcher = new NSActionDispatcher (action);
			var control = _FromValue (value, minValue, maxValue, dispatcher, NSActionDispatcher.Selector);
			control.dispatcher = dispatcher;
			return control;
		}
	}
}
#endif // !__MACCATALYST__
