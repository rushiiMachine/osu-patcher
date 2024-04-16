using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Osu.Stubs.GameModes.Options;
using Osu.Stubs.Wrappers;

namespace Osu.Patcher.Hook.Patches.Mods.AudioPreview;

[UsedImplicitly]
internal class AudioPreviewOptions : PatchOptions
{
    /// <summary>
    ///     Global toggle for disabling and enabling the patch.
    /// </summary>
    public static readonly BindableWrapper<bool> Enabled =
        new(BindableType.Bool, false, Settings.Default.EnableModAudioPreview);

    public override IEnumerable<object> CreateOptions() =>
    [
        OptionCheckbox.Constructor.Invoke([
            /* title: */ "Apply DT/NC/HT audio effects live",
            /* tooltip: */ "Applies the speed & pitch modifiers during song selection, like osu!lazer.",
            /* binding: */ Enabled.Bindable,
            /* onChange: */ new EventHandler((_, _) => ModAudioEffects.ApplyModEffects(!Enabled.Value)),
        ]),
    ];

    public override void Load(Settings config) => Enabled.Value = config.EnableModAudioPreview;
    public override void Save(Settings config) => config.EnableModAudioPreview = Enabled.Value;
}