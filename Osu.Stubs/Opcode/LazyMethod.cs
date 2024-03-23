using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Osu.Stubs.Opcode;

/// <summary>
///     A reference to a method that gets located at runtime and invoked reflectively.
/// </summary>
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

    /// <summary>
    ///     Find if not already cached and reflectively invoke this method. Does not return any value.
    /// </summary>
    /// <param name="instance">Instance of the enclosing class if this method is not static.</param>
    /// <param name="parameters">Parameters to pass to the method, if any.</param>
    public void Invoke(object? instance = null, object?[]? parameters = null) =>
        Reference.Invoke(instance, parameters);
}

// ReSharper disable once InconsistentNaming
/// <summary>
///     A reference to a method that gets located at runtime and invoked reflectively.
/// </summary>
/// <typeparam name="R">A type that the return value of this method can be casted to.</typeparam>
public class LazyMethod<R> : LazyMethod
{
    /// <inheritdoc />
    internal LazyMethod(string name, IReadOnlyList<OpCode> signature)
        : base(name, signature)
    {
    }

    /// <inheritdoc />
    internal LazyMethod(string name, Func<MethodBase?> action)
        : base(name, action)
    {
    }

    /// <summary>
    ///     Find if not already cached and reflectively invoke this method.
    /// </summary>
    /// <param name="instance">Instance of the enclosing class if this method is not static.</param>
    /// <param name="parameters">Parameters to pass to the method, if any.</param>
    /// <returns>The return value casted to the type defined by this LazyMethod.</returns>
    public new R Invoke(object? instance = null, object?[]? parameters = null) =>
        (R)Reference.Invoke(instance, parameters);

    /// <inheritdoc cref="Invoke" />
    /// <typeparam name="T">The return type of this method to cast to.</typeparam>
    public T Invoke<T>(object? instance = null, object?[]? parameters = null) =>
        (T)Reference.Invoke(instance, parameters);
}