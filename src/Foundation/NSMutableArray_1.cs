// Copyright 2015 Xamarin Inc
//
// This file contains a generic version of NSMutableArray.
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
using System.Collections.Generic;
using System.Runtime.Versioning;

using ObjCRuntime;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Foundation {

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	[Register ("NSMutableArray", SkipRegistration = true)]
	public sealed partial class NSMutableArray<TValue> : NSMutableArray, IEnumerable<TValue>
		where TValue : class, INativeObject {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableArray ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSMutableArray (NSCoder coder)
			: base (coder)
		{
		}

		internal NSMutableArray (NativeHandle handle)
			: base (handle)
		{
		}

		/// <param name="capacity">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public NSMutableArray (nuint capacity)
			: base (capacity)
		{
		}

		/// <param name="values">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableArray (params TValue [] values)
		{
			if (values is null)
				throw new ArgumentNullException (nameof (values));

			for (int i = 0; i < values.Length; i++)
				Add (values [i]);
		}

		// Strongly typed methods from NSArray
		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (TValue obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public nuint IndexOf (TValue obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			nuint result = _IndexOf (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		// Strongly typed methods from NSMutableArray
		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (TValue obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Add (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <param name="obj">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void Insert (TValue obj, nint index)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			ValidateIndex (index);

			_Insert (obj.Handle, index);
			GC.KeepAlive (obj);
		}

		/// <param name="index">To be added.</param>
		/// <param name="withObject">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public void ReplaceObject (nint index, TValue withObject)
		{
			if (withObject is null)
				throw new ArgumentNullException (nameof (withObject));

			ValidateIndex (index);

			_ReplaceObject (index, withObject.Handle);
			GC.KeepAlive (withObject);
		}

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddObjects (params TValue [] source)
		{
			if (source is null)
				throw new ArgumentNullException (nameof (source));

			for (int i = 0; i < source.Length; i++)
				if (source [i] is null)
					throw new ArgumentNullException (nameof (source) + "[" + i.ToString () + "]");

			for (int i = 0; i < source.Length; i++)
				_Add (source [i].Handle);
		}

		/// <param name="objects">To be added.</param>
		///         <param name="atIndexes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void InsertObjects (TValue [] objects, NSIndexSet atIndexes)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));

			if (atIndexes is null)
				throw new ArgumentNullException (nameof (atIndexes));

			if (objects.Length != atIndexes.Count)
				throw new ArgumentOutOfRangeException ("'" + nameof (objects) + "' and '" + nameof (atIndexes) + "' must contain the same number of elements");

			for (int i = 0; i < objects.Length; i++)
				if (objects [i] is null)
					throw new ArgumentNullException (nameof (objects) + "[" + i.ToString () + "]");

			nuint idx = atIndexes.FirstIndex;
			for (int i = 0; i < objects.Length; i++) {
				if (i > 0)
					idx = atIndexes.IndexGreaterThan (idx);

				Insert (objects [i], (nint) idx);
			}
		}

		// Additional implementations.

		public TValue this [nuint index] {
			get {
				ValidateIndex (index);
				return GetItem<TValue> (index);
			}
			set {
				if (value is null)
					throw new ArgumentNullException (nameof (value));
				ValidateIndex (index);
				_ReplaceObject ((nint) index, value.Handle);
				GC.KeepAlive (value);
			}
		}

		void ValidateIndex (nint index)
		{
			if (index < 0)
				throw new IndexOutOfRangeException (nameof (index));

			if ((nuint) index > Count)
				throw new IndexOutOfRangeException (nameof (index));
		}

		void ValidateIndex (nuint index)
		{
			if (index >= Count)
				throw new IndexOutOfRangeException (nameof (index));
		}

		#region IEnumerable<T> implementation
		/// <summary>Returns an enumerator that iterates through the array.</summary>
		/// <returns>An enumerator that can be used to iterate through the array.</returns>
		public IEnumerator<TValue> GetEnumerator ()
		{
			return new NSFastEnumerator<TValue> (this);
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		#endregion

#if false // https://github.com/dotnet/macios/issues/15577
#if !NET
		[TV (13,0), iOS (13,0)]
#else
		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
#endif
		public void ApplyDifference (NSOrderedCollectionDifference<TValue> difference)
			=> ApplyDifference ((NSOrderedCollectionDifference) difference);
#endif
	}
}
