using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

/// <summary>
///     Original: <c>osu.Helpers.DataStoreUpdater</c>
/// </summary>
[PublicAPI]
public class DataStoreUpdater
{
    /// <summary>
    ///     Original: <c>Save(bool final)</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> Save = LazyMethod<bool>.ByPartialSignature(
        "osu.Helpers.DataStoreUpdater::Save(bool)",
        [
            Ldc_I4_0,
            Stloc_0,
            Leave_S,
            Ldarg_0,
            Ldsfld,
            Stfld,
            Ldarg_0,
            Ldc_I4_0,
            Stfld,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Pop,
            Endfinally,
            Ldloc_0,
            Ret,
        ]
    );
}