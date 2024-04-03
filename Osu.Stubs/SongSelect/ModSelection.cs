using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.SongSelect;

/// <summary>
///     Original: <c>osu.GameModes.Select.ModSelection</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class ModSelection
{
    /// <summary>
    ///     Original: <c>updateMods()</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod UpdateMods = LazyMethod.ByPartialSignature(
        "osu.GameModes.Select.ModSelection::updateMods()",
        [
            Ldc_I4_1,
            Conv_I8,
            Stloc_0,
            Br_S,
            Ldloc_0,
            Conv_I4,
        ]
    );
}