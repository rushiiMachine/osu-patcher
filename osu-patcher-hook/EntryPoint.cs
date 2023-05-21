using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace osu_patcher_hook
{
    public class EntryPoint
    {
        private static Harmony _harmony;

        public static int Initialize(string _)
        {
            var stream = new FileStream("C:\\osu!\\Logs\\patcher.txt", FileMode.Create);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            Console.SetOut(writer);
            Console.SetError(writer);

            Console.WriteLine("osu! init");

            try
            {
                _harmony = new Harmony("io.github.rushiimachine.osu-patcher");
                _harmony.PatchAll(typeof(EntryPoint).Assembly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return 0;
        }
    }

    /// <summary>
    ///     A base patch that provides utility methods for signature-based patching etc.
    /// </summary>
    internal class Patch
    {
        private static readonly Module OsuModule = AppDomain.CurrentDomain.GetAssemblies()
                                                       .SingleOrDefault(assembly => assembly.GetName().Name == "osu!")
                                                       ?.GetModules().SingleOrDefault()
                                                   ?? throw new Exception("Unable to find osu! assembly!");

        protected static MethodInfo FindMethodBySignature(OpCode[] signature)
        {
            if (signature.Length <= 0) return null;

            foreach (var type in OsuModule.GetTypes())
            foreach (var method in type.GetRuntimeMethods())
            {
                var instructions = method.GetMethodBody()?.GetILAsByteArray();
                if (instructions == null) continue;

                var sequentialMatching = 0;
                foreach (var instruction in new OpCodeReader(instructions).GetOpCodes())
                {
                    if (sequentialMatching == signature.Length)
                        return method;

                    if (instruction == signature[sequentialMatching])
                        sequentialMatching++;
                    else
                        sequentialMatching = 0;
                }
            }

            return null;
        }

        /// <summary>
        ///     Finds and no-ops everything after a certain IL bytecode signature.
        /// </summary>
        /// <param name="instructions">The input instructions to patch, mainly coming from a HarmonyTranspiler.</param>
        /// <param name="signature">The signature to find within the instructions based on IL opcodes.</param>
        /// <param name="replaceAfterSignature">The amount of instructions to replace after the signature.</param>
        /// <param name="replaceAll">No-op multiple times for every signature found.</param>
        /// <returns></returns>
        protected static IEnumerable<CodeInstruction> NoopAfterBySignature(
            IEnumerable<CodeInstruction> instructions,
            OpCode[] signature,
            uint replaceAfterSignature,
            bool replaceAll = false)
        {
            var replacementRemaining = replaceAfterSignature;
            var sequentialMatching = 0;
            var found = false;

            foreach (var instruction in instructions)
                if (signature.Length > 0 && replacementRemaining > 0 && sequentialMatching == signature.Length)
                {
                    found = true;
                    replacementRemaining--;
                    yield return new CodeInstruction(OpCodes.Nop);
                }
                else
                {
                    if (signature.Length > 0)
                    {
                        if (replaceAfterSignature <= 0 && replaceAll)
                        {
                            replacementRemaining = replaceAfterSignature;
                            sequentialMatching = 0;
                        }

                        if (replacementRemaining == replaceAfterSignature &&
                            instruction.opcode == signature[sequentialMatching])
                            sequentialMatching++;
                        else
                            sequentialMatching = 0;
                    }

                    yield return instruction;
                }

            if (signature.Length > 0 && !found)
                Console.WriteLine("Could not find the target signature in method!");
            else if (found && replacementRemaining > 0 && replaceAfterSignature != replacementRemaining)
                Console.WriteLine("Not enough space in method to noop more instructions!");
        }
    }

    /// <summary>
    ///     No-ops the entire method <c>osu.Helpers.ErrorSubmission:Submit(OsuError)</c>
    ///     to prevent peppy getting spammed with errors caused by this patcher.
    /// </summary>
    [HarmonyPatch]
    internal class PatchDisableErrorReporting : Patch
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
            return FindMethodBySignature(Signature);
        }

        [HarmonyPrefix]
        private static bool Before()
        {
            return false;
        }
    }

    [HarmonyPatch]
    internal class PatchEnableRelaxFailing : Patch
    {
        private static MethodBase TargetMethod()
        {
            const string
                clz = "#=zeXZ7VnmadWamDozl0oXkDPqWT5QR", // osu.GameModes.Play.Player
                mtd = "#=zwMd5KYaUmGit"; // CheckFailed()

            return AccessTools.Method($"{clz}:{mtd}");
        }

        // Change "if ((mods & Mods.NoFail) <= Mods.None && !Player.Relaxing && !Player.Relaxing2 && ..."
        // To "if ((mods & Mods.NoFail) <= Mods.None && ..."
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return NoopAfterBySignature(
                instructions,
                new[]
                {
                    OpCodes.And,
                    OpCodes.Ldc_I4_0,
                    OpCodes.Cgt,
                    OpCodes.Brtrue_S
                },
                4
            );
        }
    }

    /// <summary>
    ///     Changes the following code in
    ///     <c>osu.GameModes.Play.Rulesets.Ruleset:IncreaseScoreHit(IncreaseScoreType, HitObject)</c>
    ///     to enable the combo break sound during Relax* scores.
    ///     <br /><br />
    ///     From:
    ///     <code><![CDATA[
    ///     if (this.ComboCounter.HitCombo > 20 && !Player.Relaxing && !Player.Relaxing2)
    /// ]]></code>
    ///     To:
    ///     <code><![CDATA[
    ///     if (this.ComboCounter.HitCombo > 20)
    /// ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchEnableRelaxComboBreakSound : Patch
    {
        // #=z04fOmc1I_BS0TV6TAo2QOUQvjceryuOcqoleWPg=:#=zSio4IZHzUUrC
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Callvirt,
            OpCodes.Ldc_I4_S,
            OpCodes.Ble_S,
            OpCodes.Ldsfld, // --------
            OpCodes.Brtrue_S, // All no-oped (4 inst)
            OpCodes.Ldsfld,
            OpCodes.Brtrue_S // --------
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return FindMethodBySignature(Signature);
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return NoopAfterBySignature(
                instructions,
                Signature.Take(Signature.Length - 4).ToArray(),
                4
            );
        }
    }

    /// <summary>
    ///     Changes the following code in <c>osu.GameplayElements.HitObjectManager:vmethod_17(HitObject)</c>
    ///     to enable showing misses in Relax* scores.
    ///     <br /><br />
    ///     From:
    ///     <code><![CDATA[
    ///     if (increaseScoreType == (IncreaseScoreType)(-131072) && !Player.Relaxing && !Player.Relaxing2)
    /// ]]></code>
    ///     To:
    ///     <code><![CDATA[
    ///     if (increaseScoreType == (IncreaseScoreType)(-131072))
    /// ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchEnableRelaxMisses : Patch
    {
        // #=zTBjFb7Vm$jY$rY4MsKxmcIvGHnQN:\u0005\u200A\u2002\u2002\u2001\u2004\u2003\u2007\u2001\u2002\u2002\u2000
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_1,
            OpCodes.Ldc_I4_8,
            OpCodes.Callvirt,
            OpCodes.Stloc_S,
            OpCodes.Ldloc_0,
            OpCodes.Ldc_I4,
            OpCodes.Bne_Un
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return FindMethodBySignature(Signature);
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return NoopAfterBySignature(
                instructions,
                Signature,
                4
            );
        }
    }

    /// <summary>
    ///     Removes the following code from <c>osu.GameModes.Ranking:loadLocalUserScore(bool showRankingDialog)</c>
    ///     to enable automatically saving Relax* scores after
    ///     <br /><br />
    ///     <code><![CDATA[
    ///     Mods mods = Player.currentScore.EnabledMods;  
    ///     if ((mods & Mods.Relax) <= Mods.None)
    ///     {
    ///         mods = Player.currentScore.EnabledMods;
    ///         if ((mods & Mods.Relax2) <= Mods.None)
    ///         {
    /// ]]></code>
    /// </summary>
    [HarmonyPatch]
    internal class PatchAutoSaveRelaxScores : Patch
    {
        // #=zG9n2xn5fBJ3KmhYrFhPv_ouHnledvs2AJ1Dwx_c=:#=zPWtjIx_tsaf1
        private static readonly OpCode[] Signature =
        {
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Ldfld,
            OpCodes.Brfalse,
            OpCodes.Ldarg_0,
            OpCodes.Ldfld,
            OpCodes.Brtrue,
            OpCodes.Ldsfld, // <--------------
            OpCodes.Ldfld,
            OpCodes.Call,
            OpCodes.Ldc_I4,
            OpCodes.Stloc_2,
            OpCodes.Stloc_1,
            OpCodes.Ldloc_1,
            OpCodes.Ldloc_2,
            OpCodes.And,
            OpCodes.Ldc_I4_0,
            OpCodes.Cgt,
            OpCodes.Brtrue, // All no-oped (24 inst)
            OpCodes.Ldsfld,
            OpCodes.Ldfld,
            OpCodes.Call,
            OpCodes.Ldc_I4,
            OpCodes.Stloc_2,
            OpCodes.Stloc_1,
            OpCodes.Ldloc_1,
            OpCodes.Ldloc_2,
            OpCodes.And,
            OpCodes.Ldc_I4_0,
            OpCodes.Cgt,
            OpCodes.Brtrue // <-------------
        };

        [HarmonyTargetMethod]
        private static MethodBase Target()
        {
            return FindMethodBySignature(Signature);
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return NoopAfterBySignature(
                instructions,
                Signature.Take(Signature.Length - 24).ToArray(),
                24
            );
        }
    }

    // [HarmonyPatch]
    // internal class PatchRemoveModCombinationRestrictions : Patch
    // {
    //     private const string clz = "#=zA68w2LnfHk3bAvNoTjj7pqCRs0P7Q2WkMrK0LXo=";
    //
    //     private static Mods selectedModsRef = AccessTools.StaticFieldRefAccess<Mods>($"{clz}:#=zxGXFDdk=");
    //
    //     private static MethodBase TargetMethod()
    //     {
    //         // Search for
    //         // ~(Mods.Key4 | Mods.Key5 | Mods.Key6 | Mods.Key7 | Mods.Key8 | Mods.FadeIn | Mods.Random | Mods.Key9 | Mods.KeyCoop | Mods.Key1 | Mods.Key3 | Mods.Key2 | Mods.Mirror)
    //
    //         const string mtd = "#=zxGgySG2NcJR6K_KRKQ==";
    //         return AccessTools.Method($"{clz}:{mtd}", new[] { typeof(Mods) });
    //     }
    //
    //     private static bool Prefix([HarmonyArgument(0)] Mods mods)
    //     {
    //         selectedModsRef = mods;
    //         return false;
    //     }
    // }

    // [HarmonyPatch]
    // internal class PatchFixRelaxScoreMultiplier : Patch
    // {
    //     private static MethodBase TargetMethod()
    //     {
    //         // Search for:
    //         // case PlayModes.Osu:
    //         // case PlayModes.Taiko:
    //
    //         const string
    //             clz = "#=zA68w2LnfHk3bAvNoTjj7pqCRs0P7Q2WkMrK0LXo=",
    //             mtd = "#=z_gY4$2rMOiN4";
    //
    //         return AccessTools.Method($"{clz}:{mtd}",
    //             new[]
    //             {
    //                 typeof(Mods),
    //                 typeof(PlayModes),
    //                 AccessTools.TypeByName("#=zkdhZ0xuyvtdonL9gD6UYabtvEflJOyazS1zegavU_9KJ")
    //             });
    //     }
    //
    //     private static void Prefix([HarmonyArgument(0)] ref Mods mods)
    //     {
    //         mods &= ~(Mods.Relax | Mods.Relax2);
    //     }
    // }
}