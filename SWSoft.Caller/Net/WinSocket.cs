using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Reflection;
using System.Net;
using SWSoft.Reflector.Api;
using System.Diagnostics;

namespace SWSoft.Net
{
    public delegate SocketError WSAStartup([In] short wVersionRequested, out WSAData lpWSAData);
    public delegate SocketError WSARecv([In] IntPtr socketHandle, [In, Out] ref WSABuffer buffer, [In] int bufferCount, [Out] out int bytesTransferred, [In, Out] ref SocketFlags socketFlags, [In] IntPtr overlapped, [In] IntPtr completionRoutine);
    public delegate SocketError Bind([In] IntPtr socketHandle, [In] byte[] socketAddress, [In] int socketAddressSize);

    [Flags]
    public enum SocketConstructorFlags
    {
        WSA_FLAG_MULTIPOINT_C_LEAF = 4,
        WSA_FLAG_MULTIPOINT_C_ROOT = 2,
        WSA_FLAG_MULTIPOINT_D_LEAF = 0x10,
        WSA_FLAG_MULTIPOINT_D_ROOT = 8,
        WSA_FLAG_OVERLAPPED = 1
    }
    public class WinSocket
    {
        /// <summary>
        /// 将非托管函数指针转换为托管委托
        /// </summary>
        /// <typeparam name="T">托管的委托类型</typeparam>
        /// <param name="handle">进程句柄</param>
        /// <returns>托管的委托</returns>
        public Delegate GetMethod<T>(Process process)
        {
            return Marshal.GetDelegateForFunctionPointer((IntPtr)Kernel32.GetProcAddress(Kernel32.GetModuleHandleA("WS2_32"), typeof(T).Name), typeof(T));
        }
    }
}
