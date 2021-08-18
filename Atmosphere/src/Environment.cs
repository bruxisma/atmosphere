using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

namespace Atmosphere {
  internal sealed class Environment {
    internal static readonly Environment Current = new Environment();

    internal Dictionary<string, string> Variables {
      // Can't do a linq .Cast<KeyValuePair<string, string>>() because the cast
      // fails for some reason :/
      get {
        var dict = new Dictionary<string, string>();
        var variables = System.Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in variables) {
          dict.Add(entry.Key.ToString(), entry.Value.ToString());
        }
        return dict;
      }
    }

    internal string this[string name] {
      get { return System.Environment.GetEnvironmentVariable(name) ?? ""; }
      set { System.Environment.SetEnvironmentVariable(name, value); }
    }
  }
}
