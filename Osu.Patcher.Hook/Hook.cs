using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Patcher.Hook.Patches;
using Osu.Performance;
using Osu.Stubs.Wrappers;
using Osu.Utils;

namespace Osu.Patcher.Hook;

[UsedImplicitly]
public static class Hook
{
    private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
    private static Harmony _harmony = null!;

    /// <summary>
    ///     An instance of all patch options that have been initialized.
    /// </summary>
    public static IReadOnlyList<PatchOptions> PatchOptions { get; private set; } = null!;

    /// <summary>
    ///     Entry point into the hook called by the injector.
    /// </summary>
    [UsedImplicitly]
    public static int Initialize(string _)
    {
        ConsoleHook.Initialize();

        try
        {
            Notifications.ShowMessage(
                "Loading osu!patcher...",
                NotificationColor.Neutral,
                3000);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

#if DEBUG
        DebugHook.Initialize();

        try
        {
            // MSBuild is flimsy with building rosu-ffi on up-to-date builds, try linking early
            Marshal.PrelinkAll(typeof(Native));
        }
        catch (Exception e)
        {
            Console.WriteLine($"MSBuild broke again; clean & rebuild: {e}");
            ShowErrorNotification();
            return 0;
        }
#endif

        try
        {
            var osuDir = Path.GetDirectoryName(OsuAssembly.Assembly.Location)!;

            // Load settings
            var settings = Settings.ReadFromDisk(osuDir);
            PatchOptions = Patches.PatchOptions.CreateAllPatchOptions().ToList();
            PatchOptions.Do(options => options.Load(settings));

            _harmony = new Harmony("osu!patcher");
            InitializePatches(_harmony);

            // Forcefully reload options to allow patches to inject new options
            OptionsUtils.ReloadOptions();

            Notifications.ShowMessage(
                "osu!patcher initialized!",
                NotificationColor.Neutral,
                5000,
                () => { Process.Start(GithubUrl); }
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ShowErrorNotification();
        }

        return 0;
    }

    /// <summary>
    ///     For every type annotated with <c>[HarmonyPatch]</c> in this assembly, initialize the patch.
    /// </summary>
    /// <param name="harmony">Harmony patcher instance</param>
    private static void InitializePatches(Harmony harmony)
    {
        foreach (var type in AccessTools.GetTypesFromAssembly(typeof(Hook).Assembly))
        {
            if (!OsuPatchProcessor.IsOsuPatch(type))
                continue;

            Debug.WriteLine($"Processing Patch {type.Name}", "Hook");

            try
            {
                new OsuPatchProcessor(harmony, type).Patch();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to initialize patch {type.Name}");
                Console.WriteLine(e);
                ShowErrorNotification();
            }
        }
    }

    /// <summary>
    ///     Show a generic error notification to the user.
    /// </summary>
    private static void ShowErrorNotification()
    {
        try
        {
            Notifications.ShowMessage(
                "osu!patcher experienced an error! Click to report.",
                NotificationColor.Error,
                20000,
                () => { Process.Start($"{GithubUrl}/issues"); });
        }
        catch (Exception e2)
        {
            Console.WriteLine("Failed to show error notification!");
            Console.WriteLine(e2);
        }
    }
}