// arguments are: <platform> <outputPath>

using System.IO;
using System.Xml;

var expectedArgumentCount = 4;
if (args.Length != expectedArgumentCount) {
	Console.WriteLine ($"Need {expectedArgumentCount} arguments, got {args.Length}");
	return 1;
}

var idx = 0;
var platform = args [idx++];
var dotnetTfm = args [idx++];
var supportedApiVersions = args [idx++].Split (' ').Select (v => v.Replace (dotnetTfm + "-", "")).ToArray ();
var outputPath = args [idx++];
var plistPath = $"../builds/Versions-{platform}.plist.in";

var doc = new XmlDocument ();
doc.Load (plistPath);
var supportedTargetPlatformVersions = doc.SelectNodes ($"/plist/dict/key[text()='SupportedTargetPlatformVersions']/following-sibling::dict[1]/key[text()='{platform}']/following-sibling::array[1]/string")!.Cast<XmlNode> ().Select (v => v.InnerText).ToArray ();

var currentSupportedTPVs = supportedTargetPlatformVersions.Where (v => v.StartsWith (dotnetTfm + "-", StringComparison.Ordinal)).Select (v => v.Substring (dotnetTfm.Length + 1));
var minSdkVersionName = $"DOTNET_MIN_{platform.ToUpper ()}_SDK_VERSION";
var minSdkVersionString = File.ReadAllLines ("../Make.config").Single (v => v.StartsWith (minSdkVersionName + "=", StringComparison.Ordinal)).Substring (minSdkVersionName.Length + 1);
var minSdkVersion = Version.Parse (minSdkVersionString);

Console.WriteLine (string.Join (";", supportedApiVersions));

using (TextWriter writer = new StreamWriter (outputPath)) {
	writer.WriteLine ($"<!-- This file contains a generated list of the {platform} platform versions that are supported for this SDK -->");
	writer.WriteLine ($"<!-- Generation script: https://github.com/dotnet/macios/blob/main/scripts/generate-target-platforms/generate-target-platforms.cs -->");
	writer.WriteLine ("<Project>");
	writer.WriteLine ("\t<ItemGroup>");

	foreach (var version in supportedTargetPlatformVersions) {
		writer.Write ($"\t\t<{platform}SdkSupportedTargetPlatformVersion Include=\"{version}\" ");
		if (!supportedApiVersions.Contains (version))
			writer.Write ($"DefineConstantsOnly=\"true\" ");
		writer.WriteLine ("/>");
	}

	writer.WriteLine ("\t</ItemGroup>");
	writer.WriteLine ("\t<ItemGroup>");
	writer.WriteLine ($"\t\t<SdkSupportedTargetPlatformVersion Condition=\"'$(TargetPlatformIdentifier)' == '{platform}'\" Include=\"@({platform}SdkSupportedTargetPlatformVersion)\" />");
	writer.WriteLine ("\t</ItemGroup>");
	writer.WriteLine ("\t<PropertyGroup>");
	writer.WriteLine ($"\t\t<{platform}MinSupportedOSPlatformVersion>{minSdkVersionString}</{platform}MinSupportedOSPlatformVersion>");
	writer.WriteLine ($"\t\t<MinSupportedOSPlatformVersion>$({platform}MinSupportedOSPlatformVersion)</MinSupportedOSPlatformVersion>");
	writer.WriteLine ("\t</PropertyGroup>");
	writer.WriteLine ("</Project>");
}

return 0;
