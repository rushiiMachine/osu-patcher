using System;
using System.Runtime.InteropServices;

namespace Osu.Performance.ROsu;

internal static class Native
{
    [DllImport("rosu.dll", EntryPoint = "calculate_osu_performance")]
    internal static extern double CalculateOsuPerformance(
        ref OsuDifficultyAttributes difficulty,
        ref OsuScoreState score,
        uint mods,
        out OsuPerformanceInfo calculatedPerformance
    );

    [DllImport("rosu.dll",
        EntryPoint = "initialize_osu_performance_gradual",
        CharSet = CharSet.Ansi,
        SetLastError = true)]
    internal static extern IntPtr InitializeOsuGradualPerformance(
        string mapPath,
        uint mods
    );

    [DllImport("rosu.dll", EntryPoint = "calculate_osu_performance_gradual")]
    internal static extern double CalculateGradualOsuPerformance(
        IntPtr state, // This is not thread safe!
        OsuJudgementResult newJudgement,
        ulong maxCombo
    );

    // This is not thread safe!
    [DllImport("rosu.dll", EntryPoint = "dispose_osu_performance_gradual")]
    internal static extern void DisposeGradualOsuPerformance(
        IntPtr state // This is not thread safe!
    );
}