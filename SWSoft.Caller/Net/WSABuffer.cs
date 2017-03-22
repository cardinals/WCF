namespace System.Net
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct WSABuffer
    {
        internal int Length;
        internal IntPtr Pointer;
    }
}

