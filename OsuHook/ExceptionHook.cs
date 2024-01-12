using System;
using System.Reflection;
using System.Threading;
using HarmonyLib;
using OsuHook.Osu;

namespace OsuHook
{
    internal class ExceptionHook
    {
        public static void Initialize()
        {
            try
            {
                var applicationType = AccessTools.TypeByName("System.Windows.Forms.Application");
                var threadExceptionEvent = applicationType.GetEvent("ThreadException");

                var newHandler = Delegate.CreateDelegate(
                    threadExceptionEvent.EventHandlerType,
                    typeof(ExceptionHook).GetMethod(nameof(HandleThreadException),
                        BindingFlags.Static | BindingFlags.NonPublic)!
                );

                threadExceptionEvent.AddEventHandler(null, newHandler);

                var unhandledExceptionModeType = AccessTools.TypeByName("System.Windows.Forms.UnhandledExceptionMode")!;
                var setUnhandledExceptionModeMethod = applicationType.GetMethod("SetUnhandledExceptionMode",
                    new[] { unhandledExceptionModeType, typeof(bool) })!;


                setUnhandledExceptionModeMethod.Invoke(null, new[]
                {
                    Enum.ToObject(unhandledExceptionModeType, /* UnhandledExceptionMode.CatchException */ 2),
                    true,
                });

                AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

                // GameBase:Scheduler
                var GameBaseMainScheduler = AccessTools
                    .Field("#=zpMBkSyYW0b9QAvkYpA==:#=zrzgKJpg=")
                    .GetValue(null)!;

                var voidDelegate = VoidDelegate.MakeInstance(() =>
                    AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException);

                // Scheduler:Add(VoidDelegate task, bool forceScheduled)
                var SchedulerAddMethod = GameBaseMainScheduler.GetType()
                    .GetMethod("#=zj4ZpgNs=", new[] { voidDelegate.GetType(), typeof(bool) })!;

                SchedulerAddMethod.Invoke(GameBaseMainScheduler, new[] { voidDelegate, false });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void HandleThreadException(object sender, ThreadExceptionEventArgs e) =>
            Console.WriteLine($"Uncaught exception: {e.Exception}");

        // ReSharper disable once UnusedParameter.Local
        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
            Console.WriteLine($"Uncaught exception: {e.ExceptionObject}");
    }
}