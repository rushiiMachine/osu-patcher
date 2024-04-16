using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.OptionText</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class OptionText
{
    /// <summary>
    ///     Original: <c>OptionText(string title, VoidDelegate? [onClick] = null)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Options.OptionText::OptionText(string, VoidDelegate)",
        [
            Ldc_I4_0,
            Stloc_0,
            Ldarg_0,
            Ldarg_1,
            Ldc_R4,
            Ldc_R4,
            Ldloc_0,
            Ldc_I4_2,
            Div,
            Conv_R4,
            Newobj,
            Ldc_R4,
            Ldc_R4,
        ]
    );
}