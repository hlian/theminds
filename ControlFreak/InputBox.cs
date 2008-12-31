using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Aspirations {
  public class InputBox : TextBox {
    public delegate void CommandDel(string cmd, string arg);
    public delegate void MessageDel(string message);

    public event CommandDel Command;
    public event MessageDel Message;

    protected override void OnKeyDown(KeyEventArgs e) {
      if (Keys.Escape == e.KeyCode) {
        Text = null; e.SuppressKeyPress = true;
        return;
      }
      if (Keys.Enter != e.KeyCode) return;
      if (Text == "") return;
      e.SuppressKeyPress = true;

      if (Text.StartsWith("/")) {
        if (Text.StartsWith("//")) {
          Message(Text.Substring(1));
        }
        else {
          string[] s = this.ParseText();
          string command = s[0]; string arg = s[1];
          Command(command, arg);
        }
      }
      else Message(Text);
      base.OnKeyDown(e);
    }

    protected string[] ParseText() {
      int firstSpace = Text.IndexOf(' ');
      string command = (-1 != firstSpace) ?
         Text.Substring(1, firstSpace - 1) : Text.Substring(1);
      string arg = (-1 != firstSpace) ?
         Text.Substring(firstSpace + 1) : "";
      return new string[] { command, arg };
    }
  }
}