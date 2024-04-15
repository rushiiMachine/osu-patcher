using System.Collections.Generic;
using Osu.Utils.Extensions;

namespace Osu.Patcher.Hook.Patches.CustomStrings;

/// <summary>
///     Store custom string values to be injected into the game later.
/// </summary>
internal static class CustomStrings
{
    /// <summary>
    ///     New OsuString value index -> new enum value name
    /// </summary>
    internal static readonly Dictionary<uint, string> OsuStringNames = new();

    /// <summary>
    ///     New OsuString value index -> the localized value.
    /// </summary>
    internal static readonly Dictionary<uint, string> OsuStrings = new();

    private static uint _lastOsuString = 0x00010000;

    /// <summary>
    ///     Add a new value to be stringified if it is not already added.
    /// </summary>
    /// <param name="name">The name of the enum value. (So <c>ToString()</c> works properly)</param>
    /// <param name="value">The hardcoded localized string value (no multi-language support)</param>
    /// <returns>The value to use to reference this entry as enums are valuetypes.</returns>
    internal static int AddOsuString(string name, string value)
    {
        if (OsuStringNames.TryGetKey(name, out var key))
            return (int)key;

        var idx = ++_lastOsuString;
        OsuStringNames.Add(idx, name);
        OsuStrings.Add(idx, value);
        return (int)idx;
    }
}