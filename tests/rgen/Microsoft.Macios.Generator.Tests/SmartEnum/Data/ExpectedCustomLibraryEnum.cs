// <auto-generated />

#nullable enable

using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System;
using System.Runtime.Versioning;

namespace CustomLibrary;

/// <summary>
/// Extension methods for the <see cref="CustomLibraryEnumExtensions" /> enumeration.
/// </summary>
[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
public static partial class CustomLibraryEnumExtensions
{

	static IntPtr[] values = new IntPtr [3];

	[Field ("None", "/path/to/customlibrary.framework")]
	internal unsafe static IntPtr None
	{
		get
		{
			fixed (IntPtr *storage = &values [0])
				return Dlfcn.CachePointer (global::ObjCRuntime.Libraries.customlibrary.Handle, "None", storage);
		}
	}

	[Field ("Medium", "/path/to/customlibrary.framework")]
	internal unsafe static IntPtr Medium
	{
		get
		{
			fixed (IntPtr *storage = &values [1])
				return Dlfcn.CachePointer (global::ObjCRuntime.Libraries.customlibrary.Handle, "Medium", storage);
		}
	}

	[Field ("High", "/path/to/customlibrary.framework")]
	internal unsafe static IntPtr High
	{
		get
		{
			fixed (IntPtr *storage = &values [2])
				return Dlfcn.CachePointer (global::ObjCRuntime.Libraries.customlibrary.Handle, "High", storage);
		}
	}

	/// <summary>
	/// Retrieves the <see cref="global::Foundation.NSString" /> constant that describes <paramref name="self" />.
	/// </summary>
	/// <param name="self">The instance on which this method operates.</param>
	public static NSString? GetConstant (this CustomLibraryEnum self)
	{
		IntPtr ptr = IntPtr.Zero;
		switch ((int) self)
		{
			case 0: // None
				ptr = None;
				break;
			case 1: // Medium
				ptr = Medium;
				break;
			case 2: // High
				ptr = High;
				break;
		}
		return (NSString?) Runtime.GetNSObject (ptr);
	}

	/// <summary>
	/// Retrieves the <see cref="CustomLibraryEnumExtensions" /> value named by <paramref name="constant" />.
	/// </summary>
	/// <param name="constant">The name of the constant to retrieve.</param>
	public static CustomLibraryEnum GetValue (NSString constant)
	{
		if (constant is null)
			throw new ArgumentNullException (nameof (constant));
		if (constant.IsEqualTo (None))
			return CustomLibraryEnum.None;
		if (constant.IsEqualTo (Medium))
			return CustomLibraryEnum.Medium;
		if (constant.IsEqualTo (High))
			return CustomLibraryEnum.High;
		throw new NotSupportedException ($"The constant {constant} has no associated enum value on this platform.");
	}

	/// <summary>
	/// Retrieves the <see cref="CustomLibraryEnumExtensions" /> value represented by the backing field value in <paramref name="handle" />.
	/// </summary>
	/// <param name="handle">The native handle with the name of the constant to retrieve.</param>
	public static CustomLibraryEnum GetValue (global::ObjCRuntime.NativeHandle handle)
	{
		using var str = Runtime.GetNSObject<NSString> (handle)!;
		return GetValue (str);
	}

	/// <summary>
	/// Retrieves the <see cref="CustomLibraryEnumExtensions" /> value represented by the backing field value in <paramref name="handle" />.
	/// </summary>
	/// <param name="handle">The native handle with the name of the constant to retrieve.</param>
	public static CustomLibraryEnum? GetNullableValue (global::ObjCRuntime.NativeHandle handle)
	{
		using var str = Runtime.GetNSObject<NSString> (handle);
		if (str is null)
			return null;
		return GetValue (str);
	}

	/// <summary>
	/// Converts an array of <see cref="CustomLibraryEnumExtensions" /> enum values into an array of their corresponding constants.
	/// </summary>
	/// <param name="values">The array of enum values to convert.</param>
	internal static NSString?[]? ToConstantArray (this CustomLibraryEnum[]? values)
	{
		if (values is null)
			return null;
		var rv = new global::System.Collections.Generic.List<NSString?> ();
		for (var i = 0; i < values.Length; i++) {
			var value = values [i];
			rv.Add (value.GetConstant ());
		}
		return rv.ToArray ();
	}

	/// <summary>
	/// Converts an array of <see cref="NSString" /> values into an array of their corresponding enum values.
	/// </summary>
	/// <param name="values">The array if <see cref="NSString" /> values to convert.</param>
	internal static CustomLibraryEnum[]? ToEnumArray (this NSString[]? values)
	{
		if (values is null)
			return null;
		var rv = new global::System.Collections.Generic.List<CustomLibraryEnum> ();
		for (var i = 0; i < values.Length; i++) {
			var value = values [i];
			rv.Add (GetValue (value));
		}
		return rv.ToArray ();
	}
}
