// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.Extensions;
using ObjCRuntime;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct Method {

	/// <summary>
	/// The data of the export attribute used to mark the value as a property binding. 
	/// </summary>
	public ExportData<ObjCBindings.Method> ExportMethodData { get; }

	/// <summary>
	/// Returns the bind from data if present in the binding.
	/// </summary>
	public BindFromData? BindAs { get; init; }

	/// <summary>
	/// Returns if the method was marked as thread safe.
	/// </summary>
	public bool IsThreadSafe => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.IsThreadSafe);

	/// <summary>
	/// True if the method was exported with the MarshalNativeExceptions flag allowing it to support native exceptions.
	/// </summary>
	public bool MarshalNativeExceptions => ExportMethodData.Flags.HasFlag (ObjCBindings.Method.MarshalNativeExceptions);

	public Method (string type, string name, TypeInfo returnType,
		SymbolAvailability symbolAvailability,
		ExportData<ObjCBindings.Method> exportMethodData,
		ImmutableArray<AttributeCodeChange> attributes,
		ImmutableArray<SyntaxToken> modifiers,
		ImmutableArray<Parameter> parameters)
	{
		Type = type;
		Name = name;
		ReturnType = returnType;
		SymbolAvailability = symbolAvailability;
		ExportMethodData = exportMethodData;
		Attributes = attributes;
		Modifiers = modifiers;
		Parameters = parameters;
	}

	public static bool TryCreate (MethodDeclarationSyntax declaration, RootBindingContext context,
		[NotNullWhen (true)] out Method? change)
	{
		if (context.SemanticModel.GetDeclaredSymbol (declaration) is not IMethodSymbol method) {
			change = null;
			return false;
		}

		var attributes = declaration.GetAttributeCodeChanges (context.SemanticModel);
		var parametersBucket = ImmutableArray.CreateBuilder<Parameter> ();
		// loop over the parameters of the construct since changes on those implies a change in the generated code
		foreach (var parameter in method.Parameters) {
			var parameterDeclaration = declaration.ParameterList.Parameters [parameter.Ordinal];
			if (!Parameter.TryCreate (parameter, parameterDeclaration, context.SemanticModel, out var parameterChange))
				continue;
			parametersBucket.Add (parameterChange.Value);
		}

		// DO NOT USE default if null, the reason is that it will set the ArgumentSemantics to be value 0, when
		// none is value 1. The reason for that is that the default of an enum is 0, that was a mistake 
		// in the old binding code.
		var exportData = method.GetExportData<ObjCBindings.Method> ()
						 ?? new (null, ArgumentSemantic.None, ObjCBindings.Method.Default);

		change = new (
			type: method.ContainingSymbol.ToDisplayString ().Trim (), // we want the full name
			name: method.Name,
			returnType: new TypeInfo (method.ReturnType),
			symbolAvailability: method.GetSupportedPlatforms (),
			exportMethodData: exportData,
			attributes: attributes,
			modifiers: [.. declaration.Modifiers],
			parameters: parametersBucket.ToImmutableArray ()) {
			BindAs = method.GetBindFromData (),
		};

		return true;
	}
}
