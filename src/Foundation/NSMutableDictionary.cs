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
// Copyright 2011 - 2014 Xamarin Inc (http://www.xamarin.com)
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using CoreFoundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSMutableDictionary : NSDictionary, IDictionary, IDictionary<NSObject, NSObject> {

		// some API, like SecItemCopyMatching, returns a retained NSMutableDictionary
		internal NSMutableDictionary (NativeHandle handle, bool owns)
			: base (handle)
		{
			if (!owns)
				DangerousRelease ();
		}

		/// <param name="objects">To be added.</param>
		///         <param name="keys">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		///         <param name="keys">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSMutableDictionary FromObjectsAndKeys (object [] objects, object [] keys)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");

			using (var no = NSArray.FromObjects (objects))
			using (var nk = NSArray.FromObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		/// <param name="keys">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSMutableDictionary FromObjectsAndKeys (NSObject [] objects, NSObject [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromNSObjects (objects))
			using (var nk = NSArray.FromNSObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		/// <param name="objects">To be added.</param>
		/// <param name="keys">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public static NSMutableDictionary FromObjectsAndKeys (object [] objects, object [] keys, nint count)
		{
			if (objects is null)
				throw new ArgumentNullException (nameof (objects));
			if (keys is null)
				throw new ArgumentNullException (nameof (keys));
			if (objects.Length != keys.Length)
				throw new ArgumentException (nameof (objects) + " and " + nameof (keys) + " arrays have different sizes");
			if (count < 1 || objects.Length < count || keys.Length < count)
				throw new ArgumentException (nameof (count));

			using (var no = NSArray.FromObjects (objects))
			using (var nk = NSArray.FromObjects (keys))
				return FromObjectsAndKeysInternal (no, nk);
		}

		#region ICollection<KeyValuePair<NSObject, NSObject>>
		void ICollection<KeyValuePair<NSObject, NSObject>>.Add (KeyValuePair<NSObject, NSObject> item)
		{
			SetObject (item.Value, item.Key);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Clear ()
		{
			RemoveAllObjects ();
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Contains (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			return ContainsKeyValuePair (keyValuePair);
		}

		void ICollection<KeyValuePair<NSObject, NSObject>>.CopyTo (KeyValuePair<NSObject, NSObject> [] array, int index)
		{
			if (array is null)
				throw new ArgumentNullException (nameof (array));
			if (index < 0)
				throw new ArgumentOutOfRangeException (nameof (index));
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException (nameof (index) + " larger than largest valid index of array");
			if (array.Length - index < (int) Count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			var e = GetEnumerator ();
			while (e.MoveNext ())
				array [index++] = e.Current;
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.Remove (KeyValuePair<NSObject, NSObject> keyValuePair)
		{
			var count = Count;
			RemoveObjectForKey (keyValuePair.Key);
			return count != Count;
		}

		int ICollection<KeyValuePair<NSObject, NSObject>>.Count {
			get { return (int) Count; }
		}

		bool ICollection<KeyValuePair<NSObject, NSObject>>.IsReadOnly {
			get { return false; }
		}
		#endregion

		#region IDictionary
		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IDictionary.Add (object key, object value)
		{
			var nsokey = key as NSObject;
			var nsovalue = value as NSObject;

			if (nsokey is null || nsovalue is null)
				throw new ArgumentException ("You can only use NSObjects for keys and values in an NSMutableDictionary");

			// Inverted args
			SetObject (nsovalue, nsokey);
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		bool IDictionary.Contains (object key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));
			var _key = key as INativeObject;
			if (_key is null)
				return false;
			bool result = ContainsKey (_key.Handle);
			GC.KeepAlive (_key);
			return result;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IDictionaryEnumerator IDictionary.GetEnumerator ()
		{
			return (IDictionaryEnumerator) ((IEnumerable<KeyValuePair<NSObject, NSObject>>) this).GetEnumerator ();
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		void IDictionary.Remove (object key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));
			var nskey = key as INativeObject;
			if (nskey is null)
				throw new ArgumentException ("The key must be an INativeObject");

			_RemoveObjectForKey (nskey.Handle);
			GC.KeepAlive (nskey);
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool IDictionary.IsFixedSize {
			get { return false; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool IDictionary.IsReadOnly {
			get { return false; }
		}

		object IDictionary.this [object key] {
			get {
				var _key = key as INativeObject;
				if (_key is null)
					return null;
				object result = _ObjectForKey (_key.Handle);
				GC.KeepAlive (_key);
				return result;
			}
			set {
				var nsokey = key as INativeObject;
				var nsovalue = value as INativeObject;

				if (nsokey is null || nsovalue is null)
					throw new ArgumentException ("You can only use INativeObjects for keys and values in an NSMutableDictionary");

				_SetObject (nsovalue.Handle, nsokey.Handle);
				GC.KeepAlive (nsovalue);
				GC.KeepAlive (nsokey);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		ICollection IDictionary.Keys {
			get { return Keys; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		ICollection IDictionary.Values {
			get { return Values; }
		}
		#endregion

		#region IDictionary<NSObject, NSObject>
		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Add (NSObject key, NSObject value)
		{
			// Inverted args.
			SetObject (value, key);
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Remove (NSObject key)
		{
			if (key is null)
				throw new ArgumentNullException (nameof (key));

			var last = Count;
			RemoveObjectForKey (key);
			return last != Count;
		}

		/// <param name="key">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool TryGetValue (NSObject key, out NSObject value)
		{
			// Can't put null in NSDictionaries, so if null is returned, the key wasn't found.
			return (value = ObjectForKey (key)) is not null;
		}

		public override NSObject this [NSObject key] {
			get {
				return ObjectForKey (key);
			}
			set {
				SetObject (value, key);
			}
		}

		public override NSObject this [NSString key] {
			get {
				return ObjectForKey (key);
			}
			set {
				SetObject (value, key);
			}
		}

		public override NSObject this [string key] {
			get {
				if (key is null)
					throw new ArgumentNullException ("key");
				var nss = NSString.CreateNative (key, false);
				try {
					return Runtime.GetNSObject (LowlevelObjectForKey (nss));
				} finally {
					NSString.ReleaseNative (nss);
				}
			}
			set {
				if (key is null)
					throw new ArgumentNullException ("key");
				var nss = NSString.CreateNative (key, false);
				try {
					LowlevelSetObject (value, nss);
				} finally {
					NSString.ReleaseNative (nss);
				}
			}
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Keys {
			get { return Keys; }
		}

		ICollection<NSObject> IDictionary<NSObject, NSObject>.Values {
			get { return Values; }
		}
		#endregion

		#region IEnumerable
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<KeyValuePair<NSObject, NSObject>>) this).GetEnumerator ();
		}
		#endregion

		#region IEnumerable<K,V>
		/// <summary>Returns an enumerator that iterates through the dictionary.</summary>
		/// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
		public IEnumerator<KeyValuePair<NSObject, NSObject>> GetEnumerator ()
		{
			foreach (var key in Keys) {
				yield return new KeyValuePair<NSObject, NSObject> (key, ObjectForKey (key));
			}
		}
		#endregion

		/// <param name="obj">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSMutableDictionary LowlevelFromObjectAndKey (IntPtr obj, IntPtr key)
		{
#if MONOMAC
			return (NSMutableDictionary) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, selDictionaryWithObject_ForKey_XHandle, obj, key));
#else
			return (NSMutableDictionary) Runtime.GetNSObject (ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (class_ptr, Selector.GetHandle ("dictionaryWithObject:forKey:"), obj, key));
#endif
		}

		/// <param name="obj">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void LowlevelSetObject (IntPtr obj, IntPtr key)
		{
#if MONOMAC
			ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetObject_ForKey_XHandle, obj, key);
#else
			ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle ("setObject:forKey:"), obj, key);
#endif
		}

		/// <param name="obj">To be added.</param>
		///         <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void LowlevelSetObject (NSObject obj, IntPtr key)
		{
			if (obj is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (obj));

			LowlevelSetObject (obj.Handle, key);
			GC.KeepAlive (obj);
		}

		public void LowlevelSetObject (string str, IntPtr key)
		{
			if (str is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (str));

			var ptr = CFString.CreateNative (str);
			LowlevelSetObject (ptr, key);
			CFString.ReleaseNative (ptr);
		}
	}
}
