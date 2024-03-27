using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     <c>pSpriteUnloadable</c> that handles text. This is the base class of <c>pSpriteText</c>.
///     Original: <c>osu.Graphics.Sprites.pText</c>
///     b20240123: <c></c>
/// </summary>
[UsedImplicitly]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class pText
{
    /// <summary>
    ///     Original: <c>set_Text(string value)</c> (property setter)
    ///     b20240123: <c></c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod SetText = new(
        "pText#set_Text(...)",
        new[]
        {
            Ldarg_0,
            Ldfld,
            Ldarg_1,
            Callvirt,
            Brfalse_S,
            Ret,
            Ldarg_0,
            Ldc_I4_1,
            Stfld,
            Ldarg_0,
            Ldarg_1,
            Stfld,
            Ret,
        },
        false,
        true
    );
}