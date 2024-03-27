using System;
using System.Reflection;
using HarmonyLib;
using Osu.Stubs;

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
            var ruleset = Ruleset.Instance.Get();

            var scoreDisplay = Ruleset.ScoreDisplay.Get(ruleset);
            if (scoreDisplay == null) return;

            if (!PerformanceCounter.TryGetTarget(out var sprite))
                return;

            pText.SetText.Invoke(sprite, [$"{pp:00.0}pp"]);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to set performance counter sprite text: {e}");
        }
    }
}

// [HarmonyPatch]
// public class fhjks
// {
//     static fhjks()
//     {
//         Console.WriteLine("sinit");
//     }
//     
//     [HarmonyTargetMethod]
//     private static MethodBase fjhsdf() => pSpriteText.Constructor.Reference;
//
//     [HarmonyPrefix]
//     private static void before(
//         [HarmonyArgument(0)] string text,
//         [HarmonyArgument(1)] string abc,
//         [HarmonyArgument(2)] float abcd
//     )
//     {
//         Console.WriteLine($"{text} {abc} {abcd}");
//     }
// }