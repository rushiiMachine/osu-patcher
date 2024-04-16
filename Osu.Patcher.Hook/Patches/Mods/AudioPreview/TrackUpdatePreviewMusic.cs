using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Audio;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

/// <summary>
///     Hooks the place where preview audio gets loaded to apply our mod audio effects.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
public class TrackUpdatePreviewMusic
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => AudioEngine.LoadAudioForPreview.Reference;

    [HarmonyPostfix]
    [UsedImplicitly]
    private static void After()
    {
        if (!AudioPreviewOptions.Enabled.Value)
            return;

        Task.Run(() => ModAudioEffects.ApplyModEffects());
    }
}