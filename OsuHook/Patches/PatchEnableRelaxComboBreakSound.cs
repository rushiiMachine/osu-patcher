using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

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
    internal class PatchEnableRelaxComboBreakSound
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
            OpCodes.Brtrue_S // --------
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return SigUtils.FindMethodBySignature(Signature);
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return SigUtils.NoopAfterBySignature(
                instructions,
                Signature.Take(Signature.Length - 4).ToArray(),
                4
            );
        }
    }
}