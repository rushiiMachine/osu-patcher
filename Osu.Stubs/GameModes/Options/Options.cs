using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.Options</c>
///     b20240123: <c>#=zIP$6zD_IcaL0ugvXAFTvJYG2gFLx</c>
/// </summary>
[PublicAPI]
public static class Options
{
    /// <summary>
    ///     Original: <c>CanExpand</c> (property getter)
    ///     b20240123: <c>#=zxcgzxWXF$WLr</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> GetCanExpand = LazyMethod<bool>.ByPartialSignature(
        "osu.GameModes.Options.Options.Options::get_CanExpand()",
        new[]
        {
            Ldc_I4,
            Stloc_1,
            Ldsfld,
            Ldloc_1,
            Stloc_3,
            Stloc_2,
            Ldloc_2,
            Ldloc_3,
            And,
            Ldc_I4_0,
            Cgt,
            Ret,
        }
    );
}