using System.Management.Automation.Runspaces;
using System.Management.Automation;

using Microsoft.PowerShell.Commands;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System;

using Xunit.Abstractions;

namespace Atmosphere.Tests {
  public interface Test : IDisposable {
    public ITestOutputHelper Output { get => null; }
  }

  public class Session : IDisposable {

    internal Session (Test test) {
      this.Environment = new Dictionary<string, string>();
      this.Scripts = new List<string>();
      this.Shell = PowerShell.Create();

      this.state = InitialSessionState.Create();
      this.state.LanguageMode = PSLanguageMode.FullLanguage;
      var env = new SessionStateProviderEntry("Environment", typeof(EnvironmentProvider), null);
      this.state.Providers.Add(env);
      this.RegisterCommand(test);
    }

    public void Dispose () {
      this.runspace?.Dispose();
      this.Shell?.Dispose();
      foreach (DictionaryEntry entry in System.Environment.GetEnvironmentVariables()) {
        System.Environment.SetEnvironmentVariable(entry.Key as string, null);
      }
    }

    internal Collection<PSObject> Invoke () {
      foreach (var entry in this.Environment) {
        var variable = new SessionStateVariableEntry($"{entry.Key}", $"{entry.Value}", null);
        this.state.EnvironmentVariables.Add(variable);
      }
      this.runspace = RunspaceFactory.CreateRunspace(this.state);
      this.runspace.Open();
      this.Shell.Runspace = this.runspace;
      foreach (var script in this.Scripts) {
        this.Shell.AddScript(script);
      }
      return this.Shell.Invoke();
    }

    internal PowerShell AddParameter(string flag, object parameter) {
      return this.Shell.AddParameter(flag, parameter);
    }
    internal PowerShell AddParameter(string flag) {
      return this.Shell.AddParameter(flag);
    }
    internal PowerShell AddCommand(string command) { return this.Shell.AddCommand(command); }
    internal PowerShell AddStatement() { return this.Shell.AddStatement(); }
    internal PowerShell AddScript(string script) { return this.Shell.AddScript(script); }

    internal void RegisterCommand(Test test) {
      var type = Type.GetType($"Atmosphere.Commands.{test.GetType().Name},Atmosphere");
      var attribute = Attribute.GetCustomAttribute(type, typeof(CmdletAttribute)) as CmdletAttribute;
      if (attribute == null) {
        throw new ArgumentException($"{type.FullName} does not have a Cmdlet attribute");
      }
      var action = $"{attribute.VerbName}-{attribute.NounName}";
      var entry = new SessionStateCmdletEntry(action, type, null);
      this.state.Commands.Add(entry);
    }

    public Dictionary<string, string> Environment { get; private set; }
    public List<string> Scripts { get; private set; }
    public PowerShell Shell { get; private set; }
    public SessionStateProxy Proxy { get => this.Shell.Runspace.SessionStateProxy; }

    private InitialSessionState state;
    private Runspace runspace;
  }
}
