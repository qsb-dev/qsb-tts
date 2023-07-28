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

		var bytes = Encoding.UTF8.GetBytes(message);
		Core.Socket.Send(bytes);
	}
}
