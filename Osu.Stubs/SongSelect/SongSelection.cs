using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.SongSelect;

/// <summary>
///     Original: <c>osu.GameModes.Select.SongSelection</c>
///     b20240102.2: <c>#=zKgaD0lVGl2RcuL9z0qvnoUGLjD870$Ll1w==</c>
/// </summary>
[PublicAPI]
public static class SongSelection
{
    /// <summary>
    ///     Original: <c>chooseBestSortMode(TreeGroupMode mode)</c>
    ///     b20240102.2: <c>#=zWDJY2KbbLKhn7OSo1w==</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod ChoseBestSortMode = LazyMethod.ByPartialSignature(
        "osu.GameModes.Select.SongSelection::chooseBestSortMode",
        [
            Ldfld,
            Ldfld,
            Ldloc_0,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Ldloc_0,
            Box,
            Ldc_I4_1,
            Callvirt,
        ]
    );

    /// <summary>
    ///     Original: <c>beatmapTreeManager_OnRightClicked(object sender, BeatmapTreeItem item)</c>
    ///     b20240102.2: <c>#=zAmaE6G1Q0ysoWbGTpb40gD4dZN45</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod BeatmapTreeManagerOnRightClicked = LazyMethod.ByPartialSignature(
        "osu.GameModes.Select.SongSelection::beatmapTreeManager_OnRightClicked",
        [
            Ldarg_2,
            Isinst,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
            Ldfld,
            Callvirt,
            Ldc_I4_S,
            Bne_Un_S,
        ]
    );

    /// <summary>
    ///     Original: <c>beatmapTreeManager</c> of type <see cref="SongSelect.BeatmapTreeManager" />
    ///     b20240102.2: <c>#=zj0IgvXxTqseooUEFmQ==</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object> BeatmapTreeManager = new(
        "osu.GameModes.Select.SongSelection::beatmapTreeManager",
        () => FindReferences().BeatmapTreeManager
    );

    internal static FoundReferences FindReferences()
    {
        var instructions = MethodReader
            .GetInstructions(BeatmapTreeManagerOnRightClicked.Reference)
            .ToArray();

        using var loadFieldInstructions = instructions
            .Where(inst => inst.Opcode == Ldfld)
            .GetEnumerator();

        // Get reference to SongSelection:beatmapTreeManager
        if (!loadFieldInstructions.MoveNext()) throw new Exception();
        var beatmapTreeManagerField = (FieldInfo)loadFieldInstructions.Current.Operand;

        // Get reference to BeatmapTreeManager:CurrentGroupMode 
        if (!loadFieldInstructions.MoveNext()) throw new Exception();
        var currentGroupModeField = (FieldInfo)loadFieldInstructions.Current.Operand;

        return new FoundReferences
        {
            BeatmapTreeManager = beatmapTreeManagerField,
            CurrentGroupMode = currentGroupModeField,
        };
    }
}

internal struct FoundReferences
{
    public FieldInfo BeatmapTreeManager;
    public FieldInfo CurrentGroupMode;
}