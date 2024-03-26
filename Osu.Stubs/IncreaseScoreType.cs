using System;
using JetBrains.Annotations;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Play.Rulesets.IncreaseScoreType</c>
///     b20240123: <c>#=zGMiEmtRx7tdNjrx8B18dVB33Br1S9OJ2_XDs0XJSAtWZ</c>
/// </summary>
[UsedImplicitly]
public class IncreaseScoreType
{
    // TODO: reverse engineer all enum values

    public const int Osu50 = 256;
    public const int Osu100 = 512;
    public const int Osu300 = 1024;

    // Used as the first parameter in the constructor for ScoreChange
    [UsedImplicitly]
    public static Type RuntimeType => ScoreChange.PrimaryConstructor
        .GetParameters()[0]
        .ParameterType;
}