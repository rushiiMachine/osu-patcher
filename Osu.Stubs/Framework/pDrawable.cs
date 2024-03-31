using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Framework;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class pDrawable
{
    /// <summary>
    ///     Original: <c>osu.Graphics.pDrawable</c>
    ///     b20240123: <c>#=zB63SnFDTnRqYMKLlscCRpu_ww$IG</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Graphics.pDrawable",
        () => ScaleTo!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>Click(bool confirmed)</c>
    ///     b20240123: <c>#=zcJ6mazw=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> Click = LazyMethod<bool>.ByPartialSignature(
        "osu.Graphics.pDrawable::Click(bool)",
        [
            Ret,
            Ldarg_0,
            Ldfld,
            Ldnull,
            Cgt_Un,
            Dup,
            Brfalse_S,
            Ldarg_0,
            Ldfld,
        ]
    );

    /// <summary>
    ///     Original: <c>ScaleTo(float final, int duration, EasingTypes easing)</c>
    ///     b20240123: <c>#=zVyF2njk=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<object> ScaleTo = LazyMethod<object>.ByPartialSignature(
        "osu.Graphics.pDrawable::ScaleTo(float, int, EasingTypes)",
        [
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
        ]
    );

    /// <summary>
    ///     Original: <c>Position</c>
    ///     b20240123: <c>#=ztOn8vDI=</c>
    ///     There is 3 fields with a type of <c>Vector2</c> on this class. The middle one is <c>Position</c>.
    /// </summary>
    [Stub]
    public static readonly LazyField<object> Position = new(
        "osu.Graphics.pDrawable::Position",
        () => Class.Reference.GetDeclaredFields().AsEnumerable()
            .Reverse()
            .Where(field => field.FieldType == Vector2.Class.Reference)
            .Skip(1)
            .First()
    );

    /// <summary>
    ///     Original: <c>Scale</c>
    ///     b20240123: <c>#=zmbpQ79A=</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<float> Scale = new(
        "osu.Graphics.pDrawable::Scale",
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
}