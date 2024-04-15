using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

/// <summary>
///     Original: <c>osu_common.Helpers.LocalisationManager</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class LocalisationManager
{
    /// <summary>
    ///     Original: <c>GetString(OsuString stringType)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<string> GetString = LazyMethod<string>.ByPartialSignature(
        "osu_common.Helpers.LocalisationManager::GetString(OsuString)",
        [
            Ldsfld,
            Ldarg_0,
            Callvirt,
            Stloc_0,
            Leave_S,
            Pop,
            Ldsfld,
            Stloc_0,
            Leave_S,
            Ldloc_0,
            Ret,
        ]
    );
}