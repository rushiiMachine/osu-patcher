using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

[HarmonyPatch]
[UsedImplicitly]
internal class PatchClearPerformanceCalculator
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Ruleset.ResetScore.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    private static void After() => PatchUpdatePerformanceCalculator.ResetCalculator();

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchClearPerformanceCalculator)}: {__exception}");
        }
    }
}