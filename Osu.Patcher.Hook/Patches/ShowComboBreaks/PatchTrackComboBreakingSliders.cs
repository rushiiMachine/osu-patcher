using System;
using System.Reflection;
using HarmonyLib;
using Osu.Patcher.Hook.Patches.Relax;
using Osu.Stubs;
using Osu.Stubs.Wrappers;

// ReSharper disable UnusedType.Global UnusedMember.Local InconsistentNaming

namespace Osu.Patcher.Hook.Patches.ShowComboBreaks;

// [HarmonyPatch]
internal class PatchTrackComboBreakingSliders : BasePatch
{
    [HarmonyTargetMethod]
    private static MethodBase Target() =>
        AllowRelaxComboBreakSound.Target();

    // Ruleset->CurrentScore->CurrentCombo
    private static ushort GetCombo()
    {
        // Ruleset#CurrentScore
        var fCurrentScore = AccessTools.Field("#=znsd474wIu4GAJ8swgEdrqaxwLN4O:#=zW5K7ouTMxMrX");
        // Score#CurrentCombo
        var fCurrentCombo = AccessTools.Field("#=zLBw8hg3V1k_gTYcAwmBw_YkrvWaEQzfUz_i_IrU=:#=zE49fcJU=");

        return (ushort)fCurrentCombo.GetValue(fCurrentScore.GetValue(null));
    }

    [HarmonyPrefix]
    private static void Before(
        [HarmonyArgument(0)] int increaseScoreType,
        [HarmonyArgument(1)] object hitObject)
    {
        // Check if slider & some sort of missed combo except for slider ends
        if (increaseScoreType < 0 && SliderOsu.RuntimeType.IsInstanceOfType(hitObject))
        {
            Console.WriteLine($"IncreaseScoreHit pre: {increaseScoreType} {hitObject}");
            // Console.WriteLine($"pre combo: {GetCombo()}");
            // ShowComboBreaksUtil.ComboBreakSliders[hitObject.GetHashCode()] = true;

            Notifications.ShowMessage("slider combo break", NotificationColor.Warning, 200);
        }
    }

    // [HarmonyPostfix]
    // private static void After(
    //     [HarmonyArgument(0)] int increaseScoreType,
    //     [HarmonyArgument(1)] object hitObject)
    // {
    //     // Check if slider & some sort of missed combo except for slider ends
    //     if (increaseScoreType < 0 && SliderOsu.Class.IsInstanceOfType(hitObject))
    //     {
    //         Console.WriteLine($"post combo: {GetCombo()}");
    //         ShowComboBreaksUtil.ComboBreakSliders[hitObject.GetHashCode()] = true;
    //     }
    // }

    [HarmonyFinalizer]
    private static Exception Finalizer(Exception __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine("Exception while running patch " +
                              $"{nameof(PatchTrackComboBreakingSliders)}: {__exception}");
        }

        return null;
    }
}

// [HarmonyPatch]
// internal class PatchTrackComboBreakingSliders
// {
//     [HarmonyTargetMethod]
//     private static MethodBase Target()
//     {
//         // osu.GameModes.Play.Rulesets.Ruleset:OnIncreaseScoreHit(IncreaseScoreType, double hpIncrease, bool increaseCombo, HitObject)
//         return AccessTools.DeclaredMethod("#=z04fOmc1I_BS0TV6TAo2QOUQvjceryuOcqoleWPg=:#=zBqtfszH0Yf2Z");
//     }
//
//     [HarmonyPostfix]
//     private static void After(
//         [HarmonyArgument(0)] object increaseScoreType,
//         [HarmonyArgument(1)] double hpIncrease,
//         [HarmonyArgument(2)] bool increaseCombo,
//         [HarmonyArgument(3)] object hitObject)
//     {
//         Console.WriteLine($"OnIncreaseScoreHit: {increaseCombo} {hpIncrease} {increaseScoreType} {hitObject}");
//         // Check if slider & some sort of missed combo except for slider ends
//         if ((int)increaseScoreType < 0 && ShowComboBreaksUtil.TSliderOsu.IsInstanceOfType(hitObject))
//             // Console.WriteLine($"OnIncreaseScoreHit: {increaseScoreType} {hitObject}");
//             ShowComboBreaksUtil.ComboBreakSliders[hitObject.GetHashCode()] = true;
//     }
// }