using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.UI;

/// <summary>
///     Allows the options screen to be expanded (with Ctrl-O) while in a Play state.
///     Changes the following code in <c>osu.GameModes.Options.Options:get_CanExpand()</c>:
///     <br /><br />
///     From:
///     <code><![CDATA[
///         case OsuModes.Play:
///             return ModManager.CheckActive(Mods.Autoplay);
///     ]]></code>
///     To:
///     <code><![CDATA[
///         case OsuModes.Play:
///             ModManager.CheckActive(Mods.Autoplay);
///             return true;
///     ]]></code>
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AllowOpenOptionsInGameplay
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Options.GetCanExpand.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.InsertAfterSignature(
            [
                Ldloc_2,
                Ldloc_3,
                And,
                Ldc_I4_0,
                Cgt,
                // -- Inject right here to replace the result of Cgt --
                // Ret,
            ],
            new CodeInstruction[]
            {
                new(Pop),
                new(Ldc_I4_1), // Push "true" onto stack
            }
        );

        return instructions;
    }
}