using System;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using HoLLy.ManagedInjector;

namespace OsuInjector
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            try
            {
                var hookDllPath = Path.GetFullPath(typeof(Program).Assembly.Location + @"\..\osu!.hook.dll");

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

                Console.WriteLine("\nPress any key to continue...");
                Console.Write("\a"); // Bell sound
                Console.ReadKey();
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
                    var exe = (string)process["Name"];
                    var pid = (uint)process["ProcessId"];
                    var cli = (string)process["CommandLine"];

                    if (exe != "osu!.exe") continue;

                    // Make sure there's a -devserver xxxxx in cli args
                    if (!new Regex(@"osu!\.exe +-devserver \w").IsMatch(cli))
                        throw new Exception("Will not inject into osu! connected to Bancho!");

                    return pid;
                }
            }

            throw new Exception("Cannot find a running osu! process!");
        }
    }
}