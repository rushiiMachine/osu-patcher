using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.GameplayElements.Scoring;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Play.Rulesets;

[PublicAPI]
public static class Ruleset
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Play.Rulesets.Ruleset</c>
    ///     b20240123: <c>#=zwRa71lIOJzp$VMz5GAIPMrt1N4rdQ4gC2Fx1Jtw=</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameModes.Play.Rulesets.Ruleset",
        () => OnIncreaseScoreHit!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>Fail(bool continuousPlay)</c>
    ///     b20240123: <c>#=zPAIY7AY=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Fail = LazyMethod.ByPartialSignature(
        "osu.GameModes.Play.Rulesets.Ruleset::Fail(bool)",
        [
            Brtrue_S,
            Leave_S,
            Ldloca_S,
            Constrained,
            Callvirt,
            Endfinally,
            Call,
            Ldc_I4_1,
        ]
    );

    /// <summary>
    ///     Original: <c>OnIncreaseScoreHit(IncreaseScoreType ist, double hpIncrease, bool increaseCombo, HitObject h)</c>
    ///     b20240123: <c>#=zH8JNJ0F2$AwN</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod OnIncreaseScoreHit = LazyMethod.ByPartialSignature(
        "osu.GameModes.Play.Rulesets.Ruleset::OnIncreaseScoreHit(IncreaseScoreType, double, bool, HitObject)",
        [
            Ldarg_3,
            Brfalse_S,
            Ldarg_0,
            Call,
            Ldarg_2,
            Ldc_R8,
            Bgt_Un_S,
            Ret,
            Ldarg_0,
        ]
    );

    /// <summary>
    ///     Original: <c>ResetScore(bool storeStatistics)</c>
    ///     b20240123: <c>#=zLIXqTYl0LIQz</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod ResetScore = LazyMethod.ByPartialSignature(
        "osu.GameModes.Play.Rulesets.Ruleset::ResetScore(bool)",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>Initialize()</c>
    ///     b20240123: <c>#=znhXnLb4=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Initialize = LazyMethod.ByPartialSignature(
        "osu.GameModes.Play.Rulesets.Ruleset::Initialize()",
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
    [Stub]
    public static readonly LazyField<object> Instance = new(
        "osu.GameModes.Play.Rulesets.Ruleset::Instance",
        () => Class.Reference.GetRuntimeFields()
            .Single(field => field.FieldType == Class.Reference)
    );

    /// <summary>
    ///     Original: <c>CurrentScore</c>
    ///     b20240123: <c>#=zk4sdboE=</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object> CurrentScore = new(
        "osu.GameModes.Play.Rulesets.Ruleset::CurrentScore",
        () => Class.Reference.GetRuntimeFields()
            .Single(field => field.FieldType == Score.Class.Reference)
    );

    /// <summary>
    ///     Original: <c>ScoreDisplay</c>
    ///     b20240123: <c>#=znVUsSpw8w4aW</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object?> ScoreDisplay = new(
        "osu.GameModes.Play.Rulesets.Ruleset::ScoreDisplay",
        () => Class.Reference.GetRuntimeFields()
            .Single(field => field.FieldType == Play.ScoreDisplay.Class.Reference)
    );
}