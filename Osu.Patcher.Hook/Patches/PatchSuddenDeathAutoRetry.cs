using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Makes the Sudden Death mod restart immediately instead of just failing.
///     Changes the following code in <c>osu.GameModes.Play.Rulesets.Ruleset:Fail(...)</c>:
///     <br /><br />
///     From:
///     <code><![CDATA[
///         if ((this.CurrentScore.EnabledMods & Mods.Perfect) > Mods.None)
///     ]]></code>
///     To:
///     <code><![CDATA[
///         const int mask = Mods.Perfect | Mods.SuddenDeath;
///         if ((this.CurrentScore.EnabledMods & mask) > Mods.None)
///     ]]></code>
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
public class PatchSuddenDeathAutoRetry
{
    private const int ModPerfect = 1 << 14;
    private const int ModSuddenDeath = 1 << 5;

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Ruleset.Fail.Reference;

    /// <summary>
    ///     Replace the instruction that loads <c>Mods.Perfect</c> with one
    ///     that loads <c>Mods.Perfect | Mods.SuddenDeath</c>.
    /// </summary>
    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
        instructions.Manipulator(
            inst => inst.opcode == Ldc_I4 && inst.OperandIs(ModPerfect),
            inst => inst.operand = ModPerfect | ModSuddenDeath
        );

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchSuddenDeathAutoRetry)}: {__exception}");
        }
    }
}