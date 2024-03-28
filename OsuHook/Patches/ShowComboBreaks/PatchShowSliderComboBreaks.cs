using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace OsuHook.Patches.ShowComboBreaks
{
    /// <summary>
    ///     Changes the following code in <c>osu.GameplayElements.HitObjectManager:vmethod_17(HitObject)</c>
    ///     to enable showing combo breaks on sliders using the data obtained through
    ///     <see cref="PatchTrackComboBreakingSliders" />.
    ///     <br /><br />
    ///     From:
    ///     <code><![CDATA[
    ///         if (increaseScoreType < (IncreaseScoreType)0) {
    ///             text2 = "hit0";  // string obfuscated
    ///         } else {
    ///             ...
    ///         }
    ///     ]]></code>
    ///     To:
    ///     <code><![CDATA[
    ///         if (PatchDrawSliderComboBreaks.IsObjectSliderComboBreaking(hitObject_arg1))
    ///             goto jmp_true:
    ///         if (increaseScoreType < (IncreaseScoreType)0) {
    ///             jmp_true:
    ///             text2 = "hit0";  // string obfuscated
    ///         } else {
    ///             ...
    ///         }
    ///     ]]></code>
    /// </summary>
    // [HarmonyPatch]
    internal class PatchDrawSliderComboBreaks : BasePatch
    {
        [HarmonyTargetMethod]
        private static MethodBase Target() =>
            // TODO: make global signature list util
            PatchEnableRelaxMisses.Target();

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpile(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            // Right before the "increaseScoreType < 0"
            var targetSignature = new[]
            {
                OpCodes.Ldc_I4_2,
                OpCodes.Bgt_Un_S,
                OpCodes.Ldc_I4,
                OpCodes.Stloc_0,
                OpCodes.Call,
                OpCodes.Pop,
            };

            var elseJmpLabel = generator.DefineLabel();

            var postInsertInstructions = InsertAfterSignature(
                instructions,
                targetSignature,
                new[]
                {
                    // Load the first argument onto the stack (Argument 0 is `this`)
                    new CodeInstruction(OpCodes.Ldarg_1),

                    // Call the method below to check w/ top of stack, result pushed to stack
                    CodeInstruction.Call(typeof(ShowComboBreaksUtil),
                        nameof(ShowComboBreaksUtil.IsHitObjectComboBreakingSlider)),

                    // Jmp to the if "true" block if result true
                    new CodeInstruction(OpCodes.Brtrue_S, elseJmpLabel),
                }
            );

            // Find first inst of the if "true" block
            // TODO: find this properly
            postInsertInstructions.FirstOrDefault(i => i.opcode == OpCodes.Ldc_I4 && (int)i.operand == -0xFC85EE6)
                ?.labels.Add(elseJmpLabel);

            return postInsertInstructions;
        }
    }
}