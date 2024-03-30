using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.UI;

/// <summary>
///     Changes the default alpha on beatmap thumbnails in song select.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class CustomSongSelectThumbnailAlpha : OsuPatch
{
    // TODO: make this user configurable through settings
    private const int NewDeselectedAlpha = 185; // 0-255

    [UsedImplicitly]
    [HarmonyTargetMethods]
    private static IEnumerable<MethodBase> Target() =>
    [
        BeatmapTreeItem.PopulateSprites.Reference,
        BeatmapTreeItem.UpdateSprites.Reference,
    ];

    /// <summary>
    ///     Changes the target color used as an alpha overlay over the thumbnail.
    ///     Replaces all <c>50</c>s as in <c>new Color(50, 50, 50, ...)</c> with a higher value.
    ///     Applied to both <c>UpdateSprites</c> and <c>PopulateSprites</c>.
    /// </summary>
    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ChangeDeselectFade(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.Manipulator(
            inst => inst.Is(Ldc_I4_S, 50),
            inst => inst.operand = NewDeselectedAlpha
        );

        return instructions;
    }


    /// <summary>
    ///     Avoid re-fading in from zero when alpha already high from previous patches.
    ///     Replaces the following code with Nop: <code>thumbnail.FadeInFromZero(instant ? 0 : 1000);</code>
    ///     This is only applicable to <c>UpdateSprites()</c>.
    /// </summary>
    [UsedImplicitly]
    [HarmonyTranspiler]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static IEnumerable<CodeInstruction> DisableFadeInFromZero(
        IEnumerable<CodeInstruction> instructions,
        MethodBase __originalMethod)
    {
        if (__originalMethod != BeatmapTreeItem.UpdateSprites.Reference)
            return instructions;

        return instructions.NoopSignature(new[]
        {
            Ldarg_0,
            Ldfld,
            Ldloc_0,
            Ldfld,
            Brtrue_S,
            Ldc_I4,
            Br_S,
            Ldc_I4_0,
            Callvirt,
            Pop,
        });
    }
}