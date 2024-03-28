using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

[UsedImplicitly]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class pSpriteText
{
    /// <summary>
    ///     Original:
    ///     <c>
    ///         pSpriteText(string text, string fontname, float spacingOverlap, Fields fieldType, Origins origin,
    ///         Clocks clock, Vector2 startPosition, float drawDepth, bool alwaysDraw, Color colour,
    ///         bool precache = true, SkinSource source = (SkinSource)7)
    ///     </c>
    ///     b20240123: <c></c>
    /// </summary>
    public static readonly LazyMethod<object> Constructor = new(
        "pSpriteText#<init>(...)",
        new[]
        {
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
        },
        true
    );

    /// <summary>
    ///     Updates internal pSpriteText state and returns a <c>Vector2</c> of the size.
    ///     Original: <c>MeasureText()</c>
    ///     b20240123: <c>#=z2Klmy0o=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<object> MeasureText = new(
        "pSpriteText#MeasureText()",
        new[]
        {
            Ldarg_0,
            Ldfld,
            Brfalse_S,
            Ldarg_0,
            Callvirt,
            Pop,
            Ldarg_0,
            Ldfld,
            Ret,
        }
    );

    /// <summary>
    ///     Original: <c>TextConstantSpacing</c>
    ///     b20240123: <c>#=zWUFISsTiUxtU</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<bool> TextConstantSpacing = new(
        "pSpriteText#TextConstantSpacing",
        () => RuntimeType.GetDeclaredFields()
            .Single(field => field.FieldType == typeof(bool))
    );

    [UsedImplicitly]
    public static Type RuntimeType => Constructor.Reference.DeclaringType!;
}