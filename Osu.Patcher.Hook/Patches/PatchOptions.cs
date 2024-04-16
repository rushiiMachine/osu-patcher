using System;
using System.Collections.Generic;
using System.Linq;
using Osu.Patcher.Hook.Patches.PatcherOptions;

namespace Osu.Patcher.Hook.Patches;

/// <summary>
///     Custom options providers to be injected into the Options menu by <see cref="AddOptionsToUi" />.
/// </summary>
public abstract class PatchOptions
{
    /// <summary>
    ///     Create new options to be added as children into the patcher section of settings.
    ///     These should be of type <c>OptionElement</c>.
    /// </summary>
    public abstract IEnumerable<object> CreateOptions();

    /// <summary>
    ///     When called, initialize internal state to what is read from the config file.
    /// </summary>
    public abstract void Load(Settings config);

    /// <summary>
    ///     When called, save current internal state to the target config to be saved.
    /// </summary>
    public abstract void Save(Settings config);

    /// <summary>
    ///     Finds and initializes an instance of every single type extending <see cref="PatchOptions" />.
    /// </summary>
    public static IEnumerable<PatchOptions> CreateAllPatchOptions()
    {
        var allTypes = typeof(PatchOptions).Assembly.GetTypes();

        return allTypes
            .Where(t => t.IsSubclassOf(typeof(PatchOptions)))
            .Select(t => (PatchOptions)Activator.CreateInstance(t));
    }
}