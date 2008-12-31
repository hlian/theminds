// Buffering separates *Line event logic from AddLine,
// thereby allowing me to match server-channel to each
// tab (different log view for each tab).

using System;
using System.Collections.Generic;
using System.Drawing;
using Aspirations;
using M = System.Windows.Forms.MethodInvoker;

namespace Theminds {
   public class Buffer {
      IAppControls app;

      // I need the two-way-osity for AddLine.
      TwoWayDictionary<ITab, Room> proust = new TwoWayDictionary<ITab, Room>(5);
      Quirk connection;
      public Buffer(IAppControls app, Quirk quirk) {
         this.app = app;
         this.connection = quirk;

         Room id = new Room(quirk, null, app.LogBox);
         proust[app.Tabber.Current] = id;

         // Page.Buffering events.
         app.Tabber.Moved += new TabDel(MoveToTab);
      }

      // If it comes from a different thread, then the line is
      // from the server (Line event). Otherwise, it's from the user
      // (SelfLine event).
      delegate void AddLineDel(string line, Color color);
      public void AddLine(string line) {
         BufferData data = new BufferData(line);

         // The hub
         PreLine(ref data);
         if (app.InvokeRequired) Line(ref data);
         else SelfLine(ref data);
         PostLine(ref data);
         
         if (data.Ignore) return;
         if (data.BroadcastId != null)
            broadcastHelper(data);
         else {
            Room tab = new Room(this.connection, data.Channel, null);
            if (!proust.ContainsKey(tab)) app.Invoke((M)(() => AddChannel(tab)));

            // AddChannel now guarantees `tab` is inside
            // `proust`, ripe for picking. Forward & reverse
            // ensures that tab.LogBox is not null.
            tab = proust[proust[tab]];
            app.Invoke(new AddLineDel(tab.LogBox.AddLine),
                  data.Line, data.Color);
         }
      }

      private void broadcastHelper(BufferData data) {
         List<Room> tabs = proust.Values;
         Broadcast(ref tabs, data.BroadcastId);
         app.BeginInvoke((M)(
            () => tabs.ForEach(t => t.LogBox.AddLine(data.Line, data.Color))
         ));
      }

      public void AddChannel() { 
         AddChannel(new Room(this.connection, null, null)); 
      }
      public void AddChannel(Room tab) {
         app.CurrentChannel = tab.Channel;
         ITab newTab = app.Tabber.Add(tab.Channel);
         tab.LogBox = new LogBox();
         app.SwitchLogBox(tab.LogBox);

         proust[newTab] = tab;
         NewChannel(tab.Channel);
      }

      // If no key exists, `t` is a new tab.
      // AddChannelTab will handle instead.
      public void MoveToTab(ITab t) {
         if (!proust.ContainsKey(t)) return;

         Room id = proust[t];
         app.SwitchLogBox(id.LogBox);
         app.CurrentChannel = id.Channel;
      }

      public void Remove(ITab t) {
         if (proust.Count == 1) return;
         if (!proust.ContainsKey(t)) return;

         string channel = proust[t].Channel;
         proust.Remove(t);
         app.Tabber.Remove(t);
         if (channel.IsChannel())
            app.Connection.Message("PART {0}", channel);
      }

      public Room GetRoom(BufferData data) {
         return new Room(this.connection, data.Channel, null);
      }

      public event LineDel PreLine = delegate { };
      public event LineDel Line = delegate { };
      public event LineDel SelfLine = delegate { };
      public event LineDel PostLine = delegate { };
      public event Action<String> NewChannel = delegate { };
      public event BroadcastDel Broadcast = delegate { };
   }

   public delegate void LineDel(ref BufferData data);
   public delegate void BroadcastDel(ref List<Room> tabs, string id);
}