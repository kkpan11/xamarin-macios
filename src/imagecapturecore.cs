//
// Copyright 2018 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

//
// imagecapturecore.cs: Bindings for the ImageCaptureCore API
//
using System;
using AppKit;
using Foundation;
using ObjCRuntime;
using CoreImage;
using CoreGraphics;
using CoreAnimation;

namespace ImageCaptureCore {

	interface IICDeviceDelegate { }

	[Static]
	interface ICDeviceCapabilities {
		[Field ("ICDeviceCanEjectOrDisconnect")]
		NSString DeviceCanEjectOrDisconnect { get; }

		[Field ("ICCameraDeviceCanTakePicture")]
		NSString CameraDeviceCanTakePicture { get; }

		[Field ("ICCameraDeviceCanTakePictureUsingShutterReleaseOnCamera")]
		NSString CameraDeviceCanTakePictureUsingShutterReleaseOnCamera { get; }

		[Field ("ICCameraDeviceCanDeleteOneFile")]
		NSString CameraDeviceCanDeleteOneFile { get; }

		[Field ("ICCameraDeviceCanDeleteAllFiles")]
		NSString CameraDeviceCanDeleteAllFiles { get; }

		[Field ("ICCameraDeviceCanSyncClock")]
		NSString CameraDeviceCanSyncClock { get; }

		[Field ("ICCameraDeviceCanReceiveFile")]
		NSString CameraDeviceCanReceiveFile { get; }

		[Field ("ICCameraDeviceCanAcceptPTPCommands")]
		NSString CameraDeviceCanAcceptPtpCommands { get; }
	}

	[Static]
	interface ICScannerStatus {
		[Field ("ICScannerStatusWarmingUp")]
		NSString WarmingUp { get; }

		[Field ("ICScannerStatusWarmUpDone")]
		NSString WarmUpDone { get; }

		[Field ("ICScannerStatusRequestsOverviewScan")]
		NSString RequestsOverviewScan { get; }
	}

	[Static]
	interface ICDeviceLocationDescriptions {
		[Field ("ICDeviceLocationDescriptionUSB")]
		NSString Usb { get; }

		[Field ("ICDeviceLocationDescriptionFireWire")]
		NSString FireWire { get; }

		[Field ("ICDeviceLocationDescriptionBluetooth")]
		NSString Bluetooth { get; }

		[Field ("ICDeviceLocationDescriptionMassStorage")]
		NSString MassStorage { get; }
	}

	[Static]
	interface ICCameraDownloadOptionKeys {
		[Field ("ICDownloadsDirectoryURL")]
		NSString DownloadsDirectoryUrl { get; }

		[Field ("ICSaveAsFilename")]
		NSString SaveAsFilename { get; }

		[Field ("ICSavedFilename")]
		NSString SavedFilename { get; }

		[Field ("ICSavedAncillaryFiles")]
		NSString SavedAncillaryFiles { get; }

		[Field ("ICOverwrite")]
		NSString Overwrite { get; }

		[Field ("ICDeleteAfterSuccessfulDownload")]
		NSString DeleteAfterSuccessfulDownload { get; }

		[Field ("ICDownloadSidecarFiles")]
		NSString DownloadSidecarFiles { get; }
	}

	[Static]
	interface ICStatusNotificationKeys { // Keys into DidReceiveStatusInformation dictionary
		[Field ("ICStatusNotificationKey")]
		NSString NotificationKey { get; }

		[Field ("ICStatusCodeKey")]
		NSString CodeKey { get; }

		[Field ("ICLocalizedStatusNotificationKey")]
		NSString LocalizedNotificationKey { get; }
	}

	// Can't be an smart enum since it is used inside 'ICDeviceDelegate' protocol.
	[Static]
	interface ICButtonType {
		[Field ("ICButtonTypeScan")]
		NSString Scan { get; }

		[Field ("ICButtonTypeMail")]
		NSString Mail { get; }

