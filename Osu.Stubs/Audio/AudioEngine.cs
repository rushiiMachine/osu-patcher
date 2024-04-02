using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Audio;

/// <summary>
///     Original: <c>osu.Audio.AudioEngine</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class AudioEngine
{
    /// <summary>
    ///     Original: <c>get_CurrentPlaybackRate()</c> (property getter)
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<double> GetCurrentPlaybackRate = LazyMethod<double>.BySignature(
        "osu.Audio.AudioEngine::get_CurrentPlaybackRate()",
        [
            Ldsfld,
            Dup,
            Brtrue_S,
            Pop,
            Ldc_R8,
            Ret,
            Callvirt,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>set_CurrentPlaybackRate()</c> (property setter)
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetCurrentPlaybackRate = LazyMethod.ByPartialSignature(
        "osu.Audio.AudioEngine::set_CurrentPlaybackRate()",
        [
            Ldsfld, // Reference to AudioEngine::Nightcore
            Ldc_I4_0,
            Ceq,
            Callvirt,
            Ldloc_0,
            Ldarg_0,
            Callvirt,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>Nightcore</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyField<bool> Nightcore = new(
        "osu.Audio.AudioEngine::Nightcore",
        () =>
        {
            // Last Ldsfld in get_CurrentPlaybackRate() is a reference to AudioEngine::Nightcore
            var instruction = MethodReader
                .GetInstructions(SetCurrentPlaybackRate.Reference)
                .Reverse()
                .First(inst => inst.Opcode == Ldsfld);

            return (FieldInfo)instruction.Operand;
        }
    );
}