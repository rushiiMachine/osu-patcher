using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace OsuHook.Patches
{
    /// <summary>
    ///     Changes the following code in <c>osu.GameplayElements.HitObjectManager:vmethod_17(HitObject)</c>
    ///     to enable showing misses in Relax* scores.
    ///     <br /><br />
    ///     From:
    ///     <code><![CDATA[
    ///         if (increaseScoreType == (IncreaseScoreType)(-131072) && !Player.Relaxing && !Player.Relaxing2)
    ///     ]]></code>
    ///     To:
    ///     <code><![CDATA[
    ///         if (increaseScoreType == (IncreaseScoreType)(-131072))
    ///     ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchEnableRelaxMisses
    {
        // #=zTBjFb7Vm$jY$rY4MsKxmcIvGHnQN:\u0005\u200A\u2002\u2002\u2001\u2004\u2003\u2007\u2001\u2002\u2002\u2000
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_1,
            OpCodes.Ldc_I4_8,
            OpCodes.Callvirt,
            OpCodes.Stloc_S,
            OpCodes.Ldloc_0,
            OpCodes.Ldc_I4,
            OpCodes.Bne_Un,
            OpCodes.Ldsfld, // ----------
            OpCodes.Brtrue, // No-oped (4 inst)
            OpCodes.Ldsfld,
            OpCodes.Brtrue // ----------
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