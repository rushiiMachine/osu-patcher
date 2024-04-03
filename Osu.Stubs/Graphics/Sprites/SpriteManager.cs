using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Graphics.Sprites;

[PublicAPI]
public static class SpriteManager
{
    /// <summary>
    ///     Original: <c>osu.Graphics.Sprites.SpriteManager</c>
    ///     b20240123: <c>#=zaNwi4uR9iF1HqyG9UwEA2vinmw4mMbeYaQ==</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Graphics.Sprites.SpriteManager",
        () => Add!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>Add(pDrawable p)</c>
    ///     b20240123: <c>#=zJdXS36o=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Add = LazyMethod.ByPartialSignature(
        "osu.Graphics.Sprites.SpriteManager::Add(pDrawable)",
        [
            Not,
            Ldarg_1,
            Callvirt,
            Leave_S,
            Ldloc_2,
            Brfalse_S,
            Ldloc_1,
            Call,
            Endfinally,
        ]
    );
}