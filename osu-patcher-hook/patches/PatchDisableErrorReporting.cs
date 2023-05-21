using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace osu_patcher_hook.patches
{
    /// <summary>
    ///     No-ops the entire method <c>osu.Helpers.ErrorSubmission:Submit(OsuError)</c>
    ///     to prevent peppy getting spammed with errors caused by this patcher.
    /// </summary>
    [HarmonyPatch]
    internal class PatchDisableErrorReporting
    {
        // #=zhC91LB1xsJMwYkF0UQ==:#=zPqLxZPA=
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldsfld,
            OpCodes.Ldc_I4_0,
            OpCodes.Ble_S,
            OpCodes.Ldsfld,
            OpCodes.Ldsfld,
            OpCodes.Sub,
            OpCodes.Ldc_I4,
            OpCodes.Bge_S,
            OpCodes.Ret,
            OpCodes.Ldsfld,
            OpCodes.Stsfld,
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Ldarg_0
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return SigUtils.FindMethodBySignature(Signature);
        }

        [HarmonyPrefix]
        private static bool Before()
        {
            return false;
        }
    }
}