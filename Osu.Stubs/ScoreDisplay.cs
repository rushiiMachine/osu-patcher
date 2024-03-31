using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

[PublicAPI]
public static class ScoreDisplay
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Play.Components.ScoreDisplay</c>
    ///     b20240124: <c>#=z6dniqZasYGnUF21A3FQQhhWHV7POD$6AVg==</c>
    /// </summary>
    public static readonly LazyType Class = new(
        "osu.GameModes.Play.Components.ScoreDisplay",
        () => Constructor!.Reference.DeclaringType!
    );

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
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Play.Components.ScoreDisplay::ScoreDisplay(SpriteManager, Vector2, bool, float, bool, bool)",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>Hide()</c>
    ///     b20240123: <c>#=zRjDThRI=</c>
    /// </summary>
    public static readonly LazyMethod Hide = LazyMethod.ByPartialSignature( // TODO: support hiding performance counter
        "osu.GameModes.Play.Components.ScoreDisplay::Hide()",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>spriteManager</c>
    ///     b20240123: <c>#=zK4XquDeTazcx</c>
    /// </summary>
    public static readonly LazyField<object> SpriteManager = new(
        "osu.GameModes.Play.Components.ScoreDisplay::spriteManager",
        () => Class.Reference.GetDeclaredFields()
            .Single(field => field.FieldType == Stubs.SpriteManager.Class.Reference)
    );
}