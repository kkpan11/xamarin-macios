using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.Localization.MSBuild;

using Xamarin.MacDev;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public abstract class XcodeToolTaskBase : XamarinTask, IHasProjectDir, IHasResourcePrefix {
		string? toolExe;

		#region Inputs

		[Required]
		public string IntermediateOutputPath { get; set; } = string.Empty;

		[Required]
		public string ProjectDir { get; set; } = string.Empty;

		[Required]
		public string ResourcePrefix { get; set; } = string.Empty;

		[Required]
		public string SdkBinPath { get; set; } = string.Empty;

		[Required]
		public string SdkDevPath { get; set; } = string.Empty;

		[Required]
		public string SdkUsrPath { get; set; } = string.Empty;

		public string ToolExe {
			get { return toolExe ?? ToolName; }
			set { toolExe = value; }
		}

		public string ToolPath { get; set; } = string.Empty;

		#endregion

		#region Outputs

		[Output]
		public ITaskItem [] BundleResources { get; set; } = Array.Empty<ITaskItem> ();

		#endregion

		protected abstract string DefaultBinDir {
			get;
		}

		protected string DeveloperRootBinDir {
			get { return Path.Combine (SdkDevPath, "usr", "bin"); }
		}

		protected string DevicePlatformBinDir {
			get { return Path.Combine (SdkDevPath, "Platforms", "iPhoneOS.platform", "Developer", "usr", "bin"); }
		}

		protected string SimulatorPlatformBinDir {
			get { return Path.Combine (SdkDevPath, "Platforms", "iPhoneSimulator.platform", "Developer", "usr", "bin"); }
		}

		protected abstract string ToolName { get; }

		protected abstract IEnumerable<ITaskItem> EnumerateInputs ();

		protected abstract void AppendCommandLineArguments (IDictionary<string, string?> environment, List<string> args, ITaskItem input, ITaskItem output);

		protected virtual string GetBundleRelativeOutputPath (ITaskItem input)
		{
			return BundleResource.GetLogicalName (this, input);
		}

		protected virtual IEnumerable<ITaskItem> GetCompiledBundleResources (ITaskItem input, ITaskItem output)
		{
			yield return output;
		}

		protected virtual bool NeedsBuilding (ITaskItem input, ITaskItem output)
		{
			var dest = output.GetMetadata ("FullPath");
			var src = input.GetMetadata ("FullPath");

			return !File.Exists (dest) || File.GetLastWriteTimeUtc (src) > File.GetLastWriteTimeUtc (dest);
		}

		string GetFullPathToTool ()
		{
			if (!string.IsNullOrEmpty (ToolPath))
				return Path.Combine (ToolPath, ToolExe);

			var path = Path.Combine (DefaultBinDir, ToolExe);

			return File.Exists (path) ? path : ToolExe;
		}

		int ExecuteTool (ITaskItem input, ITaskItem output)
		{
			var environment = new Dictionary<string, string?> ();
			var args = new List<string> ();

			environment.Add ("PATH", SdkBinPath);
			environment.Add ("XCODE_DEVELOPER_USR_PATH", SdkUsrPath);

			AppendCommandLineArguments (environment, args, input, output);

			var rv = ExecuteAsync (GetFullPathToTool (), args, environment: environment).Result;
			return rv.ExitCode;
		}

		public override bool Execute ()
		{
			var intermediate = Path.Combine (IntermediateOutputPath, ToolName);
			var bundleResources = new List<ITaskItem> ();

			foreach (var input in EnumerateInputs ()) {
				var relative = GetBundleRelativeOutputPath (input);
				ITaskItem output;

				if (!string.IsNullOrEmpty (relative)) {
					if (BundleResource.IsIllegalName (relative, out var illegal)) {
						Log.LogError (null, null, null, input.ItemSpec, 0, 0, 0, 0, MSBStrings.E0102, illegal);
						continue;
					}

					var rpath = Path.Combine (intermediate, relative);

					output = new TaskItem (rpath);
				} else {
					output = new TaskItem (intermediate);
				}

				output.SetMetadata ("LogicalName", relative);

				if (NeedsBuilding (input, output)) {
					Directory.CreateDirectory (Path.GetDirectoryName (output.ItemSpec));

					if (ExecuteTool (input, output) == -1)
						return false;
				}

				bundleResources.AddRange (GetCompiledBundleResources (input, output));
			}

			BundleResources = bundleResources.ToArray ();

			return !Log.HasLoggedErrors;
		}
	}
}
