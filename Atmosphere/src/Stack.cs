using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Atmosphere {

  using LazyEnvironment = Lazy<Stack<Dictionary<string, string>>>;

  internal static class EnvironmentStack {

    private static readonly LazyEnvironment stack = new LazyEnvironment();

    internal static void Push (Dictionary<string, string> vars) {
      stack.Value.Push(vars);
    }

    internal static void Push () { Push(Environment.Current.Variables); }
    internal static Dictionary<string, string> Pop () {
      if (stack.Value.Count == 0) { return new Dictionary<string, string>(); }
      return stack.Value.Pop();
    }
  }
}
