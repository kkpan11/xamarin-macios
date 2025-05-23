using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Threading = System.Threading.Tasks;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	public abstract class XamarinBuildTask : XamarinTask, ITaskCallback, ICancelableTask {
		public bool KeepTemporaryOutput { get; set; }

		[Required]
		public string RuntimeIdentifier { get; set; } = "";

		/// <summary>
		/// Runs the target passed in computeValueTarget and returns its result.
		/// The target must write the result into a text file using $(OutputFilePath) as path.
		/// </summary>
		/// <returns></returns>
		protected string ComputeValueUsingTarget (string computeValueTarget, string targetName)
		{
			var projectDirectory = Path.GetTempFileName ();
			File.Delete (projectDirectory);
			Directory.CreateDirectory (projectDirectory);
			var projectPath = Path.Combine (projectDirectory, targetName + ".csproj");

			var csproj = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project Sdk=""Microsoft.NET.Sdk"">
	<PropertyGroup>
		<TargetFramework>net{TargetFramework.Version}-{PlatformName}</TargetFramework>
	</PropertyGroup>
	{computeValueTarget}
</Project>
";
			File.WriteAllText (projectPath, csproj);

			var dotnetPath = this.GetDotNetPath ();
			var environment = new Dictionary<string, string?> ();
			var customHome = Environment.GetEnvironmentVariable ("DOTNET_CUSTOM_HOME");

			if (!string.IsNullOrEmpty (customHome)) {
				environment ["HOME"] = customHome;
			}
			// Disable a few things that we don't care about
			environment ["DOTNET_NOLOGO"] = "1";
			environment ["DOTNET_CLI_WORKLOAD_UPDATE_NOTIFY_DISABLE"] = "1";

			try {
				ExecuteRestoreAsync (dotnetPath, projectPath, targetName, environment).Wait ();

				// Don't try to run 'dotnet build' if restore failed.
				if (Log.HasLoggedErrors)
					return string.Empty;

				return ExecuteBuildAsync (dotnetPath, projectPath, targetName, environment).Result;
			} finally {
				if (KeepTemporaryOutput) {
					Log.LogMessage (MessageImportance.Normal, $"Temporary project directory for the {targetName} task: {projectDirectory}");
				} else {
					Directory.Delete (projectDirectory, true);
				}
			}
		}

		void AddRestoreConfigFile (List<string> arguments, string dotnetPath)
		{
			var dotnetDir = Path.GetDirectoryName (dotnetPath);
			var configFile = Path.Combine (dotnetDir, "NuGet.config");

			if (File.Exists (configFile))
				arguments.Add ("/p:RestoreConfigFile=" + configFile);
		}

		async Threading.Task ExecuteRestoreAsync (string dotnetPath, string projectPath, string targetName, Dictionary<string, string?> environment)
		{
			var projectDirectory = Path.GetDirectoryName (projectPath);
			var binlog = Path.Combine (projectDirectory, targetName + ".binlog");
			var arguments = new List<string> ();

			arguments.Add ("restore");
			AddRestoreConfigFile (arguments, dotnetPath);

			arguments.Add ("/bl:" + binlog);
			arguments.Add (projectPath);

			try {
				await ExecuteAsync (dotnetPath, arguments, environment: environment);
			} finally {
				if (KeepTemporaryOutput) {
					Log.LogMessage (MessageImportance.Normal, $"Temporary restore log for the {targetName} task: {binlog}");
				} else {
					File.Delete (binlog);
				}
			}
		}

		async Threading.Task<string> ExecuteBuildAsync (string dotnetPath, string projectPath, string targetName, Dictionary<string, string?> environment)
		{
			var projectDirectory = Path.GetDirectoryName (projectPath);
			var outputFile = Path.Combine (projectDirectory, "Output.txt");
			var binlog = Path.Combine (projectDirectory, targetName + ".binlog");
			var arguments = new List<string> ();

			arguments.Add ("build");
			AddRestoreConfigFile (arguments, dotnetPath);
			arguments.Add ("/p:OutputFilePath=" + outputFile);
			arguments.Add ("/p:RuntimeIdentifier=" + RuntimeIdentifier);
			arguments.Add ($"/t:{targetName}");
			arguments.Add ("/bl:" + binlog);
			arguments.Add (projectPath);

			await ExecuteAsync (dotnetPath, arguments, environment: environment);

			if (File.Exists (outputFile))
				return File.ReadAllText (outputFile).Trim ();
			return string.Empty;
		}

		public override sealed bool Execute ()
		{
			if (this.ShouldExecuteRemotely (SessionId))
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;
			return ExecuteLocally ();
		}

		protected abstract bool ExecuteLocally ();

		public virtual IEnumerable<ITaskItem> GetAdditionalItemsToBeCopied () => Enumerable.Empty<ITaskItem> ();

		public virtual bool ShouldCopyToBuildServer (ITaskItem item) => false;

		public virtual bool ShouldCreateOutputFile (ITaskItem item) => false;

		public virtual void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}
}

