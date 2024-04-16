using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.OptionCheckbox</c>
/// </summary>
[PublicAPI]
public class OptionCheckbox
{
    /// <summary>
    ///     Original:
    ///     <c>OptionCheckbox(string title, string tooltip, BindableBool binding = null, EventHandler onChange = null)</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Options.OptionCheckbox::OptionCheckbox(string, string, BindableBool, EventHandler)",
        [
            Br_S,
            Ldarg_3,
            Call,
            Ldarg_S,
            Call,
            Ldarg_0,
            Ldarg_3,
            Stfld,
            Ldarg_3,
            Brfalse_S,
            Ldarg_3,
            Ldarg_0,
            Ldftn,
            Newobj,
            Callvirt,
            Ret,
        ]
    );
}