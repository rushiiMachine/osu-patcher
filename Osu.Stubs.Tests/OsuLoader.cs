using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Osu.Stubs.Tests;

#pragma warning disable CS0618 // Type or member is obsolete

internal static class OsuLoader
{
    private static Task<Assembly>? _loadTask;

    /// <summary>
    ///     Download the latest osu! game file and load the assembly into the current process.
    /// </summary>
    public static Task<Assembly> UpdateAndLoad()
    {
        if (_loadTask != null)
            return _loadTask;

        if (Environment.Is64BitProcess)
            throw new Exception("Cannot load osu! into a 64-bit process!");

        return _loadTask = Task.Run(async () =>
        {
            Console.WriteLine("Updating osu!");

            var osuDir = Path.Combine(Assembly.GetExecutingAssembly().Location, "../osu!");
            var osuExe = Path.Combine(osuDir, "osu!.exe");

            if (!Directory.Exists(osuDir)) // TODO: check file hashes to force re-download if outdated
            {
                Directory.CreateDirectory(osuDir);
                await OsuApi.DownloadOsu(osuDir);
            }

            Console.WriteLine("Loading osu!.exe");

            // Add osu! directory to assembly search path
            AppDomain.CurrentDomain.AppendPrivatePath(osuDir);

            // Load the osu! executable and it's dependencies as assemblies without executing
            return Assembly.LoadFile(osuExe);
        });
    }
}