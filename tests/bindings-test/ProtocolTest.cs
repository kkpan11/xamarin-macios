using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Linq;

using Foundation;
using ObjCRuntime;

using NUnit.Framework;

using Bindings.Test;

namespace Xamarin.BindingTests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ProtocolTest {
		bool HasProtocolAttributes {
			get {
				if (TestRuntime.IsLinkAll) {
#if OPTIMIZEALL && __MACOS__
					return false;
#else
					if (!Runtime.DynamicRegistrationSupported)
						return false;
#endif
				}


				return true;
			}
		}

		[Test]
		public void Constructors ()
		{
			using var dateNow = (NSDate) DateTime.Now;

			using (var obj = IConstructorProtocol.CreateInstance<TypeProvidingProtocolConstructors> ("Hello world")) {
				Assert.AreEqual ("Hello world", obj.StringValue, "A StringValue");
				Assert.IsNull (obj.DateValue, "A DateValue");
			}

			using (var obj = IConstructorProtocol.CreateInstance<TypeProvidingProtocolConstructors> (dateNow)) {
				Assert.IsNull (obj.StringValue, "B StringValue");
				Assert.AreEqual (dateNow, obj.DateValue, "B DateValue");
			}

			using (var obj = IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors> ("Hello Subclassed")) {
				Assert.AreEqual ("Hello Subclassed", obj.StringValue, "C1 StringValue");
				Assert.IsNull (obj.DateValue, "C1 DateValue");
			}

			using (var obj = IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors> (dateNow)) {
				Assert.IsNull (obj.StringValue, "C2 StringValue");
				Assert.AreEqual (dateNow, obj.DateValue, "C2 DateValue");
			}

			if (global::XamarinTests.ObjCRuntime.Registrar.IsDynamicRegistrar) {
				Assert.Throws<RuntimeException> (() => {
					IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors2> ("Hello Subclassed 2");
				}, "D1 Exception");
			} else {
				using (var obj = IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors2> ("Hello Subclassed 2")) {
					Assert.AreEqual ("Managed interceptor! Hello Subclassed 2", obj.StringValue, "D1 StringValue");
					Assert.IsNull (obj.DateValue, "D1 DateValue");
				}
			}

			if (XamarinTests.ObjCRuntime.Registrar.IsDynamicRegistrar) {
				Assert.Throws<RuntimeException> (() => {
					IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors2> (dateNow);
				}, "D2 Exception");
			} else {
				using (var obj = IConstructorProtocol.CreateInstance<SubclassedTypeProvidingProtocolConstructors2> (dateNow)) {
					Assert.IsNull (obj.StringValue, "D2 StringValue");
					Assert.AreEqual (dateNow.AddSeconds (42), obj.DateValue, "D2 DateValue");
				}
			}
		}

		class SubclassedTypeProvidingProtocolConstructors : TypeProvidingProtocolConstructors {
			SubclassedTypeProvidingProtocolConstructors (NativeHandle handle) : base (handle) { }

		}

		class SubclassedTypeProvidingProtocolConstructors2 : TypeProvidingProtocolConstructors {
			SubclassedTypeProvidingProtocolConstructors2 (NativeHandle handle) : base (handle) { }

			[Export ("initRequired:")]
			public SubclassedTypeProvidingProtocolConstructors2 (string value)
				: base ($"Managed interceptor! " + value)
			{
			}

			[Export ("initOptional:")]
			public SubclassedTypeProvidingProtocolConstructors2 (NSDate value)
				: base (value.AddSeconds (42))
			{
			}
		}

