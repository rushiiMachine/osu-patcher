using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Osu.Utils.Lazy;

/// <summary>
///     A base type for finding stuff with the use of reflection lazily.
/// </summary>
[PublicAPI]
public abstract class LazyInfo<T> where T : MemberInfo
{
    /// <summary>
    ///     The assumed original human readable fully qualified name.
    ///     Example: <c>System.Console::WriteLine(string message)</c>
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    ///     The runtime type name of the resolved reflective type.
    /// </summary>
    public string RuntimeName => Reference.Name;

    /// <summary>
    ///     Reference to the reflective target type.
    ///     This retrieves or finds the type when called.
    /// </summary>
    public abstract T Reference { get; }

    /// <summary>
    ///     Gets the result of a <see cref="Lazy{T}" /> otherwise throws appropriate exceptions.
    /// </summary>
    /// <param name="name">
    ///     <see cref="Name" />
    /// </param>
    /// <param name="lazy">The lazy to invoke.</param>
    /// <typeparam name="TL">Subtype of LazyInfo.</typeparam>
    /// <returns>The Lazy's result value</returns>
    protected static T GetReference<TL>(string name, Lazy<T> lazy)
        where TL : LazyInfo<T>
    {
        try
        {
            var value = lazy.Value;
            if (value != null) return value;

            var lazyName = typeof(TL).Name;
            throw new Exception($"{lazyName} result cannot be null!");
        }
        catch (Exception e)
        {
            var lazyName = typeof(TL).Name;
            throw new AggregateException($"Failed to run {lazyName} {name}", e);
        }
    }
}