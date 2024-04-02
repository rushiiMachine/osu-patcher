using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Audio;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

/// <summary>
///     <c>AudioTrackBass::updatePlaybackRate()</c> forcibly resets the tempo to
///     normal if a pitch isn't applied. This forces the tempo to be set if pitch isn't applied,
///     to be used with the <see cref="ModSelectAudioPreview" /> patch
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class FixUpdatePlaybackRate
{
    // static FixUpdatePlaybackRate()
    // {
    //     Task.Run(async () =>
    //     {
    //         await Task.Delay(2000);
    //         try
    //         {
    //             var mtd = AccessTools.Method(AudioTrackBass.Class.Reference.Name + ":" +
    //                                          AudioTrackBass.UpdatePlaybackRate.Reference.Name);
    //             foreach (var instruction in MethodReader.GetInstructions(mtd))
    //             {
    //                 Console.WriteLine($"{instruction.Opcode} {instruction.Operand}");
    //             }
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(e);
    //         }
    //     });
    // }

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => AudioTrackBass.UpdatePlaybackRate.Reference;

    // TODO: WHY THE FUCK ISN'T THIS TRANSPILER APPLYING CHANGES????? PRE/POST PATCHES WORK FINE BUT THIS DOESN'T????????
    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        instructions = instructions.ManipulatorReplace(
            // Find inst that loads 0f as the parameter "value" to BASS_ChannelSetAttribute
            inst => inst.Is(Ldc_R4, 0f),
            inst => new CodeInstruction[]
            {
                new(Ldarg_0) { labels = inst.labels }, // Load "this"
                new(Ldfld, AudioTrackBass.PlaybackRate.Reference), // Load the float64 "playbackRate"
                new(Ldc_R8, 100.0), // Load the float64 "100.0"
                new(Sub), // Subtract 100.0 from playbackRate
                new(Conv_R4), // Convert to float32
            }
        );

        return instructions;
    }
}