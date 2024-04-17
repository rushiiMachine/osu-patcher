using System;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.Helpers;

[PublicAPI]
public class Obfuscated
{
    [StubInstance]
    public static readonly Obfuscated Generic = new();

    /// <summary>
    ///     Original: <c>osu.Helpers.Obfuscated{T}</c>
    ///     b20240123: <c>#=z7wrq$zbpx0eedoF3ocH29DF53lUE{#=z62rU_UA=}</c>
    /// </summary>
    [Stub]
    public readonly LazyType Class;

    /// <summary>
    ///     Original: <c>Finalize()</c>
    ///     b20240123: <c>Finalize()</c>
    /// </summary>
    [Stub]
    public readonly LazyMethod Finalize;

    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter)
    ///     b20240123: <c>#=zHT6xZVI=</c>
    /// </summary>
    [Stub]
    public readonly LazyMethod<object> GetValue;

    /// <summary>
    ///     Create an either generic or generic bound stub template.
    /// </summary>
    /// <param name="T">Type of the T generic at runtime, or null for the generic definition.</param>
    public Obfuscated(Type? T = null)
    {
        var searchType = T != null
            ? Generic.Class.Reference.MakeGenericType(T)
            : null;

        Finalize = LazyMethod.ByPartialSignature(
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
            ],
            searchType
        );

        Class = new LazyType(
            "osu.Helpers.Obfuscated<T>",
            () => Finalize.Reference.DeclaringType!
        );

        GetValue = LazyMethod<object>.BySignature(
            "osu.Helpers.Obfuscated<T>::get_Value()",
            [
                Ldarg_0,
                Ldfld,
                Ldarg_0,
                Ldfld,
                Ldarg_0,
                Ldfld,
                Callvirt,
                Ret,
            ],
            Class.Reference // TODO: make this support lazy types instead
        );
    }
}