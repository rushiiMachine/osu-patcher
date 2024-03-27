using System.Diagnostics.CodeAnalysis;
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
}