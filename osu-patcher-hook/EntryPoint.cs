using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;

namespace osu_patcher_hook
{
    public class EntryPoint
    {
        private static Harmony _harmony;

        public static int Initialize(string _)
        {
            var stream = new FileStream("C:\\osu!\\Logs\\patcher.txt", FileMode.Create);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            Console.SetOut(writer);
            Console.SetError(writer);

            try
            {
                _harmony = new Harmony("io.github.rushiimachine.osu-patcher");
                _harmony.PatchAll(typeof(EntryPoint).Assembly);

                NotificationsUtil.ShowMessage("osu!patcher initialized!", Color.WhiteSmoke, 5000,
                    () => { Process.Start("https://github.com/rushiiMachine/osu-patcher"); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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