using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Online.OsuDirect</c>
///     b20240102.2: <c>#=zz3BVwBujgm4LKna55Dwx3UA=</c>
/// </summary>
public abstract class OsuDirect
{
    /// <summary>
    ///     Original: <c>HandlePickup()</c>
    ///     b20240102.2: <c>#=zJczNz_3lxxrQQnnZog==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod HandlePickup = new(
        "osu.Online.OsuDirect#HandlePickup",
        new[]
        {
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
        }
    );
}