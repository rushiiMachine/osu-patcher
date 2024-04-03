using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Other;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Scoring;

[PublicAPI]
public class ModManager
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Scoring.ModManager</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameplayElements.Scoring.ModManager",
        () => AllowRanking!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>AllowRanking(Mods enabledMods)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod AllowRanking = LazyMethod.ByPartialSignature(
        "osu.GameplayElements.Scoring.ModManager::AllowRanking(Mods)",
        [
            Ldloc_2,
            And,
            Ldc_I4_0,
            Cgt,
            Brfalse_S,
            Ldc_I4_0,
            Ret,
            Ldc_I4,
            Ldarg_0,
            Stloc_2,
            Stloc_1,
            Ldloc_1,
            Ldloc_2,
        ]
    );

    /// <summary>
    ///     Original: <c>ActiveMods</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyField<int> ModStatus = new(
        "osu.GameplayElements.Scoring.ModManager::ModStatus",
        () => Class.Reference
            .GetDeclaredFields()
            .Single(field => field.FieldType == Mods.Type.Reference)
    );
}