using System;
using System.Diagnostics;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;

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

                NotificationsUtil.ShowMessage("osu!patcher initialized!", Color.WhiteSmoke, 5000,
                    () => { Process.Start(GithubUrl); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                try
                {
                    NotificationsUtil.ShowMessage("osu!patcher experienced an error! Click to report.", Color.Red,
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

    // [HarmonyPatch]
    // internal class PatchRemoveModCombinationRestrictions : Patch
    // {
    //     private const string clz = "#=zA68w2LnfHk3bAvNoTjj7pqCRs0P7Q2WkMrK0LXo=";
    //
    //     private static Mods selectedModsRef = AccessTools.StaticFieldRefAccess<Mods>($"{clz}:#=zxGXFDdk=");
    //
    //     private static MethodBase TargetMethod()
    //     {
    //         // Search for
    //         // ~(Mods.Key4 | Mods.Key5 | Mods.Key6 | Mods.Key7 | Mods.Key8 | Mods.FadeIn | Mods.Random | Mods.Key9 | Mods.KeyCoop | Mods.Key1 | Mods.Key3 | Mods.Key2 | Mods.Mirror)
    //
    //         const string mtd = "#=zxGgySG2NcJR6K_KRKQ==";
    //         return AccessTools.Method($"{clz}:{mtd}", new[] { typeof(Mods) });
    //     }
    //
    //     private static bool Prefix([HarmonyArgument(0)] Mods mods)
    //     {
    //         selectedModsRef = mods;
    //         return false;
    //     }
    // }

    // [HarmonyPatch]
    // internal class PatchFixRelaxScoreMultiplier : Patch
    // {
    //     private static MethodBase TargetMethod()
    //     {
    //         // Search for:
    //         // case PlayModes.Osu:
    //         // case PlayModes.Taiko:
    //
    //         const string
    //             clz = "#=zA68w2LnfHk3bAvNoTjj7pqCRs0P7Q2WkMrK0LXo=",
    //             mtd = "#=z_gY4$2rMOiN4";
    //
    //         return AccessTools.Method($"{clz}:{mtd}",
    //             new[]
    //             {
    //                 typeof(Mods),
    //                 typeof(PlayModes),
    //                 AccessTools.TypeByName("#=zkdhZ0xuyvtdonL9gD6UYabtvEflJOyazS1zegavU_9KJ")
    //             });
    //     }
    //
    //     private static void Prefix([HarmonyArgument(0)] ref Mods mods)
    //     {
    //         mods &= ~(Mods.Relax | Mods.Relax2);
    //     }
    // }
}