		[Test]
		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test verifies trimmer behavior, and as such must do trimmer-unsafe stuff.")]
		public void OnlyProtocol ()
		{
			// a binding with only [Protocol]
			var bindingAssembly = GetType ().Assembly;

			// the interface must be created
			var IP1 = bindingAssembly.GetType ("Bindings.Test.Protocol.IP1");
			Assert.IsNotNull (IP1, "IP1");
			// with a [Protocol] attribute
			var IP1Attributes = IP1.GetCustomAttributes (typeof (ProtocolAttribute), false);
			if (HasProtocolAttributes) {
				Assert.AreEqual (1, IP1Attributes.Length, "[Protocol] IP1");
				var IP1Protocol = (ProtocolAttribute) IP1Attributes [0];
				Assert.AreEqual ("P1", IP1Protocol.Name, "Name");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P1Wrapper");
				Assert.IsNotNull (wrapperType, "P1_Wrapper");
				Assert.AreEqual (wrapperType, IP1Protocol.WrapperType, "WrapperType");
			} else {
				Assert.AreEqual (0, IP1Attributes.Length, "[Protocol] IP1");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P1Wrapper");
				Assert.IsNotNull (wrapperType, "P1_Wrapper");
			}
			// but not the model
			Assert.IsNull (bindingAssembly.GetType ("Bindings.Test.Protocol.P1"), "P1");
		}

		[Test]
		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test verifies trimmer behavior, and as such must do trimmer-unsafe stuff.")]
		public void ProtocolWithBaseType ()
		{
			// a binding with [Protocol] and [BaseType]
			var bindingAssembly = GetType ().Assembly;

			// the interface must be created
			var IP2 = bindingAssembly.GetType ("Bindings.Test.Protocol.IP2");
			Assert.IsNotNull (IP2, "IP2");

			// with a [Protocol] attribute
			var IP2Attributes = IP2.GetCustomAttributes (typeof (ProtocolAttribute), false);
			if (HasProtocolAttributes) {
				Assert.AreEqual (1, IP2Attributes.Length, "[Protocol] IP2");
				var IP2Protocol = (ProtocolAttribute) IP2Attributes [0];
				Assert.AreEqual ("P2", IP2Protocol.Name, "Name");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P2Wrapper");
				Assert.IsNotNull (wrapperType, "P2_Wrapper");
				Assert.AreEqual (wrapperType, IP2Protocol.WrapperType, "WrapperType");
			} else {
				Assert.AreEqual (0, IP2Attributes.Length, "[Protocol] IP2");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P2Wrapper");
				Assert.IsNotNull (wrapperType, "P2_Wrapper");
			}

			// and a model-like class
			var model = bindingAssembly.GetType ("Bindings.Test.Protocol.P2");
			Assert.IsNotNull (model, "P2");
			// but without the [Model] attribute
			Assert.False (model.IsDefined (typeof (ModelAttribute), false), "model");
		}

		[Test]
		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test verifies trimmer behavior, and as such must do trimmer-unsafe stuff.")]
		public void ProtocolWithBaseTypeAndModel ()
		{
			// a binding with [Protocol] and [BaseType]
			var bindingAssembly = GetType ().Assembly;

			// the interface must be created
			var IP3 = bindingAssembly.GetType ("Bindings.Test.Protocol.IP3");
			Assert.IsNotNull (IP3, "IP3");

			// with a [Protocol] attribute
			var IP3Attributes = IP3.GetCustomAttributes (typeof (ProtocolAttribute), false);
			if (HasProtocolAttributes) {
				Assert.AreEqual (1, IP3Attributes.Length, "[Protocol] IP3");
				var IP3Protocol = (ProtocolAttribute) IP3Attributes [0];
				Assert.AreEqual ("P3", IP3Protocol.Name, "Name");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P3Wrapper");
				Assert.IsNotNull (wrapperType, "P3_Wrapper");
				Assert.AreEqual (wrapperType, IP3Protocol.WrapperType, "WrapperType");
			} else {
				Assert.AreEqual (0, IP3Attributes.Length, "[Protocol] IP3");

				// and a wrapper type
				var wrapperType = bindingAssembly.GetType ("Bindings.Test.Protocol.P3Wrapper");
				Assert.IsNotNull (wrapperType, "P3_Wrapper");
			}

			// and a model class
			var model = bindingAssembly.GetType ("Bindings.Test.Protocol.P3");
			Assert.IsNotNull (model, "P3");
			// with a [Model] attribute
			Assert.True (model.IsDefined (typeof (ModelAttribute), false), "model");
		}

		class MembersImplementation : NSObject, Bindings.Test.Protocol.IMemberAttributes {
			public void RequiredInstanceMethod ()
			{
			}

