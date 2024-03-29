using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Options.Options</c>
///     b20240123: <c>#=zIP$6zD_IcaL0ugvXAFTvJYG2gFLx</c>
/// </summary>
[UsedImplicitly]
public class Options
{
    /// <summary>
    ///     Original: <c>CanExpand</c> (property getter)
    ///     b20240123: <c>#=zxcgzxWXF$WLr</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<bool> GetCanExpand = new(
        "Options#get_CanExpand",
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