using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Beatmaps.Beatmap</c>
///     b20240123: <c>#=znWyq67N7qygzmzTavD7zPPRqI0zwVZPmTyOyayVHbxCf</c>
/// </summary>
[UsedImplicitly]
public static class Beatmap
{
    /// <summary>
    ///     Original: Unknown, best guess: <c>SetContainingFolder(string absoluteDirPath)</c>
    ///     b20240123: <c>#=zQwzJucCIbIUZrSZR8Q==</c>
    /// </summary>
    private static readonly LazyMethod SetContainingFolder = new(
        "Beatmap#SetContainingFolder(...)",
        new[]
        {
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
        }
    );

    /// <summary>
    ///     Original: <c>Filename</c>
    ///     b20240123: <c>#=zdZI_NOQ=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<string?> Filename = new(
        "Beatmap#Filename",
        () =>
        {
            // Find the field this code is referencing: "this.Filename = Path.GetFileName(filename);"
            var findMethod = AccessTools.Method(typeof(Path), nameof(Path.GetFileName));
            var storeInstruction = MethodReader.GetInstructions(PrimaryConstructor)
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
    [UsedImplicitly]
    public static readonly LazyField<string?> ContainingFolder = new(
        "Beatmap#ContainingFolder",
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
    ///     Original: <c>Beatmap(string filename)</c>
    ///     b20240123: <c>#=znWyq67N7qygzmzTavD7zPPRqI0zwVZPmTyOyayVHbxCf</c>
    /// </summary>
    [UsedImplicitly]
    public static ConstructorInfo PrimaryConstructor => RuntimeType
        .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
        .Single();

    [UsedImplicitly]
    public static Type RuntimeType => (Score.ConstructorFromReplayAndMap.Reference as ConstructorInfo)!
        .GetParameters()[1] // 2nd param is of type Beatmap
        .ParameterType;

    /// <summary>
    ///     Utility wrapper to get the full beatmap path of a <c>Beatmap</c>.
    /// </summary>
    /// <param name="beatmap">An instance of <c>Beatmap</c> that was initialized with the filepath.</param>
    /// <returns>The absolute path, or null if this isn't a file-backed Beatmap.</returns>
    [UsedImplicitly]
    public static string? GetBeatmapPath(object beatmap)
    {
        var filename = Filename.Get(beatmap);
        var folder = ContainingFolder.Get(beatmap);
        if (filename == null || folder == null) return null;

        var osuDir = Path.GetDirectoryName(OsuAssembly.Assembly.Location)!;

        return Path.Combine(osuDir, "Songs", folder, filename);
    }
}