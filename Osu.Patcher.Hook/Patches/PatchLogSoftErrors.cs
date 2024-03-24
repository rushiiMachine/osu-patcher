using System;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Hooks <c>osu.GameBase:softHandle(Exception)</c> to log all soft exceptions thrown inside osu! to our console.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
public class PatchLogSoftErrors
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => GameBase.SoftHandle.Reference;

    [UsedImplicitly]
    [HarmonyPrefix]
    private static void Before([HarmonyArgument(0)] Exception exception) =>
        Console.WriteLine($"osu! experienced an internal error: {exception}");
}