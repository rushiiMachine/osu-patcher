using System.Collections.Generic;
using System.Linq;

namespace Osu.Utils.Extensions;

public static class Extensions
{
    /// <summary>
    ///     Find the key for a value. This is inefficient.
    /// </summary>
    /// <param name="dict">The dictionary to search in.</param>
    /// <param name="value">Value to search for.</param>
    /// <param name="key">Out value for the found key if returning true, otherwise <c>default</c>.</param>
    /// <returns>True if found.</returns>
    public static bool TryGetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value, out TKey key)
    {
        foreach (var pair in dict)
        {
            if (!EqualityComparer<TValue>.Default.Equals(pair.Value, value))
                continue;

            key = pair.Key;
            return true;
        }

        key = default!;
        return false;
    }

    /// <summary>
    ///     Filters out null values in this sequence.
    /// </summary>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> sequence) where T : class =>
        sequence.Where(e => e != null)!;

    /// <summary>
    ///     Gets the first value in the sequence or null if none exist.
    /// </summary>
    public static T? FirstOrNull<T>(this IEnumerable<T> sequence)
        where T : class
    {
        using var enumerator = sequence.GetEnumerator();
        return enumerator.MoveNext() ? enumerator.Current : null;
    }
}