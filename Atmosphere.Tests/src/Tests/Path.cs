using System.Linq;
using System.IO;
using System;

using Xunit.Abstractions;
using Xunit;

namespace Atmosphere.Tests {

  // TODO: Move this into its own file (Alongside class Test)
  public class PathTest : Test {

    public PathTest (ITestOutputHelper output, string variable, string command) :
      this(output, variable) { this.Command = command; }
    public PathTest (ITestOutputHelper output, string variable) :
      this(output) { this.Variable = variable; }
    public PathTest (ITestOutputHelper output) : base(output) { }

    public static TheoryData<string[]> Names => new TheoryData<string[]> {
      new string[]{ "1", "2" },
      new string[]{ "3", "4" },
      new string[]{ "5" }
    };

    protected void Setup (string[] paths) {
      Session.Environment.Add(this.Variable, String.Join(Path.PathSeparator, paths));
      Session.AddCommand(this.Command);
    }
    protected void Invoke (string[] paths) {
      var result = Session.Invoke().Select((item) => {
        Assert.IsType<DirectoryInfo>(item.BaseObject);
        return item.BaseObject as DirectoryInfo;
      }).ToList();
      for (int idx = 0; idx < result.Count; idx++) {
        Assert.Equal(result[idx].Name, paths[idx]);
      }
    }

    ///<summary>Environment Variable to read from</summary>
    public string Variable { get; protected set; }
    ///<summary>Name of command to add</summary>
    public string Command { get; protected set; }
  }

  public class EnvironmentPathData : TheoryData {
    public void Add (string key, params string[] paths) {
      this.AddRow(key, paths);
    }
  }

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
