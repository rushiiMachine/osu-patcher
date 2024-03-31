using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

[PublicAPI]
public static class SkinManager
{
    /// <summary>
    ///     Original: <c>osu.Graphics.Skinning.SkinManager</c>
    ///     b20240123: <c>#=zdwZLyAQXwqtPhTfOQ$e2PRLm39DcCX13EA==</c>
    /// </summary>
    public static readonly LazyType Class = new(
        "osu.Graphics.Skinning.SkinManager",
        () => GetUseNewLayout!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>get_UseNewLayout()</c>
    ///     b20240123: <c>#=zOwgqVurLFLwR</c>
    /// </summary>
    public static readonly LazyMethod<bool> GetUseNewLayout = LazyMethod<bool>.ByPartialSignature(
        "osu.Graphics.Skinning.SkinManager::get_UseNewLayout()",
        [
            Ldsfld,
            Brfalse_S,
            Call,
            Brtrue_S,
            Ldsfld,
            Brfalse_S,
            Ldsfld,
            Ldfld,
        ]
    );

    /// <summary>
    ///     Original: <c>Current</c>
    ///     b20240123: <c>#=zUzFTHbU=</c>
    /// </summary>
    public static readonly LazyField<object> Current = new(
        "SkinManager#Current",
        // There is two fields with type SkinOsu; Current and CurrentUserSkin in that order
        () => Class.Reference.GetDeclaredFields()
            .First(f => f.FieldType == SkinOsu.Class.Reference)
    );
}