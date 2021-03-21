using System.Collections.Generic;
using System.Collections;
using System;

namespace Atmosphere.Tests {
  public abstract class TheoryData : IEnumerable<object[]> {
    readonly List<object[]> data = new List<object[]>();

    protected void AddRow(params object[] values) { this.data.Add(values); }
    public IEnumerator<object[]> GetEnumerator() { return this.data.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
  }
}
