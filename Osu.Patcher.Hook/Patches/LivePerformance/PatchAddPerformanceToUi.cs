using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks the constructor of <c>ScoreDisplay</c> to add our own <c>pTextSprite</c> for displaying
///     the performance counter to the ScoreDisplay's sprite manager.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
public class PatchAddPerformanceToScoreDisplay
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => ScoreDisplay.Constructor.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void After(
        object __instance, // ScoreDisplay
        [HarmonyArgument(0)] object spriteManager, // SpriteManager
        [HarmonyArgument(1)] object position, // Vector2
        [HarmonyArgument(2)] bool alignRight,
        [HarmonyArgument(3)] float scale,
        [HarmonyArgument(4)] bool showScore,
        [HarmonyArgument(5)] bool showAccuracy
    )
    {
        var positionX = Vector2.X.Get(position);

        // TODO: use correct position
        var startPosition = ((ConstructorInfo)Vector2.Constructor.Reference).Invoke([positionX, 0f]);

        var performanceSprite = ((ConstructorInfo)pSpriteText.Constructor.Reference).Invoke(
        [
            /* text: */ "0000.0",
            /* fontName: */ "Assets/score/score",
            /* spacingOverlap: */ 10f,
            /* fieldType: */ alignRight ? Fields.TopRight : Fields.TopLeft,
            /* origin: */ alignRight ? Origins.TopRight : Origins.TopLeft,
            /* clock: */ Clocks.Game,
            /* startPosition: */ startPosition,
            /* drawDepth: */ 0.95f,
            /* alwaysDraw: */ true,
            /* color: */ Color.White, // TODO: try GhostWhite
            /* precache: */ true,
            /* source: */ SkinSource.All,
        ]);

        SpriteManager.Add.Invoke(spriteManager, [performanceSprite]);
        PerformanceDisplay.SetPerformanceCounter(performanceSprite);
    }

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchAddPerformanceToScoreDisplay)}: {__exception}");
        }
    }
}