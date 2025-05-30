//
// Unit tests for the registrars.
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

namespace XamarinTests.ObjCRuntime {

	[Flags]
	public enum Registrars {
		Static = 1,
		ManagedStatic = Static | 2,
		Dynamic = 4,
		AllStatic = Static | ManagedStatic,
		AllDynamic = Dynamic,
	}

	public class Registrar {
		[Register ("__registration_test_CLASS")]
		class RegistrationTestClass : NSObject { }

		public static bool IsStaticRegistrar {
			get {
				return CurrentRegistrar.HasFlag (Registrars.Static);
			}
		}

		public static bool IsDynamicRegistrar {
			get {
				return CurrentRegistrar.HasFlag (Registrars.Dynamic);
			}
		}

		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test accesses internals, and this code seems to work fine with the trimmer enabled.")]
		public static Registrars CurrentRegistrar {
			get {
				var __registrar__ = typeof (Class).Assembly.GetType ("ObjCRuntime.__Registrar__");
				if (__registrar__ is not null)
					return Registrars.ManagedStatic;
				var types = new Type [] { typeof (NativeHandle), typeof (bool).MakeByRefType () };
				var find_type = typeof (Class).GetMethod ("FindType", BindingFlags.Static | BindingFlags.NonPublic, null, types, null);
				var type_to_find = typeof (RegistrationTestClass);
				var type = (Type) find_type.Invoke (null, new object [] { Class.GetHandle (type_to_find), false });
				var is_static = type_to_find == type;
				if (is_static) {
					return Registrars.Static;
				} else {
					return Registrars.Dynamic;
				}
			}
		}
	}
}
