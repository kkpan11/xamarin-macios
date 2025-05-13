// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryFieldAccessorsTests : BaseGeneratorTestClass {

	class TestDataFieldConstantGetter : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string nsStringFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSString GenericGray { get; }

	}
}
";

			yield return [nsStringFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetStringConstant (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")!"];

			const string byteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial byte GenericGray { get; }

	}
}
";

			yield return [byteFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherByteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Byte GenericGray { get; }

	}
}
";
			yield return [otherByteFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string sbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial sbyte GenericGray { get; }

	}
}
";

			yield return [sbyteFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherSbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial SByte GenericGray { get; }

	}
}
";

			yield return [otherSbyteFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string int16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial short GenericGray { get; }

	}
}
";

			yield return [int16FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherInt16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int16 GenericGray { get; }

	}
}
";

			yield return [otherInt16FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial ushort GenericGray { get; }

	}
}
";

			yield return [uint16FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherUint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt16 GenericGray { get; }

	}
}
";

			yield return [otherUint16FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string int32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial int GenericGray { get; }

	}
}
";

			yield return [int32FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherInt32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int32 GenericGray { get; }

	}
}
";

			yield return [otherInt32FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial uint GenericGray { get; }

	}
}
";

			yield return [uint32FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherUint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt32 GenericGray { get; }

	}
}
";

			yield return [otherUint32FieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string doubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial double GenericGray { get; }

	}
}
";

			yield return [doubleFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetDouble (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherDoubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Double GenericGray { get; }

	}
}
";

			yield return [otherDoubleFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetDouble (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string floatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial float GenericGray { get; }

	}
}
";

			yield return [floatFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string otherFloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Single GenericGray { get; }

	}
}
";

			yield return [otherFloatFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string intPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial IntPtr GenericGray { get; }

	}
}
";

			yield return [intPtrFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string uintPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UIntPtr GenericGray { get; }

	}
}
";

			yield return [uintPtrFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nint GenericGray { get; }

	}
}
";

			yield return [nintFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nuintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nuint GenericGray { get; }

	}
}
";

			yield return [nuintFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nfloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nfloat GenericGray { get; }

	}
}
";

			yield return [nfloatFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetNFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string cgsizeFieldProperty = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGSize GenericGray { get; }

	}
}
";

			yield return [cgsizeFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetCGSize (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string cmtagFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTag GenericGray { get; }

	}
}
";

			yield return [cmtagFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetStruct<CoreMedia.CMTag> (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"];

			const string nsArrayFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSArray GenericGray { get; }

	}
}
";

			yield return [nsArrayFieldProperty,
				"global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSArray> (global::ObjCRuntime.Dlfcn.GetIndirect (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\"))!"];

			const string nsNumberFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSNumber GenericGray { get; }

	}
}
";

			yield return [nsNumberFieldProperty,
				"global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSNumber> (global::ObjCRuntime.Dlfcn.GetIndirect (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\"))!"];

			const string sbyteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : sbyte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				sbyteEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string byteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : byte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				byteEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string shortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : short {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				shortEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string ushortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ushort {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ushortEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string intEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : int {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				intEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string uintEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : uint {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				uintEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string longEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : long {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				longEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetInt64 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string ulongEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ulong {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ulongEnumFieldProperty,
				"global::ObjCRuntime.Dlfcn.GetUInt64 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\")"
			];

			const string cmTimeFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTime GenericGray { get; }

	}
}
";

			yield return [
				cmTimeFieldProperty,
				"*((CoreMedia.CMTime*) global::ObjCRuntime.Dlfcn.dlsym (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\"))"
			];

			const string whiteFieldProperty = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial AVCaptureWhiteBalanceGains GenericGray { get; }

	}
}
";

			yield return [
				whiteFieldProperty,
				"*((AVFoundation.AVCaptureWhiteBalanceGains*) global::ObjCRuntime.Dlfcn.dlsym (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\"))"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFieldConstantGetter>]
	void FieldConstantGetterTests (ApplePlatform platform, string inputText, string expectedCall)
	{
		var (compilation, sourceTrees) =
			CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var context = new RootContext (semanticModel);
		Assert.True (Property.TryCreate (node, context, out var property));
		var compilationUnit = FieldConstantGetter (property.Value).ToString ();
		var str = compilationUnit.ToString ();
		Assert.Equal (expectedCall, FieldConstantGetter (property.Value).ToString ());
	}

	class TestDataFieldConstantSetter : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string nsStringFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSString GenericGray { get; }

	}
}
";

			yield return [nsStringFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetString (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string byteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial byte GenericGray { get; }

	}
}
";

			yield return [byteFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherByteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Byte GenericGray { get; }

	}
}
";
			yield return [otherByteFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string sbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial sbyte GenericGray { get; }

	}
}
";

			yield return [sbyteFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherSbyteFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial SByte GenericGray { get; }

	}
}
";

			yield return [otherSbyteFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string int16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial short GenericGray { get; }

	}
}
";

			yield return [int16FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherInt16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int16 GenericGray { get; }

	}
}
";

			yield return [otherInt16FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial ushort GenericGray { get; }

	}
}
";

			yield return [uint16FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherUint16FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt16 GenericGray { get; }

	}
}
";

			yield return [otherUint16FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string int32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial int GenericGray { get; }

	}
}
";

			yield return [int32FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherInt32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Int32 GenericGray { get; }

	}
}
";

			yield return [otherInt32FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial uint GenericGray { get; }

	}
}
";

			yield return [uint32FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherUint32FieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UInt32 GenericGray { get; }

	}
}
";

			yield return [otherUint32FieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string doubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial double GenericGray { get; }

	}
}
";

			yield return [doubleFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetDouble (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherDoubleFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Double GenericGray { get; }

	}
}
";

			yield return [otherDoubleFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetDouble (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string floatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial float GenericGray { get; }

	}
}
";

			yield return [floatFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string otherFloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial Single GenericGray { get; }

	}
}
";

			yield return [otherFloatFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string intPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial IntPtr GenericGray { get; }

	}
}
";

			yield return [intPtrFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string uintPtrFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial UIntPtr GenericGray { get; }

	}
}
";

			yield return [uintPtrFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nint GenericGray { get; }

	}
}
";

			yield return [nintFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nuintFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nuint GenericGray { get; }

	}
}
";

			yield return [nuintFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUIntPtr (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nfloatFieldProperty = @"
using System;
using Foundation;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial nfloat GenericGray { get; }

	}
}
";

			yield return [nfloatFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetNFloat (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string cgsizeFieldProperty = @"
using System;
using Foundation;
using CoreGraphics;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGSize GenericGray { get; }

	}
}
";

			yield return [cgsizeFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetCGSize (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string cmtagFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTag GenericGray { get; }

	}
}
";

			yield return [
				cmtagFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'CoreMedia.CMTag' is not supported.\")"];

			const string nsArrayFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSArray GenericGray { get; }

	}
}
";

			yield return [nsArrayFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetArray (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string nsNumberFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial NSNumber GenericGray { get; }

	}
}
";

			yield return [nsNumberFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetObject (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", value)"];

			const string sbyteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : sbyte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				sbyteEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetSByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (sbyte)value)"
			];

			const string byteEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : byte{
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				byteEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetByte (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (byte)value)"
			];

			const string shortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : short {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				shortEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (short)value)"
			];

			const string ushortEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ushort {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ushortEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt16 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (ushort)value)"
			];

			const string intEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : int {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				intEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (int)value)"
			];

			const string uintEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : uint {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				uintEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt32 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (uint)value)"
			];

			const string longEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : long {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				longEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetInt64 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (long)value)"
			];

			const string ulongEnumFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	public enum CGColorSpaceGenericGray : ulong {
		First, 
		Second,
	}

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CGColorSpaceGenericGray GenericGray { get; }

	}
}
";

			yield return [
				ulongEnumFieldProperty,
				"value",
				"global::ObjCRuntime.Dlfcn.SetUInt64 (global::ObjCRuntime.Libraries.CoreGraphics.Handle, \"kCGColorSpaceGenericGray\", (ulong)value)"
			];

			const string cmTimeFieldProperty = @"
