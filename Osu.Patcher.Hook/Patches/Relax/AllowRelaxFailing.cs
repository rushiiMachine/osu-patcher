using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Extensions;
using Osu.Utils.IL;

namespace Osu.Patcher.Hook.Patches.Relax;

/// <summary>
///     Changes the following code in <c>osu.GameModes.Play.Player:CheckFailed()</c>
///     to enable failing while playing Relax*.
///     <br /><br />
///     From:
///     <code><![CDATA[
///         if ((mods & Mods.NoFail) <= Mods.None && !Player.Relaxing && !Player.Relaxing2 && ...)
///     ]]></code>
///     To:
///     <code><![CDATA[
///         if ((mods & Mods.NoFail) <= Mods.None && ...)
///     ]]></code>
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AllowRelaxFailing
{
    // #=zeXZ7VnmadWamDozl0oXkDPqWT5QR:#=zwMd5KYaUmGit
    private static readonly OpCode[] Signature =
    [
        OpCodes.And,
        OpCodes.Ldc_I4_0,
        OpCodes.Cgt,
        OpCodes.Brtrue_S,
        OpCodes.Ldsfld, // ----------
        OpCodes.Brtrue_S, // No-oped (4 inst)
        OpCodes.Ldsfld,
        OpCodes.Brtrue_S, // ---------
    ];

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => OpCodeMatcher.FindMethodBySignature(Signature)!;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.NoopAfterSignature(
            Signature.Take(Signature.Length - 4).ToArray(),
            4
        );

        return instructions;
    }
}