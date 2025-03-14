// 
// Authors
//    Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc. http://www.xamarin.com
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Mono.ApiTools {

	class FieldComparer : MemberComparer {

		public FieldComparer (State state)
			: base (state)
		{
		}

		public override string GroupName {
			get { return "fields"; }
		}

		public override string ElementName {
			get { return "field"; }
		}

		void RenderFieldAttributes (FieldAttributes source, FieldAttributes target, ApiChange change)
		{
			if (!State.IgnoreNonbreaking) {
#pragma warning disable SYSLIB0050 // 'FieldAttributes.NotSerialized' is obsolete: 'Formatter-based serialization is obsolete and should not be used.' (https://aka.ms/dotnet-warnings/SYSLIB0050)
				var srcNotSerialized = (source & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;
				var tgtNotSerialized = (target & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;
#pragma warning restore SYSLIB0050
				if (srcNotSerialized != tgtNotSerialized) {
					// this is not a breaking change, so only render it if it changed.
					if (srcNotSerialized) {
						change.AppendRemoved ($"[NonSerialized]{Environment.NewLine}", false);
					} else {
						change.AppendAdded ($"[NonSerialized]{Environment.NewLine}", false);
					}
				}

				var srcHasFieldMarshal = (source & FieldAttributes.HasFieldMarshal) != 0;
				var tgtHasFieldMarshal = (target & FieldAttributes.HasFieldMarshal) != 0;
				if (srcHasFieldMarshal != tgtHasFieldMarshal) {
					// this is not a breaking change, so only render it if it changed.
					if (srcHasFieldMarshal) {
						change.AppendRemoved ("[MarshalAs]", false);
					} else {
						change.AppendAdded ("[MarshalAs]", false);
					}
					change.Append (Environment.NewLine);
				}
			}

			// the visibility values are the same for MethodAttributes and FieldAttributes, so just use the same method.
			RenderVisibility ((MethodAttributes) source, (MethodAttributes) target, change);
			// same for the static flag
			RenderStatic ((MethodAttributes) source, (MethodAttributes) target, change);

			var srcLiteral = (source & FieldAttributes.Literal) != 0;
			var tgtLiteral = (target & FieldAttributes.Literal) != 0;

			if (srcLiteral) {
				if (tgtLiteral) {
					change.Append ("const ");
				} else {
					change.AppendRemoved ("const", true).Append (" ");
				}
			} else if (tgtLiteral) {
				change.AppendAdded ("const", true).Append (" ");
			}

			var srcInitOnly = (source & FieldAttributes.InitOnly) != 0;
			var tgtInitOnly = (target & FieldAttributes.InitOnly) != 0;
			if (srcInitOnly) {
				if (tgtInitOnly) {
					change.Append ("readonly ");
				} else {
					change.AppendRemoved ("readonly", false).Append (" ");
				}
			} else if (tgtInitOnly) {
				change.AppendAdded ("readonly", true).Append (" ");
			}
		}

		string GetFullName (XElement element)
		{
			var rv = element.GetAttribute ("name");
			element = element.Parent;
			while (element is not null) {
				if (element.Name.LocalName == "assembly")
					break;
				var name = element.GetAttribute ("name");
				if (!string.IsNullOrEmpty (name))
					rv = name + "." + rv;
				element = element.Parent;
			}
			return rv;
		}

		public override bool Equals (XElement source, XElement target, ApiChanges changes)
		{
			if (base.Equals (source, target, changes))
				return true;

			var name = source.GetAttribute ("name");
			var srcValue = source.GetAttribute ("value");
			var tgtValue = target.GetAttribute ("value");
			var change = new ApiChange (GetDescription (source), State);
			change.Header = "Modified " + GroupName;

			RenderAttributes (source, target, change);

			if (State.BaseType == "System.Enum") {
				change.Append (name).Append (" = ");
				if (srcValue != tgtValue) {
					change.AppendModified (srcValue, tgtValue, true);
				} else {
					change.Append (srcValue);
				}
			} else {
				RenderFieldAttributes (source.GetFieldAttributes (), target.GetFieldAttributes (), change);

				var srcType = source.GetTypeName ("fieldtype", State);
				var tgtType = target.GetTypeName ("fieldtype", State);

				if (srcType != tgtType) {
					change.AppendModified (srcType, tgtType, true);
				} else {
					change.Append (srcType);
				}
				change.Append (" ");
				change.Append (name);

				if (srcType == "string" && srcValue is not null)
					srcValue = "\"" + srcValue + "\"";

				if (tgtType == "string" && tgtValue is not null)
					tgtValue = "\"" + tgtValue + "\"";

				if (srcValue != tgtValue) {
					change.Append (" = ");
					if (srcValue is null)
						srcValue = "null";
					if (tgtValue is null)
						tgtValue = "null";

					// Hardcode that changes to ObjCRuntime.Constants.[Sdk]Version aren't breaking.
					var fullname = GetFullName (source);
					var breaking = !source.IsExperimental ();
					switch (fullname) {
					case "ObjCRuntime.Constants.Version":
					case "ObjCRuntime.Constants.SdkVersion":
						breaking = false;
						break;
					}
					change.AppendModified (srcValue, tgtValue, breaking);
				} else if (srcValue is not null) {
					change.Append (" = ");
					change.Append (srcValue);
				}
				change.Append (";");
			}

			changes.Add (source, target, change);

			return false;
		}

		public override string GetDescription (XElement e)
		{
			var sb = new StringBuilder ();

			string name = e.GetAttribute ("name");
			string value = e.GetAttribute ("value");

			if (State.BaseType == "System.Enum") {
				sb.Append (name).Append (" = ").Append (value).Append (',');
			} else {
				var attribs = e.Attribute ("attrib");
				if (attribs is not null) {
					var attr = (FieldAttributes) Int32.Parse (attribs.Value);
					if ((attr & FieldAttributes.Public) != FieldAttributes.Public) {
						sb.Append ("protected ");
					} else {
						sb.Append ("public ");
					}

					if ((attr & FieldAttributes.Static) != 0)
						sb.Append ("static ");

					if ((attr & FieldAttributes.Literal) != 0)
						sb.Append ("const ");
				}

				string ftype = e.GetTypeName ("fieldtype", State);
				sb.Append (ftype).Append (' ');
				sb.Append (name);
				if (ftype == "string" && e.Attribute ("value") is not null) {
					if (value is null)
						sb.Append (" = null");
					else
						sb.Append (" = \"").Append (value).Append ('"');
				}
				sb.Append (';');
			}

			return sb.ToString ();
		}
	}
}
