using System.Diagnostics;
using HoLLy.ManagedInjector;

namespace osu_patcher;

internal class Program
{
    public static void Main(string[] _)
    {
        Process.Start("C:\\osu!\\osu!.exe", "-devserver example.com");

        var osuProc = Process.GetProcessesByName("osu!").FirstOrDefault()
                      ?? throw new Exception("osu! is not running!");

        Thread.Sleep(500);

        var injectable = new InjectableProcess((uint)osuProc.Id);

        injectable.Inject(
            "C:\\Projects\\_personal\\osu-patcher\\osu-patcher-hook\\bin\\Debug\\osu_patcher_hook.dll",
            "osu_patcher_hook.EntryPoint",
            "Initialize");
    }
}