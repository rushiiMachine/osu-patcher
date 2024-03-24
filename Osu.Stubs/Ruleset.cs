using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Play.Rulesets.Ruleset</c>
///     b20240102.2: <c>#=z35zTtWacH8qc$rYRcsQ6iGtK1MBEurfpzNOeLiQ=</c>
/// </summary>
public static class Ruleset
{
    /// <summary>
    ///     Original: <c>Fail(bool continuousPlay)</c>
    ///     b20240102.2: <c>#=zWoTE_tk=</c>
    /// </summary>
    public static LazyMethod Fail = new(
        "Ruleset#Fail(...)",
        new[]
        {
            Brtrue_S,
            Leave_S,
            Ldloca_S,
            Constrained,
            Callvirt,
            Endfinally,
            Call,
            Ldc_I4_1,
        }
    );
}