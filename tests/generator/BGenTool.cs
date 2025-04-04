#pragma warning disable 0649 // Field 'X' is never assigned to, and will always have its default value null

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Utils;

#nullable enable

namespace Xamarin.Tests {
	class BGenTool : Tool {
		public const string None = "None";
		AssemblyDefinition? assembly;

		public Profile Profile;
		public bool InProcess = true; // if executed using an in-process bgen. Ignored if using the classic bgen (for XM/Classic), in which case we'll always use the out-of-process bgen.
		public bool ProcessEnums;

		public List<string> ApiDefinitions = new List<string> ();
		public List<string> Sources = new List<string> ();
		public List<string> ExtraSources = new List<string> ();
		public List<string> References = new List<string> ();
		public List<string>? CompileCommand = null;

		// If BaseLibrary and AttributeLibrary are null, we calculate a default value
		public string? BaseLibrary;
		public string? AttributeLibrary;
		public bool ReferenceBclByDefault = true;
		public string? CompiledApiDefinitionAssembly = null;
		public string []? Defines;
		public string? TmpDirectory;
		public string? ResponseFile;
		public string? WarnAsError; // Set to empty string to pass /warnaserror, set to non-empty string to pass /warnaserror:<nonemptystring>
		public string? NoWarn; // Set to empty string to pass /nowarn, set to non-empty string to pass /nowarn:<nonemptystring>
		public string? Out;
		public int Verbosity = 1;

		protected override string ToolPath { get => throw new InvalidOperationException (); }
		protected override string MessagePrefix { get { return "BI"; } }
		protected override string MessageToolName { get { return "bgen"; } }

		public BGenTool ()
		{
			EnvironmentVariables = new Dictionary<string, string> ();
		}

		public void AddTestApiDefinition (string filename)
		{
			ApiDefinitions.Add (Path.Combine (Configuration.SourceRoot, "tests", "generator", filename));
		}

		public void AddExtraSourcesRelativeToGeneratorDirectory (string pathRelativeToGeneratorDirectory)
		{
			ExtraSources.Add (GetFullPathRelativeToGeneratorDirectory (pathRelativeToGeneratorDirectory));
		}

		public string GetFullPathRelativeToGeneratorDirectory (string pathRelativeToGeneratorDirectory)
		{
			return Path.Combine (Configuration.SourceRoot, "tests", "generator", pathRelativeToGeneratorDirectory);
		}

		public AssemblyDefinition ApiAssembly {
			get {
				return LoadAssembly ();
			}
		}

		public static string GetTargetFramework (Profile profile)
		{
			switch (profile) {
			case Profile.iOS:
				return TargetFramework.DotNet_iOS_String;
			case Profile.tvOS:
				return TargetFramework.DotNet_tvOS_String;
			case Profile.MacCatalyst:
				return TargetFramework.DotNet_MacCatalyst_String;
			case Profile.macOSMobile:
				return TargetFramework.DotNet_macOS_String;
			default:
				throw new NotImplementedException ($"Profile: {profile}");
			}
		}

