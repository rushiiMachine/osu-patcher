using System;
using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.HitObjects.Osu.SliderOsu</c>
    ///     b20240102.2: <c>#=zG_Fe30_qUmxpC_TyMiG8b0o7fXW0pNx67n$z1myOL9vw</c>
    /// </summary>
    internal static class SliderOsu
    {
        /// <summary>
        ///     The only constructor on this class
        /// </summary>
        private static readonly LazySignature Constructor = new LazySignature(
            "SliderOsu#SliderOsu",
            new[]
            {
                Ldc_I4_5,
                Ldc_I4_5,
                Ldc_I4_5,
                Newobj,
                Ldnull,
                Newobj,
                Stfld,
                Ldarg_0,
                Ldnull,
            },
            true
        );

        public static Type Class => Constructor.Reference.DeclaringType;
    }
}