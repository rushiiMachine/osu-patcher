using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.LivePerformance;

/// <summary>
///     Hooks the constructor of <c>ScoreDisplay</c> to add our own <c>pTextSprite</c> for displaying
///     the performance counter to the ScoreDisplay's sprite manager.
///     This needs score-p@2x.png or score-p.png in your skin's score font assets!
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
        [HarmonyArgument(3)] float scale
    )
    {
        var currentSkin = SkinManager.Current.Get();
        var scoreFont = SkinOsu.FontScore.Get(currentSkin);
        var scoreFontOverlap = SkinOsu.FontScoreOverlap.Get(currentSkin);

        var performanceSprite = ((ConstructorInfo)pSpriteText.Constructor.Reference).Invoke(
        [
            /* text: */ "00.0pp",
            /* fontName: */ scoreFont,
            /* spacingOverlap: */ (float)scoreFontOverlap,
            /* fieldType: */ alignRight ? Fields.TopRight : Fields.TopLeft,
            /* origin: */ alignRight ? Origins.TopRight : Origins.TopLeft,
            /* clock: */ Clocks.Game,
            /* startPosition: */ ((ConstructorInfo)Vector2.Constructor.Reference).Invoke([0f, 0f]),
            /* drawDepth: */ 0.95f,
            /* alwaysDraw: */ true,
            /* color: */ Color.White,
            /* precache: */ true,
            /* source: */ SkinSource.ExceptBeatmap,
        ]);

        // Cannot be startPosition directly
        var positionX = Vector2.X.Get(position) + 8f;
        var positionY = GetYOffset(Vector2.Y.Get(position), scale, __instance);
        var newPosition = ((ConstructorInfo)Vector2.Constructor.Reference).Invoke([positionX, positionY]);
        pDrawable.Position.Set(performanceSprite, newPosition);

        pDrawable.Scale.Set(performanceSprite, 0.50f);
        pSpriteText.TextConstantSpacing.Set(performanceSprite, true);
        pSpriteText.MeasureText.Invoke(performanceSprite);

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

    private static float GetYOffset(float baseYPosition, float scale, object scoreDisplay)
    {
        // Read the heights of both pSpriteTexts: s_Score, s_Accuracy
        var sprites = ScoreDisplay.RuntimeType
            .GetDeclaredFields()
            .Where(f => f.FieldType == pSpriteText.RuntimeType)
            .Select(f => f.GetValue(scoreDisplay));
        var spriteSizes = sprites
            .Where(s => s != null)
            .Select(s => pSpriteText.MeasureText.Invoke(s));
        var totalSpriteHeight = spriteSizes.Sum(v => Vector2.Y.Get(v)) * 0.58f * scale;

        // Preserve additional spacing between s_Score and s_Accuracy
        var additionalOffset = SkinManager.GetUseNewLayout.Invoke() ? 3f : 0f;

        return baseYPosition + totalSpriteHeight + additionalOffset;
    }
}