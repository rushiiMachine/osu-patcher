using JetBrains.Annotations;

namespace Osu.Utils.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    ///     Get the value at an array index or a default value if not in range.
    /// </summary>
    /// <param name="array">Source array</param>
    /// <param name="index">Potential index that could exist</param>
    /// <param name="defaultValue">Alternative default value</param>
    /// <typeparam name="T">A non-null type inside the array.</typeparam>
    /// <returns>The value or default value</returns>
    [UsedImplicitly]
    public static T? GetOrDefault<T>(this T[] array, int index, T? defaultValue) =>
        index < array.Length ? array[index] : defaultValue;

    /// <summary>
    ///     Duplicate all the values in an array a certain amount of times.
    /// </summary>
    /// <param name="array">Source array</param>
    /// <param name="times">Amount of times to copy the items.</param>
    /// <returns>A new array with shallow copied items.</returns>
    [UsedImplicitly]
    public static T[] Duplicate<T>(this T[] array, int times)
    {
        var newArray = new T[array.Length * times];

        for (var i = 0; i < times; i++)
            array.CopyTo(newArray, i * array.Length);

        return newArray;
    }
}