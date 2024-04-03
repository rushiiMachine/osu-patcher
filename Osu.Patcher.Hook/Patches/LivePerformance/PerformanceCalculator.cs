using System;
using System.Diagnostics;
using Osu.Performance;
using Osu.Stubs.GameModes.Play;
using Osu.Stubs.GameplayElements.Beatmaps;
using Osu.Stubs.GameplayElements.Scoring;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Handles initializing and disposing the performance calculator based on current score and settings.
/// </summary>
internal static class PerformanceCalculator
{
    public static OsuPerformance? Calculator { get; private set; }

    public static bool IsInitialized => Calculator != null;

    /// <summary>
    ///     Disposes the existing performance calculator, and initializes a new one according to the current score info.
    ///     This must be called after <c>Ruleset::ResetScore()</c>
    /// </summary>
    public static void ResetCalculator()
    {
        try
        {
            ResetCalculatorSync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to reset performance calculator: {e}");
        }
    }

    private static void ResetCalculatorSync()
    {
        Debug.WriteLine("Resetting performance calculator", nameof(PerformanceCalculator));

        // TODO: don't dispose it if the beatmap is the same, reuse parsed beatmap in native side
        Calculator?.Dispose();
        Calculator = null;

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

        Calculator = new OsuPerformance(beatmapPath, (uint)mods);
        Calculator.OnNewCalculation += PerformanceDisplay.UpdatePerformanceCounter;

        Debug.WriteLine("Initialized performance calculator!", nameof(PerformanceCalculator));
    }
}