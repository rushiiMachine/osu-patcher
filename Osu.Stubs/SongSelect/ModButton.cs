using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Other;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.SongSelect;

[PublicAPI]
public class ModButton
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Select.ModButton</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameModes.Select.ModButton",
        () => SetStatus!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>SetStatus(bool status, Mods specificStatus, bool playSound)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetStatus = LazyMethod.ByPartialSignature(
        "osu.GameModes.Select.ModButton::SetStatus(bool, Mods, bool)",
        [
            Ldfld,
            Ceq,
            Ldc_I4_0,
            Ceq,
            Ldarg_3,
            And,
            Brfalse_S,
            Ldloca_S,
            Initobj,
        ]
    );

    /// <summary>
    ///     Original: <c>AvailableStates</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyField<IList<int>> AvailableStates = new(
        "osu.GameModes.Select.ModButton::AvailableStates",
        () =>
        {
            // typeof(List<Mods>)
            var modsListType = typeof(List<>).MakeGenericType(Mods.Type.Reference);

            return Class.Reference
                .GetDeclaredFields()
                .Single(field => field.FieldType == modsListType);
        }
    );
}