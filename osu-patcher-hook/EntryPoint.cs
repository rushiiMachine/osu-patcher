using System;
using System.Diagnostics;
using HarmonyLib;

namespace osu_patcher_hook
{
    public class EntryPoint
    {
        private const string GithubUrl = "https://github.com/rushiiMachine/osu-patcher";
        private static Harmony _harmony;

        public static int Initialize(string _)
        {
            ConsoleUtil.InitConsoleWriteHooks();

            try
            {
                _harmony = new Harmony("io.github.rushiimachine.osu-patcher");
                _harmony.PatchAll(typeof(EntryPoint).Assembly);

                NotificationsUtil.ShowMessage(
                    "osu!patcher initialized!",
                    AccessTools.PropertyGetter("Microsoft.Xna.Framework.Graphics.Color:WhiteSmoke")
                        .Invoke(null, null),
                    5000,
                    () => { Process.Start(GithubUrl); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                try
                {
                    NotificationsUtil.ShowMessage(
                        "osu!patcher experienced an error! Click to report.",
                        AccessTools.PropertyGetter("Microsoft.Xna.Framework.Graphics.Color:Red")
                            .Invoke(null, null),
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