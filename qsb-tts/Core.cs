using System.Diagnostics;
using System.IO.Pipes;
using Newtonsoft.Json;
using OWML.Common;
using OWML.ModHelper;
using QSB;
using QSB.Utility;

namespace QSBTTS
{
    public class Core : ModBehaviour
    {
	    private static StreamWriter _writer;
		private static StreamReader _reader;
		private static NamedPipeClientStream _client;

	    public void Start()
	    {
		    QSBCore.RegisterNotRequiredForAllPlayers(this);

		    ChildProcessTracker.AddProcess(StartTTSProgram());

			Delay.RunFramesLater(60, () =>
			{
				DebugLog.DebugWrite($"Creating client and connecting...");
				_client = new NamedPipeClientStream("QSBTTS");
				_client.Connect(1000);
				DebugLog.DebugWrite($"Creating reader and writer...");
				_reader = new StreamReader(_client);
				_writer = new StreamWriter(_client);
			});

			Task.Factory.StartNew(() =>
		    {
			    while (true)
			    {
				    if (_reader == null)
				    {
						continue;
				    }
				    /*string input = "this is a test!";
				    if (String.IsNullOrEmpty(input))
					    break;
				    writer.WriteLine(input);
				    writer.Flush();*/
				    //DebugLog.DebugWrite(_reader.ReadLine());
					/*var line = _reader.ReadLineAsync();
					if (!string.IsNullOrWhiteSpace(line))
					{
						DebugLog.DebugWrite(line);
					}*/
			    }
			});
	    }

	    public static void SendToTTS(string message, ITTSAPI.TTSVoice voice)
	    {
		    if (_writer == null)
		    {
				DebugLog.DebugWrite("_writer is null!", MessageType.Error);
		    }

			DebugLog.DebugWrite($"Sending \"{message}\" in voice of {voice}");

			var data = new TTSData()
			{
				text = message,
				voice = (uint)voice
			};

			_writer.WriteLine(JsonConvert.SerializeObject(data));
			_writer.Flush();
	    }

	    private Process StartTTSProgram()
	    {
		    DebugLog.DebugWrite($"Start TTS Program");
		    var process = new Process();
		    process.StartInfo.FileName = ModHelper.Manifest.ModFolderPath + "SharpTalk-Program.exe";
		    process.Start();
		    return process;
	    }
    }

	public class TTSData
    {
	    public string text;
	    public uint voice;
    }
}
