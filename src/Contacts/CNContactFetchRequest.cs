//
// CNContactFetchRequest.cs:
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace Contacts {
	public partial class CNContactFetchRequest {

		/// <param name="keysToFetch">To be added.</param>
		///         <summary>Creates and returns a new <see cref="Contacts.CNContactFetchRequest" /> that retrieves data with the specified <paramref name="keysToFetch" />.</summary>
		///         <remarks>To be added.</remarks>
		public CNContactFetchRequest (params ICNKeyDescriptor [] keysToFetch)
			: this (NSArray.FromNativeObjects (keysToFetch))
		{
		}

		/// <param name="keysToFetch">To be added.</param>
		///         <summary>Creates a new <see cref="Contacts.CNContactFetchRequest" /> that retrieves data with the specified <paramref name="keysToFetch" />.</summary>
		///         <remarks>To be added.</remarks>
		public CNContactFetchRequest (params NSString [] keysToFetch)
			: this (NSArray.FromNSObjects (keysToFetch))
		{
		}

		// ICNKeyDescriptor == NSObjectProtocol, NSSecureCoding, NSCopying
		// but a ctor using this (ICNKeyDescriptor) would not accept NSString
		// so if you want to mix both NSString and (NSObjectProtocol, NSSecureCoding, NSCopying) you need to use
		// this constructor, which will manually verify the requirements (at runtime, not a compile time)
		/// <param name="keysToFetch">To be added.</param>
		///         <summary>Creates a new <see cref="Contacts.CNContactFetchRequest" /> that retrieves data with the specified <paramref name="keysToFetch" />.</summary>
		///         <remarks>To be added.</remarks>
		public CNContactFetchRequest (params INativeObject [] keysToFetch)
			: this (Validate (keysToFetch))
		{
		}

		// do not initialize them before they are needed, that way they won't show up in the .cctor code and the linker
		// will be able to eliminate them (along with other pieces) if the convenience .ctor is not used
		static IntPtr nsobject;
		static IntPtr nssecurecoding;
		static IntPtr nscopying;

		// NSObject.ConformsToProtocol won't work for *Wrapper types, like what returns ICNKeyDescriptor instances
		static bool ConformsToProtocol (IntPtr handle, IntPtr protocol)
		{
			return Messaging.bool_objc_msgSend_IntPtr (handle, Selector.GetHandle ("conformsToProtocol:"), protocol) != 0;
		}

		static NSArray Validate (params INativeObject [] keysToFetch)
		{
			if (nsobject == IntPtr.Zero)
				nsobject = Protocol.objc_getProtocol ("NSObject");
			if (nssecurecoding == IntPtr.Zero)
				nssecurecoding = Protocol.objc_getProtocol ("NSSecureCoding");
			if (nscopying == IntPtr.Zero)
				nscopying = Protocol.objc_getProtocol ("NSCopying");

			foreach (var key in keysToFetch) {
				var h = key.Handle;
				if (!ConformsToProtocol (h, nsobject) || !ConformsToProtocol (h, nssecurecoding) || !ConformsToProtocol (h, nscopying))
					throw new InvalidOperationException ("Keys do not conform to ICNKeyDescriptor");
			}
			return NSArray.FromNativeObjects (keysToFetch);
		}
	}
}
