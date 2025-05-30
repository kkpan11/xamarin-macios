//
// NWInterface.cs: Bindings the Netowrk nw_interface API.
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

using OS_nw_interface = System.IntPtr;

namespace Network {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWInterface : NativeObject {
		[Preserve (Conditional = true)]
		internal NWInterface (NativeHandle handle, bool owns) : base (handle, owns) { }

#if !COREBUILD
		[DllImport (Constants.NetworkLibrary)]
		static extern NWInterfaceType nw_interface_get_type (OS_nw_interface iface);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NWInterfaceType InterfaceType => nw_interface_get_type (GetCheckedHandle ());

		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_interface_get_name (OS_nw_interface iface);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name => Marshal.PtrToStringAnsi (nw_interface_get_name (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary)]
		static extern /* uint32_t */ uint nw_interface_get_index (OS_nw_interface iface);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint Index => nw_interface_get_index (GetCheckedHandle ());
#endif // !COREBUILD
	}
}