		[Field ("ICButtonTypeCopy")]
		NSString Copy { get; }

		[Field ("ICButtonTypeWeb")]
		NSString Web { get; }

		[Field ("ICButtonTypePrint")]
		NSString Print { get; }

		[Field ("ICButtonTypeTransfer")]
		NSString Transfer { get; }
	}

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ICDeviceDelegate {

		[Abstract]
		[Export ("didRemoveDevice:")]
		void DidRemoveDevice (ICDevice device);

		[Export ("device:didOpenSessionWithError:")]
		void DidOpenSession (ICDevice device, [NullAllowed] NSError error);

		[Export ("deviceDidBecomeReady:")]
		void DidBecomeReady (ICDevice device);

		[Export ("device:didCloseSessionWithError:")]
		void DidCloseSession (ICDevice device, [NullAllowed] NSError error);

		[Export ("deviceDidChangeName:")]
		void DidChangeName (ICDevice device);

		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Export ("deviceDidChangeSharingState:")]
		void DidChangeSharingState (ICDevice device);

		[Export ("device:didReceiveStatusInformation:")]
		void DidReceiveStatusInformation (ICDevice device, NSDictionary<NSString, NSObject> status);

		[Export ("device:didEncounterError:")]
		void DidEncounterError (ICDevice device, [NullAllowed] NSError error);

		[Export ("device:didReceiveButtonPress:")]
		void DidReceiveButtonPress (ICDevice device, NSString /* ICButtonType */ buttonType);

