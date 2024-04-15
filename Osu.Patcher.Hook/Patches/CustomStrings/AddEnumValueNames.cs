using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Helpers;

namespace Osu.Patcher.Hook.Patches.CustomStrings;

/// <summary>
///     Patch <c>Enum::ToString()</c> to add additional enum values that can be stringified.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AddEnumValueNames
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => typeof(Enum).Method(nameof(Enum.GetName));

    [HarmonyPrefix]
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static bool Before(
        ref string __result,
        [HarmonyArgument(0)] Type enumType,
        [HarmonyArgument(1)] object? value)
    {
        if (value == null) return true;

        var valueIdx = Convert.ToUInt32(value);

        if (enumType == OsuString.Class.Reference)
            return !CustomStrings.OsuStringNames.TryGetValue(valueIdx, out __result);

        return true;
    }
}