// <auto-generated />

#nullable enable

using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System;

namespace ObjCRuntime;

[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
static partial class Libraries
{
	static public class customlibrary
	{
		static public readonly IntPtr Handle = Dlfcn.dlopen ("/path/to/customlibrary.framework", 0);
	}
}
