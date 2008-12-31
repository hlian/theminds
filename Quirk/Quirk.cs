// Core server logic for Theminds and IRC client.
// She takes a QuirkStart for input and outputs via the Line event.

using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Reflection.AssemblyVersionAttribute("1.3")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.Permissions.SecurityPermission(
  System.Security.Permissions.SecurityAction.RequestMinimum, Execution = true)]
namespace Aspirations {
  public sealed partial class Quirk : IDisposable {
    public QuirkStart Info;
    static Random rnd = new Random();

    // Everytime Connection has a new line, we send it to this event.
    // NOT Buffer's NewLine. This merely glues Connection and LogBox.
    public delegate void NewLineDel(string line);
    public event NewLineDel NewLine;

    bool dnsResolved;
    public Quirk(QuirkStart connectionInfo) {
      this.Info = connectionInfo;
      dnsResolved = false;

      try {
        var x = Dns.GetHostEntry(Info.Server).AddressList;
        this.Info.Server = x[rnd.Next(x.Length)].ToString();
        dnsResolved = true;
      }
      catch (SocketException) {
        NewLine("Could not resolve {0}.".Fill(Info.Server));
      }
    }

    Thread connectThread;
    public bool Started = false;
    public void Start() {
      if (this.Started || !dnsResolved) return;

      var x = new ThreadStart(connect);
      connectThread = new Thread(x) { IsBackground = true };
      connectThread.Start();
      this.Started = true;
    }

    public void Message(string line) {
      if (Started) {
        NewLine(line);
        writer.WriteLine(line);
      }
      else {
        NewLine("Unsent line: " + line);
      }
    }

    public void Message(string line, params object[] args) {
      Message(String.Format(line, args));
    }

    // IDisposable with the twist in that it takes an
    // IRC quit message. I live on the edge.
    bool disposed = false;
    public void Dispose() { Dispose(null); }
    public void Dispose(string quitMsg) {
      if (disposed || null == connectThread) return;
      disposed = true;
      if (null == writer) return;

      try {
        if (null == quitMsg) Message("QUIT");
        else Message("QUIT " + quitMsg);
      }
      catch (IOException e) {
        NewLine(e.ToString());
      }
    }

    /**** Private members ****/
    StreamWriter writer;
    StreamReader reader;
    void connect() {
      Stream stream;
      try {
        var x = new TcpClient(Info.Server, Info.Port);
        stream = x.GetStream();
      }
      catch (SocketException) {
        var msg = "Could not connect to \"{0}\".".Fill(Info.Server);
        NewLine(msg); return;
      }

      reader = new StreamReader(stream);
      writer = new StreamWriter(stream);
      writer.AutoFlush = true;

      Message("NICK {0}\n{1}", Info.Nick, Info.User);

      // Start the event pump and keep it running until we have
      // to Dispose() at the end of the application runtime, at
      // which point end _very, very carefully_ and quickly.
      while (!disposed) pump();
      writer.Dispose(); reader.Dispose();
    }

    void pump() {
      string line = null;
      Action<Exception> h = e => {
        NewLine(e.ToString());
        this.Dispose();
      };

      try { line = reader.ReadLine(); }
      catch (IOException e) { h(e); }
      catch (OutOfMemoryException e) { h(e); }

      if (line != null) NewLine(line);
    }

    public override int GetHashCode() {
      return Info.GetHashCode();
    }

    public override bool Equals(object obj) {
      if (obj == null) return false;
      if (!(obj is Quirk)) return false;
      var q = (Quirk)obj;
      return (this.Info.Equals(q.Info));
    }
  }
}