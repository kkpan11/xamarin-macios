// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Interface to be implemented by all those classes that know how to emit code for a binding.
/// </summary>
interface ICodeEmitter {
	string GetSymbolName (in Binding binding);
	bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics);
	IEnumerable<string> UsingStatements { get; }
}
