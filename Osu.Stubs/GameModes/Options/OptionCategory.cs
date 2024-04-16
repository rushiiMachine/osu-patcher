using JetBrains.Annotations;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.OptionCategory</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class OptionCategory
{
    /// <summary>
    ///     Original: <c>OptionCategory(OsuString titleString, FontAwesome icon = FontAwesome.circle)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Options.OptionCategory::OptionCategory(OsuString, FontAwesome)",
        new[]
        {
            Ldarg_0,
            Ldarga_S,
            Constrained,
            Callvirt,
            Call,
        }.Duplicate(2)
    );
}