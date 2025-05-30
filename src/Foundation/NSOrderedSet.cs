//
// NSOrderedSet.cs:
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2013, Xamarin Inc
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;

using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSOrderedSet : IEnumerable<NSObject> {
		internal const string selSetWithArray = "orderedSetWithArray:";

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (params NSObject [] objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <param name="strings">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		public NSObject this [nint idx] {
			get {
				return GetObject (idx);
			}
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public T [] ToArray<T> () where T : class, INativeObject
		{
			IntPtr nsarr = _ToArray ();
			return NSArray.ArrayFromHandle<T> (nsarr);
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSOrderedSet MakeNSOrderedSet<T> (T [] values) where T : NSObject
		{
			NSArray a = NSArray.FromNSObjects (values);
			var result = (NSOrderedSet) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (class_ptr, Selector.GetHandle (selSetWithArray), a.Handle));
			GC.KeepAlive (a);
			return result;
		}

		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public IEnumerator<NSObject> GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;

			while ((obj = enumerator.NextObject ()) is not null)
				yield return obj as NSObject;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			var enumerator = _GetEnumerator ();
			NSObject obj;

			while ((obj = enumerator.NextObject ()) is not null)
				yield return obj;
		}

		public static NSOrderedSet operator + (NSOrderedSet first, NSOrderedSet second)
		{
			if (first is null)
				return new NSOrderedSet (second);
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.UnionSet (second);
			return copy;
		}

		public static NSOrderedSet operator + (NSOrderedSet first, NSSet second)
		{
			if (first is null)
				return new NSOrderedSet (second);
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.UnionSet (second);
			return copy;
		}

		public static NSOrderedSet operator - (NSOrderedSet first, NSOrderedSet second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.MinusSet (second);
			return copy;
		}

		public static NSOrderedSet operator - (NSOrderedSet first, NSSet second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet (first);
			var copy = new NSMutableOrderedSet (first);
			copy.MinusSet (second);
			return copy;
		}

		public static bool operator == (NSOrderedSet first, NSOrderedSet second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return false;

			return first.IsEqualToOrderedSet (second);
		}

		public static bool operator != (NSOrderedSet first, NSOrderedSet second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return !object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return true;

			return !first.IsEqualToOrderedSet (second);
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object other)
		{
			NSOrderedSet o = other as NSOrderedSet;
			if (o is null)
				return false;
			return IsEqualToOrderedSet (o);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		///         <returns>A int containing the hash code for this instance.</returns>
		///         <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return (int) GetNativeHash ();
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (object obj)
		{
			return Contains (NSObject.FromObject (obj));
		}
	}

	public partial class NSMutableOrderedSet {
		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (params NSObject [] objs) : this (NSArray.FromNSObjects (objs))
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (params object [] objs) : this (NSArray.FromObjects (objs))
		{
		}

		/// <param name="strings">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableOrderedSet (params string [] strings) : this (NSArray.FromStrings (strings))
		{
		}

		public new NSObject this [nint idx] {
			get {
				return GetObject (idx);
			}

			set {
				SetObject (value, idx);
			}
		}

#if false // https://github.com/dotnet/macios/issues/15577
		delegate bool NSOrderedCollectionDifferenceEquivalenceTestProxy (IntPtr blockLiteral, /* NSObject */ IntPtr first, /* NSObject */ IntPtr second);
		static readonly NSOrderedCollectionDifferenceEquivalenceTestProxy static_DiffEquality = DiffEqualityHandler;

		[MonoPInvokeCallback (typeof (NSOrderedCollectionDifferenceEquivalenceTestProxy))]
		static bool DiffEqualityHandler (IntPtr block, IntPtr first, IntPtr second)
		{
			var callback = BlockLiteral.GetTarget<NSOrderedCollectionDifferenceEquivalenceTest> (block);
			if (callback is not null) {
				var nsFirst = Runtime.GetNSObject<NSObject> (first, false);
				var nsSecond = Runtime.GetNSObject<NSObject> (second, false);
				return callback (nsFirst, nsSecond);
			}
			return false;
		}

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference GetDifference (NSOrderedSet other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest equivalenceTest)
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEquality, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference> (_GetDifference (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
