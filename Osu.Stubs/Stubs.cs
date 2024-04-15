using System;
using Osu.Utils.Lazy;

namespace Osu.Stubs;

/// <summary>
///     Stub marker for a type extending <see cref="ILazy{T}" />.
///     Used for testing stubs on new versions of osu!.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class Stub : Attribute;

/// <summary>
///     Stub marker on a static field that provides an instance of the
///     current stub class to be used as the target for getting <see cref="ILazy{T}" />
///     instances that target generic type definitions.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class StubInstance : Attribute;