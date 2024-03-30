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
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AutoSaveRelaxScores
{
    // #=zG9n2xn5fBJ3KmhYrFhPv_ouHnledvs2AJ1Dwx_c=:#=zPWtjIx_tsaf1
    private static readonly OpCode[] Signature =
    [
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
        OpCodes.Brtrue, // <-------------
    ];

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => OpCodeMatcher.FindMethodBySignature(Signature)!;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.NoopAfterSignature(
            Signature.Take(Signature.Length - 24).ToArray(),
            24
        );

        return instructions;
    }
}