// 
// AVAssetDownloadTask.cs: AVAssetDownloadTask class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//
#if !TVOS
using System;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace AVFoundation {
#if !MONOMAC
	public partial class AVAssetDownloadUrlSession : NSUrlSession {

		/// <summary>Gets the shared session data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public new static NSUrlSession SharedSession {
			get {
				throw new NotSupportedException ("NS_UNAVAILABLE");
			}
		}

		public new static NSUrlSession FromConfiguration (NSUrlSessionConfiguration configuration)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public new static NSUrlSession FromConfiguration (NSUrlSessionConfiguration configuration, INSUrlSessionDelegate sessionDelegate, NSOperationQueue delegateQueue)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public new static NSUrlSession FromWeakConfiguration (NSUrlSessionConfiguration configuration, NSObject weakDelegate, NSOperationQueue delegateQueue)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDataTask CreateDataTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDataTask CreateDataTask (NSUrl url)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSUrl fileURL)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSData bodyData)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrlRequest request)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrl url)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTask (NSData resumeData)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDataTask CreateDataTask (NSUrlRequest request, NSUrlSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDataTask CreateDataTask (NSUrl url, NSUrlSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSUrl fileURL, NSUrlSessionResponse completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionUploadTask CreateUploadTask (NSUrlRequest request, NSData bodyData, NSUrlSessionResponse completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrlRequest request, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTask (NSUrl url, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}

		public override NSUrlSessionDownloadTask CreateDownloadTaskFromResumeData (NSData resumeData, NSUrlDownloadSessionResponse? completionHandler)
		{
			throw new NotSupportedException ("NS_UNAVAILABLE");
		}
	}
#endif
}
#endif
