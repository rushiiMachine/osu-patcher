using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance;
using Osu.Stubs.GameModes.Play.Rulesets;
using Osu.Stubs.GameplayElements.Scoring;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks <c>Ruleset::OnIncreaseScoreHit(...)</c> to send score updates to our performance calculator
///     so it can recalculate based on new HitObject judgements.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class TrackOnScoreHit
{
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
        if (!PerformanceOptions.ShowPerformanceInGame.Value)
            return;

        if (!PerformanceCalculator.IsInitialized)
        {
            Debug.Fail("OnIncreaseScoreHit called before performance calculator initialized!");
            return;
        }

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

        Task.Run(() => PerformanceCalculator.Calculator?.AddJudgement(judgement, (uint)MaxCombo));
    }
}