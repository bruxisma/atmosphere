using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;

namespace Atmosphere.Commands {

  [Cmdlet(VerbsData.Import, "Environment")]
  public sealed class ImportEnvironment : ImportCommand {
    // Deserialize a JSON file to a dictionary.
    // This needs to be expanded to allow reading a PSD1, XML, and other file
    // types as desired.
    Dictionary<string, string> Read () {
      using (FileStream fs = Path.OpenRead()) {
        return JsonSerializer
          .DeserializeAsync<Dictionary<string, string>>(fs)
          .GetAwaiter()
          .GetResult();
      }
    }

    protected override void Import () {
      foreach (var entry in Read()) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }

  [Cmdlet(VerbsData.Import, "ShellScript")]
  public sealed class ImportShellScript : ImportCommand {
    protected sealed override void Import () {
    }
  }

}