		[Export ("device:didReceiveCustomNotification:data:")]
		void DidReceiveCustomNotification (ICDevice device, NSDictionary<NSString, NSObject> notification, NSData data);
	}

	[BaseType (typeof (NSObject))]
	interface ICDevice {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IICDeviceDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Export ("type")]
		ICDeviceType Type { get; }

		[NullAllowed, Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("icon")]
		CGImage Icon { get; }

		[Export ("capabilities")]
		NSString [] Capabilities { get; }

		[Export ("modulePath")]
		string ModulePath { get; }

		[NullAllowed, Export ("moduleVersion")]
		string ModuleVersion { get; }

		[Export ("moduleExecutableArchitecture")]
		int ModuleExecutableArchitecture { get; }

		[Export ("remote")]
		bool Remote { [Bind ("isRemote")] get; }

		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Export ("shared")]
		bool Shared { [Bind ("isShared")] get; }

		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Export ("hasConfigurableWiFiInterface")]
		bool HasConfigurableWiFiInterface { get; }

		[BindAs (typeof (ICTransportType))]
		[Export ("transportType")]
		NSString TransportType { get; }

		[Export ("usbLocationID")]
		int UsbLocationId { get; }

		[Export ("usbProductID")]
		int UsbProductId { get; }

		[Export ("usbVendorID")]
		int UsbVendorId { get; }

		[Export ("fwGUID")]
		long FireWireGuid { get; }

		[NullAllowed, Export ("serialNumberString")]
		string SerialNumber { get; }

		[NullAllowed, Export ("locationDescription")]
		string LocationDescription { get; }

		[Export ("hasOpenSession")]
		bool HasOpenSession { get; }

		[NullAllowed, Export ("UUIDString")]
		string Uuid { get; }

		[NullAllowed, Export ("persistentIDString")]
		string PersistentId { get; }

		[Export ("buttonPressed")]
		string ButtonPressed { get; }

		[NullAllowed, Export ("autolaunchApplicationPath")]
		string AutolaunchApplicationPath { get; set; }

		[NullAllowed, Export ("userData")]
		NSMutableDictionary UserData { get; }

		[Export ("requestOpenSession")]
		void RequestOpenSession ();

		[Export ("requestCloseSession")]
		void RequestCloseSession ();

		[Export ("requestYield")]
		void RequestYield ();

		[Export ("requestSendMessage:outData:maxReturnedDataSize:sendMessageDelegate:didSendMessageSelector:contextInfo:")]
		void RequestSendMessage (uint messageCode, NSData data, uint maxReturnedDataSize, NSObject sendMessageDelegate, Selector selector, [NullAllowed] IntPtr contextInfo);

		[Export ("requestEjectOrDisconnect")]
		void RequestEjectOrDisconnect ();
	}

	interface IICDeviceBrowserDelegate { }

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ICDeviceBrowserDelegate {

		[Abstract]
		[Export ("deviceBrowser:didAddDevice:moreComing:")]
		void DidAddDevice (ICDeviceBrowser browser, ICDevice device, bool moreComing);

		[Abstract]
		[Export ("deviceBrowser:didRemoveDevice:moreGoing:")]
		void DidRemoveDevice (ICDeviceBrowser browser, ICDevice device, bool moreGoing);

		[Export ("deviceBrowser:deviceDidChangeName:")]
		void DeviceDidChangeName (ICDeviceBrowser browser, ICDevice device);

		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[Export ("deviceBrowser:deviceDidChangeSharingState:")]
		void DeviceDidChangeSharingState (ICDeviceBrowser browser, ICDevice device);

		[Export ("deviceBrowser:requestsSelectDevice:")]
		void RequestsSelectDevice (ICDeviceBrowser browser, ICDevice device);

		[Export ("deviceBrowserDidEnumerateLocalDevices:")]
		void DidEnumerateLocalDevices (ICDeviceBrowser browser);
	}

	[BaseType (typeof (NSObject))]
	interface ICDeviceBrowser {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IICDeviceBrowserDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Export ("browsing")]
		bool Browsing { [Bind ("isBrowsing")] get; }

		[Export ("browsedDeviceTypeMask", ArgumentSemantic.Assign)]
		ICBrowsedDeviceType BrowsedDeviceTypeMask { get; set; }

		[NullAllowed, Export ("devices")]
		ICDevice [] Devices { get; }

		[NullAllowed, Export ("preferredDevice")]
		ICDevice PreferredDevice { get; }

		[Export ("start")]
		void Start ();

		[Export ("stop")]
		void Stop ();
	}

	[DisableDefaultCtor] // Created by 'ICCameraDevice'.
	[Abstract]
	[BaseType (typeof (NSObject))]
	interface ICCameraItem {

		[NullAllowed, Export ("device")]
		ICCameraDevice Device { get; }

		[NullAllowed, Export ("parentFolder")]
		ICCameraFolder ParentFolder { get; }

		[NullAllowed, Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("UTI")]
		string Uti { get; }

		[NullAllowed, Export ("fileSystemPath")]
		string FileSystemPath { get; }

		[Export ("locked")]
		bool Locked { [Bind ("isLocked")] get; }

		[Export ("raw")]
		bool Raw { [Bind ("isRaw")] get; }

		[Export ("inTemporaryStore")]
		bool InTemporaryStore { [Bind ("isInTemporaryStore")] get; }

		[NullAllowed, Export ("creationDate")]
		NSDate CreationDate { get; }

		[NullAllowed, Export ("modificationDate")]
		NSDate ModificationDate { get; }

		[NullAllowed, Export ("thumbnailIfAvailable")]
		CGImage ThumbnailIfAvailable { get; }

		[NullAllowed, Export ("largeThumbnailIfAvailable")]
		CGImage LargeThumbnailIfAvailable { get; }

		[NullAllowed, Export ("metadataIfAvailable")]
		NSDictionary<NSString, NSObject> MetadataIfAvailable { get; }

		[NullAllowed, Export ("userData")]
		NSMutableDictionary UserData { get; }

		[Export ("ptpObjectHandle")]
		uint PtpObjectHandle { get; }

		[Export ("addedAfterContentCatalogCompleted")]
		bool AddedAfterContentCatalogCompleted { [Bind ("wasAddedAfterContentCatalogCompleted")] get; }
	}

	[DisableDefaultCtor] // Created by 'ICCameraDevice'.
	[BaseType (typeof (ICCameraItem))]
	interface ICCameraFolder {

		[NullAllowed, Export ("contents")]
		ICCameraItem [] Contents { get; }
	}

	[DisableDefaultCtor] // Created by 'ICCameraDevice'.
	[BaseType (typeof (ICCameraItem))]
	interface ICCameraFile {

		[Export ("fileSize")]
		long FileSize { get; }

		[Export ("orientation", ArgumentSemantic.Assign)]
		ICExifOrientationType Orientation { get; set; }

		[Export ("duration")]
		double Duration { get; }

		[NullAllowed, Export ("sidecarFiles")]
		ICCameraItem [] SidecarFiles { get; }
	}

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ICCameraDeviceDelegate : ICDeviceDelegate {

		[Abstract]
		[Export ("cameraDevice:didAddItem:")]
		void DidAddItem (ICCameraDevice camera, ICCameraItem item);

		[Abstract]
		[Export ("cameraDevice:didRemoveItem:")]
		void DidRemoveItem (ICCameraDevice camera, ICCameraItem item);

		[Abstract]
		[Export ("cameraDevice:didRenameItems:")]
		void DidRenameItems (ICCameraDevice camera, ICCameraItem [] items);

		[Abstract]
		[Export ("cameraDevice:didCompleteDeleteFilesWithError:")]
		void DidCompleteDeleteFiles (ICCameraDevice scanner, [NullAllowed] NSError error);

		[Abstract]
		[Export ("cameraDeviceDidChangeCapability:")]
		void DidChangeCapability (ICCameraDevice camera);

#if !XAMCORE_5_0
		[Abstract]
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'DidReceiveThumbnailForItem (ICCameraDevice, CGImageRef, ICCameraItem, NSError)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidReceiveThumbnailForItem (ICCameraDevice, CGImageRef, ICCameraItem, NSError)' instead.")]
		[Export ("cameraDevice:didReceiveThumbnailForItem:")]
		void DidReceiveThumbnail (ICCameraDevice camera, ICCameraItem forItem);
#endif

		[Export ("cameraDevice:didReceiveThumbnail:forItem:error:")]
#if XAMCORE_5_0
		[Abstract]
		void DidReceiveThumbnail (ICCameraDevice camera, /* CGImageRef */ IntPtr thumbnail, ICCameraItem forItem, [NullAllowed] NSError error);
#else
		void DidReceiveThumbnailForItem (ICCameraDevice camera, /* CGImageRef */ IntPtr thumbnail, ICCameraItem forItem, [NullAllowed] NSError error);
#endif

		[Abstract]
		[Export ("cameraDevice:didReceiveMetadataForItem:")]
		void DidReceiveMetadata (ICCameraDevice camera, ICCameraItem forItem);

		[Abstract]
		[Export ("cameraDevice:didReceivePTPEvent:")]
		void DidReceivePtpEvent (ICCameraDevice camera, NSData eventData);

		[Abstract]
		[Export ("deviceDidBecomeReadyWithCompleteContentCatalog:")]
		void DidBecomeReadyWithCompleteContentCatalog (ICDevice device);

		[Export ("cameraDevice:didAddItems:")]
		void DidAddItems (ICCameraDevice camera, ICCameraItem [] items);

		[Export ("cameraDevice:didRemoveItems:")]
		void DidRemoveItems (ICCameraDevice camera, ICCameraItem [] items);

		[Export ("cameraDevice:shouldGetThumbnailOfItem:")]
		bool ShouldGetThumbnail (ICCameraDevice cameraDevice, ICCameraItem ofItem);

		[Export ("cameraDevice:shouldGetMetadataOfItem:")]
		bool ShouldGetMetadata (ICCameraDevice cameraDevice, ICCameraItem ofItem);
	}

	interface IICCameraDeviceDownloadDelegate { }

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ICCameraDeviceDownloadDelegate {

		[Export ("didDownloadFile:error:options:contextInfo:")]
		void DidDownloadFile (ICCameraFile file, [NullAllowed] NSError error, NSDictionary<NSString, NSObject> options, [NullAllowed] IntPtr contextInfo);

		[Export ("didReceiveDownloadProgressForFile:downloadedBytes:maxBytes:")]
		void DidReceiveDownloadProgress (ICCameraFile file, long downloadedBytes, long maxBytes);
	}

	[BaseType (typeof (ICDevice))]
	[DisableDefaultCtor] // ICDeviceBrowser creates instances of this class
	interface ICCameraDevice {

		[Export ("batteryLevelAvailable")]
		bool BatteryLevelAvailable { get; }

		[Export ("batteryLevel")]
		nuint BatteryLevel { get; }

		[Export ("contentCatalogPercentCompleted")]
		nuint ContentCatalogPercentCompleted { get; }

		[NullAllowed, Export ("contents")]
		ICCameraItem [] Contents { get; }

		[NullAllowed, Export ("mediaFiles")]
		ICCameraItem [] MediaFiles { get; }

		[Export ("timeOffset")]
		double TimeOffset { get; }

		[Export ("isAccessRestrictedAppleDevice")]
		bool IsAccessRestrictedAppleDevice { get; }

		[NullAllowed, Export ("mountPoint")]
		string MountPoint { get; }

		[Export ("tetheredCaptureEnabled")]
		bool TetheredCaptureEnabled { get; }

		[Export ("filesOfType:")]
		[return: NullAllowed]
		string [] GetFiles (string fileUTType);

		[Export ("requestSyncClock")]
		void RequestSyncClock ();

		[Export ("requestEnableTethering")]
		void RequestEnableTethering ();

		[Export ("requestDisableTethering")]
		void RequestDisableTethering ();

		[Export ("requestTakePicture")]
		void RequestTakePicture ();

		[Export ("requestDeleteFiles:")]
		void RequestDeleteFiles (ICCameraItem [] files);

		[Export ("cancelDelete")]
		void CancelDelete ();

		[Export ("requestDownloadFile:options:downloadDelegate:didDownloadSelector:contextInfo:")]
		void RequestDownloadFile (ICCameraFile file, NSDictionary<NSString, NSObject> options, IICCameraDeviceDownloadDelegate downloadDelegate, Selector didDownloadSelector, [NullAllowed] IntPtr contextInfo);

		[Export ("cancelDownload")]
		void CancelDownload ();

		[Export ("requestUploadFile:options:uploadDelegate:didUploadSelector:contextInfo:")]
		void RequestUploadFile (NSUrl fileUrl, NSDictionary<NSString, NSObject> options, NSObject uploadDelegate, Selector didUploadSelector, [NullAllowed] IntPtr contextInfo);

		[Export ("requestReadDataFromFile:atOffset:length:readDelegate:didReadDataSelector:contextInfo:")]
		void RequestReadDataFromFile (ICCameraFile file, long offset, long length, NSObject readDelegate, Selector didReadDataSelector, [NullAllowed] IntPtr contextInfo);

		[Export ("requestSendPTPCommand:outData:sendCommandDelegate:didSendCommandSelector:contextInfo:")]
		void RequestSendPtpCommand (NSData command, NSData outData, NSObject sendCommandDelegate, Selector didSendCommandSelector, [NullAllowed] IntPtr contextInfo);
	}

	[DisableDefaultCtor]
	[Abstract]
	[BaseType (typeof (NSObject))]
	interface ICScannerFeature {

		[Export ("type")]
		ICScannerFeatureType Type { get; }

		[NullAllowed, Export ("internalName")]
		string InternalName { get; }

		[NullAllowed, Export ("humanReadableName")]
		string HumanReadableName { get; }

		[NullAllowed, Export ("tooltip")]
		string Tooltip { get; }
	}

	[DisableDefaultCtor]
	[BaseType (typeof (ICScannerFeature))]
	interface ICScannerFeatureEnumeration {

		[Export ("currentValue", ArgumentSemantic.Assign)]
		NSObject CurrentValue { get; set; }

		[Export ("defaultValue")]
		NSObject DefaultValue { get; }

		[Export ("values")]
		NSNumber [] Values { get; }

		[Export ("menuItemLabels")]
		string [] MenuItemLabels { get; }

		[Export ("menuItemLabelsTooltips")]
		string [] MenuItemLabelsTooltips { get; }
	}

	[DisableDefaultCtor]
	[BaseType (typeof (ICScannerFeature))]
	interface ICScannerFeatureRange {

		[Export ("currentValue")]
		nfloat CurrentValue { get; set; }

		[Export ("defaultValue")]
		nfloat DefaultValue { get; }

		[Export ("minValue")]
		nfloat MinValue { get; }

		[Export ("maxValue")]
		nfloat MaxValue { get; }

		[Export ("stepSize")]
		nfloat StepSize { get; }
	}

	[DisableDefaultCtor]
	[BaseType (typeof (ICScannerFeature))]
	interface ICScannerFeatureBoolean {

		[Export ("value")]
		bool Value { get; set; }
	}

	[DisableDefaultCtor]
	[BaseType (typeof (ICScannerFeature))]
	interface ICScannerFeatureTemplate {

		[Export ("targets")]
		NSMutableArray [] Targets { get; }
	}

	[DisableDefaultCtor]
	[Abstract]
	[BaseType (typeof (NSObject))]
	interface ICScannerFunctionalUnit {

		[Export ("type")]
		ICScannerFunctionalUnitType Type { get; }

		[Export ("pixelDataType", ArgumentSemantic.Assign)]
		ICScannerPixelDataType PixelDataType { get; set; }

		[Export ("supportedBitDepths")]
		NSIndexSet SupportedBitDepths { get; }

		[Export ("bitDepth", ArgumentSemantic.Assign)]
		ICScannerBitDepth BitDepth { get; set; }

		[Export ("supportedMeasurementUnits")]
		NSIndexSet SupportedMeasurementUnits { get; }

		[Export ("measurementUnit", ArgumentSemantic.Assign)]
		ICScannerMeasurementUnit MeasurementUnit { get; set; }

		[Export ("supportedResolutions")]
		NSIndexSet SupportedResolutions { get; }

		[Export ("preferredResolutions")]
		NSIndexSet PreferredResolutions { get; }

		[Export ("resolution")]
		nuint Resolution { get; set; }

		[Export ("nativeXResolution")]
		nuint NativeXResolution { get; }

		[Export ("nativeYResolution")]
		nuint NativeYResolution { get; }

		[Export ("supportedScaleFactors")]
		NSIndexSet SupportedScaleFactors { get; }

		[Export ("preferredScaleFactors")]
		NSIndexSet PreferredScaleFactors { get; }

		[Export ("scaleFactor")]
		nuint ScaleFactor { get; set; }

		[Export ("templates")]
		ICScannerFeatureTemplate [] Templates { get; }

		[NullAllowed, Export ("vendorFeatures")]
		ICScannerFeature [] VendorFeatures { get; }

		[Export ("physicalSize")]
		CGSize PhysicalSize { get; }

		[Export ("scanArea", ArgumentSemantic.Assign)]
		CGRect ScanArea { get; set; }

		[Export ("scanAreaOrientation", ArgumentSemantic.Assign)]
		ICExifOrientationType ScanAreaOrientation { get; set; }

		[Export ("acceptsThresholdForBlackAndWhiteScanning")]
		bool AcceptsThresholdForBlackAndWhiteScanning { get; }

		[Export ("usesThresholdForBlackAndWhiteScanning")]
		bool UsesThresholdForBlackAndWhiteScanning { get; set; }

		[Export ("defaultThresholdForBlackAndWhiteScanning")]
		byte DefaultThresholdForBlackAndWhiteScanning { get; }

		[Export ("thresholdForBlackAndWhiteScanning")]
		byte ThresholdForBlackAndWhiteScanning { get; set; }

		[Export ("state")]
		ICScannerFunctionalUnitState State { get; }

		[Export ("scanInProgress")]
		bool ScanInProgress { get; }

		[Export ("scanProgressPercentDone")]
		nfloat ScanProgressPercentDone { get; }

		[Export ("canPerformOverviewScan")]
		bool CanPerformOverviewScan { get; }

		[Export ("overviewScanInProgress")]
		bool OverviewScanInProgress { get; }

		[NullAllowed, Export ("overviewImage")]
		CGImage OverviewImage { get; }

		[Export ("overviewResolution")]
		nuint OverviewResolution { get; set; }
	}

	[DisableDefaultCtor] // Created by 'ICScannerDevice'.
	[BaseType (typeof (ICScannerFunctionalUnit))]
	interface ICScannerFunctionalUnitFlatbed {

		[Export ("supportedDocumentTypes")]
		NSIndexSet SupportedDocumentTypes { get; }

		[Export ("documentType", ArgumentSemantic.Assign)]
		ICScannerDocumentType DocumentType { get; set; }

		[Export ("documentSize")]
		CGSize DocumentSize { get; }
	}

	[DisableDefaultCtor] // Created by 'ICScannerDevice'.
	[BaseType (typeof (ICScannerFunctionalUnit))]
	interface ICScannerFunctionalUnitPositiveTransparency {

		[Export ("supportedDocumentTypes")]
		NSIndexSet SupportedDocumentTypes { get; }

		[Export ("documentType", ArgumentSemantic.Assign)]
		ICScannerDocumentType DocumentType { get; set; }

		[Export ("documentSize")]
		CGSize DocumentSize { get; }
	}

	[DisableDefaultCtor] // Created by 'ICScannerDevice'.
	[BaseType (typeof (ICScannerFunctionalUnit))]
	interface ICScannerFunctionalUnitNegativeTransparency {

		[Export ("supportedDocumentTypes")]
		NSIndexSet SupportedDocumentTypes { get; }

		[Export ("documentType", ArgumentSemantic.Assign)]
		ICScannerDocumentType DocumentType { get; set; }

		[Export ("documentSize")]
		CGSize DocumentSize { get; }
	}

	[DisableDefaultCtor] // Created by 'ICScannerDevice'.
	[BaseType (typeof (ICScannerFunctionalUnit))]
	interface ICScannerFunctionalUnitDocumentFeeder {

		[Export ("supportedDocumentTypes")]
		NSIndexSet SupportedDocumentTypes { get; }

		[Export ("documentType", ArgumentSemantic.Assign)]
		ICScannerDocumentType DocumentType { get; set; }

		[Export ("documentSize")]
		CGSize DocumentSize { get; }

		[Export ("supportsDuplexScanning")]
		bool SupportsDuplexScanning { get; }

		[Export ("duplexScanningEnabled")]
		bool DuplexScanningEnabled { get; set; }

		[Export ("documentLoaded")]
		bool DocumentLoaded { get; }

		[Export ("oddPageOrientation", ArgumentSemantic.Assign)]
		ICExifOrientationType OddPageOrientation { get; set; }

		[Export ("evenPageOrientation", ArgumentSemantic.Assign)]
		ICExifOrientationType EvenPageOrientation { get; set; }

		[Export ("reverseFeederPageOrder")]
		bool ReverseFeederPageOrder { get; }
	}

	[BaseType (typeof (NSObject))]
	interface ICScannerBandData {

		[Export ("fullImageWidth")]
		nuint FullImageWidth { get; }

		[Export ("fullImageHeight")]
		nuint FullImageHeight { get; }

		[Export ("bitsPerPixel")]
		nuint BitsPerPixel { get; }

		[Export ("bitsPerComponent")]
		nuint BitsPerComponent { get; }

		[Export ("numComponents")]
		nuint NumComponents { get; }

		[Export ("bigEndian")]
		bool BigEndian { [Bind ("isBigEndian")] get; }

		[Export ("pixelDataType")]
		ICScannerPixelDataType PixelDataType { get; }

		[NullAllowed, Export ("colorSyncProfilePath", ArgumentSemantic.Retain)]
		string ColorSyncProfilePath { get; }

		[Export ("bytesPerRow")]
		nuint BytesPerRow { get; }

		[Export ("dataStartRow")]
		nuint DataStartRow { get; }

		[Export ("dataNumRows")]
		nuint DataNumRows { get; }

		[Export ("dataSize")]
		nuint DataSize { get; }

		[NullAllowed, Export ("dataBuffer", ArgumentSemantic.Retain)]
		NSData DataBuffer { get; }
	}

	interface IICScannerDeviceDelegate { }

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface ICScannerDeviceDelegate : ICDeviceDelegate {

		[Export ("scannerDeviceDidBecomeAvailable:")]
		void DidBecomeAvailable (ICScannerDevice scanner);

		[Export ("scannerDevice:didSelectFunctionalUnit:error:")]
		void DidSelectFunctionalUnit (ICScannerDevice scanner, ICScannerFunctionalUnit functionalUnit, [NullAllowed] NSError error);

		[Deprecated (PlatformName.MacOSX, 10, 7)]
		[Export ("scannerDevice:didScanToURL:data:")]
		void DidScanToUrl (ICScannerDevice scanner, NSUrl url, NSData data);

		[Export ("scannerDevice:didScanToURL:")]
		void DidScanToUrl (ICScannerDevice scanner, NSUrl url);

		[Export ("scannerDevice:didScanToBandData:")]
		void DidScanToBandData (ICScannerDevice scanner, ICScannerBandData data);

		[Export ("scannerDevice:didCompleteOverviewScanWithError:")]
		void DidCompleteOverviewScan (ICScannerDevice scanner, [NullAllowed] NSError error);

		[Export ("scannerDevice:didCompleteScanWithError:")]
		void DidCompleteScan (ICScannerDevice scanner, [NullAllowed] NSError error);
	}

	[BaseType (typeof (ICDevice))]
	interface ICScannerDevice {

		[Export ("availableFunctionalUnitTypes")]
		NSNumber [] AvailableFunctionalUnitTypes { get; }

		[Export ("selectedFunctionalUnit")]
		ICScannerFunctionalUnit SelectedFunctionalUnit { get; }

		[Export ("transferMode", ArgumentSemantic.Assign)]
		ICScannerTransferMode TransferMode { get; set; }

		[Export ("maxMemoryBandSize")]
		uint MaxMemoryBandSize { get; set; }

		[Export ("downloadsDirectory", ArgumentSemantic.Retain)]
		NSUrl DownloadsDirectory { get; set; }

		[Export ("documentName")]
		string DocumentName { get; set; }

		[Export ("documentUTI")]
		string DocumentUti { get; set; }

		[Export ("defaultUsername")]
		string DefaultUsername { get; set; }

		[Export ("requestOpenSessionWithCredentials:password:")]
		void RequestOpenSession (string username, string password);

		[Export ("requestSelectFunctionalUnit:")]
		void RequestSelectFunctionalUnit (ICScannerFunctionalUnitType type);

		[Export ("requestOverviewScan")]
		void RequestOverviewScan ();

		[Export ("requestScan")]
		void RequestScan ();

		[Export ("cancelScan")]
		void CancelScan ();
	}
}
