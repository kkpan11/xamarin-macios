using System;
using System.IO;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;
using Xamarin.Utils;
using Xamarin.Localization.MSBuild;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public class ParseDeviceSpecificBuildInformation : XamarinTask {
		#region Inputs

		[Required]
		public string Architectures { get; set; } = "";

		[Required]
		public string IntermediateOutputPath { get; set; } = "";

		[Required]
		public string OutputPath { get; set; } = "";

		[Required]
		public string TargetiOSDevice { get; set; } = "";

		#endregion

		#region Outputs

		[Output]
		public string DeviceSpecificIntermediateOutputPath { get; set; } = "";

		[Output]
		public string DeviceSpecificOutputPath { get; set; } = "";

		[Output]
		public string TargetArchitectures { get; set; } = "";

		[Output]
		public string TargetDeviceModel { get; set; } = "";

		[Output]
		public string TargetDeviceOSVersion { get; set; } = "";

		#endregion

		public override bool Execute ()
		{
			var target = TargetArchitecture.Default;
			string targetOperatingSystem;

			switch (Platform) {
			case ApplePlatform.TVOS:
				targetOperatingSystem = "tvOS";
				break;
			case ApplePlatform.iOS:
				targetOperatingSystem = "iOS";
				break;
			default:
				throw new InvalidOperationException (string.Format (MSBStrings.InvalidPlatform, Platform));
			}

			if (!Enum.TryParse<TargetArchitecture> (Architectures, out var architectures)) {
				Log.LogError (MSBStrings.E0057, Architectures);
				return false;
			}

			var plist = PObject.FromString (TargetiOSDevice) as PDictionary;
			if (plist is null) {
				Log.LogError (MSBStrings.E0058);
				return false;
			}

			if (!plist.TryGetValue<PDictionary> ("device", out var device)) {
				Log.LogError (MSBStrings.E0059);
				return false;
			}

			if (!device.TryGetValue<PString> ("architecture", out var value)) {
				Log.LogError (MSBStrings.E0060);
				return false;
			}

			if (!Enum.TryParse<TargetArchitecture> (value.Value, out var deviceArchitectures) || deviceArchitectures == TargetArchitecture.Default) {
				Log.LogError (MSBStrings.E0061, value.Value);
				return false;
			}

			if (!device.TryGetValue<PString> ("os", out var os)) {
				Log.LogError (MSBStrings.E0062);
				return false;
			}

			if (os.Value != targetOperatingSystem || (architectures & deviceArchitectures) == 0) {
				// the TargetiOSDevice property conflicts with the build configuration (*.user file?), do not build this project for a specific device
				DeviceSpecificIntermediateOutputPath = IntermediateOutputPath;
				DeviceSpecificOutputPath = OutputPath;
				TargetArchitectures = Architectures;
				TargetDeviceOSVersion = string.Empty;
				TargetDeviceModel = string.Empty;

				return !Log.HasLoggedErrors;
			}

			for (int bit = 0; bit < 32; bit++) {
				var architecture = (TargetArchitecture) (1 << bit);

				if ((architectures & architecture) == 0)
					continue;

				if ((deviceArchitectures & architecture) != 0)
					target = architecture;
			}

			TargetArchitectures = target.ToString ();

			if (!device.TryGetValue ("model", out value)) {
				Log.LogError (MSBStrings.E0063);
				return false;
			}

			TargetDeviceModel = value.Value;

			if (!device.TryGetValue ("os-version", out value)) {
				Log.LogError (MSBStrings.E0064);
				return false;
			}

			TargetDeviceOSVersion = value.Value;

			// Note: we replace ',' with '.' because the ',' breaks the Mono AOT compiler which tries to treat arguments with ','s in them as options.
			var dirName = TargetDeviceModel.ToLowerInvariant ().Replace (",", ".") + "-" + TargetDeviceOSVersion;

			DeviceSpecificIntermediateOutputPath = Path.Combine (IntermediateOutputPath, "device-builds", dirName) + "/";
			DeviceSpecificOutputPath = Path.Combine (OutputPath, "device-builds", dirName) + "/";

			return !Log.HasLoggedErrors;
		}
	}
}
