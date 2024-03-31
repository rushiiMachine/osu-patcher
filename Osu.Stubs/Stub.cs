using System;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Stub marker for a type extending <see cref="LazyInfo{T}" />.
///     Used for testing stubs on new versions of osu!.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class Stub : Attribute;