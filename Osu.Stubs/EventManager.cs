using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.IL;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Events.EventManager</c>
///     b20240102.2: <c>#=zbJqkrJF69yLASpsnblYMeq3jWETw</c>
/// </summary>
[PublicAPI]
public static class EventManager
{
    /// <summary>
    ///     Original: <c>set_ShowStoryboard()</c> (property setter)
    ///     b20240102.2: <c>#=zKZugEelWoTXb</c>
    /// </summary>
    public static readonly LazyMethod SetShowStoryboard = LazyMethod.BySignature(
        "osu.GameplayElements.Events.EventManager::set_ShowStoryBoard()",
        [
            Ldarg_0,
            Ldsfld,
            Bne_Un_S,
            Ret,
            Ldarg_0,
            Stsfld, // This references the backing field of the ShowStoryboard property
            Ldsfld,
            Brfalse_S,
            Ldsfld,
            Callvirt,
            Ret,
        ]
    );

    /// <summary>
    ///     The compiler generated backing field for the <c>ShowStoryboard</c> property.
    ///     See: <see cref="SetShowStoryboard" />
    /// </summary>
    public static readonly LazyField<bool> ShowStoryboard = new(
        "osu.GameplayElements.Events.EventManager::[ShowStoryboard backing]",
        () =>
        {
            // Find a single StoreField instruction in the setter for this property
            var instruction = MethodReader
                .GetInstructions(SetShowStoryboard.Reference)
                .Single(inst => inst.Opcode == Stsfld);

            return (FieldInfo)instruction.Operand;
        }
    );
}