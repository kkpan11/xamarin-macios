// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;

namespace Microsoft.Macios.Generator.DataModel;

class TypeInfoComparer : IComparer<TypeInfo> {

	/// <inheritdoc/>
	public int Compare (TypeInfo x, TypeInfo y)
	{
		var returnTypeComparison = String.Compare (x.FullyQualifiedName, y.FullyQualifiedName, StringComparison.Ordinal);
		if (returnTypeComparison != 0)
			return returnTypeComparison;
		var isNullableComparison = x.IsNullable.CompareTo (y.IsNullable);
		if (isNullableComparison != 0)
			return isNullableComparison;
		var isBlittableComparison = x.IsBlittable.CompareTo (y.IsBlittable);
		if (isBlittableComparison != 0)
			return isBlittableComparison;
		var isSmartEnumComparison = x.IsSmartEnum.CompareTo (y.IsSmartEnum);
		if (isSmartEnumComparison != 0)
			return isSmartEnumComparison;
		var isArrayComparison = x.IsArray.CompareTo (y.IsArray);
		if (isArrayComparison != 0)
			return isArrayComparison;
		var isReferenceTypeComparison = x.IsReferenceType.CompareTo (y.IsReferenceType);
		if (isReferenceTypeComparison != 0)
			return isReferenceTypeComparison;
		return x.IsVoid.CompareTo (y.IsVoid);
	}

}
