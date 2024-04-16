using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Options;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Graphics;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class pDropdownItem
{
    /// <summary>
    ///     Original: <c>osu.Graphics.pDropdownItem</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Graphics.pDropdownItem",
        // pDropdownItem is referenced from the OptionDropdown constructor as the parameter IEnumerable<pDropdownItem>
        () => OptionDropdown.Generic
            .Constructor.Reference
            .GetParameters()[1].ParameterType
            .GetGenericArguments()
            .First()
    );

    /// <summary>
    ///     Original: <c>pDropdownItem(string text, object value)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "osu.Graphics.pDropdownItem::pDropdownItem(string, object)",
        () => Class.Reference
            .GetDeclaredConstructors()
            .Single()
    );
}