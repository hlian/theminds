using System;
using System.Collections.Generic;

namespace Theminds {
  [DesiresAppControls]
  public class Users : Dictionary<Room, List<string>> {
    IAppControls app;
    public static Users Instance;
    public Users(IAppControls app)
      : base(5) {
      this.app = app;
      Users.Instance = this;

      // Only broadcast quit messages to the channels
      // that actually contain the quitting user.
      // The `id` is "quit.[user]". See JPQ.cs.
      app.Buffer.Broadcast += (ref List<Room> tabs, string id) => {
        if (!id.StartsWith("quit.")) return;
        string quitter = id.Split('.')[1];
        // Todo: Handle @+% fun via custom Contains method
        tabs = tabs.FindAll(t => Users.Instance[t].Contains(quitter));
      };
    }

    public void Clear(BufferData data) {
      this[app.Buffer.GetRoom(data)] = new List<string>(20);
    }

    public new List<string> this[Room index] {
      get {
        if (!this.ContainsKey(index)) return new List<string>();
        else return base[index];
      }
      set { base[index] = value; }
    }

    public string this[BufferData index] {
      set {
        Room room = app.Buffer.GetRoom(index);
        if (this.ContainsKey(room)) this[room].Add(value);
        else {
          this[room] = new List<string>(20);
          this[room].Add(value);
        }
      }
    }
  }
}