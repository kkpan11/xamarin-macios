//
// NSThread.cs: extensions for NSThread
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin Inc

#if MONOMAC

using System;

namespace Foundation {
	public partial class NSThread {
		class ActionThread : NSThread {
			Action action;

			public ActionThread (Action action)
			{
				this.action = action;
			}

			public override void Main ()
			{
				action ();
			}
		}

		/// <param name="action">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSThread Start (Action action)
		{
			if (action is null) {
				throw new ArgumentNullException ("action");
			}

			var thread = new ActionThread (action);
			thread.Start ();
			return thread;
		}
	}
}

#endif // MONOMAC
