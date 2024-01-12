using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Play.Rulesets.Ruleset</c>
    ///     b20240102.2: <c>#=z8mSlkUpuRC6KDvdR9pbSfHun2YyH$ssOgS3xz0baP2jtyDzihdAlZOk=</c>
    /// </summary>
    internal static class Ruleset
    {
        /// <summary>
        ///     Original: <c>ResetScore(bool storeStatistics)</c>
        ///     b20240102.2: <c>#=z1wtsCmhcWTw$</c>
        /// </summary>
        public static readonly LazySignature ResetScore = new LazySignature(
            "Ruleset#ResetScore",
            new[]
            {
                Leave_S,
                Ldloca_S,
                Constrained,
                Callvirt,
                Endfinally,
                Ldarg_0,
                Ldfld,
                Ldfld,
                Castclass,
            }
        );
    }
}