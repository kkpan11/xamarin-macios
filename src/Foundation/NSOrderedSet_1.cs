//
// This file contains a generic version of NSOrderedSet
//
// Authors:
//		Alex Soto	(alex.soto@xamarin.com)
//
// Copyright 2015, Xamarin Inc.
//
#nullable enable

using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Versioning;

using ObjCRuntime;

namespace Foundation {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[Register ("NSOrderedSet", SkipRegistration = true)]
	public sealed partial class NSOrderedSet<TKey> : NSOrderedSet, IEnumerable<TKey>
		where TKey : class, INativeObject {

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet ()
		{
		}

		/// <param name="coder">The unarchiver object.</param>
		///         <summary>A constructor that initializes the object from the data stored in the unarchiver object.</summary>
		///         <remarks>
		///           <para>This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).   This is part of the <see cref="Foundation.NSCoding" />  protocol.</para>
		///           <para>If developers want to create a subclass of this object and continue to support deserialization from an archive, they should implement a constructor with an identical signature: taking a single parameter of type <see cref="Foundation.NSCoder" /> and decorate it with the [Export("initWithCoder:"] attribute declaration.</para>
		///           <para>The state of this object can also be serialized by using the companion method, EncodeTo.</para>
		///         </remarks>
		public NSOrderedSet (NSCoder coder) : base (coder)
		{
		}

		internal NSOrderedSet (NativeHandle handle) : base (handle)
		{
		}

		/// <param name="start">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (TKey start) : base (start)
		{
		}

		/// <param name="objs">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (params TKey [] objs) : base (objs)
		{
		}

		/// <param name="source">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (NSSet<TKey> source) : base (source)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (NSOrderedSet<TKey> other) : base (other)
		{
		}

		/// <param name="other">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSOrderedSet (NSMutableOrderedSet<TKey> other) : base (other)
		{
		}

		public new TKey? this [nint idx] {
			get {
				var ret = _GetObject (idx);
				return Runtime.GetINativeObject<TKey> (ret, false);
			}
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey [] ToArray ()
		{
			return base.ToArray<TKey> ();
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

		/// <param name="obj">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public nint IndexOf (TKey obj)
		{
			if (obj is null)
				throw new ArgumentNullException (nameof (obj));

			nint result = _IndexOf (obj.Handle);
			GC.KeepAlive (obj);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey? FirstObject ()
		{
			var ret = _FirstObject ();
			return Runtime.GetINativeObject<TKey> (ret, false);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public TKey? LastObject ()
		{
			var ret = _LastObject ();
			return Runtime.GetINativeObject<TKey> (ret, false);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSSet<TKey>? AsSet ()
		{
			var ret = _AsSet ();
			return Runtime.GetINativeObject<NSSet<TKey>> (ret, false);
		}

		#region IEnumerable<TKey>
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

		public static NSOrderedSet<TKey>? operator + (NSOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		public static NSOrderedSet<TKey>? operator + (NSOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return second is not null ? new NSOrderedSet<TKey> (second) : null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.UnionSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		public static NSOrderedSet<TKey>? operator - (NSOrderedSet<TKey>? first, NSOrderedSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		public static NSOrderedSet<TKey>? operator - (NSOrderedSet<TKey>? first, NSSet<TKey>? second)
		{
			if (first is null)
				return null;
			if (second is null)
				return new NSOrderedSet<TKey> (first);
			var copy = new NSMutableOrderedSet<TKey> (first);
			copy.MinusSet (second);
			var copyset = new NSOrderedSet<TKey> (copy);
			return copyset;
		}

		public static bool operator == (NSOrderedSet<TKey> first, NSOrderedSet<TKey> second)
		{
			// IsEqualToOrderedSet does not allow null
			if (object.ReferenceEquals (null, first))
				return object.ReferenceEquals (null, second);
			if (object.ReferenceEquals (null, second))
				return false;

			return first.IsEqualToOrderedSet (second);
		}

		public static bool operator != (NSOrderedSet<TKey> first, NSOrderedSet<TKey> second)
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
			var o = other as NSOrderedSet<TKey>;
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

#if false // https://github.com/dotnet/macios/issues/15577

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey> GetDifference (NSOrderedSet<TKey> other, NSOrderedCollectionDifferenceCalculationOptions options)
			=> new NSOrderedCollectionDifference<TKey> (_GetDifference (other, options));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey> GetDifference (NSOrderedSet other)
			=> new NSOrderedCollectionDifference<TKey> (_GetDifference (other));

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedSet<TKey>? GetOrderedSet (NSOrderedCollectionDifference difference)
		{
			var ptr = _GetOrderedSet (difference); 
			return Runtime.GetNSObject<NSOrderedSet<TKey>> (ptr);
		}

		static readonly NSOrderedCollectionDifferenceEquivalenceTestProxy static_DiffEqualityGeneric = DiffEqualityHandlerGeneric;

		[MonoPInvokeCallback (typeof (NSOrderedCollectionDifferenceEquivalenceTestProxy))]
		static bool DiffEqualityHandlerGeneric (IntPtr block, IntPtr first, IntPtr second)
		{
			var callback = BlockLiteral.GetTarget<NSOrderedCollectionDifferenceEquivalenceTest<TKey>> (block);
			if (callback is not null) {
				var nsFirst = Runtime.GetINativeObject<TKey> (first, false);
				var nsSecond = Runtime.GetINativeObject<TKey> (second, false);
				return callback (nsFirst, nsSecond);
			}
			return false;
		}

		[SupportedOSPlatform ("ios13.0"), SupportedOSPlatform ("tvos13.0"), SupportedOSPlatform ("macos")]
		public NSOrderedCollectionDifference<TKey>? GetDifference (NSOrderedSet<TKey> other, NSOrderedCollectionDifferenceCalculationOptions options, NSOrderedCollectionDifferenceEquivalenceTest<TKey> equivalenceTest) 
		{
			if (equivalenceTest is null)
				throw new ArgumentNullException (nameof (equivalenceTest));

			var block = new BlockLiteral ();
			block.SetupBlock (static_DiffEqualityGeneric, equivalenceTest);
			try {
				return Runtime.GetNSObject<NSOrderedCollectionDifference<TKey>> (_GetDifference (other, options, ref block));
			} finally {
				block.CleanupBlock ();
			}
		}
#endif
	}
}
