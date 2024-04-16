using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

[PublicAPI]
public class OptionElement
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Options.OptionElement</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameModes.Options.OptionElement",
        () => SetExpandedOnly!.Reference.DeclaringType
    );

    /// <summary>
    ///     Original: <c>set_ExpandedOnly(bool value)</c> (property setter)
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetExpandedOnly = LazyMethod.ByPartialSignature(
        "osu.GameModes.Options.OptionElement::set_ExpandedOnly(bool)",
        [
            Callvirt,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Ldloc_0,
            Callvirt,
            Brtrue_S,
            Leave_S,
            Ldloc_0,
            Brfalse_S,
            Ldloc_0,
            Callvirt,
            Endfinally,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>set_Children(IEnumerable{OptionElement} value)</c> (property setter)
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetChildren = LazyMethod.ByPartialSignature(
        "osu.GameModes.Options.OptionElement::set_Children(IEnumerable<OptionElement>)",
        [
            Ldarg_0,
            Call,
            Brfalse_S,
            Ldloc_1,
            Ldc_I4_1,
            Callvirt,
            Ldloc_0,
            Callvirt,
            Brtrue_S,
            Leave_S,
            Ldloc_0,
            Brfalse_S,
            Ldloc_0,
            Callvirt,
            Endfinally,
            Ret,
        ]
    );
}