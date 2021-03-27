using Xunit.Abstractions;
using Xunit;

namespace Atmosphere.Tests {

  public class GetEnvironmentPath : PathTest {
    public GetEnvironmentPath (ITestOutputHelper output) :
      base(output, "ATMOSPHERE", "Get-EnvironmentPath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Session.AddParameter("Name", Variable);
      Invoke(paths);
    }
  }

  public class GetLDLibraryPath : PathTest {
    public GetLDLibraryPath (ITestOutputHelper output) :
      base(output, "LD_LIBRARY_PATH", "Get-LDLibraryPath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Invoke(paths);
    }
  }

  public class GetPkgConfigPath : PathTest {
    public GetPkgConfigPath (ITestOutputHelper output) :
      base(output, "PKG_CONFIG_PATH", "Get-PkgConfigPath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Invoke(paths);
    }
  }

  public class GetPSModulePath : PathTest {
    public GetPSModulePath (ITestOutputHelper output) :
      base(output, "PSModulePath", "Get-PSModulePath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Invoke(paths);
    }
  }

  public class GetPythonPath : PathTest {
    public GetPythonPath (ITestOutputHelper output) :
      base(output, "PYTHON_PATH", "Get-PythonPath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Invoke(paths);
    }
  }

  public class GetSystemPath : PathTest {
    public GetSystemPath (ITestOutputHelper output) :
      base(output, "PATH", "Get-SystemPath") { }

    [Theory]
    [MemberData(nameof(PathTest.Names))]
    public void BasicOperation (params string[] paths) {
      Setup(paths);
      Invoke(paths);
    }
  }

  //public class UpdateLDLibraryPath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class UpdatePkgConfigPath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class UpdatePSModulePath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class UpdatePythonPath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class UpdateSystemPath : Test {
  //  [Fact] public void Empty() { }
  //}
}
