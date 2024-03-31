using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;
using Osu.Utils.IL;

namespace Osu.Utils.Lazy;

/// <summary>
///     A reference to a method that gets located at runtime and invoked reflectively.
/// </summary>
[PublicAPI]
public class LazyMethod : LazyInfo<MethodInfo>
{
    private readonly Lazy<MethodInfo> _lazy;

    /// <summary>
    ///     Make a wrapper around Lazy for methods.
    /// </summary>
    /// <param name="name"><see cref="LazyInfo{T}.Name" /> of what type this <paramref name="action" /> is returning.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    public LazyMethod(string name, Func<MethodInfo> action)
    {
        Name = name;
        _lazy = new Lazy<MethodInfo>(action);
    }

    public override string Name { get; }

    public override MethodInfo Reference => GetReference<LazyMethod>(Name, _lazy);

    /// <summary>
    ///     Find if not already cached and reflectively invoke this method. Does not return any value.
    /// </summary>
    /// <param name="instance">Instance of the enclosing class if this method is not static.</param>
    /// <param name="parameters">Parameters to pass to the method, if any.</param>
    public void Invoke(object? instance = null, object?[]? parameters = null) =>
        Reference.Invoke(instance, parameters);

    /// <summary>
    ///     A lazy method opcode opcode matcher.
    ///     Searches every method to see if this <paramref name="signature" /> is the entire method's bytecode.
    /// </summary>
    /// <param name="name">
    ///     <see cref="LazyInfo{T}.Name" />
    /// </param>
    /// <param name="signature">Sequential opcodes to compare the target method's bytecode with.</param>
    public static LazyMethod BySignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindMethodBySignature(signature, true)!);

    /// <summary>
    ///     A lazy method opcode partial opcode matcher.
    ///     Searches every method to see if this <paramref name="signature" /> is located in the method's bytecode.
    /// </summary>
    /// <param name="name">
    ///     <see cref="LazyInfo{T}.Name" />
    /// </param>
    /// <param name="signature">Sequential opcodes to search the target method with.</param>
    public static LazyMethod ByPartialSignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindMethodBySignature(signature)!);
}

// ReSharper disable once InconsistentNaming
/// <summary>
///     A reference to a method that gets located at runtime and invoked reflectively.
/// </summary>
/// <typeparam name="R">A type that the return value of this method can be casted to.</typeparam>
[PublicAPI]
public class LazyMethod<R> : LazyMethod
{
    /// <inheritdoc />
    public LazyMethod(string name, Func<MethodInfo> action) : base(name, action)
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

    /// <inheritdoc cref="LazyMethod.BySignature" />
    public new static LazyMethod<R> BySignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindMethodBySignature(signature, true)!);

    /// <inheritdoc cref="LazyMethod.ByPartialSignature" />
    public new static LazyMethod<R> ByPartialSignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindMethodBySignature(signature)!);
}