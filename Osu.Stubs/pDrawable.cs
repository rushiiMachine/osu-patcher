using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Graphics.pDrawable</c>
///     b20240123: <c>#=zB63SnFDTnRqYMKLlscCRpu_ww$IG</c>
/// </summary>
[UsedImplicitly]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class pDrawable
{
    /// <summary>
    ///     Original: <c>Click(bool confirmed)</c>
    ///     b20240123: <c>#=zcJ6mazw=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<bool> Click = new(
        "pDrawable#Click(...)",
        new[]
        {
            Ret,
            Ldarg_0,
            Ldfld,
            Ldnull,
            Cgt_Un,
            Dup,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
        }
    );

    /// <summary>
    ///     Original: <c>ScaleTo(float final, int duration, EasingTypes easing)</c>
    ///     b20240123: <c>#=zVyF2njk=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<object> ScaleTo = new(
        "pDrawable#ScaleTo(...)",
        new[]
        {
            Ldc_I4_4, // TransformationType.Scale
            Ldarg_0,
            Ldfld, // this.Scale
            Conv_R4,
            Ldarg_1,
            Ldarg_0,
            Ldfld,
            Call,
            Ldsfld,
            Conv_I4,
            Sub,
        }
    );

    /// <summary>
    ///     Original: <c>Position</c>
    ///     b20240123: <c>#=ztOn8vDI=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object> Position = new(
        "pDrawable#Position",
        // There is 3 fields with a type of <c>Vector2</c> on this class. The middle one is <c>Position</c>.
        () => RuntimeType.GetDeclaredFields().AsEnumerable()
            .Reverse()
            .Where(field => field.FieldType == Vector2.RuntimeType)
            .Skip(1)
            .First()
    );

    /// <summary>
    ///     Original: <c>Scale</c>
    ///     b20240123: <c>#=zmbpQ79A=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<float> Scale = new(
        "pDrawable#Scale",
        () =>
        {
            // The last "ldsfld float" in the ScaleTo method is a reference to this.Scale 
            var instruction = MethodReader.GetInstructions(ScaleTo.Reference)
                .Reverse()
                .Where(inst => inst.Opcode == Ldfld)
                .First(inst => ((FieldInfo)inst.Operand).FieldType == typeof(float));

            return (FieldInfo)instruction.Operand;
        }
    );

    [UsedImplicitly]
    public static Type RuntimeType => ScaleTo.Reference.DeclaringType!;
}