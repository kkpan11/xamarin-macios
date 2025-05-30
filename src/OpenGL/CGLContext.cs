// 
// CGLContext.cs: Implements the managed CGLContext
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
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

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace OpenGL {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[ObsoletedOSPlatform ("macos10.14", "Use 'Metal' Framework instead.")]
	public class CGLContext : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGLContext (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLRetainContext (IntPtr handle);

		[DllImport (Constants.OpenGLLibrary)]
		extern static void CGLReleaseContext (IntPtr handle);

		protected internal override void Retain ()
		{
			CGLRetainContext (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGLReleaseContext (GetCheckedHandle ());
		}

		[DllImport (Constants.OpenGLLibrary)]
		extern static CGLErrorCode CGLLockContext (IntPtr ctx);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGLErrorCode Lock ()
		{
			return CGLLockContext (Handle);
		}

		[DllImport (Constants.OpenGLLibrary)]
		extern static CGLErrorCode CGLUnlockContext (IntPtr ctx);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGLErrorCode Unlock ()
		{
			return CGLUnlockContext (Handle);
		}

		[DllImport (Constants.OpenGLLibrary)]
		extern static CGLErrorCode CGLSetCurrentContext (IntPtr ctx);

		[DllImport (Constants.OpenGLLibrary)]
		extern static IntPtr CGLGetCurrentContext ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CGLContext? CurrentContext {
			get {
				IntPtr ctx = CGLGetCurrentContext ();
				if (ctx != IntPtr.Zero)
					return new CGLContext (ctx, false);
				else
					return null;
			}

			set {
				var retValue = CGLSetCurrentContext (value.GetHandle ());
				GC.KeepAlive (value);
				if (retValue != CGLErrorCode.NoError)
					throw new Exception ("Error setting the Current Context");
			}
		}
#endif // !COREBUILD
	}
}
