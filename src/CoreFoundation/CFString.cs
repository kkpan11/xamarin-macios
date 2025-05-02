//
// CFBase.cs: Contains base types
//
// Authors:
//    Miguel de Icaza (miguel@novell.com)
//    Rolf Bjarne Kvinge (rolf@xamarin.com)
//
// Copyright 2012 Xamarin Inc
//
// The class can be either constructed from a string (from user code)
// or from a handle (from iphone-sharp.dll internal calls).  This
// delays the creation of the actual managed string until actually
// required
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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

#nullable enable

namespace CoreFoundation {
	/// <summary>Represents a range from two integers: location and length.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CFRange {
		nint loc; // defined as 'long' in native code
		nint len; // defined as 'long' in native code

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int Location {
			get { return (int) loc; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int Length {
			get { return (int) len; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long LongLocation {
			get { return (long) loc; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public long LongLength {
			get { return (long) len; }
		}

		/// <param name="loc">To be added.</param>
		///         <param name="len">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CFRange (int loc, int len)
		{
			this.loc = loc;
			this.len = len;
		}

		/// <param name="l">To be added.</param>
		///         <param name="len">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CFRange (long l, long len)
		{
			this.loc = (nint) l;
			this.len = (nint) len;
		}

		public CFRange (nint l, nint len)
		{
			this.loc = l;
			this.len = len;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return string.Format ("CFRange [Location: {0} Length: {1}]", loc, len);
		}
	}

	// nothing is exposed publicly
	/// <summary>Base class for CoreFoundation objects.</summary>
	///     <remarks>To be added.</remarks>
	internal static class CFObject {

		[DllImport (Constants.CoreFoundationLibrary)]
		internal extern static void CFRelease (IntPtr obj);

		[DllImport (Constants.CoreFoundationLibrary)]
		internal extern static IntPtr CFRetain (IntPtr obj);

		/// <summary>Does nothing if <paramref name="obj" /> is IntPtr.Zero, otherwise calls CFRelease.</summary>
		internal static void SafeRelease (IntPtr obj)
		{
			if (obj == IntPtr.Zero)
				return;
			CFRelease (obj);
		}

		/// <summary>Does nothing if <paramref name="obj" /> is IntPtr.Zero, otherwise calls CFRetain.</summary>
		internal static IntPtr SafeRetain (IntPtr obj)
		{
			if (obj == IntPtr.Zero)
				return obj;
			return CFRetain (obj);
		}
	}

	/// <summary>String class used by C-only Cocoa APIs.</summary>
	///     <remarks>Use this class for creating strings that must be passed to methods in the low-level MonoTouch.CoreGraphics API.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFString
#if !COREBUILD
		: NativeObject
#endif
	{
#if !COREBUILD
		internal string? str;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected CFString () { }

		[DllImport (Constants.CoreFoundationLibrary, CharSet = CharSet.Unicode)]
		extern static IntPtr CFStringCreateWithCharacters (IntPtr allocator, IntPtr str, nint count);

		[DllImport (Constants.CoreFoundationLibrary, CharSet = CharSet.Unicode)]
		extern static nint CFStringGetLength (IntPtr handle);

		[DllImport (Constants.CoreFoundationLibrary, CharSet = CharSet.Unicode)]
		extern static unsafe char* CFStringGetCharactersPtr (IntPtr handle);

		[DllImport (Constants.CoreFoundationLibrary, CharSet = CharSet.Unicode)]
		extern static unsafe IntPtr CFStringGetCharacters (IntPtr handle, CFRange range, char* buffer);

		public static NativeHandle CreateNative (string? value)
		{
			if (value is null)
				return NativeHandle.Zero;

			using var valuePtr = new TransientString (value, TransientString.Encoding.Unicode);
			return CFStringCreateWithCharacters (IntPtr.Zero, valuePtr, value.Length);
		}

		public static void ReleaseNative (NativeHandle handle)
		{
			if (handle != NativeHandle.Zero)
				CFObject.CFRelease (handle);
		}

		/// <param name="str">To be added.</param>
		///         <summary>Creates a CFString from a C# string.</summary>
		///         <remarks>To be added.</remarks>
		public CFString (string str)
		{
			if (str is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (str));

			using var strPtr = new TransientString (str, TransientString.Encoding.Unicode);
			Handle = CFStringCreateWithCharacters (IntPtr.Zero, strPtr, str.Length);
			this.str = str;
		}

		/// <summary>Type identifier for the CoreFoundation.CFString type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isCFString = (CFType.GetTypeID (foo.Handle) == CFString.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint = "CFStringGetTypeID")]
		public extern static nint GetTypeID ();

		[Preserve (Conditional = true)]
		internal CFString (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		// to be used when an API like CF*Get* returns a CFString
		public static string? FromHandle (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			int l = (int) CFStringGetLength (handle);
			if (l == 0)
				return String.Empty;

			string str;
			bool allocate_memory = false;
			CFRange r = new CFRange (0, l);
			unsafe {
				// this returns non-null only if the string can be represented as unicode
				char* u = CFStringGetCharactersPtr (handle);
				if (u is null) {
					// alloc short string on the stack, otherwise use the heap
					allocate_memory = l > 128;
					// var m = allocate_memory ? (char*) Marshal.AllocHGlobal (l * 2) : stackalloc char [l];
					// this ^ won't compile so...
					if (allocate_memory) {
						u = (char*) Marshal.AllocHGlobal (l * 2);
					} else {
						// `u = stackalloc char [l];` won't compile either, even with cast
						char* u2 = stackalloc char [l];
						u = u2;
					}
					CFStringGetCharacters (handle, r, u);
				}
				str = new string (u, 0, l);
				if (allocate_memory)
					Marshal.FreeHGlobal ((IntPtr) u);
			}
			return str;
		}

		// to be used when an API like CF*Copy* returns a CFString
		public static string? FromHandle (NativeHandle handle, bool releaseHandle)
		{
			var s = FromHandle (handle);
			if (releaseHandle && (handle != IntPtr.Zero))
				CFObject.CFRelease (handle);
			return s;
		}

		public static implicit operator string? (CFString? x)
		{
			if (x is null)
				return null;

			if (x.str is null) {
				x.str = FromHandle (x.Handle);
				GC.KeepAlive (x);
			}

			return x.str;
		}

		[return: NotNullIfNotNull ("s")]
		public static implicit operator CFString? (string? s)
		{
			if (s is null)
				return null;

			return new CFString (s);
		}

		/// <summary>String length.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int Length {
			get {
				if (str is not null)
					return str.Length;
				else
					return (int) CFStringGetLength (Handle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static ushort CFStringGetCharacterAtIndex (IntPtr handle, nint p);

		public char this [nint p] {
			get {
				if (str is not null)
					return str [(int) p];
				else
					return (char) CFStringGetCharacterAtIndex (Handle, p);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			if (str is null)
				str = FromHandle (Handle);
			return str ?? base.ToString ()!;
		}
#endif // !COREBUILD
	}
}
