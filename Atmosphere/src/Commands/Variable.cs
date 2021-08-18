using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.Generic;
using System.IO;
using System;

namespace Atmosphere.Commands {
  [Cmdlet(VerbsCommon.Get, "EnvironmentVariable")]
  [OutputType(typeof(string))]
  public sealed class GetEnvironmentVariable : ResultCommand {
    sealed protected override void ProcessRecord () {
      WriteObject(Environment.Current[Name]);
    }
  }

  [Cmdlet(VerbsCommon.Set, "EnvironmentVariable")]
  public sealed class SetEnvironmentVariable : ModifyCommand {
    sealed protected override void ProcessRecord () {
      Environment.Current[Name] = Value;
    }
  }
}
