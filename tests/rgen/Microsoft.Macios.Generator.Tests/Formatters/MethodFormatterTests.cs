// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Formatters;

public class MethodFormatterTests : BaseGeneratorTestClass {

	class TestDataToDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string voidMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod ();
}
";
			yield return [voidMethod, "public partial void TestMethod ()"];

			const string noSpaceParamsMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod();
}
";
			yield return [noSpaceParamsMethod, "public partial void TestMethod ()"];

			const string singleParamMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(string myParam);
}
";
			yield return [singleParamMethod, "public partial void TestMethod (string myParam)"];

			const string severalParamMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(string myParam, int myParamInt);
}
";
			yield return [severalParamMethod, "public partial void TestMethod (string myParam, int myParamInt)"];

			const string nullableParamMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(string? myParam);
}
";
			yield return [nullableParamMethod, "public partial void TestMethod (string? myParam)"];

			const string nullableReturnTypeMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial string? TestMethod(string? myParam);
}
";
			yield return [nullableReturnTypeMethod, "public partial string? TestMethod (string? myParam)"];

			const string customReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial MyStruct TestMethod(string? myParam);
}
";
			yield return [customReturnMethod, $"public partial {Global ("NS.MyStruct")} TestMethod (string? myParam)"];

			const string nullableCustomReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial MyStruct? TestMethod(string? myParam);
}
";
			yield return [nullableCustomReturnMethod, $"public partial {Global ("NS.MyStruct")}? TestMethod (string? myParam)"];

			const string customParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(MyStruct myParam);
}
";
			yield return [customParamMethod, $"public partial void TestMethod ({Global ("NS.MyStruct")} myParam)"];

			const string nullableCustomParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(MyStruct? myParam);
}
";
			yield return [nullableCustomParamMethod, $"public partial void TestMethod ({Global ("NS.MyStruct")}? myParam)"];

			const string refCustomParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(ref MyStruct myParam);
}
";
			yield return [refCustomParamMethod, $"public partial void TestMethod (ref {Global ("NS.MyStruct")} myParam)"];

			const string outCustomParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(out MyStruct myParam);
}
";
			yield return [outCustomParamMethod, $"public partial void TestMethod (out {Global ("NS.MyStruct")} myParam)"];

			const string inCustomParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(in MyStruct myParam);
}
";
			yield return [inCustomParamMethod, $"public partial void TestMethod (in {Global ("NS.MyStruct")} myParam)"];

			const string refReadonlyCustomParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(ref readonly MyStruct myParam);
}
";
			yield return [refReadonlyCustomParamMethod, $"public partial void TestMethod (ref readonly {Global ("NS.MyStruct")} myParam)"];

			const string arrayParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(MyStruct[] myParam);
}
";

			yield return [arrayParamMethod, $"public partial void TestMethod ({Global ("NS.MyStruct")}[] myParam)"];

			const string nullableArrayParamMethod = @"
using System;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(MyStruct[]? myParam);
}
";

			yield return [nullableArrayParamMethod, $"public partial void TestMethod ({Global ("NS.MyStruct")}[]? myParam)"];

			const string genericParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<string> myParam);
}
";

			yield return [genericParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<string> myParam)"];

			const string genericCustomParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<MyStruct> myParam);
}
";

			yield return [genericCustomParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<{Global ("NS.MyStruct")}> myParam)"];

			const string nullableGenericParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<string>? myParam);
}
";

			yield return [nullableGenericParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<string>? myParam)"];

			const string nullableGenericCustomParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<MyStruct>? myParam);
}
";

			yield return [nullableGenericCustomParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<{Global ("NS.MyStruct")}>? myParam)"];

			const string nullableNullableGenericParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<string?>? myParam);
}
";

			yield return [nullableNullableGenericParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<string?>? myParam)"];

			const string nullableNullableGenericCustomParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(List<MyStruct?>? myParam);
}
";

			yield return [nullableNullableGenericCustomParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.List")}<{Global ("NS.MyStruct")}?>? myParam)"];

			const string genericDictParamMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(Dictionary<string, string> myParam);
}
";

			yield return [genericDictParamMethod, $"public partial void TestMethod ({Global ("System.Collections.Generic.Dictionary")}<string, string> myParam)"];

			const string genericReturnMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial List<string> TestMethod();
}
";

			yield return [genericReturnMethod, $"public partial {Global ("System.Collections.Generic.List")}<string> TestMethod ()"];

			const string genericCustomReturnMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

