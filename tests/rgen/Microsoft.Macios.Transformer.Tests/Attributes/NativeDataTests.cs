// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Transformer.Attributes;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Microsoft.Macios.Transformer.Tests.Attributes;

public class NativeDataTests : BaseTransformerTestClass {

	class TestDataTryCreate : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var path = "/some/random/path.cs";
			var simnpleNativeEnum = @"
using System;
using Foundation;
using ObjCRuntime;

[Native]
public enum AVAudioQuality : long {
	Min = 0,
	Low = 0x20,
	Medium = 0x40,
	High = 0x60,
	Max = 0x7F,
}
";

			yield return [(Sorunce: simnpleNativeEnum, Path: path), new NativeData ()];

			var nativeEnumWithName = @"
using System;
using Foundation;
using ObjCRuntime;

[Native (""Test"")]
public enum AVAudioQuality : long {
	Min = 0,
	Low = 0x20,
	Medium = 0x40,
	High = 0x60,
	Max = 0x7F,
}
";

			yield return [(Sorunce: nativeEnumWithName, Path: path), new NativeData ("Test")];

			var nativeEnumWithNameNamed = @"
using System;
using Foundation;
using ObjCRuntime;

[Native (NativeName = ""Test"")]
public enum AVAudioQuality : long {
	Min = 0,
	Low = 0x20,
	Medium = 0x40,
	High = 0x60,
	Max = 0x7F,
}
";

			yield return [(Sorunce: nativeEnumWithNameNamed, Path: path), new NativeData ("Test")];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataTryCreate>]
	void TryCreateTests (ApplePlatform platform, (string Source, string Path) source,
		NativeData expectedData)
	{
		// create a compilation used to create the transformer
		var compilation = CreateCompilation (platform, sources: source);
		var syntaxTree = compilation.SyntaxTrees.ForSource (source);
		Assert.NotNull (syntaxTree);

		var semanticModel = compilation.GetSemanticModel (syntaxTree);
		Assert.NotNull (semanticModel);

		var declaration = syntaxTree.GetRoot ()
			.DescendantNodes ().OfType<EnumDeclarationSyntax> ()
			.LastOrDefault ();
		Assert.NotNull (declaration);

		var symbol = semanticModel.GetDeclaredSymbol (declaration);
		Assert.NotNull (symbol);
		var exportData = symbol.GetAttribute<NativeData> (
			AttributesNames.NativeAttribute, NativeData.TryParse);
		Assert.Equal (expectedData, exportData);
	}

}