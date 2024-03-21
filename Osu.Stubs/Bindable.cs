using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Helpers.Bindable</c>
///     b20240102.2: <c>#=zDruHkLGdhQjyjYxqzw==</c>
/// </summary>
[UsedImplicitly]
public static class Bindable
{
    /// <summary>
    ///     Original: <c>Value.get</c> (property getter method)
    ///     b20240102.2: <c>#=zHO4Uaog=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod GetValue = new("Bindable#Value.get", () =>
    {
        var instructions = MethodReader.GetInstructions(SongSelection.beatmapTreeManager_OnRightClicked.Reference);

        // Get reference to Bindable:Value.get (property getter)
        return (MethodBase)instructions
            .First(inst => inst.Opcode == Callvirt)
            .Operand;
    });
}