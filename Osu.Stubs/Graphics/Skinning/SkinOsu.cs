using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Graphics.Skinning;

/// <summary>
///     Most names are present because this class is <c>[Serializable]</c>.
/// </summary>
[PublicAPI]
public static class SkinOsu
{
    /// <summary>
    ///     Original: <c>osu.Graphics.Skinning.SkinOsu</c>
    ///     b20240123: <c>osu.Graphics.Skinning.SkinOsu</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = LazyType.ByName("osu.Graphics.Skinning.SkinOsu");

    /// <summary>
    ///     Original: <c>FontScoreOverlap</c>
    ///     b20240123: <c>FontScoreOverlap</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<int> FontScoreOverlap = new(
        "osu.Graphics.Skinning.SkinOsu::FontScoreOverlap",
        () => Class.Reference.GetDeclaredFields()
            .Find(f => f.Name == "FontScoreOverlap")
    );

    /// <summary>
    ///     Original: <c>FontScore</c>
    ///     b20240123: <c>FontScore</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<string> FontScore = new(
        "osu.Graphics.Skinning.SkinOsu::FontScore",
        () => Class.Reference.GetDeclaredFields()
            .Find(f => f.Name == "FontScore")
    );
}