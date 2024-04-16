using System;

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
    public static T? GetOrDefault<T>(this T[] array, int index, T? defaultValue) =>
        index < array.Length ? array[index] : defaultValue;

    /// <summary>
    ///     Duplicate all the values in an array a certain amount of times.
    /// </summary>
    /// <param name="array">Source array</param>
    /// <param name="times">Amount of times to copy the items.</param>
    /// <returns>A new array with shallow copied items.</returns>
    public static T[] Duplicate<T>(this T[] array, int times)
    {
        var newArray = new T[array.Length * times];

        for (var i = 0; i < times; i++)
            array.CopyTo(newArray, i * array.Length);

        return newArray;
    }

    /// <summary>
    ///     Copy an array of elements to a new array of a type only known at runtime.
    /// </summary>
    /// <param name="srcArray">Original array. All the elements should be assignable to the new target type.</param>
    /// <param name="newType">The type to create the new array as.</param>
    /// <returns>A copy of the array with a different parameterized type.</returns>
    public static Array ToType(this Array srcArray, Type newType)
    {
        var typedArray = Array.CreateInstance(newType, srcArray.Length);

        for (var i = 0; i < typedArray.Length; i++)
            typedArray.SetValue(index: i, value: srcArray.GetValue(i));

        return typedArray;
    }
}