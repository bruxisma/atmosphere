using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace Atmosphere.Commands {

  public class ResultCommand : PSCmdlet {
    [Parameter(Position=0, Mandatory=true, ValueFromPipeline=true)]
    [ValidateNotNullOrEmpty()]
    public string Name { get; set; }
  }

  public class ModifyCommand : ResultCommand {
    [Parameter(Position=1, Mandatory=true)]
    [Alias("With")]
    public string Value { get; set; }

    sealed protected override void BeginProcessing() { Value = Value ?? ""; }
  }

  [OutputType(typeof(List<DirectoryInfo>))]
  public class GetPathCommand : PSCmdlet {
    protected GetPathCommand (string name) { this.name = name; }
    private string name;

    sealed protected override void ProcessRecord () {
      WriteObject(Environment.Current[name].IntoPathList());
    }
  }

  public class ModifyPathCommand : PSCmdlet {
    protected ModifyPathCommand (string name) { this.name = name; }
    protected string name;

    [Parameter(Position=0, Mandatory=true)]
    [Alias("With")]
    public string[] Value { get; set; }

    protected override void ProcessRecord () {
      Environment.Current[name] = Value
        .Select(path => new DirectoryInfo(path))
        .FromPathList();
    }
  }

  // Append by default, prepend otherwise. *never* override
  // This is also why we don't have Update-EnvironmentVariable anymore
  public class UpdatePathCommand : ModifyPathCommand {
    protected UpdatePathCommand (string name) : base(name) { }

    [Parameter()]
    public SwitchParameter Prepend { get; set; } = false;

    sealed protected override void ProcessRecord () {
      var previous = Environment.Current[name].IntoPathList();
      var newest = Value.Select(path => new DirectoryInfo(path));
      if (Prepend) { previous.InsertRange(0, newest); }
      else { previous.AddRange(newest); }
      Environment.Current[name] = previous.FromPathList();
    }
  }
}
