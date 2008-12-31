// I intercept NAMES messages sent upon a
// channel join and also manually by the user.
// It works with the Users class to seed
// a per-channel list of users. See RPL_NAMEREPLY
// and RPL_ENDOFNAMES in the RFC. In addition,
// I serve the dual purpose of seeding the
// UserList control.
using Aspirations;
using S = System.String;

namespace Theminds.Filters {
  [DesiresAppControls]
  class Names {
    IAppControls app;
    public Names(IAppControls app) {
      this.app = app;
      app.Buffer.Line += new LineDel(filter);
    }

    // line ~ "[server] = <channel> :[[@|+]<nick> [[@|+]<nick> [...]]]"
    // line ~ "[server] <channel> :End of /NAMES list"
    readonly string serverPrefix = App.Lion.Get("server.prefix");
    protected void filter(ref BufferData data) {
      if (data.Line.EndsWith(":End of /NAMES list.")) {
        data.Ignore = true;
        app.UserList.Flush();
        return;
      }

      string test = S.Format("{0} = ", serverPrefix);
      if (!data.Line.StartsWith(test)) return;

      // Remember the colon! Rememeber the weird tacked space!
      int[] spaces = data.Line.FindSpaces(4);
      data.Channel = data.Line.Tween(spaces[1], spaces[2] - 1);
      string[] nicks = data.Line.Substring(spaces[2] + 1).Trim().Split(' ');
      Users.Instance.Clear(data);
      foreach (string nick in nicks) {
        Users.Instance[data] = nick;
        app.UserList.Push(nick);
      }
      data.Ignore = true;
    }
  }
}