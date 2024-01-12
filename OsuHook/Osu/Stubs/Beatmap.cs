// ReSharper disable InconsistentNaming


using System;
using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Beatmaps.Beatmap</c>
    ///     b20240102.2: <c>#=zIVNEPkxvRBTQq2q8UIretksQbxQMCb4bKoMXjCtU9TcZ</c>
    /// </summary>
    internal static class Beatmap
    {
        /// <summary>
        ///     Compiler extracted method that retrieves cached difficulty ratings for each star-differing mod combination.
        ///     Original: <c>method_1(PlayModes, Mods)</c>
        ///     b20240102.2:
        ///     <c>\u0005\u2004\u200A\u2004\u2006\u2006\u200B\u200B\u2003\u2000\u200B\u2003\u200A\u2004\u2006\u200A\u2006\u2009\u200A\u2006\u200B\u200B\u2002\u2002\u200B\u2007\u2001\u2008</c>
        /// </summary>
        public static readonly LazySignature method_1 = new LazySignature(
            "Beatmap#method_1",
            new[]
            {
                Ldfld,
                Ldarg_1,
                Ldelem,
                Ldarg_2,
                Callvirt,
                Brtrue_S,
                Ldc_R8,
                Ret,
                Ldarg_0,
                Ldfld,
            }
        );

        public static Type ClassType => method_1.Reference.DeclaringType;
    }
}