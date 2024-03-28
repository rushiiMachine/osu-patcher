using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Osu.Stubs.Opcode;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs;

[UsedImplicitly]
public class Obfuscated
{
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