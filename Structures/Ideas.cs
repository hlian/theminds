using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Aspirations {
  public class Ideas : IDisposable {
    public string Get(string id) { return dict[id]; }

    public string Get(string id, string id2) {
      return Get(id + "." + id2);
    }

    public Ideas Set(string id, string value) {
      dict[id] = value;
      return this;
    }

    public Ideas Set(string id, string id2, string value) {
      return Set(id, id2, value);
    }

    // Serves as the flag for Dispose() and for a read-only
    // Ideas instance used, for example, on an internationalization
    // file like lion.txt. A read/write example is a configuration
    // file like config.txt.
    bool trackChanges; string file;
    public Ideas(string file, bool trackChanges) {
      this.trackChanges = trackChanges;
      this.file = file;
      dict = new StringDictionary();
      using (TextReader t = new StreamReader(file)) {
        string line;
        while ((line = t.ReadLine()) != null) parseLine(line);
      }
    }

    public Ideas(string file) : this(file, false) { }

    /**** Private members ****/
    StringDictionary dict;

    // [Ad hoc] The format of each line is two strings,
    // each separated by a quotation mark. A simple
    // string split and a check for \" suffices to parse.
    void parseLine(string line) {
      if (!line.StartsWith("\"")) return;

      string[] tokens = line.Split('"');
      try { dict.Add(tokens[1], tokens[3]); }
      catch (System.ArgumentException) {
        dict[tokens[1]] = tokens[3];
      }
    }

    public void Dispose() {
      if (!trackChanges) return;

      using (TextWriter t = new StreamWriter(file)) {
        foreach (string key in dict.Keys) {
          t.WriteLine(String.Format("\"{0}\" \"{1}\"", key, dict[key]));
        }
      }

      trackChanges = false;
    }
  } // class Ideas
}