using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.Generic;
using System.IO;
using System;

namespace Atmosphere.Commands {

#region Get Cmdlets
  [Cmdlet(VerbsCommon.Get, "EnvironmentPath")]
  [OutputType(typeof(List<DirectoryInfo>))]
  public class GetEnvironmentPath : ResultCommand {
    sealed protected override void ProcessRecord() {
      WriteObject(Environment.Current[Name].IntoPathList(), true);
    }
  }

  [Cmdlet(VerbsCommon.Get, "LDLibraryPath")]
  public sealed class GetLDLibraryPath : GetPathCommand {
    public GetLDLibraryPath() : base("LD_LIBRARY_PATH") { }
  }

  [Cmdlet(VerbsCommon.Get, "PkgConfigPath")]
  public sealed class GetPkgConfigPath : GetPathCommand {
    public GetPkgConfigPath () : base("PKG_CONFIG_PATH") { }
  }

  [Cmdlet(VerbsCommon.Get, "PSModulePath")]
  public sealed class GetPSModulePath : GetPathCommand {
    public GetPSModulePath () : base("PSModulePath") { }
  }

  [Cmdlet(VerbsCommon.Get, "PythonPath")]
  public sealed class GetPythonPath : GetPathCommand {
    public GetPythonPath () : base("PYTHON_PATH") { }
  }

  [Cmdlet(VerbsCommon.Get, "SystemPath")]
  public sealed class GetSystemPath : GetPathCommand {
    public GetSystemPath () : base("PATH") { }
  }
#endregion

#region Update Cmdlets
  [Cmdlet(VerbsData.Update, "LDLibraryPath")]
  public sealed class UpdateLDLibraryPath : UpdatePathCommand {
    public UpdateLDLibraryPath() : base("LD_LIBRARY_PATH") { }
  }

  [Cmdlet(VerbsData.Update, "PkgConfigPath")]
  public sealed class UpdatePkgConfigPath : UpdatePathCommand {
    public UpdatePkgConfigPath () : base("PKG_CONFIG_PATH") { }
  }

  [Cmdlet(VerbsData.Update, "PSModulePath")]
  public sealed class UpdatePSModulePath : UpdatePathCommand {
    public UpdatePSModulePath () : base("PSModulePath") { }
  }

  [Cmdlet(VerbsData.Update, "PythonPath")]
  public sealed class UpdatePythonPath : UpdatePathCommand {
    public UpdatePythonPath () : base("PYTHON_PATH") { }
  }

  [Cmdlet(VerbsData.Update, "SystemPath")]
  public sealed class UpdateSystemPath : UpdatePathCommand {
    public UpdateSystemPath () : base("PATH") { }
  }
#endregion

}
