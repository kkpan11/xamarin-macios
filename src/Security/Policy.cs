// 
// Policy.cs: Implements the managed SecPolicy wrapper.
//
// Authors: 
//	Miguel de Icaza
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2010 Novell, Inc
// Copyright 2012-2014 Xamarin Inc.
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Security {

	/// <summary>Encapsulates a security policy. A policy comprises a set of rules that specify how to evaluate a certificate for a certain level of trust.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecPolicy : NativeObject {
#if !NET
		public SecPolicy (NativeHandle handle)
			: base (handle, false, true)
		{
		}
#endif

		[Preserve (Conditional = true)]
		internal SecPolicy (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* SecPolicyRef */ SecPolicyCreateSSL (byte server, IntPtr /* CFStringRef */ hostname);

		/// <param name="server">Indicate if the policy is for a server (true) or client (false) certificate.</param>
		///         <param name="hostName">The server host name on which the policy will be applied.</param>
		///         <summary>Create a policy instance that represent the SSL/TLS profile.</summary>
		///         <returns>A SecPolicy instance that can be used to validate a SecCertificate using SecTrust.</returns>
		///         <remarks>
		///         </remarks>
		static public SecPolicy CreateSslPolicy (bool server, string hostName)
		{
			var handle = CFString.CreateNative (hostName);
			try {
				return new SecPolicy (SecPolicyCreateSSL (server.AsByte (), handle), true);
			} finally {
				CFString.ReleaseNative (handle);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* SecPolicyRef */ SecPolicyCreateBasicX509 ();

		/// <summary>Create a policy instance that represent the basic X.509 certificate profile.</summary>
		///         <returns>A SecPolicy instance that can be used to validate a SecCertificate using SecTrust.</returns>
		///         <remarks>
		///         </remarks>
		static public SecPolicy CreateBasicX509Policy ()
		{
			return new SecPolicy (SecPolicyCreateBasicX509 (), true);
		}

		/// <summary>Type identifier for the Security.SecPolicy type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="M:CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isSecPolicy = (CFType.GetTypeID (foo.Handle) == SecPolicy.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		[DllImport (Constants.SecurityLibrary, EntryPoint = "SecPolicyGetTypeID")]
		public extern static nint GetTypeID ();

#if !NET
		public static bool operator == (SecPolicy? a, SecPolicy? b)
		{
			if (a is null)
				return b is null;
			else if (b is null)
				return false;

			return a.Handle == b.Handle;
		}

		public static bool operator != (SecPolicy? a, SecPolicy? b)
		{
			if (a is null)
				return b is not null;
			else if (b is null)
				return true;
			return a.Handle != b.Handle;
		}

		// For the .net profile `DisposableObject` implements both
		// `Equals` and `GetHashCode` based on the Handle property.
		public override bool Equals (object? other)
		{
			var o = other as SecPolicy;
			return this == o;
		}

		public override int GetHashCode ()
		{
			return ((IntPtr) Handle).ToInt32 ();
		}
#endif
	}
}
