//
// PdfAnnotation.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

using System;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace PdfKit {
	public partial class PdfAnnotation {

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="value">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos18.2")]
		public bool SetValue<T> (T value, PdfAnnotationKey key) where T : class, INativeObject
		{
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

			bool result = _SetValue (value.Handle, key.GetConstant ()!);
			GC.KeepAlive (value);
			return result;
		}

		/// <param name="str">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos18.2")]
		public bool SetValue (string str, PdfAnnotationKey key)
		{
			var nstr = CFString.CreateNative (str);
			try {
				return _SetValue (nstr, key.GetConstant ()!);
			} finally {
				CFString.ReleaseNative (nstr);
			}
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos18.2")]
		public T GetValue<T> (PdfAnnotationKey key) where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_GetValue (key.GetConstant ()!), true)!;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PdfAnnotationKey AnnotationType {
			get { return PdfAnnotationKeyExtensions.GetValue (Type!); }
			set { Type = value.GetConstant ()!; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos18.2")]
		public CGPoint [] QuadrilateralPoints {
			get {
				return NSArray.ArrayFromHandleFunc<CGPoint> (_QuadrilateralPoints, (v) => {
					using (var value = new NSValue (v))
						return value.CGPointValue;
				});
			}
			set {
				if (value is null) {
					_QuadrilateralPoints = IntPtr.Zero;
				} else {
					using (var arr = new NSMutableArray ()) {
						for (int i = 0; i < value.Length; i++)
							arr.Add (NSValue.FromCGPoint (value [i]));
						_QuadrilateralPoints = arr.Handle;
					}
				}
			}
		}
	}
}
