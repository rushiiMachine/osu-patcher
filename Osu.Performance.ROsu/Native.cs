using System.Runtime.InteropServices;

namespace Osu.Performance.ROsu;

// TODO: rename dll to rosu.ffi.dll
internal class Native
{
    [DllImport("rosu.dll", EntryPoint = "calculate_osu_performance")]
    public static extern double CalculateOsuPerformance(
        ref OsuDifficultyAttributes difficulty,
        ref OsuScoreState score,
        out OsuPerformanceInfo calculatedPerformance
    );
}