using System.Collections;
using System.Diagnostics;
using System.IO.Pipes;
using NAudio.Wave;
using Newtonsoft.Json;
using OWML.Common;
using OWML.ModHelper;
using QSB;
using QSB.Utility;

namespace QSBTTS
{
    public class Core : ModBehaviour
    {
	    public static ITTSAPI.TTSVoice Voice = ITTSAPI.TTSVoice.Paul;

	    private static StreamWriter _writer;
	    private static NamedPipeClientStream _client;

		public override object GetApi() => new TTSAPI();

		public void Start()
		{
			QSBCore.RegisterNotRequiredForAllPlayers(this);

		    ChildProcessTracker.AddProcess(StartTTSProgram());

			Delay.RunFramesLater(60, () =>
			{
				_client = new NamedPipeClientStream("QSBTTS");
				_client.Connect(1000);
				_writer = new StreamWriter(_client);
			});

			var threadStart = new ThreadStart(CheckFiles);
			var backgroundThread = new Thread(threadStart);
			backgroundThread.IsBackground = true;
			backgroundThread.Start();
		}

		void CheckFiles()
		{
			var folderToCheck = Path.Combine(ModHelper.Manifest.ModFolderPath, "audiofiles");
			foreach (var file in Directory.GetFiles(folderToCheck))
			{
				File.Delete(file);
			}

			while (true)
			{
				var files = Directory.GetFiles(folderToCheck, "*.wav");
				foreach (var file in files)
				{
					var bytes = File.ReadAllBytes(file);
					File.Delete(file);
					var waveoutevent = new WaveOutEvent();
					var provider = new RawSourceWaveStream(new MemoryStream(bytes), new WaveFormat(11025, 1));
					waveoutevent.Init(provider);
					waveoutevent.Play();
				}
			}
		}

		public static void SendToTTS(string message, ITTSAPI.TTSVoice voice)
	    {
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
		    var process = new Process();
		    process.StartInfo.FileName = ModHelper.Manifest.ModFolderPath + "SharpTalk-Program.exe";
		    process.Start();
		    return process;
	    }

	    public override void Configure(IModConfig config)
	    {
		    Voice = config.GetSettingsValue<ITTSAPI.TTSVoice>("TTS Voice");
	    }
    }

	public class TTSData
    {
	    public string text;
	    public uint voice;
    }
}
