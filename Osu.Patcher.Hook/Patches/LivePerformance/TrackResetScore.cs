using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Rulesets;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks <c>Ruleset::ResetScore()</c> to also reset our performance calculator.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class TrackResetScore
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Ruleset.ResetScore.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    private static void After() => PerformanceCalculator.ResetCalculator();
}