#if DEBUG
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Hook <c>osu_common.Helpers.Logger:Log</c> to print the live log to our console.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
public static class PatchDebugLogger
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Logger.Log.Reference;

    [HarmonyPrefix]
    [UsedImplicitly]
    private static void Before(
        [HarmonyArgument(0)] string message,
        [HarmonyArgument(1)] LoggingTarget target,
        [HarmonyArgument(2)] LogLevel level
    ) => Console.WriteLine("{0} - {1}: {2}", level.ToString().First(), target, message);

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private enum LogLevel
    {
        Debug,
        Verbose,
        Important,
        Error,
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private enum LoggingTarget
    {
        Runtime,
        Network,
        Tournament,
        Update,
        Performance,
        Debug,
    }
}
#endif