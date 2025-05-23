//
// Copyright 2015 Xamarin Inc (http://www.xamarin.com)
//
// This file contains a generic version of NSMutableSet.
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
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[Register ("NSMutableSet", SkipRegistration = true)]
	public sealed partial class NSMutableSet<TKey> : NSMutableSet, IEnumerable<TKey>
		where TKey : class, INativeObject {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableSet ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSMutableSet (NSCoder coder)
			: base (coder)
		{
		}

		internal NSMutableSet (NativeHandle handle)
			: base (handle)
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableSet (params TKey [] objs)
			: base (objs)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableSet (NSSet<TKey> other)
			: base (other)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableSet (NSMutableSet<TKey> other)
			: base (other)
		{
		}

		/// <param name="capacity">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		public NSMutableSet (nint capacity)
			: base (capacity)
		{
		}

		// Strongly typed versions of API from NSSet

		/// <param name="probe">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey LookupMember (TKey probe)
		{
			if (probe is null)
				throw new ArgumentNullException (nameof (probe));

			TKey result = Runtime.GetINativeObject<TKey> (_LookupMember (probe.Handle), false);
			GC.KeepAlive (probe);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public TKey AnyObject {
			get {
				return Runtime.GetINativeObject<TKey> (_AnyObject, false);
			}
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Contains (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			bool result = _Contains (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
		}

		public static NSMutableSet<TKey> operator + (NSMutableSet<TKey> first, NSMutableSet<TKey> second)
		{
			if (first is null || first.Count == 0)
				return new NSMutableSet<TKey> (second);
			if (second is null || second.Count == 0)
				return new NSMutableSet<TKey> (first);
			var result = new NSMutableSet<TKey> (first._SetByAddingObjectsFromSet (second.Handle));
			GC.KeepAlive (second);
			return result;
		}

		public static NSMutableSet<TKey> operator - (NSMutableSet<TKey> first, NSMutableSet<TKey> second)
		{
			if (first is null || first.Count == 0)
				return null;
			if (second is null || second.Count == 0)
				return new NSMutableSet<TKey> (first);
			var copy = new NSMutableSet<TKey> (first);
			copy.MinusSet (second);
			return copy;
		}

		// Strongly typed versions of API from NSMutableSet
		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Add (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Remove (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			_Remove (obj.Handle);
			GC.KeepAlive (obj);
		}

		/// <param name="objects">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddObjects (params TKey [] objects)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));

			for (int i = 0; i < objects.Length; i++)
				if (objects [i] is null)
					throw new ArgumentNullException (nameof (objects) + "[" + i.ToString () + "]");

			using (var array = NSArray.From<TKey> (objects))
				_AddObjects (array.Handle);
		}

		#region IEnumerable<T> implementation
		/// <summary>Returns an enumerator that iterates through the set.</summary>
		/// <returns>An enumerator that can be used to iterate through the set.</returns>
		public new IEnumerator<TKey> GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion

		#region IEnumerable implementation
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new NSFastEnumerator<TKey> (this);
		}
		#endregion
	}
}
