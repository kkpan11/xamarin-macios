using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin;
using Xamarin.Tests;
using Xamarin.Utils;

namespace GeneratorTests {
	[TestFixture ()]
	[Parallelizable (ParallelScope.All)]
	public class BGenTests : BGenBase {
		// Removing the following variable might make running the unit tests in VSMac fail.
		static Type variable_to_keep_reference_to_system_runtime_compilerservices_unsafe_assembly = typeof (System.Runtime.CompilerServices.Unsafe);

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void BMac_Smoke (Profile profile)
		{
			BuildFile (profile, "bmac_smoke.cs");
		}

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void BMac_With_Hyphen_In_Name (Profile profile)
		{
			BuildFile (profile, "bmac-with-hyphen-in-name.cs");
		}

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void PropertyRedefinitionMac (Profile profile)
		{
			BuildFile (profile, "property-redefination-mac.cs");
		}

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void NSApplicationPublicEnsureMethods (Profile profile)
		{
			BuildFile (profile, "NSApplicationPublicEnsureMethods.cs");
		}

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void ProtocolDuplicateAbstract (Profile profile)
		{
			BuildFile (profile, "protocol-duplicate-abstract.cs");
		}

		[Test]
		public void Bug15283 ()
		{
			BuildFile (Profile.iOS, "bug15283.cs");
		}

		[Test]
		public void Bug15307 ()
		{
			BuildFile (Profile.iOS, "bug15307.cs");
		}

		[Test]
		public void Bug16036 ()
		{
			BuildFile (Profile.iOS, "bug16036.cs");
		}

		[Test]
		public void Bug17232 ()
		{
			BuildFile (Profile.iOS, "bug17232.cs");
		}

		[Test]
		public void Bug24078 ()
		{
			BuildFile (Profile.iOS, "bug24078-ignore-methods-events.cs");
		}

		[Test]
		public void Bug27428 ()
		{
			BuildFile (Profile.iOS, "bug27428.cs");
		}

		[Test]
		public void Bug27430 ()
		{
			BuildFile (Profile.iOS, "bug27430.cs");
		}

		[Test]
		public void Bug27986 ()
		{
			var bgen = BuildFile (Profile.iOS, false, "bug27986.cs");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMembers = ((IEnumerable<ICustomAttributeProvider>) allTypes)
				.Union (allTypes.SelectMany ((type) => type.Methods))
				.Union (allTypes.SelectMany ((type) => type.Fields))
				.Union (allTypes.SelectMany ((type) => type.Properties));

			var preserves = allMembers.Count ((v) => v.HasCustomAttributes && v.CustomAttributes.Any ((ca) => ca.AttributeType.Name == "PreserveAttribute"));
			Assert.AreEqual (36, preserves, "Preserve attribute count"); // If you modified code that generates PreserveAttributes please update the preserve count
		}

		[Test]
		public void Bug29493 ()
		{
			var bgen = BuildFile (Profile.iOS, false, "bug29493.cs");

			// Check that there is no call to Class.GetHandle with a "global::"-prefixed string
			foreach (var method in bgen.ApiAssembly.MainModule.GetTypes ().SelectMany ((v) => v.Methods)) {
				if (!method.HasBody)
					continue;
				var instructions = method.Body.Instructions;
				foreach (var ins in instructions) {
					if (ins.OpCode.FlowControl != FlowControl.Call)
						continue;
					var mr = (MethodReference) ins.Operand;
					if (mr.DeclaringType.Namespace != "ObjCRuntime" && mr.DeclaringType.Name != "Class")
						continue;
					if (mr.Name != "GetHandle" && mr.Name != "GetHandleIntrinsic")
						continue;
					var str = (string) ins.Previous.Operand;
					if (str.StartsWith ("global::", StringComparison.Ordinal))
						Assert.Fail ($"Found a call to Class.GetHandle with an invalid ('global::'-prefixed) string in {method.FullName} at offset {ins.Offset}.\n\t{string.Join ("\n\t", instructions)}");
				}
			}
		}

		[Test]
		[TestCase (Profile.macOSMobile)]
		public void Bug31788 (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.Defines = BGenTool.GetDefaultDefines (bgen.Profile);
			bgen.CreateTemporaryBinding (File.ReadAllText (Path.Combine (Configuration.SourceRoot, "tests", "generator", "bug31788.cs")));
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();

			bgen.AssertApiCallsMethod ("Test", "MarshalInProperty", "get_Shared", "xamarin_NativeHandle_objc_msgSend_exception", "MarshalInProperty.Shared getter");
			bgen.AssertApiCallsMethod ("Test", "MarshalOnProperty", "get_Shared", "xamarin_NativeHandle_objc_msgSend_exception", "MarshalOnProperty.Shared getter");
		}

		[Test]
		public void Bug34042 ()
		{
			BuildFile (Profile.iOS, "bug34042.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NSCopyingNullability (Profile profile)
		{
			var bgen = BuildFile (profile, "tests/nscopying-nullability.cs");
			bgen.AssertNoWarnings ();
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void EditorBrowsable (Profile profile)
		{
			var bgen = BuildFile (profile, false, true, "tests/editor-browsable.cs");
			var types = bgen.ApiAssembly.MainModule.Types;

			var hasEditorBrowsableAttribute = new Func<ICustomAttributeProvider, bool> ((ICustomAttributeProvider provider) => {
				return provider.CustomAttributes.Any (v => v.AttributeType.Name == "EditorBrowsableAttribute");
			});

			var strongEnumType = types.Single (v => v.Name == "StrongEnum");
			Assert.IsTrue (hasEditorBrowsableAttribute (strongEnumType), "StrongEnumType");
			var objcClassType = types.Single (v => v.Name == "ObjCClass");
			Assert.IsTrue (hasEditorBrowsableAttribute (objcClassType), "ObjCClass");
		}

		static string RenderArgument (CustomAttributeArgument arg)
		{
			var td = arg.Type.Resolve ();
			// If it's an enum value, try to find the enum field name and return that.
			if (td?.BaseType?.Name == "Enum") {
				if (arg.Value is byte b2) {
					var fields = td.Fields
									.Where (f => f.HasConstant && (byte) f.Constant == b2)
									.OrderBy (f => f.Name);
					if (fields.Any ())
						return td.FullName + "." + fields.First ().Name;
				}
			}
			var obj = arg.Value;
			if (obj is null)
				return "null";

			if (obj is string str)
				return "\"" + str + "\"";

			if (obj is byte b)
				return b.ToString ();

			if (obj is int i32)
				return i32.ToString ();

			// Good enough for now, implement more cases as required.
			throw new NotImplementedException (obj.GetType ().FullName);
		}

		static IEnumerable<CustomAttribute> GetAvailabilityAttributes (ICustomAttributeProvider provider)
		{
			if (!provider.HasCustomAttributes)
				yield break;

			foreach (var ca in provider.CustomAttributes) {
				switch (ca.AttributeType.Name) {
				case "SupportedOSPlatformAttribute":
				case "UnsupportedOSPlatformAttribute":
				case "ObsoletedOSPlatformAttribute":
					yield return ca;
					break;
				}
			}
		}

		static string RenderSupportedOSPlatformAttributes (ICustomAttributeProvider provider)
		{
			var attributes = GetAvailabilityAttributes (provider).ToArray ();
			if (attributes is null || attributes.Length == 0)
				return string.Empty;
			var lines = new List<string> ();
			foreach (var ca in attributes)
				lines.Add (RenderSupportedOSPlatformAttribute (ca));
			lines.Sort ();
			return string.Join ("\n", lines).Replace ("\r", string.Empty);
		}

		static string RenderSupportedOSPlatformAttribute (CustomAttribute ca)
		{
			return "[" + ca.AttributeType.Name.Replace ("Attribute", "") + "(" + string.Join (", ", ca.ConstructorArguments.Select (arg => RenderArgument (arg))) + ")]";
		}

		[Test]
		public void Bug35176 ()
		{
			var bgen = BuildFile (Profile.iOS, "bug35176.cs");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMembers = ((IEnumerable<ICustomAttributeProvider>) allTypes)
				.Union (allTypes.SelectMany ((type) => type.Methods))
				.Union (allTypes.SelectMany ((type) => type.Fields))
				.Union (allTypes.SelectMany ((type) => type.Properties));
			const string attrib = "SupportedOSPlatformAttribute";
			var allSupportedAttributes = allMembers.SelectMany (v => v.CustomAttributes.Where (ca => ca.AttributeType.Name == attrib).Select (ca => new Tuple<ICustomAttributeProvider, CustomAttribute> (v, ca)));
			var renderedSupportedAttributes = allSupportedAttributes.Select (v => v.Item1.ToString () + ": " + RenderSupportedOSPlatformAttribute (v.Item2) + "");
			var preserves = allSupportedAttributes.Count ();
			var renderedAttributes = "\t" + string.Join ("\n\t", renderedSupportedAttributes.OrderBy (v => v)) + "\n";
			string expectedAttributes =
@"	Bug35176.IFooInterface: [SupportedOSPlatform(""ios14.3"")]
	Bug35176.IFooInterface: [SupportedOSPlatform(""maccatalyst15.3"")]
	Bug35176.IFooInterface: [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.BarObject::BarView(): [SupportedOSPlatform(""ios14.3"")]
	UIKit.UIView Bug35176.BarObject::BarView(): [SupportedOSPlatform(""maccatalyst15.3"")]
	UIKit.UIView Bug35176.BarObject::BarView(): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.BarObject::FooView(): [SupportedOSPlatform(""ios14.3"")]
	UIKit.UIView Bug35176.BarObject::FooView(): [SupportedOSPlatform(""maccatalyst15.3"")]
	UIKit.UIView Bug35176.BarObject::FooView(): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.BarObject::get_BarView(): [SupportedOSPlatform(""ios14.4"")]
	UIKit.UIView Bug35176.BarObject::get_BarView(): [SupportedOSPlatform(""maccatalyst15.4"")]
	UIKit.UIView Bug35176.BarObject::get_BarView(): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.BarObject::GetBarMember(System.Int32): [SupportedOSPlatform(""ios14.3"")]
	UIKit.UIView Bug35176.BarObject::GetBarMember(System.Int32): [SupportedOSPlatform(""maccatalyst15.3"")]
	UIKit.UIView Bug35176.BarObject::GetBarMember(System.Int32): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.FooInterface_Extensions::GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""ios14.4"")]
	UIKit.UIView Bug35176.FooInterface_Extensions::GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""maccatalyst15.4"")]
	UIKit.UIView Bug35176.FooInterface_Extensions::GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.IFooInterface::_GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""ios14.4"")]
	UIKit.UIView Bug35176.IFooInterface::_GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""maccatalyst15.4"")]
	UIKit.UIView Bug35176.IFooInterface::_GetBarView(Bug35176.IFooInterface): [SupportedOSPlatform(""macos12.2"")]
	UIKit.UIView Bug35176.IFooInterface::get_BarView(): [SupportedOSPlatform(""ios14.4"")]
	UIKit.UIView Bug35176.IFooInterface::get_BarView(): [SupportedOSPlatform(""maccatalyst15.4"")]
	UIKit.UIView Bug35176.IFooInterface::get_BarView(): [SupportedOSPlatform(""macos12.2"")]
