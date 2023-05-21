using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace osu_patcher_hook.patches
{
    /// <summary>
    ///     Removes the following code from <c>osu.GameModes.Ranking:loadLocalUserScore(bool showRankingDialog)</c>
    ///     to enable automatically saving Relax* scores after finishing playing.
    ///     <br />
    ///     <code><![CDATA[
    ///         Mods mods = Player.currentScore.EnabledMods;  
    ///         if ((mods & Mods.Relax) <= Mods.None)
    ///         {
    ///             mods = Player.currentScore.EnabledMods;
    ///             if ((mods & Mods.Relax2) <= Mods.None)
    ///             {
    ///     ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchAutoSaveRelaxScores : Patch
    {
        // #=zG9n2xn5fBJ3KmhYrFhPv_ouHnledvs2AJ1Dwx_c=:#=zPWtjIx_tsaf1
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Ldfld,
            OpCodes.Brfalse,
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Brtrue,
            OpCodes.Ldsfld, // <--------------
            OpCodes.Ldfld,
            OpCodes.Call,
            OpCodes.Ldc_I4,
            OpCodes.Stloc_2,
            OpCodes.Stloc_1,
            OpCodes.Ldloc_1,
            OpCodes.Ldloc_2,
            OpCodes.And,
            OpCodes.Ldc_I4_0,
            OpCodes.Cgt,
            OpCodes.Brtrue, // All no-oped (24 inst)
            OpCodes.Ldsfld,
            OpCodes.Ldfld,
            OpCodes.Call,
            OpCodes.Ldc_I4,
            OpCodes.Stloc_2,
            OpCodes.Stloc_1,
            OpCodes.Ldloc_1,
            OpCodes.Ldloc_2,
            OpCodes.And,
            OpCodes.Ldc_I4_0,
            OpCodes.Cgt,
            OpCodes.Brtrue // <-------------
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return FindMethodBySignature(Signature);
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return NoopAfterBySignature(
                instructions,
                Signature.Take(Signature.Length - 24).ToArray(),
                24
            );
        }
    }
}