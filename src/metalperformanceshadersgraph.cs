using System;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using Metal;
using MetalPerformanceShaders;
using ObjCRuntime;

using MPSGraphTensorDataDictionary = Foundation.NSDictionary<MetalPerformanceShadersGraph.MPSGraphTensor, MetalPerformanceShadersGraph.MPSGraphTensorData>;
using MPSGraphTensorShapedTypeDictionary = Foundation.NSDictionary<MetalPerformanceShadersGraph.MPSGraphTensor, MetalPerformanceShadersGraph.MPSGraphShapedType>;

namespace MetalPerformanceShadersGraph {
	// MPSGraph.h

	[BaseType (typeof (NSObject))]
	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	interface MPSGraphObject {
	}

	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraph {
		[Static, Export ("new")]
		[return: Release]
		MPSGraph Create ();

		// @property (readwrite, atomic) MPSGraphOptions options;
		[Export ("options", ArgumentSemantic.Assign)]
		MPSGraphOptions Options { get; set; }

		// @property (readonly, nonatomic) NSArray<MPSGraphTensor *> * _Nonnull placeholderTensors;
		[Export ("placeholderTensors")]
		MPSGraphTensor [] PlaceholderTensors { get; }

		// -(MPSGraphExecutable * _Nonnull)compileWithDevice:(MPSGraphDevice * _Nullable)device feeds:(MPSGraphTensorShapedTypeDictionary * _Nonnull)feeds targetTensors:(NSArray<MPSGraphTensor *> * _Nonnull)targetTensors targetOperations:(NSArray<MPSGraphOperation *> * _Nullable)targetOperations compilationDescriptor:(MPSGraphCompilationDescriptor * _Nullable)compilationDescriptor __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("compileWithDevice:feeds:targetTensors:targetOperations:compilationDescriptor:")]
		MPSGraphExecutable Compile ([NullAllowed] MPSGraphDevice device, NSDictionary<MPSGraphTensor, MPSGraphShapedType> feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations, [NullAllowed] MPSGraphCompilationDescriptor compilationDescriptor);

		[Export ("runWithFeeds:targetTensors:targetOperations:")]
		MPSGraphTensorDataDictionary Run (MPSGraphTensorDataDictionary feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations);

		[Export ("runWithMTLCommandQueue:feeds:targetTensors:targetOperations:")]
		MPSGraphTensorDataDictionary Run (IMTLCommandQueue commandQueue, MPSGraphTensorDataDictionary feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations);

		[Export ("runWithMTLCommandQueue:feeds:targetOperations:resultsDictionary:")]
		void Run (IMTLCommandQueue commandQueue, MPSGraphTensorDataDictionary feeds, [NullAllowed] MPSGraphOperation [] targetOperations, MPSGraphTensorDataDictionary resultsDictionary);

		[Export ("runAsyncWithFeeds:targetTensors:targetOperations:executionDescriptor:")]
		MPSGraphTensorDataDictionary RunAsync (MPSGraphTensorDataDictionary feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations, [NullAllowed] MPSGraphExecutionDescriptor executionDescriptor);

		[Export ("runAsyncWithMTLCommandQueue:feeds:targetTensors:targetOperations:executionDescriptor:")]
		MPSGraphTensorDataDictionary RunAsync (IMTLCommandQueue commandQueue, MPSGraphTensorDataDictionary feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations, [NullAllowed] MPSGraphExecutionDescriptor executionDescriptor);

		[Export ("runAsyncWithMTLCommandQueue:feeds:targetOperations:resultsDictionary:executionDescriptor:")]
		void RunAsync (IMTLCommandQueue commandQueue, MPSGraphTensorDataDictionary feeds, [NullAllowed] MPSGraphOperation [] targetOperations, MPSGraphTensorDataDictionary resultsDictionary, [NullAllowed] MPSGraphExecutionDescriptor executionDescriptor);

		[Export ("encodeToCommandBuffer:feeds:targetTensors:targetOperations:executionDescriptor:")]
		MPSGraphTensorDataDictionary Encode (MPSCommandBuffer commandBuffer, MPSGraphTensorDataDictionary feeds, MPSGraphTensor [] targetTensors, [NullAllowed] MPSGraphOperation [] targetOperations, [NullAllowed] MPSGraphExecutionDescriptor executionDescriptor);

		[Export ("encodeToCommandBuffer:feeds:targetOperations:resultsDictionary:executionDescriptor:")]
		void Encode (MPSCommandBuffer commandBuffer, MPSGraphTensorDataDictionary feeds, [NullAllowed] MPSGraphOperation [] targetOperations, MPSGraphTensorDataDictionary resultsDictionary, [NullAllowed] MPSGraphExecutionDescriptor executionDescriptor);
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_CallOp {
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("callSymbolName:inputTensors:outputTypes:name:")]
		MPSGraphTensor [] Call (string symbolName, MPSGraphTensor [] inputTensors, MPSGraphType [] outputTypes, [NullAllowed] string name);
	}

