using System;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Play.Components.ScoreDisplay</c>
///     b20240124: <c>#=z6dniqZasYGnUF21A3FQQhhWHV7POD$6AVg==</c>
/// </summary>
[UsedImplicitly]
public class ScoreDisplay
{
    /// <summary>
    ///     Original:
    ///     <c>
    ///         ScoreDisplay(SpriteManager spriteManager,
    ///         Vector2 position,
    ///         bool alignRight,
    ///         float scale,
    ///         bool showScore,
    ///         bool showAccuracy)
    ///     </c>
    ///     b20240124: Same as class
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod Constructor = new(
        "ScoreDisplay::<init>(...)",
        new[]
        {
            Conv_R4,
            Ldarg_3,
            Brtrue_S,
            Ldc_I4_6,
            Br_S,
            Ldc_I4_8,
            Ldarg_3,
            Brtrue_S,
            Ldc_I4_0,
            Br_S,
        },
        true
    );

    /// <summary>
    ///     Original: <c>Hide()</c>
    ///     b20240123: <c>#=zRjDThRI=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod Hide = new( // TODO: support hiding performance counter
        "ScoreDisplay#Hide()",
        new[]
        {
            Ldc_I4_0,
            Ldc_I4_0,
            Ldc_I4_0,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
            Ldc_I4_0,
            Ldc_I4_0,
            Ldc_I4_0,
            Callvirt,
            Ret,
        }
    );

    /// <summary>
    ///     Original: <c>spriteManager</c>
    ///     b20240123: <c>#=zK4XquDeTazcx</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object> SpriteManager = new(
        "ScoreDisplay#spriteManager",
        () => RuntimeType.GetDeclaredFields()
            .Single(field => field.FieldType == Stubs.SpriteManager.RuntimeType)
    );

    [UsedImplicitly]
    public static Type RuntimeType => Constructor.Reference.DeclaringType!;
}