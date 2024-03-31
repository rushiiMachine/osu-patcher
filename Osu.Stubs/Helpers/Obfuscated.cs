using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

[PublicAPI]
public static class Obfuscated
{
    /// <summary>
    ///     Original: <c>osu.Helpers.Obfuscated{T}</c>
    ///     b20240123: <c>#=z7wrq$zbpx0eedoF3ocH29DF53lUE{#=z62rU_UA=}</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.Helpers.Obfuscated<T>",
        () => Finalize!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>Finalize()</c>
    ///     b20240123: <c>Finalize()</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Finalize = LazyMethod.ByPartialSignature(
        "osu.Helpers.Obfuscated<T>::Finalize()",
        [
            Newobj,
            Dup,
            Stsfld,
            Ldc_I4,
            Ldc_I4_0,
            Callvirt,
            Pop,
            Leave_S,
            Ldarg_0,
            Call,
            Endfinally,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter)
    ///     b20240123: <c>#=zHT6xZVI=</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<object> GetValue = new(
        "osu.Helpers.Obfuscated<T>::get_Value()",
        () => Class.Reference
            .GetRuntimeMethods()
            .First(mtd =>
                mtd.GetParameters().Length == 0 &&
                mtd.ReturnType.IsGenericParameter)
    );

    /// <summary>
    ///     Binds the generic parameter in <c>Obfuscated{T}</c> so the method becomes callable
    /// </summary>
    /// <param name="type">The type that should be bound to the T generic parameter.</param>
    /// <returns>A new bound method info that is invokable.</returns>
    public static MethodInfo BindGetValue(Type type) => Class.Reference
        .MakeGenericType(type)
        .GetMethod(GetValue.Reference.Name)!;
}