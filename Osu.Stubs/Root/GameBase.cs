using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Root;

/// <summary>
///     Original: <c>osu.GameBase</c>
///     b20240123: <c>#=zduF3QmjgMG4eSc$fOQ==</c>
/// </summary>
[PublicAPI]
public static class GameBase
{
    /// <summary>
    ///     Original: <c>get_ModeCanReload()</c>
    ///     b20240123: <c>#=zL6aRJUMxZO5fmlF9KQ==</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> GetModeCanReload = LazyMethod<bool>.ByPartialSignature(
        "osu.GameBase::get_ModeCanReload()",
        [
            Ldc_I4_7,
            Bgt_S,
            Ldloc_0,
            Ldc_I4_2,
            Beq_S,
            Ldloc_0,
            Ldc_I4_7,
            Beq_S,
            Br_S,
        ]
    );

    /// <summary>
    ///     Original: <c>softHandle(Exception e)</c>
    ///     b20240123: <c>#=z8BJOiJxSLUwM</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SoftHandle = LazyMethod.ByPartialSignature(
        "osu.GameBase::softHandle(Exception)",
        [
            Ldarg_0,
            Isinst,
            Brtrue_S,
            Ldarg_0,
            Isinst,
            Brtrue_S,
            Ldarg_0,
            Isinst,
            Brfalse_S,
            Ldc_I4_8,
        ]
    );
}