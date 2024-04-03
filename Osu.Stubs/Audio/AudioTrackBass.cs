using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Audio;

[PublicAPI]
public class AudioTrackBass
{
    /// <summary>
    ///     Original: <c>osu.Audio.AudioTrackBass</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Audio.AudioTrackBass",
        () => Constructor!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>AudioTrackBass(Stream data, bool quick = false, bool loop = false)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.Audio.AudioTrackBass::AudioTrackBass(Stream, bool, bool)",
        [
            Newobj,
            Throw,
            Ldarg_0,
            Ldarg_1,
            Isinst,
            Stfld,
            Ldarg_0,
            Ldfld,
        ]
    );

    /// <summary>
    ///     Original: <c>audioStreamBackwards</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyField<int> AudioStreamBackwardsHandle = new(
        "osu.Audio.AudioTrackBass::audioStreamBackwards",
        () => Class.Reference
            .GetDeclaredFields()
            .Where(field => field.FieldType == typeof(int))
            .Skip(1)
            .First()
    );
}