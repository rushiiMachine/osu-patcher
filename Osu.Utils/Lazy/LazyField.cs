using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Osu.Utils.Lazy;

/// <summary>
///     A reference to a field that gets located at runtime and interacted with reflectively.
/// </summary>
/// <typeparam name="T">The type this field should be treated as.</typeparam>
[PublicAPI]
public class LazyField<T> : ILazy<FieldInfo>
{
    private readonly Lazy<FieldInfo> _lazy;

    /// <summary>
    ///     A wrapper around Lazy for fields.
    /// </summary>
    /// <param name="name"><c>Class#Field</c> name of what this signature is matching.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    public LazyField(string name, Func<FieldInfo> action)
    {
        Name = name;
        _lazy = new Lazy<FieldInfo>(action);
    }

    public string Name { get; }

    public FieldInfo Reference => this.GetReference(Name, _lazy);

    /// <summary>
    ///     Gets the current value of this field.
    /// </summary>
    /// <param name="instance">An instance of the field's enclosing class, if not static.</param>
    /// <returns>The current field value casted to the type defined by this LazyField.</returns>
    public T Get(object? instance = null) =>
        (T)Reference.GetValue(instance);

    /// <summary>
    ///     Set a instance field to a specific value.
    /// </summary>
    /// <param name="instance">An instance of the field's enclosing class.</param>
    /// <param name="value">The new value.</param>
    public void Set(object instance, T value) =>
        Reference.SetValue(instance, value);

    /// <summary>
    ///     Set a static field to a specific value.
    /// </summary>
    /// <param name="value">The new value.</param>
    public void Set(T value) =>
        Reference.SetValue(null, value);
}