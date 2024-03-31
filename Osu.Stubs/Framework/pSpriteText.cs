using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Framework;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class pSpriteText
{
    /// <summary>
    ///     Original: <c>osu.Graphics.Sprites.pSpriteText</c>
    ///     b20240123: <c>#=zckAvJzCXrz1Buo3s_qsuXqlnRaWrA6R8Iw==</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Graphics.Sprites.pSpriteText",
        () => Constructor!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original:
    ///     <c>
    ///         pSpriteText(string text, string fontname, float spacingOverlap, Fields fieldType, Origins origin,
    ///         Clocks clock, Vector2 startPosition, float drawDepth, bool alwaysDraw, Color colour,
    ///         bool precache = true, SkinSource source = (SkinSource)7)
    ///     </c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.Graphics.Sprites.pSpriteText::pSpriteText(...)",
        [
            Ldarg_2,
            Ldarg_S,
            Call,
            Stfld,
            Ldarg_S,
            Brfalse,
            Ldc_I4_0,
            Stloc_0,
            Br_S,
            Ldarg_0,
            Ldfld,
            Ldloca_S,
        ]
    );

    /// <summary>
    ///     Updates internal pSpriteText state and returns a <c>Vector2</c> of the size.
    ///     Original: <c>MeasureText()</c>
    ///     b20240123: <c>#=z2Klmy0o=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<object> MeasureText = LazyMethod<object>.ByPartialSignature(
        "osu.Graphics.Sprites.pSpriteText::MeasureText()",
        [
            Ldarg_0,
            Ldfld,
            Brfalse_S,
            Ldarg_0,
            Callvirt,
            Pop,
            Ldarg_0,
            Ldfld,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>TextConstantSpacing</c>
    ///     b20240123: <c>#=zWUFISsTiUxtU</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<bool> TextConstantSpacing = new(
        "osu.Graphics.Sprites.pSpriteText::TextConstantSpacing",
        () => Class.Reference.GetDeclaredFields()
            .Single(field => field.FieldType == typeof(bool))
    );
}