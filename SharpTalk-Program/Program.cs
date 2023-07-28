using SharpTalk;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharpTalk_Program;
internal class Program
{
	private static Socket _socket;
	private static TcpListener _server;
	private static int _port;

	static void Main(string[] args)
	{
		StartServer();
	}

	public static void StartServer()
	{
		var host = Dns.GetHostEntry("localhost");
		var ipAddress = host.AddressList[0];
		var localEndPoint = new IPEndPoint(ipAddress, 11000);

		try
		{
			var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			listener.Bind(localEndPoint);
			listener.Listen(10);

			Console.WriteLine("Waiting for a connection...");
			_socket = listener.Accept();

			string data = null;
			byte[] bytes = null;

			while (true)
			{
				bytes = new byte[1024];

				var iRx = _socket.Receive(bytes);
				var chars = new char[iRx];
				var d = Encoding.UTF8.GetDecoder();
				d.GetChars(bytes, 0, iRx, chars, 0);
				var recv = new string(chars);

				var engine = new FonixTalkEngine();
				engine.Speak(recv);
				Console.WriteLine(recv);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
}
