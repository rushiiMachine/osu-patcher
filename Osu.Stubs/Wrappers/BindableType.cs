namespace Osu.Stubs.Wrappers;

/// <summary>
///     osu! has different Bindable types that do not inherit from each other and as such
///     <see cref="BindableWrapper{T}" /> handles them.
/// </summary>
public enum BindableType : byte
{
    Object,
    Bool,
    Double,
    Int,
}