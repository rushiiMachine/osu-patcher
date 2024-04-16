using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Select;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

/// <summary>
///     Hooks the place where ModButtons get updates in the mod selection menu to apply audio effects.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal class ModSelectAudioPreview
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => ModSelection.UpdateMods.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    private static void After()
    {
        if (!AudioPreviewOptions.Enabled.Value)
            return;

        Task.Run(() => ModAudioEffects.ApplyModEffects());
    }
}