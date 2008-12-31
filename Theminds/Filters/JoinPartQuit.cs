using System.Drawing;
using Aspirations;

namespace Theminds.Filters {
  [DesiresAppControls]
  class JoinPartQuit {
    Quirk quirk; IAppControls app;
    Ideas lion = App.Lion;
    public static JoinPartQuit Instance;
    public JoinPartQuit(IAppControls app) {
      this.app = app; quirk = app.Connection;
      app.Buffer.Line += new LineDel(filter);
      Instance = this;
    }

    // line ~ ":nick!ip join :#chan"
    // line ~ ":nick!ip part #chan :msg"
    // line ~ ":nick!ip quit :msg"
    protected void filter(ref BufferData data) {
      string line = data.Line;
      JazzNotes notes = new JazzNotes(line);
      if (!(line.Contains(" ") && line.StartsWith(":"))) return;
      int[] spaces = line.FindSpaces(3);
      notes.Spaces = spaces;

      findNickAndIp(ref notes);
      if (null == notes.Nick) return;
      notes.FromMe = (notes.Nick == quirk.Info.Nick);
      notes.Mode = line.Tween(spaces[0], spaces[1] - 1).ToLowerInvariant();

      int reasonIndex = 0;
      switch (notes.Mode) {
        case "join":
          data.Channel = line.Substring(spaces[1] + 1); break;
        case "part":
          data.Channel = line.Tween(spaces[1], spaces[2] - 1);
          reasonIndex = spaces[2] + 1; break;
        case "quit":
          reasonIndex = spaces[1] + 1;
          data.BroadcastId = "quit." + notes.Nick; break;
        default: return;
      }
      notes.ReasonIndex = reasonIndex;
      data.Color = Color.Gray;
      findMessage(ref data, ref notes);
      findReason(ref data, ref notes);

      // Output part messages caused by me to the (server)
      // tab now that the find* twins are finished.
      if ("part" == notes.Mode && notes.FromMe) {
        data.Channel = null;
        data.BroadcastId = "part";
      }
    }

    // index: index of the start of the message for us to parse
    void findReason(ref BufferData data, ref JazzNotes notes) {
      int index = notes.ReasonIndex;
      if (0 == index || notes.Line.Length <= index) return;
      string reason = lion.Get(notes.Mode, "reason");
      reason = reason.Fill(notes.Line.Substring(index));
      data.Line = lion.Get(notes.Mode, "total").Fill(data.Line, reason);
    }

    void findMessage(ref BufferData data, ref JazzNotes notes) {
      if (notes.FromMe) {
        data.Line = lion.Get(notes.Mode, "self").Fill(data.Channel); return;
      }
      data.Line = lion.Get(notes.Mode, "others").
         Fill(notes.Nick, notes.Ip, data.Channel);
    }

    void findNickAndIp(ref JazzNotes notes) {
      string user = notes.Line.Tween(0, notes.Spaces[0] - 1);
      // Roots out junk like ":<nick> MODE +x"
      if (!user.Contains("!")) return;
      notes.Nick = user.Tween(1, user.IndexOf('!'));
      notes.Ip = user.Substring(user.IndexOf('!') + 1);
    }
  }

  struct JazzNotes {
    public string Nick, Ip, Line, Mode;
    public int[] Spaces;
    public int ReasonIndex; public bool FromMe;
    public JazzNotes(string line) {
      Line = line; Spaces = null;
      Nick = Ip = Mode = null;
      ReasonIndex = 0;
      FromMe = false;
    }
  }
}