using System;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial CMTime GenericGray { get; }

	}
}
";

			yield return [
				cmTimeFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'CoreMedia.CMTime' is not supported.\")"];

			const string whiteFieldProperty = @"
using System;
using AVFoundation;
using Foundation;
using CoreMedia;
using ObjCBindings;

namespace CoreGraphics {

	[BindingType<Class>]
	public partial class CGColorSpaceNames {

		[Field<Property> (""kCGColorSpaceGenericGray"")]
		public partial AVCaptureWhiteBalanceGains GenericGray { get; }

	}
}
";

			yield return [
				whiteFieldProperty,
				"value",
				"throw new NotSupportedException(\"Setting fields of type 'AVFoundation.AVCaptureWhiteBalanceGains' is not supported.\")"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFieldConstantSetter>]
	void FieldConstantSetterTests (ApplePlatform platform, string inputText, string variableName, string expectedCall)
	{
		var (compilation, sourceTrees) =
			CreateCompilation (platform, sources: inputText);
		Assert.Single (sourceTrees);
		// get the declarations we want to work with and the semantic model
		var node = sourceTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<PropertyDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (node);
		var semanticModel = compilation.GetSemanticModel (sourceTrees [0]);
		var context = new RootContext (semanticModel);
		Assert.True (Property.TryCreate (node, context, out var property));
		Assert.Equal (expectedCall, FieldConstantSetter (property.Value, variableName).ToString ());
	}
}
