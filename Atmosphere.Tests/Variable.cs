using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System;
using Xunit;

using Atmosphere.Commands;

namespace Atmosphere.Tests {
  public class Variable {
    [Fact]
    public void GetEnvironmentVariable () {
      var session = InitialSessionState.CreateDefault();
      session.Commands.Add(new SessionStateCmdletEntry("Get-EnvironmentVariable", typeof(GetEnvironmentVariable), String.Empty));
      using (var runspace = RunspaceFactory.CreateRunspace(session)) {
        using (var ps = PowerShell.Create()) {
          ps.Runspace = runspace;
        }
      }
    }

    [Fact]
    public void SetEnvironmentVariable () {

    }
  }
}
