using Newtonsoft.Json;
using SharpTalk;
using System;
using System.IO;
using System.IO.Pipes;

namespace SharpTalk_Program;
internal class Program
{
	private static NamedPipeServerStream _server;
	private static StreamReader _reader;

	static void Main(string[] args)
	{
		Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

		Console.WriteLine("Creating server and waiting for connection....");
		_server = new NamedPipeServerStream("QSBTTS", PipeDirection.InOut, 1, PipeTransmissionMode.Message);
		_server.WaitForConnection();
		Console.WriteLine("Connected to mod!");
		_reader = new StreamReader(_server);

		while (true)
		{
			var line = _reader.ReadLine();
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

		engine.SpeakToWavFile($"audiofiles/{Guid.NewGuid()}.wav", data.text);
	}
}

public class TTSData
{
	public string text;
	public uint voice;
}
