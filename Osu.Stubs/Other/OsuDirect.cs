using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Other;

/// <summary>
///     Original: <c>osu.Online.OsuDirect</c>
///     b20240102.2: <c>#=zz3BVwBujgm4LKna55Dwx3UA=</c>
/// </summary>
[PublicAPI]
public static class OsuDirect
{
    /// <summary>
    ///     Original: <c>HandlePickup(...)</c>
    ///     b20240102.2: <c>#=zJczNz_3lxxrQQnnZog==</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod HandlePickup = LazyMethod.ByPartialSignature(
        "osu.Online.OsuDirect::HandlePickup(...)",
        [
            Ldloc_0,
            Ldfld,
            Ldloc_0,
            Ldflda,
            Call,
            Ldloc_0,
            Ldftn,
            Newobj,
            Call,
            Ret,
        ]
    );
}