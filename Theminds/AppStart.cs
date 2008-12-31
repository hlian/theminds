//#define ENABLE_TEST

using System.Diagnostics;
using System.Windows.Forms;

namespace Theminds {
  static class AppStart {
    [System.STAThread]
    static void Main() {
#if ENABLE_TEST
         Debug.Indent();
         App.LoadAttributeLovers(typeof(DesiresTestingAttribute));
         App.LoadAttributeLovers(typeof(DesiresTestingWithMockAppAttribute),
            new Theminds.Tests.MockApp());
#else
      Application.EnableVisualStyles();
      Application.Run(new App());
#endif
    }
  }
}