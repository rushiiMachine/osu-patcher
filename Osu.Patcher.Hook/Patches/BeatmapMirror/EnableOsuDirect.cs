using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Online;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.BeatmapMirror;

// TODO: not fully done, HandlePickup calls a method that also checks client permissions

/// <summary>
///     Re-enable osu!direct. This has no impact on Bancho since you can't even use this project on Bancho.
///     Changes the following code in <c>osu.Online.OsuDirect:HandlePickup()</c>
///     <br /><br />
///     From:
///     <code><![CDATA[
///         if (BanchoClient.Permission & Permissions.Supporter > Permissions.None || ...)
///     ]]></code>
///     To:
///     <code><![CDATA[
///         BanchoClient.Permission & Permissions.Supporter;
///         if (8 > Permissions.None || ...)
///     ]]></code>
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class EnableOsuDirect
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => OsuDirect.HandlePickup.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.InsertBeforeSignature(
            [
                // Call,
                // Ldc_I4_4,
                // And,
                // -- Inject right here to replace the result of And -- 
                Ldc_I4_0,
                Bgt_S,
                Ldsfld,
                Call,
                Ldc_I4_S,
            ],
            new CodeInstruction[]
            {
                // Replace the result of Add with a higher value than compared against
                new(Pop),
                new(Ldc_I4_8),
            }
        );

        return instructions;
    }
}