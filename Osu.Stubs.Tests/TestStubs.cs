using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Tests;

[TestFixture]
[Parallelizable]
public class TestStubs
{
    [TestCaseSource(nameof(LoadStubs))]
    public void TestStub(ILazy<MemberInfo> lazy) =>
        Assert.DoesNotThrow(lazy.Fill);

    private static IEnumerable<ILazy<MemberInfo>> LoadStubs()
    {
        var osuDir = Path.Combine(Environment.CurrentDirectory, "osu!");
        var osuExe = Path.Combine(osuDir, "osu!.exe");

        if (!Directory.Exists(osuDir)) // TODO: check file hashes to force re-download
        {
            Directory.CreateDirectory(osuDir);
            OsuApi.DownloadOsu(osuDir).Wait();
        }

#pragma warning disable CS0618 // Type or member is obsolete
        // Add osu! directory to assembly search path
        AppDomain.CurrentDomain.AppendPrivatePath(osuDir);
#pragma warning restore CS0618 // Type or member is obsolete

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