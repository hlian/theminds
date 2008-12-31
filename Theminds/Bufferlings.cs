using Aspirations;
using Color = System.Drawing.Color;

namespace Theminds {
   // Essentially captures the necessary information
   // to pinpoint a single channel, in preparation
   // for multiserver days.
   public struct Room {
      public Quirk Connection;
      public string Channel;
      public LogBox LogBox;

      public Room(Quirk c, string channel, LogBox l) {
         this.Connection = c;
         this.Channel = channel;
         this.LogBox = l;
      }

      public override int GetHashCode() {
         if (Channel == null) return 42;
         else return Channel.GetHashCode();
      }

      public override bool Equals(object obj) {
         if (obj == null) return false;
         if (!(obj is Room)) return false;
         Room o = (Room)obj;
         return (o.Connection == this.Connection) &&
            (o.Channel == this.Channel);
      }
   }

   public struct BufferData {
      public Color Color;
      public string Channel;
      public string Line;
      public bool Ignore;
      public string BroadcastId;
      public BufferData(string line) {
         this.Line = line;
         this.Color = Color.Black;
         this.Channel = null;
         this.Ignore = false;
         this.BroadcastId = null;
      }
   }
}