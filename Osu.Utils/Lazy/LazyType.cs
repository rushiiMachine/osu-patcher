using System;
using JetBrains.Annotations;

namespace Osu.Utils.Lazy;

/// <summary>
///     A reference to a Type that gets located at runtime and interacted with reflectively.
/// </summary>
[PublicAPI]
public class LazyType : LazyInfo<Type>
{
    private readonly Lazy<Type> _lazy;

    /// <summary>
    ///     A wrapper around Lazy for Types.
    /// </summary>
    /// <param name="name">The qualified name of this type.</param>
    /// <param name="action">The lazy action to run when the type is needed.</param>
    public LazyType(string name, Func<Type> action)
    {
        Name = name;
        _lazy = new Lazy<Type>(action);
    }

    public override string Name { get; }

    public override Type Reference => GetReference<LazyType>(Name, _lazy);

    /// <summary>
    ///     Finds a class based on it's full name including namespace.
    /// </summary>
    /// <param name="originalName">
    ///     <see cref="LazyInfo{T}.Name" />
    /// </param>
    /// <param name="runtimeName">
    ///     The full runtime name to search for, including any namespaces. If this isn't different from
    ///     <paramref name="originalName" /> then it can be omitted or null.
    /// </param>
    public static LazyType ByName(string originalName, string? runtimeName = null) =>
        new(originalName, () => OsuAssembly.GetType(runtimeName ?? originalName));
}