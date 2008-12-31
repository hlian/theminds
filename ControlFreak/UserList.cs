using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace Aspirations {
  public class UserList : ListBox {
    System.Collections.Generic.List<string> itemsBuffer;
    MethodInvoker flushDel;
    public UserList() {
      itemsBuffer = new List<string>();
      flushDel = new MethodInvoker(Flush);
    }

    public void Push(string user) { itemsBuffer.Add(user); }
    public void Flush() {
      if (InvokeRequired) { this.Invoke(flushDel); return; }
      Items.Clear(); Items.AddRange(itemsBuffer.ToArray());
      itemsBuffer.Clear();
    }
  }
}