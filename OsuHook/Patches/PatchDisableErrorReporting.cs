using System.Reflection;
using HarmonyLib;
using OsuHook.Osu.Stubs;

// ReSharper disable UnusedType.Global UnusedMember.Local

namespace OsuHook.Patches
{
    /// <summary>
    ///     Disable the error reporter to prevent sentry from being spammed with errors possibly caused by this patcher.
    /// </summary>
    [HarmonyPatch]
    internal class PatchDisableErrorReporting : BasePatch
    {
        [HarmonyTargetMethod]
        private static MethodBase Target() => ErrorSubmission.Submit.Reference;

        /// <summary>
        ///     No-op the entire method
        /// </summary>
        [HarmonyPrefix]
        private static bool Before() => false;
    }
}