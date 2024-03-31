using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Osu.Stubs.Tests;

[TestFixture]
[Parallelizable]
public class TestStubs
{
    [TestCaseSource(nameof(LoadStubs))]
    public void TestStub(ILazy<MemberInfo> lazy) => Assert.DoesNotThrow(
        () =>
        {
            try
            {
                lazy.Fill();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is ReflectionTypeLoadException typeLoadException)
                {
                    throw new AggregateException(typeLoadException.LoaderExceptions);
                }

                throw e.InnerException!;
            }
        }
    );

    [Test(Description = "Tests cannot be run in 64bit mode!", ExpectedResult = true)]
    public static bool CheckIs32Bit() => !Environment.Is64BitProcess;

    private static IEnumerable<ILazy<MemberInfo>> LoadStubs()
    {
        if (!CheckIs32Bit()) return [];

        var osuDir = Path.Combine(Assembly.GetExecutingAssembly().Location, "../osu!");
        var osuExe = Path.Combine(osuDir, "osu!.exe");

        if (!Directory.Exists(osuDir)) // TODO: check file hashes to force re-download
        {
            Directory.CreateDirectory(osuDir);
            OsuApi.DownloadOsu(osuDir).Wait();
        }

        // Add osu! directory to assembly search path
        AppDomain.CurrentDomain.AppendPrivatePath(osuDir);

        // Load the osu! executable and it's dependencies as assemblies without executing
        Assembly.LoadFile(osuExe);

        var stubs = new List<ILazy<MemberInfo>>(300);

        foreach (var type in Assembly.GetAssembly(typeof(Stub)).GetTypes())
        {
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetCustomAttribute(typeof(Stub)) == null)
                    continue;

                var stub = field.GetValue<ILazy<MemberInfo>>(null);
                if (stub == null) throw new Exception();

                stubs.Add(stub);
            }
        }

        stubs.Sort((a, b) =>
            string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

        return stubs;
    }
}