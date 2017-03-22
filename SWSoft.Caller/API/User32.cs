using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SWSoft.Api
{
    public class User32
    {

        /// <summary>
        /// 获得窗口句柄
        /// </summary>
        /// <param name="className">窗体类名</param>
        /// <param name="windowName">窗口标题</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// 总是让窗口置前
        /// SetWindowPos(hWnd, -1, 0, 0, 0, 0, 0x4000 | 0x0001 | 0x0002)
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        /// <summary>
        /// 获得指定窗口句柄的进程ID
        /// </summary>
        /// <param name="handel">窗口句柄</param>
        /// <param name="processId">存放进程ID的变量</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handel, ref int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //调用下一个钩子 
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, int msg, UIntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public static void PostMessage(IntPtr hWnd, Keys key)
        {
            PostMessage(hWnd, WM_KEYDOWN, (int)key, 0);
            PostMessage(hWnd, WM_KEYUP, (int)key, 0);
        }

        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint WM_SYSKEYDOWN = 0x0104;
        public const uint WM_SYSKEYUP = 0x0105;
    }
}
