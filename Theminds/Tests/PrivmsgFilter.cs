using System;
using System.Diagnostics;
using Theminds;
using Aspirations;
using S = System.String;

namespace Theminds.Tests {
   [DesiresTestingWithMockAppAttribute]
   class Privmsg : Filters.Privmsg {
      string channel = "#spreadbutter";
      IAppControls app;
      public Privmsg(IAppControls app)
         : base(app) {
         this.app = app;
         string speechAll = App.Lion.Get("speech.all");
         string actionAll = App.Lion.Get("action.all");
         QuirkStart info = app.Connection.Info;

         // speech, others
         test(":maria!ip PRIVMSG #spreadbutter :Hello! #moto rola",
            speechAll, "maria", "Hello! #moto rola", "maria");
         // action, others
         test(":maria!ip PRIVMSG #spreadbutter :\u0001ACTION #dances !butterfly\u0001",
            actionAll, "maria", "#dances !butterfly", "butterfly");
         // speech, self
         test("PRIVMSG #spreadbutter PEOPLE WANNA DANCE #spreadbutter",
            speechAll, info.Nick, "PEOPLE WANNA DANCE #spreadbutter", "dance");
         // action, self
         test("PRIVMSG #spreadbutter :\u0001ACTION #plucks !alfalfa!\u0001",
            actionAll, info.Nick, "#plucks !alfalfa!", "alfalfa");

         this.channel = null;
         // should ignore
         test("NOTICE PRIVMSG :ooober", "ooober");
         test("PRIVMSGCARD #s :m", "mute_monster");
         test(":n!p PRIVMSGZ #s :m", "mute_samone");
      }

      void test(string line, string id) {
         test(line, null, null, null, id);
      }

      void test(string line, string template,
         string nick, string msg, string id) {
         if (null == template) template = line;
         MockApp.PokeBuffer(app, line, (ref BufferData data) => {
            if (channel == data.Channel && template.Fill(nick, msg) == data.Line) return;
            throw new InvalidOperationException("PrimvsgFilter failure in filter() " + id);
         });
      }
   }
}