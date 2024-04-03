using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Other;

[PublicAPI]
public class Mods
{
    public const int None = 0;
    public const int NoFail = 1 << 0;
    public const int Easy = 1 << 1;
    public const int Hidden = 1 << 3;
    public const int HardRock = 1 << 4;
    public const int SuddenDeath = 1 << 5;
    public const int DoubleTime = 1 << 6;
    public const int Relax = 1 << 7;
    public const int HalfTime = 1 << 8;
    public const int Nightcore = 1 << 9;
    public const int Flashlight = 1 << 10;
    public const int Autoplay = 1 << 11;
    public const int SpunOut = 1 << 12;
    public const int Relax2 = 1 << 13;
    public const int Perfect = 1 << 14;
    public const int Key4 = 1 << 15;
    public const int Key5 = 1 << 16;
    public const int Key6 = 1 << 17;
    public const int Key7 = 1 << 18;
    public const int Key8 = 1 << 19;
    public const int FadeIn = 1 << 20;
    public const int Random = 1 << 21;
    public const int Cinema = 1 << 22;
    public const int Target = 1 << 23;
    public const int Key9 = 1 << 24;
    public const int KeyCoop = 1 << 25;
    public const int Key1 = 1 << 26;
    public const int Key3 = 1 << 27;
    public const int Key2 = 1 << 28;

    /// <summary>
    ///     Original: <c>osu_common.Mods</c>
    ///     b20240123: <c>osu_common.Mods</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Type = LazyType.ByName("osu_common.Mods");
}