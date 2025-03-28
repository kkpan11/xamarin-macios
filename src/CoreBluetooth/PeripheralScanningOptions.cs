// 
// PeripheralScanningOptions.cs: Implements strongly typed access for CBCentralManagerScanOption
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2013, Xamarin Inc.
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

using Foundation;
using CoreFoundation;
using ObjCRuntime;

#nullable enable

namespace CoreBluetooth {

	public partial class PeripheralScanningOptions {
#if !COREBUILD
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool AllowDuplicatesKey {
			get {
				var value = GetBoolValue (CBCentralManager.ScanOptionAllowDuplicatesKey);
				return value.HasValue ? value.Value : false; // default value is NO
			}
			set {
				SetNumberValue (CBCentralManager.ScanOptionAllowDuplicatesKey, value ? 1 : 0);
			}
		}
#endif
	}
}
