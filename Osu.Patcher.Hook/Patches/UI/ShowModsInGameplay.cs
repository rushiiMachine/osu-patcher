using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Play;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.UI;

/// <summary>
///     Makes the mods list overlay that is shown when entering play mode always show, like in a replay
///     but faded to a user customizable amount.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class ShowModsInGameplay
{
    // TODO: make this user configurable
    private const float ModsNewAlpha = .2f;

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Player.OnLoadComplete.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // Replace a "0f" representing a fade target value after an arbitrary "94f" constant
        // Also set the pSprite parameter "alwaysDraw" to true

        var replaceState = ReplaceState.Find;

        return instructions.Manipulator(
            inst =>
            {
                switch (replaceState)
                {
                    case ReplaceState.Find when inst.Is(Ldc_R4, 94f):
                        replaceState = ReplaceState.ReplaceAlwaysDraw;
                        return false;

                    // This is calling "InputManager.get_ReplayMode()"
                    case ReplaceState.ReplaceAlwaysDraw when inst.opcode == Call:
                        return true;

                    // This is loading the transformation fade end value
                    case ReplaceState.ReplaceFadeEndValue when inst.Is(Ldc_R4, 0f):
                        return true;

                    case ReplaceState.Finished:
                    default:
                        return false;
                }
            },
            inst =>
            {
                switch (replaceState)
                {
                    case ReplaceState.ReplaceAlwaysDraw:
                        inst.opcode = Ldc_I4_1; // Load "true" for the parameter "alwaysDraw"
                        inst.operand = null;
                        replaceState = ReplaceState.ReplaceFadeEndValue;
                        break;
                    case ReplaceState.ReplaceFadeEndValue:
                        inst.operand = ModsNewAlpha;
                        replaceState = ReplaceState.Finished;
                        break;
                    case ReplaceState.Find:
                    case ReplaceState.Finished:
                    default:
                        throw new Exception();
                }
            }
        );
    }

    private enum ReplaceState
    {
        Find,
        ReplaceAlwaysDraw,
        ReplaceFadeEndValue,
        Finished,
    }
}