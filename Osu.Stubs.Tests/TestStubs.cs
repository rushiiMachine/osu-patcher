using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Tests;

[TestFixture]
[Parallelizable]
public class TestStubs
{
    [TestCaseSource(nameof(CreateStubTests))]
    public void TestStub(ILazy<MemberInfo> lazy) => Assert.DoesNotThrow(
        () =>
        {
            try
            {
                lazy.Fill();

                if (lazy.Reference is Type type)
                {
                    TestContext.WriteLine(type.FullName);
                }
                else
                {
                    TestContext.Write(lazy.Reference.DeclaringType!.FullName);
                    TestContext.Write(" :: ");
                    TestContext.WriteLine(lazy.Reference.ToString());
                }
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

    private static IEnumerable<ILazy<MemberInfo>> CreateStubTests()
    {
        if (!CheckIs32Bit()) return [];

        OsuLoader.UpdateAndLoad().Wait();

        var stubs = StubAggregator.CollectPlainStubs()
            .Concat(StubAggregator.CollectGenericStubs())
            .ToArray();

        Array.Sort(stubs, (a, b) =>
            string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

        return stubs;
    }
}