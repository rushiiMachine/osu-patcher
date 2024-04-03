using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.GameplayElements.Scoring;
using Osu.Utils;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameplayElements.Beatmaps;

[PublicAPI]
public static class Beatmap
{
    /// <summary>
    ///     Original: <c>osu.GameplayElements.Beatmaps.Beatmap</c>
    ///     b20240123: <c>#=znWyq67N7qygzmzTavD7zPPRqI0zwVZPmTyOyayVHbxCf</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameplayElements.Beatmaps.Beatmap",
        () => Score.Constructor.Reference
            .GetParameters()[1] // 2nd param is of type Beatmap
            .ParameterType
    );

    /// <summary>
    ///     Original: <c>Beatmap(string filename)</c>
    ///     b20240123: <c>#=znWyq67N7qygzmzTavD7zPPRqI0zwVZPmTyOyayVHbxCf</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "osu.GameplayElements.Beatmaps.Beatmap::Beatmap(string)",
        () => Class.Reference
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Single()
    );

    /// <summary>
    ///     Original: Unknown, best guess: <c>SetContainingFolder(string absoluteDirPath)</c>
    ///     b20240123: <c>#=zQwzJucCIbIUZrSZR8Q==</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetContainingFolder = LazyMethod.ByPartialSignature(
        "osu.GameplayElements.Beatmaps.Beatmap::[unknown, SetContainingFolder?]",
        [
            Callvirt,
            Starg_S,
            Ldarg_0,
            Ldarg_1,
            Ldc_I4_1,
            Newarr,
            Dup,
            Ldc_I4_0,
            Ldsfld,
            Stelem_I2,
            Callvirt,
            Stfld, // Reference to ContainingFolder
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>Filename</c>
    ///     b20240123: <c>#=zdZI_NOQ=</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<string?> Filename = new(
        "osu.GameplayElements.Beatmaps.Beatmap::Filename",
        () =>
        {
            // Find the field this code is referencing: "this.Filename = Path.GetFileName(filename);"
            var findMethod = typeof(Path).GetMethod(nameof(Path.GetFileName))!;
            var storeInstruction = MethodReader.GetInstructions(Constructor.Reference)
                .SkipWhile(inst => !findMethod.Equals(inst.Operand))
                .Skip(1)
                .First();

            Debug.Assert(storeInstruction.Opcode == Stfld);

            return (FieldInfo)storeInstruction.Operand;
        }
    );

    /// <summary>
    ///     Original: Unknown, best guess: <c>ContainingFolder</c> (not absolute)
    ///     b20240123: <c>#=zDmW9P6igScNm</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<string?> ContainingFolder = new(
        "osu.GameplayElements.Beatmaps.Beatmap::ContainingFolder",
        () =>
        {
            // Last Stfld is a reference to ContainingFolder
            var storeInstruction = MethodReader
                .GetInstructions(SetContainingFolder.Reference)
                .Reverse()
                .First(inst => inst.Opcode == Stfld);

            return (FieldInfo)storeInstruction.Operand;
        }
    );

    /// <summary>
    ///     Utility wrapper to get the full beatmap path of a <c>Beatmap</c>.
    /// </summary>
    /// <param name="beatmap">An instance of <c>Beatmap</c> that was initialized with the filepath.</param>
    /// <returns>The absolute path, or null if this isn't a file-backed Beatmap.</returns>
    public static string? GetBeatmapPath(object beatmap)
    {
        var filename = Filename.Get(beatmap);
        var folder = ContainingFolder.Get(beatmap);
        if (filename == null || folder == null) return null;

        var osuDir = Path.GetDirectoryName(OsuAssembly.Assembly.Location)!;

        return Path.Combine(osuDir, "Songs", folder, filename);
    }
}