using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Patcher.Hook.Patches.UI;
using Osu.Stubs;
using Osu.Stubs.Other;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.Misc;

/// <summary>
///     Allows the <c>Play</c> game mode to be reloaded without having the Autoplay mod.
///     This allows for automatically applying settings changes (in combination with the
///     <see cref="AllowOpenOptionsInGameplay" /> patch without having to exit play mode to apply changes.
///     Changes the following code in <c>osu.GameBase:get_ModeCanReload()</c>:
///     <br /><br />
///     From:
///     <code><![CDATA[
///         if (mode == OsuModes.Play) {
///             if (InputManager.ReplayScore == null)
///                 return false;
///     ]]></code>
///     To:
///     <code><![CDATA[
///         if (mode == OsuModes.Play) {
///             return true;
///     ]]></code>
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AllowPlayModeReload
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => GameBase.GetModeCanReload.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.InsertBeforeSignature(
            [
                // Ldsfld, // Loads the ReplayScore to check if it's null
                // -- Inject right here to override the value -- 
                Brfalse_S,
                Ldsfld,
                Ldfld,
                Call,
            ],
            new CodeInstruction[]
            {
                new(Pop),
                new(Ldc_I4_1),
                new(Ret),
            }
        );

        return instructions;
    }
}