		string [] BuildArgumentArray ()
		{
			var sb = new List<string> ();
			var targetFramework = (string?) null;

			if (Profile != Profile.None)
				targetFramework = GetTargetFramework (Profile);

			if (CompileCommand is null) {
				if (!StringUtils.TryParseArguments (Configuration.DotNetCscCommand, out var args, out var ex))
					throw new InvalidOperationException ($"Unable to parse the .NET csc command '{Configuration.DotNetCscCommand}': {ex.Message}");

				CompileCommand = new List<string> (args);
			}

			if (CompileCommand.Count > 0) {
				sb.Add ($"--compile-command");
				sb.Add (string.Join (" ", StringUtils.QuoteForProcess (CompileCommand.ToArray ())));
			}

			if (CompiledApiDefinitionAssembly is not null) {
				sb.Add ($"--compiled-api-definition-assembly");
				sb.Add (CompiledApiDefinitionAssembly);
			}

			TargetFramework? tf = null;
			if (targetFramework is not null)
				tf = TargetFramework.Parse (targetFramework);

			if (BaseLibrary is null) {
				if (tf.HasValue)
					sb.Add ($"--baselib={Configuration.GetBaseLibrary (tf.Value)}");
			} else if (BaseLibrary != None) {
				sb.Add ($"--baselib={BaseLibrary}");
			}

			if (AttributeLibrary is null) {
				if (tf.HasValue)
					sb.Add ($"--attributelib={Configuration.GetBindingAttributePath (tf.Value)}");
			} else if (AttributeLibrary != None) {
				sb.Add ($"--attributelib={AttributeLibrary}");
			}

			if (!string.IsNullOrEmpty (targetFramework))
				sb.Add ($"--target-framework={targetFramework}");

			foreach (var ad in ApiDefinitions)
				sb.Add ($"--api={ad}");

			foreach (var s in Sources)
				sb.Add ($"-s={s}");

			foreach (var x in ExtraSources)
				sb.Add ($"-x={x}");

			if (ReferenceBclByDefault) {
				if (tf is null) {
					// do nothing
				} else {
					References.AddRange (Directory.GetFiles (Configuration.DotNetBclDir, "*.dll"));
				}
			}

			foreach (var r in References)
				sb.Add ($"-r={r}");

			if (!string.IsNullOrEmpty (TmpDirectory))
				sb.Add ($"--tmpdir={TmpDirectory}");

			if (!string.IsNullOrEmpty (ResponseFile))
				sb.Add ($"@{ResponseFile}");

			if (!string.IsNullOrEmpty (Out))
				sb.Add ($"--out={Out}");

			if (ProcessEnums)
				sb.Add ("--process-enums");

			if (Defines is not null) {
				foreach (var d in Defines)
					sb.Add ($"-d={d}");
			}

			if (WarnAsError is not null) {
				var arg = "--warnaserror";
				if (WarnAsError.Length > 0)
					arg += ":" + WarnAsError;
				sb.Add (arg);
			}

			NoWarn = GetPreviewNoWarn (NoWarn);

			if (NoWarn is not null) {
				var arg = "--nowarn";
				if (NoWarn.Length > 0)
					arg += ":" + NoWarn;
				sb.Add (arg);
			}
			if (Verbosity != 0)
				sb.Add ("-" + new string (Verbosity > 0 ? 'v' : 'q', Math.Abs (Verbosity)));
			return sb.ToArray ();
		}

		public static void AddPreviewNoWarn (IList<string> argumentList)
		{
			var nw = GetPreviewNoWarn (null);
			if (!string.IsNullOrEmpty (nw))
				argumentList.Add ($"-nowarn:{nw}");
		}

		[return: NotNullIfNotNull (nameof (existingNowarn))]
		public static string? GetPreviewNoWarn (string? existingNowarn)
		{
			if (Configuration.XcodeIsStable)
				return existingNowarn;

			var previewNoWarn = $"XCODE_{Configuration.XcodeVersion.Major}_{Configuration.XcodeVersion.Minor}_PREVIEW";
			if (string.IsNullOrEmpty (existingNowarn)) {
				return previewNoWarn;
			} else {
				return $"{existingNowarn},{previewNoWarn}";
			}
		}

		public void AssertExecute (string message)
		{
			Assert.AreEqual (0, Execute (), message);
		}

		public void AssertExecuteError (string message)
		{
			Assert.AreNotEqual (0, Execute (), message);
		}

		int Execute ()
		{
			var arguments = BuildArgumentArray ();
			var in_process = InProcess;
			if (in_process) {
				int rv;
				var previous_environment = new Dictionary<string, string?> ();
				foreach (var kvp in EnvironmentVariables) {
					previous_environment [kvp.Key] = Environment.GetEnvironmentVariable (kvp.Key);
					Environment.SetEnvironmentVariable (kvp.Key, kvp.Value);
				}
				ThreadStaticTextWriter.ReplaceConsole (Output);
				try {
					rv = BindingTouch.Main (arguments);
				} finally {
					ThreadStaticTextWriter.RestoreConsole ();
					foreach (var kvp in previous_environment) {
						Environment.SetEnvironmentVariable (kvp.Key, kvp.Value);
					}
				}
				Console.WriteLine (Output);
				ParseMessages ();
				return rv;
			}
			return Execute (arguments, always_show_output: true);
		}

		public void AssertApiCallsMethod (string caller_namespace, string caller_type, string caller_method, string @called_method, string message)
		{
			var type = ApiAssembly.MainModule.GetType (caller_namespace, caller_type);
			var method = type.Methods.First ((v) => v.Name == caller_method);

			AssertApiCallsMethod (method, called_method, message);
		}

		public void AssertApiCallsMethod (MethodReference method, string called_method, string message)
		{
			var instructions = method.Resolve ().Body.Instructions;
			foreach (var ins in instructions) {
				if (ins.OpCode.FlowControl != FlowControl.Call)
					continue;
				var mr = ins.Operand as MethodReference;
				if (mr is null)
					continue;
				if (mr.Name == called_method)
					return;
			}

			Assert.Fail ($"Could not find any instructions calling {called_method} in {method.FullName}: {message}\n\t{string.Join ("\n\t", instructions)}");
		}

