using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Helpers;

[PublicAPI]
public static class BindableBool
{
    /// <summary>
    ///     Original: <c>osu.Helpers.BindableBool</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Helpers.BindableBool",
        () => Constructor!.Reference.DeclaringType
    );

    /// <summary>
    ///     Original: <c>BindableBool(bool value = false)</c>
    /// </summary>
    [Stub]
    public static readonly LazyConstructor Constructor = new(
        "osu.Helpers.BindableBool::BindableBool(bool)",
        () => OsuAssembly.Types
            // Find all types that extend ValueChangedObservable
            .Where(type => ValueChangedObservable.Class.Reference.IsAssignableFrom(type))
            // Get all the declared constructors
            .SelectMany(type => type.GetDeclaredConstructors())
            // Find the single constructor that has a single parameter of type bool
            .Where(ctor => ctor.GetParameters().SingleOrNull()?.ParameterType == typeof(bool))
            .SingleOrNull()!
    );

    /// <summary>
    ///     Original: <c>Default</c>
    /// </summary>
    [Stub]
    public static readonly LazyField<object> Default = new(
        "osu.Helpers.BindableBool::Default",
        () => Class.Reference
            .GetDeclaredFields()
            .Where(field => field.Name == Bindable.Generic.Default.Reference.Name)
            .FirstOrNull()!
    );

    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter method)
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> GetValue = new(
        "osu.Helpers.BindableBool::get_Value()",
        () => Class.Reference
            .GetDeclaredMethods()
            .Where(method => method.Name == Bindable.Generic.GetValue.Reference.Name)
            .SingleOrNull()!
    );

    /// <summary>
    ///     Original: <c>set_Value()</c> (property setter method)
    /// </summary>
    [Stub]
    public static readonly LazyMethod SetValue = new(
        "osu.Helpers.BindableBool::set_Value(bool)",
        () => Class.Reference
            .GetDeclaredMethods()
            .Where(method => method.Name == Bindable.Generic.SetValue.Reference.Name)
            .SingleOrNull()!
    );
}