			public string RequiredInstanceProperty {
				get { return null; }
				set { }
			}

			public NSString RequiredReadonlyProperty {
				get { return null; }
			}
		}

		void CleanupSignatures (objc_method_description [] methods)
		{
			for (int i = 0; i < methods.Length; i++) {
				methods [i].Types = methods [i].Types.Replace ("0", "").Replace ("1", "").Replace ("2", "").Replace ("3", "").Replace ("4", "").Replace ("5", "").Replace ("6", "").Replace ("7", "").Replace ("8", "").Replace ("9", "");
			}
		}

		[Test]
		public void ProtocolMembers ()
		{
			IntPtr protocol = objc_getProtocol ("MemberAttributes");
			Assert.AreNotEqual (IntPtr.Zero, protocol, "a");

			objc_method_description [] methods;

			// Required instance methods
			methods = protocol_copyMethodDescriptionList (protocol, true, true);
			CleanupSignatures (methods);
			Assert.AreEqual (4, methods.Length, "Required Instance Methods: Count");
			AssertContains (methods, new objc_method_description ("requiredInstanceMethod", "v@:"), "Required Instance Methods: requiredInstanceMethod");
			AssertContains (methods, new objc_method_description ("requiredInstanceProperty", "@@:"), "Required Instance Methods: requiredInstanceProperty");
			AssertContains (methods, new objc_method_description ("setRequiredInstanceProperty:", "v@:@"), "Required Instance Methods: setRequiredInstanceProperty");
			AssertContains (methods, new objc_method_description ("requiredReadonlyProperty", "@@:"), "Required Instance Methods: requiredReadonlyProperty:");

			// Required static methods
			methods = protocol_copyMethodDescriptionList (protocol, true, false);
			CleanupSignatures (methods);
			Assert.AreEqual (3, methods.Length, "Required Static Methods: Count");
			AssertContains (methods, new objc_method_description ("requiredStaticMethod", "v@:"), "Required Static Methods: requiredStaticMethod");
			AssertContains (methods, new objc_method_description ("setRequiredStaticProperty:", "v@:@"), "Required Static Methods: setRequiredStaticProperty:");
			AssertContains (methods, new objc_method_description ("requiredStaticProperty", "@@:"), "Required Static Methods: requiredStaticProperty");

			// Optional instance methods
			methods = protocol_copyMethodDescriptionList (protocol, false, true);
			CleanupSignatures (methods);
			Assert.AreEqual (19, methods.Length, "Optional Instance Methods: Count");
			AssertContains (methods, new objc_method_description ("variadicMethod:", "v@:^v"), "Optional Instance Methods: variadicMethod:");
			AssertContains (methods, new objc_method_description ("methodWithReturnType", "@@:"), "Optional Instance Methods: methodWithReturnType");
			AssertContains (methods, new objc_method_description ("methodWithParameter:", "v@:i"), "Optional Instance Methods: methodWithParameter:");
			AssertContains (methods, new objc_method_description ("methodWithParameters:second:third:fourth:", "v@:iiii"), "Optional Instance Methods: methodWithParameters:second:third:fourth:");
			AssertContains (methods, new objc_method_description ("optionalInstanceMethod", "v@:"), "Optional Instance Methods: optionalInstanceMethod");
			AssertContains (methods, new objc_method_description ("methodWithRefParameters:second:third:fourth:", "v@:i^i^ii"), "Optional Instance Methods: methodWithRefParameters:second:third:fourth:");
			AssertContains (methods, new objc_method_description ("optionalInstanceProperty", "@@:"), "Optional Instance Methods: optionalInstanceProperty");
			AssertContains (methods, new objc_method_description ("setOptionalInstanceProperty:", "v@:@"), "Optional Instance Methods: setOptionalInstanceProperty:");
			AssertContains (methods, new objc_method_description ("get_propertyWithCustomAccessors", "@@:"), "Optional Instance Methods: get_propertyWithCustomAccessors");
			AssertContains (methods, new objc_method_description ("set_propertyWithCustomAccessors:", "v@:@"), "Optional Instance Methods: set_propertyWithCustomAccessors:");
			AssertContains (methods, new objc_method_description ("propertyWithArgumentSemanticNone", "@@:"), "Optional Instance Methods: propertyWithArgumentSemanticNone");
			AssertContains (methods, new objc_method_description ("setPropertyWithArgumentSemanticNone:", "v@:@"), "Optional Instance Methods: setPropertyWithArgumentSemanticNone:");
			AssertContains (methods, new objc_method_description ("propertyWithArgumentSemanticCopy", "@@:"), "Optional Instance Methods: propertyWithArgumentSemanticCopy");
			AssertContains (methods, new objc_method_description ("setPropertyWithArgumentSemanticCopy:", "v@:@"), "Optional Instance Methods: setPropertyWithArgumentSemanticCopy:");
			AssertContains (methods, new objc_method_description ("propertyWithArgumentSemanticAssign", "@@:"), "Optional Instance Methods: propertyWithArgumentSemanticAssign");
			AssertContains (methods, new objc_method_description ("setPropertyWithArgumentSemanticAssign:", "v@:@"), "Optional Instance Methods: setPropertyWithArgumentSemanticAssign:");
			AssertContains (methods, new objc_method_description ("readonlyProperty", "@@:"), "Optional Instance Methods: readonlyProperty:");
			AssertContains (methods, new objc_method_description ("propertyWithArgumentSemanticRetain", "@@:"), "Optional Instance Methods: propertyWithArgumentSemanticRetain");
			AssertContains (methods, new objc_method_description ("setPropertyWithArgumentSemanticRetain:", "v@:@"), "Optional Instance Methods: setPropertyWithArgumentSemanticRetain:");

			// Optional static methods
			methods = protocol_copyMethodDescriptionList (protocol, false, false);
			CleanupSignatures (methods);
			Assert.AreEqual (3, methods.Length, "Optional Static Methods: Count");
			AssertContains (methods, new objc_method_description ("optionalStaticMethod", "v@:"), "Optional Static Methods: optionalStaticMethod");
			AssertContains (methods, new objc_method_description ("optionalStaticProperty", "@@:"), "Optional Static Methods: optionalStaticProperty");
			AssertContains (methods, new objc_method_description ("setOptionalStaticProperty:", "v@:@"), "Optional Static Methods: setOptionalStaticProperty:");

			objc_property [] properties;
			properties = protocol_copyPropertyList (protocol);

			// The ObjC runtime won't add optional properties dynamically (the code is commented out,
			// see file objc4-647/runtime/objc-runtime-old.mm in Apple's open source code),
			// so we need to verify differently for the dynamic registrar.
			if (XamarinTests.ObjCRuntime.Registrar.IsStaticRegistrar) {
				Assert.AreEqual (9, properties.Length, "Properties: Count");
			} else {
				Assert.AreEqual (2, properties.Length, "Properties: Count");
			}

			AssertContains (properties, new objc_property ("requiredInstanceProperty", "T@\"NSString\",N", new objc_property_attribute [] {
				new objc_property_attribute ("T", "@\"NSString\""),
				new objc_property_attribute ("N", "")
			}), "Properties: requiredInstanceProperty");

			AssertContains (properties, new objc_property ("requiredReadonlyProperty", "T@\"NSString\",R,N", new objc_property_attribute [] {
				new objc_property_attribute ("T", "@\"NSString\""),
				new objc_property_attribute ("R", ""),
				new objc_property_attribute ("N", "")
			}), "Properties: requiredReadonlyProperty");

			if (XamarinTests.ObjCRuntime.Registrar.IsStaticRegistrar) {
				AssertContains (properties, new objc_property ("optionalInstanceProperty", "T@\"NSString\",N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("N", "")
				}), "Properties: optionalInstanceProperty");

				AssertContains (properties, new objc_property ("propertyWithCustomAccessors", "T@\"NSString\",N,Gget_propertyWithCustomAccessors,Sset_propertyWithCustomAccessors:", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("N", ""),
					new objc_property_attribute ("G", "get_propertyWithCustomAccessors"),
					new objc_property_attribute ("S", "set_propertyWithCustomAccessors:")
				}), "Properties: propertyWithCustomAccessors");

