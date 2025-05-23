//
// SearchKit.cs: simple bindings for Searchkit
//
// Copyright 2015 Xamarin Inc
//
// Author:
//    Miguel de Icaza
//
// TODO:
// TODO: SKIndexDocumentIteratorCreate
// TODO: SKIndexDocumentIteratorCopyNext
// TODO: SKIndexCopyInfoForDocumentIDs
// TODO: SKIndexCopyDocumentRefsForDocumentIDs
// TODO" SKIndexCopyDocumentURLsForDocumentIDs
// TODO: SKIndexCopyDocumentIDArrayForTermID
// TODO: SKIndexCopyTermIDArrayForDocumentID
// TODO: SKIndexCopyTermStringForTermID
// TODO: SKIndexGetTermIDForTermString
//

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using CoreFoundation;
using ObjCRuntime;
using Foundation;

using System.Runtime.InteropServices;

namespace SearchKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum SKIndexType {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Inverted,
		/// <summary>To be added.</summary>
		Vector,
		/// <summary>To be added.</summary>
		InvertedVector,
	};

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum SKSearchOptions {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		NoRelevanceScores = 1 << 0,
		/// <summary>To be added.</summary>
		SpaceMeansOr = 1 << 1,
		/// <summary>To be added.</summary>
		FindSimilar = 1 << 2,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public class SKSearch : NativeObject {
		[Preserve (Conditional = true)]
		internal SKSearch (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.SearchKitLibrary)]
		unsafe extern static byte SKSearchFindMatches (IntPtr handle, nint maxCount, IntPtr ids, IntPtr scores, double time, nint* foundCount);

		public bool FindMatches (nint maxCount, ref nint [] ids, double waitTime, out nint foundCount)
		{
			if (ids is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ids));
			if (ids.Length == 0)
				throw new ArgumentException ("ids should have at least one element");
			if (ids.Length != maxCount)
				throw new ArgumentException ("ids should have as many elements as maxCount");

			foundCount = default;
			unsafe {
				fixed (nint* p = ids) {
					return SKSearchFindMatches (Handle, maxCount, (IntPtr) p, IntPtr.Zero, waitTime, (nint*) Unsafe.AsPointer<nint> (ref foundCount)) != 0;
				}
			}
		}

		public bool FindMatches (nint maxCount, ref nint [] ids, ref float []? scores, double waitTime, out nint foundCount)
		{
			if (ids is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ids));
			if (ids.Length == 0)
				throw new ArgumentException ("ids should have at least one element");
			if (ids.Length != maxCount)
				throw new ArgumentException ("ids should have as many elements as maxCount");

			if (scores is not null) {
				if (scores.Length == 0)
					throw new ArgumentException ("scores should have at least one element");
				if (scores.Length != maxCount)
					throw new ArgumentException ("scores should have as many elements as maxCount");
			}
			foundCount = default;
			unsafe {
				fixed (nint* p = ids) {
					if (scores is null)
						return SKSearchFindMatches (Handle, maxCount, (IntPtr) p, IntPtr.Zero, waitTime, (nint*) Unsafe.AsPointer<nint> (ref foundCount)) != 0;
					else {
						fixed (float* s = scores) {
							return SKSearchFindMatches (Handle, maxCount, (IntPtr) p, (IntPtr) s, waitTime, (nint*) Unsafe.AsPointer<nint> (ref foundCount)) != 0;
						}
					}
				}
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static void SKSearchCancel (IntPtr h);
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Cancel ()
		{
			SKSearchCancel (Handle);
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public class SKDocument : NativeObject {
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentCreate (IntPtr scheme, IntPtr docParent, IntPtr name);
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentCreateWithURL (IntPtr url);

		static IntPtr Create (string name, SKDocument? parent = null, string? scheme = null)
		{
			if (name is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (name));
			var schemeHandle = CFString.CreateNative (scheme);
			var nameHandle = CFString.CreateNative (name);
			try {
				IntPtr result = SKDocumentCreate (schemeHandle, parent.GetHandle (), nameHandle);
				GC.KeepAlive (parent);
				return result;
			} finally {
				CFString.ReleaseNative (schemeHandle);
				CFString.ReleaseNative (nameHandle);
			}
		}

		/// <param name="name">To be added.</param>
		///         <param name="parent">To be added.</param>
		///         <param name="scheme">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SKDocument (string name, SKDocument? parent = null, string? scheme = null)
			: base (Create (name, parent, scheme), true, true)
		{
		}

		[Preserve (Conditional = true)]
		internal SKDocument (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SKDocument (NSUrl url)
			: base (SKDocumentCreateWithURL (url.GetNonNullHandle (nameof (url))), true, true)
		{
			GC.KeepAlive (url);
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentCopyURL (IntPtr h);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? Url {
			get {
				var url = SKDocumentCopyURL (GetCheckedHandle ());
				return Runtime.GetNSObject<NSUrl> (url);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentGetName (IntPtr h);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Name {
			get {
				var n = SKDocumentGetName (GetCheckedHandle ());
				return CFString.FromHandle (n);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentGetParent (IntPtr h);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SKDocument? GetParent ()
		{
			var parent = SKDocumentGetParent (GetCheckedHandle ());
			if (parent == IntPtr.Zero)
				return null;
			return new SKDocument (parent, false);
		}
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKDocumentGetSchemeName (IntPtr h);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Scheme {
			get {
				var s = SKDocumentGetSchemeName (GetCheckedHandle ());
				return CFString.FromHandle (s);
			}
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public class SKIndex : DisposableObject {
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexCreateWithURL (IntPtr url, IntPtr str, SKIndexType type, IntPtr dict);
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexCreateWithMutableData (IntPtr url, IntPtr str, SKIndexType type, IntPtr dict);
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexOpenWithURL (IntPtr url, IntPtr str, byte writeAccess);
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexOpenWithMutableData (IntPtr mutableData, IntPtr str);
		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexOpenWithData (IntPtr mutableData, IntPtr str);

		[DllImport (Constants.SearchKitLibrary)]
		extern static void SKIndexClose (IntPtr handle);

		[Preserve (Conditional = true)]
		SKIndex (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="url">To be added.</param>
		///         <param name="indexName">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="analysisProperties">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKIndex? CreateWithUrl (NSUrl url, string indexName, SKIndexType type, SKTextAnalysis analysisProperties)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			var indexNameHandle = CFString.CreateNative (indexName);
			try {
				var handle = SKIndexCreateWithURL (url.Handle, indexNameHandle, type, analysisProperties.GetHandle ());
				GC.KeepAlive (url);
				GC.KeepAlive (analysisProperties);
				if (handle == IntPtr.Zero)
					return null;
				return new SKIndex (handle, true);
			} finally {
				CFString.ReleaseNative (indexNameHandle);
			}
		}

		/// <param name="url">To be added.</param>
		///         <param name="indexName">To be added.</param>
		///         <param name="writeAccess">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKIndex? FromUrl (NSUrl url, string indexName, bool writeAccess)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			if (indexName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (indexName));
			var indexNameHandle = CFString.CreateNative (indexName);
			try {
				var handle = SKIndexOpenWithURL (url.Handle, indexNameHandle, writeAccess.AsByte ());
				GC.KeepAlive (url);
				if (handle == IntPtr.Zero)
					return null;
				return new SKIndex (handle, true);
			} finally {
				CFString.ReleaseNative (indexNameHandle);
			}
		}

		/// <param name="data">To be added.</param>
		///         <param name="indexName">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <param name="analysisProperties">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKIndex? CreateWithMutableData (NSMutableData data, string indexName, SKIndexType type, SKTextAnalysis analysisProperties)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			if (indexName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (indexName));
			var indexNameHandle = CFString.CreateNative (indexName);
			try {
				var handle = SKIndexCreateWithMutableData (data.Handle, indexNameHandle, type, analysisProperties.GetHandle ());
				GC.KeepAlive (data);
				GC.KeepAlive (analysisProperties);
				if (handle == IntPtr.Zero)
					return null;
				return new SKIndex (handle, true);
			} finally {
				CFString.ReleaseNative (indexNameHandle);
			}
		}

		/// <param name="data">To be added.</param>
		///         <param name="indexName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKIndex? FromMutableData (NSMutableData data, string indexName)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			if (indexName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (indexName));
			var indexNameHandle = CFString.CreateNative (indexName);
			try {
				var handle = SKIndexOpenWithMutableData (data.Handle, indexNameHandle);
				GC.KeepAlive (data);
				if (handle == IntPtr.Zero)
					return null;
				return new SKIndex (handle, true);
			} finally {
				CFString.ReleaseNative (indexNameHandle);
			}
		}

		/// <param name="data">To be added.</param>
		///         <param name="indexName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKIndex? FromData (NSData data, string indexName)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			if (indexName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (indexName));
			var indexNameHandle = CFString.CreateNative (indexName);
			try {
				var handle = SKIndexOpenWithData (data.Handle, indexNameHandle);
				GC.KeepAlive (data);
				if (handle == IntPtr.Zero)
					return null;
				return new SKIndex (handle, true);
			} finally {
				CFString.ReleaseNative (indexNameHandle);
			}
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Close ()
		{
			Dispose ();
		}

		/// <param name="disposing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		protected override void Dispose (bool disposing)
		{
			if (Handle != NativeHandle.Zero) {
				SKIndexClose (Handle);
			}
			base.Dispose (disposing);
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexAddDocumentWithText (IntPtr h, IntPtr doc, IntPtr str, byte canreplace);

		/// <param name="document">To be added.</param>
		///         <param name="text">To be added.</param>
		///         <param name="canReplace">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool AddDocumentWithText (SKDocument document, string text, bool canReplace)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			var textHandle = CFString.CreateNative (text);
			try {
				bool result = SKIndexAddDocumentWithText (Handle, document.Handle, textHandle, canReplace.AsByte ()) != 0;
				GC.KeepAlive (document);
				return result;
			} finally {
				CFString.ReleaseNative (textHandle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexAddDocument (IntPtr h, IntPtr doc, IntPtr mimeHintStr, byte canReplace);

		/// <param name="document">To be added.</param>
		///         <param name="mimeHint">To be added.</param>
		///         <param name="canReplace">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool AddDocument (SKDocument document, string mimeHint, bool canReplace)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			var mimeHintHandle = CFString.CreateNative (mimeHint);
			try {
				bool result = SKIndexAddDocument (Handle, document.Handle, mimeHintHandle, canReplace.AsByte ()) != 0;
				GC.KeepAlive (document);
				return result;
			} finally {
				CFString.ReleaseNative (mimeHintHandle);
			}
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.SearchKitLibrary, EntryPoint = "SKLoadDefaultExtractorPlugIns")]
		public extern static void LoadDefaultExtractorPlugIns ();

		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexFlush (IntPtr h);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Flush ()
		{
			return SKIndexFlush (Handle) != 0;
		}
		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexCompact (IntPtr h);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Compact ()
		{
			return SKIndexCompact (Handle) != 0;
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKIndexGetDocumentCount (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint DocumentCount {
			get {
				return SKIndexGetDocumentCount (Handle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKIndexGetMaximumDocumentID (IntPtr handle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint MaximumDocumentID {
			get {
				return SKIndexGetMaximumDocumentID (Handle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKIndexGetMaximumTermID (IntPtr handle);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint MaximumTermID {
			get {
				return SKIndexGetMaximumTermID (Handle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexGetAnalysisProperties (IntPtr h);
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SKTextAnalysis AnalysisProperties {
			get {
				return new SKTextAnalysis (Runtime.GetNSObject<NSDictionary> (SKIndexGetAnalysisProperties (Handle)));
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexMoveDocument (IntPtr h, IntPtr document, IntPtr newParent);
		/// <param name="document">To be added.</param>
		///         <param name="newParent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool MoveDocument (SKDocument document, SKDocument newParent)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			if (newParent is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newParent));
			bool result = SKIndexMoveDocument (Handle, document.Handle, newParent.Handle) != 0;
			GC.KeepAlive (document);
			GC.KeepAlive (newParent);
			return result;
		}


		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexRemoveDocument (IntPtr h, IntPtr doc);

		/// <param name="document">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool RemoveDocument (SKDocument document)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			bool result = SKIndexRemoveDocument (Handle, document.Handle) != 0;
			GC.KeepAlive (document);
			return result;
		}


		[DllImport (Constants.SearchKitLibrary)]
		extern static byte SKIndexRenameDocument (IntPtr h, IntPtr doc, IntPtr newName);
		/// <param name="document">To be added.</param>
		///         <param name="newName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool RenameDocument (SKDocument document, string newName)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			if (newName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newName));
			var newNameHandle = CFString.CreateNative (newName);
			try {
				bool result = SKIndexRenameDocument (Handle, document.Handle, newNameHandle) != 0;
				GC.KeepAlive (document);
				return result;
			} finally {
				CFString.ReleaseNative (newNameHandle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static void SKIndexSetMaximumBytesBeforeFlush (IntPtr h, nint value);

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKIndexGetMaximumBytesBeforeFlush (IntPtr h);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Advice ("Apple recommends to use Flush instead of setting these parameters.")]
		public nint MaximumBytesBeforeFlush {
			get {
				return SKIndexGetMaximumBytesBeforeFlush (Handle);
			}
			set {
				SKIndexSetMaximumBytesBeforeFlush (Handle, value);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKSearchCreate (IntPtr h, IntPtr str, SKSearchOptions options);

		/// <param name="query">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SKSearch Search (string query, SKSearchOptions options = SKSearchOptions.Default)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));
			var queryHandle = CFString.CreateNative (query);
			try {
				return new SKSearch (SKSearchCreate (Handle, queryHandle, options), true);
			} finally {
				CFString.ReleaseNative (queryHandle);
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKIndexCopyDocumentForDocumentID (IntPtr idx, /* SKDocumentID -> CFIndex */ nint docId);

		public SKDocument? GetDocument (nint documentId)
		{
			var doc = SKIndexCopyDocumentForDocumentID (Handle, documentId);
			if (doc == IntPtr.Zero)
				return null;
			return new SKDocument (doc, true);
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static void SKIndexSetDocumentProperties (IntPtr h, IntPtr doc, IntPtr dict);
		/// <param name="document">To be added.</param>
		///         <param name="dict">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetDocumentProperties (SKDocument document, NSDictionary dict)
		{
			if (document is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (document));
			if (dict is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dict));
			SKIndexSetDocumentProperties (Handle, document.Handle, dict.Handle);
			GC.KeepAlive (document);
			GC.KeepAlive (dict);
		}
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public class SKSummary : NativeObject {
		[Preserve (Conditional = true)]
		internal SKSummary (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr SKSummaryCreateWithString (/* NSString */ IntPtr str);

		/// <param name="text">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKSummary? Create (string text)
		{
			if (text is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (text));
			var x = CFString.CreateNative (text);
			try {
				var handle = SKSummaryCreateWithString (x);
				if (handle == IntPtr.Zero)
					return null;
				return new SKSummary (handle, true);
			} finally {
				CFString.ReleaseNative (x);
			}
		}

		/// <param name="nsString">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SKSummary? Create (NSString nsString)
		{
			if (nsString is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (nsString));

			var h = SKSummaryCreateWithString (nsString.Handle);
			GC.KeepAlive (nsString);
			if (h == IntPtr.Zero)
				return null;

			return new SKSummary (h, true);
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKSummaryGetSentenceSummaryInfo (IntPtr summary, nint maxNumSentencesInSummary, IntPtr rankOrderOfSentences, IntPtr sentenceIndexOfSentences, IntPtr paragraphIndexOfSentences);

		nint []? hack;

		public nint GetSentenceSummaryInfo (int maxNumSentencesInSummary, nint [] rankOrderOfSentences, nint [] sentenceIndexOfSentences, nint [] paragraphIndexOfSentences)
		{
			if (rankOrderOfSentences is not null && rankOrderOfSentences.Length != maxNumSentencesInSummary)
				throw new ArgumentException ("array must contain as many element as specified in maxNumSentencesInSummary", nameof (rankOrderOfSentences));

			if (sentenceIndexOfSentences is not null && sentenceIndexOfSentences.Length != maxNumSentencesInSummary)
				throw new ArgumentException ("array must contain as many element as specified in maxNumSentencesInSummary", nameof (sentenceIndexOfSentences));

			if (paragraphIndexOfSentences is not null && paragraphIndexOfSentences.Length != maxNumSentencesInSummary)
				throw new ArgumentException ("array must contain as many element as specified in maxNumSentencesInSummary", nameof (paragraphIndexOfSentences));

			//
			// Because of how fixed works and our incoming arguments might be null, we are going to use this
			// fake array to take the address of.   And then, before we call the method, we check if
			// we want to pass that value or not.
			//
			if (hack is null)
				hack = new nint [1];

			unsafe {
				nint [] arr = rankOrderOfSentences is null ? hack : rankOrderOfSentences;
				nint [] ars = sentenceIndexOfSentences is null ? hack : sentenceIndexOfSentences;
				nint [] arp = paragraphIndexOfSentences is null ? hack : paragraphIndexOfSentences;

				fixed (nint* r = arr) {
					fixed (nint* s = ars) {
						fixed (nint* p = arp) {
							fixed (nint* hp = hack) {
								return SKSummaryGetSentenceSummaryInfo (Handle, maxNumSentencesInSummary,
													(IntPtr) (r == hp ? null : r),
													(IntPtr) (s == hp ? null : s),
													(IntPtr) (p == hp ? null : p));
							}
						}
					}
				}
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKSummaryGetParagraphSummaryInfo (IntPtr summary, nint maxNumParagraphsInSummary, IntPtr rankOrderOfParagraphs, IntPtr paragraphIndexOfParagraphs);

		public nint GetParagraphSummaryInfo (nint maxNumParagraphsInSummary, nint [] rankOrderOfParagraphs, nint [] paragraphIndexOfParagraphs)
		{
			if (rankOrderOfParagraphs is not null && rankOrderOfParagraphs.Length != maxNumParagraphsInSummary)
				throw new ArgumentException ("array must contain as many element as specified in maxNumParagraphsInSummary", nameof (rankOrderOfParagraphs));
			if (paragraphIndexOfParagraphs is not null && paragraphIndexOfParagraphs.Length != maxNumParagraphsInSummary)
				throw new ArgumentException ("array must contain as many element as specified in maxNumParagraphsInSummary", nameof (paragraphIndexOfParagraphs));

			//
			// Because of how fixed works and our incoming arguments might be null, we are going to use this
			// fake array to take the address of.   And then, before we call the method, we check if
			// we want to pass that value or not.
			//
			if (hack is null)
				hack = new nint [1];

			unsafe {
				nint [] ar = rankOrderOfParagraphs is null ? hack : rankOrderOfParagraphs;
				nint [] ap = paragraphIndexOfParagraphs is null ? hack : paragraphIndexOfParagraphs;

				fixed (nint* r = ar) {
					fixed (nint* p = ap) {
						fixed (nint* hp = hack) {
							return SKSummaryGetParagraphSummaryInfo (Handle, maxNumParagraphsInSummary,
												 (IntPtr) (r == hp ? null : r),
												 (IntPtr) (p == hp ? null : p));
						}
					}
				}
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKSummaryGetSentenceCount (IntPtr summary);
		[DllImport (Constants.SearchKitLibrary)]
		extern static nint SKSummaryGetParagraphCount (IntPtr summary);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint SentenceCount {
			get {
				return SKSummaryGetSentenceCount (GetCheckedHandle ());
			}
		}
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint ParagraphCount {
			get {
				return SKSummaryGetParagraphCount (GetCheckedHandle ());
			}
		}

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr /*NSString*/ SKSummaryCopySentenceAtIndex (IntPtr summary, nint idx);

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr /*NSString*/ SKSummaryCopyParagraphAtIndex (IntPtr summary, nint idx);

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr /*NSString*/ SKSummaryCopySentenceSummaryString (IntPtr summary, nint maxSentences);

		[DllImport (Constants.SearchKitLibrary)]
		extern static IntPtr /*NSString*/  SKSummaryCopyParagraphSummaryString (IntPtr summary, nint maxParagraphs);

		public string? GetSentence (nint idx)
		{
			return CFString.FromHandle (SKSummaryCopySentenceAtIndex (Handle, idx), releaseHandle: true);
		}

		public string? GetParagraph (nint idx)
		{
			return CFString.FromHandle (SKSummaryCopyParagraphAtIndex (Handle, idx), releaseHandle: true);
		}

		public string? GetSentenceSummary (nint maxSentences)
		{
			return CFString.FromHandle (SKSummaryCopySentenceSummaryString (Handle, maxSentences), releaseHandle: true);
		}

		public string? GetParagraphSummary (nint maxParagraphs)
		{
			return CFString.FromHandle (SKSummaryCopyParagraphSummaryString (Handle, maxParagraphs), releaseHandle: true);
		}

	}
}
