using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Beatmaps.BeatmapTreeItem</c>
///     b20240102.2: <c>#=z5dQ2d9vSoRzE9rWp7h5_dJ$Z1Sw13QcKf_J$7ZjIN21zOue2gQ==</c>
/// </summary>
[UsedImplicitly]
public static class BeatmapTreeItem
{
    /// <summary>
    ///     Original: <c>PopulateSprites(TreeItemState lastState, bool instant)</c>
    ///     b20240102.2: <c>#=ztf5JjnV1ubq2KLwXBg==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod UpdateSprites = new(
        "BeatmapTreeItem#UpdateSprites(...)",
        new[]
        {
            Pop,
            Ldsfld,
            Ldftn,
            Newobj,
            Dup,
            Stsfld,
            Callvirt,
            Ret,
            Ldarg_0,
            Ldfld,
            Ldloc_0,
            Ldftn,
            Newobj,
        }
    );

    /// <summary>
    ///     Original: <c>PopulateSprites()</c>
    ///     b20240102.2: <c>#=zGdedQLY8W$wSqdNzBA==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod PopulateSprites = new(
        "BeatmapTreeItem#PopulateSprites()",
        new[]
        {
            Brfalse,
            Ldsfld,
            Ldfld,
            Ldc_I4,
            Clt,
            Ldc_I4_0,
            Ceq,
            Stloc_S,
            Ldarg_0,
            Ldc_I4_5,
            Newarr,
            Dup,
        }
    );
}