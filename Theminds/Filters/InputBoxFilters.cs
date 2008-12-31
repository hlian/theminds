using System;
using System.Drawing;
using Aspirations;
using S = System.String;

namespace Theminds.Filters {
  [DesiresAppControls]
  class InputBoxFilters {
    static String PRIVMSG = "PRIVMSG {0} :\u0001ACTION {1}\u0001";
    static String NO_PRIVMSG = App.Lion.Get("error.cannot.privmsg");
    IAppControls app;

    public InputBoxFilters(IAppControls app) {
      this.app = app;
      InputBox inputBox = app.InputBox;
      Quirk connection = app.Connection;

      inputBox.Message += new InputBox.MessageDel(PrivmsgCurrentChannel);
      inputBox.Command += (cmd, arg) => {
        switch (cmd) {
          // arg is a channel.
          case "j":
            connection.Message("JOIN " + arg); break;
          case "raw":
            connection.Message(arg); break;
          // arg is a quit message.
          case "q":
            connection.Dispose(arg); break;
          case "me":
            connection.Message(PRIVMSG, app.CurrentChannel, arg);
            break;
        }
      };
    }

    void PrivmsgCurrentChannel(string msg) {
      if (app.CurrentChannel == null)
        app.LogBox.AddLine(NO_PRIVMSG, Color.Purple);
      else {
        var s = S.Format("PRIVMSG {0} {1}", app.CurrentChannel, msg);
        app.Connection.Message(s);
      }
    }
  }
}