				AssertContains (properties, new objc_property ("propertyWithArgumentSemanticNone", "T@\"NSString\",N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("N", "")
				}), "Properties: propertyWithArgumentSemanticNone");

				AssertContains (properties, new objc_property ("propertyWithArgumentSemanticCopy", "T@\"NSString\",C,N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("N", ""),
					new objc_property_attribute ("C", "")
				}), "Properties: propertyWithArgumentSemanticCopy");

				AssertContains (properties, new objc_property ("propertyWithArgumentSemanticAssign", "T@\"NSString\",N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("N", "")
				}), "Properties: propertyWithArgumentSemanticAssign");

				AssertContains (properties, new objc_property ("propertyWithArgumentSemanticRetain", "T@\"NSString\",&,N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("&", ""),
					new objc_property_attribute ("N", "")
				}), "Properties: propertyWithArgumentSemanticRetain");

				AssertContains (properties, new objc_property ("readonlyProperty", "T@\"NSString\",R,N", new objc_property_attribute [] {
					new objc_property_attribute ("T", "@\"NSString\""),
					new objc_property_attribute ("R", ""),
					new objc_property_attribute ("N", "")
				}), "Properties: readonlyProperty");
			}
		}

		static void AssertContains<T> (T [] array, T item, string message) where T : IEquatable<T>
		{
			for (var i = 0; i < array.Length; i++) {
				var element = array [i];
				if (element is null && item is null)
					return;
				if (element is null || item is null)
					continue;
				if (element.Equals (item))
					return;
			}

			throw new Exception ($"Collection {array} does not contain item {item}: {message}");
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr objc_getProtocol (string name);

		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr protocol_getName (IntPtr protocol);

		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "protocol_copyMethodDescriptionList")]
		extern static IntPtr _protocol_copyMethodDescriptionList (IntPtr protocol, bool isRequiredMethod, bool isInstanceMethod, out int count);

		static objc_method_description [] protocol_copyMethodDescriptionList (IntPtr protocol, bool isRequiredMethod, bool isInstanceMethod)
		{
			int count;
			IntPtr methods = _protocol_copyMethodDescriptionList (protocol, isRequiredMethod, isInstanceMethod, out count);
			try {
				var rv = new objc_method_description [count];
				for (int i = 0; i < count; i++) {
					var sel = new Selector (Marshal.ReadIntPtr (methods + (IntPtr.Size * 2) * i)).Name;
					var types = Marshal.PtrToStringAuto (Marshal.ReadIntPtr (methods + (IntPtr.Size * 2) * i + IntPtr.Size));
					rv [i] = new objc_method_description (sel, types);
				}
				return rv;
			} finally {
				free (methods);
			}
		}

		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "protocol_copyPropertyList")]
		internal extern static IntPtr _protocol_copyPropertyList (IntPtr protocol, out int count);

		static void Trace (string message)
		{
			TestRuntime.NSLog (message);
		}

		static objc_property [] protocol_copyPropertyList (IntPtr protocol)
		{
			int count;
			IntPtr list = _protocol_copyPropertyList (protocol, out count);
			var rv = new objc_property [count];
			Trace ($"Protocol {new Protocol (protocol)} has {rv} properties");
			try {
				for (int i = 0; i < count; i++) {
					var prop = new objc_property ();
					IntPtr p = Marshal.ReadIntPtr (list, IntPtr.Size * i);
					rv [i] = prop;
					prop.Name = property_getName (p);
					prop.Attributes = property_getAttributes (p);
					prop.AttributeList = property_copyAttributeList (p);
					Trace ($"    #{i + 1}: Name={prop.Name} Attributes={prop.Attributes} AttributeList={prop.AttributeList}");
				}
				return rv;
			} finally {
				free (list);
			}
		}

		[DllImport ("/usr/lib/libobjc.dylib")]
		internal extern static IntPtr protocol_copyProtocolList (IntPtr protocol, out int count);


		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "property_getName")]
		extern static IntPtr _property_getName (IntPtr property);

		static string property_getName (IntPtr property)
		{
			return Marshal.PtrToStringAuto (_property_getName (property));
		}

		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "property_getAttributes")]
		extern static IntPtr _property_getAttributes (IntPtr property);

		static string property_getAttributes (IntPtr property)
		{
			var v = Marshal.PtrToStringAuto (_property_getAttributes (property));

			// Ignore any "?" attributes, apparently it's a new property attribute in Xcode 16, but since there's no documentation about it yet, just ignore it.
			var attribs = v.Split (',').Where (v => v != "?").ToArray ();
			return string.Join (",", attribs);
		}

		[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "property_copyAttributeList")]
		extern static IntPtr _property_copyAttributeList (IntPtr property, out int outCount);

		static objc_property_attribute [] property_copyAttributeList (IntPtr property)
		{
			int count;
			IntPtr list = _property_copyAttributeList (property, out count);
			var rv = new List<objc_property_attribute> (count);
			try {
				for (int i = 0; i < count; i++) {
					var attrib = new objc_property_attribute ();
					IntPtr n = Marshal.ReadIntPtr (list, (IntPtr.Size * 2) * i);
					IntPtr v = Marshal.ReadIntPtr (list, (IntPtr.Size * 2) * i + IntPtr.Size);
					attrib.Name = Marshal.PtrToStringAuto (n);
					attrib.Value = Marshal.PtrToStringAuto (v);
					// Ignore any "?" attributes, apparently it's a new property attribute in Xcode 16, but since there's no documentation about it yet, just ignore it.
					if (attrib.Name == "?" && string.IsNullOrEmpty (attrib.Value))
						continue;
					rv.Add (attrib);
				}
				return rv.ToArray ();
			} finally {
				free (list);
			}
		}

		[DllImport ("/usr/lib/libc.dylib")]
		internal extern static void free (IntPtr ptr);

		class objc_property_attribute : IEquatable<objc_property_attribute> {
			public string Name;
			public string Value;

			public objc_property_attribute ()
			{
			}

			public objc_property_attribute (string name, string value)
			{
				this.Name = name;
				this.Value = value;
			}

			bool IEquatable<objc_property_attribute>.Equals (objc_property_attribute other)
			{
				return Name == other.Name && Value == other.Value;
			}

			public override bool Equals (object obj)
			{
				var other = (objc_property_attribute) obj;
				if (other is null)
					return false;
				return Name == other.Name && Value == other.Value;
			}

			public override int GetHashCode ()
			{
				return HashCode.Combine (Name, Value);
			}

			public override string ToString ()
			{
				return string.Format ("{0} = {1}", Name, Value);
			}
		}

		class objc_property : IEquatable<objc_property> {
			public string Name;
			public string Attributes;
			public objc_property_attribute [] AttributeList;

			public objc_property ()
			{
			}

			public objc_property (string name, string attributes, objc_property_attribute [] list)
			{
				this.Name = name;
				this.Attributes = attributes;
				this.AttributeList = list;
			}

			public override string ToString ()
			{
				return string.Format ("[{0}; {1}; {2}]", Name, Attributes, string.Join (", ", new List<objc_property_attribute> (AttributeList).Select ((v) => string.Format ("{0} = {1}", v.Name, v.Value))));
			}

			bool IEquatable<objc_property>.Equals (objc_property other)
			{
				if (other.Name != Name)
					return false;
				if (other.Attributes != Attributes)
					return false;
				if (other.AttributeList.Length != AttributeList.Length)
					return false;
				foreach (var entry in AttributeList)
					if (!other.AttributeList.Contains (entry))
						return false;
				return true;
			}
		}

		class objc_method_description : IEquatable<objc_method_description> {
			public string Name;
			public string Types;

			public objc_method_description (string name, string types)
			{
				this.Name = name;
				this.Types = types;
			}

			public override string ToString ()
			{
				return string.Format ("[{0}; {1}]", Name, Types);
			}

			bool IEquatable<objc_method_description>.Equals (objc_method_description other)
			{
				return other.Name == Name && other.Types == Types;
			}
		}
	}
}
