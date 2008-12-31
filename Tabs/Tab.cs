using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Aspirations {
  public sealed class Tab : Button, ITab {
    double shrinkage = 1.0;
    public double Shrinkage {
      get { return shrinkage; }
      set {
        if (value == shrinkage) return;
        this.shrinkage = value;
        this.Width = (int)(this.trueWidth * value);
      }
    }

    // del: Add |this| to an object that will calculate its
    //    autosized width (a Form).
    int trueWidth;
    public int TrueWidth { get { return trueWidth; } }
    public Tab(KissWidthDel del)
      : base() {
      this.AutoSize = true;
      this.TabStop = false;
      del(this);
      this.trueWidth = this.Width;

      this.Width = (int)(trueWidth * Shrinkage);
      this.Font = SystemFonts.MessageBoxFont;
      this.BecomeNew();
    }

    public Tab(KissWidthDel del, string text)
      : this(del) { this.Text = text; }

    bool depressed;
    public void BecomeOld() {
      this.ForeColor = Color.LightGray;
      depressed = false;
    }

    public void BecomeNew() {
      this.ForeColor = Color.Purple;
      depressed = true;
    }

    protected override void OnPaint(PaintEventArgs pevent) {
      base.OnPaint(pevent);
      if (!depressed) return;
      ButtonRenderer.DrawButton(pevent.Graphics, this.ClientRectangle,
         this.Text, this.Font, false, PushButtonState.Pressed);
    }
  }
}