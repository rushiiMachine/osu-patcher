using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

// ReSharper disable InconsistentNaming UnusedType.Global UnusedType.Local UnusedMember.Local

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Events.EventManager</c>
    ///     b20240102.2: <c>#=zbJqkrJF69yLASpsnblYMeq3jWETw</c>
    /// </summary>
    internal static class EventManager
    {
        /// <summary>
        ///     Original: <c>set_ShowStoryboard</c> (property setter)
        ///     b20240102.2: <c>#=zKZugEelWoTXb</c>
        /// </summary>
        /// <returns></returns>
        public static readonly LazySignature set_ShowStoryboard = new LazySignature(
            "EventManager#ShowStoryboard.set",
            new[]
            {
                Ldarg_0,
                Ldsfld,
                Bne_Un_S,
                Ret,
                Ldarg_0,
                Stsfld, // This references the backing field of the ShowStoryboard property
                Ldsfld,
                Brfalse_S,
                Ldsfld,
                Callvirt,
                Ret,
            }
        );

        /// <summary>
        ///     The compiler generated backing field for the <c>ShowStoryboard</c> property.
        ///     See: <see cref="set_ShowStoryboard" />
        /// </summary>
        public static FieldInfo ShowStoryboard_backing { get; private set; }

        [HarmonyPatch]
        private class PatchObtainShowStoryboard_backing
        {
            [HarmonyTargetMethod]
            private static MethodBase Target() => set_ShowStoryboard.Reference;

            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> _instructions)
            {
                var instructions = _instructions.ToArray();

                // abusing harmony to get the instruction i need
                var storeInstruction = instructions.Single(inst => inst.opcode == Stsfld);
                ShowStoryboard_backing = (FieldInfo)storeInstruction.operand;

                return instructions;
            }
        }
    }
}