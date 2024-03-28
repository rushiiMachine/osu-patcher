using System;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Graphics.Skinning.SkinManager</c>
///     b20240123: <c>#=zdwZLyAQXwqtPhTfOQ$e2PRLm39DcCX13EA==</c>
/// </summary>
[UsedImplicitly]
public static class SkinManager
{
    /// <summary>
    ///     Original: <c>get_UseNewLayout()</c>
    ///     b20240123: <c>#=zOwgqVurLFLwR</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<bool> GetUseNewLayout = new(
        "SkinManager#get_UseNewLayout()",
        new[]
        {
            Ldsfld,
            Brfalse_S,
            Call,
            Brtrue_S,
            Ldsfld,
            Brfalse_S,
            Ldsfld,
            Ldfld,
        }
    );

    /// <summary>
    ///     Original: <c>Current</c>
    ///     b20240123: <c>#=zUzFTHbU=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<object> Current = new(
        "SkinManager#Current",
        // There is two fields with type SkinOsu; Current and CurrentUserSkin in that order
        () => RuntimeType.GetDeclaredFields()
            .First(f => f.FieldType == SkinOsu.RuntimeType)
    );

    [UsedImplicitly]
    public static Type RuntimeType = GetUseNewLayout.Reference.DeclaringType!;
}