using System.Management.Automation.Runspaces;
using System.Management.Automation;

using System.Collections.ObjectModel;
using System.Reflection;
using System;

namespace Atmosphere.Tests {
  public class Test : IDisposable {
     protected Test () {
      this.state = InitialSessionState.CreateDefault();
      this.shell = PowerShell.Create();
      var type = Type.GetType($"Atmosphere.Commands.{this.GetType().Name}, Atmosphere");
      this.Register(type);
    }

    public void Dispose () {
      this.runspace?.Dispose();
      this.shell.Dispose();
    }

    private void Register (Type type) {
      var attribute = (CmdletAttribute)Attribute.GetCustomAttribute(type, typeof(CmdletAttribute));
      if (attribute == null) {
        throw new ArgumentException($"${type.Name} does not have a Cmdlet attribute");
      }
      var name = $"{attribute.NounName}-{attribute.VerbName}";
      var entry = new SessionStateCmdletEntry(name, type, null);
      this.state.Commands.Add(entry);
    }

    public Collection<PSObject> Invoke () {
      this.runspace = RunspaceFactory.CreateRunspace(this.state);
      this.shell.Runspace = this.runspace;
      return this.shell.Invoke();
    }

    InitialSessionState state;
    Runspace runspace;
    PowerShell shell;
  }
}
