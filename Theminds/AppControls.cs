using System;
using System.Drawing;
using System.Windows.Forms;
using Aspirations;

namespace Theminds {
  public interface IAppControls {
    LogBox LogBox { get; }
    Tabber Tabber { get; }
    Quirk Connection { get; }
    Buffer Buffer { get; }
    UserList UserList { get; }
    InputBox InputBox { get; }
    string CurrentChannel { get; set; }
    bool InvokeRequired { get; }

    IAsyncResult BeginInvoke(Delegate d, params object[] args);
    object Invoke(Delegate d, params object[] args);
    void SwitchLogBox(LogBox c);

    event MethodInvoker PostOffice;
  }

  public partial class App : Form, IAppControls {
    public LogBox LogBox {
      get { return this.logBox; }
    }

    public Tabber Tabber {
      get { return this.tabber; }
    }

    public Quirk Connection {
      get { return this.quirk; }
    }

    public UserList UserList {
      get { return this.userList; }
    }

    public InputBox InputBox {
      get { return this.inputBox; }
    }

    public Buffer Buffer {
      get { return buffer; }
    }

    public string CurrentChannel { get; set; }

    public void SwitchLogBox(LogBox l) {
      if (l == logBoxPanel.Controls[0]) return;
      this.SuspendLayout();
      logBoxPanel.Controls.RemoveAt(0);
      logBoxPanel.Controls.Add(l);
      this.ResumeLayout();

      this.logBox = l;
    }
  }
}