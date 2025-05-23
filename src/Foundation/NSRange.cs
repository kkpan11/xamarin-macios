//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
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
using System.Runtime.Versioning;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {
	/// <summary>Represents a range given by a location and length.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/SimpleTextInput/">SimpleTextInput</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct NSRange : IEquatable<NSRange> {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Location;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public nint Length;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static readonly nint NotFound = nint.MaxValue;

		public NSRange (nint start, nint len)
		{
			Location = start;
			Length = len;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (Location, Length);
		}

		public override bool Equals (object obj)
		{
			return obj is NSRange other && Equals (other);
		}

		public bool Equals (NSRange other)
		{
			return Location == other.Location && Length == other.Length;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return string.Format ("[Location={0},Length={1}]", Location, Length);
		}
	}
}
