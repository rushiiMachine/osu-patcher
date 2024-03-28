using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Osu.Utils.IL;

public static class OpCodeMatcher
{
    /// <summary>
    ///     Search for a method inside the osu! assembly by an IL OpCode signature.
    /// </summary>
    /// <param name="signature">A set of sequential OpCodes to match.</param>
    /// <param name="entireMethod">Whether the signature is the entire method to search for.</param>
    /// <returns>The found method or null if none found.</returns>
    public static MethodInfo? FindMethodBySignature(IReadOnlyList<OpCode> signature, bool entireMethod = false)
    {
        if (signature.Count <= 0) return null;

        foreach (var type in OsuAssembly.Types)
        foreach (var method in type.GetMethods(BindingFlags.Instance
                                               | BindingFlags.Static
                                               | BindingFlags.Public
                                               | BindingFlags.NonPublic))
        {
            var instructions = method.GetMethodBody()?.GetILAsByteArray();
            if (instructions == null) continue;

            if (InstructionsMatchesSignature(instructions, signature, entireMethod))
                return method;
        }

        return null;
    }

    /// <summary>
    ///     Search for a constructor inside the osu! assembly by an IL OpCode signature.
    /// </summary>
    /// <param name="signature">A set of sequential OpCodes to match.</param>
    /// <param name="entireMethod">Whether the signature is the entire method to search for.</param>
    /// <returns>The found constructor (method) or null if none found.</returns>
    public static ConstructorInfo? FindConstructorBySignature(IReadOnlyList<OpCode> signature,
        bool entireMethod = false)
    {
        if (signature.Count <= 0) return null;

        foreach (var type in OsuAssembly.Types)
        foreach (var method in type.GetConstructors(BindingFlags.Instance
                                                    | BindingFlags.Static
                                                    | BindingFlags.Public
                                                    | BindingFlags.NonPublic))
        {
            var instructions = method.GetMethodBody()?.GetILAsByteArray();
            if (instructions == null) continue;

            if (InstructionsMatchesSignature(instructions, signature, entireMethod))
                return method;
        }

        return null;
    }

    /// <summary>
    ///     Check if some IL byte instructions contain a certain set of OpCodes.
    /// </summary>
    /// <param name="ilInstructions">
    ///     Raw IL instruction byte data obtained through <see cref="MethodBody.GetILAsByteArray()" />
    ///     for example.
    /// </param>
    /// <param name="signature">A set of sequential OpCodes to search for in instructions.</param>
    /// <param name="entireMethod">Whether the signature should be the entire method.</param>
    /// <returns></returns>
    private static bool InstructionsMatchesSignature(
        IReadOnlyList<byte> ilInstructions,
        IReadOnlyList<OpCode> signature,
        bool entireMethod)
    {
        var sequentialMatching = 0;
        foreach (var instruction in new OpCodeReader(ilInstructions).GetOpCodes())
        {
            if (instruction == signature[sequentialMatching])
                sequentialMatching++;
            else if (!entireMethod)
                sequentialMatching = 0;
            else return false;

            if (sequentialMatching == signature.Count)
                return true;
        }

        return false;
    }
}