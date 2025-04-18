// 
// CGPDFContentStream.cs: Implement the managed CGPDFContentStream bindings
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//     
// Copyright 2014 Xamarin Inc. All rights reserved.

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {
	/// <summary>Class that gets PDF resources as an object or stream.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPDFContentStream.h
	public class CGPDFContentStream : NativeObject {

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFContentStreamRef */ IntPtr CGPDFContentStreamCreateWithPage (/* CGPDFPageRef */ IntPtr page);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFContentStreamRef */ IntPtr CGPDFContentStreamCreateWithStream (/* CGPDFStreamRef */ IntPtr stream,
			/* CGPDFDictionaryRef */ IntPtr streamResources, /* CGPDFContentStreamRef */ IntPtr parent);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFContentStreamRef */ IntPtr CGPDFContentStreamRetain (/* CGPDFContentStreamRef */ IntPtr cs);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPDFContentStreamRelease (/* CGPDFContentStreamRef */ IntPtr cs);

		[Preserve (Conditional = true)]
		internal CGPDFContentStream (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="page">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPDFContentStream (CGPDFPage page)
			: base (CGPDFContentStreamCreateWithPage (page.GetNonNullHandle (nameof (page))), true)
		{
			GC.KeepAlive (page);
		}

		static IntPtr Create (CGPDFStream stream, NSDictionary? streamResources = null, CGPDFContentStream? parent = null)
		{
			if (stream is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (stream));

			IntPtr result = CGPDFContentStreamCreateWithStream (stream.Handle, streamResources.GetHandle (), parent.GetHandle ());
			GC.KeepAlive (stream);
			GC.KeepAlive (streamResources);
			GC.KeepAlive (parent);
			return result;
		}

		/// <param name="stream">To be added.</param>
		///         <param name="streamResources">To be added.</param>
		///         <param name="parent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CGPDFContentStream (CGPDFStream stream, NSDictionary? streamResources = null, CGPDFContentStream? parent = null)
			: base (Create (stream, streamResources, parent), true)
		{
		}

		protected internal override void Retain ()
		{
			CGPDFContentStreamRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGPDFContentStreamRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CFArrayRef */ IntPtr CGPDFContentStreamGetStreams (/* CGPDFContentStreamRef */ IntPtr cs);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPDFStream? []? GetStreams ()
		{
			var rv = CGPDFContentStreamGetStreams (Handle);
			return CFArray.ArrayFromHandleFunc (rv, (handle) => new CGPDFStream (handle));
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFObjectRef */ IntPtr CGPDFContentStreamGetResource (/* CGPDFContentStreamRef */ IntPtr cs, /* const char* */ IntPtr category, /* const char* */ IntPtr name);

		/// <param name="category">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGPDFObject? GetResource (string category, string name)
		{
			if (category is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (category));
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));

			using var categoryPtr = new TransientString (category);
			using var namePtr = new TransientString (name);
			var h = CGPDFContentStreamGetResource (Handle, categoryPtr, namePtr);
			return (h == IntPtr.Zero) ? null : new CGPDFObject (h);
		}
	}
}
