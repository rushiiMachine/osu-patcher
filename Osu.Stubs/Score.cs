using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using Osu.Stubs.Utils;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Scoring.Score</c>
///     b20240123: <c>#=zwswDPw49w3ZrQEjyKFYhq9$8W6WDMiSHDDrNV7k=</c>
/// </summary>
[UsedImplicitly]
public class Score
{
    /// <summary>
    ///     Original: <c>get_Accuracy()</c> (property getter)
    ///     b20240123: <c>#=zY_1A2REMae0xoSV0fA==</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<float> GetAccuracy = new(
        "Score#get_Accuracy()",
        new[]
        {
            Ldc_R4,
            Ret,
            Ldarg_0,
            Ldfld,
            Ldc_I4_S,
            Mul,
            Ldarg_0,
            Ldfld,
            Ldc_I4_S,
            Mul,
            Add,
        }
    );

    /// <summary>
    ///     Original: <c>Score(string input, Beatmap beatmap)</c>
    ///     b20240123: <c>#=zwswDPw49w3ZrQEjyKFYhq9$8W6WDMiSHDDrNV7k=</c>
    /// </summary>
    private static readonly LazyMethod ConstructorFromReplayAndMap = new(
        "Score#<init>(string, Beatmap)",
        () => RuntimeType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(ctor => ctor.GetParameters()
                .GetOrDefault(0, null)?
                .ParameterType == typeof(string)) // Find the only constructor with a "string" as the first parameter
    );

    /// <summary>
    ///     Original: <c>MaxCombo</c>
    ///     b20240123: <c>#=zkQ9fUTuRkHox</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<int> MaxCombo = new(
        "Score#MaxCombo",
        () =>
        {
            // Look for this: "this.MaxCombo = (int)Convert.ToUInt16(array[num++]);"
            var findMethod = AccessTools.Method(typeof(Convert), nameof(Convert.ToUInt16), [typeof(string)])!;

            var storeInstruction = MethodReader
                .GetInstructions(ConstructorFromReplayAndMap.Reference)
                .SkipWhile(inst => !findMethod.Equals(inst.Operand))
                .Skip(1)
                .First();

            Debug.Assert(storeInstruction.Opcode == Stfld);

            return (FieldInfo)storeInstruction.Operand;
        }
    );

    [UsedImplicitly]
    public static Type RuntimeType => GetAccuracy.Reference.DeclaringType!;
}