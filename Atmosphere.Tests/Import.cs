using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System;
using Xunit;

using Atmosphere.Commands;

namespace Atmosphere.Tests {
  public class Import {
    [Fact]
    public void ImportEnvironment() {
      var session = InitialSessionState.CreateDefault();
      session.Commands.Add(new SessionStateCmdletEntry("Import-Environment", typeof(ImportEnvironment), String.Empty));
      using (var runspace = RunspaceFactory.CreateRunspace(session)) {
        using (var ps = PowerShell.Create()) {
          ps.Runspace = runspace;
        }
      }
    }
  }
}
