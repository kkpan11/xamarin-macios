// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Transformer.Attributes;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct EnumMember {

	/// <summary>
	/// The data of the field attribute used to mark the value as a binding.
	/// </summary>
	public FieldData? FieldInfo { get; }

	/// <summary>
	/// Create a new change that happened on a member.
	/// </summary>
	/// <param name="name">The name of the changed member.</param>
	/// <param name="libraryName">The library name of the smart enum.</param>
	/// <param name="libraryPath">The library path to the library, null if it is a known frameworl.</param>
	/// <param name="fieldData">The binding data attached to this enum value.</param>
	/// <param name="symbolAvailability">The symbol availability of the member.</param>
	/// <param name="attributes">The list of attribute changes in the member.</param>
	public EnumMember (string name,
		string libraryName,
		string? libraryPath,
		FieldData? fieldData,
		SymbolAvailability symbolAvailability,
		ImmutableArray<AttributeCodeChange> attributes)
	{
		throw new NotImplementedException ();
	}

}
