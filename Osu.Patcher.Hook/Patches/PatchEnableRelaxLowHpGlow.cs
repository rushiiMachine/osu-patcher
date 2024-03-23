using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;

namespace Osu.Patcher.Hook.Patches;

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
[HarmonyPatch]
[UsedImplicitly]
internal class PatchEnableRelaxLowHpGlow : BasePatch
{
    private static readonly OpCode[] Signature =
    {
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
    };

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => OpCodeMatcher.FindMethodBySignature(Signature)!;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
        NoopAfterSignature(instructions, Signature.Take(Signature.Length - 4).ToArray(), 4);

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchEnableRelaxLowHpGlow)}: {__exception}");
        }
    }
}