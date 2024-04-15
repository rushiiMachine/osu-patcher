using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Helpers;

namespace Osu.Patcher.Hook.Patches.CustomStrings;

/// <summary>
///     Adds the new hardcoded <c>OsuString</c>s into the LocalisationManager.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AddLocalizedStrings
{
    // Perhaps add localization support in the future?

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => LocalisationManager.GetString.Reference;

    [HarmonyPrefix]
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static bool Before(ref string __result, [HarmonyArgument(0)] int osuString) =>
        !CustomStrings.OsuStrings.TryGetValue((uint)osuString, out __result);
}