using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using Aspirations;

namespace Theminds.Tests {
   public sealed partial class MockApp : IAppControls, ITabsParent {
      string currentChannel;
      public string CurrentChannel {
         get { return currentChannel; }
         set { currentChannel = value; }
      }

      Buffer buffer; Quirk quirk;
      Tabber tabber;
      public MockApp() {
         QuirkStart mozNet = new QuirkStart() {
            Nick = "Tongue", Port = 6667,
            Server = "irc.mozilla.org", User = "USER cryptoliter2 8 * :Hi",
         };
         quirk = new Quirk(mozNet);

         this.tabber = new Tabber(this, "(new)");
         tabber.NewTab += delegate { };
         tabber.Add("(server)");
         this.buffer = new Buffer(this, quirk);
         this.userList = new UserList();
      }

      UserList userList;
      public LogBox LogBox { get { return new LogBox(); } }
      public Tabber Tabber { get { return tabber; } }
      public Quirk Connection { get { return quirk; } }
      public Buffer Buffer { get { return buffer; } }
      public Users Users { get { return null; } }
      public UserList UserList { get { return userList; } }
      public bool InvokeRequired { get { return true; } }
      public void SwitchLogBox(LogBox c) { }
      public void GrabFocus() { }
      public int TabsWidth { get { return 5; } }

      public Aspirations.InputBox InputBox {
         get { throw new Exception("The method or operation is not implemented."); }
      }

      public IAsyncResult BeginInvoke(Delegate d, params object[] args) {
         Invoke(d, args); return null;
      }

      public object Invoke(Delegate d, params object[] args) {
         switch (args.Length) {
            case 0: return d.DynamicInvoke(null);
            case 1: return d.DynamicInvoke(args[0]);
            case 2: return d.DynamicInvoke(args[0], args[1]);
            case 3: return d.DynamicInvoke(args[0], args[1], args[2]);
            default: throw new InvalidOperationException("Time to extend MockApp#Invoke");
         }
      }

      public void AddTab(Control button) {
         throw new Exception("The method or operation is not implemented.");
      }

      public void RemoveTab(Control button) {
         throw new Exception("The method or operation is not implemented.");
      }

      public ITab CreateTab(string label) {
         return new Tab(delegate { }, "(new)");
      }
      
      public void SuspendLayout() {
         throw new Exception("The method or operation is not implemented.");
      }

      public void ResumeLayout() {
         throw new Exception("The method or operation is not implemented.");
      }

      public static bool ArrayEquality<T>(T[] before, T[] after) {
         if (before.Length != after.Length) return false;
         int length = before.Length;
         for (int i = 0; i < length; i++)
            if (!before[i].Equals(after[i])) return false;
         return true;
      }

      public static void PokeBuffer(IAppControls app, string line, LineDel girls) {
         app.Buffer.PostLine += girls;
         app.Buffer.AddLine(line);
         app.Buffer.PostLine -= girls;
      }

      public event MethodInvoker PostOffice = delegate { };
   }
}