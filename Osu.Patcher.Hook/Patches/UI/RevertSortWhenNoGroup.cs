using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Select;
using Osu.Stubs.GameplayElements.Beatmaps;
using Osu.Stubs.Helpers;
using Osu.Utils.Extensions;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Patcher.Hook.Patches.UI;

/// <summary>
///     When the song select is changed to no grouping, then the sorting is not changed back (to before the
///     grouping being selected and changing the sorting) which is extremely annoying.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class RevertSortWhenNoGroup
{
    /// <summary>
    ///     Constant value for the enum value of <c>osu.GameplayElements.Beatmaps.TreeGroupMode:None</c>
    /// </summary>
    private const int TreeGroupModeNone = 12;

    /// <summary>
    ///     Stored last sorting method used with "None" grouping
    /// </summary>
    private static int _lastSortingMethod = -1;

    private static readonly Bindable TreeGroupModeStub = new(TreeGroupMode.Class.Reference);

    public static int GetSortMethodForGrouping(
        object @this, // is SongSelection
        int newGroupMode,
        int currentSortMode,
        int newSortMode)
    {
        var beatmapTreeManager = SongSelection.BeatmapTreeManager.Get(@this);
        var currentGroupModeBindable = BeatmapTreeManager.CurrentGroupMode.Get(beatmapTreeManager);
        var currentGroupMode = TreeGroupModeStub.GetValue.Invoke<int>(currentGroupModeBindable);

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
        instructions = instructions.InsertAfterSignature(
            [
                Ldarg_0,
                Ldfld,
                Ldfld,
                Callvirt,
                Stloc_0,
            ],
            new[]
            {
                // Load the stored current sort mode and convert to int
                new(Ldloc_0),
                new(Conv_I4),

                // Store the current sort mode into a new local for later
                CodeInstruction.StoreLocal(newLocalIdx),
            }
        );

        // Add a call to our method before the if statement to update the target sort method
        instructions = instructions.InsertBeforeSignature(
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
                CodeInstruction.Call(typeof(RevertSortWhenNoGroup), nameof(GetSortMethodForGrouping)),

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
}