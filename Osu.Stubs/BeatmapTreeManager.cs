using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Beatmaps.BeatmapTreeManager</c>
///     b20240102.2: <c>#=zV51QPv13Z_vZJRxEY28cvGye77gU6YZQsv_F5muuWuN62V5sIQ==</c>
/// </summary>
[UsedImplicitly]
public static class BeatmapTreeManager
{
    /// <summary>
    ///     Original: <c>CurrentGroupMode</c> of type <c><![CDATA[ Bindable<TreeGroupMode> ]]></c>
    ///     b20240102.2: <c>#=zbF4rnAiPRGJl</c>
    /// </summary>
    public static readonly LazyField<object> CurrentGroupMode = new(
        "BeatmapTreeManager#CurrentGroupMode",
        () => SongSelection.FindReferences().CurrentGroupMode
    );
}