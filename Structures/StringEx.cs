using System;

namespace Aspirations {
  public static class StringEx {
    public static int[] FindSpaces(this string line, int times) {
      int[] spaces = new int[times];
      int last = 0, i = 0;
      // Find actual spaces.
      for (i = 0; i < times; ++i) {
        last = line.IndexOf(' ', last) + 1;
        if (last == 0) break;
        else spaces[i] = last;
      }
      // Pad out.
      for (int j = i; j < times; ++j)
        spaces[j] = line.Length + 1;

      return spaces;
    }

    // "0123456789", 3, 7 => "3456" (`end`th character removed)
    public static string Tween(this string haystack, int begin, int end) {
      if (end < haystack.Length)
        return haystack.Remove(end).Substring(begin);
      else
        return haystack.Substring(begin);
    }

    // http://www.irchelp.org/irchelp/rfc/chapter1.html#c1_3
    public static bool IsChannel(this string channel) {
      if (channel == null) return false;
      if (channel.StartsWith("#")) return true;
      if (channel.StartsWith("&")) return true;
      return false;
    }

    public static string Fill(this string template, params object[] args) {
      return String.Format(template, args);
    }
  }
}