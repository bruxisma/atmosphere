using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;

namespace Atmosphere.Commands {

  [Cmdlet(VerbsData.Import, "Environment")]
  public class ImportEnvironment : PSCmdlet {
    [Parameter(Position=0, Mandatory=true, ValueFromPipeline=true)]
    [ValidateNotNullOrEmpty()]
    public FileInfo Path { get; set; }

    [Parameter()]
    public SwitchParameter Push { get; set; } = true;

    Dictionary<string, string> Read () {
      using (FileStream fs = Path.OpenRead()) {
        return JsonSerializer
          .DeserializeAsync<Dictionary<string, string>>(fs)
          .GetAwaiter()
          .GetResult();
      }
    }

    protected sealed override void BeginProcessing() {
      if (Path.Exists) { return; }
      throw new ArgumentException("Path", $"'{Path}' does not exist");
    }

    protected override void ProcessRecord () {
      if (!Path.Exists) {
        WriteWarning($"'{Path}' does not exist. Skipping");
        return;
      }
      if (Push) { EnvironmentStack.Push(); }
      foreach (var entry in Read()) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }

  [Cmdlet(VerbsData.Import, "ShellScript")]
  public sealed class ImportShellScript : PSCmdlet {
    [Parameter(Position=0, Mandatory=true, ValueFromPipeline=true)]
    [ValidateNotNullOrEmpty()]
    public FileInfo Path { get; set; }

    // If overwriting, we *don't* push the environment first.
    [Parameter()]
    public SwitchParameter Overwrite { get; set; } = false;

    protected sealed override void BeginProcessing () {
      if (Path.Exists) { return; }
      throw new ArgumentException("Path", $"'{Path}' does not exist");
    }

    protected sealed override void ProcessRecord () {
      if (!Overwrite) { EnvironmentStack.Push(); }
    }
  }

}
