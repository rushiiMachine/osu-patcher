using OsuHook.OpcodeUtil;
using static System.Reflection.Emit.OpCodes;

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original:
    ///     <c>osu.GameplayElements.Beatmaps.BeatmapDifficultyCalculatorOsu</c>
    ///     b20240102.2:
    ///     <c>#=zrhWGiKrl1UmqBImnyQby2bTT1qD0n4Aqjlj0y0m3pOv9JLdOLp5b8fDCCHT38Qmkt86FZwV3L5nHClHCcPTOKMeuFyvmbJz9qg==</c>
    /// </summary>
    internal static class BeatmapDifficultyCalculatorOsu
    {
        /// <summary>
        ///     Original: <c>CreateDifficultyAttributes</c> is highly likely. ref:
        ///     https://github.com/ppy/osu/blob/fbc40ffc65930f1e36bb048c73623dbbd0e23062/osu.Game.Rulesets.Osu/Difficulty/OsuDifficultyCalculator.cs#L53-L58
        ///     b20240102.2: <c>#=z5tW2q8sXvXTj$c1S1Eho4Gc=</c>
        /// </summary>
        public static readonly LazySignature Method1 = new LazySignature(
            "BeatmapDifficultyCalculatorOsu#CreateDifficultyAttributes",
            new[]
            {
                Ldarg_0,
                Ldfld,
                Ldfld,
                Call,
                Ldc_I4,
                And,
                Ldc_I4_0,
                Ble_S,
            }
        );
    }
}