using System;
using System.Diagnostics;
using HarmonyLib;
using OsuHook.Osu;

// ReSharper disable UnusedType.Global UnusedMember.Global

namespace OsuHook
{
    public class MainHook
    {
        private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
        private static Harmony _harmony;

        /// <summary>
        ///     Entry point into the hook called by the injector.
        /// </summary>
        public static int Initialize(string _)
        {
            ConsoleHook.Initialize();
            ExceptionHook.Initialize();

            try
            {
                _harmony = new Harmony("io.github.rushiimachine.osu-patcher");
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
            foreach (var type in AccessTools.GetTypesFromAssembly(typeof(MainHook).Assembly))
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
}