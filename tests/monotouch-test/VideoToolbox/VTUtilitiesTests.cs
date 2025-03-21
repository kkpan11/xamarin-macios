//
// Unit tests for VTMultiPassStorage
//
// Authors:
//	Alex Soto <alex.soto@xamarin.com>
//	
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Foundation;
using VideoToolbox;
using CoreMedia;
#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using AVFoundation;
using CoreFoundation;
using CoreVideo;
using CoreGraphics;
using ObjCRuntime;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.VideoToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class VTUtilitiesTests {

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static int CFGetRetainCount (IntPtr handle);

		[Test]
		public void ToCGImageTest ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 9, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 11, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 10, 2, throwIfOtherPlatform: false);

#if MONOMAC
			var originalImage = new NSImage (NSBundle.MainBundle.PathForResource ("Xam", "png", "CoreImage"));
#else
			var originalImage = UIImage.FromBundle ("CoreImage/Xam.png");
#endif
			var originalCGImage = originalImage.CGImage;

			var pxbuffer = new CVPixelBuffer (originalCGImage.Width, originalCGImage.Height, CVPixelFormatType.CV32ARGB,
							   new CVPixelBufferAttributes { CGImageCompatibility = true, CGBitmapContextCompatibility = true });
			pxbuffer.Lock (CVPixelBufferLock.None);
			using (var colorSpace = CGColorSpace.CreateDeviceRGB ())
			using (var ctx = new CGBitmapContext (pxbuffer.BaseAddress, originalCGImage.Width, originalCGImage.Height, 8,
								 4 * originalCGImage.Width, colorSpace, CGBitmapFlags.NoneSkipLast)) {
				ctx.RotateCTM (0);
				ctx.DrawImage (new CGRect (0, 0, originalCGImage.Width, originalCGImage.Height), originalCGImage);
				pxbuffer.Unlock (CVPixelBufferLock.None);
			}

			Assert.NotNull (pxbuffer, "VTUtilitiesTests.ToCGImageTest pxbuffer should not be null");

			CGImage newImage;
			var newImageStatus = pxbuffer.ToCGImage (out newImage);

			Assert.That (newImageStatus == VTStatus.Ok, "VTUtilitiesTests.ToCGImageTest must be ok");
			Assert.NotNull (newImage, "VTUtilitiesTests.ToCGImageTest pxbuffer should not be newImage");
			Assert.AreEqual (originalCGImage.Width, newImage.Width, "VTUtilitiesTests.ToCGImageTest");
			Assert.AreEqual (originalCGImage.Height, newImage.Height, "VTUtilitiesTests.ToCGImageTest");

			var retainCount = CFGetRetainCount (newImage.Handle);
			Assert.That (retainCount, Is.EqualTo (1), "RetainCount");
		}

#if MONOMAC

		[TestCase (CMVideoCodecType.YUV422YpCbCr8)]
		[TestCase (CMVideoCodecType.Animation)]
		[TestCase (CMVideoCodecType.Cinepak)]
		[TestCase (CMVideoCodecType.JPEG)]
		[TestCase (CMVideoCodecType.JPEG_OpenDML)]
		[TestCase (CMVideoCodecType.SorensonVideo)]
		[TestCase (CMVideoCodecType.SorensonVideo3)]
		[TestCase (CMVideoCodecType.H263)]
		[TestCase (CMVideoCodecType.H264)]
		[TestCase (CMVideoCodecType.Mpeg4Video)]
		[TestCase (CMVideoCodecType.Mpeg2Video)]
		[TestCase (CMVideoCodecType.Mpeg1Video)]
		[TestCase (CMVideoCodecType.VP9)]
		[TestCase (CMVideoCodecType.DvcNtsc)]
		[TestCase (CMVideoCodecType.DvcPal)]
		[TestCase (CMVideoCodecType.DvcProPal)]
		[TestCase (CMVideoCodecType.DvcPro50NTSC)]
		[TestCase (CMVideoCodecType.DvcPro50PAL)]
		[TestCase (CMVideoCodecType.DvcProHD720p60)]
		[TestCase (CMVideoCodecType.DvcProHD720p50)]
		[TestCase (CMVideoCodecType.DvcProHD1080i60)]
		[TestCase (CMVideoCodecType.DvcProHD1080i50)]
		[TestCase (CMVideoCodecType.DvcProHD1080p30)]
		[TestCase (CMVideoCodecType.DvcProHD1080p25)]
		[TestCase (CMVideoCodecType.AppleProRes4444)]
		[TestCase (CMVideoCodecType.AppleProRes422HQ)]
		[TestCase (CMVideoCodecType.AppleProRes422)]
		[TestCase (CMVideoCodecType.AppleProRes422LT)]
		[TestCase (CMVideoCodecType.AppleProRes422Proxy)]
		[TestCase (CMVideoCodecType.Hevc)]
		public void TestRegisterSupplementalVideoDecoder (CMVideoCodecType codec)
		{
			TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);
			// ensure that the call does not crash, we do not have anyother thing to test since there is 
			// no way to know if it was a success
			VTUtilities.RegisterSupplementalVideoDecoder (codec);
		}

		static CMVideoCodecType [] GetAllCMVideoCodecTypes ()
		{
			return Enum.GetValues<CMVideoCodecType> ();
		}

		[Test]
		[TestCaseSource (nameof (GetAllCMVideoCodecTypes))]
		public void CopyVideoDecoderExtensionPropertiesTest (CMVideoCodecType codecType)
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var desc = CMFormatDescription.Create (CMMediaType.Video, (uint) codecType, out var fde);
			Assert.IsNotNull (desc, "CMFormatDescription");
			Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "CMFormatDescriptionError #2 (authorized)");
			using var dict = VTUtilities.CopyVideoDecoderExtensionProperties (desc, out var vtError);
			Assert.That (vtError, Is.EqualTo (VTStatus.CouldNotFindVideoDecoder).Or.EqualTo (VTStatus.CouldNotFindExtensionErr), "VTError");
			Assert.IsNull (dict, "CopyVideoDecoderExtensionProperties");

			// I have not been able to figure out what kind of CMVideoFormatDescription is needed for CopyVideoDecoderExtensionProperties to work,
			// so I can't test that case.
		}

		[Test]
		[TestCaseSource (nameof (GetAllCMVideoCodecTypes))]
		public void CopyRawVideoDecoderExtensionPropertiesTest (CMVideoCodecType codecType)
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var desc = CMFormatDescription.Create (CMMediaType.Video, (uint) codecType, out var fde);
			Assert.That (fde, Is.EqualTo (CMFormatDescriptionError.None), "CMFormatDescriptionError #2 (authorized)");
			Assert.IsNotNull (desc, "CMFormatDescription");
			using var dict = VTUtilities.CopyRawProcessorExtensionProperties (desc, out var vtError);
			Assert.That (vtError, Is.EqualTo (VTStatus.CouldNotCreateInstance).Or.EqualTo (VTStatus.CouldNotFindExtensionErr), "VTError");
			Assert.IsNull (dict, "CopyRawProcessorExtensionProperties");

			// I have not been able to figure out what kind of CMVideoFormatDescription is needed for VTRawProcessingSession,
			// so I can't test the case where a CMFormatDescription is handled.
		}
#endif

	}
}
