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
///     Changes the following code in <c>osu.GameModes.Play.Player:UpdateBloomEffects()</c>
///     to enable (during a Relax* play) the red glow caused by low health.
///     <br /><br />
///     From:
///     <code><![CDATA[
///         if (this.Ruleset.HpBar != null &&
///             this.Ruleset.HpBar.CurrentHp < 40.0 &&
///             !GameBase.TestMode &&
///             !Player.Relaxing &&
///             !Player.Relaxing2 &&
///             this.Ruleset.AllowFailTint &&
///             this.AllowFailShaderEffects &&
///             GameBase.FadeState == (FadeStates)2)
///     ]]></code>
///     To:
///     <code><![CDATA[
///         if (this.Ruleset.HpBar != null &&
///             this.Ruleset.HpBar.CurrentHp < 40.0 &&
///             !GameBase.TestMode && this.Ruleset.AllowFailTint &&
///             this.AllowFailShaderEffects &&
///             GameBase.FadeState == (FadeStates)2)
///     ]]></code>
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AllowRelaxLowHpGlow
{
    private static readonly OpCode[] Signature =
    [
        OpCodes.Ldarg_0,
        OpCodes.Ldfld,
        OpCodes.Ldfld,
        OpCodes.Callvirt,
        OpCodes.Ldc_R8,
        OpCodes.Bge_Un,
        OpCodes.Ldsfld,
        OpCodes.Brtrue,
        OpCodes.Ldsfld, // ----------
        OpCodes.Brtrue, // No-oped (4 inst)
        OpCodes.Ldsfld,
        OpCodes.Brtrue, // ---------
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