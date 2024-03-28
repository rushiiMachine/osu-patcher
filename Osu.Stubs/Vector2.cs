using System;
using JetBrains.Annotations;
using Osu.Utils;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>Microsoft.Xna.Framework.Vector2</c>
///     b20240123: <c>Microsoft.Xna.Framework.Vector2</c>
///     Most names are present because of this class is [Serializable].
/// </summary>
[UsedImplicitly]
public class Vector2
{
    [UsedImplicitly]
    public static Type RuntimeType = OsuAssembly.GetType("Microsoft.Xna.Framework.Vector2");

    /// <summary>
    ///     Constructor that creates a Vector2 from two distinct float values.
    ///     Original: <c>Vector2(float, float)</c>
    ///     b20240123: <c>Vector2(Single, Single)</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<object> Constructor = new(
        "Vector2#<init>(float, float)",
        () => RuntimeType.GetConstructor([typeof(float), typeof(float)])
    );

    /// <summary>
    ///     Original: <c>X</c>
    ///     b20240123: <c>X</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<float> X = new(
        "Vector2#X",
        () => RuntimeType.GetField("X")
    );

    /// <summary>
    ///     Original: <c>Y</c>
    ///     b20240123: <c>Y</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<float> Y = new(
        "Vector2#Y",
        () => RuntimeType.GetField("Y")
    );
}