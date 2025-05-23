// 
// ABSource.cs: Implements the managed ABSource
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright (C) 2012 Xamarin Inc.
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
//

#nullable enable

#if !MONOMAC

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace AddressBook {

	/// <summary>A data source that produces address book data. (See <see cref="AddressBook.ABSourceType" />.)</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	public class ABSource : ABRecord {
		[Preserve (Conditional = true)]
		internal ABSource (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		internal ABSource (NativeHandle handle, ABAddressBook addressbook)
			: base (handle, false)
		{
			AddressBook = addressbook;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get { return PropertyToString (ABSourcePropertyId.Name); }
			set { SetValue (ABSourcePropertyId.Name, value); }
		}

		// Type is already a property in ABRecord
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public ABSourceType SourceType {
			get { return (ABSourceType) (int) PropertyTo<NSNumber> (ABSourcePropertyId.Type); }
			set { SetValue (ABSourcePropertyId.Type, new NSNumber ((int) value)); }
		}
	}

	[SupportedOSPlatform ("ios")]
	[ObsoletedOSPlatform ("ios", "Use the 'Contacts' API instead.")]
	[SupportedOSPlatform ("maccatalyst")]
	[ObsoletedOSPlatform ("maccatalyst", "Use the 'Contacts' API instead.")]
	[UnsupportedOSPlatform ("macos")]
	[UnsupportedOSPlatform ("tvos")]
	static class ABSourcePropertyId {

		public static int Name { get; private set; }
		public static int Type { get; private set; }

		static ABSourcePropertyId ()
		{
			InitConstants.Init ();
		}

		internal static void Init ()
		{
			var handle = Libraries.AddressBook.Handle;
			Name = Dlfcn.GetInt32 (handle, "kABSourceNameProperty");
			Type = Dlfcn.GetInt32 (handle, "kABSourceTypeProperty");
		}

		public static int ToId (ABSourceProperty property)
		{
			switch (property) {
			case ABSourceProperty.Name:
				return Name;
			case ABSourceProperty.Type:
				return Type;
			}
			throw new NotSupportedException ("Invalid ABSourceProperty value: " + property);
		}

		public static ABSourceProperty ToSourceProperty (int id)
		{
			if (id == Name)
				return ABSourceProperty.Name;
			if (id == Type)
				return ABSourceProperty.Type;
			throw new NotSupportedException ("Invalid ABSourcePropertyId value: " + id);
		}
	}
}

#endif // !MONOMAC
