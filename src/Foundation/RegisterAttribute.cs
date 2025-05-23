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

#nullable enable

namespace Foundation {

	/// <include file="../../docs/api/Foundation/RegisterAttribute.xml" path="/Documentation/Docs[@DocId='T:Foundation.RegisterAttribute']/*" />
	[AttributeUsage (AttributeTargets.Class)]
	public sealed class RegisterAttribute : Attribute {
		string? name;
		bool is_wrapper;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public RegisterAttribute () { }
		/// <param name="name">The name to use when exposing this class to the Objective-C world.</param>
		///         <summary>Used to specify how the ECMA class is exposed as an Objective-C class.</summary>
		///         <remarks>To be added.</remarks>
		public RegisterAttribute (string name)
		{
			this.name = name;
		}

		/// <param name="name">The name to use when exposing this class to the Objective-C world.</param>
		///         <param name="isWrapper">Used to specify if the class being registered is wrapping an existing Objective-C class, or if it's a new class.</param>
		///         <summary>Used to specify how the ECMA class is exposed as an Objective-C class.</summary>
		///         <remarks>To be added.</remarks>
		public RegisterAttribute (string name, bool isWrapper)
		{
			this.name = name;
			this.is_wrapper = isWrapper;
		}

		/// <summary>The name used to expose the class.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Name {
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>Specifies whether the class being registered is wrapping an existing Objective-C class, or if it's a new class.</summary>
		///         <value>True if the class being registered is wrapping an existing Objective-C class.</value>
		///         <remarks>To be added.</remarks>
		public bool IsWrapper {
			get { return this.is_wrapper; }
			set { this.is_wrapper = value; }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SkipRegistration { get; set; }

		/// <summary>
		/// Specifies whether the Objective-C class is a stub class.
		/// Objective-C stub classes are sometimes used when bridging Swift to Objective-C.
		/// </summary>
		/// <remarks>
		///   <para>Stub classes can be identified because they include SWIFT_RESILIENT_CLASS in the generated Objective-C header.</para>
		///   <para>Example Objective-C type declaration:</para>
		///   <example>
		///     <code lang="objective-c"><![CDATA[
		///   SWIFT_RESILIENT_CLASS("_TtC16MySwiftFramework11MySwiftType")
		///   @interface MySwiftType : SwiftTypeFromDifferentSwiftFramework
		///   @end
		/// ]]></code>
		///   </example>
		/// </remarks>
		public bool IsStubClass { get; set; }
	}
}
