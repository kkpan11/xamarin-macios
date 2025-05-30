//
// NWBrowseDescriptor.cs: Bindings the Network nw_browse_descriptor_t API.
//
// Authors:
//   Manuel de la Pena (mandel@microsoft.com)
//
// Copyrigh 2019 Microsoft Inc
//
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

using OS_nw_browse_descriptor = System.IntPtr;

namespace Network {
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("maccatalyst")]
	public class NWBrowserDescriptor : NativeObject {

		[Preserve (Conditional = true)]
		internal NWBrowserDescriptor (NativeHandle handle, bool owns) : base (handle, owns) { }

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_browse_descriptor nw_browse_descriptor_create_bonjour_service (IntPtr type, IntPtr domain);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_browse_descriptor nw_browse_descriptor_create_application_service (IntPtr application_service_name);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public static NWBrowserDescriptor CreateApplicationServiceName (string applicationServiceName)
		{
			if (applicationServiceName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (applicationServiceName));

			using var applicationServiceNamePtr = new TransientString (applicationServiceName);
			return new NWBrowserDescriptor (nw_browse_descriptor_create_application_service (applicationServiceNamePtr), owns: true);
		}

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[DllImport (Constants.NetworkLibrary)]
		static extern IntPtr nw_browse_descriptor_get_application_service_name (OS_nw_browse_descriptor descriptor);

		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		public string? ApplicationServiceName {
			get {
				var appNamePtr = nw_browse_descriptor_get_application_service_name (GetCheckedHandle ());
				return Marshal.PtrToStringAnsi (appNamePtr);
			}
		}

		public static NWBrowserDescriptor CreateBonjourService (string type, string? domain)
		{
			// domain can be null, type CANNOT	
			if (type is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (type));

			using var typePtr = new TransientString (type);
			using var domainPtr = new TransientString (domain);
			return new NWBrowserDescriptor (nw_browse_descriptor_create_bonjour_service (typePtr, domainPtr), owns: true);
		}

		public static NWBrowserDescriptor CreateBonjourService (string type) => CreateBonjourService (type, null);

		[DllImport (Constants.NetworkLibrary)]
		static extern byte nw_browse_descriptor_get_include_txt_record (OS_nw_browse_descriptor descriptor);

		[DllImport (Constants.NetworkLibrary)]
		static extern void nw_browse_descriptor_set_include_txt_record (OS_nw_browse_descriptor descriptor, byte include_txt_record);

		public bool IncludeTxtRecord {
			get => nw_browse_descriptor_get_include_txt_record (GetCheckedHandle ()) != 0;
			set => nw_browse_descriptor_set_include_txt_record (GetCheckedHandle (), value.AsByte ());
		}

		[DllImport (Constants.NetworkLibrary, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr nw_browse_descriptor_get_bonjour_service_type (OS_nw_browse_descriptor descriptor);

		public string? BonjourType
			=> Marshal.PtrToStringAnsi (nw_browse_descriptor_get_bonjour_service_type (GetCheckedHandle ()));

		[DllImport (Constants.NetworkLibrary, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr nw_browse_descriptor_get_bonjour_service_domain (OS_nw_browse_descriptor descriptor);

		public string? BonjourDomain
			=> Marshal.PtrToStringAnsi (nw_browse_descriptor_get_bonjour_service_domain (GetCheckedHandle ()));
	}
}
