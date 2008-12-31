using System;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Bowel;
using Ants = Bowel.ServerPrefixNumberRegex;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using S = System.String;
using Quirk = Aspirations.Quirk;

namespace Theminds.Filters {
  [DesiresAppControls]
  class LogBoxFilters {
    MircRegex mircRegex = new MircRegex();
    Ants ants = new Ants();

    Quirk quirk; Buffer buffer;
    public LogBoxFilters(IAppControls app) {
      buffer = app.Buffer; quirk = app.Connection;

      buffer.PreLine += new LineDel(hostName);
      buffer.PreLine += new LineDel(serverPrefix);

      buffer.Line += new LineDel(ping);
      buffer.SelfLine += new LineDel(selfJoin);
      buffer.SelfLine += (ref BufferData data) => data.Color = Color.DarkRed;
      buffer.Line += (ref BufferData data) => {
        // Strip mIRC colors.
        string line = data.Line;
        if (line.Contains("\u0003")) line = mircRegex.Replace(line, "");
      };
    }

    string trucks = App.Lion.Get("server.prefix");
    // Format: <host> <reply number> <nick (optional)>
    void serverPrefix(ref BufferData data) {
      // Ignore commands from self
      string line = data.Line;
      if (!line.StartsWith(":")) return;

      // Check for <host>
      line = line.Substring(1);
      string host = quirk.Info.HostName;
      if (!line.StartsWith(host)) return;

      // Strip reply number and possibly nick.
      line = ants.Replace(line.Substring(host.Length + 1), "");
      string nick = quirk.Info.Nick;
      if (line.StartsWith(nick))
        line = line.Substring(nick.Length + 1);

      data.Line = trucks + " " + line;
    }

    void hostName(ref BufferData dc) {
      string line = dc.Line;
      if (line.StartsWith(":") == false) return;
      quirk.Info.HostName = line.Substring(1, line.IndexOf(' ') - 1);
      buffer.PreLine -= new LineDel(hostName);
    }

    void ping(ref BufferData dc) {
      if (false == dc.Line.StartsWith("PING :")) return;

      pingMessage = dc.Line;
      buffer.PostLine += new LineDel(sendPong);
      dc.Color = Color.Blue;
    }

    string pingMessage;
    void sendPong(ref BufferData data) {
      buffer.Line += new LineDel(colorPong);
      // Remove before Message or else recursion
      buffer.PostLine -= new LineDel(sendPong);

      // PING : is six letters
      quirk.Message("PONG :" + pingMessage.Substring(6));
    }

    void colorPong(ref BufferData dc) {
      if (false == dc.Line.StartsWith("PONG :")) return;
      dc.Color = Color.Blue;
      buffer.Line -= new LineDel(colorPong);
    }

    void selfJoin(ref BufferData dc) {
      string x = "JOIN "; string line = dc.Line;
      if (false == line.StartsWith(x)) return;
      dc.Color = Color.Blue;

      // Format: |JOIN #channel,#channel,#channel|
      string[] channels = line.Substring(x.Length).Split(',');
      dc.Channel = channels[channels.Length - 1];
    }
  }
}