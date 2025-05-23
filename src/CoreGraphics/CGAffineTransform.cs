// 
// CGAffineTransform.cs: Implements the managed side
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2014 Xamarin Inc.
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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace CoreGraphics {

	// CGAffineTransform.h
	/// <summary>2D Affine transformation used to convert between coordinate spaces.</summary>
	///     <remarks>
	///       <para>An affine transformation uses a matrix to transform poitns between coordinate spaces.   
	/// </para>
	///       <para>
	/// These transformation can be used to rotate, scale, shear and translate points and rectangles from one coordinate system into another.
	/// </para>
	///     </remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CGAffineTransform {
		public /* CGFloat */ nfloat A;
		public /* CGFloat */ nfloat B;
		public /* CGFloat */ nfloat C;
		public /* CGFloat */ nfloat D;
		public /* CGFloat */ nfloat Tx;
		public /* CGFloat */ nfloat Ty;

#if !XAMCORE_5_0
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'A' instead.")]
		public nfloat xx { get => A; set => A = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'B' instead.")]
		public nfloat yx { get => B; set => B = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'C' instead.")]
		public nfloat xy { get => C; set => C = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'D' instead.")]
		public nfloat yy { get => D; set => D = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'Tx' instead.")]
		public nfloat x0 { get => Tx; set => Tx = value; }
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use 'Ty' instead.")]
		public nfloat y0 { get => Ty; set => Ty = value; }
#endif // !XAMCORE_5_0

#if !COREBUILD
		//
		// Constructors
		//
		public CGAffineTransform (nfloat a, nfloat b, nfloat c, nfloat d, nfloat tx, nfloat ty)
		{
			this.A = a;
			this.B = b;
			this.C = c;
			this.D = d;
			this.Tx = tx;
			this.Ty = ty;
		}

		// Identity
		/// <summary>Returns the identity affine transformation.</summary>
		///         <returns>The identity matrix.</returns>
		///         <remarks>
		///           <para>
		/// Sets up an identity transformation, like this:
		/// </para>
		///           <para>
		/// | 1 0 0 |
		/// </para>
		///           <para>
		/// | 0 1 0 |
		/// </para>
		///           <para>
		/// | 0 0 1 |
		/// </para>
		///         </remarks>
		public static CGAffineTransform MakeIdentity ()
		{
			return new CGAffineTransform (1, 0, 0, 1, 0, 0);
		}

		public static CGAffineTransform MakeRotation (nfloat angle)
		{
			var cos = (nfloat) Math.Cos (angle);
			var sin = (nfloat) Math.Sin (angle);
			return new CGAffineTransform (
				cos, sin,
				-sin, cos,
				0, 0);
		}

		public static CGAffineTransform MakeScale (nfloat sx, nfloat sy)
		{
			return new CGAffineTransform (sx, 0, 0, sy, 0, 0);
		}

		public static CGAffineTransform MakeTranslation (nfloat tx, nfloat ty)
		{
			return new CGAffineTransform (1, 0, 0, 1, tx, ty);
		}

		//
		// Operations
		//
		/// <param name="a">The first affine.</param>
		///         <param name="b">The second affine.</param>
		///         <summary>Multiplies the two affine transformations and returns the result.</summary>
		///         <returns>The multiplied affine.</returns>
		///         <remarks>Use affine multiplication to compose multiple affine tranformations into a single affine.</remarks>
		public static CGAffineTransform Multiply (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.A * b.A + a.B * b.C,
							  a.A * b.B + a.B * b.D,
							  a.C * b.A + a.D * b.C,
							  a.C * b.B + a.D * b.D,
							  a.Tx * b.A + a.Ty * b.C + b.Tx,
							  a.Tx * b.B + a.Ty * b.D + b.Ty);
		}

		/// <param name="b">The affine used to multiply the current affine by.</param>
		///         <summary>Multiplies the current affine transformation by the specified affine transformation.</summary>
		///         <remarks>Use affine multiplication to compose multiple affine tranformations into a single affine.</remarks>
		public void Multiply (CGAffineTransform b)
		{
			var a = this;
			A = a.A * b.A + a.B * b.C;
			B = a.A * b.B + a.B * b.D;
			C = a.C * b.A + a.D * b.C;
			D = a.C * b.B + a.D * b.D;
			Tx = a.Tx * b.A + a.Ty * b.C + b.Tx;
			Ty = a.Tx * b.B + a.Ty * b.D + b.Ty;
		}

		public void Scale (nfloat sx, nfloat sy, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeScale (sx, sy), this); // t' = [ sx 0 0 sy 0 0 ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeScale (sx, sy)); // t' = t * [ sx 0 0 sy 0 0 ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ sx 0 0 sy 0 0 ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Scale (nfloat, nfloat, MatrixOrder)'.")]
		public void Scale (nfloat sx, nfloat sy)
		{
			Scale (sx, sy, MatrixOrder.Append);
		}

		public static CGAffineTransform Scale (CGAffineTransform transform, nfloat sx, nfloat sy)
		{
			return new CGAffineTransform (
				sx * transform.A,
				sx * transform.B,
				sy * transform.C,
				sy * transform.D,
				transform.Tx,
				transform.Ty);
		}

		public void Translate (nfloat tx, nfloat ty, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeTranslation (tx, ty), this); // t' = [ 1 0 0 1 tx ty ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeTranslation (tx, ty)); // t' = t * [ 1 0 0 1 tx ty ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ 1 0 0 1 tx ty ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Translate (nfloat, nfloat, MatrixOrder)'.")]
		public void Translate (nfloat tx, nfloat ty)
		{
			Translate (tx, ty, MatrixOrder.Append);
		}

		public static CGAffineTransform Translate (CGAffineTransform transform, nfloat tx, nfloat ty)
		{
			return new CGAffineTransform (
				transform.A,
				transform.B,
				transform.C,
				transform.D,
				tx * transform.A + ty * transform.C + transform.Tx,
				tx * transform.B + ty * transform.D + transform.Ty);
		}

		public void Rotate (nfloat angle, MatrixOrder order)
		{
			switch (order) {
			case MatrixOrder.Prepend: // The new operation is applied before the old operation.
				this = Multiply (MakeRotation (angle), this); // t' = [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ] * t – Swift equivalent
				break;
			case MatrixOrder.Append: // The new operation is applied after the old operation.
				this = Multiply (this, MakeRotation (angle)); // t' = t * [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ]
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (order));
			}
		}

		[Advice ("By default, the new operation is applied after the old operation: t' = t * [ cos(angle) sin(angle) -sin(angle) cos(angle) 0 0 ].\nTo have the same behavior as the native Swift API, pass 'MatrixOrder.Prepend' to 'Rotate (nfloat, MatrixOrder)'.")]
		public void Rotate (nfloat angle)
		{
			Rotate (angle, MatrixOrder.Append);
		}

		public static CGAffineTransform Rotate (CGAffineTransform transform, nfloat angle)
		{
			var cos = (nfloat) Math.Cos (angle);
			var sin = (nfloat) Math.Sin (angle);

			return new CGAffineTransform (
				cos * transform.A + sin * transform.C,
				cos * transform.B + sin * transform.D,
				cos * transform.C - sin * transform.A,
				cos * transform.D - sin * transform.B,
				transform.Tx,
				transform.Ty);
		}

		/// <summary>Determines if the affine corresponds to the identity affine.</summary>
		///         <value>Whether the affine is the identity.</value>
		///         <remarks>To be added.</remarks>
		public bool IsIdentity {
			get {
				return A == 1 && B == 0 && C == 0 && D == 1 && Tx == 0 && Ty == 0;
			}
		}

#if !MONOMAC
		// on macOS NSAffineTransform is an ObjC type
		[DllImport (Constants.UIKitLibrary)]
		extern static /* NSString */ IntPtr NSStringFromCGAffineTransform (CGAffineTransform transform);
#endif

		/// <summary>Renders the affine in textual form.</summary>
		///         <returns />
		///         <remarks>To be added.</remarks>
		public override String? ToString ()
		{
#if MONOMAC
			var s = $"[{A}, {B}, {C}, {D}, {Tx}, {Ty}]";
#else
			var s = CFString.FromHandle (NSStringFromCGAffineTransform (this));
#endif
			return s;
		}

		public static bool operator == (CGAffineTransform lhs, CGAffineTransform rhs)
		{
			return (lhs.A == rhs.A && lhs.C == rhs.C &&
				lhs.B == rhs.B && lhs.D == rhs.D &&
				lhs.Tx == rhs.Tx && lhs.Ty == rhs.Ty);
		}

		public static bool operator != (CGAffineTransform lhs, CGAffineTransform rhs)
		{
			return !(lhs == rhs);
		}

		public static CGAffineTransform operator * (CGAffineTransform a, CGAffineTransform b)
		{
			return new CGAffineTransform (a.A * b.A + a.B * b.C,
							  a.A * b.B + a.B * b.D,
							  a.C * b.A + a.D * b.C,
							  a.C * b.B + a.D * b.D,
							  a.Tx * b.A + a.Ty * b.C + b.Tx,
							  a.Tx * b.B + a.Ty * b.D + b.Ty);
		}

		/// <param name="o">The object to compare this instance against.</param>
		///         <summary>Compares the objects for equality.</summary>
		///         <returns>
		///           <see langword="true" /> if the objects are equal, <see langword="false" /> if not.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object? o)
		{
			if (o is CGAffineTransform transform) {
				return this == transform;
			} else
				return false;
		}

		/// <summary>The hashcode for this object.</summary>
		///         <returns>An integer value.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (A, C, B, D, Tx, Ty);
		}

		/// <param name="point">The point to transform.</param>
		///         <summary>Transforms the coordinates of the provided point by the affine.</summary>
		///         <returns>The point translated to the new coordinate space.</returns>
		///         <remarks>
		///           <para>
		/// The point defined by x, y is transformed like this:
		/// </para>
		///           <para>
		/// new_x = xx * x + xy * y + x0;
		/// </para>
		///           <para>
		/// new_y = yx * x + yy * y + y0;
		/// </para>
		///         </remarks>
		public CGPoint TransformPoint (CGPoint point)
		{
			return new CGPoint (A * point.X + C * point.Y + Tx,
						B * point.X + D * point.Y + Ty);
		}

		/// <param name="rect">To be added.</param>
		///         <param name="t">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGRect CGRectApplyAffineTransform (CGRect rect, CGAffineTransform t);

		/// <param name="rect">A rectangle to transform.</param>
		///         <summary>Applies the affine transform to the supplied rectangle and returns the transformed rectangle.</summary>
		///         <returns>The transformed rectangle.</returns>
		///         <remarks>
		///         </remarks>
		public CGRect TransformRect (CGRect rect)
		{
			return CGRectApplyAffineTransform (rect, this);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static CGSize CGSizeApplyAffineTransform (CGSize rect, CGAffineTransform t);

		/// <param name="size">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize TransformSize (CGSize size)
		{
			return CGSizeApplyAffineTransform (size, this);
		}

		/// <param name="t">Affine transformation to invert.</param>
		///         <summary>Inverts the affine transformation matrix.</summary>
		///         <returns>If the affine transformation can not be inverted, the same matrix is returned.</returns>
		///         <remarks>You can use the inversion matrix to map points in the target coordinate space that had been mapped to the original coordinate space.</remarks>
		[DllImport (Constants.CoreGraphicsLibrary)]
		public extern static CGAffineTransform CGAffineTransformInvert (CGAffineTransform t);

		/// <summary>Inverts this affine transformation.</summary>
		///         <returns>If the affine transformation can not be inverted, the matrix does not change.</returns>
		///         <remarks>You can use the inversion matrix to map points in the target coordinate space that had been mapped to the original coordinate space.</remarks>
		public CGAffineTransform Invert ()
		{
			return CGAffineTransformInvert (this);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGAffineTransformComponents CGAffineTransformDecompose (CGAffineTransform transform);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public CGAffineTransformComponents Decompose ()
		{
			return CGAffineTransformDecompose (this);
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern CGAffineTransform CGAffineTransformMakeWithComponents (CGAffineTransformComponents components);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public static CGAffineTransform MakeWithComponents (CGAffineTransformComponents components)
		{
			return CGAffineTransformMakeWithComponents (components);
		}
#endif // !COREBUILD
	}
}
