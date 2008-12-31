using System;
using System.Drawing;
using Aspirations;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using S = System.String;

namespace Theminds.Filters {
  [DesiresAppControls]
  class Privmsg {
    IAppControls app; Quirk quirk;
    string speechAll, actionAll;
    public Privmsg(IAppControls app) {
      this.app = app;
      this.quirk = app.Connection;

      app.Buffer.Line += new LineDel(filter);
      app.Buffer.SelfLine += new LineDel(filter);

      speechAll = App.Lion.Get("speech.all");
      actionAll = App.Lion.Get("action.all");
    }

    // There are two separate functions. The code may seem
    // the same, but the differences are subtle and prolific
    // enough to preclude condensation.
    protected void filter(ref BufferData data) {
      string line = data.Line;
      if (!line.Contains(" ")) return;

      int[] spaces = line.FindSpaces(4);
      if (line.StartsWith("PRIVMSG "))
        filterSelf(ref data, spaces);
      else if (line.StartsWith(":")
         && line.Substring(spaces[0]).StartsWith("PRIVMSG "))
        filterOthers(ref data, spaces);
      return;
    }

    // line ~ "PRIVMSG #channel msg"
    // line ~ "PRIVMSG #channel :\u0001ACTION <msg>\u0001"
    void filterSelf(ref BufferData data, int[] spaces) {
      string line = data.Line;
      data.Channel = line.Tween(spaces[0], spaces[1] - 1);

      string nick = quirk.Info.Nick;
      string msg = line.Substring(spaces[1]);
      // Notice the colon! Weird protocol.
      if (msg.StartsWith(":\u0001ACTION")) {
        msg = line.Tween(spaces[2], line.Length - 1);
        data.Line = S.Format(actionAll, nick, msg);
        data.Color = Color.Green;
        return;
      }
      data.Line = S.Format(speechAll, nick, msg);
    }

    // line ~ ":nick!ip PRIVMSG #channel :msg"
    // line ~ ":nick!ip PRIVMSG #channel :\u0001ACTION <msg>\u0001"
    void filterOthers(ref BufferData data, int[] spaces) {
      string line = data.Line;
      data.Channel = line.Tween(spaces[1], spaces[2] - 1);

      string nick = line.Tween(1, line.IndexOf('!'));
      string msg = line.Substring(spaces[2] + 1);
      if (msg.StartsWith("\u0001ACTION")) {
        msg = line.Tween(spaces[3], line.Length - 1);
        data.Line = S.Format(actionAll, nick, msg);
        data.Color = Color.Green;
        return;
      }
      data.Line = S.Format(speechAll, nick, msg);
    }
  }
}