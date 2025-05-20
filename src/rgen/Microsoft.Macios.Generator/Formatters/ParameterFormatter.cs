// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using ParameterDataModel = Microsoft.Macios.Generator.DataModel.Parameter;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Formatters;

static class ParameterFormatter {

	public static ParameterListSyntax GetParameterList (this in ImmutableArray<Parameter> parameters)
	{
		if (parameters.Length == 0)
			return ParameterList ([]);

		// the size of the array is simple to calculate, we need space for all parameters
		// and for a comma for each parameter except for the last one
		// length = parameters.Length + parameters.Length - 1
		// length = (2 * parameters.Length) - 1
		var nodes = new SyntaxNodeOrToken [(2 * parameters.Length) - 1];
		var nodesIndex = 0;
		var parametersIndex = 0;
		while (nodesIndex < nodes.Length) {
			var currentParameter = parameters [parametersIndex++];
			var parameterDeclaration = currentParameter.ToDeclaration ();
			nodes [nodesIndex++] = parameterDeclaration;
			if (currentParameter.Position < parameters.Length - 1) {
				nodes [nodesIndex++] = Token (SyntaxKind.CommaToken);
			}
		}

		return ParameterList (SeparatedList<ParameterSyntax> (nodes)).NormalizeWhitespace ();
	}

	public static ParameterSyntax ToDeclaration (this in ParameterDataModel parameter)
	{
		// modifiers come from two situations, we have the params keyword or not. We cannot have params + a ref modifier
		// so we build them based on that. If you call WithModifiers twice, you will me stepping on the previous collection
		// it won't be a merge
		var modifiers = parameter.IsParams
			? TokenList (Token (SyntaxKind.ParamsKeyword))
			: parameter.ReferenceKind.ToTokens ();

		// we are going to be ignoring the default value, the reason for it is that the partial method implementation
		// can ignore it. The following c# code is valid:
		//
		// TestPartial1.cs:
		//
		// public partial class TestPartial
		// {
		//
		//	  public partial void TestMethod(MyStruct[]? values = null);
		// 	  public void ExampleCall()
		// 	  {
		// 	  	  TestMethod();
		// 	  }
		// }
		// TestPartial2.cs:
		//
		// 
		// public partial class TestPartial
		// {
		//    public partial void TestMethod(MyStruct[]? values)
		//    {
		//
		//    }
		// }
		var syntax = Parameter (Identifier (parameter.Name))
			.WithModifiers (modifiers)
			.WithType (parameter.Type.GetIdentifierSyntax ());

		return syntax.NormalizeWhitespace ();
	}
}
