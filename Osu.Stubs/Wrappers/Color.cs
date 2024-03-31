using JetBrains.Annotations;
using Osu.Utils;

namespace Osu.Stubs.Wrappers;

using XnaColor = object;

/// <summary>
///     Original: <c>Microsoft.Xna.Framework.Graphics.Color</c>
/// </summary>
[PublicAPI]
public static class Color
{
    public static readonly XnaColor Red = GetColor("Red");
    public static readonly XnaColor Orange = GetColor("Orange");
    public static readonly XnaColor White = GetColor("White");
    public static readonly XnaColor GhostWhite = GetColor("GhostWhite");

    private static XnaColor GetColor(string name)
    {
        // https://github.com/MonoGame/MonoGame/blob/978f722811072e727002dd327aed96e16e384804/MonoGame.Xna/Graphics/Color.cs#L200-L1193
        var colorProperty = OsuAssembly
            .GetType("Microsoft.Xna.Framework.Graphics.Color")
            .GetProperty(name)!;

        return colorProperty.GetMethod.Invoke(null, null);
    }
}