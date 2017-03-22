using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using SWSoft.Reflector.Api;

namespace SWSoft.Net
{
    public partial class Connection
    {
        internal const int TCP_STATE_CLOSED = 1;
        internal const int TCP_STATE_LISTEN = 2;
        internal const int TCP_STATE_SYN_SENT = 3;
        internal const int TCP_STATE_SYN_RCVD = 4;
        internal const int TCP_STATE_ESTAB = 5;
        internal const int TCP_STATE_FIN_WAIT1 = 6;
        internal const int TCP_STATE_FIN_WAIT2 = 7;
        internal const int TCP_STATE_CLOSE_WAIT = 8;
        internal const int TCP_STATE_CLOSING = 9;
        internal const int TCP_STATE_LAST_ACK = 10;
        internal const int TCP_STATE_TIME_WAIT = 11;
        internal const int TCP_STATE_DELETE_TCB = 12;

        internal const byte NO_ERROR = 0;
        internal const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        internal const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        internal const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        internal int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal extern static int GetUdpStatistics(ref MIB_UDPSTATS pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetUdpTable(byte[] UcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetExtendedUdpTable(byte[] pUdpTable, out int dwOutBufLen, bool sort, int ipVersion, UDP_TABLE_CLASS tblClass, int reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal extern static int AllocateAndGetUdpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal extern static int GetTcpStatistics(ref MIB_TCPSTATS pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetExtendedTcpTable(byte[] pTcpTable, out int dwOutBufLen, bool sort,
            int ipVersion, TCP_TABLE_CLASS tblClass, int reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public extern static int AllocateAndGetTcpExTableFromStack(ref IntPtr pTable, bool bOrder, IntPtr heap, int zero, int flags);

        internal static string GetAPIErrorMessageDescription(int ApiErrNumber)
        {
            StringBuilder sError = new StringBuilder(512);
            int lErrorMessageLength = Kernel32.FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, (IntPtr)0, ApiErrNumber, 0, sError, sError.Capacity, (IntPtr)0);

            if (lErrorMessageLength > 0)
            {
                string strgError = sError.ToString();
                strgError = strgError.Substring(0, strgError.Length - 2);
                return strgError + " (" + ApiErrNumber.ToString() + ")";
            }

            return "none";
        }

        #region StructTCP

        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_TCPSTATS
        {
            internal int dwRtoAlgorithm;
            internal int dwRtoMin;
            internal int dwRtoMax;
            internal int dwMaxConn;
            internal int dwActiveOpens;
            internal int dwPassiveOpens;
            internal int dwAttemptFails;
            internal int dwEstabResets;
            internal int dwCurrEstab;
            internal int dwInSegs;
            internal int dwOutSegs;
            internal int dwRetransSegs;
            internal int dwInErrs;
            internal int dwOutRsts;
            internal int dwNumConns;
        }

        internal struct MIB_TCPTABLE
        {
            internal int dwNumEntries;
            internal MIB_TCPROW[] table;
        }

        /// <summary>
        /// TCP连接状态
        /// </summary>
        internal struct MIB_TCPROW
        {
            /// <summary>
            /// 状态名称
            /// </summary>
            internal string StrgState;
            /// <summary>
            /// 状态值
            /// </summary>
            internal int iState;
            /// <summary>
            /// 本地IP地址和端口
            /// </summary>
            internal IPEndPoint Local;
            /// <summary>
            /// 远程IP地址和端口
            /// </summary>
            internal IPEndPoint Remote;
        }

        internal struct MIB_EXTCPTABLE
        {
            internal int dwNumEntries;
            internal MIB_EXTCPROW[] table;
        }

        /// <summary>
        /// 进程TCP连接状态
        /// </summary>
        internal struct MIB_EXTCPROW
        {
            /// <summary>
            /// 状态名称
            /// </summary>
            internal string StrgState;
            /// <summary>
            /// 状态值
            /// </summary>
            internal int iState;
            /// <summary>
            /// 本地IP地址和端口
            /// </summary>
            internal IPEndPoint Local;
            /// <summary>
            /// 远程IP地址和端口
            /// </summary>
            internal IPEndPoint Remote;
            /// <summary>
            /// 进程ID
            /// </summary>
            internal int dwProcessId;
            /// <summary>
            /// 进程名称
            /// </summary>
            internal string ProcessName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_TCPTABLE_OWNER_MODULE
        {
            /// <summary>
            /// 模块数
            /// </summary>
            internal uint dwNumEntries;
            /// <summary>
            /// 模块列表
            /// </summary>
            internal MIB_TCPROW_OWNER_MODULE[] table;
        }

        /// <summary>
        /// TCP连接所属模块
        /// </summary>
        internal struct MIB_TCPROW_OWNER_MODULE
        {
            internal const int TCPIP_OWNING_MODULE_SIZE = 16;
            /// <summary>
            /// 状态
            /// </summary>
            internal uint dwState;
            /// <summary>
            /// 本地IP和端口
            /// </summary>
            internal IPEndPoint Local;
            /// <summary>
            /// 远程IP和端口
            /// </summary>
            internal IPEndPoint Remote;
            /// <summary>
            /// 所属进程标识
            /// </summary>
            internal uint dwOwningPid;
            /// <summary>
            /// 创建时间
            /// </summary>
            internal uint liCreateTimestamp;
            /// <summary>
            /// 模块信息
            /// </summary>
            internal ulong[] OwningModuleInfo;
        }

        internal struct MIB_TCPTABLE_OWNER_PID
        {
            internal int dwNumEntries;
            internal MIB_TCPROW_OWNER_PID[] table;
        }

        internal struct MIB_TCPROW_OWNER_PID
        {
            internal int dwState;
            internal IPEndPoint Local;
            internal IPEndPoint Remote;
            internal int dwOwningPid;
            internal string State;
            internal string ProcessName;
        }

        internal enum TCP_TABLE_CLASS
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL,
        }

        #endregion

        #region StructUDP


        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_UDPSTATS
        {
            internal int dwInDatagrams;
            internal int dwNoPorts;
            internal int dwInErrors;
            internal int dwOutDatagrams;
            internal int dwNumAddrs;
        }

        internal struct MIB_UDPROW
        {
            internal IPEndPoint Local;
        }

        internal struct MIB_EXUDPROW
        {
            /// <summary>
            /// 本地 IP 地址和端口号
            /// </summary>
            internal IPEndPoint Local;
            /// <summary>
            /// 进程标识
            /// </summary>
            internal int dwProcessId;
            /// <summary>
            /// 进程名称
            /// </summary>
            internal string ProcessName;
        }

        internal struct MIB_UDPTABLE
        {
            internal int dwNumEntries;
            internal MIB_UDPROW[] table;
        }

        internal struct MIB_EXUDPTABLE
        {
            internal int dwNumEntries;
            internal MIB_EXUDPROW[] table;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_UDPROW_OWNER_PID
        {
            internal IPEndPoint Local;
            internal int dwOwningPid;
            internal string ProcessName;
        }

        internal struct MIB_UDPROW_OWNER_MODULE
        {
            internal IPEndPoint Local;
            internal uint dwOwningPid;
            internal long liCreateTimestamp;
            internal ulong[] OwningModuleInfo;
        }

        internal struct MIB_UDPTABLE_OWNER_PID
        {
            internal int dwNumEntries;
            internal MIB_UDPROW_OWNER_PID[] table;
        }

        internal struct _MIB_UDPTABLE_OWNER_MODULE
        {
            internal uint dwNumEntries;
            internal MIB_UDPROW_OWNER_MODULE[] table;
        }

        internal enum UDP_TABLE_CLASS
        {
            UDP_TABLE_BASIC,
            UDP_TABLE_OWNER_PID,
            UDP_TABLE_OWNER_MODULE
        }

        #endregion

        #region ApiMethod

        internal MIB_TCPSTATS GetTcpStats()
        {
            MIB_TCPSTATS tcpStats = new MIB_TCPSTATS();
            GetTcpStatistics(ref tcpStats);
            return tcpStats;
        }

        internal void GetUdpStats()
        {
            MIB_UDPSTATS udpStats = new MIB_UDPSTATS();
            GetUdpStatistics(ref udpStats);
        }

        internal MIB_EXTCPTABLE GetExTcpTable()
        {
            int rowSize = 24;
            int bufferSize = 100000;
            IntPtr dummyMem = Marshal.AllocHGlobal(bufferSize);
            int res = AllocateAndGetTcpExTableFromStack(ref dummyMem, true, Kernel32.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                throw new InvalidOperationException("Error get TcpExTable from stack");
            }
            int entries = (int)Marshal.ReadIntPtr(dummyMem);
            dummyMem = IntPtr.Zero;
            Marshal.FreeHGlobal(dummyMem);

            bufferSize = entries * rowSize + 4;
            MIB_EXTCPTABLE tcpExTable = new MIB_EXTCPTABLE();
            IntPtr lpTable = Marshal.AllocHGlobal(bufferSize);
            res = AllocateAndGetTcpExTableFromStack(ref lpTable, true, Kernel32.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                throw new InvalidOperationException("Error get TcpExTable from stack");
            }
            IntPtr current = lpTable;
            int currentIndex = 0;
            entries = (int)Marshal.ReadIntPtr(current);
            tcpExTable.table = new MIB_EXTCPROW[entries];
            currentIndex += 4;
            current = (IntPtr)((int)current + currentIndex);
            for (int i = 0; i < entries; i++)
            {
                tcpExTable.table[i].StrgState = ConvertState((int)Marshal.ReadIntPtr(current));
                tcpExTable.table[i].iState = (int)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                tcpExTable.table[i].Local = new IPEndPoint(localAddr, (int)ConvertPort(localPort));
                UInt32 remoteAddr = (UInt32)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                UInt32 remotePort = 0;
                if (remoteAddr != 0)
                {
                    remotePort = (UInt32)Marshal.ReadIntPtr(current);
                    remotePort = ConvertPort(remotePort);
                }
                tcpExTable.table[i].Remote = new IPEndPoint(remoteAddr, (int)remotePort);
                current = (IntPtr)((int)current + 4);
                tcpExTable.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
                tcpExTable.table[i].ProcessName = GetProcessName(tcpExTable.table[i].dwProcessId);
                current = (IntPtr)((int)current + 4);
            }

            Marshal.FreeHGlobal(lpTable);
            current = IntPtr.Zero;
            return tcpExTable;
        }

        internal void GetExUdpTable()
        {
            int rowSize = 12;
            int bufferSize = 100000;
            IntPtr lpTable = Marshal.AllocHGlobal(bufferSize);
            int res = AllocateAndGetUdpExTableFromStack(ref lpTable, true, Kernel32.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                throw new InvalidOperationException("Error get udp table");
            }
            int entries = (int)Marshal.ReadIntPtr(lpTable);
            lpTable = IntPtr.Zero;
            Marshal.FreeHGlobal(lpTable);

            bufferSize = entries * rowSize + 4;
            MIB_EXUDPTABLE udpExTable = new MIB_EXUDPTABLE();
            lpTable = Marshal.AllocHGlobal(bufferSize);
            res = AllocateAndGetUdpExTableFromStack(ref lpTable, true, Kernel32.GetProcessHeap(), 0, 2);
            if (res != NO_ERROR)
            {
                throw new InvalidOperationException("Error get udp table");
            }
            IntPtr current = lpTable;
            int CurrentIndex = 0;
            entries = (int)Marshal.ReadIntPtr(current);
            udpExTable.dwNumEntries = entries;
            udpExTable.table = new MIB_EXUDPROW[entries];
            CurrentIndex += 4;
            current = (IntPtr)((int)current + CurrentIndex);
            for (int i = 0; i < entries; i++)
            {
                UInt32 localAddr = (UInt32)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                UInt32 localPort = (UInt32)Marshal.ReadIntPtr(current);
                current = (IntPtr)((int)current + 4);
                udpExTable.table[i].Local = new IPEndPoint(localAddr, ConvertPort(localPort));
                udpExTable.table[i].dwProcessId = (int)Marshal.ReadIntPtr(current);
                udpExTable.table[i].ProcessName = GetProcessName(udpExTable.table[i].dwProcessId);
                current = (IntPtr)((int)current + 4);
            }
            Marshal.FreeHGlobal(lpTable);
            current = IntPtr.Zero;
        }

        internal void GetTcpTable()
        {
            byte[] buffer = new byte[20000];
            int pdwSize = 20000;
            int res = GetTcpTable(buffer, out pdwSize, true);
            if (res != NO_ERROR)
            {
                buffer = new byte[pdwSize];
                res = GetTcpTable(buffer, out pdwSize, true);
                if (res != 0) throw new InvalidOperationException("Error get tcp table");
            }

            MIB_TCPTABLE tcpTable = new MIB_TCPTABLE();
            int nOffset = 0;
            tcpTable.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            tcpTable.table = new MIB_TCPROW[tcpTable.dwNumEntries];

            for (int i = 0; i < tcpTable.dwNumEntries; i++)
            {
                int st = Convert.ToInt32(buffer[nOffset]);
                tcpTable.table[i].StrgState = ConvertState(st);
                tcpTable.table[i].iState = st;
                nOffset += 4;
                string LocalAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;
                int LocalPort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                    (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);

                nOffset += 4;
                tcpTable.table[i].Local = new IPEndPoint(IPAddress.Parse(LocalAdrr), LocalPort);
                string RemoteAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;
                int RemotePort;
                if (RemoteAdrr == "0.0.0.0")
                {
                    RemotePort = 0;
                }
                else
                {
                    RemotePort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                        (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);
                }
                nOffset += 4;
                tcpTable.table[i].Remote = new IPEndPoint(IPAddress.Parse(RemoteAdrr), RemotePort);
            }
        }

        internal List<Tcp> GetExtendedTcpTable()
        {
            int AF_INET = 2;
            int buffSize = 20000;
            byte[] buffer = new byte[buffSize];

            int res = GetExtendedTcpTable(buffer, out buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            if (res != NO_ERROR) throw new InvalidOperationException("Error get extended tcp table");

            MIB_TCPTABLE_OWNER_PID tcpExtendedTable = new MIB_TCPTABLE_OWNER_PID();
            int nOffset = 0;
            tcpExtendedTable.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            tcpExtendedTable.table = new MIB_TCPROW_OWNER_PID[tcpExtendedTable.dwNumEntries];
            for (int i = 0; i < tcpExtendedTable.dwNumEntries; i++)
            {
                int st = Convert.ToInt32(buffer[nOffset]);
                tcpExtendedTable.table[i].State = ConvertState(st);
                tcpExtendedTable.table[i].dwState = st;
                nOffset += 4;
                tcpExtendedTable.table[i].Local = BufferToIPEndPoint(buffer, ref nOffset, false);
                tcpExtendedTable.table[i].Remote = BufferToIPEndPoint(buffer, ref nOffset, true);
                tcpExtendedTable.table[i].dwOwningPid = BufferToInt(buffer, ref nOffset);
                tcpExtendedTable.table[i].ProcessName = GetProcessName(tcpExtendedTable.table[i].dwOwningPid);
            }
            List<Tcp> tcps = new List<Tcp>();
            for (int i = 0; i < tcpExtendedTable.table.Length; i++)
            {
                Tcp tcp = new Tcp { ProcessId = tcpExtendedTable.table[i].dwOwningPid, LocalIp = tcpExtendedTable.table[i].Local.Address.ToString(), LocalPort = tcpExtendedTable.table[i].Local.Port, RemoteIp = tcpExtendedTable.table[i].Remote.Address.ToString(), RemotePort = tcpExtendedTable.table[i].Remote.Port, ProcessName = tcpExtendedTable.table[i].ProcessName, State = tcpExtendedTable.table[i].State };
                if (HideLocal && (tcp.LocalIp == "0.0.0.0" || tcp.LocalIp == "127.0.0.1"))
                {
                    continue;
                }
                tcps.Add(tcp);
            }
            return tcps;
        }

        internal void GetUdpTable()
        {
            byte[] buffer = new byte[20000];
            int pdwSize = 20000;
            int res = GetUdpTable(buffer, out pdwSize, true);
            if (res != NO_ERROR)
            {
                buffer = new byte[pdwSize];
                res = GetUdpTable(buffer, out pdwSize, true);
                if (res != 0) throw new InvalidOperationException("Error get udp table");
            }

            MIB_UDPTABLE udpTable = new MIB_UDPTABLE();
            int nOffset = 0;
            udpTable.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            udpTable.table = new MIB_UDPROW[udpTable.dwNumEntries];
            for (int i = 0; i < udpTable.dwNumEntries; i++)
            {
                string LocalAdrr = buffer[nOffset].ToString() + "." + buffer[nOffset + 1].ToString() + "." + buffer[nOffset + 2].ToString() + "." + buffer[nOffset + 3].ToString();
                nOffset += 4;

                int LocalPort = (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) +
                    (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);
                nOffset += 4;
                udpTable.table[i].Local = new IPEndPoint(IPAddress.Parse(LocalAdrr), LocalPort);
            }
        }

        internal void GetExtendedUdpTable()
        {
            int AF_INET = 2;
            int buffSize = 20000;
            byte[] buffer = new byte[buffSize];

            int res = GetExtendedUdpTable(buffer, out buffSize, true, AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            if (res != NO_ERROR) throw new InvalidOperationException("Error get extended udp table");

            MIB_UDPTABLE_OWNER_PID udpExtendedTable = new MIB_UDPTABLE_OWNER_PID();
            int nOffset = 0;
            udpExtendedTable.dwNumEntries = Convert.ToInt32(buffer[nOffset]);
            nOffset += 4;
            udpExtendedTable.table = new MIB_UDPROW_OWNER_PID[udpExtendedTable.dwNumEntries];
            for (int i = 0; i < udpExtendedTable.dwNumEntries; i++)
            {
                udpExtendedTable.table[i].Local = BufferToIPEndPoint(buffer, ref nOffset, false);
                udpExtendedTable.table[i].dwOwningPid = BufferToInt(buffer, ref nOffset);
                udpExtendedTable.table[i].ProcessName = GetProcessName(udpExtendedTable.table[i].dwOwningPid);
            }
        }

        internal UInt16 ConvertPort(UInt32 dwPort)
        {
            byte[] b = new byte[2];
            b[0] = byte.Parse((dwPort >> 8).ToString());
            b[1] = byte.Parse((dwPort & 0xFF).ToString());
            return BitConverter.ToUInt16(b, 0);
        }

        internal string ConvertState(int state)
        {
            string res = "";
            switch (state)
            {
                case TCP_STATE_CLOSED: res = "CLOSED"; break;
                case TCP_STATE_LISTEN: res = "LISTEN"; break;
                case TCP_STATE_SYN_SENT: res = "SYN_SENT"; break;
                case TCP_STATE_SYN_RCVD: res = "SYN_RCVD"; break;
                case TCP_STATE_ESTAB: res = "ESTAB"; break;
                case TCP_STATE_FIN_WAIT1: res = "FIN_WAIT1"; break;
                case TCP_STATE_FIN_WAIT2: res = "FIN_WAIT2"; break;
                case TCP_STATE_CLOSE_WAIT: res = "CLOSE_WAIT"; break;
                case TCP_STATE_CLOSING: res = "CLOSING"; break;
                case TCP_STATE_LAST_ACK: res = "LAST_ACK"; break;
                case TCP_STATE_TIME_WAIT: res = "TIME_WAIT"; break;
                case TCP_STATE_DELETE_TCB: res = "DELETE_TCB"; break;
            }
            return res;
        }

        internal IPEndPoint BufferToIPEndPoint(byte[] buffer, ref int nOffset, bool IsRemote)
        {
            Int64 m_Address = ((((buffer[nOffset + 3] << 0x18) | (buffer[nOffset + 2] << 0x10)) | (buffer[nOffset + 1] << 8)) | buffer[nOffset]) & ((long)0xffffffff);
            nOffset += 4;
            int m_Port = 0;
            m_Port = (IsRemote && (m_Address == 0)) ? 0 :
                        (((int)buffer[nOffset]) << 8) + (((int)buffer[nOffset + 1])) + (((int)buffer[nOffset + 2]) << 24) + (((int)buffer[nOffset + 3]) << 16);
            nOffset += 4;

            IPEndPoint temp = new IPEndPoint(m_Address, m_Port);
            return temp;
        }

        internal int BufferToInt(byte[] buffer, ref int nOffset)
        {
            int res = (((int)buffer[nOffset])) + (((int)buffer[nOffset + 1]) << 8) +
                (((int)buffer[nOffset + 2]) << 16) + (((int)buffer[nOffset + 3]) << 24);
            nOffset += 4;
            return res;
        }

        internal string GetProcessName(int pid)
        {
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pid);
                return p != null ? p.ProcessName : "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion
    }
}
