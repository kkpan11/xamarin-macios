//
// NWError.cs: Bindings the Netowrk nw_error API.
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWError : NativeObject {
		[Preserve (Conditional = true)]
		internal NWError (NativeHandle handle, bool owns) : base (handle, owns)
		{
		}

		[DllImport (Constants.NetworkLibrary)]
		static extern NWErrorDomain nw_error_get_error_domain (IntPtr error);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NWErrorDomain ErrorDomain => nw_error_get_error_domain (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern /* int */ int nw_error_get_error_code (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int ErrorCode => nw_error_get_error_code (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_error_copy_cf_error (IntPtr error);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CFException CFError {
			get {
				return CFException.FromCFError (nw_error_copy_cf_error (GetCheckedHandle ()), true);
			}
		}
	}
}
