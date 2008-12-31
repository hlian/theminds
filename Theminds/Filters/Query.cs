using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using Aspirations;
using S = System.String;

namespace Theminds.Filters {
  [DesiresAppControls]
  class Query {
    IAppControls app;
    LineDel introFilterDel;
    public Query(IAppControls app) {
      this.app = app;
      this.introFilterDel = new LineDel(introFilter);
      app.InputBox.Command += new InputBox.CommandDel(filter);
    }

    string intro = App.Lion.Get("query.intro");
    string nonce = App.Lion.Get("query.intro.nonce");
    Queue<string> args = new Queue<string>(2);
    void filter(string cmd, string arg) {
      if ("query" != cmd) return;
      this.args.Enqueue(arg);
      app.Buffer.SelfLine += introFilterDel;
      app.Buffer.AddLine(S.Format(intro, arg, nonce));
    }

    void introFilter(ref BufferData dc) {
      if (!dc.Line.StartsWith(nonce)) return;
      string arg = args.Dequeue();
      dc.Channel = arg;
      dc.Color = Color.DarkSlateBlue;
      dc.Line = dc.Line.Substring(nonce.Length);
      app.Buffer.SelfLine -= introFilterDel;
    }
  }
}