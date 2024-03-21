using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

// ReSharper disable InconsistentNaming UnusedType.Local UnusedType.Global UnusedMember.Local

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Select.SongSelection</c>
    ///     b20240102.2: <c>#=zKgaD0lVGl2RcuL9z0qvnoUGLjD870$Ll1w==</c>
    /// </summary>
    internal static class SongSelection
    {
        /// <summary>
        ///     Original: <c>chooseBestSortMode(TreeGroupMode mode)</c>
        ///     b20240102.2: <c>#=zWDJY2KbbLKhn7OSo1w==</c>
        /// </summary>
        public static readonly LazySignature choseBestSortMode = new LazySignature(
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
        public static readonly LazySignature beatmapTreeManager_OnRightClicked = new LazySignature(
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
        public static FieldInfo beatmapTreeManager { get; private set; } = null!;

        // abusing harmony to get the instructions i need
        // TODO: write reflection wrapper over the private harmony utils to load CodeInstructions directly 
        [HarmonyPatch]
        private class PatchObtainReferences
        {
            [HarmonyTargetMethod]
            private static MethodBase Target() => beatmapTreeManager_OnRightClicked.Reference;

            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> _instructions)
            {
                var instructions = _instructions.ToArray();

                using var loadFieldInstructions = instructions
                    .Where(inst => inst.opcode == Ldfld)
                    .GetEnumerator();

                // Get reference to SongSelection:beatmapTreeManager
                if (!loadFieldInstructions.MoveNext()) throw new Exception();
                var beatmapTreeManagerField = (FieldInfo)loadFieldInstructions.Current!.operand;

                // Get reference to BeatmapTreeManager:CurrentGroupMode 
                if (!loadFieldInstructions.MoveNext()) throw new Exception();
                var currentGroupModeField = (FieldInfo)loadFieldInstructions.Current!.operand;

                // Get reference to Bindable:Value.get (property getter)
                var bindableValueGetMethod = (MethodBase)instructions
                    .First(inst => inst.opcode == Callvirt)
                    .operand;

                beatmapTreeManager = beatmapTreeManagerField;
                BeatmapTreeManager.CurrentGroupMode = currentGroupModeField;
                Bindable.Value_get = bindableValueGetMethod;

                return instructions;
            }
        }
    }
}