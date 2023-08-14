using Newtonsoft.Json;
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
	private static NamedPipeServerStream _server;

	static void Main(string[] args)
	{
		Console.WriteLine("Creating server and waiting for connection....");
		_server = new NamedPipeServerStream("QSBTTS");
		_server.WaitForConnection();
		Console.WriteLine("Connected to mod!");
		StreamReader reader = new StreamReader(_server);
		StreamWriter writer = new StreamWriter(_server);
		while (true)
		{
			var line = reader.ReadLine();
			ProcessMessage(line);
		}
	}

	private static void ProcessMessage(string json)
	{
		Console.WriteLine($"Received \"{json}\"");

		var data = JsonConvert.DeserializeObject<TTSData>(json);

		var engine = new FonixTalkEngine
		{
			Voice = (TtsVoice)data.voice
		};

		engine.Speak(data.text);
	}
}

public class TTSData
{
	public string text;
	public uint voice;
}
