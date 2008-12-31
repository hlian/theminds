using System;
using System.Diagnostics;
using Theminds;
using Aspirations;

namespace Theminds.Tests {
   [DesiresTestingWithMockApp]
   class JoinPartQuit : Filters.JoinPartQuit {
      string channel = "#spreadbutter";
      IAppControls app;
      public JoinPartQuit(IAppControls app)
         : base(app) {
         this.app = app;
         string joinOthers = App.Lion.Get("join.others");
         string joinSelf = App.Lion.Get("join.self");
         string partSelf = App.Lion.Get("part.self");
         string partOthers = App.Lion.Get("part.others");
         string partReason = App.Lion.Get("part.reason");
         string partTotal = App.Lion.Get("part.total");
         string quitSelf = App.Lion.Get("quit.self");
         string quitOthers = App.Lion.Get("quit.others");
         string quitReason = App.Lion.Get("quit.reason");
         string quitTotal = App.Lion.Get("quit.total");
         QuirkStart info = app.Connection.Info;
         info.Nick = "Tongue";

         // join, others
         test(":maria!ip JOIN :#spreadbutter",
            joinOthers.Fill("maria", "ip", channel), "maria");
         // join, self
         test(":Tongue!ip JOIN :#spreadbutter",
            joinSelf.Fill(channel), "tips");
         // part, others, no reason
         test(":Tongues!ip PART #spreadbutter",
            partOthers.Fill("Tongues", "ip", channel),
            "roses");
         // part, others, a reason
         test(":Tongues!ip PART #spreadbutter :Too angsty",
            partTotal.Fill(partOthers.Fill("Tongues", "ip", channel),
            partReason.Fill("Too angsty")), "angst");

         // And now those that do not need no stinking channel.
         this.channel = null;
         // part, self, no reason
         test(":Tongue!ip PART #spreadbutter",
            partSelf.Fill("#spreadbutter"), "soulz");
         // part, self, a reason
         test(":Tongue!ip PART #spreadbutter :Too verklempt",
            partTotal.Fill(partSelf.Fill("#spreadbutter"), partReason.Fill("Too verklempt")),
            "verklempt");
         // quit, others, a reason
         test(":t!ip QUIT :Goodbye cruelle world",
            quitTotal.Fill(quitOthers.Fill("t", "ip"), quitReason.Fill("Goodbye cruelle world")),
            "poverty");
         // quit, others, no reason
         test(":Tongues!ip QUIT", quitOthers.Fill("Tongues", "ip"),
            "slip");
         // quit, self, a reason
         test(":Tongue!ip QUIT :Goodbye cruelle monkey",
            quitTotal.Fill(quitSelf, quitReason.Fill("Goodbye cruelle monkey")),
            "jane");
         // quit, self, no reason
         test(":Tongue!ip QUIT", quitSelf, "short");
         // should ignore
         test(":Tongue!ip MODE :+x", "estar en moda");
      }

      void test(string line, string id) {
         test(line, null, id);
      }

      void test(string line, string msg, string id) {
         if (null == msg) msg = line;

         MockApp.PokeBuffer(app, line, (ref BufferData data) => {
            if (channel == data.Channel && msg == data.Line) return;
            throw new InvalidOperationException("JoinPartQuit failure in filter() " + id);
         });
      }
   }
}