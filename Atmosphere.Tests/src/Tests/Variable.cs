using System.Management.Automation;
using System;
using Xunit.Abstractions;
using Xunit;

namespace Atmosphere.Tests {
  public class GetEnvironmentVariable : Test {

    public GetEnvironmentVariable (ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData("ATMOSPHERE", "")]
    [InlineData("ATMOSPHERE", "TEST")]
    [InlineData("NON-STANDARD", "VALUE")]
    public void BasicOperation (string name, string value) {
      Session.Environment.Add(name, value);
      Session.AddCommand("Get-EnvironmentVariable").AddParameter("Name", name);
      Assert.Equal(value, String.Join("", Session.Invoke()));
    }
  }

  public class SetEnvironmentVariable : Test {

    public SetEnvironmentVariable (ITestOutputHelper output) : base(output) { }

    [Theory]
    [InlineData("ATMOSPHERE", "")]
    [InlineData("ATMOSPHERE", "TEST")]
    [InlineData("NON-STANDARD", "VALUE")]
    public void BasicOperation (string name, string value) {
      Session.Environment.Add("ATMOSPHERE", "TEST");
      Session
        .AddCommand("Set-EnvironmentVariable")
        .AddParameter("Name", name)
        .AddParameter("Value", value)
        .AddStatement()
        .AddScript($"[System.Environment]::GetEnvironmentVariable('{name}')");
      Assert.Equal(value, String.Join("", Session.Invoke()));
    }

  }
}
