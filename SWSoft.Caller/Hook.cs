using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SWSoft.Framework
{
    /// <summary>
    /// 键盘钩子
    /// </summary>
    public class KeyboardHookEventArgs : EventArgs
    {
        public bool Handled;
        public int nCode;
        public int wParam;
        public int lParam;

        public KeyboardHookEventArgs()
        {
            Handled = false;
            nCode = 0;
            wParam = 0;
            lParam = 0;
        }
    }

    #region 鼠标信息类
    /// <summary>
    /// The POINT structure defines the x- and y- coordinates of a point. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        /// <summary>
        /// X 坐标位置. 
        /// </summary>
        public int x;
        /// <summary>
        /// Y 坐标位置.
        /// </summary>
        public int y;
    }

    /// <summary>
    /// The MOUSEHOOKSTRUCT structure contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        /// <summary>
        /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public POINT pt;
        /// <summary>
        /// Handle to the window that will receive the mouse message corresponding to the mouse event. 
        /// </summary>
        public int hwnd;
        /// <summary>
        /// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message. 
        /// </summary>
        public int wHitTestCode;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int dwExtraInfo;
    }

    /// <summary>
    /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class MouseLLHookStruct
    {
        /// <summary>
        /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        public POINT pt;
        /// <summary>
        /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
        /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
        /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
        /// One wheel click is defined as WHEEL_DELTA, which is 120. 
        ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
        /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
        /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used. 
        ///XBUTTON1
        ///The first X button was pressed or released.
        ///XBUTTON2
        ///The second X button was pressed or released.
        /// </summary>
        public int mouseData;
        /// <summary>
        /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose 
        ///LLMHF_INJECTED Test the event-injected flag.  
        ///0
        ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
        ///1-15
        ///Reserved.
        /// </summary>
        public int flags;
        /// <summary>
        /// Specifies the time stamp for this message.
        /// </summary>
        public int time;
        /// <summary>
        /// Specifies extra information associated with the message. 
        /// </summary>
        public int dwExtraInfo;
    }
    #endregion
    public class KeyboardHook
    {
        [DllImport("kernel32")]
        private static extern int GetCurrentThreadId();

        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(HookType idHook, HookProc lpfn, int hmod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);
        [DllImport("user32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);

        /// <summary>
        /// 钩子类型
        /// </summary>
        public enum HookType
        {
            /// <summary>
            /// 键盘
            /// </summary>
            WH_KEYBOARD = 2,
            /// <summary>
            /// 鼠标
            /// </summary>
            WH_MOUSE = 7,
        }
        /// <summary>
        /// 鼠标移动
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;
        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        private const int WM_RBUTTONDOWN = 0x204;
        /// <summary>
        /// 鼠标左键双击
        /// </summary>
        private const int WM_LBUTTONDBLCLK = 0x203;
        /// <summary>
        /// 鼠标右键双击
        /// </summary>
        private const int WM_RBUTTONDBLCLK = 0x206;
        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        private const int WM_MOUSEWHEEL = 0x020A;

        private int fKeyboardHook;
        private delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        private HookProc fKeyboardHookProcedure;

        public delegate void KeyPressEventHandler(object sender, KeyboardHookEventArgs e);
        public event KeyPressEventHandler KeyPress;


        private int fMouseHook;
        private HookProc fMouseHookProcedure;
        public event MouseEventHandler OnMouseActivity;


        public KeyboardHook()
        {
            fKeyboardHook = 0;
            fMouseHook = 0;
        }

        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        /// <returns>是否成功</returns>
        public bool InstallHook()
        {
            if (fMouseHook == 0)
            {
                fMouseHookProcedure = new HookProc(MouseHookProc);
                fMouseHook = SetWindowsHookEx(HookType.WH_MOUSE, fMouseHookProcedure, 0, GetCurrentThreadId());
            }
            if (fMouseHook == 0)
            {
                return false;
            }
            if (fKeyboardHook == 0)
            {
                fKeyboardHookProcedure = new HookProc(KeyboardHookProc);
                fKeyboardHook = SetWindowsHookEx(HookType.WH_KEYBOARD, fKeyboardHookProcedure, 0, GetCurrentThreadId());
            }
            if (fKeyboardHook == 0)
            {
                return false;
            }
            return true;
        }

        public bool UnInstallHook()
        {
            bool result = true;
            if (fKeyboardHook != 0)
            {
                result = (UnhookWindowsHookEx(fMouseHook));
                fMouseHook = 0;
                result = (UnhookWindowsHookEx(fKeyboardHook));
                fKeyboardHook = 0;
            }
            return result;
        }

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            bool handled = false;
            if ((nCode >= 0) && (KeyPress != null))
            {
                KeyboardHookEventArgs e = new KeyboardHookEventArgs();
                e.nCode = nCode;
                e.wParam = wParam;
                e.lParam = lParam.ToInt32();
                KeyPress(this, e);
                handled = e.Handled;
            }
            //if (handled)
            //    return 1;
            //return 0;
            //call next hook
            if (handled)
                return 1;
            return CallNextHookEx(fKeyboardHook, nCode, wParam, lParam);
        }


        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            // if ok and someone listens to our events
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                //Marshall the data from callback.
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                //detect button clicked
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        break;
                    case WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        break;
                    case WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                        break;
                }

                //double clicks
                int clickCount = 0;
                if (button != MouseButtons.None)
                    if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK) clickCount = 2;
                    else clickCount = 1;

                //generate event 
                MouseEventArgs e = new MouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.pt.x,
                                                   mouseHookStruct.pt.y,
                                                   mouseDelta);
                //raise it
                OnMouseActivity(this, e);
            }
            //call next hook
            return CallNextHookEx(fMouseHook, nCode, wParam, lParam);
        }
    }



}
