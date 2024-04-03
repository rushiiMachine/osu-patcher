using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Audio;
using Osu.Stubs.SongSelect;
using static Osu.Stubs.Other.Mods;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal class ModSelectAudioPreview
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => ModButton.SetStatus.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void After(
        object __instance, // typeof(ModButton)
        [HarmonyArgument(1)] int mod,
        [HarmonyArgument(2)] bool playSound)
    {
        // These calls happen for all mods on any mod update
        // and don't actually indicate a ModButton being pressed
        if (!playSound) return;

        // var availableModStates = ModButton.AvailableStates.Get(__instance);
        //
        // // Check that this is the DT+NC or HF button
        // if (availableModStates[0] is not (DoubleTime or HalfTime))
        //     return;

        ApplyChanges(mod);
    }

    internal static void ApplyChanges(int mods)
    {
        ResetChanges();

        switch (mods & (DoubleTime | Nightcore | HalfTime))
        {
            case DoubleTime:
                UpdateAudioRate(rate => rate * 1.5);
                break;
            case Nightcore:
                AudioEngine.Nightcore.Set(true);
                UpdateAudioRate(rate => rate * 1.5);
                break;
            case HalfTime:
                UpdateAudioRate(rate => rate * 0.75);
                break;
        }
    }

    /// <summary>
    ///     Resets the audio stream effects back to default.
    /// </summary>
    private static void ResetChanges()
    {
        AudioEngine.Nightcore.Set(false);
        UpdateAudioRate(_ => 100);
    }

    private static void UpdateAudioRate(Func<double, double> onModify)
    {
        var currentRate = AudioEngine.GetCurrentPlaybackRate.Invoke();
        var newRate = onModify.Invoke(currentRate);

        AudioEngine.SetCurrentPlaybackRate.Invoke(parameters: [newRate]);
    }
}