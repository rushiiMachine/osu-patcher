using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Scoring.Processors.ScoreChange</c>
///     b20240123: <c>#=zLpbYBJJuhPuc612JcL6v88v6458xNWATpDbhSpQKC6L_</c>
/// </summary>
[UsedImplicitly]
public class ScoreChange
{
    /// <summary>
    ///     Original: <c>HitValue</c>
    ///     b20240123: <c>#=zifzkx$8=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<int> HitValue = new(
        "ScoreChange#HitValue",
        () => RuntimeType.GetFields()
            .First(field => field.FieldType == IncreaseScoreType.RuntimeType)
    );

    // Used as the first parameter to ScoreProcessor#Add(...)
    [UsedImplicitly]
    public static Type RuntimeType => ScoreProcessor.AddScoreChange.Reference
        .GetParameters()[0]
        .ParameterType;

    [UsedImplicitly]
    public static ConstructorInfo PrimaryConstructor => RuntimeType.GetConstructors()
        .Single(ctor => ctor.GetParameters().Length == 3);
}