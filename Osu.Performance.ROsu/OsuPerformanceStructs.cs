using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Osu.Performance.ROsu;

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

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum OsuJudgement : byte
{
    None = 0,
    Result300 = 1,
    Result100 = 2,
    Result50 = 3,
    ResultMiss = 4,
}