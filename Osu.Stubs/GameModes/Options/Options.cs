using JetBrains.Annotations;
using Osu.Utils.Lazy;
using static System.Reflection.Emit.OpCodes;

namespace Osu.Stubs.GameModes.Options;

[PublicAPI]
public static class Options
{
    /// <summary>
    ///     Original: <c>osu.GameModes.Options.Options</c>
    ///     b20240123: <c>#=zIP$6zD_IcaL0ugvXAFTvJYG2gFLx</c>
    /// </summary>
    [Stub]
    public static readonly LazyType Class = new(
        "osu.GameModes.Options.Options",
        () => InitializeOptions!.Reference.DeclaringType!
    );

    /// <summary>
    ///     Original: <c>InitializeOptions()</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod InitializeOptions = LazyMethod.ByPartialSignature(
        "osu.GameModes.Options.Options::InitializeOptions()",
        [
            Ldc_I4,
            Ldsfld,
            Ldnull,
            Newobj,
            Stelem_Ref,
            Callvirt,
            Ldloc_2,
            Stelem_Ref,
            Dup,
            Ldc_I4_1,
            Ldc_I4_S,
            Ldnull,
        ]
    );

    /// <summary>
    ///     Original: <c>Add(OptionElement option)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod Add = LazyMethod.ByPartialSignature(
        "osu.GameModes.Options.Options::Add(OptionElement)",
        [
            Call,
            Ldloc_0,
            Callvirt,
            Brtrue_S,
            Leave_S,
            Ldloc_0,
            Brfalse_S,
            Ldloc_0,
            Callvirt,
            Endfinally,
            Ldarg_1,
            Isinst,
            Stloc_2,
        ]
    );

    /// <summary>
    ///     Original: <c>ReloadElements(bool jumpToTop)</c>
    ///     b20240123: <c></c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod ReloadElements = LazyMethod.ByPartialSignature(
        "osu.GameModes.Options.Options::ReloadElements(bool)",
        [
            Dup,
            Brtrue_S,
            Pop,
            Ldsfld,
            Ldftn,
            Newobj,
            Dup,
            Stsfld,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Callvirt,
            Ldarg_0,
            Ldfld,
            Ldc_I4_1,
            Callvirt,
            Ldarg_0,
            Call,
            Ldarg_0,
            Call,
            Ret,
        ]
    );

    /// <summary>
    ///     Original: <c>CanExpand</c> (property getter)
    ///     b20240123: <c>#=zxcgzxWXF$WLr</c>
    /// </summary>
    [Stub]
    public static readonly LazyMethod<bool> GetCanExpand = LazyMethod<bool>.ByPartialSignature(
        "osu.GameModes.Options.Options.Options::get_CanExpand()",
        new[]
        {
            Ldc_I4,
            Stloc_1,
            Ldsfld,
            Ldloc_1,
            Stloc_3,
            Stloc_2,
            Ldloc_2,
            Ldloc_3,
            And,
            Ldc_I4_0,
            Cgt,
            Ret,
        }
    );
}