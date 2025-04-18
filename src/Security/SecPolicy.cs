// 
// SecPolicy.cs: Implements the managed SecPolicy wrapper.
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	/// <summary>Encapsulates a security policy. A policy comprises a set of rules that specify how to evaluate a certificate for a certain level of trust.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecPolicy {

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable CFDictionaryRef */ SecPolicyCopyProperties (IntPtr /* SecPolicyRef */ policyRef);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSDictionary? GetProperties ()
		{
			var dict = SecPolicyCopyProperties (Handle);
			return Runtime.GetNSObject<NSDictionary> (dict, true);
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable SecPolicyRef */ SecPolicyCreateRevocation (/* CFOptionFlags */ nuint revocationFlags);

#if NET
		/// <param name="revocationFlags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		static public SecPolicy? CreateRevocationPolicy (SecRevocation revocationFlags)
		{
			var policy = SecPolicyCreateRevocation ((nuint) (ulong) revocationFlags);
			return policy == IntPtr.Zero ? null : new SecPolicy (policy, true);
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* __nullable SecPolicyRef */ SecPolicyCreateWithProperties (IntPtr /* CFTypeRef */ policyIdentifier, IntPtr /* CFDictionaryRef */ properties);

#if NET
		/// <param name="policyIdentifier">To be added.</param>
		///         <param name="properties">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		static public SecPolicy CreatePolicy (NSString policyIdentifier, NSDictionary properties)
		{
			if (policyIdentifier is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (policyIdentifier));
			IntPtr dh = properties.GetHandle ();

			// note: only accept known OIDs or return null (unit test will alert us if that change, FIXME in Apple code)
			// see: https://github.com/Apple-FOSS-Mirror/libsecurity_keychain/blob/master/lib/SecPolicy.cpp#L245
			IntPtr ph = SecPolicyCreateWithProperties (policyIdentifier.Handle, dh);
			GC.KeepAlive (properties);
			GC.KeepAlive (policyIdentifier);
			if (ph == IntPtr.Zero)
				throw new ArgumentException ("Unknown policyIdentifier");
			return new SecPolicy (ph, true);
		}
	}
}
