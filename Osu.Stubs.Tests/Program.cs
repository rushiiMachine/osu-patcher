using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Osu.Stubs.Tests;

internal static class Program
{
    private static async Task Main()
    {
        var osuDir = Path.Combine(Environment.CurrentDirectory, "osu!");
        var osuExe = Path.Combine(osuDir, "osu!.exe");

        if (!Directory.Exists(osuDir))
        {
            Directory.CreateDirectory(osuDir);
            await OsuApi.DownloadOsu(osuDir);
        }

        AppDomain.CurrentDomain.AppendPrivatePath(osuDir);
        Assembly.LoadFile(osuExe);

        foreach (var type in Assembly.GetAssembly(typeof(Stub)).GetTypes())
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetCustomAttribute(typeof(Stub)) == null)
                    continue;

                var lazy = field.GetValue<ILazy<MemberInfo>>(null);

                try
                {
                    Console.WriteLine($"{lazy.Name} -> {lazy.Reference}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{lazy.Name} -> [Failure]");
                    Console.WriteLine(e);
                }
            }
        }
    }
}