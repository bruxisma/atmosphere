using System.Reflection;
using System;

using Xunit.Abstractions;

namespace Atmosphere.Tests {
  public interface Test : IDisposable {
    public ITestOutputHelper Output { get => null; }
  }
}
