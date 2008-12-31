using System;
using System.Drawing;
using System.Windows.Forms;
using Aspirations;

namespace Theminds.Filters {
  [DesiresAppControls]
  class Who {
    IAppControls app;
    LineDel filterDel;
    public Who(IAppControls app) {
      // Hook me up to the command /w in addition to filtering
      // WHO commands; thus, Who serves as a dual purpose class.
      app.InputBox.Command += (cmd, arg) => {
        if ("w" != cmd) return;
        if (!app.CurrentChannel.IsChannel()) return;
        app.Buffer.Line += filterDel;
        app.Connection.Message("WHO " + app.CurrentChannel);
      };
      app.Buffer.SelfLine += new LineDel(whoMessageFilter);
      filterDel = new LineDel(filter);
      this.app = app;
    }

    void whoMessageFilter(ref BufferData data) {
      if (!data.Line.StartsWith("WHO ")) return;
      data.Channel = app.CurrentChannel;
    }

    // `userList` flickers with naive Clear&Add()s.
    // [0:server] [1:channel] [2:user] [3:host] [4:server] [5:nick]...
    readonly string serverPrefix = App.Lion.Get("server.prefix");
    void filter(ref BufferData data) {
      string[] tokens = data.Line.Split(' ');
      if (!tokens[1].IsChannel()) return;

      data.Channel = tokens[1];
      data.Color = Color.DarkBlue;
      data.Ignore = true;
      if (data.Line.Contains("End of /WHO"))
        app.BeginInvoke(new MethodInvoker(stop));
      else
        app.UserList.Push(tokens[5]);
    }

    void stop() {
      app.UserList.Flush();
      app.Buffer.Line -= filterDel;
    }
  }
}