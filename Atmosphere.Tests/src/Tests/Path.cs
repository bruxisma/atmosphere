using System.Management.Automation;
using System.Collections.Generic;
using System.IO;
using System;

using Xunit.Abstractions;
using Xunit;

using Atmosphere.Commands;

namespace Atmosphere.Tests {

  public class EnvironmentPathData : TheoryData {
    public void Add (string key, params string[] paths) {
      this.AddRow(key, paths);
    }
  }

  public class GetEnvironmentPath : Test {
    private readonly ITestOutputHelper output;
    private readonly Session session;

    public GetEnvironmentPath (ITestOutputHelper output) {
      this.output = output;
      this.session = new Session(this);
    }

    public void Dispose () { this.session.Dispose(); }
    public ITestOutputHelper Output { get => this.output; }

    [Theory]
    [MemberData(nameof(Paths))]
    public void BasicOperation(string name, params string[] paths) {
      this.session.Environment.Add(name, String.Join(Path.PathSeparator, paths));
      this.session.AddCommand("Get-EnvironmentPath").AddParameter("Name", name);
      var result = this.session.Invoke();
      for (int idx = 0; idx < result.Count; idx++) {
          Assert.IsType<DirectoryInfo>(result[idx].BaseObject);
          var item = result[idx].BaseObject as DirectoryInfo;
          var path = paths[idx] as string;
          Assert.Equal(item.Name, path);
      }
    }

    public static EnvironmentPathData Paths => new EnvironmentPathData {
      { "ATMOSPHERE", "1", "2" },
      { "ATMOSPHERE", "2", "3" },
      { "ATMOSPHERE", "4" },
    };
  }

  //public class GetLDLibraryPath : Test {
  //  [Fact]
  //  public void Empty() { }
  //}

  //public class GetPkgConfigPath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class GetPSModulePath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class GetPythonPath : Test {
  //  [Fact] public void Empty() { }
  //}
  //public class GetSystemPath : Test {
  //  [Fact] public void Empty() { }
  //}

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
