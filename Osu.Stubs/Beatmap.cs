using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Beatmaps.Beatmap</c>
///     b20240123: <c>#=znWyq67N7qygzmzTavD7zPPRqI0zwVZPmTyOyayVHbxCf</c>
/// </summary>
[UsedImplicitly]
public static class Beatmap
{
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

            Debug.Assert(storeInstruction.Opcode == OpCodes.Stfld);

            return (FieldInfo)storeInstruction.Operand;
        }
    );

    /// <summary>
    ///     Original: <c>ContainingFolderAbsolute</c>
    ///     b20240123: <c>#=zDmW9P6igScNm</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyField<string?> ContainingFolderAbsolute = new(
        "Beatmap#ContainingFolderAbsolute",
        () =>
        {
            // TODO: find this field properly
            return RuntimeType.Field("#=zDmW9P6igScNm");

            // // Second Stfld is a reference to ContainingFolderAbsolute
            // var storeInstruction = MethodReader
            //     .GetInstructions(PrimaryConstructor)
            //     .Where(inst => inst.Opcode == OpCodes.Stfld)
            //     .Skip(1)
            //     .First();
            //
            // return (FieldInfo)storeInstruction.Operand;
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
    ///     Utility wrapper to get the full beatmap path of a <c>Beatmap</c> object
    /// </summary>
    /// <param name="beatmap">An instance of <c>Beatmap</c> that was initialized with the filepath.</param>
    /// <returns>The absolute path, or null if this isn't a file-backed Beatmap.</returns>
    [UsedImplicitly]
    public static string? GetBeatmapPath(object beatmap)
    {
        var filename = Filename.Get(beatmap);
        var directory = ContainingFolderAbsolute.Get(beatmap);

        if (filename != null && directory != null)
        {
            return Path.Combine(directory, filename);
        }

        return null;
    }
}