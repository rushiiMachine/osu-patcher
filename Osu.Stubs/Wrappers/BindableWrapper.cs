using System;
using JetBrains.Annotations;
using Osu.Stubs.Helpers;
using Osu.Utils.Extensions;

namespace Osu.Stubs.Wrappers;

/// <summary>
///     Wrapper around the osu! Bindable to make using generics way easier
/// </summary>
/// <typeparam name="T">A compile-time accessible type that is compatible with the target Bindable's inner type.</typeparam>
[PublicAPI]
public class BindableWrapper<T>
{
    private readonly Bindable _bindableObjectStub = null!;
    private readonly BindableType _bindableType;

    /// <summary>
    ///     Create a wrapper around any of the existing Bindable types.
    /// </summary>
    /// <param name="bindable">An existing bindable of the correct type.</param>
    public BindableWrapper(object bindable)
    {
        Bindable = bindable;

        // Find and verify the bindable type against T
        if (bindable.GetType().IsConstructedGenericType &&
            bindable.GetType().GetGenericTypeDefinition() == Helpers.Bindable.Generic.Class.Reference)
        {
            // Is Bindable<?>
#if DEBUG
            var typeParam = bindable.GetType()
                .GetGenericArguments()
                .GetOrDefault(0, null);

            if (!typeof(T).IsAssignableFrom(typeParam))
                throw new Exception($"Existing Bindable<T>'s type parameter ({typeParam}) " +
                                    $"cannot be assigned to {typeof(T)}");
#endif

            // Is Bindable<T>
            _bindableType = BindableType.Object;
            _bindableObjectStub = new Bindable(typeof(T));
        }
        else if (bindable.GetType() == BindableBool.Class.Reference)
        {
            // Is BindableBool
            _bindableType = BindableType.Bool;
        }
        else
        {
            throw new ArgumentException("Unknown existing bindable type");
        }
    }

    /// <summary>
    ///     Create a new Bindable.
    /// </summary>
    /// <param name="type">The type of the bindable to create. (<c>Bindable{T}</c>, <c>BindableBool</c>, etc.)</param>
    /// <param name="initialValue">The first value to fill the Bindable.</param>
    public BindableWrapper(BindableType type, T initialValue)
    {
        EnsureTMatch(type);

        if (_bindableType == BindableType.Object)
            _bindableObjectStub = new Bindable(typeof(T));

        _bindableType = type;
        Bindable = CreateBindable(type, initialValue);
    }

    /// <summary>
    ///     Create a new Bindable with a default value.
    /// </summary>
    /// <param name="type">The type of the bindable to create. (<c>Bindable{T}</c>, <c>BindableBool</c>, etc.)</param>
    /// <param name="initialValue">The first value to fill the Bindable.</param>
    /// <param name="defaultValue">The default value which can be different from initial.</param>
    public BindableWrapper(BindableType type, T initialValue, T? defaultValue) : this(type, initialValue)
    {
        // Set the default value
        switch (type)
        {
            case BindableType.Object:
                _bindableObjectStub.Default.Set(Bindable, defaultValue);
                break;
            case BindableType.Bool:
                BindableBool.Default.Set(Bindable, defaultValue);
                break;
            case BindableType.Double:
                throw new NotImplementedException("BindableDouble is not supported");
            case BindableType.Int:
                throw new NotImplementedException("BindableInt is not supported");
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    /// <summary>
    ///     Get the inner raw Bindable instance.
    /// </summary>
    public object Bindable { get; }

    /// <summary>
    ///     Get/Set the inner value of this bindable.
    /// </summary>
    public T Value
    {
        get => _bindableType switch
        {
            BindableType.Object => _bindableObjectStub.GetValue.Invoke<T>(Bindable),
            BindableType.Bool => BindableBool.GetValue.Invoke<T>(Bindable),
            BindableType.Double => throw new NotImplementedException("BindableDouble is not supported"),
            BindableType.Int => throw new NotImplementedException("BindableInt is not supported"),
            _ => throw new ArgumentOutOfRangeException(nameof(BindableType), _bindableType, null),
        };
        set
        {
            switch (_bindableType)
            {
                case BindableType.Object:
                    _bindableObjectStub.SetValue.Invoke(Bindable, [value]);
                    break;
                case BindableType.Bool:
                    BindableBool.SetValue.Invoke(Bindable, [value]);
                    break;
                case BindableType.Double:
                    throw new NotImplementedException("BindableDouble is not supported");
                case BindableType.Int:
                    throw new NotImplementedException("BindableInt is not supported");
                default:
                    throw new ArgumentOutOfRangeException(nameof(BindableType), _bindableType, null);
            }
        }
    }

    /// <summary>
    ///     Ensure that the generic parameter <typeparamref name="T" /> matches
    ///     any restrictions based on the target bindable type.
    /// </summary>
    /// <exception cref="ArgumentException">If T doesn't match the bindable type.</exception>
    private static void EnsureTMatch(BindableType type)
    {
        switch (type)
        {
            case BindableType.Object:
            case BindableType.Bool when typeof(T) == typeof(bool):
            case BindableType.Double when typeof(T) == typeof(double):
            case BindableType.Int when typeof(T) == typeof(int):
                return;
            default:
                throw new ArgumentException("Supplied value does not match the supplied bindable type!");
        }
    }

    /// <summary>
    ///     Constructs a new bindable with an initial value.
    /// </summary>
    /// <param name="type">The type of the bindable to create. (<c>Bindable{T}</c>, <c>BindableBool</c>, etc.)</param>
    /// <param name="value">The new active value to be set.</param>
    /// <returns>An instance of a bindable.</returns>
    private object CreateBindable(BindableType type, object? value) =>
        type switch
        {
            BindableType.Object => _bindableObjectStub.Constructor.Invoke([value]),
            BindableType.Bool => BindableBool.Constructor.Invoke([value]),
            BindableType.Double => throw new NotImplementedException("BindableDouble is not supported"),
            BindableType.Int => throw new NotImplementedException("BindableInt is not supported"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
}