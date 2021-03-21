using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace Atmosphere.Commands {
  using EnvironmentDict = Hashtable;

  [Cmdlet(VerbsCommon.Push, "Environment")]
  public class PushEnvironment : PSCmdlet {
    [Parameter(Position = 0)]
    public EnvironmentDict State { get; set; }

    sealed protected override void BeginProcessing () {
      if (State.IsNullOrEmpty()) { State = Environment.Current.Variables.IntoHashtable(); }
    }

    sealed protected override void ProcessRecord () {
      EnvironmentStack.Push(State.IntoDictionary());
    }
  }

  [Cmdlet(VerbsCommon.Pop, "Environment")]
  public class PopEnvironment : PSCmdlet {
    protected override void ProcessRecord () {
      foreach (var entry in EnvironmentStack.Pop()) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }
}
