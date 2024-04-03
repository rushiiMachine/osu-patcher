using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Audio;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

// ReSharper disable InconsistentNaming

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

/// <summary>
///     Apply various fixes to <c>AudioTrackBass</c> to make <c>BASS_ATTRIB_TEMPO</c> work again on
///     "Preview" audio streams. We use this in combination with <see cref="ModSelectAudioPreview" />
///     to reapply the constant pitch speed modifier (for DoubleTime).
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal class FixUpdatePlaybackRate
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => AudioTrackBass.Constructor.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        // Remove the conditional & block that checks for this.Preview and always do the full initialization
        // Otherwise, the "quick" initialization never uses BASS_FX_TempoCreate, which makes setting
        // BASSAttribute.BASS_ATTRIB_TEMPO impossible (what's used for DoubleTime).
        // instructions = instructions.NoopSignature(
        instructions = instructions.NoopSignature(
            // if (Preview) { audioStream = audioStreamForwards = audioStreamPrefilter; }
            [
                Ldarg_0,
                Callvirt,
                Brfalse_S,
                Ldarg_0,
                Ldarg_0,
                Ldarg_0,
                Ldfld,
                Dup,
                Stloc_1,
                Stfld,
                Ldloc_1,
                Call,
                Br_S,
            ]
        );

        // Change this "BASSFlag flags = Preview ? 0 : (BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);"
        // into "BASSFlag flags = Preview ? BASSFlag.BASS_STREAM_DECODE : (BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);"
        // This is so BASS_FX_TempoCreate still works when called later
        var foundFlags = false;
        const int BASS_STREAM_DECODE = 0x200000;
        const int BASS_STREAM_PRESCAN = 0x20000;
        instructions = instructions.Manipulator(
            inst => foundFlags || inst.Is(Ldc_I4, BASS_STREAM_DECODE | BASS_STREAM_PRESCAN),
            inst =>
            {
                if (inst.OperandIs(BASS_STREAM_DECODE | BASS_STREAM_PRESCAN))
                {
                    foundFlags = true;
                    return;
                }

                // This is the "? 0"
                if (inst.opcode != Ldc_I4_0)
                    return;

                inst.opcode = Ldc_I4;
                inst.operand = BASS_STREAM_DECODE;
                foundFlags = false;
            }
        );

        // Load speed optimization: disable BASS_FX_ReverseCreate when the "quick" parameter is true (aka. Preview)
        var found = false;
        instructions = instructions.Reverse().ManipulatorReplace(
            inst => found || inst.Is(Stfld, AudioTrackBass.AudioStreamBackwardsHandle.Reference),
            inst =>
            {
                // Only targeting the Call instruction before Stfld
                if (inst.opcode != Call)
                {
                    found = true;
                    return [inst];
                }

                found = false;
                var labelTrue = generator.DefineLabel();
                var labelFalse = generator.DefineLabel();

                return new[]
                {
                    new(Ldarg_2), // "bool quick"
                    new(Brfalse_S, labelFalse),

                    // Clean up the 3 values to the Call on stack
                    new(Pop),
                    new(Pop),
                    new(Pop),
                    new(Ldc_I4_0), // Load a "0" to be used for Stfld AudioStreamBackwardsHandle
                    new(Br_S, labelTrue),

                    inst.WithLabels([labelFalse]),
                    new(Nop) { labels = [labelTrue] },
                }.Reverse();
            }
        ).Reverse();

        return instructions;
    }
}