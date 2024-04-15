using System.Collections.Generic;
using System.Linq;

namespace Osu.Utils.Extensions;

public static class Extensions
{
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