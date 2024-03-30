using System;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     A marker for osu! patches for <see cref="OsuPatchProcessor" /> to handle.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OsuPatch : Attribute;