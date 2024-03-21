using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

// ReSharper disable InconsistentNaming UnusedType.Global UnusedType.Local UnusedMember.Local

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.GameplayElements.Events.EventManager</c>
///     b20240102.2: <c>#=zbJqkrJF69yLASpsnblYMeq3jWETw</c>
/// </summary>
[UsedImplicitly]
public static class EventManager
{
    /// <summary>
    ///     Original: <c>set_ShowStoryboard</c> (property setter)
    ///     b20240102.2: <c>#=zKZugEelWoTXb</c>
    /// </summary>
    /// <returns></returns>
    [UsedImplicitly]
    public static readonly LazyMethod set_ShowStoryboard = new(
        "EventManager#ShowStoryboard.set",
        new[]
        {
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
        }
    );

    /// <summary>
    ///     The compiler generated backing field for the <c>ShowStoryboard</c> property.
    ///     See: <see cref="set_ShowStoryboard" />
    /// </summary>
    [UsedImplicitly]
    public static LazyField ShowStoryboard_backing = new(
        "EventManager#ShowStoryboard.backing",
        () =>
        {
            // Find a single StoreField instruction in the setter for this property
            var instruction = MethodReader
                .GetInstructions(set_ShowStoryboard.Reference)
                .Single(inst => inst.Opcode == Stsfld);

            return (FieldInfo)instruction.Operand;
        }
    );
}