";

			expectedAttributes = expectedAttributes.Replace ("\r", string.Empty);
			renderedAttributes = renderedAttributes.Replace ("\r", string.Empty);

			if (renderedAttributes != expectedAttributes) {
				Console.WriteLine ($"Expected:");
				Console.WriteLine (expectedAttributes);
				Console.WriteLine ($"Actual:");
				Console.WriteLine (renderedAttributes);
			}

			Assert.AreEqual (expectedAttributes, renderedAttributes, "Introduced attributes");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void INativeObjectsInBlocks (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.Defines = BGenTool.GetDefaultDefines (bgen.Profile);
			bgen.AddTestApiDefinition ("tests/inativeobjects-in-blocks.cs");
			bgen.AddExtraSourcesRelativeToGeneratorDirectory ("tests/inativeobjects-in-blocks-sources.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();
		}

		[Test]
		public void Bug36457 ()
		{
			BuildFile (Profile.iOS, "bug36457.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void Bug39614 (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("bug39614.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertWarning (1103, "'FooType`1' does not live under a namespace; namespaces are a highly recommended .NET best practice");
		}

		[TestCase (Profile.iOS)]
		public void Bug18035 (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("bug18025.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertWarning (1103, "'FooType' does not live under a namespace; namespaces are a highly recommended .NET best practice");
		}

		[Test]
		public void Bug40282 ()
		{
			BuildFile (Profile.iOS, "bug40282.cs");
		}

		[Test]
		public void Bug42742 ()
		{
			var bgen = BuildFile (Profile.iOS, "bug42742.cs");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMembers = ((IEnumerable<ICustomAttributeProvider>) allTypes)
				.Union (allTypes.SelectMany ((type) => type.Methods))
				.Union (allTypes.SelectMany ((type) => type.Fields))
				.Union (allTypes.SelectMany ((type) => type.Properties));

			var preserves = allMembers.Sum ((v) => v.CustomAttributes.Count ((ca) => ca.AttributeType.Name == "AdviceAttribute"));
			Assert.AreEqual (33, preserves, "Advice attribute count"); // If you modified code that generates AdviceAttributes please update the attribute count
		}

		[Test]
		public void Bug43579 ()
		{
			BuildFile (Profile.iOS, "bug43579.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void Bug46292 (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.ProcessEnums = true;
			bgen.AddTestApiDefinition ("bug46292.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMembers = ((IEnumerable<ICustomAttributeProvider>) allTypes)
				.Union (allTypes.SelectMany ((type) => type.Methods))
				.Union (allTypes.SelectMany ((type) => type.Fields))
				.Union (allTypes.SelectMany ((type) => type.Properties));

			var attribCount = allMembers.Count ((v) => v.HasCustomAttributes && v.CustomAttributes.Any ((ca) => ca.AttributeType.Name == "ObsoleteAttribute"));
			Assert.AreEqual (2, attribCount, "attribute count");
		}

		[Test]
		public void Bug53076 ()
		{
			var bgen = BuildFile (Profile.iOS, "bug53076.cs");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMethods = bgen.ApiAssembly.MainModule.GetTypes ().SelectMany ((type) => type.Methods);

			// Count all *Async methods whose first parameter is 'IMyFooProtocol'.
			var methodCount = allMethods.Count ((v) => v.Name.EndsWith ("Async", StringComparison.Ordinal) && v.Parameters.Count > 0 && v.Parameters [0].ParameterType.Name == "IMyFooProtocol");
			Assert.AreEqual (10, methodCount, "Async method count");
		}

		[Test]
		public void Bug53076WithModel ()
		{
			var bgen = BuildFile (Profile.iOS, "bug53076withmodel.cs");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMethods = bgen.ApiAssembly.MainModule.GetTypes ().SelectMany ((type) => type.Methods);

			// Count all *Async methods whose first parameter is 'IMyFooProtocol'.
			var methodCount = allMethods.Count ((v) => v.Name.EndsWith ("Async", StringComparison.Ordinal) && v.Parameters.Count > 0 && v.Parameters [0].ParameterType.Name == "IMyFooProtocol");
			Assert.AreEqual (10, methodCount, "Async method count");
		}

		[Test]
		public void StackOverflow20696157 ()
		{
			BuildFile (Profile.iOS, "sof20696157.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void TypesInMultipleNamespaces (Profile profile)
		{
			BuildFile (profile, "tests/types-in-multiple-namespaces.cs");
		}

		[Test]
		public void HyphenInName ()
		{
			BuildFile (Profile.iOS, "btouch-with-hyphen-in-name.cs");
		}

		[Test]
		public void PropertyRedefinition ()
		{
			BuildFile (Profile.iOS, "property-redefination-ios.cs");
		}

		[Test]
		public void ArrayFromHandleBug ()
		{
			BuildFile (Profile.iOS, "arrayfromhandlebug.cs");
		}

		[Test]
		public void StrongDictSupportTemplatedDicts ()
		{
			BuildFile (Profile.iOS, "strong-dict-support-templated-dicts.cs");
		}

		[Test]
		public void GenericStrongDictionary ()
		{
			BuildFile (Profile.iOS, "generic-strong-dictionary.cs");
		}

		[Test]
		public void GenericNSObjectParameter ()
		{
			BuildFile (Profile.iOS, "generic-type-nsobject.cs");
		}

		[Test]
		public void BindAsTests ()
		{
			BuildFile (Profile.iOS, "bindastests.cs");
		}

		[Test]
		public void Forum54078 ()
		{
			var bgen = BuildFile (Profile.iOS, "forum54078.cs");

			var api = bgen.ApiAssembly;
			var type = api.MainModule.GetType ("Test", "CustomController");
			foreach (var method in type.Methods)
				Asserts.DoesNotThrowExceptions (method, type.FullName);
		}

		[Test]
		public void Desk63279 ()
		{
			BuildFile (Profile.iOS, "desk63279A.cs", "desk63279B.cs");
		}

		[Test]
		public void Desk79124 ()
		{
			var bgen = BuildFile (Profile.iOS, "desk79124.cs");
			bgen.AssertType ("Desk79124.WYPopoverBackgroundView/WYPopoverBackgroundViewAppearance");
		}

		[Test]
		public void MultipleApiDefinitions1 ()
		{
			BuildFile (Profile.iOS, "multiple-api-definitions1.cs");
		}

		[Test]
		public void MultipleApiDefinitions2 ()
		{
			BuildFile (Profile.iOS, "multiple-api-definitions2-a.cs", "multiple-api-definitions2-b.cs");
		}


		[Test]
		[TestCase (Profile.iOS)]
		public void INativeObjectArraysInBlocks (Profile profile)
		{
			BuildFile (profile, "tests/inativeobject-arrays-in-blocks.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void ClassNameCollision (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.Defines = BGenTool.GetDefaultDefines (bgen.Profile);
			bgen.Sources.Add (Path.Combine (Configuration.SourceRoot, "tests", "generator", "classNameCollision-enum.cs"));
			bgen.ApiDefinitions.Add (Path.Combine (Configuration.SourceRoot, "tests", "generator", "classNameCollision.cs"));
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void VirtualWrap (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("virtualwrap.cs");
			bgen.CreateTemporaryBinding ();
			bgen.ProcessEnums = true;
			bgen.AssertExecute ("build");

			// verify virtual methods
			var attribs = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot;
			bgen.AssertMethod ("WrapTest.MyFooClass", "FromUrl", attribs, null, "Foundation.NSUrl");
			bgen.AssertMethod ("WrapTest.MyFooClass", "FromUrl", attribs, null, "System.String");
			bgen.AssertMethod ("WrapTest.MyFooClass", "get_FooNSString", attribs | MethodAttributes.SpecialName, "Foundation.NSString");
			bgen.AssertMethod ("WrapTest.MyFooClass", "get_FooString", attribs | MethodAttributes.SpecialName, "System.String");

			// verify non-virtual methods
			attribs = MethodAttributes.Public | MethodAttributes.HideBySig;
			bgen.AssertMethod ("WrapTest.MyFooClass", "FromUrlN", attribs, null, "System.String");
			bgen.AssertMethod ("WrapTest.MyFooClass", "get_FooNSStringN", attribs | MethodAttributes.SpecialName, "Foundation.NSString");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NoAsyncInternalWrapper (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("noasyncinternalwrapper.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");

			var allTypes = bgen.ApiAssembly.MainModule.GetTypes ().ToArray ();
			var allMembers = ((IEnumerable<MemberReference>) allTypes)
				.Union (allTypes.SelectMany ((type) => type.Methods))
				.Union (allTypes.SelectMany ((type) => type.Fields))
				.Union (allTypes.SelectMany ((type) => type.Properties));

			Assert.AreEqual (2, allMembers.Count ((member) => member.Name == "RequiredMethodAsync"), "Expected 2 RequiredMethodAsync members in generated code. If you modified code that generates RequiredMethodAsync (AsyncAttribute) please update the RequiredMethodAsync count.");

			var attribs = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig;
			bgen.AssertMethod ("NoAsyncInternalWrapperTests.MyFooDelegate_Extensions", "RequiredMethodAsync", attribs, "System.Threading.Tasks.Task", "NoAsyncInternalWrapperTests.IMyFooDelegate", "System.Int32");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NoAsyncWarningCS0219 (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("noasyncwarningcs0219.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void FieldEnumTests (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.ProcessEnums = true;
			bgen.AddTestApiDefinition ("fieldenumtests.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void SmartEnumWithFramework (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.ProcessEnums = true;
			bgen.AddTestApiDefinition ("smartenumwithframework.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");

			bgen.AssertApiLoadsField ("SmartEnumWithFramework.FooEnumTestExtensions", "get_First", "ObjCRuntime.Libraries/CoreImage", "Handle", "First getter");
			bgen.AssertApiLoadsField ("SmartEnumWithFramework.FooEnumTestExtensions", "get_Second", "ObjCRuntime.Libraries/CoreImage", "Handle", "Second getter");
		}

		[Test]
		public void ForcedType ()
		{
			var bgen = BuildFile (Profile.iOS, false, "forcedtype.cs");

			var allMethods = bgen.ApiAssembly.MainModule.GetTypes ().SelectMany ((type) => type.Methods);

			// Count the number of calls to GetINativeObject
			var getINativeObjectCalls = allMethods.Sum ((method) => {
				if (!method.HasBody)
					return 0;
				return method.Body.Instructions.Count ((ins) => {
					if (ins.OpCode.FlowControl != FlowControl.Call)
						return false;
					var mr = (MethodReference) ins.Operand;
					return mr.Name == "GetINativeObject";
				});
			});

			Assert.AreEqual (12, getINativeObjectCalls, "Preserve attribute count"); // If you modified code that generates PreserveAttributes please update the preserve count
		}

		[Test]
		public void IsDirectBinding ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/is-direct-binding.cs");

			var callsMethod = new Func<MethodDefinition, string, bool> ((method, name) => {
				return method.Body.Instructions.Any ((ins) => {
					switch (ins.OpCode.Code) {
					case Code.Call:
					case Code.Calli:
					case Code.Callvirt:
						var mr = ins.Operand as MethodReference;
						return mr.Name == name;
					default:
						return false;
					}
				});
			});

			// The normal constructor should get the IsDirectBinding value, and call both objc_msgSend and objc_msgSendSuper
			var cConstructor = bgen.ApiAssembly.MainModule.GetType ("NS", "C").Methods.First ((v) => v.IsConstructor && !v.HasParameters && !v.IsStatic);
			Assert.That (callsMethod (cConstructor, "set_IsDirectBinding"), "C: set_IsDirectBinding");
			Assert.That (callsMethod (cConstructor, "get_IsDirectBinding"), "C: get_IsDirectBinding");
			Assert.That (callsMethod (cConstructor, "IntPtr_objc_msgSend"), "C: objc_msgSend");
			Assert.That (callsMethod (cConstructor, "IntPtr_objc_msgSendSuper"), "C: objc_msgSendSuper");

			// The constructor for a model should not get the IsDirectBinding value, because it's always 'false'. Neither should it call objc_msgSend, only objc_msgSendSuper
			var pConstructor = bgen.ApiAssembly.MainModule.GetType ("NS", "P").Methods.First ((v) => v.IsConstructor && !v.HasParameters && !v.IsStatic);
			Assert.That (callsMethod (pConstructor, "set_IsDirectBinding"), "P: set_IsDirectBinding");
			Assert.That (!callsMethod (pConstructor, "get_IsDirectBinding"), "P: get_IsDirectBinding");
			Assert.That (!callsMethod (pConstructor, "IntPtr_objc_msgSend"), "P: objc_msgSend");
			Assert.That (callsMethod (pConstructor, "IntPtr_objc_msgSendSuper"), "P: objc_msgSendSuper");

			// The constructor for a sealed class should not get the IsDirectBinding value, because it's always true. Neither should it call objc_msgSendSuper, only objc_msgSend.
			var sConstructor = bgen.ApiAssembly.MainModule.GetType ("NS", "S").Methods.First ((v) => v.IsConstructor && !v.HasParameters && !v.IsStatic);
			Assert.That (callsMethod (sConstructor, "set_IsDirectBinding"), "S: set_IsDirectBinding");
			Assert.That (!callsMethod (sConstructor, "get_IsDirectBinding"), "S: get_IsDirectBinding");
			Assert.That (callsMethod (sConstructor, "IntPtr_objc_msgSend"), "S: objc_msgSend");
			Assert.That (!callsMethod (sConstructor, "IntPtr_objc_msgSendSuper"), "S: objc_msgSendSuper");
		}

		[Test]
		public void Bug57531 () => BuildFile (Profile.iOS, "bug57531.cs");

		[Test]
		public void Bug57870 () => BuildFile (Profile.iOS, true, true, "bug57870.cs");

		[Test]
		public void GHIssue3869 () => BuildFile (Profile.iOS, "ghissue3869.cs");

		[Test]
		[TestCase ("issue3875.cs", "api0__Issue3875_AProtocol")]
		[TestCase ("issue3875B.cs", "BProtocol")]
		[TestCase ("issue3875C.cs", "api0__Issue3875_AProtocol")]
		public void Issue3875 (string file, string modelName)
		{
			var bgen = BuildFile (Profile.iOS, file);
			var attrib = bgen.ApiAssembly.MainModule.GetType ("Issue3875", "AProtocol").CustomAttributes.Where ((v) => v.AttributeType.Name == "RegisterAttribute").First ();
			Assert.AreEqual (modelName, attrib.ConstructorArguments [0].Value, "Custom ObjC name");
		}

		[Test]
		public void GHIssue5444 () => BuildFile (Profile.iOS, "ghissue5444.cs");

		[Test]
		[TestCase (Profile.iOS)]
		public void GH5416_method (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("ghissue5416b.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertWarning (1118, "[NullAllowed] should not be used on methods, like 'Foundation.NSString Method(Foundation.NSDate, Foundation.NSObject)', but only on properties, parameters and return values.");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void GH5416_setter (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("ghissue5416a.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
			bgen.AssertWarning (1118, "[NullAllowed] should not be used on methods, like 'System.Void set_Setter(Foundation.NSString)', but only on properties, parameters and return values.");
		}

		[Test]
		public void GHIssue5692 () => BuildFile (Profile.iOS, "ghissue5692.cs");

		[Test]
		public void GHIssue7304 () => BuildFile (Profile.macOSMobile, "ghissue7304.cs");

		[Test]
		public void RefOutParameters ()
		{
			BuildFile (Profile.macOSMobile, true, "tests/ref-out-parameters.cs");
		}

		[Test]
		public void ReturnRelease ()
		{
			BuildFile (Profile.iOS, "tests/return-release.cs");
		}

		[Test]
		public void GHIssue6626 () => BuildFile (Profile.iOS, "ghissue6626.cs");

		[Test]
		public void StrongDictsNativeEnums () => BuildFile (Profile.iOS, "strong-dict-native-enum.cs");

		[Test]
		public void IgnoreUnavailableProtocol ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/ignore-unavailable-protocol.cs");
			var myClass = bgen.ApiAssembly.MainModule.GetType ("NS", "MyClass");
			var myProtocol = bgen.ApiAssembly.MainModule.GetType ("NS", "IMyProtocol");
			var myClassInterfaces = myClass.Interfaces.Select (v => v.InterfaceType.Name).ToArray ();
			Assert.That (myClassInterfaces, Does.Not.Contain ("IMyProtocol"), "IMyProtocol");
			Assert.IsNull (myProtocol, "MyProtocol null");
		}

		[Test]
		public void VSTS970507 ()
		{
			BuildFile (Profile.iOS, "tests/vsts-970507.cs");
		}

		[Test]
		public void DiamondProtocol ()
		{
			BuildFile (Profile.iOS, "tests/diamond-protocol.cs");
		}

		[Test]
		public void GHIssue9065_Sealed () => BuildFile (Profile.iOS, nowarnings: true, "ghissue9065.cs");

		[Test]
		public void GHIssue18645_DuplicatedFiled () => BuildFile (Profile.iOS, nowarnings: true, "ghissue18645.cs");

		// looking for [BindingImpl (BindingImplOptions.Optimizable)]
		bool IsOptimizable (MethodDefinition method)
		{
			const int Optimizable = 0x2; // BindingImplOptions flag

			if (!method.HasCustomAttributes)
				return false;

			foreach (var ca in method.CustomAttributes) {
				if (ca.AttributeType.Name != "BindingImplAttribute")
					continue;
				foreach (var a in ca.ConstructorArguments)
					return (((int) a.Value & Optimizable) == Optimizable);
			}
			return false;
		}

		[Test]
		public void DisposeAttributeOptimizable ()
		{
			var profile = Profile.iOS;
			var bgen = BuildFile (profile, "tests/dispose-attribute.cs");

			// processing custom attributes (like its properties) will call Resolve so we must be able to find the platform assembly to run this test
			var resolver = bgen.ApiAssembly.MainModule.AssemblyResolver as BaseAssemblyResolver;
			resolver.AddSearchDirectory (Configuration.GetRefDirectory (profile.AsPlatform ()));

			// [Dispose] is, by default, not optimizable
			var with_dispose = bgen.ApiAssembly.MainModule.GetType ("NS", "WithDispose").Methods.First ((v) => v.Name == "Dispose");
			Assert.NotNull (with_dispose, "WithDispose");
			Assert.That (IsOptimizable (with_dispose), Is.False, "WithDispose/Optimizable");

			// [Dispose] can opt-in being optimizable
			var with_dispose_optin = bgen.ApiAssembly.MainModule.GetType ("NS", "WithDisposeOptInOptimizable").Methods.First ((v) => v.Name == "Dispose");
			Assert.NotNull (with_dispose_optin, "WithDisposeOptInOptimizable");
			Assert.That (IsOptimizable (with_dispose_optin), Is.True, "WithDisposeOptInOptimizable/Optimizable");

			// Without a [Dispose] attribute the generated method is optimizable
			var without_dispose = bgen.ApiAssembly.MainModule.GetType ("NS", "WithoutDispose").Methods.First ((v) => v.Name == "Dispose");
			Assert.NotNull (without_dispose, "WitoutDispose");
			Assert.That (IsOptimizable (without_dispose), Is.True, "WitoutDispose/Optimizable");
		}

		[Test]
		public void SnippetAttributesOptimizable ()
		{
			var profile = Profile.iOS;
			var bgen = BuildFile (profile, "tests/snippet-attributes.cs");

			// processing custom attributes (like its properties) will call Resolve so we must be able to find the platform assembly to run this test
			var resolver = bgen.ApiAssembly.MainModule.AssemblyResolver as BaseAssemblyResolver;
			resolver.AddSearchDirectory (Configuration.GetRefDirectory (profile.AsPlatform ()));

			// [SnippetAttribute] subclasses are, by default, not optimizable
			var not_opt = bgen.ApiAssembly.MainModule.GetType ("NS", "NotOptimizable");
			Assert.NotNull (not_opt, "NotOptimizable");
			var pre_not_opt = not_opt.Methods.First ((v) => v.Name == "Pre");
			Assert.That (IsOptimizable (pre_not_opt), Is.False, "NotOptimizable/Pre");
			var prologue_not_opt = not_opt.Methods.First ((v) => v.Name == "Prologue");
			Assert.That (IsOptimizable (prologue_not_opt), Is.False, "NotOptimizable/Prologue");
			var post_not_opt = not_opt.Methods.First ((v) => v.Name == "Post");
			Assert.That (IsOptimizable (post_not_opt), Is.False, "NotOptimizable/Post");

			// [SnippetAttribute] subclasses can opt-in being optimizable
			var optin_opt = bgen.ApiAssembly.MainModule.GetType ("NS", "OptInOptimizable");
			Assert.NotNull (optin_opt, "OptInOptimizable");
			var pre_optin_opt = optin_opt.Methods.First ((v) => v.Name == "Pre");
			Assert.That (IsOptimizable (pre_optin_opt), Is.True, "OptInOptimizable/Pre");
			var prologue_optin_opt = optin_opt.Methods.First ((v) => v.Name == "Prologue");
			Assert.That (IsOptimizable (prologue_optin_opt), Is.True, "OptInOptimizable/Prologue");
			var post_optin_opt = optin_opt.Methods.First ((v) => v.Name == "Post");
			Assert.That (IsOptimizable (post_optin_opt), Is.True, "OptInOptimizable/Post");

			// Without a [SnippetAttribute] subclass attribute the generated method is optimizable
			var nothing = bgen.ApiAssembly.MainModule.GetType ("NS", "NoSnippet").Methods.First ((v) => v.Name == "Nothing");
			Assert.NotNull (nothing, "NoSnippet");
			Assert.That (IsOptimizable (nothing), Is.True, "Nothing/Optimizable");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NativeEnum (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.ProcessEnums = true;
			bgen.Defines = BGenTool.GetDefaultDefines (bgen.Profile);
			bgen.Sources = new string [] { Path.Combine (Configuration.SourceRoot, "tests", "generator", "tests", "nativeenum-extensions.cs") }.ToList ();
			bgen.ApiDefinitions = new string [] { Path.Combine (Configuration.SourceRoot, "tests", "generator", "tests", "nativeenum.cs") }.ToList ();
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
		}

		[Test]
		public void DelegateWithINativeObjectReturnType ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/delegate-with-inativeobject-return-type.cs");
			bgen.AssertExecute ("build");

			// Assert that the return type from the delegate is IntPtr
			var type = bgen.ApiAssembly.MainModule.GetType ("ObjCRuntime", "Trampolines").NestedTypes.First (v => v.Name == "DMyHandler");
			Assert.NotNull (type, "DMyHandler");
			var method = type.Methods.First (v => v.Name == "Invoke");
			Assert.AreEqual ("ObjCRuntime.NativeHandle", method.ReturnType.FullName, "Return type");
		}

		[Test]
		public void ProtocolBindProperty ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/protocol-bind-property.cs");
			bgen.AssertExecute ("build");

			// Assert that the return type from the delegate is IntPtr
			var type = bgen.ApiAssembly.MainModule.GetType ("NS", "MyProtocol_Extensions");
			Assert.NotNull (type, "MyProtocol_Extensions");

			var method = type.Methods.First (v => v.Name == "GetOptionalProperty");
			var ldstr = method.Body.Instructions.Single (v => v.OpCode == OpCodes.Ldstr);
			Assert.AreEqual ("isOptionalProperty", (string) ldstr.Operand, "isOptionalProperty");


			method = type.Methods.First (v => v.Name == "SetOptionalProperty");
			ldstr = method.Body.Instructions.Single (v => v.OpCode == OpCodes.Ldstr);
			Assert.AreEqual ("setOptionalProperty:", (string) ldstr.Operand, "setOptionalProperty");

			type = bgen.ApiAssembly.MainModule.GetType ("NS", "MyProtocolWrapper");
			Assert.NotNull (type, "MyProtocolWrapper");

			method = type.Methods.First (v => v.Name == "get_AbstractProperty");
			ldstr = method.Body.Instructions.Single (v => v.OpCode == OpCodes.Ldstr);
			Assert.AreEqual ("isAbstractProperty", (string) ldstr.Operand, "isAbstractProperty");

			method = type.Methods.First (v => v.Name == "set_AbstractProperty");
			ldstr = method.Body.Instructions.Single (v => v.OpCode == OpCodes.Ldstr);
			Assert.AreEqual ("setAbstractProperty:", (string) ldstr.Operand, "setAbstractProperty");
		}

		[Test]
		public void AbstractTypeTest ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/abstract-type.cs");
			bgen.AssertExecute ("build");

			// Assert that the return type from the delegate is IntPtr
			var type = bgen.ApiAssembly.MainModule.GetType ("NS", "MyObject");
			Assert.NotNull (type, "MyObject");
			Assert.IsFalse (type.IsAbstract, "IsAbstract");

			var method = type.Methods.First (v => v.Name == ".ctor" && !v.HasParameters && !v.IsStatic);
			Assert.IsTrue (method.IsFamily, "IsProtected ctor");

			method = type.Methods.First (v => v.Name == "AbstractMember" && !v.HasParameters && !v.IsStatic);
			var throwInstruction = method.Body?.Instructions?.FirstOrDefault (v => v.OpCode == OpCodes.Throw);
			Assert.IsTrue (method.IsPublic, "IsPublic ctor");
			Assert.IsTrue (method.IsVirtual, "IsVirtual");
			Assert.IsFalse (method.IsAbstract, "IsAbstract");
			Assert.IsNotNull (throwInstruction, "Throw");
		}

		[Test]
		[Ignore ("https://github.com/dotnet/roslyn/issues/61525")]
		public void NativeIntDelegates ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/nint-delegates.cs");

			Func<string, bool> verifyDelegate = (typename) => {
				// Assert that the return type from the delegate is IntPtr
				var type = bgen.ApiAssembly.MainModule.GetType ("NS", typename);
				Assert.NotNull (type, typename);
				var method = type.Methods.First (m => m.Name == "Invoke");
				Assert.IsNotNull (method.MethodReturnType.CustomAttributes.FirstOrDefault (attr => attr.AttributeType.Name == "NativeIntegerAttribute"), "Return type for delegate " + typename);
				foreach (var p in method.Parameters) {
					Assert.IsNotNull (p.CustomAttributes.FirstOrDefault (attr => attr.AttributeType.Name == "NativeIntegerAttribute"), $"Parameter {p.Name}'s type for delegate " + typename);
				}

				return false;
			};

			verifyDelegate ("D1");
			verifyDelegate ("D2");
			verifyDelegate ("D3");
			verifyDelegate ("NSTableViewColumnRowPredicate");
		}

		[Test]
		public void CSharp10Syntax ()
		{
			BuildFile (Profile.iOS, "tests/csharp10syntax.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void AttributesFromInlinedProtocols (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());

			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("tests/attributes-from-inlined-protocols.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");

			var type = bgen.ApiAssembly.MainModule.GetType ("NS", "TypeA");

			var expectedAttributes = new string [] {
@"[BindingImpl(3)]
[Export(""someMethod1:"")]
[SupportedOSPlatform(""ios13.0"")]
[SupportedOSPlatform(""maccatalyst"")]
[UnsupportedOSPlatform(""tvos"")]",

@"[BindingImpl(3)]
[Export(""someMethod2:"")]
[SupportedOSPlatform(""ios13.0"")]
[SupportedOSPlatform(""maccatalyst"")]
[UnsupportedOSPlatform(""tvos"")]",

@"[BindingImpl(3)]
[Export(""someMethod3:"")]
[SupportedOSPlatform(""ios"")]
[SupportedOSPlatform(""maccatalyst"")]
[UnsupportedOSPlatform(""tvos"")]",

@"[BindingImpl(3)]
[Export(""someMethod4:"")]
[SupportedOSPlatform(""ios"")]
[SupportedOSPlatform(""maccatalyst"")]
[UnsupportedOSPlatform(""tvos"")]",
			};

			int someMethodCount = expectedAttributes.Length;
			var someMethod = new MethodDefinition [someMethodCount];
			var renderedSomeMethod = new string [someMethodCount];
			var failures = new List<string> ();
			for (var i = 0; i < someMethodCount; i++) {
				someMethod [i] = type.Methods.Single (v => v.Name == "SomeMethod" + (i + 1).ToString ());
				renderedSomeMethod [i] = string.Join ("\n", someMethod [i].CustomAttributes.Select (ca => RenderSupportedOSPlatformAttribute (ca)).OrderBy (v => v));

				expectedAttributes [i] = expectedAttributes [i].Replace ("\r", string.Empty);
				renderedSomeMethod [i] = renderedSomeMethod [i].Replace ("\r", string.Empty);

				if (expectedAttributes [i] == renderedSomeMethod [i])
					continue;

				var msg =
					$"{someMethod [i].Name} has different attributes.\n" +
					$"Expected attributes:\n" +
					expectedAttributes [i] + "\n" +
					"Actual attributes:\n" +
					renderedSomeMethod [i];
				Console.WriteLine ($"❌ {msg}\n");
				failures.Add (msg);
			}

			Assert.That (failures, Is.Empty, "Failures");
		}

		[Test]
		public void NFloatType ()
		{
			var bgen = BuildFile (Profile.iOS, "tests/nfloat.cs");

			var messaging = bgen.ApiAssembly.MainModule.Types.FirstOrDefault (v => v.Name == "Messaging");
			Assert.IsNotNull (messaging, "Messaging");
			var pinvoke = messaging.Methods.FirstOrDefault (v => v.Name == "xamarin_nfloat_objc_msgSend_exception");
			Assert.IsNotNull (pinvoke, "PInvoke");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NoAvailabilityForAccessors (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("tests/no-availability-for-accessors.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");

			bgen.AssertMethod ("NS.Whatever", "get_PropA");
			bgen.AssertNoMethod ("NS.Whatever", "set_PropA", parameterTypes: "Foundation.NSObject");
			bgen.AssertMethod ("NS.Whatever", "set_PropB", parameterTypes: "Foundation.NSObject");
			bgen.AssertNoMethod ("NS.Whatever", "get_PropB");
			bgen.AssertMethod ("NS.Whatever", "get_IPropA");
			bgen.AssertNoMethod ("NS.Whatever", "set_IPropA", parameterTypes: "Foundation.NSObject");
			bgen.AssertMethod ("NS.Whatever", "set_IPropB", parameterTypes: "Foundation.NSObject");
			bgen.AssertNoMethod ("NS.Whatever", "get_IPropB");
			bgen.AssertMethod ("NS.Whatever", "get_IPropAOpt");
			bgen.AssertNoMethod ("NS.Whatever", "set_IPropAOpt", parameterTypes: "Foundation.NSObject");
			bgen.AssertMethod ("NS.Whatever", "set_IPropBOpt", parameterTypes: "Foundation.NSObject");
			bgen.AssertNoMethod ("NS.Whatever", "get_IPropBOpt");
			bgen.AssertMethod ("NS.Whatever", ".ctor");
			bgen.AssertMethod ("NS.Whatever", ".ctor", parameterTypes: "Foundation.NSObjectFlag");
			bgen.AssertMethod ("NS.Whatever", ".ctor", parameterTypes: "ObjCRuntime.NativeHandle");
			bgen.AssertPublicMethodCount ("NS.Whatever", 10); // 6 accessors + 3 constructors + ClassHandle getter

			bgen.AssertMethod ("NS.IIProtocol", "get_IPropA");
			bgen.AssertMethod ("NS.IIProtocol", "get_IPropAOpt");
			bgen.AssertNoMethod ("NS.IIProtocol", "set_IPropA", parameterTypes: "Foundation.NSObject");
			bgen.AssertMethod ("NS.IIProtocol", "set_IPropB", parameterTypes: "Foundation.NSObject");
			bgen.AssertMethod ("NS.IIProtocol", "set_IPropBOpt", parameterTypes: "Foundation.NSObject");
			bgen.AssertNoMethod ("NS.IIProtocol", "get_IPropB");
			bgen.AssertPublicMethodCount ("NS.IIProtocol", 4);

			bgen.AssertMethod ("NS.IProtocol_Extensions", "GetIPropAOpt", parameterTypes: "NS.IIProtocol");
			bgen.AssertMethod ("NS.IProtocol_Extensions", "SetIPropBOpt", parameterTypes: new string [] { "NS.IIProtocol", "Foundation.NSObject" });
			bgen.AssertPublicMethodCount ("NS.IProtocol_Extensions", 2);
		}

		[Test]
		public void GeneratedAttributeOnPropertyAccessors ()
		{
			var bgen = BuildFile (Profile.MacCatalyst, "tests/generated-attribute-on-property-accessors.cs");

			var messaging = bgen.ApiAssembly.MainModule.Types.First (v => v.Name == "ISomething");
			var property = messaging.Properties.First (v => v.Name == "IsLoadedInProcess");
			var getter = messaging.Methods.First (v => v.Name == "get_IsLoadedInProcess");
			var expectedPropertyAttributes =
@"[SupportedOSPlatform(""maccatalyst"")]
[SupportedOSPlatform(""macos"")]
[UnsupportedOSPlatform(""ios"")]
[UnsupportedOSPlatform(""tvos"")]";
			expectedPropertyAttributes = expectedPropertyAttributes.Replace ("\r", string.Empty);

			Assert.AreEqual (expectedPropertyAttributes, RenderSupportedOSPlatformAttributes (property), "Property attributes");
			Assert.AreEqual (string.Empty, RenderSupportedOSPlatformAttributes (getter), "Getter Attributes");
		}

		[Test]
		public void GeneratedAttributeOnPropertyAccessors2 ()
		{
			var bgen = BuildFile (Profile.MacCatalyst, "tests/generated-attribute-on-property-accessors2.cs");

			var messaging = bgen.ApiAssembly.MainModule.Types.First (v => v.Name == "ISomething");
			var property = messaging.Properties.First (v => v.Name == "MicrophoneEnabled");
			var getter = messaging.Methods.First (v => v.Name == "get_MicrophoneEnabled");
			var setter = messaging.Methods.First (v => v.Name == "set_MicrophoneEnabled");

			var expectedPropertyAttributes =
@"[SupportedOSPlatform(""ios"")]
[SupportedOSPlatform(""maccatalyst"")]
[SupportedOSPlatform(""macos13.0"")]
[UnsupportedOSPlatform(""tvos"")]";
			var expectedSetterAttributes =
@"[SupportedOSPlatform(""ios"")]
[SupportedOSPlatform(""maccatalyst"")]
[SupportedOSPlatform(""macos13.0"")]
[UnsupportedOSPlatform(""tvos"")]";

			expectedPropertyAttributes = expectedPropertyAttributes.Replace ("\r", string.Empty);
			expectedSetterAttributes = expectedSetterAttributes.Replace ("\r", string.Empty);

			Assert.AreEqual (expectedPropertyAttributes, RenderSupportedOSPlatformAttributes (property), "Property attributes");
			Assert.AreEqual (string.Empty, RenderSupportedOSPlatformAttributes (getter), "Getter Attributes");
			Assert.AreEqual (expectedSetterAttributes, RenderSupportedOSPlatformAttributes (setter), "Setter Attributes");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void NewerAvailabilityInInlinedProtocol (Profile profile)
		{
			var bgen = BuildFile (profile, "tests/newer-availability-in-inlined-protocol.cs");

			var expectedMethods = new [] {
				new {
					Type = "Whatever",
					MethodCount = 21,
					Methods = new [] {
						new { Method = "get_IPropA", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Method = "get_IPropAOpt", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Method = "set_IPropB", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "set_IPropBOpt", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "get_IPropC", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Method = "set_IPropC", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Method = "get_IPropCOpt", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Method = "set_IPropCOpt", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },

						new { Method = "get_IPropD", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "get_IPropDOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "set_IPropE", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "set_IPropEOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "get_IPropF", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "set_IPropF", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "get_IPropFOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Method = "set_IPropFOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
					},
				},
				new {
					Type = "IIProtocol",
					MethodCount = 17,
					Methods = new [] {
						new { Method = ".cctor", Attributes = "" },
						new { Method = "get_IPropA", Attributes = "" },
						new { Method = "_GetIPropA", Attributes = "" },
						new { Method = "set_IPropB", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "_SetIPropB", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "get_IPropC", Attributes = "" },
						new { Method = "_GetIPropC", Attributes = "" },
						new { Method = "set_IPropC", Attributes = "" },
						new { Method = "_SetIPropC", Attributes = "" },
						new { Method = "get_IPropAOpt", Attributes = "" },
						new { Method = "_GetIPropAOpt", Attributes = "" },
						new { Method = "set_IPropBOpt", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "_SetIPropBOpt", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "get_IPropCOpt", Attributes = "" },
						new { Method = "get_IPropCOpt", Attributes = "" },
						new { Method = "set_IPropCOpt", Attributes = "" },
						new { Method = "_SetIPropCOpt", Attributes = "" },
					},
				},
				new {
					Type = "IProtocol_Extensions",
					MethodCount = 4,
					Methods = new [] {
						new { Method = "GetIPropAOpt", Attributes = "" },
						new { Method = "SetIPropBOpt", Attributes = "[SupportedOSPlatform(\"tvos150.0\")]" },
						new { Method = "GetIPropCOpt", Attributes = "" },
						new { Method = "SetIPropCOpt", Attributes = "" },
					},
				},
				new {
					Type = "IIProtocolLower",
					MethodCount = 17,
					Methods = new [] {
						new { Method = ".cctor", Attributes = "" },
						new { Method = "get_IPropD", Attributes = "" },
						new { Method = "_GetIPropD", Attributes = "" },
						new { Method = "set_IPropE", Attributes = "[SupportedOSPlatform(\"tvos110.0\")]" },
						new { Method = "_SetIPropE", Attributes = "[SupportedOSPlatform(\"tvos110.0\")]" },
						new { Method = "get_IPropF", Attributes = "" },
						new { Method = "_GetIPropF", Attributes = "" },
						new { Method = "set_IPropF", Attributes = "" },
						new { Method = "_SetIPropF", Attributes = "" },
						new { Method = "get_IPropDOpt", Attributes = "" },
						new { Method = "_GetIPropDOpt", Attributes = "" },
						new { Method = "set_IPropEOpt", Attributes = "[SupportedOSPlatform(\"tvos110.0\")]" },
						new { Method = "_SetIPropEOpt", Attributes = "[SupportedOSPlatform(\"tvos110.0\")]" },
						new { Method = "get_IPropFOpt", Attributes = "" },
						new { Method = "_GetIPropFOpt", Attributes = "" },
						new { Method = "set_IPropFOpt", Attributes = "" },
						new { Method = "_SetIPropFOpt", Attributes = "" },
					},
				},
				new {
					Type = "IProtocolLower_Extensions",
					MethodCount = 4,
					Methods = new [] {
						new { Method = "GetIPropDOpt", Attributes = "" },
						new { Method = "SetIPropEOpt", Attributes = "[SupportedOSPlatform(\"tvos110.0\")]" },
						new { Method = "GetIPropFOpt", Attributes = "" },
						new { Method = "SetIPropFOpt", Attributes = "" },
					},
				},
			};

			var expectedProperties = new []  {
				new {
					Type = "Whatever",
					PropertyCount = 13,
					Properties = new [] {
						new { Property = "IPropA", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Property = "IPropB", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Property = "IPropAOpt", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Property = "IPropBOpt", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Property = "IPropC", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Property = "IPropCOpt", Attributes = "[SupportedOSPlatform(\"tvos130.0\")]" },
						new { Property = "IPropD", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Property = "IPropE", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Property = "IPropDOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Property = "IPropEOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Property = "IPropF", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
						new { Property = "IPropFOpt", Attributes = "[SupportedOSPlatform(\"tvos120.0\")]" },
					},
				},
				new {
					Type = "IIProtocol",
					PropertyCount = 6,
					Properties = new [] {
						new { Property = "IPropA", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Property = "IPropB", Attributes = "" },
						new { Property = "IPropC", Attributes = "" },
						new { Property = "IPropAOpt", Attributes = "[SupportedOSPlatform(\"tvos140.0\")]" },
						new { Property = "IPropBOpt", Attributes = "" },
						new { Property = "IPropCOpt", Attributes = "" },
					},
				},
				new {
					Type = "IProtocol_Extensions",
					PropertyCount = 0,
					Properties = new [] {
						new { Property = "fake property for c# anonymous type compilation", Attributes = "..." },
					},
				},
				new {
					Type = "IIProtocolLower",
					PropertyCount = 6,
					Properties = new [] {
						new { Property = "IPropD", Attributes = "[SupportedOSPlatform(\"tvos100.0\")]" },
						new { Property = "IPropE", Attributes = "" },
						new { Property = "IPropF", Attributes = "" },
						new { Property = "IPropDOpt", Attributes = "[SupportedOSPlatform(\"tvos100.0\")]" },
						new { Property = "IPropEOpt", Attributes = "" },
						new { Property = "IPropFOpt", Attributes = "" },
					},
				},
				new {
					Type = "IProtocolLower_Extensions",
					PropertyCount = 0,
					Properties = new [] {
						new { Property = "fake property for c# anonymous type compilation", Attributes = "..." },
					},
				},
			};

			var failures = new List<string> ();

			Assert.Multiple (() => {

				foreach (var expected in expectedMethods) {
					var type = bgen.ApiAssembly.MainModule.Types.FirstOrDefault (v => v.Name == expected.Type);
					Assert.IsNotNull (type, $"Type not found: {expected.Type}");
					if (type is null)
						continue;
					Assert.AreEqual (expected.MethodCount, type.Methods.Count, $"Unexpected method count for {expected.Type}.\n\tActual methods:\n\t\t{string.Join ("\n\t\t", type.Methods.Select (v => v.FullName))}");
					if (expected.MethodCount == 0)
						continue;
					foreach (var expectedMember in expected.Methods) {
						var member = type.Methods.SingleOrDefault (v => v.Name == expectedMember.Method);
						Assert.IsNotNull (member, $"Method not found: {expectedMember.Method} in {type.FullName}");
						var renderedAttributes = RenderSupportedOSPlatformAttributes (member);
						var expectedAttributes = expectedMember.Attributes.Replace ("\r", string.Empty);
						if (renderedAttributes != expectedAttributes) {
							var msg =
								$"Property: {type.FullName}::{member.Name}\n" +
								$"\tExpected attributes:\n" +
								$"\t\t{string.Join ("\n\t\t", expectedMember.Attributes.Split ('\n'))}\n" +
								$"\tActual attributes:\n" +
								$"\t\t{string.Join ("\n\t\t", renderedAttributes.Split ('\n'))}";
							failures.Add (msg);
							Console.WriteLine ($"❌ {msg}");
						}
					}
				}

				foreach (var expected in expectedProperties) {
					var type = bgen.ApiAssembly.MainModule.Types.FirstOrDefault (v => v.Name == expected.Type);
					Assert.IsNotNull (type, $"Type not found: {expected.Type}");
					if (type is null)
						continue;
					Assert.AreEqual (expected.PropertyCount, type.Properties.Count, $"Unexpected property count for {expected.Type}.\n\tActual properties:\n\t\t{string.Join ("\n\t\t", type.Properties.Select (v => v.Name))}");
					if (expected.PropertyCount == 0)
						continue;
					foreach (var expectedMember in expected.Properties) {
						var member = type.Properties.SingleOrDefault (v => v.Name == expectedMember.Property);
						Assert.IsNotNull (member, $"Property not found: {expectedMember.Property} in {type.FullName}");
						if (member is null)
							continue;
						var renderedAttributes = RenderSupportedOSPlatformAttributes (member);
						var expectedAttributes = expectedMember.Attributes.Replace ("\r", string.Empty);
						if (renderedAttributes != expectedAttributes) {
							var msg =
								$"Property: {type.FullName}::{member.Name}\n" +
								$"\tExpected attributes:\n" +
								$"\t\t{string.Join ("\n\t\t", expectedMember.Attributes.Split ('\n'))}\n" +
								$"\tActual attributes:\n" +
								$"\t\t{string.Join ("\n\t\t", renderedAttributes.Split ('\n'))}";
							failures.Add (msg);
							Console.WriteLine ($"❌ {msg}");
						}
					}
				}
			});

			Assert.That (failures, Is.Empty, "Failures");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void ErrorDomain (Profile profile)
		{
			BuildFile (profile, true, true, "tests/errordomain.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void ObsoletedOSPlatform (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.AddTestApiDefinition ("tests/obsoletedosplatform.cs");
			bgen.CreateTemporaryBinding ();
			bgen.AssertExecute ("build");
		}

		[Test]
		public void InternalDelegate ()
		{
			BuildFile (Profile.iOS, "tests/internal-delegate.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		[TestCase (Profile.MacCatalyst)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.tvOS)]
		public void XmlDocs (Profile profile)
		{
			var bgen = BuildFile (profile, false, true, "tests/xmldocs.cs");
			Assert.That (bgen.XmlDocumentation, Does.Exist);
			var contents = File.ReadAllText (bgen.XmlDocumentation);
			var expectedContentsPath = Path.Combine (Configuration.SourceRoot, "tests", "generator", $"ExpectedXmlDocs.{profile.AsPlatform ().AsString ()}.xml");
			if (!File.Exists (expectedContentsPath))
				File.WriteAllText (expectedContentsPath, string.Empty);

			var expectedContents = File.ReadAllText (expectedContentsPath);

			// Fix up a few potential whitespace differences we don't care about.
			contents = contents.Trim ().Replace ("\r", "");
			expectedContents = expectedContents.Trim ().Replace ("\r", "");

			if (contents != expectedContents) {
				if (!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("WRITE_KNOWN_FAILURES"))) {
					File.WriteAllText (expectedContentsPath, contents);
					Assert.AreEqual (expectedContents, contents, $"Xml docs: The known failures have been updated in {expectedContentsPath}, so please commit the results. Re-running the test should now succeed.");
				} else {
					Assert.AreEqual (expectedContents, contents, $"Xml docs: If this is expected, set the WRITE_KNOWN_FAILURES=1 environment variable, run the test again, and commit the changes to the {expectedContentsPath} file.");
				}
			}
		}

		[Test]
		[TestCase (Profile.iOS)]
		[TestCase (Profile.MacCatalyst)]
		[TestCase (Profile.macOSMobile)]
		[TestCase (Profile.tvOS)]
		public void PreviewAPIs (Profile profile)
		{
			var bgen = BuildFile (profile, false, true, "tests/preview.cs");

			// Each Experimental attribute in the api definition has its own diagnostic ID (with an incremental number)
			// Here we collect all diagnostic IDS for all the Experimental attributes in the compiled assembly,
			// and assert that they're all present at least once.
			var module = bgen.ApiAssembly.MainModule;
			var allExperimentalAttributes = module.GetCustomAttributes ().Where (v => v.AttributeType.Name == "ExperimentalAttribute");
			var allExperimentalDiagnosticIds = allExperimentalAttributes.Select (v => (string) v.ConstructorArguments [0].Value).ToHashSet ();
			var previewApiCount = 32;
			var expectedDiagnosticIds = Enumerable.Range (1, previewApiCount).Select (v => $"BGEN{v:0000}").ToHashSet ();

			var unexpectedDiagnosticIds = allExperimentalDiagnosticIds.Except (expectedDiagnosticIds).OrderBy (v => v);
			var missingDiagnosticIds = expectedDiagnosticIds.Except (allExperimentalDiagnosticIds).OrderBy (v => v);

			Assert.That (unexpectedDiagnosticIds, Is.Empty, "No unexpected diagnostic IDs"); // you probably need to increase the previewApiCount variable above (if you added more definitions to the tests/preview.cs file).
			Assert.That (missingDiagnosticIds, Is.Empty, "No missing diagnostic IDs");
		}

		[Test]
		public void DelegateParameterAttributes ()
		{
			BuildFile (Profile.iOS, "tests/delegate-parameter-attributes.cs");
		}

		[Test]
		public void Issue19612 ()
		{
			var profile = Profile.iOS;
			var filename = Path.Combine (Configuration.SourceRoot, "tests", "generator", "issue19612.cs");

			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());

			// Compile the temporary assembly and pass the compiled assembly to the generator instead
			// of relying on the generator to compile.
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var tmpassembly = Path.Combine (tmpdir, "temporaryAssembly.dll");
			var cscArguments = new List<string> ();
			if (!StringUtils.TryParseArguments (Configuration.DotNetCscCommand, out var cscCommand, out var ex))
				throw new InvalidOperationException ($"Unable to parse the .NET csc command '{Configuration.DotNetCscCommand}': {ex.Message}");
			cscArguments.AddRange (cscCommand);
			var cscExecutable = cscArguments [0];
			cscArguments.RemoveAt (0);
			cscArguments.Add (filename);
			cscArguments.Add ($"/out:{tmpassembly}");
			cscArguments.Add ("/target:library");
			cscArguments.Add ($"/r:{Path.Combine (Configuration.DotNetBclDir, "System.Runtime.dll")}");
			var tf = TargetFramework.Parse (BGenTool.GetTargetFramework (profile));
			cscArguments.Add ($"/r:{Configuration.GetBindingAttributePath (tf)}");
			cscArguments.Add ($"/r:{Configuration.GetBaseLibrary (tf)}");
			BGenTool.AddPreviewNoWarn (cscArguments);
			var rv = ExecutionHelper.Execute (cscExecutable, cscArguments);
			Assert.AreEqual (0, rv, "CSC exit code");

			var bgen = new BGenTool ();
			bgen.Profile = profile;
			bgen.CompiledApiDefinitionAssembly = tmpassembly;
			bgen.Defines = BGenTool.GetDefaultDefines (bgen.Profile);
			bgen.CreateTemporaryBinding (filename);
			bgen.AssertExecute ("build");
			bgen.AssertNoWarnings ();
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void BackingFieldType (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = BuildFile (profile, true, true, "tests/backingfieldtype.cs");

			const string nintName = "System.IntPtr";
			const string nuintName = "System.UIntPtr";

			var testCases = new [] {
				new { BackingFieldType = "NSNumber", NullableType = "Foundation.NSNumber", RenderedBackingFieldType = "Foundation.NSNumber", SimplifiedNullableType = "Foundation.NSNumber" },
				new { BackingFieldType = "NSInteger", NullableType = $"System.Nullable`1<{nintName}>", RenderedBackingFieldType = nintName, SimplifiedNullableType = "System.Nullable`1" },
				new { BackingFieldType = "NSUInteger", NullableType = $"System.Nullable`1<{nuintName}>", RenderedBackingFieldType = nuintName, SimplifiedNullableType = "System.Nullable`1" },
				new { BackingFieldType = "Int32", NullableType = $"System.Nullable`1<System.Int32>", RenderedBackingFieldType = "System.Int32", SimplifiedNullableType = "System.Nullable`1" },
				new { BackingFieldType = "Int64", NullableType = $"System.Nullable`1<System.Int64>", RenderedBackingFieldType = "System.Int64", SimplifiedNullableType = "System.Nullable`1" },
				new { BackingFieldType = "UInt32", NullableType = $"System.Nullable`1<System.UInt32>", RenderedBackingFieldType = "System.UInt32", SimplifiedNullableType = "System.Nullable`1" },
				new { BackingFieldType = "UInt64", NullableType = $"System.Nullable`1<System.UInt64>", RenderedBackingFieldType = "System.UInt64", SimplifiedNullableType = "System.Nullable`1" },
			};

			foreach (var tc in testCases) {
				var getConstant = bgen.ApiAssembly.MainModule.GetType ("BackingField", $"{tc.BackingFieldType}FieldTypeExtensions").Methods.First ((v) => v.Name == "GetConstant");
				Assert.AreEqual (tc.NullableType, getConstant.ReturnType.FullName, $"{tc.BackingFieldType}: GetConstant return type");

				var getValue = bgen.ApiAssembly.MainModule.GetType ("BackingField", $"{tc.BackingFieldType}FieldTypeExtensions").Methods.First ((v) => v.Name == "GetValue");
				Assert.AreEqual (tc.RenderedBackingFieldType, getValue.Parameters [0].ParameterType.FullName, $"{tc.BackingFieldType}: GetValue parameter type");

				var toEnumArray = bgen.ApiAssembly.MainModule.GetType ("BackingField", $"{tc.BackingFieldType}FieldTypeExtensions").Methods.First ((v) => v.Name == "ToEnumArray");
				Assert.IsTrue (toEnumArray.ReturnType.IsArray, $"{tc.BackingFieldType} ToEnumArray return type IsArray");
				Assert.AreEqual ($"{tc.BackingFieldType}FieldType", toEnumArray.ReturnType.GetElementType ().Name, $"{tc.BackingFieldType} ToEnumArray return type");
				Assert.IsTrue (toEnumArray.Parameters [0].ParameterType.IsArray, $"{tc.BackingFieldType} ToEnumArray parameter type IsArray");
				Assert.AreEqual (tc.RenderedBackingFieldType, toEnumArray.Parameters [0].ParameterType.GetElementType ().FullName, $"{tc.BackingFieldType} ToEnumArray parameter type");

				var toConstantArray = bgen.ApiAssembly.MainModule.GetType ("BackingField", $"{tc.BackingFieldType}FieldTypeExtensions").Methods.First ((v) => v.Name == "ToConstantArray");
				Assert.IsTrue (toConstantArray.ReturnType.IsArray, $"{tc.BackingFieldType} ToConstantArray return type IsArray");
				Assert.AreEqual (tc.SimplifiedNullableType, toConstantArray.ReturnType.GetElementType ().FullName, $"{tc.BackingFieldType} ToConstantArray return type");
				Assert.IsTrue (toConstantArray.Parameters [0].ParameterType.IsArray, $"{tc.BackingFieldType} ToConstantArray parameter type IsArray");
				Assert.AreEqual ($"{tc.BackingFieldType}FieldType", toConstantArray.Parameters [0].ParameterType.GetElementType ().Name, $"{tc.BackingFieldType} ToConstantArray parameter type");
			}
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void UnderlyingFieldType (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			BuildFile (profile, true, true, "tests/underlyingfieldtype.cs");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void DelegatesWithNullableReturnType (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = BuildFile (profile, "tests/delegate-nullable-return.cs");
			bgen.AssertNoWarnings ();

			var delegateCallback = bgen.ApiAssembly.MainModule.GetType ("NS", "MyCallback").Methods.First ((v) => v.Name == "EndInvoke");
			Assert.That (delegateCallback.MethodReturnType.CustomAttributes.Any (v => v.AttributeType.Name == "NullableAttribute"), "Nullable return type");
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void DelegatesWithPointerTypes (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = BuildFile (profile, "tests/delegate-types.cs");
			bgen.AssertNoWarnings ();

			var delegateCallback = bgen.ApiAssembly.MainModule.GetType ("NS", "MyCallback").Methods.First ((v) => v.Name == "EndInvoke");
			Assert.IsTrue (delegateCallback.MethodReturnType.ReturnType.IsPointer, "Pointer return type");
			foreach (var p in delegateCallback.Parameters.Where (v => v.Name != "result")) {
				Assert.IsTrue (p.ParameterType.IsPointer, $"Pointer parameter type: {p.Name}");
			}
		}

		[Test]
		[TestCase (Profile.iOS)]
		public void ReleaseAttribute (Profile profile)
		{
			Configuration.IgnoreIfIgnoredPlatform (profile.AsPlatform ());
			var bgen = BuildFile (profile, "tests/release-attribute.cs");
			bgen.AssertNoWarnings ();

			var passesOwnsEqualsTrue = new Func<MethodDefinition, bool> ((method) => {
				foreach (var ins in method.Body.Instructions) {
					switch (ins.OpCode.Code) {
					case Code.Call:
					case Code.Calli:
					case Code.Callvirt:
						var mr = (MethodReference) ins.Operand;
						switch (mr.Name) {
						case "GetINativeObject":
						case "GetNSObject":
						case "FromHandle":
							var prev = ins.Previous;
							return prev.OpCode.Code == Code.Ldc_I4_1;
						}
						break;
					}
				}
				return false;
			});

			// The last argument in the call to GetNSObject, GetINativeObject or FromHandle (or any other object-creating methods) must be 'true'.
			var methods = bgen.ApiAssembly.MainModule.GetType ("NS", "ReleaseAttributeTest").Methods
								.Where ((v) => !v.IsConstructor)
								.Where (v => v.Name != "get_ClassHandle");
			Assert.Multiple (() => {
				foreach (var method in methods)
					Assert.True (passesOwnsEqualsTrue (method), method.Name);
			});
		}
	}
}
