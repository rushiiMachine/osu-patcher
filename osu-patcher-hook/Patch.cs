using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace osu_patcher_hook
{
    /// <summary>
    ///     A base patch that provides utility methods for signature-based patching.
    /// </summary>
    internal class Patch
    {
        private static readonly Module OsuModule;

        static Patch()
        {
            var osuAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == "osu!");

            OsuModule = osuAssembly?.GetModules().SingleOrDefault()
                        ?? throw new Exception("Unable to find a loaded osu! assembly!");
        }

        protected static MethodInfo FindMethodBySignature(OpCode[] signature)
        {
            if (signature.Length <= 0) return null;

            foreach (var type in OsuModule.GetTypes())
            foreach (var method in type.GetRuntimeMethods())
            {
                var instructions = method.GetMethodBody()?.GetILAsByteArray();
                if (instructions == null) continue;

                var sequentialMatching = 0;
                foreach (var instruction in new OpCodeReader(instructions).GetOpCodes())
                {
                    if (sequentialMatching == signature.Length)
                        return method;

                    if (instruction == signature[sequentialMatching])
                        sequentialMatching++;
                    else
                        sequentialMatching = 0;
                }
            }

            return null;
        }

        /// <summary>
        ///     Finds and no-ops everything after a certain IL bytecode signature.
        /// </summary>
        /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
        /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
        /// <param name="replaceAfterSignature">The amount of instructions to replace after the signature.</param>
        /// <param name="replaceAll">No-op multiple times for every signature found.</param>
        /// <returns></returns>
        protected static IEnumerable<CodeInstruction> NoopAfterBySignature(
            IEnumerable<CodeInstruction> instructions,
            OpCode[] signature,
            uint replaceAfterSignature,
            bool replaceAll = false)
        {
            var replacementRemaining = replaceAfterSignature;
            var sequentialMatching = 0;
            var found = false;

            foreach (var instruction in instructions)
                if (signature.Length > 0 && replacementRemaining > 0 && sequentialMatching == signature.Length)
                {
                    found = true;
                    replacementRemaining--;
                    yield return new CodeInstruction(OpCodes.Nop);
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

            if (signature.Length > 0 && !found)
                Console.WriteLine("Could not find the target signature in method!");
            else if (found && replacementRemaining > 0 && replaceAfterSignature != replacementRemaining)
                Console.WriteLine("Not enough space in method to noop more instructions!");
        }
    }
}