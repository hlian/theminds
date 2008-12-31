using System.Drawing;
using System.Windows.Forms;
using Aspirations;

namespace Theminds {
  sealed partial class App : Form, IAppControls {
    LogBox logBox = new LogBox();
    InputBox inputBox = new InputBox();
    UserList userList = new UserList();

    RichTextBox userNotice = new RichTextBox();
    Panel userPanel = new Panel();
    Panel tabsPanel = new Panel();
    Panel logBoxPanel = new Panel();
    Splitter inputOutputSplit = new Splitter();
    Splitter activePassiveOutputSplit = new Splitter();

    Tabber tabber;
    void SetUpForm() {
      this.WindowState = FormWindowState.Maximized;
      this.KeyPreview = true;
      this.SuspendLayout();

      inputBox.Dock = DockStyle.Bottom;
      inputBox.Font = new System.Drawing.Font("Lucida Console", 10);
      inputBox.Multiline = false;

      logBoxPanel.Controls.Add(logBox);
      logBoxPanel.Dock = DockStyle.Fill;
      logBoxPanel.AutoSize = true;

      /* The users, the notices, the panel, the myth */
      userNotice.Dock = DockStyle.Top;
      userNotice.BackColor = Color.Blue;

      userList.ScrollAlwaysVisible = true;
      userList.IntegralHeight = false;
      userList.Dock = DockStyle.Fill;
      userList.Width = 200;
      userList.Font = SystemFonts.MessageBoxFont;

      userPanel.Controls.Add(userList);
      userPanel.Controls.Add(userNotice);
      userPanel.Dock = DockStyle.Right;

      /* Splitters! */
      inputOutputSplit.Dock = DockStyle.Bottom;
      inputOutputSplit.Height = 7;
      activePassiveOutputSplit.Dock = DockStyle.Right;
      activePassiveOutputSplit.Width = 7;

      tabsPanel.Dock = DockStyle.Bottom;
      tabsPanel.AutoSize = true;

      tabber = new Tabber(this, "(new)");
      tabber.NewTab += new TabDel(tabber.MoveTo);
      tabber.Add("(server)");

      // Order is critical
      this.Controls.AddRange(new Control[] {
				logBoxPanel, activePassiveOutputSplit, userPanel, inputOutputSplit,
				inputBox, tabsPanel});
      this.Padding = new Padding(5);
      InputBox.Select();

      inputBox.TabIndex = 0;
      userList.TabIndex = 1;
      logBox.TabIndex = 2;
      userNotice.TabIndex = 3;
      this.ResumeLayout();
    }
  }
}