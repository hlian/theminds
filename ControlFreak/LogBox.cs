using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Aspirations {
  public class LogBox : RichTextBox {
    public delegate void LineDel(ref string line, ref Color color);

    public LogBox()
      : base() {
      NativeMethods.ParaFormat fmt = new NativeMethods.ParaFormat();
      fmt.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(fmt);
      fmt.dwMask = 0x00000100; // PFM_LINESPACING
      fmt.dyLineSpacing = 20; // 20 = single-space, 40 = double-, &c
      fmt.bLineSpacingRule = 5;
      NativeMethods.SendMessage(this.Handle, 1095, (IntPtr)0, ref fmt);

      this.Font = new System.Drawing.Font("Calibri", 11);
      this.WordWrap = true;
      this.SelectionHangingIndent = 30;
      this.ReadOnly = true;
      this.Dock = DockStyle.Fill;
    }

    // http://www.dotnet247.com/247reference/msgs/55/279639.aspx
    // http://www.dotnet4all.com/Dot-Net-Books/2004/08/how-to-auto-scroll-richtextbox-in.html
    public void AddLine(string line, Color color) {
      // Any changes to |log| unselects any text the user has selected.
      int oldSelectStart = SelectionStart;
      int oldSelectLength = SelectionLength;
      if (Color.Black != color) {
        Select(TextLength, 0);

        this.SelectionColor = color;

        // (Helpful) Side-effect: appends the text
        this.SelectedText = line + "\n";
        this.SelectionColor = Color.Black;
      }
      else { AppendText(line + "\n"); }

      // In case the user selects something when she updates.
      Select(oldSelectStart, oldSelectLength);

      // Scrolls her to the last line. See links above.
      NativeMethods.SendMessage(Handle, 0x115, (IntPtr)7, IntPtr.Zero);
    }
  }
}