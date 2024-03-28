using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;
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
    public static readonly LazyMethod Fail = new(
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

    /// <summary>
    ///     Original: <c>OnIncreaseScoreHit(IncreaseScoreType ist, double hpIncrease, bool increaseCombo, HitObject h)</c>
    ///     b20240123: <c>#=zH8JNJ0F2$AwN</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod OnIncreaseScoreHit = new(
        "Ruleset#OnIncreaseScoreHit(...)",
        new[]
        {
            Ldarg_3,
            Brfalse_S,
            Ldarg_0,
            Call,
            Ldarg_2,
            Ldc_R8,
            Bgt_Un_S,
            Ret,
            Ldarg_0,
        }
    );

    /// <summary>
    ///     Original: <c>ResetScore(bool storeStatistics)</c>
    ///     b20240123: <c>#=zLIXqTYl0LIQz</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod ResetScore = new(
        "Ruleset#ResetScore(...)",
        new[]
        {
            Ldfld,
            Ldarg_1,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Brtrue_S,
            Ret,
            Ldarg_0,
            Ldfld,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
            Callvirt,
        }
    );

    /// <summary>
    ///     Original: <c>Initialize()</c>
    ///     b20240123: <c></c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod Initialize = new(
        "Ruleset#Initialize()",
        new[]
        {
            Ldarg_0,
            Callvirt,
        }.Duplicate(8)
    );

    /// <summary>
    ///     Original: <c>CurrentScore</c>
    ///     b20240123: <c>Instance</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object> Instance = new(
        "Ruleset#Instance",
        () => RuntimeType.GetRuntimeFields()
            .Single(field => field.FieldType == RuntimeType)
    );

    /// <summary>
    ///     Original: <c>CurrentScore</c>
    ///     b20240123: <c>#=zk4sdboE=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object> CurrentScore = new(
        "Ruleset#CurrentScore",
        () => RuntimeType.GetRuntimeFields()
            .Single(field => field.FieldType == Score.RuntimeType)
    );

    /// <summary>
    ///     Original: <c>ScoreDisplay</c>
    ///     b20240123: <c></c> TODO: find this
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object?> ScoreDisplay = new(
        "Ruleset#ScoreDisplay",
        () => RuntimeType.GetRuntimeFields()
            .Single(field => field.FieldType == Stubs.ScoreDisplay.RuntimeType)
    );

    [UsedImplicitly]
    public static Type RuntimeType => OnIncreaseScoreHit.Reference.DeclaringType!;
}