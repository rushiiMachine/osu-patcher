using System.Runtime.InteropServices;

namespace Osu.Performance.ROsu;

public class OsuPerformance(OsuDifficultyAttributes difficulty)
{
    /// <summary>
    ///     Calculates a score while returning the complete info.
    ///     If this is a failed score, or in progress for whatever reason, then the end of the score will be
    ///     calculated based on the sum of the amount of hits recorded in <paramref name="score" />.
    /// </summary>
    /// <param name="score">A score that's been played on a map representing the difficulty attributes of this calculator.</param>
    public OsuPerformanceInfo CalculateScore(OsuScoreState score)
    {
        Native.CalculateOsuPerformance(ref difficulty, ref score, out var performance);
        return performance;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuDifficultyAttributes
{
    public double Stars;
    public ulong MaxCombo;
    public double SpeedNoteCount;

    public double ApproachRate;
    public double OverallDifficulty;
    public double HealthRate;

    public double AimSkill;
    public double SpeedSkill;
    public double FlashlightSkill;
    public double SliderSkill;

    public ulong CircleCount;
    public ulong SliderCount;
    public ulong SpinnerCount;
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuScoreState
{
    public ulong ScoreMaxCombo;
    public ulong Score300s;
    public ulong Score100s;
    public ulong Score50s;
    public ulong ScoreMisses;
}

[StructLayout(LayoutKind.Sequential)]
public struct OsuPerformanceInfo
{
    public double TotalPP;
    public double AimPP;
    public double SpeedPP;
    public double AccuracyPP;
    public double FlashlightPP;
    public double EffectiveMissCount;
}