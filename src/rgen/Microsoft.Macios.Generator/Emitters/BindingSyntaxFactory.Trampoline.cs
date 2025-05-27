// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	/// <summary>
	/// Returns the statement with the return type of the invoke method for the given type info delegate.
	/// </summary>
	/// <param name="typeInfo"></param>
	/// <param name="auxVariableName"></param>
	/// <returns></returns>
	internal static ExpressionSyntax? GetTrampolineInvokeReturnType (TypeInfo typeInfo, string auxVariableName)
	{
		// ignore those types that are not delegates or that are a delegate with a void return type
		if (!typeInfo.IsDelegate || typeInfo.Delegate.ReturnType.IsVoid)
			return null;

#pragma warning disable format
		// based on the return type of the delegate we build a statement that will return the expected value
		return typeInfo.Delegate.ReturnType switch {

			//  Runtime.RetainAndAutoreleaseNSObject (NSArray.FromNSObjects(auxVariable))
			{ IsArray: true, ArrayElementTypeIsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument(NSArrayFromNSObjects ([Argument (IdentifierName (auxVariableName))]))]),
			
			// Runtime.RetainAndAutoreleaseNativeObject (auxVariable)
			{ IsArray: false, IsINativeObject: true, IsNSObject: false, IsInterface: false}
				=> RetainAndAutoreleaseNativeObject ([Argument(IdentifierName (auxVariableName))]),
			
			// Runtime.RetainAndAutoreleaseNSObject (auxVariable)
			{ IsArray: false, IsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument (IdentifierName(auxVariableName))]),
			
			//  NSString.CreateNative (auxVariable, true);
			{ SpecialType: SpecialType.System_String }
				=> NStringCreateNative ([Argument (IdentifierName(auxVariableName)), BoolArgument (true)]),
			
			// (UIntPtr) (ulong) myParam 
			{ IsNativeEnum: true }
				=> CastToNative (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// auxVariable ? (byte) 1 : (byte) 0; 
			{ SpecialType: SpecialType.System_Boolean } 
				=> CastToByte (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// default case, return the value as is
			_ => IdentifierName (auxVariableName),

		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns the expression for the creation of the NativeInvocationClass for a given trampoline.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose class we want to create.</param>
	/// <param name="arguments">The arguments for pass to the create method.</param>
	/// <returns>The expression needed to create the native invocation class.</returns>
	internal static ExpressionSyntax CreateTrampolineNativeInvocationClass (string trampolineName, ImmutableArray<ArgumentSyntax> arguments)
	{
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.NativeInvocationClass);
		var staticClassName = IdentifierName (className);
		return StaticInvocationExpression (staticClassName, "Create", arguments, suppressNullableWarning: true);
	}


	/// <summary>
	/// Returns the needed data to build the parameter syntax for the native trampoline delegate.
	/// </summary>
	/// <param name="trampolineName">The trampoline name of the parameter we want to generate.</param>
	/// <param name="parameter">The parameter we want to generate for the lower invoke method.</param>
	/// <returns>The parameter syntax needed for the parameter.</returns>
	internal static ParameterSyntax GetTrampolineInvokeParameter (string trampolineName, in DelegateParameter parameter)
	{
		var parameterIdentifier = Identifier (parameter.Name);
#pragma warning disable format
		(SyntaxToken ParameterName, TypeSyntax? ParameterType) parameterInfo = parameter switch {
			// pointer parameter 
			{ Type.IsPointer: true } 
				=> (parameterIdentifier, 
					parameter.Type.GetIdentifierSyntax ()),
			
			// parameters that are passed by reference, depend on the type that is referenced
			{ IsByRef: true, Type.IsReferenceType: false, Type.IsNullable: true} 
				=> (parameterIdentifier, 
					PointerType (parameter.Type.GetIdentifierSyntax ())),
			
			{ IsByRef: true, Type.SpecialType: SpecialType.System_Boolean} 
				=> (parameterIdentifier,
					PointerType (PredefinedType (Token(SyntaxKind.ByteKeyword)))),
			
			{ IsByRef: true, Type.IsReferenceType: true, Type.IsNullable: false} 
				=> (parameterIdentifier,
					PointerType (NativeHandle)),
			
			// delegate parameter is a NativeHandle
			{ Type.IsDelegate: true } => (parameterIdentifier, IntPtr),
			
			// native enum, return the conversion expression to the native type
			{ Type.IsNativeEnum: true}
				=> (parameterIdentifier, IdentifierName(parameter.Type.EnumUnderlyingType!.Value.GetKeyword ())),

			// boolean, convert it to byte
			{ Type.SpecialType: SpecialType.System_Boolean }
				=> (parameterIdentifier, 
					PredefinedType (Token(SyntaxKind.ByteKeyword))),

			// same name, native handle
			{ Type.IsArray: true }
				=> (parameterIdentifier, NativeHandle),

			// string
			// same name, native handle
			{ Type.SpecialType: SpecialType.System_String }
				=> (parameterIdentifier, NativeHandle),

			// same name, NativeHandle
			{ Type.IsProtocol: true } => (parameterIdentifier, NativeHandle),

			// same name, NativeHandle
			{ ForcedType: not null } => (parameterIdentifier, NativeHandle),

			// special types

			// CoreMedia.CMSampleBuffer
			// same name, native handle
			{ Type.FullyQualifiedName: "CoreMedia.CMSampleBuffer" } => (parameterIdentifier, NativeHandle),

			// AudioToolbox.AudioBuffers
			// same name, native handle
			{ Type.FullyQualifiedName: "AudioToolbox.AudioBuffers" } => (parameterIdentifier, NativeHandle),

			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case

			// same name, native handle
			{ Type.IsNSObject: true } => (parameterIdentifier, NativeHandle),

			// same name, native handle
			{ Type.IsINativeObject: true } => (parameterIdentifier, NativeHandle),
			
			// by default, we will use the parameter name as is and the type of the parameter
			_ => (parameterIdentifier, parameter.Type.GetIdentifierSyntax ()),
		};
#pragma warning restore format
		
		return Parameter (parameterInfo.ParameterName)
			.WithType (parameterInfo.ParameterType)
			.NormalizeWhitespace ();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="typeInfo"></param>
	/// <returns></returns>
	internal static TypeSyntax GetLowLevelType (in TypeInfo typeInfo)
	{
#pragma warning disable format
		return typeInfo switch {
			// pointer parameter 
			{ IsPointer: true } => typeInfo.GetIdentifierSyntax (),
			
			// delegate parameter is a NativeHandle
			{ IsDelegate: true } => IntPtr,
			
			// native enum, return the conversion expression to the native type
			{ IsNativeEnum: true} =>  IdentifierName(typeInfo.EnumUnderlyingType!.Value.GetKeyword ()),

			// boolean, convert it to byte
			{ SpecialType: SpecialType.System_Boolean } => PredefinedType (Token(SyntaxKind.ByteKeyword)),

			{ IsArray: true } => NativeHandle,

			{ SpecialType: SpecialType.System_String } =>  NativeHandle,

			{ IsProtocol: true } => NativeHandle,

			// special types

			// CoreMedia.CMSampleBuffer
			{ FullyQualifiedName: "CoreMedia.CMSampleBuffer" } => NativeHandle,

			// AudioToolbox.AudioBuffers
			{ FullyQualifiedName: "AudioToolbox.AudioBuffers" } => NativeHandle,

			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case

			// same name, native handle
			{ IsNSObject: true } => NativeHandle,

			// same name, native handle
			{ IsINativeObject: true } => NativeHandle,
			
			// by default, we will use the parameter name as is and the type of the parameter
			_ => typeInfo.GetIdentifierSyntax (),
		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns the needed data to build the parameter syntax for the native trampoline delegate.
	/// </summary>
	/// <param name="parameter">The parameter we want to generate for the lower invoke method.</param>
	/// <returns>The parameter syntax needed for the parameter.</returns>
	internal static (SyntaxToken ParameterName, TypeSyntax ParameterType) GetTrampolineInvokeParameter (in DelegateParameter parameter)
	{
		// in the general case we will return the low level type conversion of the parameter type but we 
		// need to handle in a special case those parameters that are passed by reference
		var parameterIdentifier = Identifier (parameter.Name);
#pragma warning disable format
		(SyntaxToken ParameterName, TypeSyntax ParameterType) parameterInfo = parameter switch {
			// parameters that are passed by reference, depend on the type that is referenced
			{ IsByRef: true, Type.IsReferenceType: false, Type.IsNullable: true} 
				=> (parameterIdentifier, 
					PointerType (parameter.Type.ToNonNullable ().GetIdentifierSyntax ())),
			
			{ IsByRef: true, Type.SpecialType: SpecialType.System_Boolean} 
				=> (parameterIdentifier,
					PointerType (PredefinedType (Token(SyntaxKind.ByteKeyword)))),
			
			{ IsByRef: true, Type.IsReferenceType: true, Type.IsNullable: false} 
				=> (parameterIdentifier,
					PointerType (NativeHandle)),
			
			// by default, we will use the parameter name as is and the type of the parameter
			_ => (parameterIdentifier, GetLowLevelType (parameter.Type)),
		};
#pragma warning restore format
		return parameterInfo;
	}

	/// <summary>
	/// Returns the argument syntax of a parameter to be used for the trampoliner to invoke a delegate.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose parameter we are generating.</param>
	/// <param name="parameter">The parameter whose argument syntax has to be calculated.</param>
	/// <returns>The argument syntax for the given parameter.</returns>
	internal static ArgumentSyntax GetTrampolineInvokeArgument (string trampolineName, in DelegateParameter parameter)
	{
		// build the needed expression based on the information of the parameter.
		var parameterIdentifier = IdentifierName (parameter.Name);
#pragma warning disable format
		var expression = parameter switch {
			// pointer parameter 
			{ Type.IsPointer: true } => parameterIdentifier,
			
			// parameters that are passed by reference, the nomenclator will return the name of the
			// temporary variable to use for the trampoline, there is no need for us to do anything
			{ IsByRef: true } => IdentifierName (Nomenclator.GetNameForTempTrampolineVariable (parameter) ?? parameter.Name),
			
			// delegate parameter, c callback
			// System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ParameterType> (ParameterName)
			{ Type.IsDelegate: true, IsCCallback: true } => 
				GetDelegateForFunctionPointer (parameter.Type.GetIdentifierSyntax (), [Argument (parameterIdentifier)]),
			
			// delegate parameter, block callback
			// TrampolineNativeInvocationClass.Create (ParameterName)!
			{ Type.IsDelegate: true, IsBlockCallback: true } 
				=> CreateTrampolineNativeInvocationClass (trampolineName, [Argument (parameterIdentifier)]),
			
			// native enum, return the conversion expression to the native type
			{ Type.IsNativeEnum: true} 
				=> CastToNative (parameter)!,
			
			// boolean, convert it to byte
			{ Type.SpecialType: SpecialType.System_Boolean } 
				=> CastToBool (parameter.Name, parameter.Type)!,
			
			// array types
			
			// CFArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementTypeIsWrapped: true } 
				=> GetCFArrayFromHandle (parameter.Type.ToArrayElementType ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true), 
			
			// NSArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementIsINativeObject: true } 
				=> GetNSArrayFromHandle (parameter.Type.ToArrayElementType ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// string[]
			// CFArray.StringArrayFromHandle (ParameterName)!
			{ Type.IsArray: true, Type.ArrayElementType: SpecialType.System_String }
				=> SuppressNullableWarning (StringArrayFromHandle ([Argument (parameterIdentifier)])),
			
			// string
			// CFString.FromHandle (ParameterName)!
			{ Type.SpecialType: SpecialType.System_String }
				=> SuppressNullableWarning (StringFromHandle ([Argument (parameterIdentifier)])),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsProtocol: true } => 
				GetINativeObject (parameter.Type.GetIdentifierSyntax (), [
						Argument (parameterIdentifier), 
						BoolArgument (false)
					], suppressNullableWarning: true),
			// Runtime.GetINativeObject<ParameterType> (ParameterName, true, Forced.Owns)!
			{ ForcedType: not null } => GetINativeObject (parameter.Type.GetIdentifierSyntax (), 
				[
					Argument (parameterIdentifier),
					BoolArgument (true),
					BoolArgument (parameter.ForcedType.Value.Owns)
				], suppressNullableWarning: true),
			
			// special types
			
			// CoreMedia.CMSampleBuffer
			// {0} == IntPtr.Zero ? null! : new global::CoreMedia.CMSampleBuffer ({0}, false)
			{ Type.FullyQualifiedName: "CoreMedia.CMSampleBuffer" } =>
				IntPtrZeroCheck (parameter.Name, 
					expressionSyntax: New (parameter.Type, [Argument (parameterIdentifier), BoolArgument (false)]), 
					suppressNullableWarning: true),
			
			// AudioToolbox.AudioBuffers
			// new global::AudioToolbox.AudioBuffers ({0})
			{ Type.FullyQualifiedName: "AudioToolbox.AudioBuffers" } =>
				New (parameter.Type, [Argument (parameterIdentifier)]),
			
			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case
			
			// Runtime.GetNSObject<ParameterType> (ParameterName)! 
			{ Type.IsNSObject: true } =>
				GetNSObject (parameter.Type.GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsINativeObject: true } =>
				GetINativeObject (parameter.Type.GetIdentifierSyntax (), [
					Argument (parameterIdentifier), 
					BoolArgument (false)
				], suppressNullableWarning: true),
			
			// by default, we will use the parameter name as is
			_ => parameterIdentifier
		};
#pragma warning restore format
		
		// this are arguments no parameters, therefore we do not need to add the ref modifiers
		return Argument (expression);
	}

	internal static ImmutableArray<SyntaxNode> GetTrampolinePreInvokeByRefArgument (in DelegateParameter parameter)
	{
		// there are two cases in which we need to do something with the byref parameters:
		// 1. the parameter is by ref and nullable, we need to create a temporary variable to hold the value
		// 2. the parameter is a boolean type and we need a conversion
		// any other case we can just use the parameter as is and we will return an empty array
		var tempVariableName = Nomenclator.GetNameForTempTrampolineVariable (parameter);
		if (tempVariableName is null)
			return [];

		if (parameter.Type.IsNullable) {
			// declare a new variable to hold the temp var
			// ParameterType? tempVariable = null;
			var declarationNode = LocalDeclarationStatement (
				VariableDeclaration (parameter.Type.GetIdentifierSyntax ())
					.WithVariables (
						SingletonSeparatedList (
							VariableDeclarator (
									Identifier (tempVariableName))
								.WithInitializer (
									EqualsValueClause (
										LiteralExpression (
											SyntaxKind.NullLiteralExpression))))));
			// check for the parameter being null and assign the value if needed.
			// if (parameterName is not null)
			//     tempVariable =  *parameterName;
			var ifNode = IfStatement (
				IsPatternExpression (
					IdentifierName (parameter.Name),
					UnaryPattern (
						ConstantPattern (
							LiteralExpression (
								SyntaxKind.NullLiteralExpression)))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						IdentifierName (tempVariableName),
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)))));
			return [
				declarationNode,
				ifNode,
			];
		}

		if (parameter.Type.SpecialType == SpecialType.System_Boolean) {
			// generates the following:
			// bool {tempVariable} = *{parameterName} != 0;
			var variableDeclaration = LocalDeclarationStatement (
				VariableDeclaration (
						PredefinedType (
							Token (SyntaxKind.BoolKeyword)))
					.WithVariables (
						SingletonSeparatedList (
							VariableDeclarator (
									Identifier (tempVariableName))
								.WithInitializer (
									EqualsValueClause (
										BinaryExpression (
											SyntaxKind.NotEqualsExpression,
											PrefixUnaryExpression (
												SyntaxKind.PointerIndirectionExpression,
												IdentifierName (parameter.Name)),
											LiteralExpression (
												SyntaxKind.NumericLiteralExpression,
												Literal (0))))))));
			return [variableDeclaration];
		}

		// default case, we do not need to do anything
		return [];
	}

	internal static ImmutableArray<SyntaxNode> GetTrampolinePostInvokeByRefArgument (string trampolineName,
		in DelegateParameter parameter)
	{
		// similar to the pre invoke case, we need to do something with the byref parameters:
		// 1. the parameter is by ref and nullable we need to assign the value
		// 2. the parameter is a boolean type we need to convert back the value from a byte
		var tempVariableName = Nomenclator.GetNameForTempTrampolineVariable (parameter);
		if (tempVariableName is null)
			return [];

		if (parameter.Type.IsNullable) {
			// check if the temp variable has a value and assign it to the parameter
			// if (ParameterName is not null && TempVariable.HasValue)
			//     *ParameterName = TempVariable.Value;
			var ifNode = IfStatement (
				BinaryExpression (
					SyntaxKind.LogicalAndExpression,
					IsPatternExpression (
						IdentifierName (parameter.Name),
						UnaryPattern (
							ConstantPattern (
								LiteralExpression (
									SyntaxKind.NullLiteralExpression)))),
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName (tempVariableName),
						IdentifierName ("HasValue"))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)),
						MemberAccessExpression (
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName (tempVariableName),
							IdentifierName ("Value")))));
			return [ifNode];
		}

		if (parameter.Type.SpecialType == SpecialType.System_Boolean) {
			// set the boolean value from a byte
			// *ParameterName = TempVariable ? (byte) 1 : (byte) 0;
			var assignment = AssignmentExpression (
				SyntaxKind.SimpleAssignmentExpression,
				PrefixUnaryExpression (
					SyntaxKind.PointerIndirectionExpression,
					IdentifierName (parameter.Name)),
				ConditionalExpression (
					IdentifierName (tempVariableName),
					CastExpression (
						PredefinedType (
							Token (SyntaxKind.ByteKeyword)),
						LiteralExpression (
							SyntaxKind.NumericLiteralExpression,
							Literal (1))).WithTrailingTrivia (Space),
					CastExpression (
						PredefinedType (
							Token (SyntaxKind.ByteKeyword)),
						LiteralExpression (
							SyntaxKind.NumericLiteralExpression,
							Literal (0).WithLeadingTrivia (Space)))));
			return [ExpressionStatement (assignment)];
		}

		if (parameter.Type.IsReferenceType) {
			// assign the value of the temp variable to the parameter	
			var assignment = IfStatement (
				IsPatternExpression (
					IdentifierName (parameter.Name),
					UnaryPattern (
						ConstantPattern (
							LiteralExpression (
								SyntaxKind.NullLiteralExpression)))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)),
						InvocationExpression (
								MemberAccessExpression (
									SyntaxKind.SimpleMemberAccessExpression,
									IdentifierName ("Runtime"),
									IdentifierName ("RetainAndAutoreleaseNativeObject").WithTrailingTrivia (Space)))
							.WithArgumentList (
								ArgumentList (
									SingletonSeparatedList (
										Argument (
											IdentifierName (tempVariableName))))))));
			return [assignment];
		}
		return [];
	}

	/// <summary>
	/// Returns the list of expressions that need to be executed before the trampoline is invoked. This allows to
	/// help the trampoline to convert the parameters to the expected types.
	/// </summary>
	/// <param name="trampolineName">The trampoline name to which the conversion is needed.</param>
	/// <param name="parameter">The parameters whose conversions we need.</param>
	/// <returns>An immutable array with the needed conversion expressions. Empty is return if no conversion
	/// is needed.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolinePreInvokeArgumentConversions (string trampolineName,
		in DelegateParameter parameter)
	{
		// decide the type of conversion we need to do based on the type of the parameter
		return parameter switch { { IsByRef: true } => GetTrampolinePreInvokeByRefArgument (parameter),
			_ => []
		};
	}

	internal static ImmutableArray<SyntaxNode> GetTrampolinePostInvokeArgumentConversions (string trampolineName,
		in DelegateParameter parameter)
	{
		// decide the type of conversion we need to do based on the type of the parameter
		return parameter switch { { IsByRef: true } => GetTrampolinePostInvokeByRefArgument (trampolineName, parameter),
			_ => []
		};
	}

	/// <summary>
	/// Return a immutable array of arguments to be used for the trampoline invoke method. The arguments are all
	/// the different expressions needed to pass the parameters to the trampoline.
	/// </summary>
	/// <param name="trampolineName">The trampoline whose parameters we are generating.</param>
	/// <param name="delegateInfo">The delegate info of the trampoline we are generating.</param>
	/// <returns>An immutable array with the argument expressions needed to invoke the trampoline delegate.</returns>
	internal static ImmutableArray<TrampolineArgumentSyntax> GetTrampolineInvokeArguments (string trampolineName,
		in DelegateInfo delegateInfo)
	{
		// create the builder for the arguments, we already know the size of the array
		var bucket = ImmutableArray.CreateBuilder<TrampolineArgumentSyntax> (delegateInfo.Parameters.Length);
		foreach (var parameter in delegateInfo.Parameters) {
			var argument = new TrampolineArgumentSyntax (GetTrampolineInvokeArgument (trampolineName, parameter)) {
				PreDelegateCallConversion = GetTrampolinePreInvokeArgumentConversions (trampolineName, parameter),
				PostDelegateCallConversion = GetTrampolinePostInvokeArgumentConversions (trampolineName, parameter),
			};
			bucket.Add (argument);
		}
		return bucket.ToImmutable ();
	}

	/// <summary>
	/// Generates the call to the trampoline delegate.
	/// </summary>
	/// <param name="delegateInfo">The information of the delegate.</param>
	/// <param name="argumentSyntax">The argument syntax for the parameters of the delegate.</param>
	/// <returns>The needed statement to call the delegate with the parameters of the trampoline.</returns>
	internal static StatementSyntax CallTrampolineDelegate (in DelegateInfo delegateInfo,
		in ImmutableArray<TrampolineArgumentSyntax> argumentSyntax)
	{
		// we always need to create a block that performs the call to the trampoline variable with the argument syntax
		// these arguments already have all the needed conversions
		var args = argumentSyntax
			.Select (x => x.ArgumentSyntax)
			.ToImmutableArray ();
		var invocation = InvocationExpression (
				IdentifierName (Nomenclator.GetTrampolineDelegateVariableName ()).WithTrailingTrivia (Space))
			.WithArgumentList (ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ())));

		// return the invocation expression if the delegate return type is a void type
		if (delegateInfo.ReturnType.IsVoid)
			return ExpressionStatement (invocation);

		// perform an assigment to the return variable
		var declaration = VariableDeclaration (
				IdentifierName (
					Identifier (
						TriviaList (),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						TriviaList (Space))))
			.WithVariables (
				SingletonSeparatedList (
					VariableDeclarator (
							Identifier (Nomenclator.GetReturnVariableName ()))
						.WithInitializer (
							EqualsValueClause (invocation.WithLeadingTrivia (Space)).WithLeadingTrivia (Space))));
		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Helper method to get the parameters of the trampoline delegate and its invoke implementation.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate type info.</param>
	/// <returns>The parameter list for the delegate to be used in the trampoline.</returns>
	static ParameterListSyntax GetBlockDelegateParameters (in TypeInfo delegateTypeInfo)
	{
		// build the arguments for the delegate, but add a IntPtr parameter at the start of the list 
		var parameterBucket = ImmutableArray.CreateBuilder<ParameterSyntax> (delegateTypeInfo.Delegate!.Parameters.Length + 1);
		// block parameter needed for the trampoline
		parameterBucket.Add (
			Parameter (Identifier (Nomenclator.GetTrampolineBlockParameterName (delegateTypeInfo.Delegate!.Parameters)))
				.WithType (IntPtr));
		// calculate the rest of the parameters  
		foreach (var currentParameter in delegateTypeInfo.Delegate!.Parameters) {
			// build the parameter
			var parameterInfo = GetTrampolineInvokeParameter (currentParameter);
			var parameter = Parameter (parameterInfo.ParameterName)
				.WithType (parameterInfo.ParameterType)
				.NormalizeWhitespace ();
			parameterBucket.Add (parameter);
		}

		var parametersSyntax = ParameterList (
			SeparatedList<ParameterSyntax> (
				parameterBucket.ToImmutableArray ().ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();
		return parametersSyntax;
	}

	/// <summary>
	/// Return the delegate declaration for the trampoline delegate. The trampoline delegate is a delegate that
	/// takes as a first parameter a IntPtr that represents the block to be called. The rest of the parameters are
	/// the same as the original delegate.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate type information.</param>
	/// <param name="delegateName">The name of the delegate generated.</param>
	/// <returns>The syntax of the delegate.</returns>
	internal static SyntaxNode GetTrampolineDelegateDeclaration (in TypeInfo delegateTypeInfo, out string delegateName)
	{
		// generate a new delegate type with the addition of the IntPtr parameter for block
		var modifiers = TokenList (Token (SyntaxKind.UnsafeKeyword), Token (SyntaxKind.InternalKeyword));
		delegateName = Nomenclator.GetTrampolineClassName (delegateTypeInfo.Name, Nomenclator.TrampolineClassType.DelegateType);

		var parametersSyntax = GetBlockDelegateParameters (delegateTypeInfo);
		// delegate declaration
		var declaration = DelegateDeclaration (
				GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType), // return the low level type, not the manged version
				Identifier (delegateName))
			.WithModifiers (modifiers).NormalizeWhitespace ()
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));

		return declaration;
	}

	/// <summary>
	/// Generatees the delegate pointer variable for the trampoline. The delegate pointer is a function pointer that
	/// takes the same parameters as the delegate but with the addition of a IntPtr parameter that represents
	/// the native block.
	/// </summary>
	/// <param name="delegateTypeInfo">The information of the delegate.</param>
	/// <returns>The expression that defines a variable that holds the function pointer.</returns>
	internal static StatementSyntax GetTrampolineDelegatePointer (in TypeInfo delegateTypeInfo)
	{
		// build the function parameter list
		var parameterBucket = ImmutableArray.CreateBuilder<FunctionPointerParameterSyntax> (delegateTypeInfo.Delegate!.Parameters.Length + 1);
		// block parameter needed for the trampoline
		parameterBucket.Add (FunctionPointerParameter (IntPtr));
		// calculate the rest of the parameters  
		foreach (var currentParameter in delegateTypeInfo.Delegate!.Parameters) {
			// build the parameter
			var (_, parameterType) = GetTrampolineInvokeParameter (currentParameter);
			parameterBucket.Add (FunctionPointerParameter (parameterType));
		}
		// we need to add the return type of the delegate
		parameterBucket.Add (FunctionPointerParameter (GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType)));

		// generates the function parameter list:
		// example:
		// <IntPtr, int, int, int>
		// that is, the block ptr, the parameters and the return type
		var parametersSyntax = FunctionPointerParameterList (
			SeparatedList<FunctionPointerParameterSyntax> (
				parameterBucket.ToImmutableArray ().ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();

		// function pointer type
		var pointerType = FunctionPointerType ()
			.WithCallingConvention (
				FunctionPointerCallingConvention (
					Token (SyntaxKind.UnmanagedKeyword)))
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));
		// declare the delegate pointer variable:
		var declaration = VariableDeclaration (pointerType)
			.WithVariables (SingletonSeparatedList (
				VariableDeclarator (
						Identifier (Nomenclator.GetTrampolineDelegatePointerVariableName ()))
					.WithInitializer (
						EqualsValueClause (
							PrefixUnaryExpression (
								SyntaxKind.AddressOfExpression,
								IdentifierName (Nomenclator.GetTrampolineInvokeMethodName ()))))));

		return LocalDeclarationStatement (declaration.NormalizeWhitespace ());
	}

	/// <summary>
	/// Returns the method declaration for the trampoline invoke method. The trampoline invoke method, this is the
	/// method that will be invoked by the native code.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate whose signature we want to declare.</param>
	/// <returns>The invoke member delcaration.</returns>
	internal static MemberDeclarationSyntax GetTrampolineInvokeSignature (in TypeInfo delegateTypeInfo)
	{
		var modifiers = TokenList (
			Token (SyntaxKind.InternalKeyword),
			Token (SyntaxKind.StaticKeyword),
			Token (SyntaxKind.UnsafeKeyword));
		var parametersSyntax = GetBlockDelegateParameters (delegateTypeInfo);

		var method = MethodDeclaration (
				GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType), // return the low level type, not the manged version
				Identifier (Nomenclator.GetTrampolineInvokeMethodName ()))
			.WithModifiers (modifiers).NormalizeWhitespace ()
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));
		return method;
	}
}
