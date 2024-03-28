using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

/// <summary>
///     Original: <c>osu.Helpers.Obfuscated{T}</c>
///     b20240123: <c>#=z7wrq$zbpx0eedoF3ocH29DF53lUE{#=z62rU_UA=}</c>
/// </summary>
[UsedImplicitly]
public class Obfuscated
{
    /// <summary>
    ///     Original: <c>Finalize()</c>
    ///     b20240123: <c>Finalize()</c>
    /// </summary>
    private static readonly LazyMethod Finalize = new(
        "Obfuscated#Finalize()",
        new[]
        {
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
        }
    );

    /// <summary>
    ///     Original: <c>get_Value()</c> (property getter)
    ///     b20240123: <c>#=zHT6xZVI=</c>
    /// </summary>
    [UsedImplicitly]
    public static readonly LazyMethod<object> GetValue = new(
        "Obfuscated#get_Value()",
        () => RuntimeType.GetRuntimeMethods()
            .First(mtd => mtd.GetParameters().Length == 0 && mtd.ReturnType.IsGenericParameter)
    );

    [UsedImplicitly]
    public static Type RuntimeType => Finalize.Reference.DeclaringType!;

    // Binds the generic parameter in Obfuscated<T> so the method becomes callable
    public static MethodBase BindGetValue(Type type) => RuntimeType
        .MakeGenericType(type)
        .GetMethod(GetValue.Reference.Name)!;
}