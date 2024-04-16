using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     Original: <c>osu.GameModes.Options.OptionSection</c>
///     b20240123: <c></c>
/// </summary>
[PublicAPI]
public class OptionSection
{
    /// <summary>
    ///     Original: <c>OptionSection(OsuString title, IEnumerable{string} keywords)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = LazyConstructor.BySignature(
        "osu.GameModes.Options.OptionSection::OptionSection(OsuString, IEnumerable<string>)",
        [
            Ldarg_0,
            Ldarg_1,
            Call,
            Ldarg_2,
            Call,
            Ldarg_0,
            Ldarga_S,
            Constrained,
            Callvirt,
            Call,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>set_Children(IEnumerable{OptionElement} value)</c> (property setter)
    ///     b20240123: <c></c>
    ///     Override of <c>OptionElement::set_Children(...)</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetChildren = new(
        "osu.GameModes.Options.OptionSection::set_Children(IEnumerable<OptionElement>)",
        () => Constructor.Reference
            .DeclaringType!
            .GetMethod(OptionElement.SetChildren.Reference.Name, BindingFlags.NonPublic | BindingFlags.Instance)
    );
}