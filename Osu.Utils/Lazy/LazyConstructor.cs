using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using JetBrains.Annotations;
using Osu.Utils.IL;

namespace Osu.Utils.Lazy;

/// <summary>
///     A reference to a constructor that gets located at runtime and invoked reflectively.
/// </summary>
[PublicAPI]
public class LazyConstructor : ILazy<ConstructorInfo>
{
    private readonly Lazy<ConstructorInfo> _lazy;

    /// <summary>
    ///     Make a wrapper around Lazy for constructor.
    /// </summary>
    /// <param name="name"><see cref="ILazy{T}.Name" /> of type what this <paramref name="action" /> is returning.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    public LazyConstructor(string name, Func<ConstructorInfo> action)
    {
        Name = name;
        _lazy = new Lazy<ConstructorInfo>(action);
    }

    public string Name { get; }

    public ConstructorInfo Reference => this.GetReference(Name, _lazy);

    public override string ToString() => $"{nameof(LazyConstructor)}({Name})";

    /// <summary>
    ///     Find if not already cached and reflectively invoke this constructor to create a new instance of a class.
    /// </summary>
    /// <param name="parameters">Parameters to pass to the constructor, if any.</param>
    public object Invoke(object?[]? parameters = null) =>
        Reference.Invoke(parameters);

    /// <inheritdoc cref="LazyMethod.BySignature" />
    public static LazyConstructor BySignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindConstructorBySignature(signature, true)!);

    /// <inheritdoc cref="LazyMethod.ByPartialSignature" />
    public static LazyConstructor ByPartialSignature(string name, IReadOnlyList<OpCode> signature) =>
        new(name, () => OpCodeMatcher.FindConstructorBySignature(signature)!);
}