using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Scoring.Processors.ScoreProcessor</c>
///     b20240123: <c>#=zBbxc56nwToQ2q_6LjFIHXSZoq$I8pyNIyxBZVi76rJHE</c>
/// </summary>
[UsedImplicitly]
public class ScoreProcessor
{
    /// <summary>
    ///     Original: <c>Reset(bool storeStatistics)</c>
    ///     b20240123: <c>#=z6nCyHQk=</c>
    /// </summary>
    [UsedImplicitly]
    public static LazyMethod Clear = new(
        "ScoreProcessor#Clear(...)",
        new[]
        {
            Ldc_R8,
            Stfld,
            Ldarg_0,
            Ldc_R8,
            Stfld,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Ret,
        }
    );

    /// <summary>
    ///     Original: <c>Add(ScoreChange change)</c>
    ///     b20240123: <c>#=zJdXS36o=</c>
    /// </summary>
    [UsedImplicitly]
    public static LazyMethod AddScoreChange = new(
        "ScoreProcessor#Add(...)",
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
    [UsedImplicitly]
    public static LazyField<int> MaximumCombo = new(
        "ScoreProcessor#MaximumCombo",
        () =>
        {
            return AddScoreChange.Reference.DeclaringType!
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(field => field.FieldType == typeof(int) && field.IsFamily); // protected int
        }
    );
}