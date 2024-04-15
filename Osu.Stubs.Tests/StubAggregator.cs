using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Osu.Utils.Extensions;
using Osu.Utils.Lazy;

namespace Osu.Stubs.Tests;

internal abstract class StubAggregator
{
    /// <summary>
    ///     Collects all static <see cref="ILazy{T}" />s marked with <see cref="Stub" />
    ///     from all types within the <c>Osu.Stubs</c> assembly.
    /// </summary>
    public static IEnumerable<ILazy<MemberInfo>> CollectPlainStubs()
    {
        var types = Assembly.GetAssembly(typeof(Stub)).GetTypes();
        var stubs = types
            .SelectMany(CollectStubFields)
            .Where(field => field.IsStatic)
            .Select(field => field.GetValue<ILazy<MemberInfo>>(null))
            .ToArray();

        return stubs;
    }

    /// <summary>
    ///     Collects all instance <see cref="ILazy{T}" />s marked with <see cref="Stub" />
    ///     from all types within the <c>Osu.Stubs</c> assembly.
    ///     This requires that each class containing a <c>Stub</c> to have one public static field
    ///     that is marked with <see cref="StubInstance" />, providing an instance of the current stub class
    ///     that initializes all <see cref="ILazy{T}" />s to get the generic type definitions.
    /// </summary>
    public static IEnumerable<ILazy<MemberInfo>> CollectGenericStubs()
    {
        // Collect all types from the Osu.Stubs assembly
        var allTypes = Assembly.GetAssembly(typeof(Stub)).GetTypes();

        // Collect all static fields from all types marked with [StubInstance]
        var stubInstanceFields = allTypes
            .Select(type => type
                .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.GetCustomAttribute(typeof(StubInstance)) != null)
                .FirstOrNull())
            .WhereNotNull()
            .Where(field => field.FieldType == field.DeclaringType); // Make sure field references own class type

        // Get all stub instances from the instance fields
        var stubInstances = stubInstanceFields.Select(field => field.GetValue(null));

        // Map the stub instance to all the collected stub fields
        var stubInstanceStubFields = stubInstances.Select(stubInstance => new
        {
            stubInstance,
            stubFields = CollectStubFields(stubInstance.GetType())
                .Where(field => !field.IsStatic),
        });

        // Collect and flatmap the ILazy values of every [Stub] field via the stub instance
        return stubInstanceStubFields.SelectMany(stubs =>
            stubs.stubFields.Select(field =>
                field.GetValue<ILazy<MemberInfo>>(stubs.stubInstance)));
    }

    /// <summary>
    ///     Collects all public static fields with the type <see cref="ILazy{T}" />,
    ///     marked with <see cref="Stub" /> from a specific <see cref="Type" />.
    /// </summary>
    private static IEnumerable<FieldInfo> CollectStubFields(Type type) => type
        .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
        .Where(field => field.GetCustomAttribute(typeof(Stub)) != null);
}