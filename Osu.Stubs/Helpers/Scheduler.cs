using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

[PublicAPI]
public class Scheduler
{
    /// <summary>
    ///     Original: <c>osu_common.Helpers.Scheduler</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu_common.Helpers.Scheduler",
        () => Add!.Reference.DeclaringType
    );

    /// <summary>
    ///     Original: <c>Add(VoidDelegate task, bool forceScheduled)</c>
    ///     Returns whether the supplied task was immediately run without scheduling.
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> Add = LazyMethod<bool>.ByPartialSignature(
        "osu_common.Helpers.Scheduler::Add(VoidDelegate, bool)",
        [
            Ldloca_S,
            Call,
            Ldarg_0,
            Ldfld,
            Ldarg_1,
            Callvirt,
            Leave_S,
            Ldloc_1,
            Brfalse_S,
            Ldloc_0,
            Call,
            Endfinally,
            Ldc_I4_0,
            Ret,
        ]
    );
}