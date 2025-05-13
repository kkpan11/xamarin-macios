//
// UIApplication.cs: Extensions to UIApplication
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
// Copyright 2014, Xamarin Inc.
// Copyright 2019 Microsoft Corporation.
//

using System;
using System.ComponentModel;
using System.Threading;
using ObjCRuntime;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;

#nullable enable

namespace UIKit {
	/// <include file="../../docs/api/UIKit/UIKitThreadAccessException.xml" path="/Documentation/Docs[@DocId='T:UIKit.UIKitThreadAccessException']/*" />
	public class UIKitThreadAccessException : Exception {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIKitThreadAccessException () : base ("UIKit Consistency error: you are calling a UIKit method that can only be invoked from the UI thread.")
		{
		}
	}

	public partial class UIApplication
	: UIResponder {
		static Thread? mainThread;
		/// <summary>Determines whether the debug builds of MonoTouch will enforce that calls done to UIKit are only issued from the UI thread.</summary>
		///         <remarks>
		///           <para>
		///             On debug builds, MonoTouch will enforce that calls made to
		///             UIKit APIs are only done from the UIKit thread.  This is
		///             useful to spot code that could inadvertently use UIKit from
		///             a non-UI thread which can corrupt the UIKit state and could
		///             lead to very hard to debug problems.
		///           </para>
		///           <para>
		///             But sometimes it might be useful to disable this check,
		///             either because you can ensure that UIKit is not in use at
		///             this point or because MonoTouch might be enforcing the
		///             checks in APIs that might have later been relaxed or made
		///             thread safe by iOS.
		///
		///           </para>
		///         </remarks>
		public static bool CheckForIllegalCrossThreadCalls = true;
		/// <summary>If <see langword="true" />, the system will try to diagnose potential mistakes where events and delegate-object overrides are in conflict.</summary>
		///         <remarks>To be added.</remarks>
		public static bool CheckForEventAndDelegateMismatches = true;

		// We link with __Internal here so that this function is interposable from third-party native libraries.
		// See: https://github.com/xamarin/MicrosoftInTune/issues/3 for an example.
		[DllImport ("__Internal")]
		unsafe extern static int xamarin_UIApplicationMain (int argc, /* char[]* */ IntPtr argv, /* NSString* */ IntPtr principalClassName, /* NSString* */ IntPtr delegateClassName, IntPtr* gchandle);

		static int UIApplicationMain (int argc, /* char[]* */ string []? argv, /* NSString* */ IntPtr principalClassName, /* NSString* */ IntPtr delegateClassName)
		{
			var strArr = TransientString.AllocStringArray (argv);
			IntPtr gchandle;
			int rv;
			unsafe {
				rv = xamarin_UIApplicationMain (argc, strArr, principalClassName, delegateClassName, &gchandle);
			}
			TransientString.FreeStringArray (strArr, argv?.Length ?? 0);
			Runtime.ThrowException (gchandle);
			return rv;
		}

		// called from NSExtension.Initialize (so other, future stuff, can be added if needed)
		// NOTE: must be called from the main thread, e.g. for extensions
		internal static void Initialize ()
		{
			if (mainThread is not null)
				return;

			SynchronizationContext.SetSynchronizationContext (new UIKitSynchronizationContext ());
			mainThread = Thread.CurrentThread;
		}

		/// <include file="../../docs/api/UIKit/UIApplication.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIApplication.Main(System.String[],System.String,System.String)']/*" />
		[Obsolete ("Use the overload with 'Type' instead of 'String' parameters for type safety.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public static void Main (string []? args, string? principalClassName, string? delegateClassName)
		{
			using var p = new TransientCFString (principalClassName);
			using var d = new TransientCFString (delegateClassName);
			Initialize ();
			UIApplicationMain (args?.Length ?? 0, args, p, d);
		}

		/// <include file="../../docs/api/UIKit/UIApplication.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIApplication.Main(System.String[],System.Type,System.Type)']/*" />
		public static void Main (string []? args, Type? principalClass, Type? delegateClass)
		{
			using var p = new TransientCFString (principalClass is null ? null : new Class (principalClass).Name);
			using var d = new TransientCFString (delegateClass is null ? null : new Class (delegateClass).Name);
			Initialize ();
			UIApplicationMain (args?.Length ?? 0, args, p, d);
		}

		/// <param name="args">Command line parameters from the Main program.</param>
		///         <summary>Launches the main application loop with the given command line parameters.</summary>
		///         <remarks>This launches the main application loop, assumes that the main application class is UIApplication, and uses the UIApplicationDelegate instance specified in the main NIB file for this program.</remarks>
		public static void Main (string []? args)
		{
			Initialize ();
			UIApplicationMain (args?.Length ?? 0, args, IntPtr.Zero, IntPtr.Zero);
		}

		/// <summary>Assertion to ensure that this call is being done from the UIKit thread.</summary>
		///         <remarks>
		///           <para>
		///             This method is used internally by MonoTouch to ensure that
		///             accesses done to UIKit classes and methods are only
		///             performed from the UIKit thread.  This is necessary because
		///             the UIKit API is not thread-safe and accessing it from
		///             multiple threads will corrupt the application state and will
		///             likely lead to a crash that is hard to identify.
		///           </para>
		///           <para>
		///             MonoTouch only performs the thread checks in debug builds.
		///             Release builds have this feature disabled.
		///
		///           </para>
		///         </remarks>
		public static void EnsureUIThread ()
		{
			// note: some extensions, like keyboards, won't call Main (and set mainThread)
			// FIXME: do better than disabling the feature
			if (CheckForIllegalCrossThreadCalls && (mainThread is not null) && (mainThread != Thread.CurrentThread))
				throw new UIKitThreadAccessException ();
		}

		internal static void EnsureEventAndDelegateAreNotMismatched (object del, Type expectedType)
		{
			if (CheckForEventAndDelegateMismatches && !(expectedType.IsAssignableFrom (del.GetType ())))
				throw new InvalidOperationException (string.Format ("Event registration is overwriting existing delegate. Either just use events or your own delegate: {0} {1}", del.GetType (), expectedType));
		}

		internal static void EnsureDelegateAssignIsNotOverwritingInternalDelegate (object? currentDelegateValue, object? newDelegateValue, Type internalDelegateType)
		{
			if (UIApplication.CheckForEventAndDelegateMismatches && currentDelegateValue is not null && newDelegateValue is not null
				&& currentDelegateValue.GetType ().IsAssignableFrom (internalDelegateType)
				&& !newDelegateValue.GetType ().IsAssignableFrom (internalDelegateType))
				throw new InvalidOperationException (string.Format ("Event registration is overwriting existing delegate. Either just use events or your own delegate: {0} {1}", newDelegateValue.GetType (), internalDelegateType));
		}
	}

	/// <summary>Provides data for the  event.</summary>
	///     <remarks>
	///     </remarks>
	public partial class UIContentSizeCategoryChangedEventArgs {
		/// <summary>The new size of the content, e.g., the new font size, in points.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public UIContentSizeCategory NewValue {
			get {
				return UIContentSizeCategoryExtensions.GetValue (WeakNewValue);
			}
		}
	}
}
