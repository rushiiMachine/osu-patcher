using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Osu.Stubs.Opcode;

internal static class OsuAssembly
{
    private static readonly Module Module;

    /// <summary>
    ///     Cache the already-loaded osu!.exe .NET assembly to search in.
    /// </summary>
    static OsuAssembly()
    {
        var osuAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .SingleOrDefault(assembly => assembly.GetName().Name == "osu!");

        Module = osuAssembly?.GetModules().SingleOrDefault()
                 ?? throw new Exception("Unable to find a loaded osu! assembly! " +
                                        "Is this loaded into the correct process?");
    }

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