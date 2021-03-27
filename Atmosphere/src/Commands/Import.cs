using System.Management.Automation.Language;
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

    Dictionary<string, string> DataFile() {
      ParseError[] errors;
      Token[] tokens;
      var ast = Parser.ParseFile(Path.ToString(), out tokens, out errors);
      if (errors.Length > 0) {
        /* Raise/Write an error record here */
        throw new InvalidDataException();
      }
      var data = ast.Find(a => a is HashtableAst, false);
      var pairs = (data.SafeGetValue() as HashtableAst).KeyValuePairs;
      var dict = new Dictionary<string, string>(pairs.Count);
      foreach ((var key, var value) in pairs) {
        dict[key.SafeGetValue().ToString()] = value.SafeGetValue().ToString();
      }
      return dict;
    }

    protected override void Import () {
      foreach (var entry in Read()) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }
}
