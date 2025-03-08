using System.Linq;
using JetBrains.Annotations;
using Osu.Utils.Lazy;

namespace Osu.Stubs.GameplayElements.Beatmaps;

[PublicAPI]
public static class TreeGroupMode
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Beatmaps.TreeGroupMode</c>
    ///     b20250309.2: <c>#=zfX6ditFy761_9UdFtla$TvoslqAVtIxU7Qdtog4iwkox</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameplayElements.Beatmaps.TreeGroupMode",
        () => BeatmapTreeManager.CurrentGroupMode.Reference
            .FieldType
            .GetGenericArguments()
            .Single()
    );
}