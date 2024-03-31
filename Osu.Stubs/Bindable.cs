using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Helpers.Bindable</c>
///     b20240102.2: <c>#=zDruHkLGdhQjyjYxqzw==</c>
/// </summary>
[PublicAPI]
public static class Bindable
{
    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter method)
    ///     b20240102.2: <c>#=zHO4Uaog=</c>
    /// </summary>
    public static readonly LazyMethod<object> GetValue = new(
        "osu.Helpers.Bindable::get_Value()",
        () =>
        {
            var instructions = MethodReader
                .GetInstructions(SongSelection.BeatmapTreeManagerOnRightClicked.Reference);

            // Get reference to Bindable:Value.get (property getter)
            return (MethodInfo)instructions
                .First(inst => inst.Opcode == Callvirt)
                .Operand;
        }
    );
}