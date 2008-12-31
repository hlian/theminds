using System;
using System.Windows.Forms;
using System.Reflection;
using Aspirations;
using CancelEventArgs = System.ComponentModel.CancelEventArgs;

namespace Theminds {
  public sealed partial class App : Form {
    public static Ideas Lion = new Ideas(@"lion.txt", true);
    Buffer buffer; Quirk quirk;
    public App() {
      this.SetUpForm(); // SetUpForm.cs

      var mozNet = new QuirkStart() {
        Nick = "Tongue",
        Port = 6667,
        Server = "irc.mozilla.org",
        User = "USER cryptoliter2 8 * :Hi",
      };
      quirk = new Quirk(mozNet);

      this.buffer = new Buffer(this, quirk);
      quirk.NewLine += new Quirk.NewLineDel(Buffer.AddLine);
      App.LoadAttributeLovers(typeof(DesiresAppControlsAttribute), this);

      PostOffice();
      quirk.Start();
    }

    protected override void OnClosing(CancelEventArgs e) {
      quirk.Dispose();
      Lion.Dispose();
      base.OnClosing(e);
    }

    protected override void OnKeyDown(KeyEventArgs e) {
      if (e.Control) {
        e.SuppressKeyPress = true;
        switch (e.KeyCode) {
          case Keys.PageUp: tabber.MoveToPrev(); break;
          case Keys.PageDown: tabber.MoveToNext(); break;
          case Keys.T: buffer.AddChannel(); break;
          case Keys.W: buffer.Remove(tabber.Current); break;
          case Keys.Q: this.Close(); break;
          default: e.SuppressKeyPress = false; break;
        }
      }

      base.OnKeyDown(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e) {
      logBox.Select();
      base.OnMouseWheel(e);
    }

    public static void Alert(object alert) {
      MessageBox.Show(alert.ToString());
    }

    public static void Debug(object o) {
      System.Diagnostics.Debug.WriteLine(o);
    }

    public static void LoadAttributeLovers(Type attribute,
       params object[] args) {
      Type[] types = Assembly.GetExecutingAssembly().GetTypes();
      try {
        foreach (Type type in types) {
          var plugins = type.GetCustomAttributes(attribute, false);
          if (plugins.Length < 1) continue;
          Activator.CreateInstance(type, args);
        }
      }
      catch (TargetInvocationException e) {
        var x = e.InnerException;
        throw new InvalidOperationException(x.Message + x.StackTrace);
      }
    }

    // Called after I construct all DesireAppControls filters
    // so those filters may start mingling among each other,
    // swapping body fluids and pirate stories.
    public event MethodInvoker PostOffice = delegate { };
  }
}