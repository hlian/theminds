namespace Aspirations {
  public interface ITabsParent {
    void AddTab(System.Windows.Forms.Control button);
    void RemoveTab(System.Windows.Forms.Control button);
    ITab CreateTab(string label);

    int TabsWidth { get; }

    void SuspendLayout();
    void ResumeLayout();
    void GrabFocus();
  }

  public delegate void KissWidthDel(System.Windows.Forms.Control b);
  public interface ITab {
    int Width { get; set; }
    int TrueWidth { get; }
    int Left { get; set; }
    double Shrinkage { get; set; }

    int GetHashCode();
    void BecomeOld();
    void BecomeNew();
  }
}