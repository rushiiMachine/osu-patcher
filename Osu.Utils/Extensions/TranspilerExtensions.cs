using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace Osu.Utils.Extensions;

/// <summary>
///     Utilities for patching bytecode through the usage of <see cref="HarmonyTranspiler" />.
/// </summary>
public static class TranspilerExtensions
{
    /// <summary>
    ///     Finds a certain IL signature and inserts instructions after it. This only matches the signature once.
    /// </summary>
    /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
    /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
    /// <param name="newInstructions">The instructions to insert after a signature match.</param>
    public static IEnumerable<CodeInstruction> InsertAfterSignature(
        this IEnumerable<CodeInstruction> instructions,
        IReadOnlyList<OpCode> signature,
        IReadOnlyList<CodeInstruction> newInstructions)
    {
        var sequentialMatching = 0;
        var found = false;

        foreach (var instruction in instructions)
        {
            if (!found)
            {
                if (sequentialMatching == signature.Count)
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

        if (!found)
            throw new Exception("Could not find the target signature in method!");
    }

    /// <summary>
    ///     Finds a certain IL signature and inserts instructions before it.
    /// </summary>
    /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
    /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
    /// <param name="newInstructions">The instructions to insert before a signature match.</param>
    /// <param name="all">Match all instances of a signature.</param>
    public static IEnumerable<CodeInstruction> InsertBeforeSignature(
        this IEnumerable<CodeInstruction> instructions,
        IReadOnlyList<OpCode> signature,
        IReadOnlyList<CodeInstruction> newInstructions,
        bool all = false)
    {
        var found = false;
        var curInstIdx = 0;
        var allInstructions = instructions.ToArray();

        while (curInstIdx < allInstructions.Length)
        {
            if (found && !all)
            {
                // Return the rest of the instructions
                yield return allInstructions[curInstIdx++];
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

                foreach (var newInstruction in newInstructions)
                    yield return newInstruction;

                yield return allInstructions[curInstIdx++];
            }
        }

        if (!found)
            throw new Exception("Could not find the target signature in method!");
    }

    /// <summary>
    ///     Finds and no-ops an IL bytecode signature.
    /// </summary>
    /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
    /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
    /// <param name="all">Match all instances of a signature.</param>
    public static IEnumerable<CodeInstruction> NoopSignature(
        this IEnumerable<CodeInstruction> instructions,
        IReadOnlyList<OpCode> signature,
        bool all = false)
    {
        var found = false;
        var curInstIdx = 0;
        var allInstructions = instructions.ToArray();

        while (curInstIdx < allInstructions.Length)
        {
            if (found && !all)
            {
                // Return the rest of the instructions
                yield return allInstructions[curInstIdx++];
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
                    var curInst = allInstructions[curInstIdx++]!;
                    curInst.opcode = OpCodes.Nop;
                    curInst.operand = null;

                    yield return curInst;
                }
            }
        }

        if (!found)
            throw new Exception("Could not find the target signature in method!");
    }

    /// <summary>
    ///     Finds and no-ops everything after a certain IL bytecode signature.
    /// </summary>
    /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
    /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
    /// <param name="replaceAfterSignature">The amount of instructions to replace after the signature.</param>
    /// <param name="all">Match all instances of a signature.</param>
    public static IEnumerable<CodeInstruction> NoopAfterSignature(
        this IEnumerable<CodeInstruction> instructions,
        OpCode[] signature,
        uint replaceAfterSignature,
        bool all = false)
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

                instruction.opcode = OpCodes.Nop;
                instruction.operand = null;

                yield return instruction;
            }
            else
            {
                if (signature.Length > 0)
                {
                    if (replaceAfterSignature <= 0 && all)
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
}