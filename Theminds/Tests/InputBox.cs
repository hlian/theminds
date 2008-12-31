using System;
using System.Diagnostics;
using Theminds;
using Aspirations;

namespace Theminds.Tests {
   [DesiresTesting]
   class InputBox : Aspirations.InputBox {
      public void TestParseText() {
         this.Text = "/cmd";
         string[] a = base.ParseText();
         string[] b = new string[] { "cmd", "" };
         if (a[0] == b[0] && a[1] == b[1]) return;

         Debug.Fail("ParseText() failed");
      }

      public InputBox() {
         Type t = typeof(InputBox);
         string[] tests = new string[] {"TestParseText"};
         foreach (string test in tests) {
            t.GetMethod(test).Invoke(this, null);
         }
      }
   }
}