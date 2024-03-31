using JetBrains.Annotations;

namespace Osu.Stubs.Wrappers;

/// <summary>
///     Original: <c>osu.Graphics.Skinning.SkinSource</c>
/// </summary>
[PublicAPI]
public static class SkinSource
{
    public const int None = 0;
    public const int Osu = 1;
    public const int Skin = 2;
    public const int Beatmap = 4;
    public const int Temporal = 8;
    public const int ExceptBeatmap = Osu | Skin;
    public const int All = Osu | Skin | Beatmap;
}