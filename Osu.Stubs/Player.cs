using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

[PublicAPI]
public static class Player
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Play.Player</c>
    ///     b20240124: <c>#=zOTWUr4vq60U15SRmD_JItyatbhdR</c>
    /// </summary>
    public static readonly LazyType Class = new(
        "osu.GameModes.Play.Player",
        () => GetAllowDoubleSkip!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>get_AllowDoubleSkip()</c> (property getter)
    ///     b20240124: <c>#=zp29IlAJ43g4WRArPQA==</c>
    /// </summary>
    public static readonly LazyMethod GetAllowDoubleSkip = LazyMethod.ByPartialSignature(
        "osu.GameModes.Play.Player::get_AllowDoubleSkip()",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>OnLoadComplete(bool success)</c>
    ///     b20240124: <c>#=zXb_K4cZvV$uy</c>
    /// </summary>
    public static readonly LazyMethod<bool> OnLoadComplete = LazyMethod<bool>.ByPartialSignature(
        "osu.GameModes.Play.Player::OnLoadComplete(bool)",
        [
            Br,
            Ldloc_S,
            Callvirt,
            Unbox_Any,
            Stloc_2,
            Ldsfld,
            Ldfld,
            Call,
        ]
    );

    /// <summary>
    ///     Original: <c>currentScore</c>
    ///     b20240124: <c>#=zF6h5l4j0$TfX</c>
    /// </summary>
    public static readonly LazyField<object?> CurrentScore = new(
        "osu.GameModes.Play.Player::currentScore",
        () => Class.Reference
            .GetFields(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(field => field.FieldType == Score.Class.Reference)
    );
}