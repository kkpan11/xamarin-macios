---
title: "Build optimizations"
description: "This document explains the various optimizations that are applied at build time for Xamarin.iOS and Xamarin.Mac apps."
ms.service: xamarin
ms.assetid: 84B67E31-B217-443D-89E5-CFE1923CB14E
author: davidortinau
ms.author: daortin
ms.date: 04/16/2018
no-loc: [Objective-C, registrar, Registrar]
---

# Build optimizations

This document explains the various optimizations that are applied at build time for
Xamarin.iOS and Xamarin.Mac apps.

## Remove UIApplication.EnsureUIThread / NSApplication.EnsureUIThread

Removes calls to [UIApplication.EnsureUIThread][1] (for Xamarin.iOS) or
`NSApplication.EnsureUIThread` (for Xamarin.Mac).

This optimization will change the following type of code:

```csharp
public virtual void AddChildViewController (UIViewController childController)
{
    global::UIKit.UIApplication.EnsureUIThread ();
    // ...
}
```

into the following:

```csharp
public virtual void AddChildViewController (UIViewController childController)
{
    // ...
}
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

By default it's enabled for release builds.

The default behavior can be overridden by passing `--optimize=[+|-]remove-uithread-checks` to mtouch/mmp.

[1]: /dotnet/api/UIKit.UIApplication.EnsureUIThread

## Inline IntPtr.Size

Inlines the constant value of `IntPtr.Size` according to the target platform.

This optimization will change the following type of code:

```csharp
if (IntPtr.Size == 8) {
    Console.WriteLine ("64-bit platform");
} else {
    Console.WriteLine ("32-bit platform");
}
```

into the following (when building for a 64-bit platform):

```csharp
if (8 == 8) {
    Console.WriteLine ("64-bit platform");
} else {
    Console.WriteLine ("32-bit platform");
}
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

By default it's enabled if targeting a single architecture, or for the
platform assembly (**Xamarin.iOS.dll**, **Xamarin.TVOS.dll**,
**Xamarin.WatchOS.dll** or **Xamarin.Mac.dll**).

If targeting multiple architectures, this optimization will create different
assemblies for the 32-bit version and the 64-bit version of the app, and both
versions will have to be included in the app, effectively increasing the final
app size instead of decreasing it.

The default behavior can be overridden by passing `--optimize=[+|-]inline-intptr-size` to mtouch/mmp.

## Inline NSObject.IsDirectBinding

`NSObject.IsDirectBinding` is an instance property that determines whether a
particular instance is of a wrapper type or not (a wrapper type is a managed
type that maps to a native type; for instance the managed `UIKit.UIView` type
maps to the native `UIView` type - the opposite is a user type, in this case
`class MyUIView : UIKit.UIView` would be a user type).

It's necessary to know the value of `IsDirectBinding` when calling into
Objective-C, because the value determines which version of `objc_msgSend` to
use.

Given only the following code:

```csharp
class UIView : NSObject {
    public virtual string SomeProperty {
        get {
            if (IsDirectBinding) {
                return "true";
            } else {
                return "false"
            }
        }
    }
}

class NSUrl : NSObject {
    public virtual string SomeOtherProperty {
        get {
            if (IsDirectBinding) {
                return "true";
            } else {
                return "false"
            }
        }
    }
}

class MyUIView : UIView {
}
```

We can determine that in `UIView.SomeProperty` the value of
`IsDirectBinding` is not a constant and cannot be inlined:

```csharp
void uiView = new UIView ();
Console.WriteLine (uiView.SomeProperty); /* prints 'true' */
void myView = new MyUIView ();
Console.WriteLine (myView.SomeProperty); // prints 'false'
```

However, it's possible to look at the all the types in the app and determine
that there are no types that inherit from `NSUrl`, and it's thus safe to
inline the `IsDirectBinding` value to a constant `true`:

```csharp
void myURL = new NSUrl ();
Console.WriteLine (myURL.SomeOtherProperty); // prints 'true'
// There's no way to make SomeOtherProperty print anything but 'true', since there are no NSUrl subclasses.
```

In particular, this optimization will change the following type of code (this
is the binding code for `NSUrl.AbsoluteUrl`):

```csharp
if (IsDirectBinding) {
    return Runtime.GetNSObject<NSUrl> (global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("absoluteURL")));
} else {
    return Runtime.GetNSObject<NSUrl> (global::ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("absoluteURL")));
}
```

into the following (when it can be determined that there are no subclasses of
`NSUrl` in the app):

