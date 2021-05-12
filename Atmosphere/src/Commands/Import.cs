using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.IO;
using System;

namespace Atmosphere.Commands {

  [Cmdlet(VerbsData.Import, "Environment")]
  public sealed class ImportEnvironment : ImportCommand {
    static Regex extract = new Regex(@"\b(?<key>[^=]+)=(?<variable>.*)\b", RegexOptions.Compiled);

    /// <summary>
    /// JSON files *must* contain an object that can be converted into a
    /// Dictionary<string, string>.
    /// </summary>
    public static Dictionary<string, string> JSON (FileInfo path) {
      using var fs = path.OpenRead();
      return JsonSerializer
        .DeserializeAsync<Dictionary<string, string>>(fs)
        .GetAwaiter()
        .GetResult();
    }

    ///<summary>
    /// .env (spoken as "dotenv") files have several varying formats in the wild.
    /// We do not support all of them, but instead support the most basic syntax,
    /// specifically that of a `KEY=VAL` syntax, with no pre-existing variable
    /// expansion *nor* lines starting with `export` (though we might in the
    /// future)
    /// Some additional extensions we support are things like
    /// ```
    ///  # A commented line where the comment is prefixed with whitespace
    ///    
    /// # A blank line with a bunch of whitespace
    /// ```
    ///</summary>
    public static Dictionary<string, string> DotEnv (FileInfo path) {
      var entries = new Dictionary<string, string>();
      foreach (var line in File.ReadAllLines(path.ToString())) {
        var text = line.TrimStart();
        /* We skip empty lines or lines starting with a '#' comment */
        if (text.StartsWith('#') || text.IsEmpty()) { continue; }
        var match = extract.Match(text);
        entries.Add(match.Get("key"), match.Get("value"));
      }
      return entries;
    }

    /// <summary>
    /// Most of this code is effectively line for line the same error as that
    /// found in `Import-PowerShellDataFile`, however the results are combined
    /// into a single dictionary and then returned.
    /// </summary>
    public static Dictionary<string, string> DataFile (FileInfo path) {
      ParseError[] errors;
      Token[] tokens;
      var ast = Parser.ParseFile(path.ToString(), out tokens, out errors);
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
      foreach (var entry in JSON(Path)) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }
}
