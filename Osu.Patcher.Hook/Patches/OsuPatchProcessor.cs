using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Utils.Extensions;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Custom patch processor to automatically add additional things to the target patches.
/// </summary>
public class OsuPatchProcessor : PatchClassProcessor
{
    // List<AttributePatch> HarmonyLib.PatchClassProcessor::patchMethods
    private static readonly FieldInfo FieldPatchMethods = typeof(PatchClassProcessor)
        .GetField("patchMethods", AccessTools.all)!;

    // AttributePatch HarmonyLib.AttributePatch::Create(MethodInfo)
    private static readonly MethodInfo MethodCreateAttributePatch =
        AccessTools.Method("HarmonyLib.AttributePatch:Create");

    // void OsuPatchProcessor::BaseFinalizer(Exception?)
    private static readonly MethodInfo GenericMethodBaseFinalizer = typeof(OsuPatchProcessor)
        .GetMethod("BaseFinalizer", AccessTools.all)!;

    // void List<HarmonyLib.AttributePatch>::Add(HarmonyLib.AttributePatch)
    private static readonly MethodInfo ListAttributePatchAdd = typeof(List<>)
        .MakeGenericType([AccessTools.TypeByName("HarmonyLib.AttributePatch")])
        .GetMethod("Add")!;

    /// <summary>
    ///     Create a new class patch processor that adds extra attribute patches to the patch.
    /// </summary>
    public OsuPatchProcessor(Harmony instance, Type type) : base(instance, type)
    {
        AddFinalizerPatch(type);
    }

    /// <summary>
    ///     Checks if the specified type is marked as an <see cref="OsuPatch" />.
    /// </summary>
    public static bool IsOsuPatch(Type type) =>
        type.GetCustomAttribute(typeof(OsuPatch)) != null;

    /// <summary>
    ///     Adds a new <see cref="HarmonyFinalizer" /> attribute patch to the
    ///     current patch processor targeting a specific <paramref name="type" />.
    /// </summary>
    private void AddFinalizerPatch(Type type)
    {
        var boundMethod = GenericMethodBaseFinalizer.MakeGenericMethod([type]);
        var attributePatch = MethodCreateAttributePatch.Invoke<object>(null, [boundMethod]);

        var attributePatches = FieldPatchMethods.GetValue(this)!;
        ListAttributePatchAdd.Invoke(attributePatches, [attributePatch]);
    }

    [UsedImplicitly]
    [HarmonyFinalizer]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void BaseFinalizer<P>(Exception? __exception)
    {
        if (__exception == null)
            return;

        Console.WriteLine($"[F] [Patch{typeof(P).Name}]: {__exception}");
    }
}