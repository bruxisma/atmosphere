using System.Linq;
using System.IO;
using System;

using Xunit.Abstractions;
using Xunit;

namespace Atmosphere.Tests {

  public abstract class Test : IDisposable {
    private readonly ITestOutputHelper output;
    private readonly Session session;

    protected Test (ITestOutputHelper output) {
      this.output = output;
      this.session = new Session(this);
    }

    public virtual void Dispose () { this.session.Dispose(); }
    public ITestOutputHelper Output { get => this.output; }
    public Session Session { get => this.session; }
  }


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
}
