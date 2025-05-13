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
using System.ComponentModel;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSWindow {

		// Automatically set ReleaseWhenClosed=false in every constructor.
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Obsolete ("Set 'TrackReleasedWhenClosed' and call 'ReleaseWhenClosed()' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static bool DisableReleasedWhenClosedInConstructor;

		// * If 'releasedWhenClosed' is true at the end of the constructor, automatically call 'retain'.
		// * Enable our own managed 'ReleaseWhenClosed (bool)' method (which will call 'retain'/'release' as needed).
		// * Don't do anything in Close (most importantly do not call Dispose).
		// If 'TrackReleasedWhenClosed' is not set, we default to 'false' until .NET 9, and then we default to 'true'.
		// Hopefully we can then remove the variable and just implement the tracking as the only behavior at some point
		// (XAMCORE_5_0 && NET10_0?)
		static bool? track_relased_when_closed;
		public static bool TrackReleasedWhenClosed {
			get {
#if NET9_0_OR_GREATER
				return track_relased_when_closed != false;
#else
				return track_relased_when_closed == true;
#endif
			}
			set {
				track_relased_when_closed = value;
			}
		}

		/// <param name="windowRef">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public NSWindow FromWindowRef (IntPtr windowRef)
		{
			return new NSWindow (windowRef);
		}

		void InitializeReleasedWhenClosed ()
		{
			if (TrackReleasedWhenClosed) {
				if (DangerousReleasedWhenClosed)
					DangerousRetain ();
			} else if (!DisableReleasedWhenClosedInConstructor) {
				DangerousReleasedWhenClosed = false;
			}
		}

		// Call DangerousRetain when 'ReleasedWhenClosed' changes from false to true.
		// Call DangerousRelease when 'ReleasedWhenClosed' changes from true to false.
		public void ReleaseWhenClosed (bool value = true)
		{
			if (!TrackReleasedWhenClosed)
				throw new InvalidOperationException ($"The NSWindow.{nameof (TrackReleasedWhenClosed)} field must be set to 'true' when calling this method.");

			if (DangerousReleasedWhenClosed == value)
				return;

			if (value) {
				DangerousRetain ();
			} else {
				DangerousRelease ();
			}
			DangerousReleasedWhenClosed = value;
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Close ()
		{
			if (TrackReleasedWhenClosed) {
				_Close ();
				return;
			}

			// Windows that do not have a WindowController use released_when_closed
			// if set to true, the call to Close will release the object, and we will
			// end up with a double free.
			//
			// If that is the case, we take a reference first, and to keep the behavior
			// we call Dispose after that.
			if (WindowController is null) {
				bool released_when_closed = DangerousReleasedWhenClosed;
				if (released_when_closed)
					CFObject.CFRetain (Handle);
				_Close ();
				if (released_when_closed)
					Dispose ();
			} else
				_Close ();
		}

		// note: if needed override the protected Get|Set methods
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string FrameAutosaveName {
			get { return GetFrameAutosaveName (); }
			// ignore return value (bool)
			set { SetFrameAutosaveName (value); }
		}

		/// <param name="mask">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSEvent NextEventMatchingMask (NSEventMask mask)
		{
			return NextEventMatchingMask ((uint) mask);
		}

		/// <param name="mask">To be added.</param>
		///         <param name="expiration">To be added.</param>
		///         <param name="mode">To be added.</param>
		///         <param name="deqFlag">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSEvent NextEventMatchingMask (NSEventMask mask, NSDate expiration, string mode, bool deqFlag)
		{
			return NextEventMatchingMask ((uint) mask, expiration, mode, deqFlag);
		}

		/// <param name="mask">To be added.</param>
		///         <param name="beforeLastEvent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void DiscardEventsMatchingMask (NSEventMask mask, NSEvent beforeLastEvent)
		{
			DiscardEventsMatchingMask ((uint) mask, beforeLastEvent);
		}

		// NSString NSWindowDidBecomeKeyNotification;
		// NSString NSWindowDidBecomeMainNotification;
		// NSString NSWindowDidChangeScreenNotification;
		// NSString NSWindowDidDeminiaturizeNotification;
		// NSString NSWindowDidExposeNotification;
		// NSString NSWindowDidMiniaturizeNotification;
		// NSString NSWindowDidMoveNotification;
		// NSString NSWindowDidResignKeyNotification;
		// NSString NSWindowDidResignMainNotification;
		// NSString NSWindowDidResizeNotification;
		// NSString NSWindowDidUpdateNotification;
		// NSString NSWindowWillCloseNotification;
		// NSString NSWindowWillMiniaturizeNotification;
		// NSString NSWindowWillMoveNotification;
		// NSString NSWindowWillBeginSheetNotification;
		// NSString NSWindowDidEndSheetNotification;
		// NSString NSWindowDidChangeScreenProfileNotification
		// NSString NSWindowWillStartLiveResizeNotification
		// NSString NSWindowDidEndLiveResizeNotification


	}
}
#endif // !__MACCATALYST__
