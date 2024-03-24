using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameBase</c>
///     b20240123: <c>#=zduF3QmjgMG4eSc$fOQ==</c>
/// </summary>
[UsedImplicitly]
public class GameBase
{
    /// <summary>
    ///     Original: <c>softHandle(Exception e)</c>
    ///     b20240123: <c>#=z8BJOiJxSLUwM</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod SoftHandle = new(
        "GameBase#softHandle(...)",
        new[]
        {
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
        }
    );
}