```csharp
if (true) {
    return Runtime.GetNSObject<NSUrl> (global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle ("absoluteURL")));
} else {
    return Runtime.GetNSObject<NSUrl> (global::ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("absoluteURL")));
}
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is always enabled by default for Xamarin.iOS, and always disabled by
default for Xamarin.Mac (because it's possible to dynamically load assemblies
in Xamarin.Mac, it's not possible to determine that a particular class is
never subclassed).

The default behavior can be overridden by passing
`--optimize=[+|-]inline-isdirectbinding` to mtouch/mmp.

## Inline Runtime.Arch

This optimization will change the following type of code:

```csharp
if (Runtime.Arch == Arch.DEVICE) {
    Console.WriteLine ("Running on device");
} else {
    Console.WriteLine ("Running in the simulator");
}
```

into the following (when building for device):

```csharp
if (Arch.DEVICE == Arch.DEVICE) {
    Console.WriteLine ("Running on device");
} else {
    Console.WriteLine ("Running in the simulator");
}
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is always enabled by default for Xamarin.iOS (it's not available for
Xamarin.Mac).

The default behavior can be overridden by passing
`--optimize=[+|-]inline-runtime-arch` to mtouch.

## Dead code elimination

This optimization will change the following type of code:

```csharp
if (true) {
    Console.WriteLine ("Doing this");
} else {
    Console.WriteLine ("Not doing this");
}
```

into:

```csharp
Console.WriteLine ("Doing this");
```

It will also evaluate constant comparisons, like this:

```csharp
if (8 == 8) {
    Console.WriteLine ("Doing this");
} else {
    Console.WriteLine ("Not doing this");
}
```

and determine that the expression `8 == 8` is a always true, and reduce it to:

```csharp
Console.WriteLine ("Doing this");
```

This is a powerful optimization when used together with the inlining
optimizations, because it can transform the following type of code (this is
the binding code for `NFCIso15693ReadMultipleBlocksConfiguration.Range`):

```csharp
NSRange ret;
if (IsDirectBinding) {
    if (Runtime.Arch == Arch.DEVICE) {
        if (IntPtr.Size == 8) {
            ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSend (this.Handle, Selector.GetHandle ("range"));
        } else {
            global::ObjCRuntime.Messaging.NSRange_objc_msgSend_stret (out ret, this.Handle, Selector.GetHandle ("range"));
        }
    } else if (IntPtr.Size == 8) {
        ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSend (this.Handle, Selector.GetHandle ("range"));
    } else {
        ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSend (this.Handle, Selector.GetHandle ("range"));
    }
} else {
    if (Runtime.Arch == Arch.DEVICE) {
        if (IntPtr.Size == 8) {
            ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("range"));
        } else {
            global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper_stret (out ret, this.SuperHandle, Selector.GetHandle ("range"));
        }
    } else if (IntPtr.Size == 8) {
        ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("range"));
    } else {
        ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("range"));
    }
}
return ret;
```

into this (when building for a 64-bit device, and when also able to
ensure there are no `NFCIso15693ReadMultipleBlocksConfiguration` subclasses
in the app):

```csharp
NSRange ret;
ret = global::ObjCRuntime.Messaging.NSRange_objc_msgSend (this.Handle, Selector.GetHandle ("range"));
return ret;
```

The AOT compiler is already able to do eliminate dead code like this, but this
optimization is done inside the linker, which means that the linker able to
see that there are multiple methods that are not used anymore, and may thus be
removed (unless used elsewhere):

* `global::ObjCRuntime.Messaging.NSRange_objc_msgSend_stret`
* `global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper`
* `global::ObjCRuntime.Messaging.NSRange_objc_msgSendSuper_stret`

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is always enabled by default (when the linker is enabled).

The default behavior can be overridden by passing
`--optimize=[+|-]dead-code-elimination` to mtouch/mmp.

## Optimize calls to BlockLiteral.SetupBlock

The Xamarin.iOS/Mac runtime needs to know the block signature when creating an
Objective-C block for a managed delegate. This might be a fairly expensive
operation. This optimization will calculate the block signature at build time,
and modify the IL to call a `SetupBlock` method that takes the signature as an
argument instead. Doing this avoids the need for calculating the signature at
runtime.

Benchmarks show that this speeds up calling a block by a factor of 10 to 15.

