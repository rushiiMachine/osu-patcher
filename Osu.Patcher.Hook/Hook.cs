using System;
using System.Diagnostics;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Performance.ROsu;
using Osu.Utils;

namespace Osu.Patcher.Hook;

[UsedImplicitly]
public static class Hook
{
    private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
    private static Harmony _harmony = null!;

    /// <summary>
    ///     Entry point into the hook called by the injector.
    /// </summary>
    [UsedImplicitly]
    public static int Initialize(string _)
    {
        ConsoleHook.Initialize();

        try
        {
            Console.WriteLine(Test.test());
            _harmony = new Harmony("osu!patcher");
            InitializePatches(_harmony);

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
            try
            {
                // This executes for every type and ignores if no [HarmonyPatch] is present
                harmony.CreateClassProcessor(type).Patch();
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