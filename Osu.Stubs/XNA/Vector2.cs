using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs.XNA;

/// <summary>
///     Most names are present because this class is <c>[Serializable]</c>.
/// </summary>
[PublicAPI]
public static class Vector2
{
    /// <summary>
    ///     Original: <c>Microsoft.Xna.Framework.Vector2</c>
    ///     b20240123: <c>Microsoft.Xna.Framework.Vector2</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = LazyType.ByName("Microsoft.Xna.Framework.Vector2");

    /// <summary>
    ///     Constructor that creates a Vector2 from two distinct float values.
    ///     Original: <c>Vector2(float, float)</c>
    ///     b20240123: <c>Vector2(Single, Single)</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "Microsoft.Xna.Framework.Vector2::Vector2(float, float)",
        () => Class.Reference.GetConstructor([typeof(float), typeof(float)])
    );

    /// <summary>
    ///     Original: <c>X</c>
    ///     b20240123: <c>X</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<float> X = new(
        "Microsoft.Xna.Framework.Vector2::X",
        () => Class.Reference.GetField("X")
    );

    /// <summary>
    ///     Original: <c>Y</c>
    ///     b20240123: <c>Y</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<float> Y = new(
        "Microsoft.Xna.Framework.Vector2::Y",
        () => Class.Reference.GetField("Y")
    );
}