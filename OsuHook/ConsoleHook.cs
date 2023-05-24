using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace OsuHook
{
    /// <summary>
    ///     A utility for immediately showing a console window with AllocConsole() upon something being written to
    ///     stdout/stderr and redirecting any further output to that window.
    /// </summary>
    internal static class ConsoleHook
    {
        private static volatile bool _consoleInitialized;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern int AllocConsole();

        public static void Initialize()
        {
            if (_consoleInitialized) return;

            Console.SetOut(new InterceptingWriter(false));
            Console.SetError(new InterceptingWriter(true));
        }

        private class InterceptingWriter : TextWriter
        {
            private const int StdOutputHandle = -11, StdErrorHandle = -12;

            private readonly bool _isStdErr;

            internal InterceptingWriter(bool isStdErr)
            {
                _isStdErr = isStdErr;
            }

            public override Encoding Encoding => Encoding.UTF8;

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(char value)
            {
                // Console window already present, redirect to new stdout/stderr
                if (_consoleInitialized)
                {
                    if (_isStdErr)
                        Console.Error.Write(value);
                    else
                        Console.Write(value);

                    return;
                }

                _consoleInitialized = true;

                AllocConsole();

                // Redirect Console to the new stdout/stderr
                var stdOutHandle = new SafeFileHandle(GetStdHandle(StdOutputHandle), true);
                var stdErrHandle = new SafeFileHandle(GetStdHandle(StdErrorHandle), true);

                var stdOut = new StreamWriter(new FileStream(stdOutHandle, FileAccess.Write));
                var stdErr = new StreamWriter(new FileStream(stdErrHandle, FileAccess.Write));
                stdOut.AutoFlush = true;
                stdErr.AutoFlush = true;

                Console.SetOut(stdOut);
                Console.SetError(stdErr);

                // Write the original value to the new out
                if (!_isStdErr)
                    stdOut.Write(value);
                else
                    stdErr.Write(value);
            }
        }
    }
}