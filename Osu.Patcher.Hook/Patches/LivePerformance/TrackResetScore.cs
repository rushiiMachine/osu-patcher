using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks <c>Ruleset::ResetScore()</c> to also reset our performance calculator and caches.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class TrackResetScore
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
            Console.WriteLine($"Exception due to {nameof(TrackResetScore)}: {__exception}");
        }
    }
}