using System;
using System.Reflection;

namespace Osu.Stubs.Opcode;

public class LazyField
{
    private readonly Lazy<FieldInfo?> _lazy;
    private readonly string _name;

    /// <summary>
    ///     A wrapper around Lazy for fields. <see cref="LazyMethod" />
    /// </summary>
    /// <param name="name"><c>Class#Field</c> name of what this signature is matching.</param>
    /// <param name="action">The lazy action to run when the value is needed.</param>
    internal LazyField(string name, Func<FieldInfo?> action)
    {
        _name = name;
        _lazy = new Lazy<FieldInfo?>(action);
    }

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
}