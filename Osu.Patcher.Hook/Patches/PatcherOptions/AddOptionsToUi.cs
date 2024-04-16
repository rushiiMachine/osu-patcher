using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Options;
using Osu.Utils.Extensions;
using static Osu.Patcher.Hook.Patches.CustomStrings.CustomStrings;

namespace Osu.Patcher.Hook.Patches.PatcherOptions;

/// <summary>
///     Add this patcher's options to the options menu at the bottom.
/// </summary>
[OsuPatch]
[HarmonyPatch]
[UsedImplicitly]
internal static class AddOptionsToUi
{
    [UsedImplicitly]
    [HarmonyTargetMethod]
    private static MethodBase Target() => Options.InitializeOptions.Reference;

    [UsedImplicitly]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void After(object __instance) =>
        Options.Add.Invoke(__instance, [CreatePatcherCategoryOption()]);

    private static object CreatePatcherCategoryOption()
    {
        // https://fontawesome.com/icons/eye-dropper?f=classic&s=solid
        const int fontAwesomeSyringe = 0xF1FB;

        var osuPatcherString = AddOsuString("OsuPatcher", "osu!patcher");
        var categoryChildren = new[]
        {
            CreatePatcherSectionOption(),
            CreatePatchOptionSpacer(2),
            CreatePatcherVersionOption(),
        }.ToType(OptionElement.Class.Reference);

        var category = OptionCategory.Constructor.Invoke([osuPatcherString, fontAwesomeSyringe]);
        OptionElement.SetChildren.Invoke(category, [categoryChildren]);

        return category;
    }

    private static object CreatePatcherSectionOption()
    {
        var patcherString = AddOsuString("Patcher", "Patcher");

        // Collect the various options from patches into OptionElement[]
        var sectionChildren = CollectOptions()
            .ToArray().ToType(OptionElement.Class.Reference);

        var keywords = new[] { "patcher", "injector" };
        var section = OptionSection.Constructor.Invoke([patcherString, keywords]);
        OptionSection.SetChildren.Invoke(section, [sectionChildren]);

        return section;
    }

    private static object CreatePatcherVersionOption()
    {
        // TODO: make it open release page when clicked
        const string version = "1.0.0"; // TODO: use real assembly version
        return OptionVersion.Constructor.Invoke([$"Patcher v{version}"]);
    }

    private static IEnumerable<object> CollectOptions()
    {
        // Collect all classes extending CustomOptions and initialize them to call CreateOptionsSprites()
        var options = Hook.PatchOptions
            .SelectMany(opts => opts.CreateOptions());

        return options;
    }

    private static object CreatePatchOptionSpacer(int lines = 1) => OptionText.Constructor.Invoke([
        /* title: */ new string('\n', lines),
        /* onClick: */ null,
    ]);
}