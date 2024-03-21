using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

// ReSharper disable InconsistentNaming UnusedType.Local UnusedType.Global UnusedMember.Local

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameModes.Select.SongSelection</c>
///     b20240102.2: <c>#=zKgaD0lVGl2RcuL9z0qvnoUGLjD870$Ll1w==</c>
/// </summary>
[UsedImplicitly]
public static class SongSelection
{
    /// <summary>
    ///     Original: <c>chooseBestSortMode(TreeGroupMode mode)</c>
    ///     b20240102.2: <c>#=zWDJY2KbbLKhn7OSo1w==</c>
    /// </summary>
    public static readonly LazyMethod choseBestSortMode = new(
        "SongSelection#chooseBestSortMode",
        new[]
        {
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
        }
    );

    /// <summary>
    ///     Original: <c>beatmapTreeManager_OnRightClicked(object sender, BeatmapTreeItem item)</c>
    ///     b20240102.2: <c>#=zAmaE6G1Q0ysoWbGTpb40gD4dZN45</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod beatmapTreeManager_OnRightClicked = new(
        "SongSelection#beatmapTreeManager_OnRightClicked",
        new[]
        {
            Ldarg_2,
            Isinst,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
            Ldfld,
            Callvirt,
            Ldc_I4_S,
            Bne_Un_S,
        }
    );

    /// <summary>
    ///     Original: <c>beatmapTreeManager</c> of type <see cref="BeatmapTreeManager" />
    ///     b20240102.2: <c>#=zj0IgvXxTqseooUEFmQ==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField beatmapTreeManager = new(
        "SongSelection#beatmapTreeManager",
        () => FindReferences().BeatmapTreeManager
    );

    internal static FoundReferences FindReferences()
    {
        var instructions = MethodReader
            .GetInstructions(beatmapTreeManager_OnRightClicked.Reference)
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

    internal struct FoundReferences
    {
        public FieldInfo BeatmapTreeManager;
        public FieldInfo CurrentGroupMode;
    }
}