namespace osu_patcher_hook.patches
{
    // TODO: make this patch allow only Sudden Death/Perfect *client side* only
    // ie. store real selected mods independently of Player.CurrentScore and make the player exit if conditions met
    //     sending invalid mods to server = ez ban

    // [HarmonyPatch]
    // internal class PatchRemoveModCombinationRestrictions
    // {
    //     private const string clz = "#=zA68w2LnfHk3bAvNoTjj7pqCRs0P7Q2WkMrK0LXo=";
    //
    //     private static Mods selectedModsRef = AccessTools.StaticFieldRefAccess<Mods>($"{clz}:#=zxGXFDdk=");
    //
    //     [HarmonyTargetMethod]
    //     private static MethodBase Target()
    //     {
    //         const string mtd = "#=zxGgySG2NcJR6K_KRKQ==";
    //         return AccessTools.Method($"{clz}:{mtd}", new[] { typeof(Mods) });
    //     }
    //
    //     [HarmonyPrefix]
    //     private static bool Before([HarmonyArgument(0)] Mods mods)
    //     {
    //         selectedModsRef = mods;
    //         return false;
    //     }
    // }
}