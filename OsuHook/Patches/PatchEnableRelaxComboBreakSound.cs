using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using OsuHook.OpcodeUtil;

// ReSharper disable UnusedType.Global UnusedMember.Local

namespace OsuHook.Patches
{
    /// <summary>
    ///     Changes the following code in
    ///     <c>osu.GameModes.Play.Rulesets.Ruleset:IncreaseScoreHit(IncreaseScoreType, HitObject)</c>
    ///     to enable the combo break sound during Relax* scores.
    ///     <br /><br />
    ///     From:
    ///     <code><![CDATA[
    ///         if (this.ComboCounter.HitCombo > 20 && !Player.Relaxing && !Player.Relaxing2)
    ///     ]]></code>
    ///     To:
    ///     <code><![CDATA[
    ///         if (this.ComboCounter.HitCombo > 20)
    ///     ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchEnableRelaxComboBreakSound : BasePatch
    {
        // #=z04fOmc1I_BS0TV6TAo2QOUQvjceryuOcqoleWPg=:#=zSio4IZHzUUrC
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Callvirt,
            OpCodes.Ldc_I4_S,
            OpCodes.Ble_S,
            OpCodes.Ldsfld, // --------
            OpCodes.Brtrue_S, // All no-oped (4 inst)
            OpCodes.Ldsfld,
            OpCodes.Brtrue_S, // --------
        };

        [HarmonyTargetMethod]
        internal static MethodBase Target() => OpCodeMatcher.FindMethodBySignature(Signature);

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
            NoopAfterSignature(instructions, Signature.Take(Signature.Length - 4).ToArray(), 4);
    }
}