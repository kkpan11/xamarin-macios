// 
// ABAddressFormatting.cs: 
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright (C) 2012 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace AddressBookUI {

	// http://developer.apple.com/library/ios/#DOCUMENTATION/AddressBookUI/Reference/AddressBookUI_Functions/Reference/reference.html#//apple_ref/c/func/ABCreateStringWithAddressDictionary
	/// <summary>Utility class that formats one of the <see cref="Foundation.NSDictionary" /> returned by the <see cref="AddressBook.ABPerson.GetVCards(AddressBook.ABPerson[])" /> method.</summary>
	///     <remarks>
	///       <para>This class works with the <see cref="Foundation.NSDictionary" />s that are returned by the <see cref="AddressBook.ABPerson.GetVCards(AddressBook.ABPerson[])" /> method, as shown in the following example:
	/// 		</para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// 				NSError err;
	/// 				var ab = ABAddressBook.Create(out err);
	/// 				if(err is null && ABAddressBook.GetAuthorizationStatus() == ABAuthorizationStatus.Authorized)
	/// 				{
	/// 					var person = ab.GetPeople()[0];
	/// 					var address = person.GetAddresses()[0];
	/// 					var formattedAddress = ABAddressFormatting.ToString(address.Value, false);
	/// 				}
	/// 			]]></code>
	///       </example>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static public class ABAddressFormatting {

		[DllImport (Constants.AddressBookUILibrary)]
		static extern IntPtr /* NSString */ ABCreateStringWithAddressDictionary (IntPtr /* NSDictionary */ address, byte addCountryName);

		/// <param name="address">To be added.</param>
		///         <param name="addCountryName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public string ToString (NSDictionary address, bool addCountryName)
		{
			if (address is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (address));

			NativeHandle addressHandle = address.Handle;
			using (NSString s = new NSString (ABCreateStringWithAddressDictionary (addressHandle, addCountryName ? (byte) 1 : (byte) 0))) {
				GC.KeepAlive (address);
				return s.ToString ();
			}
		}
	}
}
