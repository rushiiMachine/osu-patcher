using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Graphics.Notifications.NotificationManager</c>
///     v20230326: <c>#=zKAl4ByX632sEL0mCYHkNNO8=</c>
/// </summary>
[UsedImplicitly]
public static class NotificationManager
{
    /// <summary>
    ///     Original:
    ///     <code>
    ///         osu.Graphics.Notifications.NotificationManager:ShowMessage(
    ///             string s,
    ///             Microsoft.Xna.Framework.Graphics.Color background,
    ///             int duration,
    ///             VoidDelegate action,
    ///         )
    ///     </code>
    ///     v20230326: <c>#=zKAl4ByX632sEL0mCYHkNNO8=:#=zKgeB1K0=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod ShowMessage = new(
        "NotificationManager#ShowMessage",
        new[]
        {
            Ldarg_1,
            Stfld,
            Ldloc_0,
            Ldarg_3,
            Stfld,
            Ldsfld,
            Ldloc_0,
            Ldftn,
            Newobj,
        }
    );
}