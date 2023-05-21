using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Xna.Framework.Graphics;

namespace osu_patcher_hook
{
    internal static class NotificationsUtil
    {
        public delegate void ShowMessageAction();

        // osu.Graphics.Notifications.NotificationManager:ShowMessage(string s, Color background, int duration, VoidDelegate action)
        // #=zKAl4ByX632sEL0mCYHkNNO8=:#=zKgeB1K0=
        private static readonly MethodBase MShowMessage = SigUtils.FindMethodBySignature(new[]
        {
            OpCodes.Ldarg_1,
            OpCodes.Stfld,
            OpCodes.Ldloc_0,
            OpCodes.Ldarg_3,
            OpCodes.Stfld,
            OpCodes.Ldsfld,
            OpCodes.Ldloc_0,
            OpCodes.Ldftn,
            OpCodes.Newobj
        });

        public static void ShowMessage(string message,
            Color color,
            int duration = 5000,
            ShowMessageAction action = null)
        {
            action = action ?? (() => { });

            // We don't know the type of VoidDelegate at compile time
            var voidDelegateType = MShowMessage.GetParameters()[3].ParameterType;
            var voidDelegate = Delegate.CreateDelegate(voidDelegateType, action, "Invoke");

            MShowMessage.Invoke(null, new object[] { message, color, duration, voidDelegate });
        }
    }
}