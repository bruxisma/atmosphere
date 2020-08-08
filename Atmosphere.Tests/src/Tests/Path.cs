using Atmosphere.Commands;
using System;
using Xunit;

namespace Atmosphere.Tests {
  public class GetEnvironmentPath : Test {
    [Fact]
    public void Empty() { }
  }

  public class GetLDLibraryPath : Test {
    [Fact]
    public void Empty() { }
  }

  public class GetPkgConfigPath : Test { }
  public class GetPSModulePath : Test { }
  public class GetPythonPath : Test { }
  public class GetSystemPath : Test { }

  public class UpdateLDLibraryPath : Test { }
  public class UpdatePkgConfigPath : Test { }
  public class UpdatePSModulePath : Test { }
  public class UpdatePythonPath : Test { }
  public class UpdateSystemPath : Test { }
}
