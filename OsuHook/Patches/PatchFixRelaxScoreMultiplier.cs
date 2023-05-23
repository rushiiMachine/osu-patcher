namespace OsuHook.Patches
{
    // TODO: change this patch to be client-side ui only
    // ie. calculate score independently of actual Player.CurrentScore state
    //     and hook the renderer for score to draw the real score
    //     because changing score data = ez ban

    // [HarmonyPatch]
    // internal class PatchFixRelaxScoreMultiplier
    // {
    //     [HarmonyTargetMethod]
    //     private static MethodBase Target()
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
    //     [HarmonyPrefix]
    //     private static void Before([HarmonyArgument(0)] ref Mods mods)
    //     {
    //         mods &= ~(Mods.Relax | Mods.Relax2);
    //     }
    // }
}