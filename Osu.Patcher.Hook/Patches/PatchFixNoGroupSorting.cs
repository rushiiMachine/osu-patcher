using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     When the song select is changed to no grouping, then the sorting is not changed back (to before the grouping being
///     selected and changing the sorting) which is extremely annoying.
/// </summary>
[HarmonyPatch]
[UsedImplicitly]
public class PatchFixNoGroupSorting : BasePatch
{
    /// <summary>
    ///     Constant value for the enum value of <c>osu.GameplayElements.Beatmaps.TreeGroupMode:None</c>
    /// </summary>
    private const int TreeGroupModeNone = 12;

    /// <summary>
    ///     Stored last sorting method used with "None" grouping
    /// </summary>
    private static int _lastSortingMethod = -1;

    public static int GetSortMethodForGrouping(
        object @this, // is SongSelection
        int newGroupMode,
        int currentSortMode,
        int newSortMode)
    {
        var beatmapTreeManager = SongSelection.BeatmapTreeManager.Get(@this);
        var currentGroupModeBindable = BeatmapTreeManager.CurrentGroupMode.Get(beatmapTreeManager);
        var currentGroupMode = Bindable.GetValue.Invoke<int>(currentGroupModeBindable);

        // We only care if the grouping method changes
        if (currentGroupMode == newGroupMode)
            return newSortMode;

        // If new grouping is "None", then bring back previous sorting method
        if (newGroupMode == TreeGroupModeNone)
            return _lastSortingMethod > 0 ? _lastSortingMethod : newSortMode;

        // If switching away from None group, store the current sorting method for later 
        if (currentGroupMode == TreeGroupModeNone)
            _lastSortingMethod = currentSortMode;

        return newSortMode;
    }

    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => SongSelection.ChoseBestSortMode.Reference;

    [UsedImplicitly]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var newLocalIdx = generator.DeclareLocal(typeof(int)).LocalIndex;
        instructions = InsertAfterSignature(
            instructions,
            new[]
            {
                Ldarg_0,
                Ldfld,
                Ldfld,
                Callvirt,
                Stloc_0,
            },
            new[]
            {
                // Load the stored current sort mode and convert to int
                CodeInstruction.LoadLocal(0),
                new CodeInstruction(Conv_I4),

                // Store the current sort mode into a new local for later
                CodeInstruction.StoreLocal(newLocalIdx),
            }
        );

        // Add a call to our method before the if statement to update the target sort method
        instructions = InsertBeforeSignature(
            instructions,
            new[]
            {
                // Ldloc_0, // This contains all the labels pointing to it
                Ldarg_0,
                Ldfld,
                Ldfld,
                Callvirt,
                Beq_S,
            },
            new[]
            {
                // Load "this" (0th parameter)
                CodeInstruction.LoadArgument(0),

                // Load "new group mode" enum value (1st parameter) and convert to int 
                CodeInstruction.LoadArgument(1),
                new CodeInstruction(Conv_I4),

                // Load "current sort mode" (ref: above patch) (already int)
                CodeInstruction.LoadLocal(newLocalIdx),

                // Load "new sort mode" enum value and convert to int
                CodeInstruction.LoadLocal(0),
                new CodeInstruction(Conv_I4),

                // Call our method with the above parameters in the stack, returning a single int value
                CodeInstruction.Call(typeof(PatchFixNoGroupSorting), nameof(GetSortMethodForGrouping)),

                // Store result of call into "new sort method" local
                CodeInstruction.StoreLocal(0),

                // Reload the local already on the stack
                // TODO: instead of doing this patch before the load (ref: L92) this would require moving the labels as well
                new CodeInstruction(Pop),
                CodeInstruction.LoadLocal(0),
            }
        );

        return instructions;
    }

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(Exception? __exception)
    {
        if (__exception != null)
        {
            Console.WriteLine($"Exception due to {nameof(PatchFixNoGroupSorting)}: {__exception}");
        }
    }
}