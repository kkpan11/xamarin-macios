//
// Authors:
//      James Clancey james.clancey@gmail.com>
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

#if MONOMAC

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ObjCRuntime;

namespace Foundation {

	public partial class NSIndexSet : IEnumerable, IEnumerable<nuint> {

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			if (this.Count == 0)
				yield break;

			for (nuint i = this.FirstIndex; i <= this.LastIndex;) {
				yield return i;
				i = this.IndexGreaterThan (i);
			}
		}

		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public IEnumerator<nuint> GetEnumerator ()
		{
			if (this.Count == 0)
				yield break;

			for (nuint i = this.FirstIndex; i <= this.LastIndex;) {
				yield return i;
				i = this.IndexGreaterThan (i);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public nuint [] ToArray ()
		{
			nuint [] indexes = new nuint [Count];

			if (this.Count == 0)
				return indexes;

			int j = 0;
			for (nuint i = this.FirstIndex; i <= this.LastIndex;) {
				indexes [j++] = i;
				i = this.IndexGreaterThan (i);
			}
			return indexes;
		}

		internal T [] ToInt64EnumArray<T> () where T: System.Enum
		{
			var array = ToArray ();
			var rv = new T [array.Length];
			for (var i = 0; i < array.Length; i++)
				rv [i] = (T) (object) (long) array [i];
			return rv;
		}

		internal HashSet<T> ToInt64EnumHashSet<T> () where T: System.Enum
		{
			var array = ToArray ();
			var rv = new HashSet<T> ();
			for (var i = 0; i < array.Length; i++)
				rv.Add ((T) (object) (long) array [i]);
			return rv;
		}

		/// <param name="items">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSIndexSet FromArray (nuint [] items)
		{
			if (items is null)
				return new NSIndexSet ();

			var indexSet = new NSMutableIndexSet ();
			foreach (var index in items)
				indexSet.Add (index);
			return indexSet;
		}

		/// <param name="items">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSIndexSet FromArray (uint [] items)
		{
			if (items is null)
				return new NSIndexSet ();

			var indexSet = new NSMutableIndexSet ();
			foreach (var index in items)
				indexSet.Add ((nuint) index);
			return indexSet;
		}

		/// <param name="items">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSIndexSet FromArray (int [] items)
		{
			if (items is null)
				return new NSIndexSet ();

			var indexSet = new NSMutableIndexSet ();
			foreach (var index in items) {
				if (index < 0)
					throw new ArgumentException ("One of the items values is negative");
				indexSet.Add ((nuint) (uint) index);
			}
			return indexSet;
		}

		/// <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSIndexSet (uint value) : this ((nuint) value)
		{
		}

		public NSIndexSet (nint value) : this ((nuint) value)
		{
			if (value < 0)
				throw new ArgumentException ("value must be positive");
			// init done by the base ctor
		}

		/// <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSIndexSet (int value) : this ((nuint) (uint) value)
		{
			if (value < 0)
				throw new ArgumentException ("value must be positive");
			// init done by the base ctor
		}
	}
}

#endif
