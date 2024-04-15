using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

/// <summary>
///     This is a stub supporting the T generic for Bindable.
///     All <see cref="ILazy{T}" />s will be bound to a type with T.
/// </summary>
[PublicAPI]
public class Bindable
{
    [StubInstance]
    public static readonly Bindable Generic = new();

    /// <summary>
    ///     Original: <c>osu.Helpers.Bindable{T}</c>
    ///     b20240102.2: <c>#=zDruHkLGdhQjyjYxqzw==</c>
    /// </summary>
    [Stub]
    public readonly LazyType Class;

    /// <summary>
    ///     Original: <c>Bindable(T value)</c>
    ///     b20240102.2: <c></c>
    /// </summary>
    [Stub]
    public readonly LazyConstructor Constructor;

    /// <summary>
    ///     Original: <c>Default</c>
    ///     b20240102.2: <c></c>
    /// </summary>
    [Stub]
    public readonly LazyField<object> Default;

    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter method)
    ///     b20240102.2: <c>#=zHO4Uaog=</c>
    /// </summary>
    [Stub]
    public readonly LazyMethod<object> GetValue;

    /// <summary>
    ///     Original: <c>set_Value(T value)</c> (property setter method)
    ///     b20240102.2: <c></c>
    /// </summary>
    [Stub]
    public readonly LazyMethod SetValue;

    /// <summary>
    ///     Create an either generic or generic bound stub template.
    /// </summary>
    /// <param name="T">Type of the T generic at runtime, or null for the generic definition.</param>
    public Bindable(Type? T = null)
    {
        var searchType = T != null
            ? Generic.Class.Reference.MakeGenericType(T)
            : null;

        SetValue = LazyMethod.ByPartialSignature(
            "osu.Helpers.Bindable<T>::set_Value(T)",
            [
                Box,
                Brtrue_S,
                Pop,
                Ldc_I4_0,
                Br_S,
                Ldarg_1,
                Box,
                Constrained,
                Callvirt,
                Ldc_I4_0,
                Ceq,
                Brfalse_S,
                Ret,
            ],
            searchType
        );

        Class = new LazyType(
            "osu.Helpers.Bindable<T>",
            () => SetValue.Reference.DeclaringType
        );
        
        GetValue = new LazyMethod<object>(
            "osu.Helpers.Bindable<T>::get_Value()",
            () => Class.Reference
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Single(method => method.Name == Obfuscated.Generic.GetValue.Reference.Name)
        );

        Constructor = new LazyConstructor(
            "osu.Helpers.Bindable<T>::Bindable(T)",
            () => Class.Reference
                .GetDeclaredConstructors() // Only a single constructor that has 1 parameter
                .Single(ctor => ctor.GetParameters().Length == 1)
        );

        Default = new LazyField<object>(
            "osu.Helpers.Bindable<T>::Default",
            () => Class.Reference
                .GetDeclaredFields()
                .Single(field => field.IsPublic) // Only a single field that is public
        );
    }
}