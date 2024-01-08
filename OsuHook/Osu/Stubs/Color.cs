using System;
using HarmonyLib;

namespace OsuHook.Osu.Stubs
{
    using XnaColor = Object;

    /// <summary>
    ///     Original: <c>Microsoft.Xna.Framework.Graphics.Color</c>
    /// </summary>
    public static class Color
    {
        public static XnaColor Red => GetColor("Red");
        public static XnaColor Orange => GetColor("Orange");
        public static XnaColor GhostWhite => GetColor("GhostWhite");

        private static XnaColor GetColor(string name) =>
            // https://github.com/MonoGame/MonoGame/blob/978f722811072e727002dd327aed96e16e384804/MonoGame.Xna/Graphics/Color.cs#L200-L1193
            AccessTools.PropertyGetter($"Microsoft.Xna.Framework.Graphics.Color:{name}")
                .Invoke(null, null);
    }
}