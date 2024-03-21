using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Osu.Stubs.Opcode;

public class LazyMethod
{
    private readonly Lazy<MethodBase?> _lazy;
    private readonly string _name;

    /// <summary>
    ///     A lazy method signature matcher
    /// </summary>
    /// <param name="name"><c>Class#Method</c> name of what this signature is matching.</param>
    /// <param name="signature">Sequential opcodes to search the target method with.</param>
    internal LazyMethod(string name, IReadOnlyList<OpCode> signature)
    {
        _name = name;
        _lazy = new Lazy<MethodBase?>(() => OpCodeMatcher.FindMethodBySignature(signature));
    }

    /// <summary>
    ///     Make a wrapper around Lazy for methods.
    /// </summary>
    /// <param name="name"><c>Class#Method</c> name of what this <paramref name="action" /> is returning.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    internal LazyMethod(string name, Func<MethodBase?> action)
    {
        _name = name;
        _lazy = new Lazy<MethodBase?>(action);
    }

    public MethodBase Reference
    {
        get
        {
            var value = _lazy.Value;

            if (value == null)
                throw new Exception($"Method was not found for signature of {_name}");

            return value;
        }
    }
}