It will transform the following [code](https://github.com/dotnet/macios/blob/018f7153441d9d7e0f58e2046f39eeb46f1ff480/src/UIKit/UIAccessibility.cs#L198-L211):

```csharp
public static void RequestGuidedAccessSession (bool enable, Action<bool> completionHandler)
{
    // ...
    block_handler.SetupBlock (callback, completionHandler);
    // ...
}
```

into:

```csharp
public static void RequestGuidedAccessSession (bool enable, Action<bool> completionHandler)
{
    // ...
    block_handler.SetupBlockImpl (callback, completionHandler, true, "v@?B");
    // ...
}
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is enabled by default when using the static registrar (in Xamarin.iOS the
static registrar is enabled by default for device builds, and in Xamarin.Mac
the static registrar is enabled by default for release builds).

The default behavior can be overridden by passing
`--optimize=[+|-]blockliteral-setupblock` to mtouch/mmp.

## Optimize support for protocols

The Xamarin.iOS/Mac runtime needs information about how managed types
implements Objective-C protocols. This information is stored in interfaces
(and attributes on these interfaces), which is not a very efficient format,
nor is it linker-friendly.

One example is that these interfaces store information about all protocol
members in a `[ProtocolMember]` attribute, which among other things contain
references to the parameter types of those members. This means that simply
implementing such an interface will make the linker preserve all types used in
that interface, even for optional members the app never calls or implements.

This optimization will make the static registrar store any required
information in an efficient format that uses little memory that's easy and
quick to find at runtime.

It will also teach the linker that it does not necessarily need to preserve
these interfaces, nor any of the related attributes.

This optimization requires both the linker and the static registrar to be
enabled.

On Xamarin.iOS this optimization is enabled by default when both the linker
and the static registrar are enabled.

On Xamarin.Mac this optimization is never enabled by default, because
Xamarin.Mac supports loading assemblies dynamically, and those assemblies
might not have been known at build time (and thus not optimized).

The default behavior can be overridden by passing
`--optimize=-register-protocols` to mtouch/mmp.

## Remove the dynamic registrar

Both the Xamarin.iOS and the Xamarin.Mac runtime include support for
[registering managed types](~/ios/internals/registrar.md)
with the Objective-C runtime. It can either be done at build time or at
runtime (or partially at build time and the rest at runtime), but if it's
completely done at build time, it means the supporting code for doing it at
runtime can be removed. This results in a significant decrease in app size, in
particular for smaller apps such as extensions or watchOS apps.

This optimization requires both the static registrar and the linker to be
enabled.

The linker will attempt to determine if it's safe to remove the dynamic
registrar, and if so will try to remove it.

Since Xamarin.Mac supports dynamically loading assemblies at runtime (which
were not known at build time), it's impossible to determine at build time
whether this is a safe optimization. This means that this optimization is
never enabled by default for Xamarin.Mac apps.

The default behavior can be overridden by passing
`--optimize=[+|-]remove-dynamic-registrar` to mtouch/mmp.

If the default is overridden to remove the dynamic registrar, the linker will
emit warnings if it detects that it's not safe (but the dynamic registrar will
still be removed).

## Inline Runtime.DynamicRegistrationSupported

Inlines the value of `Runtime.DynamicRegistrationSupported` as determined at
build time.

If the dynamic registrar is removed (see the [Remove the dynamic
registrar](#remove-the-dynamic-registrar) optimization), this is a
constant `false` value, otherwise it's a constant `true` value.

This optimization will change the following type of code:

```csharp
if (Runtime.DynamicRegistrationSupported) {
    Console.WriteLine ("do something");
} else {
    throw new Exception ("dynamic registration is not supported");
}
```

into the following when the dynamic registrar is removed:

```csharp
throw new Exception ("dynamic registration is not supported");
```

into the following when the dynamic registrar is not removed:

```csharp
Console.WriteLine ("do something");
```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is always enabled by default (when the linker is enabled).

The default behavior can be overridden by passing
`--optimize=[+|-]inline-dynamic-registration-supported` to mtouch/mmp.

## Precompute methods to create managed delegates for Objective-C blocks

When Objective-C calls a selector that takes a block as a parameter, and then
managed code has overriden that method, the Xamarin.iOS / Xamarin.Mac runtime
needs to create a delegate for that block.

The binding code generated by the binding generator will include a
`[BlockProxy]` attribute, which specifies the type with a `Create` method that
can do this.

Given the following Objective-C code:

```objc
@interface ObjCBlockTester : NSObject {
}
-(void) classCallback: (void (^)())completionHandler;
-(void) callClassCallback;
@end

@implementation ObjCBlockTester
-(void) classCallback: (void (^)())completionHandler
{
}

-(void) callClassCallback
{
    [self classCallback: ^()
    {
        NSLog (@"called!");
    }];
}
@end
```

and the following binding code:

```csharp
[BaseType (typeof (NSObject))]
interface ObjCBlockTester
{
    [Export ("classCallback:")]
    void ClassCallback (Action completionHandler);
}
```

the generator will produce:

```csharp
[Register("ObjCBlockTester", true)]
public unsafe partial class ObjCBlockTester : NSObject {
    // unrelated code...

    [Export ("callClassCallback")]
    [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
    public virtual void CallClassCallback ()
    {
        if (IsDirectBinding) {
            ApiDefinition.Messaging.void_objc_msgSend (this.Handle, Selector.GetHandle ("callClassCallback"));
        } else {
            ApiDefinition.Messaging.void_objc_msgSendSuper (this.SuperHandle, Selector.GetHandle ("callClassCallback"));
        }
    }

    [Export ("classCallback:")]
    [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
    public unsafe virtual void ClassCallback ([BlockProxy (typeof (Trampolines.NIDActionArity1V0))] System.Action completionHandler)
    {
        // ...

    }
}

static class Trampolines
{
    [UnmanagedFunctionPointerAttribute (CallingConvention.Cdecl)]
    [UserDelegateType (typeof (System.Action))]
    internal delegate void DActionArity1V0 (IntPtr block);

    static internal class SDActionArity1V0 {
        static internal readonly DActionArity1V0 Handler = Invoke;

        [MonoPInvokeCallback (typeof (DActionArity1V0))]
        static unsafe void Invoke (IntPtr block) {
            var descriptor = (BlockLiteral *) block;
            var del = (System.Action) (descriptor->Target);
            if (del != null)
                del (obj);
        }
    }

    internal class NIDActionArity1V0 {
        IntPtr blockPtr;
        DActionArity1V0 invoker;

        [Preserve (Conditional=true)]
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        public unsafe NIDActionArity1V0 (BlockLiteral *block)
        {
            blockPtr = _Block_copy ((IntPtr) block);
            invoker = block->GetDelegateForBlock<DActionArity1V0> ();
        }

        [Preserve (Conditional=true)]
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        ~NIDActionArity1V0 ()
        {
            _Block_release (blockPtr);
        }

        [Preserve (Conditional=true)]
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        public unsafe static System.Action Create (IntPtr block)
        {
            if (block == IntPtr.Zero)
                return null;
            if (BlockLiteral.IsManagedBlock (block)) {
                var existing_delegate = ((BlockLiteral *) block)->Target as System.Action;
                if (existing_delegate != null)
                    return existing_delegate;
            }
            return new NIDActionArity1V0 ((BlockLiteral *) block).Invoke;
        }

        [Preserve (Conditional=true)]
        [BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
        unsafe void Invoke ()
        {
            invoker (blockPtr);
        }
    }
}
```

When Objective-C calls `[ObjCBlockTester callClassCallback]`, the Xamarin.iOS
/ Xamarin.Mac runtime will look at the `[BlockProxy (typeof (Trampolines.NIDActionArity1V0))]`
attribute on the parameter. It will then look up the `Create` method on that type,
and call that method to create the delegate.

This optimization will find the `Create` method at build time, and the static
registrar will generate code that looks up the method at runtime using the
metadata tokens instead using the attribute and reflection (this is much
faster, and also allows the linker to remove the corresponding runtime code,
making the app smaller).

If mmp/mtouch is unable to find the `Create` method, then a MT4174/MM4174
warning will be shown, and the lookup will be performed at runtime instead.
The most probable cause is manually written binding code without the required
`[BlockProxy]` attributes.

This optimization requires the static registrar to be enabled.

It is always enabled by default (as long as the static registrar is enabled).

The default behavior can be overridden by passing
`--optimize=[+|-]static-delegate-to-block-lookup` to mtouch/mmp.

## Remove unsupported IL for bitcode

Removes unsupported IL for bitcode, and replaces it with a `NotSupportedException`.

There are certain types of IL that Xamarin.iOS doesn't support when compiling
to bitcode. This optimization will replace the unsupported IL with an
`NotSupportedException`, and will emit a warning at build time.

This ensures that any unsupported IL will be detected at runtime even when not
compiling to bitcode (in particular it will mean that the behavior between
Debug and Release device builds is identical, since Debug builds do not
compile to bitcode, while Release builds do).

This optimization will change the following code:

```csharp
void Method ()
{
	try {
		throw new Exception ("FilterMe");
	} catch (Exception e) when (e.Message == "FilterMe") {
		Console.WriteLine ("filtered");
	}
}
```

into the following:

```csharp
void Method ()
{
	throw new NotSupportedException ("This method contains IL not supported when compiled to bitcode.");
}
```

This optimization does not require the linker to be enabled, it will process
all assemblies, even those not linked.

It is only applicable to watchOS, and then it's enabled by default when
building for device.

The default behavior can be overridden by passing
`--optimize=[+|-]remove-unsupported-il-for-bitcode` to mtouch/mmp.

## Inline Runtime.IsARM64CallingConvention

Inlines the value of `Runtime.IsARM64CallingConvention` as determined at
build time.

It's usually possible to determine at build time if we'll be running on an
ARM64 cpu at runtime, and in that case we can inline the value of this
property to a constant `true` or `false` value.

This optimization will change the following type of code:

```csharp
if (Runtime.IsARM64CallingConvention) {
	Console.WriteLine ("Running on ARM64");
} else {
	Console.WriteLine ("Not running on ARM64");
}
```

into the following when running on ARM64:

```csharp
if (true) {
	Console.WriteLine ("Running on ARM64");
} else {
	Console.WriteLine ("Not running on ARM64");
}
```

or into the following when not running on ARM64:

```csharp
if (false) {
	Console.WriteLine ("Running on ARM64");
} else {
	Console.WriteLine ("Not running on ARM64");
}

```

This optimization requires the linker to be enabled, and is only applied to
methods with the `[BindingImpl (BindingImplOptions.Optimizable)]` attribute.

It is always enabled by default (when the linker is enabled).

The default behavior can be overridden by passing
`--optimize=[+|-]inline-is-arm64-calling-convention` to mtouch/mmp.

## Seal and Devirtualize

This optimization requires the linker to be enabled and is applied globally
on all code inside the application.

When the linker _knows_ all the code inside an application it can do global
optimization like:
* seal types (if not subclassed);
* mark method as final (if not overriden in subclasses); and
* devirtualize methods (if never overriden)

The changes allow the AOT compiler to apply further optimizations when 
generating native code.

This optimization is not safe when dynamically loading code. As such it does
not exist for Xamarin.Mac. For Xamarin.iOS it is enabled, by default,
unless the interpreter is used.

The default behavior can be overridden by passing
`--optimize=[+|-]seal-and-devirtualize` to `mtouch`.

## Static constructors for BeforeFieldInit removal

This optimization requires the linker to be enabled and is applied globally
on all code inside the application.

This optimization allows the linker not to mark every `.cctor` when a type
is preserved, e.g. whenever the class/static constructor `.cctor` is only
used for field initialization and those fields are not marked themselves 
then it is possible to remove the `.cctor`.

This optimization is enabled, by default, on both Xamarin.iOS and 
Xamarin.Mac. However it represent a change from older versions of the linker. 
It is possible that some existing code depend on this side effect (i.e. 
those `.cctor` not being removed). In such case the optimization can be 
disabled  until the correct linker annotations (e.g. 
`[Preserve (Conditional=true)]`) are added.

The default behavior can be overridden by passing
`--optimize=[+|-]cctor-beforefieldinit` to `mtouch` or `mmp`.

## Custom Attributes Removal

This optimization requires the linker to be enabled and is applied globally
on all assemblies inside the application.

This optimization removes a number of, rarely used, custom attributes from
assemblies. In turn this allows the linker to later remove the associated
code from the base class libraries (BCL). This help reduce both the
metadata and code size for your application.

This optimization is enabled, by default, on both Xamarin.iOS and
Xamarin.Mac. If some of the removed custom attributes are required for
your application you can disable this optimization, without totally
disabling the managed linker.

The default behavior can be overridden by passing
`--optimize=[+|-]custom-attributes-removal` to `mtouch` or `mmp`.

## Force Rejected Types Removal

This optimization can be enabled when it's not possible to use the 
managed linker (e.g. **Don't link**) or when the managed linker cannot
remove references to deprecated types that would cause an application
to be rejected by Apple.

References to the existing types will be renamed, e.g. `UIWebView` to 
`DeprecatedWebView`, in every assemblies.

The type definition is also renamed (for validity) and all custom 
attributes on the types and their members will be removed. 
Code inside the members will be replaced with a
`throw new NotSupportedException ();`.

The default behavior can be overridden by passing
`--optimize=[+|-]force-rejected-types-removal` to `mtouch`.

The exact list of types might change over time and is best read directly from
the [source code](https://github.com/dotnet/macios/blob/main/tools/linker/RemoveRejectedTypesStep.cs).
