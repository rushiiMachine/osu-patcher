using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Osu.Stubs.Opcode;

public static class OsuAssembly
{
    private static readonly Module Module;

    /// <summary>
    ///     Cache the already-loaded osu!.exe .NET assembly to search in.
    /// </summary>
    static OsuAssembly()
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .SingleOrDefault(assembly => assembly.GetName().Name == "osu!");
        var module = assembly?.GetModules()?.SingleOrDefault();

        if (assembly == null || module == null)
            throw new Exception("Unable to find a loaded osu! assembly! Is this loaded into the correct process?");

        Assembly = assembly;
        Module = module;
    }

    internal static Assembly Assembly { [UsedImplicitly] get; private set; }

    /// <summary>
    ///     Retrieve all the types located in the osu! assembly.
    /// </summary>
    public static IEnumerable<Type> Types => Module.GetTypes();

    /// <summary>
    ///     Retrieve a single type from the osu! assembly.
    /// </summary>
    /// <param name="className">The qualified path to the target class to find.</param>
    /// <returns>The target Type otherwise an exception will be thrown if not found.</returns>
    public static Type GetType(string className) =>
        Module.GetType(className, ignoreCase: false, throwOnError: true);
}