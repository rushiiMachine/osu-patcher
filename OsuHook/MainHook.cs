using System;
using System.Diagnostics;
using HarmonyLib;
using OsuHook.Osu;

namespace OsuHook
{
    public class MainHook
    {
        private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
        private static Harmony _harmony;

        public static int Initialize(string _)
        {
            ConsoleHook.Initialize();

            try
            {
                _harmony = new Harmony("io.github.rushiimachine.osu-patcher");
                _harmony.PatchAll(typeof(MainHook).Assembly);

                Notifications.ShowMessage(
                    "osu!patcher initialized!",
                    Notifications.GetColor("GhostWhite"),
                    5000,
                    () => { Process.Start(GithubUrl); });
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to show error notification!");
                Console.WriteLine(e);

                try
                {
                    Notifications.ShowMessage(
                        "osu!patcher experienced an error! Click to report.",
                        Notifications.GetColor("Red"),
                        20000,
                        () => { Process.Start($"{GithubUrl}/issues"); });
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2);
                }
            }

            return 0;
        }
    }
}