using System.IO;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Helpers;
using Osu.Utils;

namespace Osu.Patcher.Hook.Patches.PatcherOptions;

/// <summary>
///     Hooks osu!'s datastore saving mechanism to save our own options every so often.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class SaveOptions
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => DataStoreUpdater.Save.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    internal static void Save()
    {
        var osuDir = Path.GetDirectoryName(OsuAssembly.Assembly.Location)!;
        var settings = new Settings();

        // Write settings to this instance
        Hook.PatchOptions.Do(opts => opts.Save(settings));

        Settings.WriteToDisk(settings, osuDir);
    }
}