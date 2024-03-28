using System.Reflection;
using HarmonyLib;
using OsuHook.Osu.Stubs;

// ReSharper disable UnusedType.Global UnusedMember.Local

namespace OsuHook.Patches.ShowComboBreaks
{
    /// <summary>
    ///     Clear our stored slider hit judgement results when the score is reset.
    /// </summary>
    [HarmonyPatch]
    internal class PatchTrackScoreClear : BasePatch
    {
        [HarmonyTargetMethod]
        private static MethodBase Target() => Ruleset.ResetScore.Reference;

        [HarmonyPostfix]
        private static void After() => ShowComboBreaksUtil.Reset();
    }
}