using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameplayElements.Scoring.Processors;

/// <summary>
///     Original: <c>osu.GameplayElements.Scoring.Processors.ScoreProcessor</c>
///     b20240123: <c>#=zBbxc56nwToQ2q_6LjFIHXSZoq$I8pyNIyxBZVi76rJHE</c>
/// </summary>
[PublicAPI]
public static class ScoreProcessor
{
    /// <summary>
    ///     Original: <c>Reset(bool storeStatistics)</c>
    ///     b20240123: <c>#=z6nCyHQk=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Clear = LazyMethod.ByPartialSignature(
        "osu.GameplayElements.Scoring.Processors.ScoreProcessor::Clear(bool)",
        [
            Ldc_R8,
            Stfld,
            Ldarg_0,
            Ldc_R8,
            Stfld,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>Add(ScoreChange change)</c>
    ///     b20240123: <c>#=zJdXS36o=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod AddScoreChange = LazyMethod.ByPartialSignature(
        "osu.GameplayElements.Scoring.Processors.ScoreProcessor::Add(ScoreChange)",
        new[]
        {
            Brfalse_S,
            Ldarg_1,
            Ldfld,
            Ldc_I4_0,
            Ble_S,
            Ldarg_1,
            Dup,
            Ldfld,
            Ldc_I4_1,
            Add,
            Stfld,
            Br_S,
            Ldarg_1,
        }
    );

    /// <summary>
    ///     Original: <c>MaximumCombo</c>
    ///     b20240123: <c>#=zqaALxNP6RgRC</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<int> MaximumCombo = new(
        "osu.GameplayElements.Scoring.Processors.ScoreProcessor::MaximumCombo",
        () => AddScoreChange.Reference.DeclaringType!
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(field => field.IsFamily && field.FieldType == typeof(int)) // checks for "protected int"
    );
}