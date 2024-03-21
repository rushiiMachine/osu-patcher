using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Fix the double skipping logic for storyboards (skip first to start of storyboard, then map).
///     <br /><br />
///     Changes the following in <c>Player:AllowDoubleSkip.get</c> from
///     <code><![CDATA[
///         int leadIn = leadInTime < 10000 ? -leadInTime : 0;
///     ]]></code>
///     To:
///     <code><![CDATA[
///         if (!EventManager.ShowStoryboard) return;
///         int leadIn = leadInTime < 0 ? -leadInTime : 0;
///     ]]></code>
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class PatchFixDoubleSkipping : BasePatch
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Player.AllowDoubleSkip_get.Reference;

    /// <summary>
    ///     Prefix patch to check if storyboard is enabled.
    /// </summary>
    [UsedImplicitly]
    [HarmonyPrefix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static bool Before(ref bool __result)
    {
        if (!(bool)EventManager.ShowStoryboard_backing.Reference.GetValue(null))
        {
            __result = false;

            // Prevent original function from executing
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Replace the instruction that loads the integer 10000 with one that loads 0.
    /// </summary>
    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
        instructions.Manipulator(
            inst => inst.OperandIs(10000),
            inst => inst.operand = 0
        );
}