	// @interface MPSGraphGradientOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphGradientOps {
		// -(NSDictionary<MPSGraphTensor *,MPSGraphTensor *> * _Nonnull)gradientForPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor withTensors:(NSArray<MPSGraphTensor *> * _Nonnull)tensors name:(NSString * _Nullable)name __attribute__((swift_name("gradients(of:with:name:)")));
		[Export ("gradientForPrimaryTensor:withTensors:name:")]
		NSDictionary<MPSGraphTensor, MPSGraphTensor> Gradients (MPSGraphTensor of, MPSGraphTensor [] with, [NullAllowed] string name);
	}

	// @interface MPSGraphActivationOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphActivationOps {
		// -(MPSGraphTensor * _Nonnull)reLUWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("reLUWithTensor:name:")]
		MPSGraphTensor ReLU (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reLUGradientWithIncomingGradient:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source name:(NSString * _Nullable)name;
		[Export ("reLUGradientWithIncomingGradient:sourceTensor:name:")]
		MPSGraphTensor ReLUGradient (MPSGraphTensor gradient, MPSGraphTensor source, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sigmoidWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("sigmoidWithTensor:name:")]
		MPSGraphTensor Sigmoid (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sigmoidGradientWithIncomingGradient:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source name:(NSString * _Nullable)name;
		[Export ("sigmoidGradientWithIncomingGradient:sourceTensor:name:")]
		MPSGraphTensor SigmoidGradient (MPSGraphTensor gradient, MPSGraphTensor source, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)softMaxWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("softMaxWithTensor:axis:name:")]
		MPSGraphTensor SoftMax (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)softMaxGradientWithIncomingGradient:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("softMaxGradientWithIncomingGradient:sourceTensor:axis:name:")]
		MPSGraphTensor SoftMaxGradient (MPSGraphTensor gradient, MPSGraphTensor source, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)leakyReLUWithTensor:(MPSGraphTensor * _Nonnull)tensor alpha:(double)alpha name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("leakyReLUWithTensor:alpha:name:")]
		MPSGraphTensor LeakyReLU (MPSGraphTensor tensor, double alpha, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)leakyReLUWithTensor:(MPSGraphTensor * _Nonnull)tensor alphaTensor:(MPSGraphTensor * _Nonnull)alphaTensor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("leakyReLUWithTensor:alphaTensor:name:")]
		MPSGraphTensor LeakyReLU (MPSGraphTensor tensor, MPSGraphTensor alphaTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)leakyReLUGradientWithIncomingGradient:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source alphaTensor:(MPSGraphTensor * _Nonnull)alphaTensor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("leakyReLUGradientWithIncomingGradient:sourceTensor:alphaTensor:name:")]
		MPSGraphTensor LeakyReLUGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor alphaTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphArithmeticOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphArithmeticOps {
		// -(MPSGraphTensor * _Nonnull)identityWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("identityWithTensor:name:")]
		MPSGraphTensor Identity (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)exponentWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("exponentWithTensor:name:")]
		MPSGraphTensor Exponent (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)exponentBase2WithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("exponentBase2WithTensor:name:")]
		MPSGraphTensor ExponentBase2 (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)exponentBase10WithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("exponentBase10WithTensor:name:")]
		MPSGraphTensor ExponentBase10 (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logarithmWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("logarithmWithTensor:name:")]
		MPSGraphTensor Logarithm (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logarithmBase2WithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("logarithmBase2WithTensor:name:")]
		MPSGraphTensor LogarithmBase2 (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logarithmBase10WithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("logarithmBase10WithTensor:name:")]
		MPSGraphTensor LogarithmBase10 (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)squareWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("squareWithTensor:name:")]
		MPSGraphTensor Square (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)squareRootWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("squareRootWithTensor:name:")]
		MPSGraphTensor SquareRoot (MPSGraphTensor tensor, [NullAllowed] string name);

		[iOS (18, 0), MacCatalyst (18, 0), TV (18, 0), Mac (15, 0)]
		[Export ("reciprocalSquareRootWithTensor:name:")]
		MPSGraphTensor ReciprocalSquareRoot (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reverseSquareRootWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'ReciprocalSquareRoot' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'ReciprocalSquareRoot' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'ReciprocalSquareRoot' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'ReciprocalSquareRoot' instead.")]
		[Export ("reverseSquareRootWithTensor:name:")]
		MPSGraphTensor ReverseSquareRoot (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reciprocalWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("reciprocalWithTensor:name:")]
		MPSGraphTensor Reciprocal (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)absoluteWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("absoluteWithTensor:name:")]
		MPSGraphTensor Absolute (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("absoluteSquareWithTensor:name:")]
		MPSGraphTensor AbsoluteSquare (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)negativeWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("negativeWithTensor:name:")]
		MPSGraphTensor Negative (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)signWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("signWithTensor:name:")]
		MPSGraphTensor Sign (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)signbitWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("signbitWithTensor:name:")]
		MPSGraphTensor Signbit (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)ceilWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("ceilWithTensor:name:")]
		MPSGraphTensor Ceil (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)floorWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("floorWithTensor:name:")]
		MPSGraphTensor Floor (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)roundWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("roundWithTensor:name:")]
		MPSGraphTensor Round (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)rintWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("rintWithTensor:name:")]
		MPSGraphTensor Rint (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sinWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("sinWithTensor:name:")]
		MPSGraphTensor Sin (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)cosWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("cosWithTensor:name:")]
		MPSGraphTensor Cos (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)tanWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("tanWithTensor:name:")]
		MPSGraphTensor Tan (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sinhWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("sinhWithTensor:name:")]
		MPSGraphTensor Sinh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)coshWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("coshWithTensor:name:")]
		MPSGraphTensor Cosh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)tanhWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("tanhWithTensor:name:")]
		MPSGraphTensor Tanh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)asinWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("asinWithTensor:name:")]
		MPSGraphTensor Asin (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)acosWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("acosWithTensor:name:")]
		MPSGraphTensor Acos (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)atanWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("atanWithTensor:name:")]
		MPSGraphTensor Atan (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)asinhWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("asinhWithTensor:name:")]
		MPSGraphTensor Asinh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)acoshWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("acoshWithTensor:name:")]
		MPSGraphTensor Acosh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)atanhWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("atanhWithTensor:name:")]
		MPSGraphTensor Atanh (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)notWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("notWithTensor:name:")]
		MPSGraphTensor Not (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)isInfiniteWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("isInfiniteWithTensor:name:")]
		MPSGraphTensor IsInfinite (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)isFiniteWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("isFiniteWithTensor:name:")]
		MPSGraphTensor IsFinite (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)isNaNWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("isNaNWithTensor:name:")]
		MPSGraphTensor IsNaN (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)erfWithTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name;
		[Export ("erfWithTensor:name:")]
		MPSGraphTensor Erf (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("truncateWithTensor:name:")]
		MPSGraphTensor Truncate (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseNOTWithTensor:name:")]
		MPSGraphTensor BitwiseNot (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwisePopulationCountWithTensor:name:")]
		MPSGraphTensor BitwisePopulationCount (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("conjugateWithTensor:name:")]
		MPSGraphTensor Conjugate (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)additionWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("addition(_:_:name:)")));
		[Export ("additionWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Addition (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)subtractionWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("subtraction(_:_:name:)")));
		[Export ("subtractionWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Subtraction (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)multiplicationWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("multiplication(_:_:name:)")));
		[Export ("multiplicationWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Multiplication (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)divisionWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("division(_:_:name:)")));
		[Export ("divisionWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Division (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)moduloWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("modulo(_:_:name:)")));
		[Export ("moduloWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Modulo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)powerWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("power(_:_:name:)")));
		[Export ("powerWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Power (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)minimumWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("minimum(_:_:name:)")));
		[Export ("minimumWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Minimum (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)maximumWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("maximum(_:_:name:)")));
		[Export ("maximumWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Maximum (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)minimumWithNaNPropagationWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("minimumWithNaNPropagation(_:_:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("minimumWithNaNPropagationWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor MinimumWithNaNPropagation (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)maximumWithNaNPropagationWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("maximumWithNaNPropagation(_:_:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("maximumWithNaNPropagationWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor MaximumWithNaNPropagation (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)equalWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("equal(_:_:name:)")));
		[Export ("equalWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor EqualTo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)notEqualWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("notEqual(_:_:name:)")));
		[Export ("notEqualWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor NotEqualTo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)lessThanWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("lessThan(_:_:name:)")));
		[Export ("lessThanWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LessThan (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)lessThanOrEqualToWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("lessThanOrEqualTo(_:_:name:)")));
		[Export ("lessThanOrEqualToWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LessThanOrEqualTo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)greaterThanWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("greaterThan(_:_:name:)")));
		[Export ("greaterThanWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor GreaterThan (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)greaterThanOrEqualToWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("greaterThanOrEqualTo(_:_:name:)")));
		[Export ("greaterThanOrEqualToWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor GreaterThanOrEqualTo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalANDWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalAND(_:_:name:)")));
		[Export ("logicalANDWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalAnd (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalORWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalOR(_:_:name:)")));
		[Export ("logicalORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalOr (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalNANDWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalNAND(_:_:name:)")));
		[Export ("logicalNANDWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalNand (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalNORWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalNOR(_:_:name:)")));
		[Export ("logicalNORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalNor (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalXORWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalXOR(_:_:name:)")));
		[Export ("logicalXORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalXor (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)logicalXNORWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("logicalXNOR(_:_:name:)")));
		[Export ("logicalXNORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor LogicalXnor (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)atan2WithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name;
		[Export ("atan2WithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor Atan2 (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseANDWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor BitwiseAnd (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor BitwiseOr (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseXORWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor BitwiseXor (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseLeftShiftWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor BitwiseLeftShift (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("bitwiseRightShiftWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor BitwiseRightShift (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)selectWithPredicateTensor:(MPSGraphTensor * _Nonnull)predicateTensor truePredicateTensor:(MPSGraphTensor * _Nonnull)truePredicateTensor falsePredicateTensor:(MPSGraphTensor * _Nonnull)falseSelectTensor name:(NSString * _Nullable)name __attribute__((swift_name("select(predicate:trueTensor:falseTensor:name:)")));
		[Export ("selectWithPredicateTensor:truePredicateTensor:falsePredicateTensor:name:")]
		MPSGraphTensor Select (MPSGraphTensor predicateTensor, MPSGraphTensor truePredicateTensor, MPSGraphTensor falseSelectTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)clampWithTensor:(MPSGraphTensor * _Nonnull)tensor minValueTensor:(MPSGraphTensor * _Nonnull)minValueTensor maxValueTensor:(MPSGraphTensor * _Nonnull)maxValueTensor name:(NSString * _Nullable)name __attribute__((swift_name("clamp(_:min:max:name:)")));
		[Export ("clampWithTensor:minValueTensor:maxValueTensor:name:")]
		MPSGraphTensor Clamp (MPSGraphTensor tensor, MPSGraphTensor minValueTensor, MPSGraphTensor maxValueTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)divisionNoNaNWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("divisionNoNaN(_:_:name:)")));
		[Export ("divisionNoNaNWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor DivisionNoNaN (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)floorModuloWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("floorModulo(_:_:name:)")));
		[Export ("floorModuloWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor FloorModulo (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("realPartOfTensor:name:")]
		MPSGraphTensor RealPart (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("imaginaryPartOfTensor:name:")]
		MPSGraphTensor ImaginaryPart (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("complexTensorWithRealTensor:imaginaryTensor:name:")]
		MPSGraphTensor ComplexTensor (MPSGraphTensor realTensor, MPSGraphTensor imaginaryTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphConvolution2DOpDescriptor : NSObject <NSCopying>
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphConvolution2DOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) NSUInteger strideInX;
		[Export ("strideInX")]
		nuint StrideInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger strideInY;
		[Export ("strideInY")]
		nuint StrideInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInX;
		[Export ("dilationRateInX")]
		nuint DilationRateInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInY;
		[Export ("dilationRateInY")]
		nuint DilationRateInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingLeft;
		[Export ("paddingLeft")]
		nuint PaddingLeft { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingRight;
		[Export ("paddingRight")]
		nuint PaddingRight { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingTop;
		[Export ("paddingTop")]
		nuint PaddingTop { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingBottom;
		[Export ("paddingBottom")]
		nuint PaddingBottom { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) MPSGraphTensorNamedDataLayout dataLayout;
		[Export ("dataLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout DataLayout { get; set; }

		// @property (readwrite, nonatomic) MPSGraphTensorNamedDataLayout weightsLayout;
		[Export ("weightsLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout WeightsLayout { get; set; }

		// @property (readwrite, nonatomic) NSUInteger groups;
		[Export ("groups")]
		nuint Groups { get; set; }

		// +(instancetype _Nullable)descriptorWithStrideInX:(NSUInteger)strideInX strideInY:(NSUInteger)strideInY dilationRateInX:(NSUInteger)dilationRateInX dilationRateInY:(NSUInteger)dilationRateInY groups:(NSUInteger)groups paddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom paddingStyle:(MPSGraphPaddingStyle)paddingStyle dataLayout:(MPSGraphTensorNamedDataLayout)dataLayout weightsLayout:(MPSGraphTensorNamedDataLayout)weightsLayout;
		[Static]
		[Export ("descriptorWithStrideInX:strideInY:dilationRateInX:dilationRateInY:groups:paddingLeft:paddingRight:paddingTop:paddingBottom:paddingStyle:dataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphConvolution2DOpDescriptor Create (nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, nuint groups, nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		// +(instancetype _Nullable)descriptorWithStrideInX:(NSUInteger)strideInX strideInY:(NSUInteger)strideInY dilationRateInX:(NSUInteger)dilationRateInX dilationRateInY:(NSUInteger)dilationRateInY groups:(NSUInteger)groups paddingStyle:(MPSGraphPaddingStyle)paddingStyle dataLayout:(MPSGraphTensorNamedDataLayout)dataLayout weightsLayout:(MPSGraphTensorNamedDataLayout)weightsLayout;
		[Static]
		[Export ("descriptorWithStrideInX:strideInY:dilationRateInX:dilationRateInY:groups:paddingStyle:dataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphConvolution2DOpDescriptor Create (nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, nuint groups, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		// -(void)setExplicitPaddingWithPaddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom;
		[Export ("setExplicitPaddingWithPaddingLeft:paddingRight:paddingTop:paddingBottom:")]
		void SetExplicitPadding (nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom);
	}

	// @interface MPSGraphConvolutionOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphConvolutionOps {
		// -(MPSGraphTensor * _Nonnull)convolution2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights descriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolution2D(_:weights:descriptor:name:)")));
		[Export ("convolution2DWithSourceTensor:weightsTensor:descriptor:name:")]
		MPSGraphTensor Convolution2D (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolution2DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShape:(MPSShape * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolution2DDataGradient(_:weights:outputShape:forwardConvolutionDescriptor:name:)")));
		[Export ("convolution2DDataGradientWithIncomingGradientTensor:weightsTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution2DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolution2DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)gradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShapeTensor:(MPSGraphTensor * _Nonnull)outputShapeTensor forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("convolution2DDataGradient(_:weights:outputShapeTensor:forwardConvolutionDescriptor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("convolution2DDataGradientWithIncomingGradientTensor:weightsTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution2DDataGradient (MPSGraphTensor gradient, MPSGraphTensor weights, MPSGraphTensor outputShapeTensor, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolution2DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient sourceTensor:(MPSGraphTensor * _Nonnull)source outputShape:(MPSShape * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolution2DWeightsGradient(_:source:outputShape:forwardConvolutionDescriptor:name:)")));
		[Export ("convolution2DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution2DWeightsGradient (MPSGraphTensor incomingGradient, MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolution2DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source outputShapeTensor:(MPSGraphTensor * _Nonnull)outputShapeTensor forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("convolution2DWeightsGradient(_:source:outputShapeTensor:forwardConvolutionDescriptor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("convolution2DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution2DWeightsGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor outputShapeTensor, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("convolution3DWithSourceTensor:weightsTensor:descriptor:name:")]
		MPSGraphTensor Convolution3D (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphConvolution3DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("convolution3DDataGradientWithIncomingGradientTensor:weightsTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution3DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution3DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("convolution3DDataGradientWithIncomingGradientTensor:weightsTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution3DDataGradient (MPSGraphTensor gradient, MPSGraphTensor weights, MPSGraphTensor outputShapeTensor, MPSGraphConvolution3DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("convolution3DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution3DWeightsGradient (MPSGraphTensor incomingGradient, MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution3DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("convolution3DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor Convolution3DWeightsGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor outputShapeTensor, MPSGraphConvolution3DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphConvolutionTransposeOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphConvolutionTransposeOps {
		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShape:(MPSShape * _Nonnull)outputShape descriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolutionTranspose2D(_:weights:outputShape:descriptor:name:)")));
		[Export ("convolutionTranspose2DWithSourceTensor:weightsTensor:outputShape:descriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2D (MPSGraphTensor source, MPSGraphTensor weights, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShapeTensor:(MPSGraphTensor * _Nonnull)outputShape descriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("convolutionTranspose2D(_:weights:outputShapeTensor:descriptor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("convolutionTranspose2DWithSourceTensor:weightsTensor:outputShapeTensor:descriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2D (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphTensor outputShape, MPSGraphConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShape:(MPSShape * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolutionTranspose2DDataGradient(_:weights:outputShape:forwardConvolutionDescriptor:name:)")));
		[Export ("convolutionTranspose2DDataGradientWithIncomingGradientTensor:weightsTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShapeTensor:(MPSGraphTensor * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("convolutionTranspose2DDataGradient(_:weights:outputShapeTensor:forwardConvolutionDescriptor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("convolutionTranspose2DDataGradientWithIncomingGradientTensor:weightsTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, MPSGraphTensor outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)source outputShape:(MPSShape * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((swift_name("convolutionTranspose2DWeightsGradient(_:weights:outputShape:forwardConvolutionDescriptor:name:)")));
		[Export ("convolutionTranspose2DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShape:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2DWeightsGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)convolutionTranspose2DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)source outputShapeTensor:(MPSGraphTensor * _Nonnull)outputShape forwardConvolutionDescriptor:(MPSGraphConvolution2DOpDescriptor * _Nonnull)forwardConvolutionDescriptor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("convolutionTranspose2DWeightsGradient(_:weights:outputShapeTensor:forwardConvolutionDescriptor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("convolutionTranspose2DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShapeTensor:forwardConvolutionDescriptor:name:")]
		MPSGraphTensor ConvolutionTranspose2DWeightsGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor source, MPSGraphTensor outputShape, MPSGraphConvolution2DOpDescriptor forwardConvolutionDescriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphControlFlowOps (MPSGraph)
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphControlFlowOps {
		// -(NSArray<MPSGraphTensor *> * _Nonnull)controlDependencyWithOperations:(NSArray<MPSGraphOperation *> * _Nonnull)operations dependentBlock:(MPSGraphControlFlowDependencyBlock _Nonnull)dependentBlock name:(NSString * _Nullable)name;
		[Export ("controlDependencyWithOperations:dependentBlock:name:")]
		MPSGraphTensor [] ControlDependency (MPSGraphOperation [] operations, MPSGraphControlFlowDependencyBlock dependentBlock, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)ifWithPredicateTensor:(MPSGraphTensor * _Nonnull)predicateTensor thenBlock:(MPSGraphIfThenElseBlock _Nonnull)thenBlock elseBlock:(MPSGraphIfThenElseBlock _Nullable)elseBlock name:(NSString * _Nullable)name __attribute__((swift_name("if(_:then:else:name:)")));
		[Export ("ifWithPredicateTensor:thenBlock:elseBlock:name:")]
		MPSGraphTensor [] If (MPSGraphTensor predicateTensor, MPSGraphIfThenElseBlock thenBlock, [NullAllowed] MPSGraphIfThenElseBlock elseBlock, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)whileWithInitialInputs:(NSArray<MPSGraphTensor *> * _Nonnull)initialInputs before:(MPSGraphWhileBeforeBlock _Nonnull)before after:(MPSGraphWhileAfterBlock _Nonnull)after name:(NSString * _Nullable)name __attribute__((swift_name("while(initialInputs:before:after:name:)")));
		[Export ("whileWithInitialInputs:before:after:name:")]
		MPSGraphTensor [] While (MPSGraphTensor [] initialInputs, MPSGraphWhileBeforeBlock before, MPSGraphWhileAfterBlock after, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)forLoopWithLowerBound:(MPSGraphTensor * _Nonnull)lowerBound upperBound:(MPSGraphTensor * _Nonnull)upperBound step:(MPSGraphTensor * _Nonnull)step initialBodyArguments:(NSArray<MPSGraphTensor *> * _Nonnull)initialBodyArguments body:(MPSGraphForLoopBodyBlock _Nonnull)body name:(NSString * _Nullable)name __attribute__((swift_name("for(lowerBound:upperBound:step:initialBodyArguments:body:name:)")));
		[Export ("forLoopWithLowerBound:upperBound:step:initialBodyArguments:body:name:")]
		MPSGraphTensor [] For (MPSGraphTensor lowerBound, MPSGraphTensor upperBound, MPSGraphTensor step, MPSGraphTensor [] initialBodyArguments, MPSGraphForLoopBodyBlock body, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)forLoopWithNumberOfIterations:(MPSGraphTensor * _Nonnull)numberOfIterations initialBodyArguments:(NSArray<MPSGraphTensor *> * _Nonnull)initialBodyArguments body:(MPSGraphForLoopBodyBlock _Nonnull)body name:(NSString * _Nullable)name __attribute__((swift_name("for(numberOfIterations:initialBodyArguments:body:name:)")));
		[Export ("forLoopWithNumberOfIterations:initialBodyArguments:body:name:")]
		MPSGraphTensor [] For (MPSGraphTensor numberOfIterations, MPSGraphTensor [] initialBodyArguments, MPSGraphForLoopBodyBlock body, [NullAllowed] string name);
	}

	// typedef NSArray<MPSGraphTensor *> * _Nonnull (^MPSGraphControlFlowDependencyBlock)();
	delegate MPSGraphTensor [] MPSGraphControlFlowDependencyBlock ();

	// typedef NSArray<MPSGraphTensor *> * _Nonnull (^MPSGraphIfThenElseBlock)();
	delegate MPSGraphTensor [] MPSGraphIfThenElseBlock ();

	// typedef MPSGraphTensor * _Nonnull (^MPSGraphWhileBeforeBlock)(NSArray<MPSGraphTensor *> * _Nonnull, NSMutableArray<MPSGraphTensor *> * _Nonnull);
	delegate MPSGraphTensor MPSGraphWhileBeforeBlock (MPSGraphTensor [] inputTensors, NSMutableArray<MPSGraphTensor> resultTensors);

	// typedef NSArray<MPSGraphTensor *> * _Nonnull (^MPSGraphWhileAfterBlock)(NSArray<MPSGraphTensor *> * _Nonnull);
	delegate MPSGraphTensor [] MPSGraphWhileAfterBlock (MPSGraphTensor [] bodyBlockArguments);

	// typedef NSArray<MPSGraphTensor *> * _Nonnull (^MPSGraphForLoopBodyBlock)(MPSGraphTensor * _Nonnull, NSArray<MPSGraphTensor *> * _Nonnull);
	delegate MPSGraphTensor [] MPSGraphForLoopBodyBlock (MPSGraphTensor index, MPSGraphTensor [] iterationArguments);

	// @interface MPSGraphDepthwiseConvolution2DOpDescriptor : NSObject <NSCopying>
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphDepthwiseConvolution2DOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) NSUInteger strideInX;
		[Export ("strideInX")]
		nuint StrideInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger strideInY;
		[Export ("strideInY")]
		nuint StrideInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInX;
		[Export ("dilationRateInX")]
		nuint DilationRateInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInY;
		[Export ("dilationRateInY")]
		nuint DilationRateInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingLeft;
		[Export ("paddingLeft")]
		nuint PaddingLeft { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingRight;
		[Export ("paddingRight")]
		nuint PaddingRight { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingTop;
		[Export ("paddingTop")]
		nuint PaddingTop { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingBottom;
		[Export ("paddingBottom")]
		nuint PaddingBottom { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) MPSGraphTensorNamedDataLayout dataLayout;
		[Export ("dataLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout DataLayout { get; set; }

		// @property (readwrite, nonatomic) MPSGraphTensorNamedDataLayout weightsLayout;
		[Export ("weightsLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout WeightsLayout { get; set; }

		// +(instancetype _Nullable)descriptorWithStrideInX:(NSUInteger)strideInX strideInY:(NSUInteger)strideInY dilationRateInX:(NSUInteger)dilationRateInX dilationRateInY:(NSUInteger)dilationRateInY paddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom paddingStyle:(MPSGraphPaddingStyle)paddingStyle dataLayout:(MPSGraphTensorNamedDataLayout)dataLayout weightsLayout:(MPSGraphTensorNamedDataLayout)weightsLayout;
		[Static]
		[Export ("descriptorWithStrideInX:strideInY:dilationRateInX:dilationRateInY:paddingLeft:paddingRight:paddingTop:paddingBottom:paddingStyle:dataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphDepthwiseConvolution2DOpDescriptor Create (nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		// +(instancetype _Nullable)descriptorWithDataLayout:(MPSGraphTensorNamedDataLayout)dataLayout weightsLayout:(MPSGraphTensorNamedDataLayout)weightsLayout;
		[Static]
		[Export ("descriptorWithDataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphDepthwiseConvolution2DOpDescriptor Create (MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		// -(void)setExplicitPaddingWithPaddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom;
		[Export ("setExplicitPaddingWithPaddingLeft:paddingRight:paddingTop:paddingBottom:")]
		void SetExplicitPadding (nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom);
	}

	// @interface MPSGraphDepthwiseConvolution3DOpDescriptor : NSObject <NSCopying>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphDepthwiseConvolution3DOpDescriptor : NSCopying {
		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull strides;
		[BindAs (typeof (int []))]
		[Export ("strides", ArgumentSemantic.Copy)]
		NSNumber [] Strides { get; set; }

		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull dilationRates;
		[BindAs (typeof (int []))]
		[Export ("dilationRates", ArgumentSemantic.Copy)]
		NSNumber [] DilationRates { get; set; }

		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull paddingValues;
		[BindAs (typeof (int []))]
		[Export ("paddingValues", ArgumentSemantic.Copy)]
		NSNumber [] PaddingValues { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) NSInteger channelDimensionIndex;
		[Export ("channelDimensionIndex")]
		nint ChannelDimensionIndex { get; set; }

		// +(instancetype _Nullable)descriptorWithStrides:(NSArray<NSNumber *> * _Nonnull)strides dilationRates:(NSArray<NSNumber *> * _Nonnull)dilationRates paddingValues:(NSArray<NSNumber *> * _Nonnull)paddingValues paddingStyle:(MPSGraphPaddingStyle)paddingStyle;
		[Static]
		[Export ("descriptorWithStrides:dilationRates:paddingValues:paddingStyle:")]
		[return: NullAllowed]
		MPSGraphDepthwiseConvolution3DOpDescriptor Create ([BindAs (typeof (int []))] NSNumber [] strides, [BindAs (typeof (int []))] NSNumber [] dilationRates, [BindAs (typeof (int []))] NSNumber [] paddingValues, MPSGraphPaddingStyle paddingStyle);

		// +(instancetype _Nullable)descriptorWithPaddingStyle:(MPSGraphPaddingStyle)paddingStyle;
		[Static]
		[Export ("descriptorWithPaddingStyle:")]
		[return: NullAllowed]
		MPSGraphDepthwiseConvolution3DOpDescriptor Create (MPSGraphPaddingStyle paddingStyle);
	}

	// @interface MPSGraphDepthwiseConvolutionOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphDepthwiseConvolutionOps {
		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights descriptor:(MPSGraphDepthwiseConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution2D(_:weights:descriptor:name:)")));
		[Export ("depthwiseConvolution2DWithSourceTensor:weightsTensor:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution2D (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphDepthwiseConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution2DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShape:(MPSShape * _Nonnull)outputShape descriptor:(MPSGraphDepthwiseConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution2DDataGradient(_:weights:outputShape:descriptor:name:)")));
		[Export ("depthwiseConvolution2DDataGradientWithIncomingGradientTensor:weightsTensor:outputShape:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution2DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphDepthwiseConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution2DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient sourceTensor:(MPSGraphTensor * _Nonnull)source outputShape:(MPSShape * _Nonnull)outputShape descriptor:(MPSGraphDepthwiseConvolution2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution2DWeightsGradient(_:source:outputShape:descriptor:name:)")));
		[Export ("depthwiseConvolution2DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShape:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution2DWeightsGradient (MPSGraphTensor incomingGradient, MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphDepthwiseConvolution2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution3DWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights descriptor:(MPSGraphDepthwiseConvolution3DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution3D(_:weights:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("depthwiseConvolution3DWithSourceTensor:weightsTensor:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution3D (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphDepthwiseConvolution3DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution3DDataGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient weightsTensor:(MPSGraphTensor * _Nonnull)weights outputShape:(MPSShape * _Nullable)outputShape descriptor:(MPSGraphDepthwiseConvolution3DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution3DDataGradient(_:weights:outputShape:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("depthwiseConvolution3DDataGradientWithIncomingGradientTensor:weightsTensor:outputShape:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution3DDataGradient (MPSGraphTensor incomingGradient, MPSGraphTensor weights, [NullAllowed][BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphDepthwiseConvolution3DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthwiseConvolution3DWeightsGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradient sourceTensor:(MPSGraphTensor * _Nonnull)source outputShape:(MPSShape * _Nonnull)outputShape descriptor:(MPSGraphDepthwiseConvolution3DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("depthwiseConvolution3DWeightsGradient(_:source:outputShape:descriptor:name:)")));
		[Export ("depthwiseConvolution3DWeightsGradientWithIncomingGradientTensor:sourceTensor:outputShape:descriptor:name:")]
		MPSGraphTensor DepthwiseConvolution3DWeightsGradient (MPSGraphTensor incomingGradient, MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphDepthwiseConvolution3DOpDescriptor descriptor, [NullAllowed] string name);
	}

	// @interface GatherNDOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_GatherNDOps {
		// -(MPSGraphTensor * _Nonnull)gatherNDWithUpdatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor batchDimensions:(NSUInteger)batchDimensions name:(NSString * _Nullable)name;
		[Export ("gatherNDWithUpdatesTensor:indicesTensor:batchDimensions:name:")]
		MPSGraphTensor GatherND (MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, nuint batchDimensions, [NullAllowed] string name);
	}

	// @interface GatherOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_GatherOps {
		// -(MPSGraphTensor * _Nonnull)gatherWithUpdatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor axis:(NSUInteger)axis batchDimensions:(NSUInteger)batchDimensions name:(NSString * _Nullable)name;
		[Export ("gatherWithUpdatesTensor:indicesTensor:axis:batchDimensions:name:")]
		MPSGraphTensor Gather (MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, nuint axis, nuint batchDimensions, [NullAllowed] string name);
	}

	// @interface MPSGraphLossOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphLossOps {
		// -(MPSGraphTensor * _Nonnull)softMaxCrossEntropyWithSourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor labelsTensor:(MPSGraphTensor * _Nonnull)labelsTensor axis:(NSInteger)axis reductionType:(MPSGraphLossReductionType)reductionType name:(NSString * _Nullable)name __attribute__((swift_name("softMaxCrossEntropy(_:labels:axis:reuctionType:name:)")));
		[Export ("softMaxCrossEntropyWithSourceTensor:labelsTensor:axis:reductionType:name:")]
		MPSGraphTensor SoftMaxCrossEntropy (MPSGraphTensor source, MPSGraphTensor labels, nint axis, MPSGraphLossReductionType reductionType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)softMaxCrossEntropyGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)gradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor labelsTensor:(MPSGraphTensor * _Nonnull)labelsTensor axis:(NSInteger)axis reductionType:(MPSGraphLossReductionType)reductionType name:(NSString * _Nullable)name __attribute__((swift_name("softMaxCrossEntropyGradient(_:source:labels:axis:reuctionType:name:)")));
		[Export ("softMaxCrossEntropyGradientWithIncomingGradientTensor:sourceTensor:labelsTensor:axis:reductionType:name:")]
		MPSGraphTensor SoftMaxCrossEntropyGradient (MPSGraphTensor gradientTensor, MPSGraphTensor sourceTensor, MPSGraphTensor labelsTensor, nint axis, MPSGraphLossReductionType reductionType, [NullAllowed] string name);
	}

	// @interface MPSGraphMatrixMultiplicationOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphMatrixMultiplicationOps {
		// -(MPSGraphTensor * _Nonnull)matrixMultiplicationWithPrimaryTensor:(MPSGraphTensor * _Nonnull)primaryTensor secondaryTensor:(MPSGraphTensor * _Nonnull)secondaryTensor name:(NSString * _Nullable)name __attribute__((swift_name("matrixMultiplication(primary:secondary:name:)")));
		[Export ("matrixMultiplicationWithPrimaryTensor:secondaryTensor:name:")]
		MPSGraphTensor MatrixMultiplication (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("HammingDistanceWithPrimaryTensor:secondaryTensor:resultDataType:name:")]
		MPSGraphTensor HammingDistance (MPSGraphTensor primaryTensor, MPSGraphTensor secondaryTensor, MPSDataType resultDataType, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("scaledDotProductAttentionWithQueryTensor:keyTensor:valueTensor:maskTensor:scale:name:")]
		MPSGraphTensor ScaledDotProductAttention (MPSGraphTensor queryTensor, MPSGraphTensor keyTensor, MPSGraphTensor valueTensor, [NullAllowed] MPSGraphTensor maskTensor, float scale, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("scaledDotProductAttentionWithQueryTensor:keyTensor:valueTensor:scale:name:")]
		MPSGraphTensor ScaledDotProductAttention (MPSGraphTensor queryTensor, MPSGraphTensor keyTensor, MPSGraphTensor valueTensor, float scale, [NullAllowed] string name);
	}

	// @interface MPSGraphCreateSparseOpDescriptor : NSObject <NSCopying>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphCreateSparseOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) MPSGraphSparseStorageType sparseStorageType;
		[Export ("sparseStorageType", ArgumentSemantic.Assign)]
		MPSGraphSparseStorageType SparseStorageType { get; set; }

		// @property (readwrite, nonatomic) MPSDataType dataType;
		[Export ("dataType", ArgumentSemantic.Assign)]
		MPSDataType DataType { get; set; }

		// +(instancetype _Nullable)descriptorWithStorageType:(MPSGraphSparseStorageType)sparseStorageType dataType:(MPSDataType)dataType __attribute__((swift_name("sparseDescriptor(descriptorWithStorageType:dataType:)")));
		[Static]
		[Export ("descriptorWithStorageType:dataType:")]
		[return: NullAllowed]
		MPSGraphCreateSparseOpDescriptor Create (MPSGraphSparseStorageType sparseStorageType, MPSDataType dataType);
	}

	// @interface MPSGraphSparseOps (MPSGraph)
	[Category]
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphSparseOps {
		// -(MPSGraphTensor * _Nonnull)sparseTensorWithType:(MPSGraphSparseStorageType)sparseStorageType tensors:(NSArray<MPSGraphTensor *> * _Nonnull)inputTensorArray shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("sparseTensor(sparseTensorWithType:tensors:shape:dataType:name:)")));
		[Export ("sparseTensorWithType:tensors:shape:dataType:name:")]
		MPSGraphTensor Sparse (MPSGraphSparseStorageType sparseStorageType, MPSGraphTensor [] inputTensorArray, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sparseTensorWithDescriptor:(MPSGraphCreateSparseOpDescriptor * _Nonnull)sparseDescriptor tensors:(NSArray<MPSGraphTensor *> * _Nonnull)inputTensorArray shape:(MPSShape * _Nonnull)shape name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("sparseTensor(sparseTensorWithDescriptor:tensors:shape:name:)")));
		[Export ("sparseTensorWithDescriptor:tensors:shape:name:")]
		MPSGraphTensor Sparse (MPSGraphCreateSparseOpDescriptor sparseDescriptor, MPSGraphTensor [] inputTensorArray, [BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);
	}

	// @interface MPSGraphVariableOp : MPSGraphOperation
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphOperation))]
	[DisableDefaultCtor]
	interface MPSGraphVariableOp {
		// @property (readonly, nonatomic) MPSShape * _Nonnull shape;
		[BindAs (typeof (int []))]
		[Export ("shape")]
		NSNumber [] Shape { get; }

		// @property (readonly, nonatomic) MPSDataType dataType;
		[Export ("dataType")]
		MPSDataType DataType { get; }
	}

	// @interface MemoryOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MemoryOps {
		// -(MPSGraphTensor * _Nonnull)placeholderWithShape:(MPSShape * _Nullable)shape dataType:(MPSDataType)dataType name:(NSString * _Nullable)name __attribute__((swift_name("placeholder(shape:dataType:name:)")));
		[Export ("placeholderWithShape:dataType:name:")]
		MPSGraphTensor Placeholder ([NullAllowed][BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)placeholderWithShape:(MPSShape * _Nullable)shape name:(NSString * _Nullable)name __attribute__((swift_name("placeholder(shape:name:)")));
		[Export ("placeholderWithShape:name:")]
		MPSGraphTensor Placeholder ([NullAllowed][BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)constantWithData:(NSData * _Nonnull)data shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType __attribute__((swift_name("constant(_:shape:dataType:)")));
		[Export ("constantWithData:shape:dataType:")]
		MPSGraphTensor Constant (NSData data, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		// -(MPSGraphTensor * _Nonnull)constantWithScalar:(double)scalar dataType:(MPSDataType)dataType __attribute__((swift_name("constant(_:dataType:)")));
		[Export ("constantWithScalar:dataType:")]
		MPSGraphTensor Constant (double scalar, MPSDataType dataType);

		// -(MPSGraphTensor * _Nonnull)constantWithScalar:(double)scalar shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType __attribute__((swift_name("constant(_:shape:dataType:)")));
		[Export ("constantWithScalar:shape:dataType:")]
		MPSGraphTensor Constant (double scalar, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("constantWithRealPart:imaginaryPart:")]
		MPSGraphTensor Constant (double realPart, double imaginaryPart);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("constantWithRealPart:imaginaryPart:dataType:")]
		MPSGraphTensor Constant (double realPart, double imaginaryPart, MPSDataType dataType);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("constantWithRealPart:imaginaryPart:shape:dataType:")]
		MPSGraphTensor Constant (double realPart, double imaginaryPart, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		// -(MPSGraphTensor * _Nonnull)variableWithData:(NSData * _Nonnull)data shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType name:(NSString * _Nullable)name;
		[Export ("variableWithData:shape:dataType:name:")]
		MPSGraphTensor Variable (NSData data, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)readVariable:(MPSGraphTensor * _Nonnull)variable name:(NSString * _Nullable)name __attribute__((swift_name("read(_:name:)")));
		[Export ("readVariable:name:")]
		MPSGraphTensor Read (MPSGraphTensor variable, [NullAllowed] string name);

		// -(MPSGraphOperation * _Nonnull)assignVariable:(MPSGraphTensor * _Nonnull)variable withValueOfTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name __attribute__((swift_name("assign(_:tensor:name:)")));
		[Export ("assignVariable:withValueOfTensor:name:")]
		MPSGraphOperation Assign (MPSGraphTensor variable, MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("variableFromTensorWithTensor:name:")]
		MPSGraphTensor Variable (MPSGraphTensor tensor, [NullAllowed] string name);
	}

	// @interface MPSGraphNormalizationOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphNormalizationOps {
		// -(MPSGraphTensor * _Nonnull)meanOfTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nonnull)axes name:(NSString * _Nullable)name;
		[Export ("meanOfTensor:axes:name:")]
		MPSGraphTensor Mean (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)varianceOfTensor:(MPSGraphTensor * _Nonnull)tensor meanTensor:(MPSGraphTensor * _Nonnull)meanTensor axes:(NSArray<NSNumber *> * _Nonnull)axes name:(NSString * _Nullable)name;
		[Export ("varianceOfTensor:meanTensor:axes:name:")]
		MPSGraphTensor Variance (MPSGraphTensor tensor, MPSGraphTensor meanTensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)varianceOfTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nonnull)axes name:(NSString * _Nullable)name;
		[Export ("varianceOfTensor:axes:name:")]
		MPSGraphTensor Variance (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)normalizationWithTensor:(MPSGraphTensor * _Nonnull)tensor meanTensor:(MPSGraphTensor * _Nonnull)mean varianceTensor:(MPSGraphTensor * _Nonnull)variance gammaTensor:(MPSGraphTensor * _Nullable)gamma betaTensor:(MPSGraphTensor * _Nullable)beta epsilon:(float)epsilon name:(NSString * _Nullable)name __attribute__((swift_name("normalize(_:mean:variance:gamma:beta:epsilon:name:)")));
		[Export ("normalizationWithTensor:meanTensor:varianceTensor:gammaTensor:betaTensor:epsilon:name:")]
		MPSGraphTensor Normalization (MPSGraphTensor tensor, MPSGraphTensor mean, MPSGraphTensor variance, [NullAllowed] MPSGraphTensor gamma, [NullAllowed] MPSGraphTensor beta, float epsilon, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)normalizationGammaGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor meanTensor:(MPSGraphTensor * _Nonnull)meanTensor varianceTensor:(MPSGraphTensor * _Nonnull)varianceTensor reductionAxes:(NSArray<NSNumber *> * _Nonnull)axes epsilon:(float)epsilon name:(NSString * _Nullable)name;
		[Export ("normalizationGammaGradientWithIncomingGradientTensor:sourceTensor:meanTensor:varianceTensor:reductionAxes:epsilon:name:")]
		MPSGraphTensor NormalizationGammaGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor sourceTensor, MPSGraphTensor meanTensor, MPSGraphTensor varianceTensor, [BindAs (typeof (int []))] NSNumber [] axes, float epsilon, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)normalizationBetaGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor reductionAxes:(NSArray<NSNumber *> * _Nonnull)axes name:(NSString * _Nullable)name;
		[Export ("normalizationBetaGradientWithIncomingGradientTensor:sourceTensor:reductionAxes:name:")]
		MPSGraphTensor NormalizationBetaGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor sourceTensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)normalizationGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor meanTensor:(MPSGraphTensor * _Nonnull)meanTensor varianceTensor:(MPSGraphTensor * _Nonnull)varianceTensor gammaTensor:(MPSGraphTensor * _Nullable)gamma gammaGradientTensor:(MPSGraphTensor * _Nullable)gammaGradient betaGradientTensor:(MPSGraphTensor * _Nullable)betaGradient reductionAxes:(NSArray<NSNumber *> * _Nonnull)axes epsilon:(float)epsilon name:(NSString * _Nullable)name;
		[Export ("normalizationGradientWithIncomingGradientTensor:sourceTensor:meanTensor:varianceTensor:gammaTensor:gammaGradientTensor:betaGradientTensor:reductionAxes:epsilon:name:")]
		MPSGraphTensor NormalizationGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor sourceTensor, MPSGraphTensor meanTensor, MPSGraphTensor varianceTensor, [NullAllowed] MPSGraphTensor gamma, [NullAllowed] MPSGraphTensor gammaGradient, [NullAllowed] MPSGraphTensor betaGradient, [BindAs (typeof (int []))] NSNumber [] axes, float epsilon, [NullAllowed] string name);
	}

	// @interface MPSGraphOneHotOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphOneHotOps {
		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth axis:(NSUInteger)axis dataType:(MPSDataType)dataType onValue:(double)onValue offValue:(double)offValue name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:axis:dataType:onValue:offValue:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, nuint axis, MPSDataType dataType, double onValue, double offValue, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth dataType:(MPSDataType)dataType onValue:(double)onValue offValue:(double)offValue name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:dataType:onValue:offValue:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, MPSDataType dataType, double onValue, double offValue, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth axis:(NSUInteger)axis dataType:(MPSDataType)dataType name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:axis:dataType:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, nuint axis, MPSDataType dataType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth axis:(NSUInteger)axis name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:axis:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, nuint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth dataType:(MPSDataType)dataType name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:dataType:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, MPSDataType dataType, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)oneHotWithIndicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor depth:(NSUInteger)depth name:(NSString * _Nullable)name;
		[Export ("oneHotWithIndicesTensor:depth:name:")]
		MPSGraphTensor OneHot (MPSGraphTensor indicesTensor, nuint depth, [NullAllowed] string name);
	}

	// @interface MPSGraphOptimizerOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphOptimizerOps {
		// -(MPSGraphTensor * _Nonnull)stochasticGradientDescentWithLearningRateTensor:(MPSGraphTensor * _Nonnull)learningRateTensor valuesTensor:(MPSGraphTensor * _Nonnull)valuesTensor gradientTensor:(MPSGraphTensor * _Nonnull)gradientTensor name:(NSString * _Nullable)name __attribute__((swift_name("stochasticGradientDescent(learningRate:values:gradient:name:)")));
		[Export ("stochasticGradientDescentWithLearningRateTensor:valuesTensor:gradientTensor:name:")]
		MPSGraphTensor StochasticGradientDescent (MPSGraphTensor learningRate, MPSGraphTensor values, MPSGraphTensor gradient, [NullAllowed] string name);

		// -(MPSGraphOperation * _Nonnull)applyStochasticGradientDescentWithLearningRateTensor:(MPSGraphTensor * _Nonnull)learningRateTensor variable:(MPSGraphVariableOp * _Nonnull)variable gradientTensor:(MPSGraphTensor * _Nonnull)gradientTensor name:(NSString * _Nullable)name __attribute__((swift_name("applyStochasticGradientDescent(learningRate:variable:gradient:name:)")));
		[Export ("applyStochasticGradientDescentWithLearningRateTensor:variable:gradientTensor:name:")]
		MPSGraphOperation ApplyStochasticGradientDescent (MPSGraphTensor learningRate, MPSGraphVariableOp variable, MPSGraphTensor gradient, [NullAllowed] string name);

		[Export ("adamWithLearningRateTensor:beta1Tensor:beta2Tensor:epsilonTensor:beta1PowerTensor:beta2PowerTensor:valuesTensor:momentumTensor:velocityTensor:maximumVelocityTensor:gradientTensor:name:")]
		MPSGraphTensor [] Adam (MPSGraphTensor learningRateTensor, MPSGraphTensor beta1Tensor, MPSGraphTensor beta2Tensor, MPSGraphTensor epsilonTensor, MPSGraphTensor beta1PowerTensor, MPSGraphTensor beta2PowerTensor, MPSGraphTensor valuesTensor, MPSGraphTensor momentumTensor, MPSGraphTensor velocityTensor, [NullAllowed] MPSGraphTensor maximumVelocityTensor, MPSGraphTensor gradientTensor, [NullAllowed] string name);

		[Export ("adamWithCurrentLearningRateTensor:beta1Tensor:beta2Tensor:epsilonTensor:valuesTensor:momentumTensor:velocityTensor:maximumVelocityTensor:gradientTensor:name:")]
		MPSGraphTensor [] Adam (MPSGraphTensor currentLearningRateTensor, MPSGraphTensor beta1Tensor, MPSGraphTensor beta2Tensor, MPSGraphTensor epsilonTensor, MPSGraphTensor valuesTensor, MPSGraphTensor momentumTensor, MPSGraphTensor velocityTensor, [NullAllowed] MPSGraphTensor maximumVelocityTensor, MPSGraphTensor gradientTensor, [NullAllowed] string name);
	}

	[iOS (16, 2), TV (16, 2), Mac (13, 1), MacCatalyst (16, 2)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphQuantizationOps {
		[Export ("quantizeTensor:scale:zeroPoint:dataType:name:")]
		MPSGraphTensor Quantize (MPSGraphTensor tensor, double scale, double zeroPoint, MPSDataType dataType, [NullAllowed] string name);

		[Export ("dequantizeTensor:scale:zeroPoint:dataType:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, double scale, double zeroPoint, MPSDataType dataType, [NullAllowed] string name);

		[Export ("quantizeTensor:scaleTensor:zeroPoint:dataType:axis:name:")]
		MPSGraphTensor Quantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, double zeroPoint, MPSDataType dataType, nint axis, [NullAllowed] string name);

		[Export ("dequantizeTensor:scaleTensor:zeroPoint:dataType:axis:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, double zeroPoint, MPSDataType dataType, nint axis, [NullAllowed] string name);

		[Export ("quantizeTensor:scaleTensor:zeroPointTensor:dataType:axis:name:")]
		MPSGraphTensor Quantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, MPSGraphTensor zeroPointTensor, MPSDataType dataType, nint axis, [NullAllowed] string name);

		[Export ("dequantizeTensor:scaleTensor:zeroPointTensor:dataType:axis:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, MPSGraphTensor zeroPointTensor, MPSDataType dataType, nint axis, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("dequantizeTensor:scaleTensor:zeroPointTensor:dataType:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, MPSGraphTensor zeroPointTensor, MPSDataType dataType, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("dequantizeTensor:scaleTensor:dataType:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor scaleTensor, MPSDataType dataType, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("dequantizeTensor:LUTTensor:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor lookupTableTensor, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("dequantizeTensor:LUTTensor:axis:name:")]
		MPSGraphTensor Dequantize (MPSGraphTensor tensor, MPSGraphTensor lookupTableTensor, nint axis, [NullAllowed] string name);
	}

	// @interface MPSGraphPooling2DOpDescriptor : NSObject <NSCopying>
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphPooling2DOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) NSUInteger kernelWidth;
		[Export ("kernelWidth")]
		nuint KernelWidth { get; set; }

		// @property (readwrite, nonatomic) NSUInteger kernelHeight;
		[Export ("kernelHeight")]
		nuint KernelHeight { get; set; }

		// @property (readwrite, nonatomic) NSUInteger strideInX;
		[Export ("strideInX")]
		nuint StrideInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger strideInY;
		[Export ("strideInY")]
		nuint StrideInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInX;
		[Export ("dilationRateInX")]
		nuint DilationRateInX { get; set; }

		// @property (readwrite, nonatomic) NSUInteger dilationRateInY;
		[Export ("dilationRateInY")]
		nuint DilationRateInY { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingLeft;
		[Export ("paddingLeft")]
		nuint PaddingLeft { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingRight;
		[Export ("paddingRight")]
		nuint PaddingRight { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingTop;
		[Export ("paddingTop")]
		nuint PaddingTop { get; set; }

		// @property (readwrite, nonatomic) NSUInteger paddingBottom;
		[Export ("paddingBottom")]
		nuint PaddingBottom { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) MPSGraphTensorNamedDataLayout dataLayout;
		[Export ("dataLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout DataLayout { get; set; }

		// @property (readwrite, nonatomic) BOOL ceilMode __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Export ("ceilMode")]
		bool CeilMode { get; set; }

		// @property (readwrite, nonatomic) BOOL includeZeroPadToAverage __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0)]
		[MacCatalyst (15, 0)]
		[Export ("includeZeroPadToAverage")]
		bool IncludeZeroPadToAverage { get; set; }

		// +(instancetype _Nullable)descriptorWithKernelWidth:(NSUInteger)kernelWidth kernelHeight:(NSUInteger)kernelHeight strideInX:(NSUInteger)strideInX strideInY:(NSUInteger)strideInY dilationRateInX:(NSUInteger)dilationRateInX dilationRateInY:(NSUInteger)dilationRateInY paddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom paddingStyle:(MPSGraphPaddingStyle)paddingStyle dataLayout:(MPSGraphTensorNamedDataLayout)dataLayout;
		[Static]
		[Export ("descriptorWithKernelWidth:kernelHeight:strideInX:strideInY:dilationRateInX:dilationRateInY:paddingLeft:paddingRight:paddingTop:paddingBottom:paddingStyle:dataLayout:")]
		[return: NullAllowed]
		MPSGraphPooling2DOpDescriptor Create (nuint kernelWidth, nuint kernelHeight, nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout);

		// +(instancetype _Nullable)descriptorWithKernelWidth:(NSUInteger)kernelWidth kernelHeight:(NSUInteger)kernelHeight strideInX:(NSUInteger)strideInX strideInY:(NSUInteger)strideInY paddingStyle:(MPSGraphPaddingStyle)paddingStyle dataLayout:(MPSGraphTensorNamedDataLayout)dataLayout;
		[Static]
		[Export ("descriptorWithKernelWidth:kernelHeight:strideInX:strideInY:paddingStyle:dataLayout:")]
		[return: NullAllowed]
		MPSGraphPooling2DOpDescriptor Create (nuint kernelWidth, nuint kernelHeight, nuint strideInX, nuint strideInY, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout);

		// -(void)setExplicitPaddingWithPaddingLeft:(NSUInteger)paddingLeft paddingRight:(NSUInteger)paddingRight paddingTop:(NSUInteger)paddingTop paddingBottom:(NSUInteger)paddingBottom;
		[Export ("setExplicitPaddingWithPaddingLeft:paddingRight:paddingTop:paddingBottom:")]
		void SetExplicitPadding (nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom);


		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("returnIndicesMode", ArgumentSemantic.Assign)]
		MPSGraphPoolingReturnIndicesMode ReturnIndicesMode { get; set; }

		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("returnIndicesDataType", ArgumentSemantic.Assign)]
		MPSDataType ReturnIndicesDataType { get; set; }
	}

	// @interface MPSGraphPooling4DOpDescriptor : NSObject <NSCopying>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphPooling4DOpDescriptor : NSCopying {
		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull kernelSizes;
		[BindAs (typeof (int []))]
		[Export ("kernelSizes", ArgumentSemantic.Copy)]
		NSNumber [] KernelSizes { get; set; }

		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull strides;
		[BindAs (typeof (int []))]
		[Export ("strides", ArgumentSemantic.Copy)]
		NSNumber [] Strides { get; set; }

		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull dilationRates;
		[BindAs (typeof (int []))]
		[Export ("dilationRates", ArgumentSemantic.Copy)]
		NSNumber [] DilationRates { get; set; }

		// @property (readwrite, copy, nonatomic) NSArray<NSNumber *> * _Nonnull paddingValues;
		[BindAs (typeof (int []))]
		[Export ("paddingValues", ArgumentSemantic.Copy)]
		NSNumber [] PaddingValues { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) BOOL ceilMode;
		[Export ("ceilMode")]
		bool CeilMode { get; set; }

		// @property (readwrite, nonatomic) BOOL includeZeroPadToAverage;
		[Export ("includeZeroPadToAverage")]
		bool IncludeZeroPadToAverage { get; set; }

		// +(instancetype _Nullable)descriptorWithKernelSizes:(NSArray<NSNumber *> * _Nonnull)kernelSizes strides:(NSArray<NSNumber *> * _Nonnull)strides dilationRates:(NSArray<NSNumber *> * _Nonnull)dilationRates paddingValues:(NSArray<NSNumber *> * _Nonnull)paddingValues paddingStyle:(MPSGraphPaddingStyle)paddingStyle;
		[Static]
		[Export ("descriptorWithKernelSizes:strides:dilationRates:paddingValues:paddingStyle:")]
		[return: NullAllowed]
		MPSGraphPooling4DOpDescriptor Create ([BindAs (typeof (int []))] NSNumber [] kernelSizes, [BindAs (typeof (int []))] NSNumber [] strides, [BindAs (typeof (int []))] NSNumber [] dilationRates, [BindAs (typeof (int []))] NSNumber [] paddingValues, MPSGraphPaddingStyle paddingStyle);

		// +(instancetype _Nullable)descriptorWithKernelSizes:(NSArray<NSNumber *> * _Nonnull)kernelSizes paddingStyle:(MPSGraphPaddingStyle)paddingStyle;
		[Static]
		[Export ("descriptorWithKernelSizes:paddingStyle:")]
		[return: NullAllowed]
		MPSGraphPooling4DOpDescriptor Create ([BindAs (typeof (int []))] NSNumber [] kernelSizes, MPSGraphPaddingStyle paddingStyle);

		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("returnIndicesMode", ArgumentSemantic.Assign)]
		MPSGraphPoolingReturnIndicesMode ReturnIndicesMode { get; set; }

		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("returnIndicesDataType", ArgumentSemantic.Assign)]
		MPSDataType ReturnIndicesDataType { get; set; }

	}

	// @interface MPSGraphPoolingOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphPoolingOps {
		// -(MPSGraphTensor * _Nonnull)maxPooling2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("maxPooling2DWithSourceTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling2D (MPSGraphTensor source, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("maxPooling2DReturnIndicesWithSourceTensor:descriptor:name:")]
		MPSGraphTensor [] MaxPooling2DReturnIndices (MPSGraphTensor source, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)maxPooling2DGradientWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("maxPooling2DGradientWithGradientTensor:sourceTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling2DGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("maxPooling2DGradientWithGradientTensor:indicesTensor:outputShape:descriptor:name:")]
		MPSGraphTensor MaxPooling2DGradient (MPSGraphTensor gradient, MPSGraphTensor indices, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("maxPooling2DGradientWithGradientTensor:indicesTensor:outputShapeTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling2DGradient (MPSGraphTensor gradient, MPSGraphTensor indices, MPSGraphTensor outputShape, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)avgPooling2DWithSourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("avgPooling2DWithSourceTensor:descriptor:name:")]
		MPSGraphTensor AvgPooling2D (MPSGraphTensor source, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)avgPooling2DGradientWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling2DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("avgPooling2DGradientWithGradientTensor:sourceTensor:descriptor:name:")]
		MPSGraphTensor AvgPooling2DGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphPooling2DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)maxPooling4DWithSourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("maxPooling4D(_:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("maxPooling4DWithSourceTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling4D (MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 3), Mac (12, 2), iOS (15, 3), MacCatalyst (15, 3)]
		[Export ("maxPooling4DReturnIndicesWithSourceTensor:descriptor:name:")]
		MPSGraphTensor [] MaxPooling4DReturnIndices (MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)maxPooling4DGradientWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("maxPooling4DGradient(_:source:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("maxPooling4DGradientWithGradientTensor:sourceTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling4DGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("maxPooling4DGradientWithGradientTensor:indicesTensor:outputShape:descriptor:name:")]
		MPSGraphTensor MaxPooling4DGradient (MPSGraphTensor gradient, MPSGraphTensor indices, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("maxPooling4DGradientWithGradientTensor:indicesTensor:outputShapeTensor:descriptor:name:")]
		MPSGraphTensor MaxPooling4DGradient (MPSGraphTensor gradient, MPSGraphTensor indices, MPSGraphTensor outputShape, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)avgPooling4DWithSourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("avgPooling4D(_:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("avgPooling4DWithSourceTensor:descriptor:name:")]
		MPSGraphTensor AvgPooling4D (MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)avgPooling4DGradientWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("avgPooling4DGradient(_:source:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("avgPooling4DGradientWithGradientTensor:sourceTensor:descriptor:name:")]
		MPSGraphTensor AvgPooling4DGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)L2NormPooling4DWithSourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("L2NormPooling4D(_:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("L2NormPooling4DWithSourceTensor:descriptor:name:")]
		MPSGraphTensor L2NormPooling4D (MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)L2NormPooling4DGradientWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient sourceTensor:(MPSGraphTensor * _Nonnull)source descriptor:(MPSGraphPooling4DOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name __attribute__((swift_name("L2NormPooling4DGradient(_:source:descriptor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("L2NormPooling4DGradientWithGradientTensor:sourceTensor:descriptor:name:")]
		MPSGraphTensor L2NormPooling4DGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphPooling4DOpDescriptor descriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphRandomOpDescriptor : NSObject <NSCopying>
	[TV (15, 2), Mac (12, 1), iOS (15, 2), MacCatalyst (15, 2)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphRandomOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) MPSGraphRandomDistribution distribution;
		[Export ("distribution", ArgumentSemantic.Assign)]
		MPSGraphRandomDistribution Distribution { get; set; }

		// @property (readwrite, nonatomic) MPSDataType dataType;
		[Export ("dataType", ArgumentSemantic.Assign)]
		MPSDataType DataType { get; set; }

		// @property (readwrite, nonatomic) float min;
		[Export ("min")]
		float Min { get; set; }

		// @property (readwrite, nonatomic) float max;
		[Export ("max")]
		float Max { get; set; }

		// @property (readwrite, nonatomic) NSInteger minInteger;
		[Export ("minInteger")]
		nint MinInteger { get; set; }

		// @property (readwrite, nonatomic) NSInteger maxInteger;
		[Export ("maxInteger")]
		nint MaxInteger { get; set; }

		// @property (readwrite, nonatomic) float mean;
		[Export ("mean")]
		float Mean { get; set; }

		// @property (readwrite, nonatomic) float standardDeviation;
		[Export ("standardDeviation")]
		float StandardDeviation { get; set; }

		// @property (readwrite, nonatomic) MPSGraphRandomNormalSamplingMethod samplingMethod;
		[Export ("samplingMethod", ArgumentSemantic.Assign)]
		MPSGraphRandomNormalSamplingMethod SamplingMethod { get; set; }

		// +(instancetype _Nullable)descriptorWithDistribution:(MPSGraphRandomDistribution)distribution dataType:(MPSDataType)dataType;
		[Static]
		[Export ("descriptorWithDistribution:dataType:")]
		[return: NullAllowed]
		MPSGraphRandomOpDescriptor Create (MPSGraphRandomDistribution distribution, MPSDataType dataType);
	}

	// @interface MPSGraphRandomOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphRandomOps {
		// -(MPSGraphTensor * _Nonnull)randomPhiloxStateTensorWithSeed:(NSUInteger)seed name:(NSString * _Nullable)name;
		[Export ("randomPhiloxStateTensorWithSeed:name:")]
		MPSGraphTensor RandomPhiloxState (nuint seed, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomPhiloxStateTensorWithCounterLow:(NSUInteger)counterLow counterHigh:(NSUInteger)counterHigh key:(NSUInteger)key name:(NSString * _Nullable)name;
		[Export ("randomPhiloxStateTensorWithCounterLow:counterHigh:key:name:")]
		MPSGraphTensor RandomPhiloxState (nuint counterLow, nuint counterHigh, nuint key, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomTensorWithShape:(MPSShape * _Nonnull)shape descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShape:descriptor:name:")]
		MPSGraphTensor Random ([BindAs (typeof (int []))] NSNumber [] shape, MPSGraphRandomOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShapeTensor:descriptor:name:")]
		MPSGraphTensor Random (MPSGraphTensor shapeTensor, MPSGraphRandomOpDescriptor descriptor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomTensorWithShape:(MPSShape * _Nonnull)shape descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor seed:(NSUInteger)seed name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShape:descriptor:seed:name:")]
		MPSGraphTensor Random ([BindAs (typeof (int []))] NSNumber [] shape, MPSGraphRandomOpDescriptor descriptor, nuint seed, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor seed:(NSUInteger)seed name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShapeTensor:descriptor:seed:name:")]
		MPSGraphTensor Random (MPSGraphTensor shapeTensor, MPSGraphRandomOpDescriptor descriptor, nuint seed, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)randomTensorWithShape:(MPSShape * _Nonnull)shape descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor stateTensor:(MPSGraphTensor * _Nonnull)state name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShape:descriptor:stateTensor:name:")]
		MPSGraphTensor [] Random ([BindAs (typeof (int []))] NSNumber [] shape, MPSGraphRandomOpDescriptor descriptor, MPSGraphTensor state, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)randomTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor descriptor:(MPSGraphRandomOpDescriptor * _Nonnull)descriptor stateTensor:(MPSGraphTensor * _Nonnull)state name:(NSString * _Nullable)name;
		[Export ("randomTensorWithShapeTensor:descriptor:stateTensor:name:")]
		MPSGraphTensor [] Random (MPSGraphTensor shapeTensor, MPSGraphRandomOpDescriptor descriptor, MPSGraphTensor state, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomUniformTensorWithShape:(MPSShape * _Nonnull)shape name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShape:name:")]
		MPSGraphTensor RandomUniform ([BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomUniformTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShapeTensor:name:")]
		MPSGraphTensor RandomUniform (MPSGraphTensor shapeTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomUniformTensorWithShape:(MPSShape * _Nonnull)shape seed:(NSUInteger)seed name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShape:seed:name:")]
		MPSGraphTensor RandomUniform ([BindAs (typeof (int []))] NSNumber [] shape, nuint seed, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)randomUniformTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor seed:(NSUInteger)seed name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShapeTensor:seed:name:")]
		MPSGraphTensor RandomUniform (MPSGraphTensor shapeTensor, nuint seed, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)randomUniformTensorWithShape:(MPSShape * _Nonnull)shape stateTensor:(MPSGraphTensor * _Nonnull)state name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShape:stateTensor:name:")]
		MPSGraphTensor [] RandomUniform ([BindAs (typeof (int []))] NSNumber [] shape, MPSGraphTensor state, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)randomUniformTensorWithShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor stateTensor:(MPSGraphTensor * _Nonnull)state name:(NSString * _Nullable)name;
		[Export ("randomUniformTensorWithShapeTensor:stateTensor:name:")]
		MPSGraphTensor [] RandomUniform (MPSGraphTensor shapeTensor, MPSGraphTensor state, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)dropoutTensor:(MPSGraphTensor * _Nonnull)tensor rate:(double)rate name:(NSString * _Nullable)name __attribute__((swift_name("dropout(_:rate:name:)")));
		[Export ("dropoutTensor:rate:name:")]
		MPSGraphTensor Dropout (MPSGraphTensor tensor, double rate, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)dropoutTensor:(MPSGraphTensor * _Nonnull)tensor rateTensor:(MPSGraphTensor * _Nonnull)rate name:(NSString * _Nullable)name __attribute__((swift_name("dropout(_:rate:name:)")));
		[Export ("dropoutTensor:rateTensor:name:")]
		MPSGraphTensor Dropout (MPSGraphTensor tensor, MPSGraphTensor rate, [NullAllowed] string name);
	}

	// @interface MPSGraphReductionOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphReductionOps {
		// -(MPSGraphTensor * _Nonnull)reductionSumWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionSumWithTensor:axis:name:")]
		MPSGraphTensor ReductionSum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionSumWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionSumWithTensor:axes:name:")]
		MPSGraphTensor ReductionSum (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMaximumWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionMaximumWithTensor:axis:name:")]
		MPSGraphTensor ReductionMaximum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMaximumWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionMaximumWithTensor:axes:name:")]
		MPSGraphTensor ReductionMaximum (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMinimumWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionMinimumWithTensor:axis:name:")]
		MPSGraphTensor ReductionMinimum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMinimumWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionMinimumWithTensor:axes:name:")]
		MPSGraphTensor ReductionMinimum (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMaximumPropagateNaNWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionMaximumPropagateNaNWithTensor:axis:name:")]
		MPSGraphTensor ReductionMaximumPropagateNaN (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMaximumPropagateNaNWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionMaximumPropagateNaNWithTensor:axes:name:")]
		MPSGraphTensor ReductionMaximumPropagateNaN (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMinimumPropagateNaNWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionMinimumPropagateNaNWithTensor:axis:name:")]
		MPSGraphTensor ReductionMinimumPropagateNaN (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionMinimumPropagateNaNWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionMinimumPropagateNaNWithTensor:axes:name:")]
		MPSGraphTensor ReductionMinimumPropagateNaN (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionProductWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name;
		[Export ("reductionProductWithTensor:axis:name:")]
		MPSGraphTensor ReductionProduct (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionProductWithTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nullable)axes name:(NSString * _Nullable)name;
		[Export ("reductionProductWithTensor:axes:name:")]
		MPSGraphTensor ReductionProduct (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionArgMaximumWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(macCatalyst, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), MacCatalyst (15, 0), iOS (15, 0)]
		[Export ("reductionArgMaximumWithTensor:axis:name:")]
		MPSGraphTensor ReductionArgMaximum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reductionArgMinimumWithTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(macCatalyst, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), MacCatalyst (15, 0), iOS (15, 0)]
		[Export ("reductionArgMinimumWithTensor:axis:name:")]
		MPSGraphTensor ReductionArgMinimum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (15, 3), MacCatalyst (15, 3), Mac (12, 2), iOS (15, 3)]
		[Export ("reductionAndWithTensor:axis:name:")]
		MPSGraphTensor ReductionAnd (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (15, 3), MacCatalyst (15, 3), Mac (12, 2), iOS (15, 3)]
		[Export ("reductionAndWithTensor:axes:name:")]
		MPSGraphTensor ReductionAnd (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		[TV (15, 3), MacCatalyst (15, 3), Mac (12, 2), iOS (15, 3)]
		[Export ("reductionOrWithTensor:axis:name:")]
		MPSGraphTensor ReductionOr (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (15, 3), MacCatalyst (15, 3), Mac (12, 2), iOS (15, 3)]
		[Export ("reductionOrWithTensor:axes:name:")]
		MPSGraphTensor ReductionOr (MPSGraphTensor tensor, [NullAllowed][BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);
	}

	// @interface MPSGraphResizeOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphResizeOps {
		// -(MPSGraphTensor * _Nonnull)resizeTensor:(MPSGraphTensor * _Nonnull)imagesTensor size:(MPSShape * _Nonnull)size mode:(MPSGraphResizeMode)mode centerResult:(BOOL)centerResult alignCorners:(BOOL)alignCorners layout:(MPSGraphTensorNamedDataLayout)layout name:(NSString * _Nullable)name __attribute__((swift_name("resize(_:size:mode:centerResult:alignCorners:layout:name:)")));
		[Export ("resizeTensor:size:mode:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor Resize (MPSGraphTensor imagesTensor, [BindAs (typeof (int []))] NSNumber [] size, MPSGraphResizeMode mode, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)resizeTensor:(MPSGraphTensor * _Nonnull)imagesTensor sizeTensor:(MPSGraphTensor * _Nonnull)size mode:(MPSGraphResizeMode)mode centerResult:(BOOL)centerResult alignCorners:(BOOL)alignCorners layout:(MPSGraphTensorNamedDataLayout)layout name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("resize(_:sizeTensor:mode:centerResult:alignCorners:layout:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("resizeTensor:sizeTensor:mode:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor Resize (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphResizeMode mode, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)resizeWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient input:(MPSGraphTensor * _Nonnull)input mode:(MPSGraphResizeMode)mode centerResult:(BOOL)centerResult alignCorners:(BOOL)alignCorners layout:(MPSGraphTensorNamedDataLayout)layout name:(NSString * _Nullable)name;
		[Export ("resizeWithGradientTensor:input:mode:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor ResizeGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphResizeMode mode, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeTensor:sizeTensor:mode:centerResult:alignCorners:name:")]
		MPSGraphTensor Resize (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphResizeMode mode, bool centerResult, bool alignCorners, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeNearestWithTensor:sizeTensor:nearestRoundingMode:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor ResizeNearest (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphResizeNearestRoundingMode nearestRoundingMode, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeNearestWithTensor:sizeTensor:nearestRoundingMode:centerResult:alignCorners:name:")]
		MPSGraphTensor ResizeNearest (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphResizeNearestRoundingMode nearestRoundingMode, bool centerResult, bool alignCorners, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeBilinearWithTensor:sizeTensor:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor ResizeBilinear (MPSGraphTensor imagesTensor, MPSGraphTensor size, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeBilinearWithTensor:sizeTensor:centerResult:alignCorners:name:")]
		MPSGraphTensor ResizeBilinear (MPSGraphTensor imagesTensor, MPSGraphTensor size, bool centerResult, bool alignCorners, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeTensor:sizeTensor:scaleOffsetTensor:mode:layout:name:")]
		MPSGraphTensor Resize (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scaleOffset, MPSGraphResizeMode mode, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeTensor:sizeTensor:scaleTensor:offsetTensor:mode:name:")]
		MPSGraphTensor Resize (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scale, MPSGraphTensor offset, MPSGraphResizeMode mode, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeNearestWithTensor:sizeTensor:scaleOffsetTensor:nearestRoundingMode:layout:name:")]
		MPSGraphTensor ResizeNearest (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scaleOffset, MPSGraphResizeNearestRoundingMode nearestRoundingMode, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeNearestWithTensor:sizeTensor:scaleTensor:offsetTensor:nearestRoundingMode:name:")]
		MPSGraphTensor ResizeNearest (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scale, MPSGraphTensor offset, MPSGraphResizeNearestRoundingMode nearestRoundingMode, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeBilinearWithTensor:sizeTensor:scaleOffsetTensor:layout:name:")]
		MPSGraphTensor ResizeBilinear (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scaleOffset, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeBilinearWithTensor:sizeTensor:scaleTensor:offsetTensor:name:")]
		MPSGraphTensor ResizeBilinear (MPSGraphTensor imagesTensor, MPSGraphTensor size, MPSGraphTensor scale, MPSGraphTensor offset, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeNearestWithGradientTensor:input:nearestRoundingMode:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor ResizeNearestWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphResizeNearestRoundingMode nearestRoundingMode, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeBilinearWithGradientTensor:input:centerResult:alignCorners:layout:name:")]
		MPSGraphTensor ResizeBilinearWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, bool centerResult, bool alignCorners, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeWithGradientTensor:input:scaleOffsetTensor:mode:layout:name:")]
		MPSGraphTensor ResizeWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scaleOffset, MPSGraphResizeMode mode, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeWithGradientTensor:input:scaleTensor:offsetTensor:mode:name:")]
		MPSGraphTensor ResizeWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scale, MPSGraphTensor offset, MPSGraphResizeMode mode, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeNearestWithGradientTensor:input:scaleOffsetTensor:nearestRoundingMode:layout:name:")]
		MPSGraphTensor ResizeNearestWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scaleOffset, MPSGraphResizeNearestRoundingMode nearestRoundingMode, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeNearestWithGradientTensor:input:scaleTensor:offsetTensor:nearestRoundingMode:name:")]
		MPSGraphTensor ResizeNearestWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scale, MPSGraphTensor offset, MPSGraphResizeNearestRoundingMode nearestRoundingMode, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("resizeBilinearWithGradientTensor:input:scaleOffsetTensor:layout:name:")]
		MPSGraphTensor ResizeBilinearWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scaleOffset, MPSGraphTensorNamedDataLayout layout, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("resizeBilinearWithGradientTensor:input:scaleTensor:offsetTensor:name:")]
		MPSGraphTensor ResizeBilinearWithGradient (MPSGraphTensor gradient, MPSGraphTensor input, MPSGraphTensor scale, MPSGraphTensor offset, [NullAllowed] string name);
	}

	// @interface ScatterNDOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_ScatterNDOps {
		// -(MPSGraphTensor * _Nonnull)scatterNDWithUpdatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor shape:(MPSShape * _Nonnull)shape batchDimensions:(NSUInteger)batchDimensions mode:(MPSGraphScatterMode)mode name:(NSString * _Nullable)name;
		[Export ("scatterNDWithUpdatesTensor:indicesTensor:shape:batchDimensions:mode:name:")]
		MPSGraphTensor ScatterND (MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [BindAs (typeof (int []))] NSNumber [] shape, nuint batchDimensions, MPSGraphScatterMode mode, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)scatterNDWithUpdatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor shape:(MPSShape * _Nonnull)shape batchDimensions:(NSUInteger)batchDimensions name:(NSString * _Nullable)name;
		[Export ("scatterNDWithUpdatesTensor:indicesTensor:shape:batchDimensions:name:")]
		MPSGraphTensor ScatterND (MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [BindAs (typeof (int []))] NSNumber [] shape, nuint batchDimensions, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)scatterNDWithDataTensor:(MPSGraphTensor * _Nonnull)dataTensor updatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor batchDimensions:(NSUInteger)batchDimensions mode:(MPSGraphScatterMode)mode name:(NSString * _Nullable)name __attribute__((swift_name("scatterNDWithData(_:updates:indices:batchDimensions:mode:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("scatterNDWithDataTensor:updatesTensor:indicesTensor:batchDimensions:mode:name:")]
		MPSGraphTensor ScatterND (MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, nuint batchDimensions, MPSGraphScatterMode mode, [NullAllowed] string name);
	}

	// @interface MPSGraphScatterOps (MPSGraph)
	[Category]
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphScatterOps {
		// -(MPSGraphTensor * _Nonnull)scatterWithUpdatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor shape:(MPSShape * _Nonnull)shape axis:(NSInteger)axis mode:(MPSGraphScatterMode)mode name:(NSString * _Nullable)name __attribute__((swift_name("scatter(_:indices:shape:axis:mode:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[Export ("scatterWithUpdatesTensor:indicesTensor:shape:axis:mode:name:")]
		MPSGraphTensor Scatter (MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [BindAs (typeof (int []))] NSNumber [] shape, nint axis, MPSGraphScatterMode mode, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)scatterWithDataTensor:(MPSGraphTensor * _Nonnull)dataTensor updatesTensor:(MPSGraphTensor * _Nonnull)updatesTensor indicesTensor:(MPSGraphTensor * _Nonnull)indicesTensor axis:(NSInteger)axis mode:(MPSGraphScatterMode)mode name:(NSString * _Nullable)name __attribute__((swift_name("scatterWithData(_:updates:indices:axis:mode:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[Export ("scatterWithDataTensor:updatesTensor:indicesTensor:axis:mode:name:")]
		MPSGraphTensor Scatter (MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, nint axis, MPSGraphScatterMode mode, [NullAllowed] string name);
	}

	// @interface MPSGraphStencilOpDescriptor : NSObject <NSCopying>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphStencilOpDescriptor : NSCopying {
		// @property (readwrite, nonatomic) MPSGraphReductionMode reductionMode;
		[Export ("reductionMode", ArgumentSemantic.Assign)]
		MPSGraphReductionMode ReductionMode { get; set; }

		// @property (readwrite, copy, nonatomic) MPSShape * _Nonnull offsets;
		[BindAs (typeof (int []))]
		[Export ("offsets", ArgumentSemantic.Copy)]
		NSNumber [] Offsets { get; set; }

		// @property (readwrite, copy, nonatomic) MPSShape * _Nonnull strides;
		[BindAs (typeof (int []))]
		[Export ("strides", ArgumentSemantic.Copy)]
		NSNumber [] Strides { get; set; }

		// @property (readwrite, copy, nonatomic) MPSShape * _Nonnull dilationRates;
		[BindAs (typeof (int []))]
		[Export ("dilationRates", ArgumentSemantic.Copy)]
		NSNumber [] DilationRates { get; set; }

		// @property (readwrite, copy, nonatomic) MPSShape * _Nonnull explicitPadding;
		[BindAs (typeof (int []))]
		[Export ("explicitPadding", ArgumentSemantic.Copy)]
		NSNumber [] ExplicitPadding { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingMode boundaryMode;
		[Export ("boundaryMode", ArgumentSemantic.Assign)]
		MPSGraphPaddingMode BoundaryMode { get; set; }

		// @property (readwrite, nonatomic) MPSGraphPaddingStyle paddingStyle;
		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		// @property (readwrite, nonatomic) float paddingConstant;
		[Export ("paddingConstant")]
		float PaddingConstant { get; set; }

		// +(instancetype _Nullable)descriptorWithReductionMode:(MPSGraphReductionMode)reductionMode offsets:(MPSShape * _Nonnull)offsets strides:(MPSShape * _Nonnull)strides dilationRates:(MPSShape * _Nonnull)dilationRates explicitPadding:(MPSShape * _Nonnull)explicitPadding boundaryMode:(MPSGraphPaddingMode)boundaryMode paddingStyle:(MPSGraphPaddingStyle)paddingStyle paddingConstant:(float)paddingConstant;
		[Static]
		[Export ("descriptorWithReductionMode:offsets:strides:dilationRates:explicitPadding:boundaryMode:paddingStyle:paddingConstant:")]
		[return: NullAllowed]
		MPSGraphStencilOpDescriptor Create (MPSGraphReductionMode reductionMode, [BindAs (typeof (int []))] NSNumber [] offsets, [BindAs (typeof (int []))] NSNumber [] strides, [BindAs (typeof (int []))] NSNumber [] dilationRates, [BindAs (typeof (int []))] NSNumber [] explicitPadding, MPSGraphPaddingMode boundaryMode, MPSGraphPaddingStyle paddingStyle, float paddingConstant);

		// +(instancetype _Nullable)descriptorWithOffsets:(MPSShape * _Nonnull)offsets explicitPadding:(MPSShape * _Nonnull)explicitPadding;
		[Static]
		[Export ("descriptorWithOffsets:explicitPadding:")]
		[return: NullAllowed]
		MPSGraphStencilOpDescriptor Create ([BindAs (typeof (int []))] NSNumber [] offsets, [BindAs (typeof (int []))] NSNumber [] explicitPadding);

		// +(instancetype _Nullable)descriptorWithExplicitPadding:(MPSShape * _Nonnull)explicitPadding;
		[Static]
		[Export ("descriptorWithExplicitPadding:")]
		[return: NullAllowed]
		MPSGraphStencilOpDescriptor Create ([BindAs (typeof (int []))] NSNumber [] explicitPadding);

		// +(instancetype _Nullable)descriptorWithPaddingStyle:(MPSGraphPaddingStyle)paddingStyle;
		[Static]
		[Export ("descriptorWithPaddingStyle:")]
		[return: NullAllowed]
		MPSGraphStencilOpDescriptor Create (MPSGraphPaddingStyle paddingStyle);
	}

	// @interface MPSGraphStencilOps (MPSGraph)
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphStencilOps {
		// -(MPSGraphTensor * _Nonnull)stencilWithSourceTensor:(MPSGraphTensor * _Nonnull)source weightsTensor:(MPSGraphTensor * _Nonnull)weights descriptor:(MPSGraphStencilOpDescriptor * _Nonnull)descriptor name:(NSString * _Nullable)name;
		[Export ("stencilWithSourceTensor:weightsTensor:descriptor:name:")]
		MPSGraphTensor Stencil (MPSGraphTensor source, MPSGraphTensor weights, MPSGraphStencilOpDescriptor descriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphTensorShapeOps (MPSGraph)
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphTensorShapeOps {
		// -(MPSGraphTensor * _Nonnull)reshapeTensor:(MPSGraphTensor * _Nonnull)tensor withShape:(MPSShape * _Nonnull)shape name:(NSString * _Nullable)name __attribute__((swift_name("reshape(_:shape:name:)")));
		[Export ("reshapeTensor:withShape:name:")]
		MPSGraphTensor Reshape (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reshapeTensor:(MPSGraphTensor * _Nonnull)tensor withShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0))) __attribute__((swift_name("reshape(_:shapeTensor:name:)")));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("reshapeTensor:withShapeTensor:name:")]
		MPSGraphTensor Reshape (MPSGraphTensor tensor, MPSGraphTensor shapeTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)transposeTensor:(MPSGraphTensor * _Nonnull)tensor dimension:(NSUInteger)dimensionIndex withDimension:(NSUInteger)dimensionIndex2 name:(NSString * _Nullable)name;
		[Export ("transposeTensor:dimension:withDimension:name:")]
		MPSGraphTensor Transpose (MPSGraphTensor tensor, nuint dimensionIndex, nuint dimensionIndex2, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("transposeTensor:permutation:name:")]
		MPSGraphTensor Transpose (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] permutation, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sliceTensor:(MPSGraphTensor * _Nonnull)tensor dimension:(NSUInteger)dimensionIndex start:(NSInteger)start length:(NSInteger)length name:(NSString * _Nullable)name;
		[Export ("sliceTensor:dimension:start:length:name:")]
		MPSGraphTensor Slice (MPSGraphTensor tensor, nuint dimensionIndex, nint start, nint length, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sliceTensor:(MPSGraphTensor * _Nonnull)tensor starts:(NSArray<NSNumber *> * _Nonnull)starts ends:(NSArray<NSNumber *> * _Nonnull)ends strides:(NSArray<NSNumber *> * _Nonnull)strides name:(NSString * _Nullable)name;
		[Export ("sliceTensor:starts:ends:strides:name:")]
		MPSGraphTensor Slice (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sliceTensor:(MPSGraphTensor * _Nonnull)tensor starts:(NSArray<NSNumber *> * _Nonnull)starts ends:(NSArray<NSNumber *> * _Nonnull)ends strides:(NSArray<NSNumber *> * _Nonnull)strides startMask:(uint32_t)startMask endMask:(uint32_t)endMask squeezeMask:(uint32_t)squeezeMask name:(NSString * _Nullable)name;
		[Export ("sliceTensor:starts:ends:strides:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor Slice (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		[TV (18, 2), MacCatalyst (18, 2), Mac (15, 2), iOS (18, 2)]
		[Export ("sliceTensor:startTensor:endTensor:strideTensor:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor Slice (MPSGraphTensor tensor, MPSGraphTensor startTensor, MPSGraphTensor endTensor, MPSGraphTensor strideTensor, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		[TV (18, 2), MacCatalyst (18, 2), Mac (15, 2), iOS (18, 2)]
		[Export ("sliceTensor:startTensor:sizeTensor:squeezeMask:name:")]
		MPSGraphTensor Slice (MPSGraphTensor tensor, MPSGraphTensor startTensor, MPSGraphTensor sizeTensor, uint squeezeMask, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sliceGradientTensor:(MPSGraphTensor * _Nonnull)inputGradientTensor fwdInShapeTensor:(MPSGraphTensor * _Nonnull)fwdInShapeTensor starts:(NSArray<NSNumber *> * _Nonnull)starts ends:(NSArray<NSNumber *> * _Nonnull)ends strides:(NSArray<NSNumber *> * _Nonnull)strides name:(NSString * _Nullable)name;
		[Export ("sliceGradientTensor:fwdInShapeTensor:starts:ends:strides:name:")]
		MPSGraphTensor SliceGradient (MPSGraphTensor inputGradientTensor, MPSGraphTensor fwdInShapeTensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)sliceGradientTensor:(MPSGraphTensor * _Nonnull)inputGradientTensor fwdInShapeTensor:(MPSGraphTensor * _Nonnull)fwdInShapeTensor starts:(NSArray<NSNumber *> * _Nonnull)starts ends:(NSArray<NSNumber *> * _Nonnull)ends strides:(NSArray<NSNumber *> * _Nonnull)strides startMask:(uint32_t)startMask endMask:(uint32_t)endMask squeezeMask:(uint32_t)squeezeMask name:(NSString * _Nullable)name;
		[Export ("sliceGradientTensor:fwdInShapeTensor:starts:ends:strides:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor SliceGradient (MPSGraphTensor inputGradientTensor, MPSGraphTensor fwdInShapeTensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		[TV (18, 2), MacCatalyst (18, 2), Mac (15, 2), iOS (18, 2)]
		[Export ("sliceGradientTensor:fwdInShapeTensor:startTensor:endTensor:strideTensor:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor SliceGradient (MPSGraphTensor inputGradientTensor, MPSGraphTensor fwdInShapeTensor, MPSGraphTensor startTensor, MPSGraphTensor endTensor, MPSGraphTensor strideTensor, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		[TV (18, 2), MacCatalyst (18, 2), Mac (15, 2), iOS (18, 2)]
		[Export ("sliceGradientTensor:fwdInShapeTensor:startTensor:sizeTensor:squeezeMask:name:")]
		MPSGraphTensor SliceGradient (MPSGraphTensor inputGradientTensor, MPSGraphTensor fwdInShapeTensor, MPSGraphTensor startTensor, MPSGraphTensor sizeTensor, uint squeezeMask, [NullAllowed] string name);

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("sliceUpdateDataTensor:updateTensor:startsTensor:endsTensor:stridesTensor:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor SliceUpdateData (MPSGraphTensor dataTensor, MPSGraphTensor updateTensor, MPSGraphTensor startsTensor, MPSGraphTensor endsTensor, MPSGraphTensor stridesTensor, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("sliceUpdateDataTensor:updateTensor:starts:ends:strides:startMask:endMask:squeezeMask:name:")]
		MPSGraphTensor SliceUpdateData (MPSGraphTensor dataTensor, MPSGraphTensor updateTensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, uint startMask, uint endMask, uint squeezeMask, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)concatTensor:(MPSGraphTensor * _Nonnull)tensor withTensor:(MPSGraphTensor * _Nonnull)tensor2 dimension:(NSInteger)dimensionIndex name:(NSString * _Nullable)name;
		[Export ("concatTensor:withTensor:dimension:name:")]
		MPSGraphTensor Concat (MPSGraphTensor tensor, MPSGraphTensor tensor2, nint dimensionIndex, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)concatTensors:(NSArray<MPSGraphTensor *> * _Nonnull)tensors dimension:(NSInteger)dimensionIndex name:(NSString * _Nullable)name;
		[Export ("concatTensors:dimension:name:")]
		MPSGraphTensor ConcatTensors (MPSGraphTensor [] tensors, nint dimensionIndex, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)concatTensors:(NSArray<MPSGraphTensor *> * _Nonnull)tensors dimension:(NSInteger)dimensionIndex interleave:(BOOL)interleave name:(NSString * _Nullable)name;
		[Export ("concatTensors:dimension:interleave:name:")]
		MPSGraphTensor ConcatTensors (MPSGraphTensor [] tensors, nint dimensionIndex, bool interleave, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)tileTensor:(MPSGraphTensor * _Nonnull)tensor withMultiplier:(MPSShape * _Nonnull)multiplier name:(NSString * _Nullable)name;
		[Export ("tileTensor:withMultiplier:name:")]
		MPSGraphTensor Tile (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] multiplier, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)tileGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor withMultiplier:(MPSShape * _Nonnull)multiplier name:(NSString * _Nullable)name;
		[Export ("tileGradientWithIncomingGradientTensor:sourceTensor:withMultiplier:name:")]
		MPSGraphTensor TileGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor sourceTensor, [BindAs (typeof (int []))] NSNumber [] multiplier, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)padTensor:(MPSGraphTensor * _Nonnull)tensor withPaddingMode:(MPSGraphPaddingMode)paddingMode leftPadding:(MPSShape * _Nonnull)leftPadding rightPadding:(MPSShape * _Nonnull)rightPadding constantValue:(double)constantValue name:(NSString * _Nullable)name;
		[Export ("padTensor:withPaddingMode:leftPadding:rightPadding:constantValue:name:")]
		MPSGraphTensor Pad (MPSGraphTensor tensor, MPSGraphPaddingMode paddingMode, [BindAs (typeof (int []))] NSNumber [] leftPadding, [BindAs (typeof (int []))] NSNumber [] rightPadding, double constantValue, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)padGradientWithIncomingGradientTensor:(MPSGraphTensor * _Nonnull)incomingGradientTensor sourceTensor:(MPSGraphTensor * _Nonnull)sourceTensor paddingMode:(MPSGraphPaddingMode)paddingMode leftPadding:(MPSShape * _Nonnull)leftPadding rightPadding:(MPSShape * _Nonnull)rightPadding name:(NSString * _Nullable)name;
		[Export ("padGradientWithIncomingGradientTensor:sourceTensor:paddingMode:leftPadding:rightPadding:name:")]
		MPSGraphTensor PadGradient (MPSGraphTensor incomingGradientTensor, MPSGraphTensor sourceTensor, MPSGraphPaddingMode paddingMode, [BindAs (typeof (int []))] NSNumber [] leftPadding, [BindAs (typeof (int []))] NSNumber [] rightPadding, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)spaceToDepth2DTensor:(MPSGraphTensor * _Nonnull)tensor widthAxisTensor:(MPSGraphTensor * _Nonnull)widthAxisTensor heightAxisTensor:(MPSGraphTensor * _Nonnull)heightAxisTensor depthAxisTensor:(MPSGraphTensor * _Nonnull)depthAxisTensor blockSize:(NSUInteger)blockSize usePixelShuffleOrder:(BOOL)usePixelShuffleOrder name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("spaceToDepth2DTensor:widthAxisTensor:heightAxisTensor:depthAxisTensor:blockSize:usePixelShuffleOrder:name:")]
		MPSGraphTensor SpaceToDepth2D (MPSGraphTensor tensor, MPSGraphTensor widthAxisTensor, MPSGraphTensor heightAxisTensor, MPSGraphTensor depthAxisTensor, nuint blockSize, bool usePixelShuffleOrder, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthToSpace2DTensor:(MPSGraphTensor * _Nonnull)tensor widthAxisTensor:(MPSGraphTensor * _Nonnull)widthAxisTensor heightAxisTensor:(MPSGraphTensor * _Nonnull)heightAxisTensor depthAxisTensor:(MPSGraphTensor * _Nonnull)depthAxisTensor blockSize:(NSUInteger)blockSize usePixelShuffleOrder:(BOOL)usePixelShuffleOrder name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("depthToSpace2DTensor:widthAxisTensor:heightAxisTensor:depthAxisTensor:blockSize:usePixelShuffleOrder:name:")]
		MPSGraphTensor DepthToSpace2D (MPSGraphTensor tensor, MPSGraphTensor widthAxisTensor, MPSGraphTensor heightAxisTensor, MPSGraphTensor depthAxisTensor, nuint blockSize, bool usePixelShuffleOrder, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)spaceToDepth2DTensor:(MPSGraphTensor * _Nonnull)tensor widthAxis:(NSUInteger)widthAxis heightAxis:(NSUInteger)heightAxis depthAxis:(NSUInteger)depthAxis blockSize:(NSUInteger)blockSize usePixelShuffleOrder:(BOOL)usePixelShuffleOrder name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("spaceToDepth2DTensor:widthAxis:heightAxis:depthAxis:blockSize:usePixelShuffleOrder:name:")]
		MPSGraphTensor SpaceToDepth2D (MPSGraphTensor tensor, nuint widthAxis, nuint heightAxis, nuint depthAxis, nuint blockSize, bool usePixelShuffleOrder, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)depthToSpace2DTensor:(MPSGraphTensor * _Nonnull)tensor widthAxis:(NSUInteger)widthAxis heightAxis:(NSUInteger)heightAxis depthAxis:(NSUInteger)depthAxis blockSize:(NSUInteger)blockSize usePixelShuffleOrder:(BOOL)usePixelShuffleOrder name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("depthToSpace2DTensor:widthAxis:heightAxis:depthAxis:blockSize:usePixelShuffleOrder:name:")]
		MPSGraphTensor DepthToSpace2D (MPSGraphTensor tensor, nuint widthAxis, nuint heightAxis, nuint depthAxis, nuint blockSize, bool usePixelShuffleOrder, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("spaceToBatchTensor:spatialAxes:batchAxis:blockDimensions:usePixelShuffleOrder:name:")]
		MPSGraphTensor SpaceToBatch (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] spatialAxes, nint batchAxis, [BindAs (typeof (int []))] NSNumber [] blockDimensions, bool usePixelShuffleOrder, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("spaceToBatchTensor:spatialAxesTensor:batchAxisTensor:blockDimensionsTensor:usePixelShuffleOrder:name:")]
		MPSGraphTensor SpaceToBatch (MPSGraphTensor tensor, MPSGraphTensor spatialAxesTensor, MPSGraphTensor batchAxisTensor, MPSGraphTensor blockDimensionsTensor, bool usePixelShuffleOrder, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("batchToSpaceTensor:spatialAxes:batchAxis:blockDimensions:usePixelShuffleOrder:name:")]
		MPSGraphTensor BatchToSpace (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] spatialAxes, nint batchAxis, [BindAs (typeof (int []))] NSNumber [] blockDimensions, bool usePixelShuffleOrder, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("batchToSpaceTensor:spatialAxesTensor:batchAxisTensor:blockDimensionsTensor:usePixelShuffleOrder:name:")]
		MPSGraphTensor BatchToSpace (MPSGraphTensor tensor, MPSGraphTensor spatialAxesTensor, MPSGraphTensor batchAxisTensor, MPSGraphTensor blockDimensionsTensor, bool usePixelShuffleOrder, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reverseTensor:(MPSGraphTensor * _Nonnull)tensor axesTensor:(MPSGraphTensor * _Nonnull)axesTensor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("reverseTensor:axesTensor:name:")]
		MPSGraphTensor Reverse (MPSGraphTensor tensor, MPSGraphTensor axesTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reverseTensor:(MPSGraphTensor * _Nonnull)tensor axes:(NSArray<NSNumber *> * _Nonnull)axes name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("reverseTensor:axes:name:")]
		MPSGraphTensor Reverse (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)reverseTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("reverseTensor:name:")]
		MPSGraphTensor Reverse (MPSGraphTensor tensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)flatten2DTensor:(MPSGraphTensor * _Nonnull)tensor axis:(NSInteger)axis name:(NSString * _Nullable)name __attribute__((swift_name("flatten2D(_:axis:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("flatten2DTensor:axis:name:")]
		MPSGraphTensor Flatten2D (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)flatten2DTensor:(MPSGraphTensor * _Nonnull)tensor axisTensor:(MPSGraphTensor * _Nonnull)axisTensor name:(NSString * _Nullable)name __attribute__((swift_name("flatten2D(_:axisTensor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("flatten2DTensor:axisTensor:name:")]
		MPSGraphTensor Flatten2D (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)broadcastTensor:(MPSGraphTensor * _Nonnull)tensor toShape:(MPSShape * _Nonnull)shape name:(NSString * _Nullable)name __attribute__((swift_name("broadcast(_:shape:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("broadcastTensor:toShape:name:")]
		MPSGraphTensor Broadcast (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)broadcastTensor:(MPSGraphTensor * _Nonnull)tensor toShapeTensor:(MPSGraphTensor * _Nonnull)shapeTensor name:(NSString * _Nullable)name __attribute__((swift_name("broadcast(_:shapeTensor:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("broadcastTensor:toShapeTensor:name:")]
		MPSGraphTensor Broadcast (MPSGraphTensor tensor, MPSGraphTensor shapeTensor, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)shapeOfTensor:(MPSGraphTensor * _Nonnull)tensor name:(NSString * _Nullable)name __attribute__((swift_name("shapeOf(_:name:)"))) __attribute__((availability(macos, introduced=12.0))) __attribute__((availability(ios, introduced=15.0))) __attribute__((availability(tvos, introduced=15.0)));
		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("shapeOfTensor:name:")]
		MPSGraphTensor Shape (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("castTensor:toType:name:")]
		MPSGraphTensor Cast (MPSGraphTensor tensor, MPSDataType type, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("sliceUpdateDataTensor:updateTensor:startsTensor:endsTensor:stridesTensor:name:")]
		MPSGraphTensor SliceUpdateData (MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, MPSGraphTensor startsTensor, MPSGraphTensor endsTensor, MPSGraphTensor stridesTensor, [NullAllowed] string name);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("sliceUpdateDataTensor:updateTensor:starts:ends:strides:name:")]
		MPSGraphTensor SliceUpdateData (MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, [BindAs (typeof (int []))] NSNumber [] starts, [BindAs (typeof (int []))] NSNumber [] ends, [BindAs (typeof (int []))] NSNumber [] strides, [NullAllowed] string name);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("reinterpretCastTensor:toType:name:")]
		MPSGraphTensor ReinterpretCast (MPSGraphTensor tensor, MPSDataType type, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("stackTensors:axis:name:")]
		MPSGraphTensor Stack (MPSGraphTensor [] inputTensors, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("splitTensor:splitSizes:axis:name:")]
		MPSGraphTensor [] Split (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] splitSizes, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("splitTensor:splitSizesTensor:axis:name:")]
		MPSGraphTensor [] Split (MPSGraphTensor tensor, MPSGraphTensor splitSizesTensor, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("splitTensor:numSplits:axis:name:")]
		MPSGraphTensor [] Split (MPSGraphTensor tensor, nuint numSplits, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("squeezeTensor:name:")]
		MPSGraphTensor Squeeze (MPSGraphTensor tensor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("squeezeTensor:axis:name:")]
		MPSGraphTensor Squeeze (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("squeezeTensor:axes:name:")]
		MPSGraphTensor Squeeze (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("squeezeTensor:axesTensor:name:")]
		MPSGraphTensor Squeeze (MPSGraphTensor tensor, MPSGraphTensor axesTensor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("expandDimsOfTensor:axis:name:")]
		MPSGraphTensor ExpandDims (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("expandDimsOfTensor:axes:name:")]
		MPSGraphTensor ExpandDims (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("expandDimsOfTensor:axesTensor:name:")]
		MPSGraphTensor ExpandDims (MPSGraphTensor tensor, MPSGraphTensor axesTensor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("coordinateAlongAxis:withShape:name:")]
		MPSGraphTensor CoordinateAlongAxis (nint axis, [BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("coordinateAlongAxisTensor:withShape:name:")]
		MPSGraphTensor CoordinateAlongAxis (MPSGraphTensor axisTensor, [BindAs (typeof (int []))] NSNumber [] shape, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("coordinateAlongAxis:withShapeTensor:name:")]
		MPSGraphTensor CoordinateAlongAxis (nint axis, MPSGraphTensor shapeTensor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("coordinateAlongAxisTensor:withShapeTensor:name:")]
		MPSGraphTensor CoordinateAlongAxis (MPSGraphTensor axisTensor, MPSGraphTensor shapeTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphTopKOps (MPSGraph)
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphTopKOps {
		// -(NSArray<MPSGraphTensor *> * _Nonnull)topKWithSourceTensor:(MPSGraphTensor * _Nonnull)source k:(NSUInteger)k name:(NSString * _Nullable)name __attribute__((swift_name("topK(_:k:name:)")));
		[Export ("topKWithSourceTensor:k:name:")]
		MPSGraphTensor [] TopK (MPSGraphTensor source, nuint k, [NullAllowed] string name);

		// -(NSArray<MPSGraphTensor *> * _Nonnull)topKWithSourceTensor:(MPSGraphTensor * _Nonnull)source kTensor:(MPSGraphTensor * _Nonnull)kTensor name:(NSString * _Nullable)name __attribute__((swift_name("topK(_:kTensor:name:)")));
		[Export ("topKWithSourceTensor:kTensor:name:")]
		MPSGraphTensor [] TopK (MPSGraphTensor source, MPSGraphTensor kTensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("topKWithSourceTensor:axis:k:name:")]
		MPSGraphTensor [] TopK (MPSGraphTensor source, nint axis, nuint k, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("bottomKWithSourceTensor:axis:k:name:")]
		MPSGraphTensor [] BottomK (MPSGraphTensor source, nint axis, nuint k, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("topKWithSourceTensor:axisTensor:kTensor:name:")]
		MPSGraphTensor [] TopK (MPSGraphTensor source, MPSGraphTensor axisTensor, MPSGraphTensor kTensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("bottomKWithSourceTensor:axisTensor:kTensor:name:")]
		MPSGraphTensor [] BottomK (MPSGraphTensor source, MPSGraphTensor axisTensor, MPSGraphTensor kTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphTopKGradientOps (MPSGraph)
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphTopKGradientOps {
		// -(MPSGraphTensor * _Nonnull)topKWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient source:(MPSGraphTensor * _Nonnull)source k:(NSUInteger)k name:(NSString * _Nullable)name __attribute__((swift_name("topKGradient(_:input:k:name:)")));
		[Export ("topKWithGradientTensor:source:k:name:")]
		MPSGraphTensor TopKGradient (MPSGraphTensor gradient, MPSGraphTensor source, nuint k, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("topKWithGradientTensor:source:axis:k:name:")]
		MPSGraphTensor TopKWithGradient (MPSGraphTensor gradient, MPSGraphTensor source, nint axis, nuint k, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("bottomKWithGradientTensor:source:axis:k:name:")]
		MPSGraphTensor BottomKWithGradient (MPSGraphTensor gradient, MPSGraphTensor source, nint axis, nuint k, [NullAllowed] string name);

		// -(MPSGraphTensor * _Nonnull)topKWithGradientTensor:(MPSGraphTensor * _Nonnull)gradient source:(MPSGraphTensor * _Nonnull)source kTensor:(MPSGraphTensor * _Nonnull)kTensor name:(NSString * _Nullable)name __attribute__((swift_name("topKGradient(_:input:kTensor:name:)")));
		[Export ("topKWithGradientTensor:source:kTensor:name:")]
		MPSGraphTensor TopKGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor kTensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("topKWithGradientTensor:source:axisTensor:kTensor:name:")]
		MPSGraphTensor TopKWithGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor axisTensor, MPSGraphTensor kTensor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("bottomKWithGradientTensor:source:axisTensor:kTensor:name:")]
		MPSGraphTensor BottomKWithGradient (MPSGraphTensor gradient, MPSGraphTensor source, MPSGraphTensor axisTensor, MPSGraphTensor kTensor, [NullAllowed] string name);
	}

	delegate void MPSGraphCompilationCompletionHandler (MPSGraphExecutable executable, [NullAllowed] NSError error);

	// @interface MPSGraphCompilationDescriptor : NSObject <NSCopying>
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphCompilationDescriptor : NSCopying {
		[Export ("disableTypeInference")]
		void DisableTypeInference ();

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("optimizationLevel")]
		MPSGraphOptimization OptimizationLevel { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("waitForCompilationCompletion")]
		bool WaitForCompilationCompletion { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("compilationCompletionHandler")]
		MPSGraphCompilationCompletionHandler CompilationCompletionHandler { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("dispatchQueue")]
		DispatchQueue DispatchQueue { get; set; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Deprecated (PlatformName.iOS, 17, 0, message: "MPSGraph will automatically provide the best performance and power efficiency with MPSGraphOptimization.Level1.")]
		[Deprecated (PlatformName.MacOSX, 14, 0, message: "MPSGraph will automatically provide the best performance and power efficiency with MPSGraphOptimization.Level1.")]
		[Deprecated (PlatformName.TvOS, 17, 0, message: "MPSGraph will automatically provide the best performance and power efficiency with MPSGraphOptimization.Level1.")]
		[Deprecated (PlatformName.MacCatalyst, 17, 0, message: "MPSGraph will automatically provide the best performance and power efficiency with MPSGraphOptimization.Level1.")]
		[Export ("optimizationProfile")]
		MPSGraphOptimizationProfile OptimizationProfile { get; set; }

		[TV (17, 1), Mac (14, 1), iOS (17, 1), MacCatalyst (17, 1)]
		[Export ("callables")]
		[NullAllowed]
		NSDictionary<NSString, MPSGraphExecutable> Callables { get; set; }
	}

	// @interface MPSGraphDevice : NSObject
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphDevice {
		// @property (readonly, nonatomic) MPSGraphDeviceType type;
		[Export ("type")]
		MPSGraphDeviceType Type { get; }

		// @property (readonly, nonatomic) id<MTLDevice> _Nullable metalDevice;
		[NullAllowed, Export ("metalDevice")]
		IMTLDevice MetalDevice { get; }

		// +(instancetype _Nonnull)deviceWithMTLDevice:(id<MTLDevice> _Nonnull)metalDevice;
		[Static]
		[Export ("deviceWithMTLDevice:")]
		MPSGraphDevice Create (IMTLDevice metalDevice);
	}

	// typedef void (^MPSGraphExecutableCompletionHandler)(NSArray<MPSGraphTensorData *> * _Nonnull, NSError * _Nullable);
	delegate void MPSGraphExecutableCompletionHandler (MPSGraphTensorData [] results, [NullAllowed] NSError error);

	// typedef void (^MPSGraphExecutableScheduledHandler)(NSArray<MPSGraphTensorData *> * _Nonnull, NSError * _Nullable);
	delegate void MPSGraphExecutableScheduledHandler (MPSGraphTensorData [] results, [NullAllowed] NSError error);

	// @interface MPSGraphExecutableExecutionDescriptor : NSObject
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphExecutableExecutionDescriptor : NSCopying {
		// @property (readwrite, atomic) MPSGraphExecutableScheduledHandler _Nonnull scheduledHandler;
		[Export ("scheduledHandler", ArgumentSemantic.Assign)]
		MPSGraphExecutableScheduledHandler ScheduledHandler { get; set; }

		// @property (readwrite, atomic) MPSGraphExecutableCompletionHandler _Nonnull completionHandler;
		[Export ("completionHandler", ArgumentSemantic.Assign)]
		MPSGraphExecutableCompletionHandler CompletionHandler { get; set; }

		// @property (readwrite, atomic) BOOL waitUntilCompleted;
		[Export ("waitUntilCompleted")]
		bool WaitUntilCompleted { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("waitForEvent:value:")]
		void WaitForEvent (IMTLSharedEvent @event, ulong value);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("signalEvent:atExecutionEvent:value:")]
		void SignalEvent (IMTLSharedEvent @event, MPSGraphExecutionStage executionStage, ulong value);
	}

	// @interface MPSGraphExecutable : NSObject
	[TV (15, 0), iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphExecutable {
		// @property (readwrite, atomic) MPSGraphOptions options;
		[Export ("options", ArgumentSemantic.Assign)]
		MPSGraphOptions Options { get; set; }

		// @property (readonly, atomic) NSArray<MPSGraphTensor *> * _Nullable feedTensors;
		[NullAllowed, Export ("feedTensors")]
		MPSGraphTensor [] FeedTensors { get; }

		// @property (readonly, atomic) NSArray<MPSGraphTensor *> * _Nullable targetTensors;
		[NullAllowed, Export ("targetTensors")]
		MPSGraphTensor [] TargetTensors { get; }

		// -(void)specializeWithDevice:(MPSGraphDevice * _Nullable)device inputTypes:(NSArray<MPSGraphType *> * _Nonnull)inputTypes compilationDescriptor:(MPSGraphCompilationDescriptor * _Nullable)compilationDescriptor;
		[Export ("specializeWithDevice:inputTypes:compilationDescriptor:")]
		void Specialize ([NullAllowed] MPSGraphDevice device, MPSGraphType [] inputTypes, [NullAllowed] MPSGraphCompilationDescriptor compilationDescriptor);

		// -(NSArray<MPSGraphTensorData *> * _Nonnull)runWithMTLCommandQueue:(id<IMTLCommandQueue> _Nonnull)commandQueue inputsArray:(NSArray<MPSGraphTensorData *> * _Nonnull)inputsArray resultsArray:(NSArray<MPSGraphTensorData *> * _Nullable)resultsArray executionDescriptor:(MPSGraphExecutableExecutionDescriptor * _Nullable)executionDescriptor __attribute__((swift_name("run(with:inputs:results:executionDescriptor:)")));
		[Export ("runWithMTLCommandQueue:inputsArray:resultsArray:executionDescriptor:")]
		MPSGraphTensorData [] Run (IMTLCommandQueue commandQueue, MPSGraphTensorData [] inputsArray, [NullAllowed] MPSGraphTensorData [] resultsArray, [NullAllowed] MPSGraphExecutableExecutionDescriptor executionDescriptor);

		// -(NSArray<MPSGraphTensorData *> * _Nonnull)runAsyncWithMTLCommandQueue:(id<IMTLCommandQueue> _Nonnull)commandQueue inputsArray:(NSArray<MPSGraphTensorData *> * _Nonnull)inputsArray resultsArray:(NSArray<MPSGraphTensorData *> * _Nullable)resultsArray executionDescriptor:(MPSGraphExecutableExecutionDescriptor * _Nullable)executionDescriptor __attribute__((swift_name("runAsync(with:inputs:results:executionDescriptor:)")));
		[Export ("runAsyncWithMTLCommandQueue:inputsArray:resultsArray:executionDescriptor:")]
		MPSGraphTensorData [] RunAsync (IMTLCommandQueue commandQueue, MPSGraphTensorData [] inputsArray, [NullAllowed] MPSGraphTensorData [] resultsArray, [NullAllowed] MPSGraphExecutableExecutionDescriptor executionDescriptor);

		// -(NSArray<MPSGraphTensorData *> * _Nonnull)encodeToCommandBuffer:(MPSCommandBuffer * _Nonnull)commandBuffer inputsArray:(NSArray<MPSGraphTensorData *> * _Nonnull)inputsArray resultsArray:(NSArray<MPSGraphTensorData *> * _Nullable)resultsArray executionDescriptor:(MPSGraphExecutableExecutionDescriptor * _Nullable)executionDescriptor __attribute__((swift_name("encode(to:inputs:results:executionDescriptor:)")));
		[Export ("encodeToCommandBuffer:inputsArray:resultsArray:executionDescriptor:")]
		MPSGraphTensorData [] Encode (MPSCommandBuffer commandBuffer, MPSGraphTensorData [] inputsArray, [NullAllowed] MPSGraphTensorData [] resultsArray, [NullAllowed] MPSGraphExecutableExecutionDescriptor executionDescriptor);

		[Internal]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("initWithCoreMLPackageAtURL:compilationDescriptor:")]
		NativeHandle _InitWithCoreMLPackage (NSUrl coreMLPackageUrl, [NullAllowed] MPSGraphCompilationDescriptor compilationDescriptor);

		[Internal]
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("initWithMPSGraphPackageAtURL:compilationDescriptor:")]
		NativeHandle _InitWithMPSGraphPackage (NSUrl mpsgraphPackageUrl, [NullAllowed] MPSGraphCompilationDescriptor compilationDescriptor);

		[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
		[Export ("getOutputTypesWithDevice:inputTypes:compilationDescriptor:")]
		[return: NullAllowed]
		MPSGraphShapedType [] GetOutputTypes ([NullAllowed] MPSGraphDevice device, MPSGraphType [] inputTypes, [NullAllowed] MPSGraphCompilationDescriptor compilationDescriptor);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("serializeToMPSGraphPackageAtURL:descriptor:")]
		void SerializeToMPSGraphPackage (NSUrl url, [NullAllowed] MPSGraphExecutableSerializationDescriptor descriptor);
	}

	// typedef void (^MPSGraphCompletionHandler)(MPSGraphTensorDataDictionary * _Nonnull, NSError * _Nullable);
	delegate void MPSGraphCompletionHandler (MPSGraphTensorDataDictionary resultsDictionary, [NullAllowed] NSError error);

	// typedef void (^MPSGraphScheduledHandler)(MPSGraphTensorDataDictionary * _Nonnull, NSError * _Nullable);
	delegate void MPSGraphScheduledHandler (MPSGraphTensorDataDictionary resultsDictionary, [NullAllowed] NSError error);

	// @interface MPSGraphExecutionDescriptor : NSObject
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphExecutionDescriptor {
		// @property (readwrite, atomic) MPSGraphScheduledHandler _Nonnull scheduledHandler;
		[Export ("scheduledHandler", ArgumentSemantic.Assign)]
		MPSGraphScheduledHandler ScheduledHandler { get; set; }

		// @property (readwrite, atomic) MPSGraphCompletionHandler _Nonnull completionHandler;
		[Export ("completionHandler", ArgumentSemantic.Assign)]
		MPSGraphCompletionHandler CompletionHandler { get; set; }

		// @property (readwrite, atomic) BOOL waitUntilCompleted;
		[Export ("waitUntilCompleted")]
		bool WaitUntilCompleted { get; set; }

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[NullAllowed, Export ("compilationDescriptor", ArgumentSemantic.Copy)]
		MPSGraphCompilationDescriptor CompilationDescriptor { get; set; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("waitForEvent:value:")]
		void WaitForEvent (IMTLSharedEvent @event, ulong value);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("signalEvent:atExecutionEvent:value:")]
		void SignalEvent (IMTLSharedEvent @event, MPSGraphExecutionStage executionStage, ulong value);
	}

	// @interface MPSGraphType: NSObject<NSCopying>
	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphType : NSCopying {
	}

	// MPSGraphType was introduced in iOS 15 (macOS 12) and became the base class for
	// MPSGraphShapedType which existed in iOS 14.
	// @interface MPSGraphShapedType : MPSGraphType
	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MPSGraphType))]
	interface MPSGraphShapedType {
		// @property (readwrite, copy, atomic) MPSShape * _Nullable shape;
		[NullAllowed]
		[BindAs (typeof (int []))]
		[Export ("shape", ArgumentSemantic.Copy)]
		NSNumber [] Shape { get; set; }

		// @property (readwrite, atomic) MPSDataType dataType;
		[Export ("dataType", ArgumentSemantic.Assign)]
		MPSDataType DataType { get; set; }

		// -(instancetype _Nonnull)initWithShape:(MPSShape * _Nullable)shape dataType:(MPSDataType)dataType;
		[Export ("initWithShape:dataType:")]
		IntPtr Constructor ([NullAllowed][BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		// -(BOOL)isEqualTo:(MPSGraphShapedType * _Nullable)object;
		[Export ("isEqualTo:")]
		bool IsEqualTo ([NullAllowed] MPSGraphShapedType @object);
	}

	// MPSGraphOperation.h

	// @interface MPSGraphOperation : NSObject <NSCopying>
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	[DisableDefaultCtor]
	interface MPSGraphOperation : NSCopying {
		// @property (readonly, nonatomic) NSArray<MPSGraphTensor *> * _Nonnull inputTensors;
		[Export ("inputTensors")]
		MPSGraphTensor [] InputTensors { get; }

		// @property (readonly, nonatomic) NSArray<MPSGraphTensor *> * _Nonnull outputTensors;
		[Export ("outputTensors")]
		MPSGraphTensor [] OutputTensors { get; }

		// @property (readonly, nonatomic) NSArray<MPSGraphOperation *> * _Nonnull controlDependencies;
		[Export ("controlDependencies")]
		MPSGraphOperation [] ControlDependencies { get; }

		// @property (readonly, nonatomic) MPSGraph * _Nonnull graph;
		[Export ("graph")]
		MPSGraph Graph { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; }
	}

	// MPSGraphTensor.h

	// @interface MPSGraphTensor : NSObject <NSCopying>
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	[DisableDefaultCtor]
	interface MPSGraphTensor : NSCopying {
		// @property (readonly, copy, nonatomic) MPSShape * _Nullable shape;
		[NullAllowed]
		[BindAs (typeof (int []))]
		[Export ("shape", ArgumentSemantic.Copy)]
		NSNumber [] Shape { get; }

		// @property (readonly, nonatomic) MPSDataType dataType;
		[Export ("dataType")]
		MPSDataType DataType { get; }

		// @property (readonly, nonatomic) MPSGraphOperation * _Nonnull operation;
		[Export ("operation")]
		MPSGraphOperation Operation { get; }
	}

	// @interface MPSGraphTensorData : NSObject
	[iOS (14, 0), TV (14, 0), MacCatalyst (14, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphTensorData {
		// @property (readonly, copy, nonatomic) MPSShape * _Nonnull shape;
		[BindAs (typeof (int []))]
		[Export ("shape", ArgumentSemantic.Copy)]
		NSNumber [] Shape { get; }

		// @property (readonly, nonatomic) MPSDataType dataType;
		[Export ("dataType")]
		MPSDataType DataType { get; }

		// @property (readonly, nonatomic) MPSGraphDevice * _Nonnull device;
		[Export ("device")]
		MPSGraphDevice Device { get; }

		// -(instancetype _Nonnull)initWithDevice:(MPSGraphDevice * _Nonnull)device data:(NSData * _Nonnull)data shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType;
		[Export ("initWithDevice:data:shape:dataType:")]
		IntPtr Constructor (MPSGraphDevice device, NSData data, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		// -(instancetype _Nonnull)initWithMTLBuffer:(id<IMTLBuffer> _Nonnull)buffer shape:(MPSShape * _Nonnull)shape dataType:(MPSDataType)dataType __attribute__((swift_name("init(_:shape:dataType:)")));
		[Export ("initWithMTLBuffer:shape:dataType:")]
		IntPtr Constructor (IMTLBuffer buffer, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType);

		// -(instancetype _Nonnull)initWithMPSMatrix:(MPSMatrix * _Nonnull)matrix __attribute__((swift_name("init(_:)")));
		[Export ("initWithMPSMatrix:")]
		IntPtr Constructor (MPSMatrix matrix);

		// -(instancetype _Nonnull)initWithMPSMatrix:(MPSMatrix * _Nonnull)matrix rank:(NSUInteger)rank __attribute__((swift_name("init(_:rank:)")));
		[Export ("initWithMPSMatrix:rank:")]
		IntPtr Constructor (MPSMatrix matrix, nuint rank);

		// -(instancetype _Nonnull)initWithMPSVector:(MPSVector * _Nonnull)vector __attribute__((swift_name("init(_:)")));
		[Export ("initWithMPSVector:")]
		IntPtr Constructor (MPSVector vector);

		// -(instancetype _Nonnull)initWithMPSVector:(MPSVector * _Nonnull)vector rank:(NSUInteger)rank __attribute__((swift_name("init(_:rank:)")));
		[Export ("initWithMPSVector:rank:")]
		IntPtr Constructor (MPSVector vector, nuint rank);

		// -(instancetype _Nonnull)initWithMPSNDArray:(MPSNDArray * _Nonnull)ndarray __attribute__((swift_name("init(_:)")));
		[Export ("initWithMPSNDArray:")]
		IntPtr Constructor (MPSNDArray ndarray);

		// Use NSArray here instead of [] to match the MetalPerformanceShaders API
		// -(instancetype _Nonnull)initWithMPSImageBatch:(MPSImageBatch * _Nonnull)imageBatch __attribute__((swift_name("init(_:)")));
		[Export ("initWithMPSImageBatch:")]
		IntPtr Constructor (NSArray<MPSImage> imageBatch);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("initWithMTLBuffer:shape:dataType:rowBytes:")]
		NativeHandle Constructor (IMTLBuffer buffer, [BindAs (typeof (int []))] NSNumber [] shape, MPSDataType dataType, nuint rowBytes);

		// -(MPSNDArray * _Nonnull)mpsndarray;
		[Export ("mpsndarray")]
		MPSNDArray MPSNDArray { get; }
	}


	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	// Not a native enum
	public enum MPSGraphDeploymentPlatform : ulong {
		macOS = 0,
		iOS = 1,
		tvOS = 2,
		[iOS (17, 4), TV (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
		visionOS = 3,
	}

	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	// Not a native enum
	public enum MPSGraphExecutionStage : ulong {
		Completed = 0,
	}

	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	[Native]
	[NativeName ("MPSGraphFFTScalingMode")]
	public enum MPSGraphFftScalingMode : ulong {
		None = 0,
		Size,
		Unitary,
	}

	[iOS (17, 0), TV (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
	[Native]
	public enum MPSGraphNonMaximumSuppressionCoordinateMode : ulong {
		CornersHeightFirst = 0,
		CornersWidthFirst = 1,
		CentersHeightFirst = 2,
		CentersWidthFirst = 3,
	}

	[iOS (15, 3), TV (15, 3), Mac (12, 2), MacCatalyst (15, 3)]
	[Native]
	public enum MPSGraphPoolingReturnIndicesMode : ulong {
		None,
		GlobalFlatten1d,
		GlobalFlatten2d,
		GlobalFlatten3d,
		GlobalFlatten4d,
		LocalFlatten1d,
		LocalFlatten2d,
		LocalFlatten3d,
		LocalFlatten4d,
	}

	[iOS (16, 0), TV (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[Native]
	public enum MPSGraphResizeNearestRoundingMode : ulong {
		RoundPreferCeil = 0,
		RoundPreferFloor = 1,
		Ceil = 2,
		Floor = 3,
		[iOS (16, 3), TV (16, 3), Mac (13, 2), MacCatalyst (16, 3)]
		RoundToEven = 4,
		[iOS (16, 3), TV (16, 3), Mac (13, 2), MacCatalyst (16, 3)]
		RoundToOdd = 5,
	}

	[iOS (15, 4), TV (15, 4), Mac (12, 3), MacCatalyst (15, 4)]
	[Native]
	[NativeName ("MPSGraphRNNActivation")]
	public enum MPSGraphRnnActivation : ulong {
		None = 0,
		Relu,
		Tanh,
		Sigmoid,
		HardSigmoid,
	}

	[TV (16, 3), Mac (13, 2), iOS (16, 3), MacCatalyst (16, 3)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphConvolution3DOpDescriptor : NSCopying {
		[Export ("strideInX")]
		nuint StrideInX { get; set; }

		[Export ("strideInY")]
		nuint StrideInY { get; set; }

		[Export ("strideInZ")]
		nuint StrideInZ { get; set; }

		[Export ("dilationRateInX")]
		nuint DilationRateInX { get; set; }

		[Export ("dilationRateInY")]
		nuint DilationRateInY { get; set; }

		[Export ("dilationRateInZ")]
		nuint DilationRateInZ { get; set; }

		[Export ("paddingLeft")]
		nuint PaddingLeft { get; set; }

		[Export ("paddingRight")]
		nuint PaddingRight { get; set; }

		[Export ("paddingTop")]
		nuint PaddingTop { get; set; }

		[Export ("paddingBottom")]
		nuint PaddingBottom { get; set; }

		[Export ("paddingFront")]
		nuint PaddingFront { get; set; }

		[Export ("paddingBack")]
		nuint PaddingBack { get; set; }

		[Export ("paddingStyle", ArgumentSemantic.Assign)]
		MPSGraphPaddingStyle PaddingStyle { get; set; }

		[Export ("dataLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout DataLayout { get; set; }

		[Export ("weightsLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout WeightsLayout { get; set; }

		[Export ("groups")]
		nuint Groups { get; set; }

		[Static]
		[Export ("descriptorWithStrideInX:strideInY:strideInZ:dilationRateInX:dilationRateInY:dilationRateInZ:groups:paddingLeft:paddingRight:paddingTop:paddingBottom:paddingFront:paddingBack:paddingStyle:dataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphConvolution3DOpDescriptor Create (nuint strideInX, nuint strideInY, nuint strideInZ, nuint dilationRateInX, nuint dilationRateInY, nuint dilationRateInZ, nuint groups, nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, nuint paddingFront, nuint paddingBack, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		[Static]
		[Export ("descriptorWithStrideInX:strideInY:strideInZ:dilationRateInX:dilationRateInY:dilationRateInZ:groups:paddingStyle:dataLayout:weightsLayout:")]
		[return: NullAllowed]
		MPSGraphConvolution3DOpDescriptor Create (nuint strideInX, nuint strideInY, nuint strideInZ, nuint dilationRateInX, nuint dilationRateInY, nuint dilationRateInZ, nuint groups, MPSGraphPaddingStyle paddingStyle, MPSGraphTensorNamedDataLayout dataLayout, MPSGraphTensorNamedDataLayout weightsLayout);

		[Export ("setExplicitPaddingWithPaddingLeft:paddingRight:paddingTop:paddingBottom:paddingFront:paddingBack:")]
		void SetExplicitPadding (nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, nuint paddingFront, nuint paddingBack);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphExecutableSerializationDescriptor {
		[Export ("append")]
		bool Append { get; set; }

		[Export ("deploymentPlatform", ArgumentSemantic.Assign)]
		MPSGraphDeploymentPlatform DeploymentPlatform { get; set; }

		[Export ("minimumDeploymentTarget")]
		string MinimumDeploymentTarget { get; set; }
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MPSGraphObject), Name = "MPSGraphFFTDescriptor")]
	interface MPSGraphFftDescriptor : NSCopying {
		[Export ("inverse")]
		bool Inverse { get; set; }

		[Export ("scalingMode", ArgumentSemantic.Assign)]
		MPSGraphFftScalingMode ScalingMode { get; set; }

		[Export ("roundToOddHermitean")]
		bool RoundToOddHermitean { get; set; }

		[Static]
		[Export ("descriptor")]
		[return: NullAllowed]
		MPSGraphFftDescriptor Create ();
	}

	[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (MPSGraphObject), Name = "MPSGraphGRUDescriptor")]
	interface MPSGraphGruDescriptor : NSCopying {
		[Export ("reverse")]
		bool Reverse { get; set; }

		[Export ("bidirectional")]
		bool Bidirectional { get; set; }

		[Export ("training")]
		bool Training { get; set; }

		[Export ("resetGateFirst")]
		bool ResetGateFirst { get; set; }

		[Export ("resetAfter")]
		bool ResetAfter { get; set; }

		[Export ("flipZ")]
		bool FlipZ { get; set; }

		[Export ("updateGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation UpdateGateActivation { get; set; }

		[Export ("resetGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation ResetGateActivation { get; set; }

		[Export ("outputGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation OutputGateActivation { get; set; }

		[Static]
		[Export ("descriptor")]
		[return: NullAllowed]
		MPSGraphGruDescriptor Create ();
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MPSGraphObject))]
	interface MPSGraphImToColOpDescriptor : NSCopying {
		[Export ("kernelWidth")]
		nuint KernelWidth { get; set; }

		[Export ("kernelHeight")]
		nuint KernelHeight { get; set; }

		[Export ("strideInX")]
		nuint StrideInX { get; set; }

		[Export ("strideInY")]
		nuint StrideInY { get; set; }

		[Export ("dilationRateInX")]
		nuint DilationRateInX { get; set; }

		[Export ("dilationRateInY")]
		nuint DilationRateInY { get; set; }

		[Export ("paddingLeft")]
		nuint PaddingLeft { get; set; }

		[Export ("paddingRight")]
		nuint PaddingRight { get; set; }

		[Export ("paddingTop")]
		nuint PaddingTop { get; set; }

		[Export ("paddingBottom")]
		nuint PaddingBottom { get; set; }

		[Export ("dataLayout", ArgumentSemantic.Assign)]
		MPSGraphTensorNamedDataLayout DataLayout { get; set; }

		[Static]
		[Export ("descriptorWithKernelWidth:kernelHeight:strideInX:strideInY:dilationRateInX:dilationRateInY:paddingLeft:paddingRight:paddingTop:paddingBottom:dataLayout:")]
		[return: NullAllowed]
		MPSGraphImToColOpDescriptor Create (nuint kernelWidth, nuint kernelHeight, nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom, MPSGraphTensorNamedDataLayout dataLayout);

		[Static]
		[Export ("descriptorWithKernelWidth:kernelHeight:strideInX:strideInY:dilationRateInX:dilationRateInY:dataLayout:")]
		[return: NullAllowed]
		MPSGraphImToColOpDescriptor Create (nuint kernelWidth, nuint kernelHeight, nuint strideInX, nuint strideInY, nuint dilationRateInX, nuint dilationRateInY, MPSGraphTensorNamedDataLayout dataLayout);

		[Export ("setExplicitPaddingWithPaddingLeft:paddingRight:paddingTop:paddingBottom:")]
		void SetExplicitPadding (nuint paddingLeft, nuint paddingRight, nuint paddingTop, nuint paddingBottom);
	}

	[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
	[BaseType (typeof (MPSGraphObject), Name = "MPSGraphLSTMDescriptor")]
	interface MPSGraphLstmDescriptor : NSCopying {
		[Export ("reverse")]
		bool Reverse { get; set; }

		[Export ("bidirectional")]
		bool Bidirectional { get; set; }

		[Export ("produceCell")]
		bool ProduceCell { get; set; }

		[Export ("training")]
		bool Training { get; set; }

		[Export ("forgetGateLast")]
		bool ForgetGateLast { get; set; }

		[Export ("inputGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation InputGateActivation { get; set; }

		[Export ("forgetGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation ForgetGateActivation { get; set; }

		[Export ("cellGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation CellGateActivation { get; set; }

		[Export ("outputGateActivation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation OutputGateActivation { get; set; }

		[Export ("activation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation Activation { get; set; }

		[Static]
		[Export ("descriptor")]
		[return: NullAllowed]
		MPSGraphLstmDescriptor Create ();
	}

	[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
	[BaseType (typeof (MPSGraphObject), Name = "MPSGraphSingleGateRNNDescriptor")]
	interface MPSGraphSingleGateRnnDescriptor : NSCopying {
		[Export ("reverse")]
		bool Reverse { get; set; }

		[Export ("bidirectional")]
		bool Bidirectional { get; set; }

		[Export ("training")]
		bool Training { get; set; }

		[Export ("activation", ArgumentSemantic.Assign)]
		MPSGraphRnnActivation Activation { get; set; }

		[Static]
		[Export ("descriptor")]
		[return: NullAllowed]
		MPSGraphSingleGateRnnDescriptor Create ();
	}

	// @interface MPSGraphSortOps (MPSGraph)
	[TV (14, 0), Mac (11, 0), iOS (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphSortOps {
		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("sortWithTensor:axis:descending:name:")]
		MPSGraphTensor Sort (MPSGraphTensor tensor, nint axis, bool descending, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("sortWithTensor:axisTensor:descending:name:")]
		MPSGraphTensor Sort (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool descending, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("sortWithTensor:axis:name:")]
		MPSGraphTensor Sort (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("sortWithTensor:axisTensor:name:")]
		MPSGraphTensor Sort (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("argSortWithTensor:axis:descending:name:")]
		MPSGraphTensor ArgSort (MPSGraphTensor tensor, nint axis, bool descending, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("argSortWithTensor:axisTensor:descending:name:")]
		MPSGraphTensor ArgSort (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool descending, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("argSortWithTensor:axis:name:")]
		MPSGraphTensor ArgSort (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
		[Export ("argSortWithTensor:axisTensor:name:")]
		MPSGraphTensor ArgSort (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);
	}

	[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphLinearAlgebraOps {
		[Export ("bandPartWithTensor:numLower:numUpper:name:")]
		MPSGraphTensor BandPart (MPSGraphTensor inputTensor, nint numLower, nint numUpper, [NullAllowed] string name);

		[Export ("bandPartWithTensor:numLowerTensor:numUpperTensor:name:")]
		MPSGraphTensor BandPart (MPSGraphTensor inputTensor, MPSGraphTensor numLowerTensor, MPSGraphTensor numUpperTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphImToColOps (MPSGraph)
	[TV (14, 0), Mac (11, 0), iOS (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphImToColOps {
		[Export ("imToColWithSourceTensor:descriptor:name:")]
		MPSGraphTensor ImToCol (MPSGraphTensor source, MPSGraphImToColOpDescriptor descriptor, [NullAllowed] string name);

		[Export ("colToImWithSourceTensor:outputShape:descriptor:name:")]
		MPSGraphTensor ColToIm (MPSGraphTensor source, [BindAs (typeof (int []))] NSNumber [] outputShape, MPSGraphImToColOpDescriptor descriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphCumulativeOps (MPSGraph)
	[TV (14, 0), Mac (11, 0), iOS (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphCumulativeOps {
		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeSumWithTensor:axis:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeSum (MPSGraphTensor tensor, nint axis, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeSumWithTensor:axisTensor:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeSum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeSumWithTensor:axis:name:")]
		MPSGraphTensor CumulativeSum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeSumWithTensor:axisTensor:name:")]
		MPSGraphTensor CumulativeSum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeProductWithTensor:axis:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeProduct (MPSGraphTensor tensor, nint axis, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeProductWithTensor:axisTensor:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeProduct (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeProductWithTensor:axis:name:")]
		MPSGraphTensor CumulativeProduct (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeProductWithTensor:axisTensor:name:")]
		MPSGraphTensor CumulativeProduct (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMinimumWithTensor:axis:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeMinimum (MPSGraphTensor tensor, nint axis, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMinimumWithTensor:axisTensor:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeMinimum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMinimumWithTensor:axis:name:")]
		MPSGraphTensor CumulativeMinimum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMinimumWithTensor:axisTensor:name:")]
		MPSGraphTensor CumulativeMinimum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMaximumWithTensor:axis:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeMaximum (MPSGraphTensor tensor, nint axis, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMaximumWithTensor:axisTensor:exclusive:reverse:name:")]
		MPSGraphTensor CumulativeMaximum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, bool exclusive, bool reverse, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMaximumWithTensor:axis:name:")]
		MPSGraphTensor CumulativeMaximum (MPSGraphTensor tensor, nint axis, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("cumulativeMaximumWithTensor:axisTensor:name:")]
		MPSGraphTensor CumulativeMaximum (MPSGraphTensor tensor, MPSGraphTensor axisTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphFourierTransformOps (MPSGraph)
	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphFourierTransformOps {
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("fastFourierTransformWithTensor:axes:descriptor:name:")]
		MPSGraphTensor FastFourierTransform (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("fastFourierTransformWithTensor:axesTensor:descriptor:name:")]
		MPSGraphTensor FastFourierTransform (MPSGraphTensor tensor, MPSGraphTensor axesTensor, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("realToHermiteanFFTWithTensor:axes:descriptor:name:")]
		MPSGraphTensor RealToHermiteanFft (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("realToHermiteanFFTWithTensor:axesTensor:descriptor:name:")]
		MPSGraphTensor RealToHermiteanFft (MPSGraphTensor tensor, MPSGraphTensor axesTensor, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("HermiteanToRealFFTWithTensor:axes:descriptor:name:")]
		MPSGraphTensor HermiteanToRealFft (MPSGraphTensor tensor, [BindAs (typeof (int []))] NSNumber [] axes, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);

		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("HermiteanToRealFFTWithTensor:axesTensor:descriptor:name:")]
		MPSGraphTensor HermiteanToRealFft (MPSGraphTensor tensor, MPSGraphTensor axesTensor, MPSGraphFftDescriptor descriptor, [NullAllowed] string name);
	}

	// @interface MPSGraphGatherAlongAxisOps (MPSGraph)
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphGatherAlongAxisOps {
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("gatherAlongAxis:withUpdatesTensor:indicesTensor:name:")]
		MPSGraphTensor GatherAlongAxis (nint axis, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("gatherAlongAxisTensor:withUpdatesTensor:indicesTensor:name:")]
		MPSGraphTensor GatherAlongAxis (MPSGraphTensor axisTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphRNNOps (MPSGraph)
	[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphRnnOps {
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNWithSourceTensor:recurrentWeight:inputWeight:bias:initState:mask:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnn (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor mask, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNWithSourceTensor:recurrentWeight:inputWeight:bias:initState:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnn (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNWithSourceTensor:recurrentWeight:initState:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnn (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor initState, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:stateGradient:inputWeight:bias:initState:mask:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnnGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, [NullAllowed] MPSGraphTensor stateGradient, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor mask, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:inputWeight:bias:initState:mask:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnnGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor mask, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:inputWeight:bias:initState:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnnGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("singleGateRNNGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:initState:descriptor:name:")]
		MPSGraphTensor [] SingleGateRnnGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, [NullAllowed] MPSGraphTensor initState, MPSGraphSingleGateRnnDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMWithSourceTensor:recurrentWeight:inputWeight:bias:initState:initCell:mask:peephole:descriptor:name:")]
		MPSGraphTensor [] Lstm (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, [NullAllowed] MPSGraphTensor mask, [NullAllowed] MPSGraphTensor peephole, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMWithSourceTensor:recurrentWeight:inputWeight:bias:initState:initCell:descriptor:name:")]
		MPSGraphTensor [] Lstm (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMWithSourceTensor:recurrentWeight:initState:initCell:descriptor:name:")]
		MPSGraphTensor [] Lstm (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:cellOutputFwd:stateGradient:cellGradient:inputWeight:bias:initState:initCell:mask:peephole:descriptor:name:")]
		MPSGraphTensor [] LstmGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor cellOutputFwd, [NullAllowed] MPSGraphTensor stateGradient, [NullAllowed] MPSGraphTensor cellGradient, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, [NullAllowed] MPSGraphTensor mask, [NullAllowed] MPSGraphTensor peephole, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:cellOutputFwd:inputWeight:bias:initState:initCell:mask:descriptor:name:")]
		MPSGraphTensor [] LstmGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor cellOutputFwd, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, [NullAllowed] MPSGraphTensor mask, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:cellOutputFwd:inputWeight:bias:initState:initCell:descriptor:name:")]
		MPSGraphTensor [] LstmGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor cellOutputFwd, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor initCell, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("LSTMGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:cellOutputFwd:descriptor:name:")]
		MPSGraphTensor [] LstmGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor cellOutputFwd, MPSGraphLstmDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUWithSourceTensor:recurrentWeight:inputWeight:bias:initState:mask:secondaryBias:descriptor:name:")]
		MPSGraphTensor [] Gru (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor mask, [NullAllowed] MPSGraphTensor secondaryBias, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUWithSourceTensor:recurrentWeight:inputWeight:bias:initState:descriptor:name:")]
		MPSGraphTensor [] Gru (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUWithSourceTensor:recurrentWeight:inputWeight:bias:descriptor:name:")]
		MPSGraphTensor [] Gru (MPSGraphTensor source, MPSGraphTensor recurrentWeight, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:outputFwd:stateGradient:inputWeight:bias:initState:mask:secondaryBias:descriptor:name:")]
		MPSGraphTensor [] GruGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor outputFwd, [NullAllowed] MPSGraphTensor stateGradient, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, [NullAllowed] MPSGraphTensor mask, [NullAllowed] MPSGraphTensor secondaryBias, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:outputFwd:inputWeight:bias:initState:descriptor:name:")]
		MPSGraphTensor [] GruGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor outputFwd, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, [NullAllowed] MPSGraphTensor initState, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("GRUGradientsWithSourceTensor:recurrentWeight:sourceGradient:zState:outputFwd:inputWeight:bias:descriptor:name:")]
		MPSGraphTensor [] GruGradients (MPSGraphTensor source, MPSGraphTensor recurrentWeight, MPSGraphTensor sourceGradient, MPSGraphTensor zState, MPSGraphTensor outputFwd, [NullAllowed] MPSGraphTensor inputWeight, [NullAllowed] MPSGraphTensor bias, MPSGraphGruDescriptor descriptor, [NullAllowed] string name);
	}

	[TV (16, 1), Mac (13, 0), iOS (16, 1), MacCatalyst (16, 1)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphMatrixInverseOps {
		[Export ("inverseOfTensor:name:")]
		MPSGraphTensor Inverse (MPSGraphTensor inputTensor, [NullAllowed] string name);
	}

	// @interface MPSGraphNonMaximumSuppressionOps (MPSGraph)
	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphNonMaximumSuppressionOps {
		[Export ("nonMaximumSuppressionWithBoxesTensor:scoresTensor:IOUThreshold:scoreThreshold:perClassSuppression:coordinateMode:name:")]
		MPSGraphTensor NonMaximumSuppression (MPSGraphTensor boxesTensor, MPSGraphTensor scoresTensor, float iouThreshold, float scoreThreshold, bool perClassSuppression, MPSGraphNonMaximumSuppressionCoordinateMode coordinateMode, [NullAllowed] string name);

		[Export ("nonMaximumSuppressionWithBoxesTensor:scoresTensor:classIndicesTensor:IOUThreshold:scoreThreshold:perClassSuppression:coordinateMode:name:")]
		MPSGraphTensor NonMaximumSuppression (MPSGraphTensor boxesTensor, MPSGraphTensor scoresTensor, MPSGraphTensor classIndicesTensor, float iouThreshold, float scoreThreshold, bool perClassSuppression, MPSGraphNonMaximumSuppressionCoordinateMode coordinateMode, [NullAllowed] string name);
	}

	// @interface NonZeroOps (MPSGraph)
	[TV (14, 0), Mac (11, 0), iOS (14, 0), MacCatalyst (14, 0)]
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_NonZeroOps {
		[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("nonZeroIndicesOfTensor:name:")]
		MPSGraphTensor NonZeroIndices (MPSGraphTensor tensor, [NullAllowed] string name);
	}

	// @interface MPSGraphSampleGrid (MPSGraph)
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphSampleGrid {
		[TV (16, 2), Mac (13, 1), iOS (16, 2), MacCatalyst (16, 2)]
		[Export ("sampleGridWithSourceTensor:coordinateTensor:layout:normalizeCoordinates:relativeCoordinates:alignCorners:paddingMode:samplingMode:constantValue:name:")]
		MPSGraphTensor SampleGrid (MPSGraphTensor source, MPSGraphTensor coordinates, MPSGraphTensorNamedDataLayout layout, bool normalizeCoordinates, bool relativeCoordinates, bool alignCorners, MPSGraphPaddingMode paddingMode, MPSGraphResizeMode samplingMode, double constantValue, [NullAllowed] string name);

		[TV (16, 2), Mac (13, 1), iOS (16, 2), MacCatalyst (16, 2)]
		[Export ("sampleGridWithSourceTensor:coordinateTensor:layout:normalizeCoordinates:relativeCoordinates:alignCorners:paddingMode:nearestRoundingMode:constantValue:name:")]
		MPSGraphTensor SampleGrid (MPSGraphTensor source, MPSGraphTensor coordinates, MPSGraphTensorNamedDataLayout layout, bool normalizeCoordinates, bool relativeCoordinates, bool alignCorners, MPSGraphPaddingMode paddingMode, MPSGraphResizeNearestRoundingMode nearestRoundingMode, double constantValue, [NullAllowed] string name);
	}

	// @interface MPSGraphScatterAlongAxisOps (MPSGraph)
	[Category]
	[BaseType (typeof (MPSGraph))]
	interface MPSGraph_MPSGraphScatterAlongAxisOps {
		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("scatterAlongAxis:withUpdatesTensor:indicesTensor:shape:mode:name:")]
		MPSGraphTensor ScatterAlongAxis (nint axis, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [BindAs (typeof (int []))] NSNumber [] shape, MPSGraphScatterMode mode, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("scatterAlongAxisTensor:withUpdatesTensor:indicesTensor:shape:mode:name:")]
		MPSGraphTensor ScatterAlongAxis (MPSGraphTensor axisTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, [BindAs (typeof (int []))] NSNumber [] shape, MPSGraphScatterMode mode, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("scatterAlongAxis:withDataTensor:updatesTensor:indicesTensor:mode:name:")]
		MPSGraphTensor ScatterAlongAxis (nint axis, MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, MPSGraphScatterMode mode, [NullAllowed] string name);

		[TV (15, 4), Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("scatterAlongAxisTensor:withDataTensor:updatesTensor:indicesTensor:mode:name:")]
		MPSGraphTensor ScatterAlongAxis (MPSGraphTensor axisTensor, MPSGraphTensor dataTensor, MPSGraphTensor updatesTensor, MPSGraphTensor indicesTensor, MPSGraphScatterMode mode, [NullAllowed] string name);
	}

}
