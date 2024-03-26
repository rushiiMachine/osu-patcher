using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Play.Player</c>
///     b20240124: <c>#=zOTWUr4vq60U15SRmD_JItyatbhdR</c>
/// </summary>
[UsedImplicitly]
public static class Player
{
    /// <summary>
    ///     Original: <c>AllowDoubleSkip.get</c> (property getter)
    ///     b20240124: <c>#=zp29IlAJ43g4WRArPQA==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod GetAllowDoubleSkip = new(
        "Player#AllowDoubleSkip.get",
        new[]
        {
            Neg,
            Stloc_0,
            Ldarg_0,
            Isinst,
            Brtrue_S,
            Ldsfld,
            Ldloc_0,
            Call,
            Brtrue_S,
            Ldc_I4_0,
            Br_S,
        }
    );

    /// <summary>
    ///     Original: <c>currentScore</c>
    ///     b20240124: <c>#=zF6h5l4j0$TfX</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object?> CurrentScore = new(
        "Player#currentScore",
        () => RuntimeType
            .GetFields(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(field => field.FieldType == Score.RuntimeType)
    );

    [UsedImplicitly]
    public static Type RuntimeType => GetAllowDoubleSkip.Reference.DeclaringType!;
}