public struct MyStruct {
	public string Name;
	public string Surname;
}

[BindingType<Class>]
public partial class MyClass {
	public partial List<MyStruct> TestMethod();
}
";

			yield return [genericCustomReturnMethod, $"public partial {Global ("System.Collections.Generic.List")}<{Global ("NS.MyStruct")}> TestMethod ()"];

			const string nullableGenericReturnMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial List<string>? TestMethod();
}
";

			yield return [nullableGenericReturnMethod, $"public partial {Global ("System.Collections.Generic.List")}<string>? TestMethod ()"];

			const string nullableNullableGenericReturnMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial List<string?>? TestMethod();
}
";

			yield return [nullableNullableGenericReturnMethod, $"public partial {Global ("System.Collections.Generic.List")}<string?>? TestMethod ()"];

			const string paramsMethod = @"
using System;
using System.Collections.Generic;
using ObjCBindings;
namespace NS;

[BindingType<Class>]
public partial class MyClass {
	public partial void TestMethod(params string[] data);
}
";

			yield return [paramsMethod, "public partial void TestMethod (params string[] data)"];


			const string actionNoParam = @"
using System;

namespace NS {
	public class MyClass {
		public partial void TestMethod (Action cb) {}
	}
}
";
			yield return [actionNoParam, $"public partial void TestMethod ({Global ("System.Action")} cb)"];

			const string actionSingleParam = @"
using System;

namespace NS {
	public class MyClass {
		public partial void TestMethod (Action<string> cb) {}
	}
}
";
			yield return [actionSingleParam, $"public partial void TestMethod ({Global ("System.Action")}<string> cb)"];

			const string actionSingleNullableParam = @"
using System;

namespace NS {
	public class MyClass {
		public partial void TestMethod (Action<string?> cb) {}
	}
}
";
			yield return [actionSingleNullableParam, $"public partial void TestMethod ({Global ("System.Action")}<string?> cb)"];

			const string actionMultiParam = @"
using System;

namespace NS {
	public class MyClass {
		public partial void TestMethod (Action<string, string> cb) {}
	}
}
";
			yield return [actionMultiParam, $"public partial void TestMethod ({Global ("System.Action")}<string, string> cb)"];

			const string funcSingleParam = @"
using System;

namespace NS {
	public class MyClass {
		public partial void TestMethod (Func<string, string> cb) {}
	}
}
";
			yield return [funcSingleParam, $"public partial void TestMethod ({Global ("System.Func")}<string, string> cb)"];

			const string customDelegate = @"
using System;

namespace NS {
	public class MyClass {
		public delegate int? Callback(string name, string? middleName, params string[] surname);

		public partial void TestMethod (Callback cb) {}
	}
}
";
			yield return [customDelegate, $"public partial void TestMethod ({Global ("NS.MyClass.Callback")} cb)"];

			const string intReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial int TestMethod(string myParam, int myParamInt);
}
";
			yield return [intReturnMethod, "public partial int TestMethod (string myParam, int myParamInt)"];

			const string nullableIntReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial int? TestMethod(string myParam, int myParamInt);
}
";
			yield return [nullableIntReturnMethod, "public partial int? TestMethod (string myParam, int myParamInt)"];

			const string intPtrReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial IntPtr TestMethod(string myParam, int myParamInt);
}
";
			yield return [intPtrReturnMethod, "public partial IntPtr TestMethod (string myParam, int myParamInt)"];

			const string nullableIntPtrReturnMethod = @"
using System;
using ObjCBindings;
namespace NS;
[BindingType<Class>]
public partial class MyClass {
	public partial IntPtr? TestMethod(string myParam, int myParamInt);
}
";
			yield return [nullableIntPtrReturnMethod, "public partial IntPtr? TestMethod (string myParam, int myParamInt)"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataToDeclaration>]
	public void ToDeclarationTests (ApplePlatform platform, string inputText, string expectedDeclaration)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ()
			.OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		Assert.NotNull (semanticModel);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var method));
		Assert.NotNull (method);
		var methodDeclaration = method.ToDeclaration ();
		Assert.NotNull (methodDeclaration);
		Assert.Equal (expectedDeclaration, methodDeclaration.ToString ());
	}
}
