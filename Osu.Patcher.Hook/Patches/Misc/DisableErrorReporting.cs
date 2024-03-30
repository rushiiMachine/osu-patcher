using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.Misc;

/// <summary>
///     Disable the error reporter to prevent sentry from being spammed with errors possibly caused by this patcher.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class DisableErrorReporting : OsuPatch
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => ErrorSubmission.Submit.Reference;

    /// <summary>
    ///     No-op the entire method
    /// </summary>
    [UsedImplicitly]
    [HarmonyPrefix]
    private static bool Before() => false;
}