		public void AssertApiLoadsField (string caller_type, string caller_method, string declaring_type, string field, string message)
		{
			var type = ApiAssembly.MainModule.GetTypes ().First ((v) => v.FullName == caller_type);
			var method = type.Methods.First ((v) => v.Name == caller_method);

			AssertApiLoadsField (method, declaring_type, field, message);
		}

		public void AssertApiLoadsField (MethodReference method, string declaring_type, string field, string message)
		{
			var instructions = method.Resolve ().Body.Instructions;
			foreach (var ins in instructions) {
				if (ins.OpCode.Code != Code.Ldsfld && ins.OpCode.Code != Code.Ldfld)
					continue;
				var fr = ins.Operand as FieldReference;
				if (fr is null)
					continue;
				if (fr.DeclaringType.FullName != declaring_type)
					continue;
				if (fr.Name == field)
					return;
			}

			Assert.Fail ($"Could not find any instructions loading the field {declaring_type}.{field} in {method.FullName}: {message}\n\t{string.Join ("\n\t", instructions)}");
		}

		public void AssertPublicTypeCount (int count, string? message = null)
		{
			var assembly = LoadAssembly ();

			var actual = assembly.MainModule.Types.Where ((v) => v.IsPublic || v.IsNestedPublic);
			if (actual.Count () != count)
				Assert.Fail ($"Expected {count} public type(s), found {actual} public type(s). {message}\n\t{string.Join ("\n\t", actual.Select ((v) => v.FullName).ToArray ())}");
		}

		public void AssertPublicMethodCount (string typename, int count, string? message = null)
		{
			var assembly = LoadAssembly ();

			var t = assembly.MainModule.Types.First ((v) => v.FullName == typename);
			var actual = t.Methods.Where ((v) => {
				if (v.IsPrivate || v.IsAssembly || v.IsFamilyAndAssembly)
					return false;
				return true;
			});
			if (actual.Count () != count) {
				Assert.Fail ($"Expected {count} publicly accessible method(s) in {typename}, found {actual.Count ()} publicly accessible method(s): {message}\n\t{string.Join ("\n\t", actual.Select (v => v.FullName).OrderBy (v => v))}");
			}
		}

		public void AssertType (string fullname, TypeAttributes? attributes = null, string? message = null)
		{
			var assembly = LoadAssembly ();

			var allTypes = assembly.MainModule.GetTypes ().ToArray ();
			var t = allTypes.FirstOrDefault ((v) => v.FullName == fullname);
			if (t is null) {
				Assert.Fail ($"No type named '{fullname}' in the generated assembly. {message}\nList of types:\n\t{string.Join ("\n\t", allTypes.Select ((v) => v.FullName))}");
				return;
			}
			if (attributes is not null)
				Assert.AreEqual (attributes.Value, t.Attributes, $"Incorrect attributes for type {fullname}.");
		}

		public void AssertMethod (string typename, string method, params string [] parameterTypes)
		{
			AssertMethod (typename, method, null, null, parameterTypes);
		}

		public void AssertMethod (string typename, string method, string? returnType = null, params string [] parameterTypes)
		{
			AssertMethod (typename, method, null, returnType, parameterTypes);
		}

		public void AssertMethod (string typename, string method, MethodAttributes? attributes = null, string? returnType = null, params string [] parameterTypes)
		{
			var m = FindMethod (typename, method, returnType, parameterTypes);
			if (m is null) {
				Assert.Fail ($"No method '{method}' with signature '{string.Join ("', '", parameterTypes)}' on the type '{typename}' was found.");
				return;
			}
			if (attributes.HasValue)
				Assert.AreEqual (attributes.Value, m.Attributes, "Attributes for {0}", m.FullName);
		}

		public void AssertNoMethod (string typename, string method, string? returnType = null, params string [] parameterTypes)
		{
			var m = FindMethod (typename, method, returnType, parameterTypes);
			if (m is not null)
				Assert.Fail ($"Unexpectedly found method '{method}' with signature '{string.Join ("', '", parameterTypes)}' on the type '{typename}'.");
		}

