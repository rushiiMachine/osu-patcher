using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

// ReSharper disable UnusedType.Global UnusedMember.Local InconsistentNaming

namespace OsuHook.Patches
{
    /// <summary>
    ///     Disable the effect Relax has on a beatmap's star rating.
    ///     Remove the following code from <c>BeatmapDifficultyCalculatorOsu</c>:
    ///     <code><![CDATA[
    ///          if ((this.#=zu_MPb7Rzosaq.#=zRx1ESxOo8Jm0 & Mods.Relax) > Mods.None)
    ///          {
    ///              num *= 0.9;
    ///              num2 = 0.0;
    ///              num3 *= 0.7;
    ///          }
    ///      ]]></code>
    /// </summary>
    // [HarmonyPatch]
    public class PatchDisableRelaxStarMultiplier : BasePatch
    {
        // private static readonly OpCode[] Signature =
        // {
        //     // Stloc_1,
        //     // // if statement
        //     // Ldarg_0,
        //     // Ldfld,
        //     // Ldfld,
        //     // Call,
        //     // Ldc_I4,
        //     // And,
        //     // Ldc_I4_0,
        //     // Ble_S, // jumps to just past the end of this signature
        //     // if body
        //     Ldloc_0,
        //     Ldc_R8,
        //     Mul,
        //     Stloc_0,
        //     Ldc_R8,
        //     Stloc_2,
        //     Ldloc_1,
        //     Ldc_R8,
        //     Mul,
        //     Stloc_1,
        // };
        //
        // [HarmonyTargetMethod]
        // private static MethodBase Target() => OpCodeMatcher.FindMethodBySignature(Signature);
        //
        // [HarmonyTranspiler]
        // private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
        //     NoopSignature(instructions, Signature);


        // [HarmonyTargetMethod]
        // private static MethodBase Target() => Beatmap.method_1.Reference;

        // [HarmonyPrefix]
        // private static void Before(ref object[] __args)
        // {
        // const int modRelax = 1 << 7;
        // __args[1] = (int)__args[1] & ~modRelax;
        // Console.WriteLine((int)__args[1]);
        // }

        [HarmonyTargetMethod]
        private static MethodBase Target() =>
            // AccessTools.Method(
            // "#=zrhWGiKrl1UmqBImnyQby2bTT1qD0n4Aqjlj0y0m3pOv9JLdOLp5b8fDCCHT38Qmkt86FZwV3L5nHClHCcPTOKMeuFyvmbJz9qg==:#=zwdR$CuA8xPiHtvvxOy7Cs9c=");
            // "#=zrhWGiKrl1UmqBImnyQby2bTT1qD0n4Aqjlj0y0m3pOv9JLdOLp5b8fDCCHT38Qmkt86FZwV3L5nHClHCcPTOKMeuFyvmbJz9qg==:#=zNQvrKrrttCZ8");
            // );
            AccessTools.TypeByName("#=zEPx_kXmLvW4NAaszRD_NEFjlF9PqbC4AOTGb$KGzM3i2SAtjjvCpN8Dw$20OiPKfgQwXbHY=")
                .GetRuntimeMethods()
                .Single(mtd => mtd.Name == "#=zNQvrKrrttCZ8" && mtd.GetParameters().Length == 3);

        [HarmonyAfter]
        private static void After(ref int __result)
        {
            Console.WriteLine($"here {__result}");
            const int modRelax = 1 << 7;
            __result &= ~modRelax;
            Console.WriteLine($"here2 {__result}");
        }
    }
}