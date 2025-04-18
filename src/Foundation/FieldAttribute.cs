//
// FieldAttribute.cs: The Field attribute
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2013 Xamarin Inc
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

#nullable enable

namespace Foundation {
	/// <summary>This attribute is present on properties to indicate that they reflect an underlying unmanaged global variable.</summary>
	///     <remarks>
	///       When this attribute is present on a property, it indicates that the property actually reflects an underlying unmanaged global variable.
	///     </remarks>
	[AttributeUsage (AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class FieldAttribute : Attribute {
		/// <param name="symbolName">The unmanaged symbol that this field represents.</param>
		///         <summary>Creates a new FieldAttribute instance with the specific symbol to bind.</summary>
		///         <remarks>
		/// 	  Used by Objective-C bindings to bind an unmanaged global variable as a static field.   
		/// 	</remarks>
		public FieldAttribute (string symbolName)
		{
			SymbolName = symbolName;
		}
		/// <param name="symbolName">The unmanaged symbol that this field represents.</param>
		///         <param name="libraryName">The library name to bind.   Use "__Internal" for referencing symbols on libraries that are statically linked with your application.</param>
		///         <summary>Creates a new FieldAttribute instance with the specific symbol to bind.</summary>
		///         <remarks>
		/// 	  Used by Objective-C bindings to bind an unmanaged global variable as a static field.   
		/// 	</remarks>
		public FieldAttribute (string symbolName, string libraryName)
		{
			SymbolName = symbolName;
			LibraryName = libraryName;
		}
		/// <summary>The global symbol that this field represents.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string SymbolName { get; set; }
		/// <summary>The library name where the global symbol is looked up from.</summary>
		///         <value>
		///         </value>
		///         <remarks>The special name "__Internal" means that the symbol is looked up on the current executable.</remarks>
		public string? LibraryName { get; set; }
	}
}
