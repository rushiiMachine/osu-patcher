using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.ShowComboBreaks;

internal static class ShowComboBreaksUtil
{
    // Stored by SliderOsu#GetHashCode() -> "isComboBroken"
    internal static readonly Dictionary<int, bool> ComboBreakSliders = new Dictionary<int, bool>();

    // osu.GameplayElements.HitObjects.Osu.SliderOsu#sliderTicksHitStatus
    private static readonly FieldInfo FSliderOsu_sliderTicksHitStatus;

    // osu.GameplayElements.HitObjects.Osu.SliderOsu#sliderStartCircle
    private static readonly FieldInfo FSliderOsu_sliderStartCircle;

    // osu.GameplayElements.HitObjects.HitObject#IsHit
    private static readonly FieldInfo FHitObject_IsHit;

    static ShowComboBreaksUtil()
    {
        FSliderOsu_sliderTicksHitStatus = SliderOsu.RuntimeType.GetRuntimeFields()
            .Single(f => f.FieldType.IsArray && f.FieldType.GetElementType() == typeof(bool));

        FSliderOsu_sliderStartCircle =
            AccessTools.Field("#=zoa4eC6Sw5rwIwh2Hbi$3M6X$Z6nv8UnYgxNpICPLicAP:#=z39rOeuDVskYOgRwpx3zjK74=");

        FHitObject_IsHit = AccessTools.Field("#=zqwKEmiGIDb0qpLnR3aavM7g3al9jl960OQ==:#=zIDw7YQY=");

        // HitCicleOsu = #=zOFC2jmTKji24ougWFWDM$GEqjzktzU4gcKFF$gYj32T9m7QUzQ== 
    }

    public static void Reset()
    {
        Console.WriteLine("clearing");
        ComboBreakSliders.Clear();
    }

    /// <summary>
    ///     Used by <see cref="PatchDrawSliderComboBreaks" /> in order to inject a call to this
    ///     to check whether a HitObject is an std slider that has caused broken combo.
    /// </summary>
    /// <param name="hitObject"></param>
    /// <returns></returns>
    public static bool IsHitObjectComboBreakingSlider(object hitObject)
    {
        if (SliderOsu.RuntimeType.IsInstanceOfType(hitObject))
        {
            var sliderTicks = (bool[])FSliderOsu_sliderTicksHitStatus.GetValue(hitObject);
            var startCircle = FSliderOsu_sliderStartCircle.GetValue(hitObject);
            var isStartCircleHit = FHitObject_IsHit.GetValue(startCircle);

            Console.WriteLine(
                $"sliderTicksHitStatus: {sliderTicks.Join()}; isStartCircleHit: {isStartCircleHit}");
        }

        return SliderOsu.RuntimeType.IsInstanceOfType(hitObject)
               && ComboBreakSliders.TryGetValue(hitObject.GetHashCode(), out var isBreaking)
               && isBreaking;
    }
}