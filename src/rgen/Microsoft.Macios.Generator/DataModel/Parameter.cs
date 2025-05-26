// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Readonly structure that represents a change in a parameter.
/// </summary>
[StructLayout (LayoutKind.Auto)]
readonly partial struct Parameter : IEquatable<Parameter> {
	/// <summary>
	/// Parameter position in the method.
	/// </summary>
	public int Position { get; }

	/// <summary>
	/// Type of the parameter.
	/// </summary>
	public TypeInfo Type { get; }

	/// <summary>
	/// Parameter name
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// True if the parameter is optional
	/// </summary>
	public bool IsOptional { get; init; }

	/// <summary>
	/// True if a parameter is using the 'params' modifier.
	/// </summary>
	public bool IsParams { get; init; }

	/// <summary>
	/// True if the parameter represents the 'this' pointer.
	/// </summary>
	public bool IsThis { get; init; }

	/// <summary>
	/// Optional default value.
	/// </summary>
	public string? DefaultValue { get; init; }

	/// <summary>
	/// The reference type used.
	/// </summary>
	public ReferenceKind ReferenceKind { get; init; }

	/// <summary>
	/// The parameter is passed by reference. This means any possible reference mode: in, out, ref.
	/// </summary>
	public bool IsByRef => ReferenceKind != ReferenceKind.None;

	/// <summary>
	/// List of attributes attached to the parameter.
	/// </summary>
	public ImmutableArray<AttributeCodeChange> Attributes { get; init; } = [];

	public Parameter (int position, TypeInfo type, string name)
	{
		Position = position;
		Type = type;
		Name = name;
	}

	/// <inheritdoc/>
	public bool Equals (Parameter other)
	{
		if (Position != other.Position)
			return false;
		if (Type != other.Type)
			return false;
		if (Name != other.Name)
			return false;
		if (IsOptional != other.IsOptional)
			return false;
		if (IsParams != other.IsParams)
			return false;
		if (IsThis != other.IsThis)
			return false;
		if (DefaultValue != other.DefaultValue)
			return false;
		if (ReferenceKind != other.ReferenceKind)
			return false;
		if (BindAs != other.BindAs)
			return false;
		if (ForcedType != other.ForcedType)
			return false;
		if (Type.Delegate != other.Type.Delegate)
			return false;

		var attributeComparer = new AttributesEqualityComparer ();
		return attributeComparer.Equals (Attributes, other.Attributes);
	}

	/// <inheritdoc/>
	public override bool Equals (object? obj)
	{
		return obj is Parameter other && Equals (other);
	}

	/// <inheritdoc/>
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (Position);
		hashCode.Add (Type);
		hashCode.Add (Name);
		hashCode.Add (IsOptional);
		hashCode.Add (IsParams);
		hashCode.Add (IsThis);
		hashCode.Add (DefaultValue);
		hashCode.Add ((int) ReferenceKind);
		hashCode.Add (Type.Delegate);
		hashCode.Add (BindAs);
		return hashCode.ToHashCode ();
	}

	public static bool operator == (Parameter left, Parameter right)
	{
		return left.Equals (right);
	}

	public static bool operator != (Parameter left, Parameter right)
	{
		return !left.Equals (right);
	}

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ("{");
		sb.Append ($"Position: {Position}, ");
		sb.Append ($"Type: {Type}, ");
		sb.Append ($"Name: {Name}, ");
		sb.Append ("Attributes: [");
		sb.AppendJoin (", ", Attributes);
		sb.Append ($"] IsOptional: {IsOptional}, ");
		sb.Append ($"IsParams: {IsParams}, ");
		sb.Append ($"IsThis: {IsThis}, ");
		sb.Append ($"DefaultValue: {DefaultValue}, ");
		sb.Append ($"ReferenceKind: {ReferenceKind}, ");
		sb.Append ($"BindAs: {BindAs?.ToString () ?? "null"}, ");
		sb.Append ($"ForcedType: {ForcedType?.ToString () ?? "null"}, ");
		sb.Append ($"Delegate: {Type.Delegate?.ToString () ?? "null"} }}");
		return sb.ToString ();
	}
}
