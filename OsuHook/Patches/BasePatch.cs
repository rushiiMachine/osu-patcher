using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace OsuHook.Patches
{
    /// <summary>
    ///     A base patch that provides utility methods for signature-based patching.
    /// </summary>
    public abstract class BasePatch
    {
        #region Patching Utils

        /// <summary>
        ///     Finds and no-ops everything after a certain IL bytecode signature.
        /// </summary>
        /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
        /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
        /// <param name="replaceAfterSignature">The amount of instructions to replace after the signature.</param>
        /// <param name="replaceAll">No-op multiple times for every signature found.</param>
        /// <returns></returns>
        internal static IEnumerable<CodeInstruction> NoopAfterSignature(
            IEnumerable<CodeInstruction> instructions,
            OpCode[] signature,
            uint replaceAfterSignature,
            bool replaceAll = false)
        {
            var replacementRemaining = replaceAfterSignature;
            var sequentialMatching = 0;
            var found = false;

            foreach (var instruction in instructions)
            {
                if (signature.Length > 0 && replacementRemaining > 0 && sequentialMatching == signature.Length)
                {
                    found = true;
                    replacementRemaining--;
                    yield return new CodeInstruction(OpCodes.Nop)
                        .WithBlocks(instruction.blocks)
                        .WithLabels(instruction.labels);
                }
                else
                {
                    if (signature.Length > 0)
                    {
                        if (replaceAfterSignature <= 0 && replaceAll)
                        {
                            replacementRemaining = replaceAfterSignature;
                            sequentialMatching = 0;
                        }

                        if (replacementRemaining == replaceAfterSignature &&
                            instruction.opcode == signature[sequentialMatching])
                            sequentialMatching++;
                        else
                            sequentialMatching = 0;
                    }

                    yield return instruction;
                }
            }

            if (signature.Length > 0 && !found)
                throw new Exception("Could not find the target signature in method!");

            if (found && replacementRemaining > 0 && replaceAfterSignature != replacementRemaining)
                throw new Exception("Not enough space in method to noop more instructions!");
        }

        /// <summary>
        ///     Finds and no-ops an IL bytecode signature.
        /// </summary>
        /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
        /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
        /// <param name="replaceAll">No-op multiple times for every signature found.</param>
        /// <returns></returns>
        internal static IEnumerable<CodeInstruction> NoopSignature(
            IEnumerable<CodeInstruction> instructions,
            IReadOnlyList<OpCode> signature,
            bool replaceAll = false)
        {
            var found = false;
            var curInstIdx = 0;
            var allInstructions = instructions.ToArray();

            while (curInstIdx < allInstructions.Length)
            {
                if (found && !replaceAll)
                {
                    // Return the rest of the instructions
                    yield return allInstructions[curInstIdx];

                    curInstIdx++;
                    continue;
                }

                var match = signature
                    .Select((op, idx) => new { opcode = op, index = idx })
                    .All(v => v.opcode == allInstructions[curInstIdx + v.index].opcode);

                if (!match)
                {
                    // Emit current instruction and try matching from next instruction
                    yield return allInstructions[curInstIdx++];
                }
                else
                {
                    found = true;

                    for (var i = 0; i < signature.Count; i++)
                    {
                        var curInst = allInstructions[curInstIdx++];

                        yield return new CodeInstruction(OpCodes.Nop)
                            .WithBlocks(curInst.blocks)
                            .WithLabels(curInst.labels);
                    }
                }
            }

            if (!found)
                throw new Exception("Could not find the target signature in method!");
        }

        internal static IEnumerable<CodeInstruction> InsertAfterSignature(
            IEnumerable<CodeInstruction> instructions,
            CodeInstruction[] newInstructions,
            OpCode[] signature)
        {
            var sequentialMatching = 0;
            var found = false;

            foreach (var instruction in instructions)
            {
                if (!found)
                {
                    if (sequentialMatching == signature.Length)
                    {
                        found = true;
                        foreach (var newInstruction in newInstructions)
                            yield return newInstruction;
                    }
                    else if (instruction.opcode == signature[sequentialMatching])
                    {
                        sequentialMatching++;
                    }
                    else
                    {
                        sequentialMatching = 0;
                    }
                }

                yield return instruction;
            }
        }

        #endregion
    }
}