using System;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Graphics.Sprites.SpriteManager</c>
///     b20240123: <c>#=zaNwi4uR9iF1HqyG9UwEA2vinmw4mMbeYaQ==</c>
/// </summary>
[UsedImplicitly]
public class SpriteManager
{
    /// <summary>
    ///     Original: <c>Add(pDrawable p)</c>
    ///     b20240123: <c>#=zJdXS36o=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod Add = new(
        "SpriteManager#Add(...)",
        new[]
        {
            Not,
            Ldarg_1,
            Callvirt,
            Leave_S,
            Ldloc_2,
            Brfalse_S,
            Ldloc_1,
            Call,
            Endfinally,
        }
    );

    [UsedImplicitly]
    public static Type RuntimeTime => Add.Reference.DeclaringType!;
}