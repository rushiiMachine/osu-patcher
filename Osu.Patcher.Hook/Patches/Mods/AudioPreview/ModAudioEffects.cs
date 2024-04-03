using System;
using Osu.Stubs.Audio;
using Osu.Stubs.Scoring;
using static Osu.Stubs.Other.Mods;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

/// <summary>
///     Handles applying and resetting the audio effects on the AudioEngine.
/// </summary>
internal static class ModAudioEffects
{
    /// <summary>
    ///     Applies the audio changes to the AudioEngine based on the current global mods.
    /// </summary>
    internal static void ApplyModEffects()
    {
        ResetChanges();

        var mods = ModManager.ModStatus.Get();

        // NC always comes with DT
        if ((mods & Nightcore) > None)
            mods &= ~DoubleTime;

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

    /// <summary>
    ///     Gets, modifies, and writes back the CurrentPlaybackRate on the AudioEngine.
    /// </summary>
    /// <param name="onModify">Rate transformer.</param>
    private static void UpdateAudioRate(Func<double, double> onModify)
    {
        var currentRate = AudioEngine.GetCurrentPlaybackRate.Invoke();
        var newRate = onModify.Invoke(currentRate);

        AudioEngine.SetCurrentPlaybackRate.Invoke(parameters: [newRate]);
    }
}