using JetBrains.Annotations;

namespace Osu.Utils.Extensions;

public static class ArrayExtensions
{
    [UsedImplicitly]
    public static T? GetOrDefault<T>(this T[] array, int index, T? defaultValue) =>
        index < array.Length ? array[index] : defaultValue;

    [UsedImplicitly]
    public static T[] Duplicate<T>(this T[] array, int times)
    {
        var newArray = new T[array.Length * times];

        for (var i = 0; i < times; i++)
            array.CopyTo(newArray, i * array.Length);

        return newArray;
    }
}