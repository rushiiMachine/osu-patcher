using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

[HarmonyPatch]
[UsedImplicitly]
public static class PatchUpdatePerformanceCalculator
{
    private static OsuPerformance? _performance;

    internal static void ResetCalculator() => new Thread(() =>
    {
        Debug.WriteLine("Resetting performance calculator");

        _performance?.Dispose();
        _performance = null;

        var currentScore = Player.CurrentScore.Get();
        if (currentScore == null) return;

        var modsObfuscated = Score.EnabledMods.Get(currentScore);
        var mods = Score.EnabledModsGetValue.Invoke(modsObfuscated);

        // Clear relax mod for now (live pp calculations for relax are fucking garbage)
        mods &= ~(1 << 7);

        var beatmap = Score.Beatmap.Get(currentScore);
        if (beatmap == null) return;

        var beatmapPath = Beatmap.GetBeatmapPath(beatmap);
        if (beatmapPath == null) return;

        _performance = new OsuPerformance(beatmapPath, (uint)mods);
        _performance.OnNewCalculation += Console.WriteLine;
    }).Start();

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

        var judgement = (increaseScoreType & ~IncreaseScoreType.OsuComboModifiers) switch
        {
            IncreaseScoreType.Osu300 => OsuJudgement.Result300,
            IncreaseScoreType.Osu100 => OsuJudgement.Result100,
            IncreaseScoreType.Osu50 => OsuJudgement.Result50,
            IncreaseScoreType.OsuMiss => OsuJudgement.ResultMiss,
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