using Atmosphere.Commands;
using Xunit.Abstractions;
using Xunit;

namespace Atmosphere.Tests {
  public class GetEnvironmentVariable : Test {
    private readonly ITestOutputHelper output;
    private readonly Session session;

    public GetEnvironmentVariable(ITestOutputHelper output) {
      this.output = output;
      this.session = new Session(this);
    }

    public void Dispose () { this.session.Dispose(); }
    public ITestOutputHelper Output { get => this.output; }

    [Theory]
    [InlineData("ATMOSPHERE", "")]
    [InlineData("ATMOSPHERE", "TEST")]
    [InlineData("NON-STANDARD", "VALUE")]
    public void BasicOperation(string Name, string Value) {
      this.session.Environment.Add(Name, Value);
      this.session.AddCommand("Get-EnvironmentVariable").AddParameter("Name", Name);
      var results = this.session.Invoke();
      foreach (var result in results) {
        Assert.Equal($"{result}", Value);
      }
    }
  }

  public class SetEnvironmentVariable : Test {
    private readonly ITestOutputHelper output;
    private readonly Session session;

    public SetEnvironmentVariable(ITestOutputHelper output) {
      this.output = output;
      this.session = new Session(this);
    }

    public void Dispose () { this.session.Dispose(); }
    public ITestOutputHelper Output { get => this.output; }

    [Theory]
    [InlineData("ATMOSPHERE", "")]
    public void SetToEmptyString(string Name, string Value) {
      this.session.Environment.Add("ATMOSPHERE", "TEST");
      this.session
        .AddCommand("Set-EnvironmentVariable")
        .AddParameter("Name", Name)
        .AddParameter("Value", Value)
        .AddStatement()
        .AddScript("[System.Environment]::GetEnvironmentVariable('ATMOSPHERE')");
      var results = this.session.Invoke();
      foreach (var result in results) {
        Assert.Equal($"{result}", Value);
      }
    }

  }
}
