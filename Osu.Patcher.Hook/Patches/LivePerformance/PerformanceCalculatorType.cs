using System;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     The type of performance counter display to show in UI.
/// </summary>
[Serializable]
public enum PerformanceCalculatorType
{
    /// <summary>
    ///     Use Bancho pp calculations.
    /// </summary>
    Bancho,

    /// <summary>
    ///     Use Akatsuki pp calculations.
    /// </summary>
    Akatsuki,

    /// <summary>
    ///     Use Akatsuki pp calculations when either Relax or Autopilot is enabled, and Bancho otherwise.
    /// </summary>
    AkatsukiLimited,
}