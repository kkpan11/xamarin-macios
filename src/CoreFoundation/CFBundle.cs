//
// Copyright 2015 Xamarin Inc
//

#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace CoreFoundation {

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public partial class CFBundle : NativeObject {

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		public enum PackageType {
			/// <summary>To be added.</summary>
			Application,
			/// <summary>To be added.</summary>
			Framework,
			/// <summary>To be added.</summary>
			Bundle,
		}

		/// <summary>To be added.</summary>
		///     <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public struct PackageInfo {
			/// <param name="type">To be added.</param>
			///         <param name="creator">To be added.</param>
			///         <summary>To be added.</summary>
			///         <remarks>To be added.</remarks>
			public PackageInfo (CFBundle.PackageType type, string creator)
			{
				this.Type = type;
				this.Creator = creator;
			}

			/// <summary>To be added.</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public PackageType Type { get; private set; }
			/// <summary>To be added.</summary>
			///         <value>To be added.</value>
			///         <remarks>To be added.</remarks>
			public string Creator { get; private set; }
		}

		[Preserve (Conditional = true)]
		internal CFBundle (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFBundleRef */ IntPtr CFBundleCreate ( /* CFAllocatorRef can be null */ IntPtr allocator, /* CFUrlRef */ IntPtr bundleURL);

		static IntPtr Create (NSUrl bundleUrl)
		{
			if (bundleUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundleUrl));

			IntPtr result = CFBundleCreate (IntPtr.Zero, bundleUrl.Handle);
			GC.KeepAlive (bundleUrl);
			return result;
		}

		/// <param name="bundleUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CFBundle (NSUrl bundleUrl)
			: base (Create (bundleUrl), true)
		{
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArrayRef */ IntPtr CFBundleCreateBundlesFromDirectory (/* CFAllocatorRef can be null */ IntPtr allocator, /* CFUrlRef */ IntPtr directoryURL, /* CFStringRef */ IntPtr bundleType);

		/// <param name="directoryUrl">To be added.</param>
		///         <param name="bundleType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFBundle []? GetBundlesFromDirectory (NSUrl directoryUrl, string bundleType)
		{
			if (directoryUrl is null) // NSUrl cannot be "" by definition
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (directoryUrl));
			if (String.IsNullOrEmpty (bundleType))
				throw new ArgumentException (nameof (bundleType));
			var bundleTypeHandle = CFString.CreateNative (bundleType);
			try {
				var rv = CFBundleCreateBundlesFromDirectory (IntPtr.Zero, directoryUrl.Handle, bundleTypeHandle);
				GC.KeepAlive (directoryUrl);
				return CFArray.ArrayFromHandleFunc (rv, (handle) => new CFBundle (handle, true), true);
			} finally {
				CFString.ReleaseNative (bundleTypeHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFBundleGetAllBundles ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFBundle []? GetAll ()
		{
			// as per apple documentation: 
			// CFBundleGetAllBundles
			//
			// 	'This function is potentially expensive and not thread-safe'
			//
			// This means, that we should not trust the size of the array, since is a get and
			// might be modified by a diff thread. We are going to clone the array and make sure
			// that Apple does not modify the array while we work with it. That avoids changes
			// in the index or in the bundles returned.
			using (var cfBundles = new CFArray (CFBundleGetAllBundles (), false))
			using (var cfBundlesCopy = cfBundles.Clone ()) {
				return CFArray.ArrayFromHandleFunc<CFBundle> (cfBundlesCopy.Handle, (handle) => new CFBundle (handle, false), false);
			}
		}


		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFBundleGetBundleWithIdentifier (/* CFStringRef */ IntPtr bundleID);

		/// <param name="bundleID">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFBundle? Get (string bundleID)
		{
			if (String.IsNullOrEmpty (bundleID))
				throw new ArgumentException (nameof (bundleID));
			var bundleIDHandler = CFString.CreateNative (bundleID);
			try {
				var cfBundle = CFBundleGetBundleWithIdentifier (bundleIDHandler);
				if (cfBundle == IntPtr.Zero)
					return null;
				// follow the Get rule and retain the obj
				return new CFBundle (cfBundle, false);
			} finally {
				CFString.ReleaseNative (bundleIDHandler);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFBundleGetMainBundle ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CFBundle? GetMain ()
		{
			var cfBundle = CFBundleGetMainBundle ();
			if (cfBundle == IntPtr.Zero)
				return null;
			// follow the get rule and retain
			return new CFBundle (cfBundle, false);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFBundleIsExecutableLoaded (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool HasLoadedExecutable {
			get { return CFBundleIsExecutableLoaded (Handle) != 0; }
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe extern static byte CFBundlePreflightExecutable (IntPtr bundle, IntPtr* error);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool PreflightExecutable (out NSError? error)
		{
			IntPtr errorPtr = IntPtr.Zero;
			// follow the create rule, no need to retain
			byte loaded;
			unsafe {
				loaded = CFBundlePreflightExecutable (Handle, &errorPtr);
			}
			error = Runtime.GetNSObject<NSError> (errorPtr);
			return loaded != 0;
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe extern static byte CFBundleLoadExecutableAndReturnError (IntPtr bundle, IntPtr* error);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool LoadExecutable (out NSError? error)
		{
			IntPtr errorPtr = IntPtr.Zero;
			// follows the create rule, no need to retain
			byte loaded;
			unsafe {
				loaded = CFBundleLoadExecutableAndReturnError (Handle, &errorPtr);
			}
			error = Runtime.GetNSObject<NSError> (errorPtr);
			return loaded != 0;
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static void CFBundleUnloadExecutable (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void UnloadExecutable ()
		{
			CFBundleUnloadExecutable (Handle);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyAuxiliaryExecutableURL (IntPtr bundle, /* CFStringRef */ IntPtr executableName);

		/// <param name="executableName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl? GetAuxiliaryExecutableUrl (string executableName)
		{
			if (String.IsNullOrEmpty (executableName))
				throw new ArgumentException (nameof (executableName));
			var executableNameHandle = CFString.CreateNative (executableName);
			try {
				// follows the create rule no need to retain
				var urlHandle = CFBundleCopyAuxiliaryExecutableURL (Handle, executableNameHandle);
				return Runtime.GetNSObject<NSUrl> (urlHandle, true);
			} finally {
				CFString.ReleaseNative (executableNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyBuiltInPlugInsURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? BuiltInPlugInsUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopyBuiltInPlugInsURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyExecutableURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? ExecutableUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopyExecutableURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyPrivateFrameworksURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? PrivateFrameworksUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopyPrivateFrameworksURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyResourcesDirectoryURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? ResourcesDirectoryUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopyResourcesDirectoryURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopySharedFrameworksURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? SharedFrameworksUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopySharedFrameworksURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopySharedSupportURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? SharedSupportUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopySharedSupportURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopySupportFilesDirectoryURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? SupportFilesDirectoryUrl {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopySupportFilesDirectoryURL (Handle), true);
			}
		}

		// the parameters do not take CFString because we want to be able to pass null (IntPtr.Zero) to the resource type and subdir names
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyResourceURL (IntPtr bundle, /* CFStringRef */ IntPtr resourceName, /* CFString */ IntPtr resourceType, /* CFString */ IntPtr subDirName);

		/// <param name="resourceName">To be added.</param>
		///         <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl? GetResourceUrl (string resourceName, string resourceType, string subDirName)
		{
			if (String.IsNullOrEmpty (resourceName))
				throw new ArgumentException (nameof (resourceName));

			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			var resourceNameHandle = CFString.CreateNative (resourceName);
			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			try {
				// follows the create rules and therefore we do not need to retain
				var urlHandle = CFBundleCopyResourceURL (Handle, resourceNameHandle, resourceTypeHandle, dirNameHandle);
				return Runtime.GetNSObject<NSUrl> (urlHandle, true);
			} finally {
				CFString.ReleaseNative (resourceNameHandle);
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyResourceURLInDirectory (/* CFUrlRef */ IntPtr bundleURL, /* CFStringRef */ IntPtr resourceName, /* CFStringRef */ IntPtr resourceType, /* CFStringRef */ IntPtr subDirName);

		/// <param name="bundleUrl">To be added.</param>
		///         <param name="resourceName">To be added.</param>
		///         <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSUrl? GetResourceUrl (NSUrl bundleUrl, string resourceName, string resourceType, string subDirName)
		{
			if (bundleUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundleUrl));

			if (String.IsNullOrEmpty (resourceName))
				throw new ArgumentException (nameof (resourceName));

			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			var resourceNameHandle = CFString.CreateNative (resourceName);
			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			try {
				// follows the create rules and therefore we do not need to retain
				var urlHandle = CFBundleCopyResourceURLInDirectory (bundleUrl.Handle, resourceNameHandle, resourceTypeHandle, dirNameHandle);
				GC.KeepAlive (bundleUrl);
				return Runtime.GetNSObject<NSUrl> (urlHandle, true);
			} finally {
				CFString.ReleaseNative (resourceNameHandle);
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyResourceURLsOfType (IntPtr bundle, /* CFStringRef */ IntPtr resourceType, /* CFStringRef */ IntPtr subDirName);

		/// <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl? []? GetResourceUrls (string resourceType, string subDirName)
		{
			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			try {
				var rv = CFBundleCopyResourceURLsOfType (Handle, resourceTypeHandle, dirNameHandle);
				return CFArray.ArrayFromHandleFunc (rv, (handle) => Runtime.GetNSObject<NSUrl> (handle, true), true);
			} finally {
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyResourceURLsOfTypeInDirectory (/* CFUrlRef */ IntPtr bundleURL, /* CFStringRef */ IntPtr resourceType, /* CFStringRef */ IntPtr subDirName);

		/// <param name="bundleUrl">To be added.</param>
		///         <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSUrl? []? GetResourceUrls (NSUrl bundleUrl, string resourceType, string subDirName)
		{
			if (bundleUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundleUrl));

			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			try {
				var rv = CFBundleCopyResourceURLsOfTypeInDirectory (bundleUrl.Handle, resourceTypeHandle, dirNameHandle);
				GC.KeepAlive (bundleUrl);
				return CFArray.ArrayFromHandleFunc (rv, (handle) => Runtime.GetNSObject<NSUrl> (handle, true), true);
			} finally {
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyResourceURLForLocalization (IntPtr bundle, /* CFStringRef */ IntPtr resourceName, /* CFStringRef */ IntPtr resourceType, /* CFStringRef */ IntPtr subDirName,
																					/* CFStringRef */ IntPtr localizationName);

		/// <param name="resourceName">To be added.</param>
		///         <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <param name="localizationName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl? GetResourceUrl (string resourceName, string resourceType, string subDirName, string localizationName)
		{
			if (String.IsNullOrEmpty (resourceName))
				throw new ArgumentException (nameof (resourceName));

			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			if (String.IsNullOrEmpty (localizationName))
				throw new ArgumentException (nameof (localizationName));

			var resourceNameHandle = CFString.CreateNative (resourceName);
			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			var localizationNameHandle = CFString.CreateNative (localizationName);
			try {
				var urlHandle = CFBundleCopyResourceURLForLocalization (Handle, resourceNameHandle, resourceTypeHandle, dirNameHandle, localizationNameHandle);
				return Runtime.GetNSObject<NSUrl> (urlHandle, true);
			} finally {
				CFString.ReleaseNative (resourceNameHandle);
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
				CFString.ReleaseNative (localizationNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyResourceURLsOfTypeForLocalization (IntPtr bundle, /* CFStringRef */ IntPtr resourceType, /* CFStringRef */ IntPtr subDirName,
																						  /* CFStringRef */ IntPtr localizationName);

		/// <param name="resourceType">To be added.</param>
		///         <param name="subDirName">To be added.</param>
		///         <param name="localizationName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSUrl? []? GetResourceUrls (string resourceType, string subDirName, string localizationName)
		{
			if (String.IsNullOrEmpty (resourceType))
				throw new ArgumentException (nameof (resourceType));

			if (String.IsNullOrEmpty (localizationName))
				throw new ArgumentException (nameof (localizationName));

			var resourceTypeHandle = CFString.CreateNative (resourceType);
			var dirNameHandle = CFString.CreateNative (string.IsNullOrEmpty (subDirName) ? null : subDirName);
			var localizationNameHandle = CFString.CreateNative (localizationName);
			try {
				var rv = CFBundleCopyResourceURLsOfTypeForLocalization (Handle, resourceTypeHandle, dirNameHandle, localizationNameHandle);
				return CFArray.ArrayFromHandleFunc (rv, (handle) => Runtime.GetNSObject<NSUrl> (handle, true), true);
			} finally {
				CFString.ReleaseNative (resourceTypeHandle);
				CFString.ReleaseNative (dirNameHandle);
				CFString.ReleaseNative (localizationNameHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFString */ IntPtr CFBundleCopyLocalizedString (IntPtr bundle, /* CFStringRef */ IntPtr key, /* CFStringRef */ IntPtr value, /* CFStringRef */ IntPtr tableName);

		/// <summary>Gets a localized string.</summary>
		/// <param name="key">The key of the localized string to return.</param>
		/// <param name="defaultValue">A default value if no localized string is found for the specified <paramref name="key" />.</param>
		/// <param name="tableName">The name of the strings file to look in. The default is the 'Localizable.strings' file.</param>
		/// <returns>A localized string for the key <paramref name="key" /> in the table <paramref name="tableName" />.</returns>
		/// <remarks>Returns a localized string for the current bundle.</remarks>
		public string? GetLocalizedString (string key, string defaultValue, string? tableName = null)
		{
			if (String.IsNullOrEmpty (key))
				throw new ArgumentException (nameof (key));

			// we do allow null and simply use an empty string to avoid the extra check
			if (defaultValue is null)
				defaultValue = string.Empty;

			var keyHandle = CFString.CreateNative (key);
			var defaultValueHandle = CFString.CreateNative (defaultValue);
			var tableNameHandle = CFString.CreateNative (tableName);
			try {
				var rv = CFBundleCopyLocalizedString (Handle, keyHandle, defaultValueHandle, tableNameHandle);
				return CFString.FromHandle (rv, releaseHandle: true);
			} finally {
				CFString.ReleaseNative (keyHandle);
				CFString.ReleaseNative (defaultValueHandle);
				CFString.ReleaseNative (tableNameHandle);
			}
		}

		[SupportedOSPlatform ("macos15.4")]
		[SupportedOSPlatform ("ios18.4")]
		[SupportedOSPlatform ("tvos18.4")]
		[SupportedOSPlatform ("maccatalyst18.4")]
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFStringRef */ IntPtr CFBundleCopyLocalizedStringForLocalizations (/* CFBundleRef */ IntPtr bundle, /* CFStringRef */ IntPtr key, /* CFStringRef */ IntPtr value, /* CFStringRef */ IntPtr tableName, /* CFArrayRef */ IntPtr localizations);

		/// <summary>Gets a localized string for a list of possible localizations.</summary>
		/// <param name="key">The key of the localized string to return.</param>
		/// <param name="defaultValue">A default value if no localized string is found for the specified <paramref name="key" />.</param>
		/// <param name="tableName">The name of the strings file to look in. Specify <c>null</c> to use the default 'Localizable.strings' file.</param>
		/// <param name="localizations">An array of BCP 47 language codes to determine which localized string to return.</param>
		/// <returns>A localized string for the key <paramref name="key" /> in the table <paramref name="tableName" />.</returns>
		/// <remarks>Returns the most suitable localized string for the current bundle.</remarks>
		[SupportedOSPlatform ("macos15.4")]
		[SupportedOSPlatform ("ios18.4")]
		[SupportedOSPlatform ("tvos18.4")]
		[SupportedOSPlatform ("maccatalyst18.4")]
		public string? GetLocalizedString (string key, string? defaultValue, string? tableName, string [] localizations)
		{
			if (string.IsNullOrEmpty (key))
				throw new ArgumentException (nameof (key));

			if (localizations is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (localizations));

			using var keyHandle = new TransientCFString (key);
			using var defaultValueHandle = new TransientCFString (defaultValue);
			using var tableNameHandle = new TransientCFString (tableName);
			using var localizationsArrayHandle = new TransientCFObject (CFArray.Create (localizations));
			var rv = CFBundleCopyLocalizedStringForLocalizations (Handle, keyHandle, defaultValueHandle, tableNameHandle, localizationsArrayHandle);
			return CFString.FromHandle (rv, releaseHandle: true);
		}

		/// <summary>Gets a localized string for a list of possible localizations.</summary>
		/// <param name="key">The key of the localized string to return.</param>
		/// <param name="defaultValue">A default value if no localized string is found for the specified <paramref name="key" />.</param>
		/// <param name="tableName">The name of the strings file to look in.</param>
		/// <param name="localizations">An array of languages to determine which localized string to return.</param>
		/// <returns>A localized string for the key <paramref name="key" /> in the table <paramref name="tableName" />.</returns>
		/// <remarks>Returns the most suitable localized string for the current bundle.</remarks>
		[SupportedOSPlatform ("macos15.4")]
		[SupportedOSPlatform ("ios18.4")]
		[SupportedOSPlatform ("tvos18.4")]
		[SupportedOSPlatform ("maccatalyst18.4")]
		public string? GetLocalizedString (string key, string defaultValue, string tableName, params CultureInfo [] localizations)
		{
			var locs = new string [localizations.Length];
			for (var i = 0; i < localizations.Length; i++)
				locs [i] = localizations [i].Name;
			return GetLocalizedString (key, defaultValue, tableName, locs);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyLocalizationsForPreferences (/* CFArrayRef */ IntPtr locArray, /* CFArrayRef */ IntPtr prefArray);

		/// <param name="locArray">To be added.</param>
		///         <param name="prefArray">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? []? GetLocalizationsForPreferences (string [] locArray, string [] prefArray)
		{
			if (locArray is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (locArray));
			if (prefArray is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (prefArray));

			var cfLocalArrayHandle = IntPtr.Zero;
			var cfPrefArrayHandle = IntPtr.Zero;
			try {
				cfLocalArrayHandle = CFArray.Create (locArray);
				cfPrefArrayHandle = CFArray.Create (prefArray);

				var rv = CFBundleCopyLocalizationsForPreferences (cfLocalArrayHandle, cfPrefArrayHandle);
				return CFArray.StringArrayFromHandle (rv, true);
			} finally {
				if (cfLocalArrayHandle != IntPtr.Zero)
					CFObject.CFRelease (cfLocalArrayHandle);
				if (cfPrefArrayHandle != IntPtr.Zero)
					CFObject.CFRelease (cfPrefArrayHandle);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyLocalizationsForURL (/* CFUrlRef */ IntPtr url);

		/// <param name="bundle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? []? GetLocalizations (NSUrl bundle)
		{
			if (bundle is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundle));
			var rv = CFBundleCopyLocalizationsForURL (bundle.Handle);
			GC.KeepAlive (bundle);
			return CFArray.StringArrayFromHandle (rv, true);
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyPreferredLocalizationsFromArray (/* CFArrayRef */ IntPtr locArray);

		/// <param name="locArray">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? []? GetPreferredLocalizations (string [] locArray)
		{
			if (locArray is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (locArray));

			var cfString = new CFString [locArray.Length];
			for (int index = 0; index < locArray.Length; index++) {
				cfString [index] = new CFString (locArray [index]);
			}
			using (var cfLocArray = CFArray.FromNativeObjects (cfString)) {
				var rv = CFBundleCopyPreferredLocalizationsFromArray (cfLocArray.Handle);
				return CFArray.StringArrayFromHandle (rv, true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFUrlRef */ IntPtr CFBundleCopyBundleURL (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSUrl? Url {
			get {
				return Runtime.GetNSObject<NSUrl> (CFBundleCopyBundleURL (Handle), true);
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFString */ IntPtr CFBundleGetDevelopmentRegion (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? DevelopmentRegion {
			get { return CFString.FromHandle (CFBundleGetDevelopmentRegion (Handle)); }
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFString */ IntPtr CFBundleGetIdentifier (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Identifier {
			get { return CFString.FromHandle (CFBundleGetIdentifier (Handle)); }
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFDictionary */ IntPtr CFBundleGetInfoDictionary (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary? InfoDictionary {
			get {
				// follows the Get rule, we need to retain
				return Runtime.GetNSObject<NSDictionary> (CFBundleGetInfoDictionary (Handle));
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* NSDictionary */ IntPtr CFBundleGetLocalInfoDictionary (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary? LocalInfoDictionary {
			get {
				// follows the Get rule, we need to retain
				return Runtime.GetNSObject<NSDictionary> (CFBundleGetLocalInfoDictionary (Handle));
			}
		}

		// We do not bind CFDictionaryRef CFBundleCopyInfoDictionaryInDirectory because we will use CFBundleCopyInfoDictionaryForURL. As per the apple documentation
		// For a directory URL, this is equivalent to CFBundleCopyInfoDictionaryInDirectory. For a plain file URL representing an unbundled application, this function
		// will attempt to read an information dictionary either from the (__TEXT, __info_plist) section of the file (for a Mach-O file) or from a plst resource. 

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* NSDictionary */ IntPtr CFBundleCopyInfoDictionaryForURL (/* CFUrlRef */ IntPtr url);

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static NSDictionary? GetInfoDictionary (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			// follow the create rule, no need to retain
			NSDictionary? result = Runtime.GetNSObject<NSDictionary> (CFBundleCopyInfoDictionaryForURL (url.Handle));
			GC.KeepAlive (url);
			return result;
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		unsafe extern static void CFBundleGetPackageInfo (IntPtr bundle, uint* packageType, uint* packageCreator);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public PackageInfo Info {
			get {
				uint type = 0;
				uint creator = 0;

				unsafe {
					CFBundleGetPackageInfo (Handle, &type, &creator);
				}
				var creatorStr = Runtime.ToFourCCString (creator);
				switch (type) {
				case 1095782476: // ""APPL
					return new PackageInfo (CFBundle.PackageType.Application, creatorStr);
				case 1179473739: // "FMWK"
					return new PackageInfo (CFBundle.PackageType.Framework, creatorStr);
				case 1112425548: // "BNDL" so that we know we did not forget about this value
				default:
					return new PackageInfo (CFBundle.PackageType.Bundle, creatorStr);
				}
			}
		}

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static /* CFArray */ IntPtr CFBundleCopyExecutableArchitectures (IntPtr bundle);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CFBundle.Architecture []? Architectures {
			get {
				var rv = CFBundleCopyExecutableArchitectures (Handle);
				return CFArray.ArrayFromHandleFunc (rv,
					(handle) => {
						byte rv;
						int value;
						unsafe {
							rv = CFDictionary.CFNumberGetValue (handle, /* kCFNumberSInt32Type */ 3, &value);
						}
						if (rv != 0)
							return (CFBundle.Architecture) value;
						return default (CFBundle.Architecture);
					}, true);
			}
		}

#if MONOMAC
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFBundleIsExecutableLoadable (IntPtr bundle);

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static bool IsExecutableLoadable (CFBundle bundle)
		{
			if (bundle is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundle));

			bool result = CFBundleIsExecutableLoadable (bundle.GetCheckedHandle ()) != 0;
			GC.KeepAlive (bundle);
			return result;
		}

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFBundleIsExecutableLoadableForURL (IntPtr bundle);

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static bool IsExecutableLoadable (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			bool result = CFBundleIsExecutableLoadableForURL (url.Handle) != 0;
			GC.KeepAlive (url);
			return result;
		}

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreFoundationLibrary)]
		extern static byte CFBundleIsArchitectureLoadable (/*cpu_type_t => integer_t => int*/ Architecture architecture);

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static bool IsArchitectureLoadable (Architecture architecture) => CFBundleIsArchitectureLoadable (architecture) != 0;

#endif
	}
}
