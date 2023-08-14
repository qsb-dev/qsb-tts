using HarmonyLib;
using QSB.Patches;
using QSB.HUD;

namespace QSBTTS.Patches;

[HarmonyPatch(typeof(MultiplayerHUDManager))]
internal class MultiplayerHUDManagerPatches : QSBPatch
{
	public override QSBPatchTypes Type => QSBPatchTypes.OnModStart;

	[HarmonyPrefix]
	[HarmonyPatch(nameof(MultiplayerHUDManager.WriteSystemMessage))]
	public static void WriteSystemMessage(string message)
	{
		Core.SendToTTS(message, ITTSAPI.TTSVoice.Betty);
	}
}