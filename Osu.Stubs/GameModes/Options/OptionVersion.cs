using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.OptionVersion</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class OptionVersion
{
    /// <summary>
    ///     Original: <c>OptionVersion(string title)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Options.OptionVersion::OptionVersion(string)",
        [
            Ldc_I4_S,
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