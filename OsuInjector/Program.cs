using System;
using System.IO;
using System.Management;
using HoLLy.ManagedInjector;

namespace OsuInjector
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            try
            {
                var hookDllPath = Path.GetFullPath(typeof(Program).Assembly.Location + "\\..\\osu!.hook.dll");
                Console.WriteLine(hookDllPath);

                if (!File.Exists(hookDllPath))
                    throw new Exception("Unable to find osu!.hook.dll in the current directory!");

                using (var proc = new InjectableProcess(GetOsuPid()))
                {
                    proc.Inject(hookDllPath, "OsuHook.MainHook", "Initialize");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Console.Read();
            }
        }

        /// <summary>
        ///     Find a <c>osu!.exe</c> process that has a devserver in the cli arguments.
        ///     (Not connected to osu!bancho)
        /// </summary>
        /// <returns>The process id of the first matching process.</returns>
        /// <exception cref="Exception">If found invalid osu! process or no process at all.</exception>
        private static uint GetOsuPid()
        {
            using (var mgmt = new ManagementClass("Win32_Process"))
            using (var processes = mgmt.GetInstances())
            {
                foreach (var process in processes)
                {
                    var exeName = (string)process["Name"];

                    if (exeName == "osu!.exe")
                    {
                        var commandLine = (string)process["CommandLine"];
                        var pid = (uint)process["ProcessId"];

                        if (!commandLine.Contains(" -devserver "))
                            throw new Exception("Will not inject into osu! connected to Bancho!");

                        return pid;
                    }
                }
            }

            throw new Exception("Cannot find a running osu! process!");
        }
    }
}