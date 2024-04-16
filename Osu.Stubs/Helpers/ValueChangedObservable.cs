using System.Linq;
using JetBrains.Annotations;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Helpers;

[PublicAPI]
public static class ValueChangedObservable
{
    /// <summary>
    ///     Original: <c>osu.Helpers.ValueChangedObservable</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Helpers.ValueChangedObservable",
        () => Bindable.Generic.Class.Reference
            .GetInterfaces()
            .Where(type => type.GetMembers().Length == 5)
            .SingleOrNull()!
    );
}