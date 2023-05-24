using System;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using OsuHook.Signature;

namespace OsuHook.Osu
{
    using NotificationColor = Object;

    internal static class Notifications
    {
        // #=zKAl4ByX632sEL0mCYHkNNO8=:#=zKgeB1K0=
        // osu.Graphics.Notifications.NotificationManager:ShowMessage(string s, Color background, int duration, VoidDelegate action)
        private static readonly MethodBase NotificationManagerShowMessage = Signatures.FindMethodBySignature(new[]
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

        /// <summary>
        ///     Show a popup notification inside osu! at the bottom right of the screen.
        /// </summary>
        /// <param name="message">The notification raw text content.</param>
        /// <param name="color">A color object obtained through <see cref="GetColor" />.</param>
        /// <param name="duration">
        ///     Milliseconds to show the notification for after the user sees the notification
        ///     for the first time.
        /// </param>
        /// <param name="action">Notification onclick action.</param>
        public static void ShowMessage(string message,
            object color,
            int duration = 5000,
            Action action = null)
        {
            action = action ?? (() => { });

            // We don't know the type of VoidDelegate at compile time
            var voidDelegateType = NotificationManagerShowMessage.GetParameters()[3].ParameterType;
            var voidDelegate = Delegate.CreateDelegate(voidDelegateType, action, "Invoke");

            NotificationManagerShowMessage.Invoke(null, new[] { message, color, duration, voidDelegate });
        }

        /// <summary>
        ///     Get a color object instance from the MonoGame library for the target color.
        /// </summary>
        /// <param name="colorName">A property name inside <c>Microsoft.Xna.Framework.Graphics.Color</c></param>
        /// <returns>A color object for use in <c>ShowMessage(...)</c></returns>
        public static object GetColor(string colorName)
        {
            return AccessTools.PropertyGetter($"Microsoft.Xna.Framework.Graphics.Color:{colorName}")
                .Invoke(null, null);
        }
    }
}