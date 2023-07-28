using System.Diagnostics;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Epic.OnlineServices;
using OWML.ModHelper;
using QSB.Utility;

namespace QSBTTS
{
    public class Core : ModBehaviour
    {
	    public static Socket Socket;

	    public void Start()
	    {
		    var process = new Process();
			DebugLog.DebugWrite(ModHelper.Manifest.ModFolderPath + "SharpTalk-Program.exe");
			process.StartInfo.FileName = ModHelper.Manifest.ModFolderPath + "SharpTalk-Program.exe";
			process.Start();

			ChildProcessTracker.AddProcess(process);

			var host = Dns.GetHostEntry("localhost");
		    var ipAddress = host.AddressList[0];
		    var endPoint = new IPEndPoint(ipAddress, 11000);

		    Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		    Socket.Connect(endPoint);
		    DebugLog.DebugWrite($"Socket connected to {Socket.RemoteEndPoint}");

		    var msg = Encoding.UTF8.GetBytes("JOHN MADDEN");
			Socket.Send(msg);
	    }
    }
}
