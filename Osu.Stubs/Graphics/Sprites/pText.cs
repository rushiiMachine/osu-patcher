using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Graphics.Sprites;

/// <summary>
///     Sprite that handles text. This is the base class of <c>pSpriteText</c>.
///     Original: <c>osu.Graphics.Sprites.pText</c>
///     b20240123: <c>#=zrPwR$Vhf84Bn$SnxyFkpTe8J9fMu</c>
/// </summary>
[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class pText
{
    /// <summary>
    ///     Original: <c>set_Text(string value)</c> (property setter)
    ///     b20240123: <c>#=zeWQSmtI=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetText = LazyMethod.BySignature(
        "osu.Graphics.Sprites.pText::set_Text(string)",
        [
            Ldarg_0,
            Ldfld,
            Ldarg_1,
            Call,
            Brfalse_S,
            Ret,
            Ldarg_0,
            Ldc_I4_1,
            Stfld,
            Ldarg_0,
            Ldarg_1,
            Stfld,
            Ret,
        ]
    );
}