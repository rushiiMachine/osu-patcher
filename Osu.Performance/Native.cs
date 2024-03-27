using System;
using System.Runtime.InteropServices;

namespace Osu.Performance;

public static class Native
{
    [DllImport("rosu.ffi.dll", EntryPoint = "calculate_osu_performance")]
    internal static extern OsuPerformanceInfo CalculateOsuPerformance(
        ref OsuDifficultyAttributes difficulty,
        ref OsuScoreState score,
        uint mods
    );

    [DllImport("rosu.ffi.dll",
        EntryPoint = "initialize_osu_performance_gradual",
        CharSet = CharSet.Ansi,
        SetLastError = true)]
    internal static extern IntPtr InitializeOsuGradualPerformance(
        string mapPath,
        uint mods
    );

    [DllImport("rosu.ffi.dll", EntryPoint = "calculate_osu_performance_gradual")]
    internal static extern double CalculateGradualOsuPerformance(
        IntPtr state, // This is not thread safe!
        OsuJudgement judgement,
        ulong maxCombo
    );

    // This is not thread safe!
    [DllImport("rosu.ffi.dll", EntryPoint = "dispose_osu_performance_gradual")]
    internal static extern void DisposeGradualOsuPerformance(
        IntPtr state // This is not thread safe!
    );
}