//
// PencilKit C# bindings
//
// Authors:
//	TJ Lambert  <t-anlamb@microsoft.com>
//	Whitney Schmidt  <whschm@microsoft.com>
//
// Copyright 2019, 2020 Microsoft Corporation All rights reserved.
//

#if MONOMAC
using AppKit;
using UIColor = AppKit.NSColor;
using UIImage = AppKit.NSImage;

using UIBarButtonItem = Foundation.NSObject;
using UIScrollViewDelegate = Foundation.NSObjectProtocol;
using UIScrollView = Foundation.NSObject;
using UIGestureRecognizer = Foundation.NSObject;
using UIResponder = Foundation.NSObject;
using UIView = Foundation.NSObject;
using UIViewController = Foundation.NSObject;
using UIWindow = Foundation.NSObject;
using UIUserInterfaceStyle = Foundation.NSObject;
using BezierPath = AppKit.NSBezierPath;
#else
using UIKit;
using BezierPath = UIKit.UIBezierPath;
#endif

using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;
using CoreGraphics;

namespace PencilKit {

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Native]
	enum PKEraserType : long {
		Vector,
		Bitmap,
		[iOS (16, 4), Mac (13, 3), MacCatalyst (16, 4)]
		FixedWidthBitmap,
	}

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	enum PKInkType {
		[Field ("PKInkTypePen")]
		Pen,

		[Field ("PKInkTypePencil")]
		Pencil,

		[Field ("PKInkTypeMarker")]
		Marker,

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("PKInkTypeMonoline")]
		Monoline,

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("PKInkTypeFountainPen")]
		FountainPen,

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("PKInkTypeWatercolor")]
		Watercolor,

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Field ("PKInkTypeCrayon")]
		Crayon,
	}

	[iOS (14, 0), NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Native]
	enum PKCanvasViewDrawingPolicy : ulong {
		Default,
		AnyInput,
		PencilOnly,
	}

	[iOS (17, 0), Mac (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 17, 0)]
	[Native]
	public enum PKContentVersion : long {
		Version1 = 1,
		Version2 = 2,
		[iOS (17, 4), Mac (14, 4), MacCatalyst (17, 4)]
		Version3 = 3,
	}

	[iOS (13, 0), NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (NSObject))]
	[Protocol, Model]
	interface PKCanvasViewDelegate : UIScrollViewDelegate {

		[Export ("canvasViewDrawingDidChange:")]
		void DrawingDidChange (PKCanvasView canvasView);

		[Export ("canvasViewDidFinishRendering:")]
		void DidFinishRendering (PKCanvasView canvasView);

		[Export ("canvasViewDidBeginUsingTool:")]
		void DidBeginUsingTool (PKCanvasView canvasView);

		[Export ("canvasViewDidEndUsingTool:")]
		void EndUsingTool (PKCanvasView canvasView);

		[iOS (18, 1), NoMacCatalyst]
		[Export ("canvasView:didRefineStrokes:withNewStrokes:")]
		void DidRefineStrokes (PKCanvasView canvasView, PKStroke [] strokes, PKStroke [] newStrokes);
	}

	interface IPKCanvasViewDelegate { }

	[iOS (13, 0), NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (UIScrollView))]
	interface PKCanvasView : PKToolPickerObserver {

		// This exists in the base class
		// [Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		// NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate"), NullAllowed, New]
		IPKCanvasViewDelegate Delegate { get; set; }

		[Export ("drawing", ArgumentSemantic.Copy)]
		PKDrawing Drawing { get; set; }

		[Export ("tool", ArgumentSemantic.Copy)]
		PKTool Tool { get; set; }

		[Export ("rulerActive")]
		bool RulerActive { [Bind ("isRulerActive")] get; set; }

		[Export ("drawingGestureRecognizer")]
		UIGestureRecognizer DrawingGestureRecognizer { get; }

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'DrawingPolicy' property instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'DrawingPolicy' property instead.")]
		[Export ("allowsFingerDrawing")]
		bool AllowsFingerDrawing { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("drawingPolicy", ArgumentSemantic.Assign)]
		PKCanvasViewDrawingPolicy DrawingPolicy { get; set; }

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("maximumSupportedContentVersion", ArgumentSemantic.Assign)]
		PKContentVersion MaximumSupportedContentVersion { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("drawingEnabled")]
		bool DrawingEnabled {
			[Bind ("isDrawingEnabled")]
			get;
			set;
		}
	}

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface PKDrawing : NSCopying, NSSecureCoding {

		[Field ("PKAppleDrawingTypeIdentifier")]
		NSString AppleDrawingTypeIdentifier { get; }

		[DesignatedInitializer]
		[Export ("initWithData:error:")]
		NativeHandle Constructor (NSData data, [NullAllowed] out NSError error);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithStrokes:")]
		NativeHandle Constructor (PKStroke [] strokes);

		[Export ("dataRepresentation")]
		NSData DataRepresentation { get; }

		[Export ("bounds")]
		CGRect Bounds { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("strokes")]
		PKStroke [] Strokes { get; }

		[Export ("imageFromRect:scale:")]
		UIImage GetImage (CGRect rect, nfloat scale);

		[Export ("drawingByApplyingTransform:")]
		PKDrawing GetDrawing (CGAffineTransform transform);

		[Export ("drawingByAppendingDrawing:")]
		PKDrawing GetDrawing (PKDrawing drawing);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("drawingByAppendingStrokes:")]
		PKDrawing GetDrawing (PKStroke [] strokes);

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("requiredContentVersion")]
		PKContentVersion RequiredContentVersion { get; }
	}

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (PKTool))]
	[DisableDefaultCtor]
	interface PKEraserTool {

		[Export ("eraserType")]
		PKEraserType EraserType { get; }

		[DesignatedInitializer]
		[Export ("initWithEraserType:")]
		NativeHandle Constructor (PKEraserType eraserType);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("initWithEraserType:width:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKEraserType eraserType, nfloat width);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("width")]
		nfloat Width { get; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Static]
		[Export ("defaultWidthForEraserType:")]
		nfloat GetDefaultWidth (PKEraserType eraserType);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Static]
		[Export ("minimumWidthForEraserType:")]
		nfloat GetMinimumWidth (PKEraserType eraserType);

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Static]
		[Export ("maximumWidthForEraserType:")]
		nfloat GetMaximumWidth (PKEraserType eraserType);
	}

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (PKTool))]
	[DisableDefaultCtor]
	interface PKInkingTool {

		[DesignatedInitializer]
		[Export ("initWithInkType:color:width:")]
		NativeHandle Constructor ([BindAs (typeof (PKInkType))] NSString type, UIColor color, nfloat width);

		[Export ("initWithInkType:color:")]
		NativeHandle Constructor ([BindAs (typeof (PKInkType))] NSString type, UIColor color);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("initWithInk:width:")]
		NativeHandle Constructor (PKInk ink, nfloat width);

		[Static]
		[Export ("defaultWidthForInkType:")]
		nfloat GetDefaultWidth ([BindAs (typeof (PKInkType))] NSString inkType);

		[Static]
		[Export ("minimumWidthForInkType:")]
		nfloat GetMinimumWidth ([BindAs (typeof (PKInkType))] NSString inkType);

		[Static]
		[Export ("maximumWidthForInkType:")]
		nfloat GetMaximumWidth ([BindAs (typeof (PKInkType))] NSString inkType);

		[Export ("inkType")]
		[BindAs (typeof (PKInkType))]
		NSString InkType { get; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("convertColor:fromUserInterfaceStyle:to:")]
		UIColor ConvertColor (UIColor color, UIUserInterfaceStyle fromUserInterfaceStyle, UIUserInterfaceStyle toUserInterfaceStyle);

		[Export ("color")]
		UIColor Color { get; }

		[Export ("width")]
		nfloat Width { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("ink")]
		PKInk Ink { get; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("requiredContentVersion")]
		PKContentVersion RequiredContentVersion { get; }
	}

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (PKTool))]
	[DesignatedDefaultCtor]
	interface PKLassoTool { }

	[iOS (13, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKTool : NSCopying { }

	interface IPKToolPickerObserver { }

	[iOS (13, 0), NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[Protocol]
	interface PKToolPickerObserver {

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'SelectedToolItemDidChange' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'SelectedToolItemDidChange' instead.")]
		[Export ("toolPickerSelectedToolDidChange:")]
		void SelectedToolDidChange (PKToolPicker toolPicker);

		[Export ("toolPickerIsRulerActiveDidChange:")]
		void IsRulerActiveDidChange (PKToolPicker toolPicker);

		[Export ("toolPickerVisibilityDidChange:")]
		void VisibilityDidChange (PKToolPicker toolPicker);

		[Export ("toolPickerFramesObscuredDidChange:")]
		void FramesObscuredDidChange (PKToolPicker toolPicker);

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("toolPickerSelectedToolItemDidChange:")]
		void SelectedToolItemDidChange (PKToolPicker toolPicker);
	}

	[iOS (13, 0), NoMac]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface PKToolPicker {

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("init")]
		NativeHandle Constructor ();

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("initWithToolItems:")]
		NativeHandle Constructor (PKToolPickerItem [] items);

		[Export ("addObserver:")]
		void AddObserver (IPKToolPickerObserver observer);

		[Export ("removeObserver:")]
		void RemoveObserver (IPKToolPickerObserver observer);

		[Export ("setVisible:forFirstResponder:")]
		void SetVisible (bool visible, UIResponder responder);

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'SelectedToolItem' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'SelectedToolItem' instead.")]
		[Export ("selectedTool", ArgumentSemantic.Strong)]
		PKTool SelectedTool { get; set; }

		[Export ("rulerActive")]
		bool RulerActive { [Bind ("isRulerActive")] get; set; }

		[Export ("isVisible")]
		bool IsVisible { get; }

		[Export ("frameObscuredInView:")]
		CGRect GetFrameObscured (UIView view);

		[Export ("overrideUserInterfaceStyle", ArgumentSemantic.Assign)]
		UIUserInterfaceStyle OverrideUserInterfaceStyle { get; set; }

		[Export ("colorUserInterfaceStyle", ArgumentSemantic.Assign)]
		UIUserInterfaceStyle ColorUserInterfaceStyle { get; set; }

		[Deprecated (PlatformName.iOS, 14, 0, message: "Create individual instances instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Create individual instances instead.")]
		[Static]
		[return: NullAllowed]
		[Export ("sharedToolPickerForWindow:")]
		PKToolPicker GetSharedToolPicker (UIWindow window);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("showsDrawingPolicyControls")]
		bool ShowsDrawingPolicyControls { get; set; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed]
		[Export ("stateAutosaveName")]
		string StateAutosaveName { get; set; }

		[iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("maximumSupportedContentVersion", ArgumentSemantic.Assign)]
		PKContentVersion MaximumSupportedContentVersion { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("selectedToolItem", ArgumentSemantic.Strong)]
		PKToolPickerItem SelectedToolItem { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("selectedToolItemIdentifier", ArgumentSemantic.Copy)]
		string SelectedToolItemIdentifier { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("toolItems")]
		PKToolPickerItem [] ToolItems { get; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("accessoryItem", ArgumentSemantic.Strong), NullAllowed]
		UIBarButtonItem AccessoryItem { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[iOS (18, 0), MacCatalyst (18, 0)]
		[Wrap ("WeakDelegate"), NullAllowed]
		IPKToolPickerDelegate Delegate { get; set; }

	}

	[iOS (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface PKInk : NSCopying {
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("initWithInkType:color:")]
		[DesignatedInitializer]
		NativeHandle Constructor (/* enum PKInkType */ NSString type, UIColor color);

		[Wrap ("this (type.GetConstant ()!, color)")]
		NativeHandle Constructor (PKInkType type, UIColor color);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("inkType")]
		NSString WeakInkType { get; }

		[Wrap ("PKInkTypeExtensions.GetValue(WeakInkType)")]
		PKInkType InkType { get; }

		[Export ("color")]
		UIColor Color { get; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("requiredContentVersion")]
		PKContentVersion RequiredContentVersion { get; }
	}

	[iOS (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface PKFloatRange : NSCopying {
		[Export ("initWithLowerBound:upperBound:")]
		NativeHandle Constructor (nfloat lowerBound, nfloat upperBound);

		[Export ("lowerBound")]
		nfloat LowerBound { get; }

		[Export ("upperBound")]
		nfloat UpperBound { get; }
	}

	[iOS (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface PKStroke : NSCopying {
		[Export ("initWithInk:strokePath:transform:mask:")]
		NativeHandle Constructor (PKInk ink, PKStrokePath path, CGAffineTransform transform, [NullAllowed] BezierPath mask);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("initWithInk:strokePath:transform:mask:randomSeed:")]
		NativeHandle Constructor (PKInk ink, PKStrokePath strokePath, CGAffineTransform transform, [NullAllowed] BezierPath mask, uint randomSeed);

		[Export ("ink")]
		PKInk Ink { get; }

		[Export ("transform")]
		CGAffineTransform Transform { get; }

		[Export ("path")]
		PKStrokePath Path { get; }

		[NullAllowed, Export ("mask")]
		BezierPath Mask { get; }

		[Export ("renderBounds")]
		CGRect RenderBounds { get; }

		[Export ("maskedPathRanges")]
		PKFloatRange [] MaskedPathRanges { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("randomSeed")]
		uint RandomSeed { get; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("requiredContentVersion")]
		PKContentVersion RequiredContentVersion { get; }
	}

	delegate void PKInterpolatedPointsEnumeratorHandler (PKStrokePoint strokePoint, out bool stop);

	[iOS (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface PKStrokePath : NSCopying {
		[Export ("initWithControlPoints:creationDate:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKStrokePoint [] controlPoints, NSDate creationDate);

		[Export ("count")]
		nuint Count { get; }

		[Export ("creationDate")]
		NSDate CreationDate { get; }

		[Export ("pointAtIndex:")]
		PKStrokePoint GetPoint (nuint index);

		[Export ("objectAtIndexedSubscript:")]
		PKStrokePoint GetObject (nuint indexedSubscript);

		[Export ("interpolatedLocationAt:")]
		CGPoint GetInterpolatedLocation (nfloat parametricValue);

		[Export ("interpolatedPointAt:")]
		PKStrokePoint GetInterpolatedPoint (nfloat parametricValue);

		[Export ("enumerateInterpolatedPointsInRange:strideByDistance:usingBlock:")]
		void EnumerateInterpolatedPointsByDistanceStep (PKFloatRange range, nfloat distanceStep, PKInterpolatedPointsEnumeratorHandler enumeratorHandler);

		[Export ("enumerateInterpolatedPointsInRange:strideByTime:usingBlock:")]
		void EnumerateInterpolatedPointsByTimeStep (PKFloatRange range, double timeStep, PKInterpolatedPointsEnumeratorHandler enumeratorHandler);

		[Export ("enumerateInterpolatedPointsInRange:strideByParametricStep:usingBlock:")]
		void EnumerateInterpolatedPointsByParametricStep (PKFloatRange range, nfloat parametricStep, PKInterpolatedPointsEnumeratorHandler enumeratorHandler);

		[Export ("parametricValue:offsetByDistance:")]
		nfloat GetParametricValue (nfloat parametricValue, nfloat distanceStep);

		[Export ("parametricValue:offsetByTime:")]
		nfloat GetParametricValue (nfloat parametricValue, double timeStep);
	}

	[iOS (14, 0)]
	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKStrokePoint : NSCopying {
		[Export ("initWithLocation:timeOffset:size:opacity:force:azimuth:altitude:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGPoint location, double timeOffset, CGSize size, nfloat opacity, nfloat force, nfloat azimuth, nfloat altitude);

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("initWithLocation:timeOffset:size:opacity:force:azimuth:altitude:secondaryScale:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGPoint location, double timeOffset, CGSize size, nfloat opacity, nfloat force, nfloat azimuth, nfloat altitude, nfloat secondaryScale);

		[Export ("location")]
		CGPoint Location { get; }

		[Export ("timeOffset")]
		double TimeOffset { get; }

		[Export ("size")]
		CGSize Size { get; }

		[Export ("opacity")]
		nfloat Opacity { get; }

		[Export ("azimuth")]
		nfloat Azimuth { get; }

		[Export ("force")]
		nfloat Force { get; }

		[Export ("altitude")]
		nfloat Altitude { get; }

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("secondaryScale")]
		nfloat SecondaryScale { get; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	[DisableDefaultCtor]
	interface PKToolPickerCustomItem {
		[Export ("initWithConfiguration:")]
		NativeHandle Constructor (PKToolPickerCustomItemConfiguration configuration);

		[Export ("configuration")]
		PKToolPickerCustomItemConfiguration Configuration { get; }

		[Export ("color", ArgumentSemantic.Strong)]
		UIColor Color { get; set; }

		[Export ("allowsColorSelection", ArgumentSemantic.Assign)]
		bool AllowsColorSelection { get; set; }

		[Export ("width", ArgumentSemantic.Assign)]
		nfloat Width { get; set; }

		[Export ("reloadImage")]
		void ReloadImage ();
	}

	delegate UIImage PKToolPickerCustomItemConfigurationImageProviderCallback (PKToolPickerCustomItem toolPickerItem);
	delegate UIViewController PKToolPickerCustomItemConfigurationViewControllerProvider (PKToolPickerCustomItem toolPickerItem);

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKToolPickerCustomItemConfiguration : NSCopying {
		[Export ("initWithIdentifier:name:")]
		NativeHandle Constructor (string identifier, string name);

		[Export ("identifier", ArgumentSemantic.Copy)]
		string Identifier { get; set; }

		[Export ("name", ArgumentSemantic.Copy)]
		string Name { get; set; }

		[Export ("imageProvider", ArgumentSemantic.Copy), NullAllowed]
		PKToolPickerCustomItemConfigurationImageProviderCallback ImageProvider { get; set; }

		[Export ("viewControllerProvider", ArgumentSemantic.Copy), NullAllowed]
		PKToolPickerCustomItemConfigurationViewControllerProvider ViewControllerProvider { get; set; }

		[Export ("defaultWidth", ArgumentSemantic.Assign)]
		nfloat DefaultWidth { get; set; }

		[Export ("widthVariants", ArgumentSemantic.Copy)]
		NSDictionary<NSNumber, UIImage> WidthVariants { get; set; }

		[Export ("defaultColor", ArgumentSemantic.Strong)]
		UIColor DefaultColor { get; set; }

		[Export ("allowsColorSelection", ArgumentSemantic.Assign)]
		bool AllowsColorSelection { get; set; }

		[Export ("toolAttributeControls", ArgumentSemantic.Assign)]
		PKToolPickerCustomItemControlOptions ToolAttributeControls { get; set; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	[DisableDefaultCtor]
	interface PKToolPickerEraserItem {
		[Export ("initWithEraserType:")]
		NativeHandle Constructor (PKEraserType eraserType);

		[Export ("initWithEraserType:width:")]
		NativeHandle Constructor (PKEraserType eraserType, nfloat width);

		[Export ("eraserTool")]
		PKEraserTool EraserTool { get; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	[DisableDefaultCtor]
	interface PKToolPickerInkingItem {
		[Export ("initWithInkType:")]
		NativeHandle Constructor (PKInkType inkType);

		[Export ("initWithInkType:color:")]
		NativeHandle Constructor (PKInkType inkType, UIColor color);

		[Export ("initWithInkType:width:")]
		NativeHandle Constructor (PKInkType inkType, nfloat width);

		[Export ("initWithInkType:color:width:")]
		NativeHandle Constructor (PKInkType inkType, UIColor color, nfloat width);

		[Export ("initWithInkType:color:width:identifier:")]
		NativeHandle Constructor (PKInkType inkType, UIColor color, nfloat width, [NullAllowed] string identifier);

		[Export ("inkingTool")]
		PKInkingTool InkingTool { get; }

		[Export ("allowsColorSelection", ArgumentSemantic.Assign)]
		bool AllowsColorSelection { get; set; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKToolPickerItem : NSCopying {
		[Export ("identifier")]
		string Identifier { get; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	interface PKToolPickerLassoItem {
		[Export ("lassoTool")]
		PKLassoTool LassoTool { get; }
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	interface PKToolPickerRulerItem {
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (PKToolPickerItem))]
	interface PKToolPickerScribbleItem {
	}

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface PKToolPickerDelegate {
	}

	interface IPKToolPickerDelegate { }

	[iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[Native]
	[Flags]
	enum PKToolPickerCustomItemControlOptions : ulong {
		None = 0,
		Width = 1 << 0,
		Opacity = 1 << 1,
	}
}
