//
// CFPropertyList.cs: partial internal binding for CFPropertyList
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2013 Xamarin, Inc.

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

namespace CoreFoundation {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFPropertyList : NativeObject {
		static nint CFDataTypeID = CFData.GetTypeID ();
		static nint CFStringTypeID = CFString.GetTypeID ();
		static nint CFArrayTypeID = CFArray.GetTypeID ();
		static nint CFDictionaryTypeID = CFDictionary.GetTypeID ();
		static nint CFBooleanTypeID = CFBoolean.GetTypeID ();

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern nint CFDateGetTypeID ();

		static nint CFDateTypeID = CFDateGetTypeID ();

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern nint CFNumberGetTypeID ();

		static nint CFNumberTypeID = CFNumberGetTypeID ();

		[Preserve (Conditional = true)]
		internal CFPropertyList (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe static extern IntPtr CFPropertyListCreateWithData (IntPtr allocator, IntPtr dataRef, nuint options, nint* format, /* CFError * */ IntPtr* error);

		/// <param name="data">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static (CFPropertyList? PropertyList, CFPropertyListFormat Format, NSError? Error)
			FromData (NSData data, CFPropertyListMutabilityOptions options = CFPropertyListMutabilityOptions.Immutable)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			if (data.Handle == IntPtr.Zero)
				throw new ObjectDisposedException (nameof (data));

			nint fmt;
			IntPtr error;
			IntPtr ret;
			unsafe {
				ret = CFPropertyListCreateWithData (IntPtr.Zero, data.Handle, (nuint) (ulong) options, &fmt, &error);
				GC.KeepAlive (data);
			}
			if (ret != IntPtr.Zero)
				return (new CFPropertyList (ret, owns: true), (CFPropertyListFormat) (long) fmt, null);
			return (null, CFPropertyListFormat.XmlFormat1, new NSError (error));
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFPropertyListCreateDeepCopy (IntPtr allocator, IntPtr propertyList, nuint mutabilityOption);

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CFPropertyList DeepCopy (CFPropertyListMutabilityOptions options = CFPropertyListMutabilityOptions.MutableContainersAndLeaves)
		{
			return new CFPropertyList (CFPropertyListCreateDeepCopy (IntPtr.Zero, Handle, (nuint) (ulong) options), owns: true);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe extern static /*CFDataRef*/IntPtr CFPropertyListCreateData (IntPtr allocator, IntPtr propertyList, nint format, nuint options, IntPtr* error);

		/// <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public (NSData? Data, NSError? Error) AsData (CFPropertyListFormat format = CFPropertyListFormat.BinaryFormat1)
		{
			IntPtr error;
			IntPtr x;
			unsafe {
				x = CFPropertyListCreateData (IntPtr.Zero, Handle, (nint) (long) format, 0, &error);
			}
			if (x == IntPtr.Zero)
				return (null, new NSError (error));
			return (Runtime.GetNSObject<NSData> (x, owns: true), null);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFPropertyListIsValid (IntPtr plist, nint format);

		/// <param name="format">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool IsValid (CFPropertyListFormat format)
		{
			return CFPropertyListIsValid (Handle, (nint) (long) format) != 0;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public object? Value {
			get {
				if (Handle == IntPtr.Zero) {
					return null;
				}

				var typeid = CFType.GetTypeID (Handle);

				if (typeid == CFDataTypeID) {
					return Runtime.GetNSObject<NSData> (Handle);
				} else if (typeid == CFStringTypeID) {
					return Runtime.GetNSObject<NSString> (Handle);
				} else if (typeid == CFArrayTypeID) {
					return Runtime.GetNSObject<NSArray> (Handle);
				} else if (typeid == CFDictionaryTypeID) {
					return Runtime.GetNSObject<NSDictionary> (Handle);
				} else if (typeid == CFDateTypeID) {
					return Runtime.GetNSObject<NSDate> (Handle);
				} else if (typeid == CFBooleanTypeID) {
					return (bool) Runtime.GetNSObject<NSNumber> (Handle);
				} else if (typeid == CFNumberTypeID) {
					return Runtime.GetNSObject<NSNumber> (Handle);
				}

				return null;
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum CFPropertyListFormat : long {
		/// <summary>To be added.</summary>
		OpenStep = 1,
		/// <summary>To be added.</summary>
		XmlFormat1 = 100,
		/// <summary>To be added.</summary>
		BinaryFormat1 = 200,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	[Native]
	public enum CFPropertyListMutabilityOptions : ulong {
		/// <summary>To be added.</summary>
		Immutable = 0,
		/// <summary>To be added.</summary>
		MutableContainers = 1 << 0,
		/// <summary>To be added.</summary>
		MutableContainersAndLeaves = 1 << 1,
	}
}
