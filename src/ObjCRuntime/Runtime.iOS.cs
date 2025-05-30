
#nullable enable

#if !MONOMAC

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;
using Registrar;
using UIKit;

namespace ObjCRuntime {

	/// <summary>Provides information about the Xamarin.iOS Runtime.</summary>
	///     <remarks>
	///     </remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/SysSound/">SysSound</related>
	public static partial class Runtime {
#if !COREBUILD
#if TVOS
		internal const string ProductName = "Microsoft.tvOS";
#elif IOS
		internal const string ProductName = "Microsoft.iOS";
#else
#error Unknown platform
#endif
#if TVOS
		internal const string AssemblyName = "Microsoft.tvOS.dll";
#elif IOS
		internal const string AssemblyName = "Microsoft.iOS.dll";
#else
#error Unknown platform
#endif

#if !__MACCATALYST__
		/// <summary>The architecture where the code is currently running.</summary>
		///         <remarks>
		///           <para>Use this to determine the architecture on which the program is currently running (device or simulator).</para>
		///         </remarks>
		public readonly static Arch Arch = (Arch) GetRuntimeArch ();
#endif

		unsafe static void InitializePlatform (InitializationOptions* options)
		{
			UIApplication.Initialize ();
		}

#if !__MACCATALYST__
		[SuppressGCTransition] // The native function is a single "return <constant>;" so this should be safe.
		[DllImport ("__Internal")]
		static extern int xamarin_get_runtime_arch ();

		// The linker will replace the contents of this method with constant return value depending on the circumstances.
		// The linker will not do that with P/Invokes (https://github.com/dotnet/linker/issues/2586), so
		// we need an indirection here. The P/Invoke itself will be removed by the linker once the contents
		// of this method have been replaced with a constant value.
		static int GetRuntimeArch ()
		{
			return xamarin_get_runtime_arch ();
		}
#endif

#if TVOS || __MACCATALYST__
		[Advice ("This method is present only to help porting code.")]
		public static void StartWWAN (Uri uri, Action<Exception?> callback)
		{
			NSRunLoop.Main.BeginInvokeOnMainThread (() => callback (null));
		}

		[Advice ("This method is present only to help porting code.")]
		public static void StartWWAN (Uri uri)
		{
		}
#else
		/// <param name="uri">Uri to probe to start the WWAN connection.</param>
		///         <param name="callback">Callback that will be called when the WWAN connection has been started up. This callback will be invoked on the main thread. If there was an exception while trying to start the WWAN, it will be passed to the callback, otherwise null is passed.</param>
		///         <summary>This method forces the WAN network access to be woken up asynchronously.</summary>
		///         <remarks>
		///           <para>When the phone is not on WiFi, this will force the networking stack to start.</para>
		///         </remarks>
		public static void StartWWAN (Uri uri, Action<Exception?> callback)
		{
			if (uri is null)
				throw new ArgumentNullException (nameof (uri));

			if (callback is null)
				throw new ArgumentNullException (nameof (callback));

			DispatchQueue.DefaultGlobalQueue.DispatchAsync (() => {
				Exception? ex = null;
				try {
					StartWWAN (uri);
				} catch (Exception x) {
					ex = x;
				}

				NSRunLoop.Main.BeginInvokeOnMainThread (() => callback (ex));
			});
		}

		[DllImport ("__Internal")]
		static extern void xamarin_start_wwan (IntPtr uri);

		/// <param name="uri">Uri to probe to start the WWAN connection.</param>
		///         <summary>This method forces the WAN network access to be woken up.</summary>
		///         <remarks>
		///           <para>When the phone is not on WiFi, this will force the networking stack to start.</para>
		///         </remarks>
		public static void StartWWAN (Uri uri)
		{
			if (uri is null)
				throw new ArgumentNullException (nameof (uri));

			if (uri.Scheme != "http" && uri.Scheme != "https")
				throw new ArgumentException ("uri is not a valid http or https uri", uri.ToString ());

			if (Runtime.Arch == Arch.SIMULATOR)
				return;

			using var uriPtr = new TransientString (uri.ToString ());
			xamarin_start_wwan (uriPtr);
		}
#endif // !TVOS
#endif // !COREBUILD
	}

#if !__MACCATALYST__
	/// <summary>Used to represent the host on which this app is running.</summary>
	public enum Arch {
		/// <summary>Running on a physical device.</summary>
		DEVICE,
		/// <summary>Running on the desktop simulator.</summary>
		SIMULATOR,
	}
#endif
}

#endif // MONOMAC
