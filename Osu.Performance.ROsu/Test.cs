using System;
using System.Runtime.InteropServices;

namespace Osu.Performance.ROsu
{
    public class Test
    {
        [DllImport("rosu.dll")]
        public static extern int test();
    }
}