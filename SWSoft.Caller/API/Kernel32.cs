using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SWSoft.Reflector.Api
{
    public class Kernel32
    {
        #region Fields

        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_RESERVE = 0x2000;
        public const uint MEM_RELEASE = 0x8000;
        public const uint MEM_DECOMMIT = 0x4000;
        public const int Process_ALL_ACCESS = 0x1F0FFF;
        public const int Process_CREATE_THREAD = 0x2;
        public const int Process_VM_OPERATION = 0x8;
        public const int Process_VM_WRITE = 0x20;

        #endregion

        #region Propertys

        public Process Process { get; set; }
        protected IntPtr Handle { get { return Process.Handle; } }

        #endregion

        #region API

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcessHeap();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FormatMessage(int flags, IntPtr source, int messageId, int languageId, StringBuilder buffer, int size, IntPtr arguments);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);

        /// <summary>
        /// 读取内存字节集数据
        /// </summary>
        /// <param name="handle">进程句柄</param>
        /// <param name="address">内存地址</param>
        /// <param name="data">数据存储变量</param>
        /// <param name="size">长度</param>
        /// <param name="read">读取长度</param>
        [DllImport("Kernel32.dll")]
        private static extern void ReadProcessMemory(IntPtr handle, uint address, [Out] byte[] data, int size, int read);

        [DllImport("Kernel32.dll")]
        protected static extern bool WriteProcessMemory(IntPtr handle, uint address, byte[] lpBuffer, int size, int write);

        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();

        /// <summary>
        /// 打开一个已存在的进程对象
        /// </summary>
        /// <param name="dwDesiredAccess">进程访问权限</param>
        /// <param name="bInheritHandle">是否可以继承</param>
        /// <param name="dwProcessId">进程Id</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// 向指定进程申请虚拟空间,并返回开始地址
        /// </summary>
        /// <param name="hProcess">进程句柄</param>
        /// <param name="lpAddress">要申请的地址,要自动分配可传0</param>
        /// <param name="size">申请空间大小</param>
        /// <param name="flAllocationType">分配类型</param>
        /// <param name="flProtect">读取权限</param>
        /// <returns>空间开始地址</returns>
        [DllImport("Kernel32.dll")]
        public static extern uint VirtualAllocEx(IntPtr handle, uint lpAddress, int size, uint flAllocationType, uint flProtect);

        /// <summary>
        /// 取消或者释放调用进程的虚拟地址空间页的一个区域
        /// </summary>
        /// <param name="handel">区域地址</param>
        /// <param name="address">区域大小</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr handle, uint lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr handle, uint attrib, uint size, uint address, uint par, uint flags, uint threadid);

        [DllImport("Kernel32.dll")]
        public static extern uint ResumeThread(IntPtr thHandle);

        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WaitForSingleObject(IntPtr handle, uint dwMilliseconds);


        [DllImport("Kernel32.dll", EntryPoint = "GetTickCount", CharSet = CharSet.Auto)]
        public static extern int GetTickCount();

        [DllImport("kernel32.dll")]
        public static extern uint GetProcAddress(IntPtr handle, string lpname);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetModuleHandleA(string name);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string filePath);

        [DllImport("Kernel32.dll")]
        public static extern bool CreateProcess(String imageName,
                                                String cmdLine,
                                                IntPtr lpProcessAttributes,
                                                IntPtr lpThreadAttributes,
                                                bool boolInheritHandles,
                                                CreationFlags dwCreationFlags,
                                                IntPtr lpEnvironment,
                                                String lpszCurrentDir,
                                                ref StartupInfo si,
                                                out ProcessInformation pi);
        #endregion

        [Flags]
        public enum CreationFlags : int
        {
            NONE = 0,
            DEBUG_PROCESS = 0x00000001,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            CREATE_SUSPENDED = 0x00000004,
            DETACHED_PROCESS = 0x00000008,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NO_WINDOW = 0x08000000,
        }

        [Flags]
        public enum STARTF : uint
        {
            STARTF_USESHOWWINDOW = 0x00000001,
            STARTF_USESIZE = 0x00000002,
            STARTF_USEPOSITION = 0x00000004,
            STARTF_USECOUNTCHARS = 0x00000008,
            STARTF_USEFILLATTRIBUTE = 0x00000010,
            STARTF_RUNFULLSCREEN = 0x00000020,  // ignored for non-x86 platforms
            STARTF_FORCEONFEEDBACK = 0x00000040,
            STARTF_FORCEOFFFEEDBACK = 0x00000080,
            STARTF_USESTDHANDLES = 0x00000100,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StartupInfo
        {
            public Int32 cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public STARTF dwFlags;
            public ShowWindow wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ProcessInformation
        {
            public IntPtr ProcessHandle;
            public IntPtr ThreadHandle;
            public uint ProcessId;
            public uint ThreadId;
        }

        public enum ShowWindow : short
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        #region NET Override

        protected byte ReadByte(uint address, int move = 0)
        {
            return ReadBytes(address, 1, move)[0];
        }

        protected byte[] ReadBytes(uint address, int size, int move = 0)
        {
            byte[] buff = new byte[size];
            if (move != 0)
            {
                byte[] temp = new byte[4];
                ReadProcessMemory(Handle, address, temp, 4, 0);
                address = BitConverter.ToUInt32(temp, 0) + (uint)move;
            }
            ReadProcessMemory(Handle, address, buff, size, 0);
            return buff;
        }

        protected short ReadShort(uint address, int move = 0)
        {
            return BitConverter.ToInt16(ReadBytes(address, 2, move), 0);
        }

        /// <summary>
        /// 读取32位整数,如果有偏移则读取的是偏移后的值
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="move">偏移量</param>
        protected uint ReadUInt(uint address, int move = 0)
        {
            return BitConverter.ToUInt32(ReadBytes(address, 4, move), 0);
        }

        protected float ReadFloat(uint address, int move = 0)
        {
            return BitConverter.ToSingle(ReadBytes(address, sizeof(float),move), 0);
        }

        protected double ReadDouble(uint address, int move = 0)
        {
            return BitConverter.ToDouble(ReadBytes(address, sizeof(double), move), 0);
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <param name="move"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        protected string ReadString(uint address, int size, int move = 0, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            if (move != 0)
            {
                address = (uint)(ReadUInt(address) + move);
            }
            return encoding.GetString(ReadBytes(address, size));
        }

        protected bool WriteByte(uint address, byte buff, int move = 0)
        {
            return WriteBytes(address, new byte[] { buff }, move);
        }

        protected bool WriteBytes(uint address, byte[] buffer, int move = 0)
        {
            if (move != 0)
            {
                address = (uint)(ReadUInt(address) + move);
            }
            try
            {
                return WriteProcessMemory(Handle, address, buffer, buffer.Length, 0);
            }
            catch (AccessViolationException)
            {
                return true;
            }
        }

        protected bool Call(uint address, uint paramAdd = 0x0)
        {
            return WaitForSingleObject(CreateRemoteThread(address, paramAdd), 10);
        }

        protected bool Call(IntPtr handel, uint address, uint paramAdd = 0x0)
        {
            return WaitForSingleObject(CreateRemoteThread(handel, address, paramAdd), 10);
        }

        protected bool HookDll(string path)
        {
            IntPtr handel = OpenProcess(Process_ALL_ACCESS, true, Process.Id);
            uint load = GetProcAddress(GetModuleHandleA("Kernel32"), "LoadLibraryA");
            uint paramAdd = VirtualAllocEx(handel, path.Length + 1);
            WriteBytes(paramAdd, Encoding.Default.GetBytes(path));
            return Call(handel, load, paramAdd);
        }

        protected uint VirtualAllocEx(IntPtr handel, int size = 0x1000)
        {
            return VirtualAllocEx(handel, 0, size, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        }


        protected IntPtr CreateRemoteThread(uint address, uint paramAdd = 0)
        {
            return CreateRemoteThread(Handle, 0, 0, address, paramAdd, 0, 0);
        }

        protected IntPtr CreateRemoteThread(IntPtr handel, uint address, uint paramAdd = 0)
        {
            return CreateRemoteThread(handel, 0, 0, address, paramAdd, 0, 0);
        }

        protected bool CreateProcess(string filename)
        {
            StartupInfo si = new StartupInfo();
            ProcessInformation pi;
            return CreateProcess(null, filename, IntPtr.Zero, IntPtr.Zero, true, CreationFlags.NONE, IntPtr.Zero, "", ref si, out pi);
        }

        #endregion
    }
}