using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Osu.Stubs.Opcode;

/// <summary>
///     A reference to a field that gets located at runtime and interacted with reflectively.
/// </summary>
/// <typeparam name="T">The type this field should be treated as.</typeparam>
public class LazyField<T>
{
    private readonly Lazy<FieldInfo?> _lazy;
    private readonly string _name;

    /// <summary>
    ///     A wrapper around Lazy for fields.
    /// </summary>
    /// <param name="name"><c>Class#Field</c> name of what this signature is matching.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    internal LazyField(string name, Func<FieldInfo?> action)
    {
        _name = name;
        _lazy = new Lazy<FieldInfo?>(action);
    }

    [UsedImplicitly]
    public FieldInfo Reference
    {
        get
        {
            var value = _lazy.Value;

            if (value == null)
                throw new Exception($"Field {_name} was not found");

            return value;
        }
    }

    /// <summary>
    ///     Gets the current value of this field.
    /// </summary>
    /// <param name="instance">An instance of the field's enclosing class, if not static.</param>
    /// <returns>The current field value casted to the type defined by this LazyField.</returns>
    public T Get(object? instance = null) =>
        (T)Reference.GetValue(instance);
}