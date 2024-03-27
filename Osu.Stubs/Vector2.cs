using System;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;

namespace Osu.Stubs;

[UsedImplicitly]
public class Vector2
{
    [UsedImplicitly]
    public static Type RuntimeType = OsuAssembly.GetType("Microsoft.Xna.Framework.Vector2");

    /// <summary>
    ///     Constructor that creates a Vector2 from two distinct float values.
    /// </summary>
    [UsedImplicitly]
    public static LazyMethod<object> Constructor = new(
        "Vector2#<init>(float, float)",
        () => RuntimeType.GetConstructor([typeof(float), typeof(float)])
    );

    [UsedImplicitly]
    public static LazyField<float> X = new(
        "Vector2#X",
        () => RuntimeType.GetField("X")
    );

    [UsedImplicitly]
    public static LazyField<float> Y = new(
        "Vector2#Y",
        () => RuntimeType.GetField("Y")
    );
}