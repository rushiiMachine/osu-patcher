using JetBrains.Annotations;

namespace Osu.Stubs.Utils;

public static class Extensions
{
    [UsedImplicitly]
    public static T? GetOrDefault<T>(this T[] array, int index, T? defaultValue) =>
        index < array.Length ? array[index] : defaultValue;
}