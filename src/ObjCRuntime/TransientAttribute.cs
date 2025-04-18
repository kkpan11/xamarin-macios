//
// Attribute to mark properties as transient, so that we don't keep
// a managed reference to them more than absolutely required.
//
// Copyright 2013 Xamarin Inc
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
using System;

namespace ObjCRuntime {
	/// <include file="../../docs/api/ObjCRuntime/TransientAttribute.xml" path="/Documentation/Docs[@DocId='T:ObjCRuntime.TransientAttribute']/*" />
	[AttributeUsage (AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class TransientAttribute : Attribute {
	}
}
