using System.Text;
using HarmonyLib;
using QSB.HUD.Messages;
using QSB.Patches;
using QSB.Player;

namespace QSBTTS.Patches;

[HarmonyPatch(typeof(ChatMessage))]
internal class ChatMessagePatches : QSBPatch
{
	public override QSBPatchTypes Type => QSBPatchTypes.OnModStart;

	[HarmonyPrefix]
	[HarmonyPatch(nameof(ChatMessage.OnReceiveRemote))]
	public static void OnReceiveRemote(ChatMessage __instance)
	{
		var message = __instance.Data.message;

		var playerName = QSBPlayerManager.GetPlayer(__instance.From).Name;
		var prefixLength = $"{playerName}: ".Length;
		message = message.Substring(prefixLength);

		message = ProcessMessage(message);

		Core.SendToTTS(message, ITTSAPI.TTSVoice.Paul);
	}

	private static string ProcessMessage(string message)
	{
		// do some stuff here to make OW's words be pronounced properly.
		// this should be done by creating a custom dictionary, but
		// i have no idea how to do that.

		message = message.Replace(" nomai ", " no-my ");

		return message;
	}
}
