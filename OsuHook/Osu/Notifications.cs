using System;
using OsuHook.Osu.Stubs;

namespace OsuHook.Osu
{
    internal static class Notifications
    {
        /// <summary>
        ///     Show a popup notification inside osu! at the bottom right of the screen.
        /// </summary>
        /// <param name="message">The notification raw text content.</param>
        /// <param name="color">The color this notification's border should be.</param>
        /// <param name="duration">
        ///     Milliseconds to show the notification for after the user sees the notification for the first time.
        /// </param>
        /// <param name="action">Notification onclick action.</param>
        public static void ShowMessage(
            string message,
            NotificationColor color,
            int duration = 5000,
            Action action = null)
        {
            var xnaColor = color switch
            {
                NotificationColor.Error => Color.Red,
                NotificationColor.Warning => Color.Orange,
                _ => Color.GhostWhite,
            };

            var voidDelegate = VoidDelegate.MakeInstance(action ?? delegate { });

            NotificationManager.ShowMessage.Reference
                .Invoke(null, new[] { message, xnaColor, duration, voidDelegate });
        }
    }

    internal enum NotificationColor
    {
        Neutral,
        Warning,
        Error,
    }
}