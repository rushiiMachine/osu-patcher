using System;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches.Misc;

/// <summary>
///     Hooks <c>osu.GameBase:softHandle(Exception)</c> to log all soft exceptions thrown inside osu! to our console.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
internal class LogSoftErrors : OsuPatch
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => GameBase.SoftHandle.Reference;

    [UsedImplicitly]
    [HarmonyPrefix]
    private static void Before([HarmonyArgument(0)] Exception exception) =>
        Console.WriteLine($"osu! experienced an internal error: {exception}");
}