using System;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

/// <summary>
///     This is a stub supporting the T generic in OptionDropdown.
///     All <see cref="ILazy{T}" />s will be bound to a type with T.
/// </summary>
[PublicAPI]
public class OptionDropdown
{
    [StubInstance]
    public static readonly OptionDropdown Generic = new();

    /// <summary>
    ///     Original: <c>osu.GameModes.Options.OptionDropdown{T}</c>
    /// </summary>
    [Stub]
    public readonly LazyType Class;

    /// <summary>
    ///     Original:
    ///     <c>
    ///         OptionDropdown(OsuString title,
    ///         IEnumerable{pDropdownItem} items = null,
    ///         Bindable{T} binding = null,
    ///         EventHandler onChange = null)
    ///     </c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public readonly LazyConstructor Constructor;

    /// <summary>
    ///     Create an either generic or generic bound stub template.
    /// </summary>
    /// <param name="T">Type of the T generic at runtime, or null for the generic definition.</param>
    public OptionDropdown(Type? T = null)
    {
        var searchType = T != null
            ? Generic.Class.Reference.MakeGenericType(T)
            : null;

        Constructor = LazyConstructor.ByPartialSignature(
            "osu.GameModes.Options.OptionDropdown<T>::OptionDropdown(OsuString, IEnumerable<pDropdownItem>, Bindable<T>, EventHandler",
            [
                Sub,
                Ldc_R4,
                Newobj,
                Dup,
                Ldc_I4_S,
                Stfld,
                Stfld,
                Ldarg_0,
                Ldfld,
                Ldarg_0,
                Ldftn,
                Newobj,
            ],
            searchType
        );

        Class = new LazyType(
            "osu.GameModes.Options.OptionDropdown<T>",
            () => Constructor.Reference.DeclaringType
        );
    }
}