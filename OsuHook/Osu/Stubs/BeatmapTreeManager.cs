using System.Reflection;

namespace OsuHook.Osu.Stubs
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Beatmaps:BeatmapTreeManager</c>
    ///     b20240102.2: <c>#=zV51QPv13Z_vZJRxEY28cvGye77gU6YZQsv_F5muuWuN62V5sIQ==</c>
    /// </summary>
    internal static class BeatmapTreeManager
    {
        /// <summary>
        ///     Original: <c>CurrentGroupMode</c> of type <c><![CDATA[ Bindable<TreeGroupMode> ]]></c>
        ///     b20240102.2: <c>#=zbF4rnAiPRGJl</c>
        ///     This field reference is populated by <see cref="SongSelection.PatchObtainReferences" />
        /// </summary>
        public static FieldInfo CurrentGroupMode = null!;
    }
}