		MethodDefinition? FindMethod (string typename, string method, string? returnType, params string [] parameterTypes)
		{
			var assembly = LoadAssembly ();
			var t = assembly.MainModule.Types.FirstOrDefault ((v) => v.FullName == typename);
			if (t is null)
				return null;

			var m = t.Methods.FirstOrDefault ((v) => {
				if (v.Name != method)
					return false;
				if (v.Parameters.Count != parameterTypes.Length)
					return false;
				for (int i = 0; i < v.Parameters.Count; i++)
					if (v.Parameters [i].ParameterType.FullName != parameterTypes [i])
						return false;
				return true;
			});
			return m;
		}

		AssemblyDefinition LoadAssembly ()
		{
			if (assembly is null) {
				var parameters = new ReaderParameters ();
				var resolver = new DefaultAssemblyResolver ();
				var searchdir = Path.GetDirectoryName (Configuration.GetBaseLibrary (Profile.AsPlatform ()));
				resolver.AddSearchDirectory (searchdir);
				parameters.AssemblyResolver = resolver;
				var tmpDirectory = EnsureTempDir ();
				assembly = AssemblyDefinition.ReadAssembly (AssemblyPath, parameters);
			}
			return assembly;
		}

		public string AssemblyPath {
			get {
				var tmpDirectory = EnsureTempDir ();
				return Out ?? (Path.Combine (tmpDirectory, Path.GetFileNameWithoutExtension (ApiDefinitions [0]).Replace ('-', '_') + ".dll"));
			}
		}

		public string XmlDocumentation {
			get {
				return Path.ChangeExtension (AssemblyPath, ".xml");
			}
		}

		string EnsureTempDir ()
		{
			if (TmpDirectory is null)
				TmpDirectory = Cache.CreateTemporaryDirectory ();
			return TmpDirectory;
		}

		public void CreateTemporaryBinding (params string [] api_definition)
		{
			var tmpDirectory = EnsureTempDir ();
			for (int i = 0; i < api_definition.Length; i++) {
				var api = Path.Combine (tmpDirectory, $"api{i}.cs");
				File.WriteAllText (api, api_definition [i]);
				ApiDefinitions.Add (api);
			}
			WorkingDirectory = TmpDirectory;
			Out = Path.Combine (tmpDirectory, "api0.dll");
		}

		public static string [] GetDefaultDefines (Profile profile)
		{
			switch (profile) {
			case Profile.macOSMobile:
				return new string [] { "MONOMAC" };
			case Profile.iOS:
				return new string [] { "IOS" };
			case Profile.MacCatalyst:
				return new string [] { "MACCATALYST" };
			case Profile.tvOS:
				return new string [] { "TVOS" };
			default:
				throw new NotImplementedException (profile.ToString ());
			}
		}
	}

	// This class will replace stdout/stderr with its own thread-static storage for stdout/stderr.
	// This means we're capturing stdout/stderr per thread.
	class ThreadStaticTextWriter : TextWriter {
		[ThreadStatic]
		static TextWriter? current_writer;

		static ThreadStaticTextWriter instance = new ThreadStaticTextWriter ();
		static object lock_obj = new object ();
		static int counter;

		static TextWriter? original_stdout;
		static TextWriter? original_stderr;

		public static void ReplaceConsole (StringBuilder sb)
		{
			lock (lock_obj) {
				if (counter == 0) {
					original_stdout = Console.Out;
					original_stderr = Console.Error;
					Console.SetOut (instance);
					Console.SetError (instance);
				}
				counter++;
				current_writer = new StringWriter (sb);
			}
		}

		public static void RestoreConsole ()
		{
			lock (lock_obj) {
				current_writer?.Dispose ();
				current_writer = null;
				counter--;
				if (counter == 0) {
					Console.SetOut (original_stdout!);
					Console.SetError (original_stderr!);
					original_stdout = null;
					original_stderr = null;
				}
			}
		}

		ThreadStaticTextWriter ()
		{
		}

		public TextWriter CurrentWriter {
			get {
				lock (lock_obj) {
					if (current_writer is null)
						return original_stdout ?? Console.Out;
					return current_writer;
				}
			}
		}

		public override Encoding Encoding => Encoding.UTF8;

		public override void WriteLine ()
		{
			lock (lock_obj)
				CurrentWriter.WriteLine ();
		}

		public override void Write (char value)
		{
			lock (lock_obj)
				CurrentWriter.Write (value);
		}

		public override void Write (string? value)
		{
			lock (lock_obj)
				CurrentWriter.Write (value);
		}

		public override void Write (char []? buffer)
		{
			lock (lock_obj)
				CurrentWriter.Write (buffer);
		}

		public override void WriteLine (string? value)
		{
			lock (lock_obj)
				CurrentWriter.WriteLine (value);
		}
	}
}
