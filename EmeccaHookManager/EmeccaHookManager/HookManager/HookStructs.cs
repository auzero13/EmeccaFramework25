using System;
using System.Runtime.InteropServices;

namespace Emecca.Medical.HookManager
{
    internal class HookStructs
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSELLHookStruct
        {
            public Point Point;
            public int MouseData;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBOARDLLHookStruct
        {
            public int VirtualKeyCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }
    }
}
