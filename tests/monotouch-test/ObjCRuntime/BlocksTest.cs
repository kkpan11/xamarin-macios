//
// Unit tests for Blocks
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.ObjCRuntime {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class BlocksTest {
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static IntPtr objc_getClass (string name);

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern IntPtr imp_implementationWithBlock (ref BlockLiteral block);

		[DllImport ("/usr/lib/libobjc.dylib")]
		static extern bool class_addMethod (IntPtr cls, IntPtr name, IntPtr imp, string types);

		[Test]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe void SignatureA ()
		{
			delegate* unmanaged<IntPtr, byte, void> trampoline = &SignatureTestA;
			using var block = new BlockLiteral (trampoline, null, typeof (BlocksTest), nameof (SignatureTestA));
#if __MACOS__
			var boolIsB = Runtime.IsARM64CallingConvention;
#elif __MACCATALYST__
			var boolIsB = Runtime.IsARM64CallingConvention;
#else
			var boolIsB = true;
#endif
			Assert.AreEqual (boolIsB ? "v@?B" : "v@?c", GetBlockSignature (&block), $"Signature ARM64: {Runtime.IsARM64CallingConvention}");
		}

		[Test]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe void SignatureB ()
		{
			delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &SignatureTestB;
			using var block = new BlockLiteral (trampoline, null, typeof (BlocksTest), nameof (SignatureTestB));
			Assert.AreEqual ("v@?@", GetBlockSignature (&block), "Signature");
		}

		[Test]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe void SignatureC ()
		{
			delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &SignatureTestC;
			using var block = new BlockLiteral (trampoline, null, typeof (BlocksTest), nameof (SignatureTestC));
			// This is the wrong signature, but the registrar has no way of figuring out the correct
			// one without the UserDelegateType attribute on the target method.
			Assert.AreEqual ("v@?^v^v", GetBlockSignature (&block), "Signature");
		}

		[UserDelegateType (typeof (Action<bool>))]
		[UnmanagedCallersOnly]
		static void SignatureTestA (IntPtr block, byte value)
		{
		}

		[UserDelegateType (typeof (Action<NSError>))]
		[UnmanagedCallersOnly]
		static void SignatureTestB (IntPtr block, IntPtr value)
		{
		}

		[UnmanagedCallersOnly]
		static void SignatureTestC (IntPtr block, IntPtr value)
		{
		}

#pragma warning disable 649
		[StructLayout (LayoutKind.Sequential)]
		struct TestBlockDescriptor {
			public IntPtr reserved;
			public IntPtr size;
			public IntPtr copy_helper;
			public IntPtr dispose;
			public IntPtr signature;
		}
		[StructLayout (LayoutKind.Sequential)]
		unsafe struct TestBlockLiteral {
			IntPtr isa;
			int flags;
			int reserved;
			IntPtr invoke;
			public TestBlockDescriptor* block_descriptor;
		}
#pragma warning restore 649

		static unsafe string GetBlockSignature (BlockLiteral* block)
		{
			var test_block = (TestBlockLiteral*) block;
			var signatureUtf8Ptr = test_block->block_descriptor->signature;
			var signature = Marshal.PtrToStringAuto (signatureUtf8Ptr);
			return signature;
		}

		[Test]
		public void TestSetupBlock ()
		{
			if (!Runtime.DynamicRegistrationSupported)
				Assert.Ignore ("This test requires the dynamic registrar to be available.");

			using (var obj = new TestClass ()) {
				TestClass.OnCallback = ((IntPtr blockArgument, NativeHandle self, IntPtr argument) => {
					Assert.AreNotEqual (IntPtr.Zero, blockArgument, "block");
					Assert.AreEqual (obj.Handle, self, "self");
					Assert.AreEqual (argument, (IntPtr) 0x12345678, "argument");
				});
				Messaging.void_objc_msgSend_IntPtr (obj.Handle, Selector.GetHandle ("testBlocks:"), (IntPtr) 0x12345678);
			}
		}

		class TestClass : NSObject {
			[MonoPInvokeCallback (typeof (TestBlockCallbackDelegate))]
			static void TestBlockCallback (IntPtr block, NativeHandle self, IntPtr argument)
			{
				OnCallback (block, self, argument);
			}

			static TestBlockCallbackDelegate callback = new TestBlockCallbackDelegate (TestBlockCallback);

			public delegate void TestBlockCallbackDelegate (IntPtr block, NativeHandle self, IntPtr argument);
			public static TestBlockCallbackDelegate OnCallback;

			static TestClass ()
			{
				var cls = Class.GetHandle (typeof (TestClass));
				var block = new BlockLiteral ();
				block.SetupBlock (callback, null);
				var imp = imp_implementationWithBlock (ref block);
				class_addMethod (cls, Selector.GetHandle ("testBlocks:"), imp, "v@:^v");
			}
		}

#if !DEVICE && !MONOMAC && !AOT && !__MACCATALYST__ // some of these tests cause the AOT compiler to assert
		// No MonoPInvokeCallback
		static void InvalidTrampoline1 () { }

		// Wrong delegate signature in MonoPInvokeCallback
		[MonoPInvokeCallback (typeof (Action<IntPtr>))]
		static void InvalidTrampoline2 () { }

		// Wrong delegate signature in MonoPInvokeCallback
		[MonoPInvokeCallback (typeof (Func<IntPtr>))]
		static void InvalidTrampoline3 () { }

		// Wrong delegate signature in MonoPInvokeCallback
		[MonoPInvokeCallback (typeof (Action<IntPtr>))]
		static void InvalidTrampoline4 (int x) { }

		// Wrong delegate signature in MonoPInvokeCallback
		[MonoPInvokeCallback (typeof (Func<IntPtr>))]
		static int InvalidTrampoline5 () { return 0; }
#endif // !DEVICE

		[Test]
		public void InvalidBlockTrampolines ()
		{
			BlockLiteral block = new BlockLiteral ();
			Action userDelegate = new Action (() => Console.WriteLine ("Hello world!"));

			Assert.Throws<ArgumentNullException> (() => block.SetupBlock (null, userDelegate), "null trampoline");

#if !DEVICE && !MONOMAC && !AOT && !__MACCATALYST__
			if (Runtime.Arch == Arch.SIMULATOR) {
				// These checks only occur in the simulator
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Action) InvalidBlockTrampolines, userDelegate), "instance trampoline");
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Action) InvalidTrampoline1, userDelegate), "invalid trampoline 1");
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Action) InvalidTrampoline2, userDelegate), "invalid trampoline 2");
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Action) InvalidTrampoline3, userDelegate), "invalid trampoline 3");
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Action<int>) InvalidTrampoline4, userDelegate), "invalid trampoline 4");
				Assert.Throws<ArgumentException> (() => block.SetupBlock ((Func<int>) InvalidTrampoline5, userDelegate), "invalid trampoline 5");
			}
#endif // !DEVICE
		}
	}
}
