using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

[PublicAPI]
public class OptionSection
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Options.OptionSection</c>
    ///     b20250309.2: <c>#=z6o8TWFro5CFT4nNbnpcUroUVOeFA</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameModes.Options.OptionElement",
        () => Constructor2!.Reference.DeclaringType
    );

    /// <summary>
    ///     Original: <c>OptionSection(OsuString title, IEnumerable{string} keywords)</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "osu.GameModes.Options.OptionSection::OptionSection(OsuString title, IEnumerable{string} keywords)",
        () => Class.Reference.GetDeclaredConstructors().First()
    );

    /// <summary>
    ///     Original: <c>OptionSection(string title, IEnumerable{string} keywords)</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor2 = LazyConstructor.ByPartialSignature(
        "osu.GameModes.Options.OptionSection::OptionSection(string, IEnumerable{string} keywords)",
        [
            Callvirt,
            Stloc_1,
            Ldarg_0,
            Ldloc_1,
            Call,
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