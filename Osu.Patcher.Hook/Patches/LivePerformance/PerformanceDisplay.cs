using System;
using Osu.Stubs.Framework;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

internal static class PerformanceDisplay
{
    /// <summary>
    ///     The last known patched instance of our <c>pSpriteText</c> performance counter sprite.
    /// </summary>
    private static readonly WeakReference<object?> PerformanceCounter = new(null);

    /// <summary>
    ///     Set a new active performance counter to update.
    /// </summary>
    /// <param name="sprite">The <c>pSpriteText</c> performance counter sprite.</param>
    public static void SetPerformanceCounter(object sprite) =>
        PerformanceCounter.SetTarget(sprite);

    /// <summary>
    ///     Change the pp value for the currently active performance counter.
    /// </summary>
    public static void UpdatePerformanceCounter(double pp)
    {
        try
        {
            if (!PerformanceCounter.TryGetTarget(out var sprite) || sprite == null)
                return;

            // Technically this should be run with "GameBase.Scheduler.AddOnce(() => ...)" but it works anyways, so...
            pText.SetText.Invoke(sprite, [$"{pp:00.0}pp"]);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to set performance counter sprite text: {e}");
        }
    }
}