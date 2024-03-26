using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance.ROsu;
using Osu.Stubs;
using Osu.Stubs.Opcode;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

[HarmonyPatch]
[UsedImplicitly]
public static class PatchUpdatePerformanceCalculator
{
    private static OsuPerformance? _performance;

    internal static void ResetCalculator()
    {
        _performance?.Dispose();
        _performance = null;

        var currentScore = Player.CurrentScore.Get();
        if (currentScore == null) return;

        var beatmap = Score.Beatmap.Get(currentScore);
        if (beatmap == null) return;

        var beatmapSubPath = Beatmap.GetBeatmapPath(beatmap);
        if (beatmapSubPath == null) return;

        var osuDir = Path.GetDirectoryName(OsuAssembly.Assembly.Location)!;
        var beatmapPath = Path.Combine(osuDir, "Songs", beatmapSubPath);

        _performance = new OsuPerformance(beatmapPath, 0);
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
        if (_performance == null) return;

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