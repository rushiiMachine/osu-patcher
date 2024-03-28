using System;
using System.Diagnostics;

namespace Osu.Patcher.Hook;

/// <summary>
///     Register a listener for <c>Debug</c> to listen to this patcher's debug logs.
/// </summary>
internal class DebugHook : TraceListener
{
    private DebugHook()
    {
    }

    public override string Name => "osu!patcher";

    public static void Initialize() => Debug.Listeners.Add(new DebugHook());

    public override void WriteLine(object o) =>
        Console.WriteLine($"[D] {o}");

    public override void WriteLine(string? message) =>
        Console.WriteLine($"[D] {message}");

    public override void WriteLine(object o, string category) =>
        Console.WriteLine($"[D] [{category}] {o}");

    public override void WriteLine(string? message, string category) =>
        Console.WriteLine($"[D] [{category}] {message}");

    public override void Write(object o) => WriteLine(o);
    public override void Write(string? message) => WriteLine(message);
    public override void Write(object o, string category) => WriteLine(o, category);
    public override void Write(string? message, string category) => WriteLine(message, category);
}