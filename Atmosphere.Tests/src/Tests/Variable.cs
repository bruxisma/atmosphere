using System.Management.Automation;
using System;
using Xunit.Abstractions;
using Xunit;

using Atmosphere.Commands;

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
    public void BasicOperation(string name, string value) {
      this.session.Environment.Add(name, value);
      this.session.AddCommand("Get-EnvironmentVariable").AddParameter("Name", name);
      Assert.Equal(value, String.Join("", this.session.Invoke()));
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
    [InlineData("ATMOSPHERE", "TEST")]
    [InlineData("NON-STANDARD", "VALUE")]
    public void BasicOperation(string name, string value) {
      this.session.Environment.Add("ATMOSPHERE", "TEST");
      this.session
        .AddCommand("Set-EnvironmentVariable")
        .AddParameter("Name", name)
        .AddParameter("Value", value)
        .AddStatement()
        .AddScript($"[System.Environment]::GetEnvironmentVariable('{name}')");
      Assert.Equal(value, String.Join("", this.session.Invoke()));
    }

  }
}
