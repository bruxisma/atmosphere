using System.Management.Automation;
using System.Collections;

namespace Atmosphere.Commands {
  using EnvironmentDict = Hashtable;

  [Cmdlet(VerbsCommon.Push, "Environment")]
  public sealed class PushEnvironment : PSCmdlet {
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
  public sealed class PopEnvironment : PSCmdlet {
    protected override void ProcessRecord () {
      foreach (var entry in EnvironmentStack.Pop()) {
        Environment.Current[entry.Key] = entry.Value;
      }
    }
  }
}
