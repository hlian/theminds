using System;
using System.Diagnostics;
using Uganda = Aspirations.UserList;

namespace Theminds.Tests {
   [DesiresTestingWithMockApp]
   class Names : Filters.Names {
      IAppControls app; Uganda uganda;
      
      // Filters.Names has a hiddene dependency on the
      // Filter.Users.Instance static memeber, which we
      // seed here with anonymous construction in lieu
      // of DesiresAppControls.
      public Names(IAppControls app)
         : base(app) {
         new Users(app);
         this.app = app; this.uganda = app.UserList;

         test("{0} = #spreadbutterNamesFilter :@Sam maria jark ", "fresh",
            new string[] { "@Sam", "maria", "jark" });
      }

      string serverPrefix = App.Lion.Get("server.prefix");
      // Simulating a complete join session entails
      // feeding in a join line.
      void test(string line, string id, string[] countries) {
         line = String.Format(line, serverPrefix);
         app.Buffer.AddLine(":nick!ip join :#spreadbutterNamesFilter");
         app.Buffer.AddLine(line);
         app.Buffer.AddLine("[server] #spreadbutterNamesFilter :End of /NAMES list.");

         string[] o = new string[uganda.Items.Count];
         uganda.Items.CopyTo(o, 0);
         if (MockApp.ArrayEquality(o, countries)) return;
         throw new InvalidOperationException("NamesFilter faiure in " + id);
      }
   }
}