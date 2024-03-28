using System;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Graphics.Skinning.OsuSkin</c>
///     b20240123: <c>osu.Graphics.Skinning.OsuSkin</c>
///     Most names are present because of this class is [Serializable].
/// </summary>
[UsedImplicitly]
public static class SkinOsu
{
    /// <summary>
    ///     Original: <c>FontScoreOverlap</c>
    ///     b20240123: <c>FontScoreOverlap</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<int> FontScoreOverlap = new(
        "OsuSkin#FontScoreOverlap",
        () => RuntimeType.GetDeclaredFields()
            .Find(f => f.Name == "FontScoreOverlap")
    );

    /// <summary>
    ///     Original: <c>FontScore</c>
    ///     b20240123: <c>FontScore</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<string> FontScore = new(
        "OsuSkin#FontScore",
        () => RuntimeType.GetDeclaredFields()
            .Find(f => f.Name == "FontScore")
    );

    [UsedImplicitly]
    public static Type RuntimeType => OsuAssembly.GetType("osu.Graphics.Skinning.SkinOsu");
}