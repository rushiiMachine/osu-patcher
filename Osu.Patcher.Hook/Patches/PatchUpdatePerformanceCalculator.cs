using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance.ROsu;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches;

[HarmonyPatch]
[UsedImplicitly]
public static class PatchUpdatePerformanceCalculator
{
    private static OsuPerformance _performance = new(
        @"C:\osu!\Songs\1099140 Laszlo - Linus tech tips intro\Laszlo - Linus tech tips intro (Sytho) [Tragic love Ultra].osu",
        0
    );

    static PatchUpdatePerformanceCalculator()
    {
        _performance.OnNewCalculation += Console.WriteLine;
    }

    public static void ResetCalculator()
    {
        _performance.Dispose();
        _performance = new OsuPerformance(
            @"C:\osu!\Songs\1099140 Laszlo - Linus tech tips intro\Laszlo - Linus tech tips intro (Sytho) [Tragic love Ultra].osu",
            0
        );
        _performance.OnNewCalculation += Console.WriteLine;
    }

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Ruleset.OnIncreaseScoreHit.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void After(
        object __instance, // is Ruleset
        [HarmonyArgument(0)] int increaseScoreType,
        [HarmonyArgument(2)] bool increaseCombo)
    {
        const int HitScoreMask = IncreaseScoreType.Osu300 |
                                 IncreaseScoreType.Osu100 |
                                 IncreaseScoreType.Osu50 |
                                 IncreaseScoreType.MissBit;

        var judgement = (increaseScoreType & HitScoreMask) switch
        {
            IncreaseScoreType.Osu300 => OsuJudgement.Result300,
            IncreaseScoreType.Osu100 => OsuJudgement.Result100,
            IncreaseScoreType.Osu50 => OsuJudgement.Result50,
            IncreaseScoreType.MissBit => OsuJudgement.ResultMiss,
            _ => OsuJudgement.None,
        };

        // If this can't increase the max combo and it doesn't change the judgement counts
        // then skip this since it can't alter the pp values.
        // TODO: fix issue in rosu-ffi
        // if (!increaseCombo && judgement == OsuJudgement.None) return;
        if (judgement == OsuJudgement.None) return;

        var CurrentScore = Ruleset.CurrentScore.Get(__instance);
        var MaxCombo = Score.MaxCombo.Get(CurrentScore);

        _performance.AddJudgement(judgement, (uint)MaxCombo);
    }

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchUpdatePerformanceCalculator)}: {__exception}");
        }
    }
}

[HarmonyPatch]
internal class PatchClearPerformanceCalculator
{
    // TODO Ruleset#ResetScore(bool)
    [HarmonyTargetMethod]
    private static MethodBase Target() => ScoreProcessor.Clear.Reference;

    [HarmonyPostfix]
    private static void After()
    {
        Console.WriteLine("Clearing performance calculator!");
        PatchUpdatePerformanceCalculator.ResetCalculator();
    }
}