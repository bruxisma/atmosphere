using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

namespace Atmosphere {
  internal static class Extensions {

    internal static bool IsNullOrEmpty<T>(this IEnumerable<T> source) {
      return source == null || source.IsEmpty();
    }
    internal static bool IsEmpty<T>(this IEnumerable<T> source) {
      return !source.Any();
    }

    internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
      foreach (T item in source) { action(item); }
    }

    internal static string Join<T>(this IEnumerable<T> source, char separator) {
      return String.Join(separator, source);
    }

    internal static string FromPathList(this IEnumerable<DirectoryInfo> source) {
      return source.Join(Path.PathSeparator);
    }

    internal static List<DirectoryInfo> IntoPathList(this string source) {
      return source
        .Split(Path.PathSeparator)
        .Where(path => !path.IsNullOrEmpty())
        .Select(path => new DirectoryInfo(path))
        .ToList();
    }
  }
}
