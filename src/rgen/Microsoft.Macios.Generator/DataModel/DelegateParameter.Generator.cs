// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct DelegateParameter {

	/// <summary>
	/// Returns the forced type data if present in the binding.
	/// </summary>
	public ForcedTypeData? ForcedType { get; init; }

	public static bool TryCreate (IParameterSymbol symbol,
		[NotNullWhen (true)] out DelegateParameter? parameter)
	{
		parameter = new (symbol.Ordinal, new (symbol.Type), symbol.GetSafeName ()) {
			ForcedType = symbol.GetForceTypeData (),
			IsOptional = symbol.IsOptional,
			IsParams = symbol.IsParams,
			IsThis = symbol.IsThis,
			ReferenceKind = symbol.RefKind.ToReferenceKind (),
			IsCCallback = symbol.HasAttribute (AttributesNames.CCallbackAttribute),
			IsBlockCallback = symbol.HasAttribute (AttributesNames.BlockCallbackAttribute),
		};
		return true;
	}

}
