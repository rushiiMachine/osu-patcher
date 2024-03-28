using System.Reflection;

namespace Osu.Utils.Extensions;

public static class ReflectionExtensions
{
    /// <summary>
    ///     Reflectively get a field's value and cast it to a specific type.
    /// </summary>
    /// <param name="field">The target field.</param>
    /// <param name="instance">Instance of the enclosing class, if any.</param>
    /// <typeparam name="T">Field type to cast to.</typeparam>
    public static T GetValue<T>(this FieldInfo field, object? instance)
        => (T)field.GetValue(instance);

    /// <summary>
    ///     Reflectively invoke a method and cast the return value to a specific type.
    /// </summary>
    /// <param name="method">The target method.</param>
    /// <param name="instance">Instance of the enclosing class, if any.</param>
    /// <param name="parameters">Parameters to pass to the method, if any.</param>
    /// <typeparam name="T">Return type to cast to.</typeparam>
    public static T Invoke<T>(this MethodBase method, object? instance, object?[]? parameters)
        => (T)method.Invoke(instance, parameters);
}