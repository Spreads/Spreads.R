using System;
using System.Runtime.InteropServices;
using UnixRStruct = Spreads.R.Internals.Unix.RStart;

namespace Spreads.R.Internals.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RStart
    {
        internal UnixRStruct Common;
        public IntPtr rhome;
        public IntPtr home;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah1 ReadConsole;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah2 WriteConsole;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah3 CallBack;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah4 ShowMessage;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah5 YesNoCancel;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah6 Busy;

        public UiMode CharacterMode;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public blah7 WriteConsoleEx;
    }
}