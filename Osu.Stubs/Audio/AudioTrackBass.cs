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
        () => UpdatePlaybackRate!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>updatePlaybackRate()</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod UpdatePlaybackRate = LazyMethod.ByPartialSignature(
        "osu.Audio.AudioTrackBass::updatePlaybackRate()",
        [
            Conv_R8,
            Ldarg_0,
            Ldfld,
            Mul,
            Ldc_R8,
            Div,
            Conv_R4,
            Call,
            Pop,
        ]
    );

    /// <summary>
    ///     Original: <c>playbackRate</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyField<double> PlaybackRate = new(
        "osu.Audio.AudioTrackBass::playbackRate",
        () => Class.Reference
            .GetDeclaredFields()
            .Single(field => field.FieldType == typeof(double))
    );
}