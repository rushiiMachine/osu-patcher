using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Helpers;
using Osu.Utils.Extensions;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameplayElements.Scoring;

[PublicAPI]
public static class Score
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Scoring.Score</c>
    ///     b20240123: <c>#=zwswDPw49w3ZrQEjyKFYhq9$8W6WDMiSHDDrNV7k=</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameplayElements.Scoring.Score",
        () => GetAccuracy!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>Score(string input, Beatmap beatmap)</c>
    ///     b20240123: <c>#=zwswDPw49w3ZrQEjyKFYhq9$8W6WDMiSHDDrNV7k=</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "osu.GameplayElements.Scoring.Score::Score(string, Beatmap)",
        () => Class.Reference
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single(ctor => ctor.GetParameters()
                .GetOrDefault(0, null)?
                .ParameterType == typeof(string)) // Find the only constructor with a "string" as the first parameter
    );

    /// <summary>
    ///     Original: <c>get_Accuracy()</c> (property getter)
    ///     b20240123: <c>#=zY_1A2REMae0xoSV0fA==</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<float> GetAccuracy = LazyMethod<float>.ByPartialSignature(
        "osu.GameplayElements.Scoring.Score::get_Accuracy()",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>Beatmap</c>
    ///     b20240123: <c>#=zhcWn5UkrdlUu</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object?> Beatmap = new(
        "osu.GameplayElements.Scoring.Score::Beatmap",
        () => Class.Reference
            .GetRuntimeFields()
            .Single(inst => inst.FieldType == Beatmaps.Beatmap.Class.Reference)
    );

    /// <summary>
    ///     Original: <c>MaxCombo</c>
    ///     b20240123: <c>#=zkQ9fUTuRkHox</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<int> MaxCombo = new(
        "osu.GameplayElements.Scoring.Score::MaxCombo",
        () =>
        {
            // Look for this: "this.MaxCombo = (int)Convert.ToUInt16(array[num++]);"
            var findMethod = AccessTools.Method(typeof(Convert), nameof(Convert.ToUInt16), [typeof(string)])!;

            var storeInstruction = MethodReader
                .GetInstructions(Constructor.Reference)
                .SkipWhile(inst => !findMethod.Equals(inst.Operand))
                .Skip(1)
                .First();

            Debug.Assert(storeInstruction.Opcode == Stfld);

            return (FieldInfo)storeInstruction.Operand;
        }
    );

    /// <summary>
    ///     Is of type <c>Obfuscated{Mods}</c>
    ///     Original: <c>EnabledMods</c>
    ///     b20240123: <c>#=zxL1NzqBwrqNU</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object> EnabledMods = new(
        "osu.GameplayElements.Scoring.Score::EnabledMods",
        () => Class.Reference
            .GetDeclaredFields()
            .Single(field => field.FieldType.IsGenericType &&
                             field.FieldType.GetGenericTypeDefinition() == Obfuscated.Class.Reference)
    );

    /// <summary>
    ///     The generic method <c>Obfuscated{T}::get_Value()</c> with the type parameter T bound to <c>Mods</c>.
    /// </summary>
    [Stub]
    public static readonly LazyMethod<int> EnabledModsGetValue = new(
        "osu.Helpers.Obfuscated<Mods>::get_Value()",
        () =>
        {
            var modsType = EnabledMods.Reference.FieldType
                .GetGenericArguments().First();

            return Obfuscated.BindGetValue(modsType);
        }
    );
}