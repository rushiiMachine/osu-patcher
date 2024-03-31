using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Osu.Utils.Lazy;

/// <summary>
///     A base type for finding stuff with the use of reflection lazily.
/// </summary>
[PublicAPI]
public interface ILazy<out T> where T : MemberInfo
{
    /// <summary>
    ///     The assumed original human readable fully qualified name.
    ///     Example: <c>System.Console::WriteLine(string message)</c>
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    ///     Reference to the reflective target type.
    ///     This retrieves or finds the type when called.
    /// </summary>
    public abstract T Reference { get; }
}

[PublicAPI]
public static class LazyExtensions
{
    /// <summary>
    ///     Execute the lazy action to fill in the lazy value if not already done so.
    /// </summary>
    public static void Fill(this ILazy<MemberInfo> lazy) => _ = lazy.Reference;

    /// <summary>
    ///     Gets the result of a <see cref="Lazy{T}" /> otherwise throws appropriate exceptions.
    /// </summary>
    /// <param name="lazy">The ILazy instance, used for obtaining the ILazy implementor's name.</param>
    /// <param name="name">
    ///     <see cref="ILazy{T}.Name" />
    /// </param>
    /// <param name="realLazy">The lazy to invoke.</param>
    /// <typeparam name="T">The result type of ILazy</typeparam>
    /// <returns>The Lazy's result value</returns>
    internal static T GetReference<T>(this ILazy<T> lazy, string name, Lazy<T> realLazy)
        where T : MemberInfo
    {
        try
        {
            var value = realLazy.Value;
            if (value != null) return value;

            var lazyName = lazy.GetType().Name;
            throw new Exception($"{lazyName} result cannot be null!");
        }
        catch (Exception e)
        {
            var lazyName = lazy.GetType().Name;
            throw new AggregateException($"Failed to run {lazyName} {name}", e);
        }
    }
}