using JetBrains.Annotations;
using Osu.Stubs.GameModes.Options;
using Osu.Stubs.Helpers;
using Osu.Stubs.Root;

namespace Osu.Stubs.Wrappers;

[PublicAPI]
public static class OptionsUtils
{
    /// <summary>
    ///     Reloads the options panel to reset the available options.
    /// </summary>
    /// <param name="scrollToTop">Forcefully scroll to the top after reloading.</param>
    public static void ReloadOptions(bool scrollToTop = true)
    {
        var mainScheduler = GameBase.Scheduler.Get();
        var options = GameBase.Options.Get();

        var task = VoidDelegate.MakeInstance(() =>
            Options.ReloadElements.Invoke(options, [scrollToTop]));

        Scheduler.Add.Invoke(mainScheduler, [
            /* task: */ task,
            /* forceScheduled: */ false,
        ]);
    }
}