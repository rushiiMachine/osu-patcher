using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HoLLy.ManagedInjector;

namespace OsuInjector
{
    internal class Program
    {
        public static void Main(string[] _)
        {
            try
            {
                var hookDllPath = Path.Combine(typeof(Program).Assembly.Location, "..\\osu!.hook.dll");

                if (!File.Exists(hookDllPath))
                    throw new Exception("Unable to find osu!.hook.dll in the current directory!");

                Process.Start("C:\\osu!\\osu!.exe", "-devserver example.com");

                var osuProc = Process.GetProcessesByName("osu!").FirstOrDefault()
                              ?? throw new Exception("osu! is not running!");

                Thread.Sleep(1000);

                var injectable = new InjectableProcess((uint)osuProc.Id);
                injectable.Inject(hookDllPath, "OsuHook.MainHook", "Initialize");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Console.Read();
            }
        }
    }
}