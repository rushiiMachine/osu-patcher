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
    private static MethodBase Target() => ScoreProcessor.AddScoreChange.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void After([HarmonyArgument(0)] object scoreChange, object __instance)
    {
        const int HitScoreMask = IncreaseScoreType.Osu300 | IncreaseScoreType.Osu100 | IncreaseScoreType.Osu50;

        var maxCombo = ScoreProcessor.MaximumCombo.Get(__instance);
        var hitScoreResult = ScoreChange.HitValue.Get(scoreChange);

        // Console.WriteLine($"IncreaseScoreHit: {hitScoreResult} {hitScoreResult & HitScoreMask}");
        OsuJudgementResult? judgement = (hitScoreResult & HitScoreMask) switch
        {
            IncreaseScoreType.Osu300 => OsuJudgementResult.Result300,
            IncreaseScoreType.Osu100 => OsuJudgementResult.Result100,
            IncreaseScoreType.Osu50 => OsuJudgementResult.Result50,
            _ => null,
        };

        if (hitScoreResult < 0)
        {
            judgement = OsuJudgementResult.ResultMiss;
        }
        else if (!judgement.HasValue)
        {
            return;
        }

        _performance.AddJudgement(judgement.Value, (ulong)maxCombo);
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
    [HarmonyTargetMethod]
    private static MethodBase Target() => ScoreProcessor.Clear.Reference;

    [HarmonyPostfix]
    private static void After()
    {
        Console.WriteLine("Clearing performance calculator!");
        PatchUpdatePerformanceCalculator.ResetCalculator();
    }
}