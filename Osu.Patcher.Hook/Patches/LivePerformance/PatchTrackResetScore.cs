using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks <c>Ruleset::ResetScore()</c> to also reset our performance calculator.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class PatchTrackResetScore
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Ruleset.ResetScore.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    private static void After() => PerformanceCalculator.ResetCalculator();

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchTrackResetScore)}: {__exception}");
        }
    }
}