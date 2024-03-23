using System;
using System.IO;
using System.Management;
using System.Runtime.Versioning;
using HoLLy.ManagedInjector;

namespace Osu.Patcher.Injector;

[SupportedOSPlatform("windows")]
internal static class Injector
{
    public static void Main()
    {
        try
        {
            using var proc = new InjectableProcess(GetOsuPid());
            var dllPath = Path.GetFullPath(typeof(Injector).Assembly.Location + @"\..\osu!.hook.dll");

            proc.Inject(dllPath, "Osu.Patcher.Hook.Hook", "Initialize");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);

            Console.WriteLine("\nPress any key to continue...");
            Console.Write("\a"); // Bell sound
            Console.ReadKey();
        }
    }

    /// <summary>
    ///     Find a <c>osu!.exe</c> process that has a <c>devserver</c> in the cli arguments. (Not connected to Bancho)
    /// </summary>
    /// <returns>The process id of the first matching process.</returns>
    /// <exception cref="Exception">If found invalid osu! process or no process at all.</exception>
    private static uint GetOsuPid()
    {
        using var mgmt = new ManagementClass("Win32_Process");
        using var processes = mgmt.GetInstances();

        foreach (var process in processes)
        {
            var exe = (string)process["Name"];
            var pid = (uint)process["ProcessId"];
            var cli = (string)process["CommandLine"];

            if (exe != "osu!.exe") continue;

            // Make sure there's a "-devserver xyz" in cli args
            var args = cli.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args is not [_, "-devserver", { Length: > 3 }])
                throw new Exception("Will not inject into osu! connected to Bancho!");

            return pid;
        }

        throw new Exception("Cannot find a running osu! process!");
    }
}