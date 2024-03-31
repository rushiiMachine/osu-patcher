using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

/// <summary>
///     Original: <c>osu_common.Helpers.Logger</c>
///     b20240102.2: <c>#=zN_Wxi7NCKC8KJdpmHc2RwYM=</c>
/// </summary>
[PublicAPI]
public static class Logger
{
    /// <summary>
    ///     Original: <c>Log(string message, LoggingTarget target, LogLevel level)</c>
    ///     b20240102.2: <c>#=zzl_m9cI=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Log = LazyMethod.BySignature(
        "osu_common.Helpers.Logger::Log(string, LoggingTarget, LogLevel)",
        [
            Ldarg_1,
            Ldc_I4_1,
            Call,
            Ldarg_0,
            Ldarg_2,
            Callvirt,
            Leave_S,
            Pop,
            Leave_S,
            Ret,
        ]
    );
}