using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using SWSoft.Reflector.Api;
using System.IO.Compression;
using System.Diagnostics;

namespace SWSoft.Net
{
    public partial class Connection
    {
        #region Properties

        /// <summary>
        /// Tcp连接表
        /// </summary>
        public List<Tcp> TcpTable { get { return GetExtendedTcpTable(); } }
        /// <summary>
        /// Udp连接表
        /// </summary>
        public Udp UdpTable { get; set; }
        /// <summary>
        /// 是否隐藏本地连接
        /// </summary>
        public bool HideLocal { get; set; }

        public Process Process { get; set; }

        /// <summary>
        /// 创建一个获得本地网络连接状态的对象
        /// </summary>
        /// <param name="hide">是否隐藏本地连接</param>
        public Connection(bool hide = true)
        {
            HideLocal = hide;
        }

        public int ProcessId(string processName)
        {
            foreach (var item in TcpTable)
            {
                if (item.ProcessName == "iexplore")
                {
                    return item.ProcessId;
                }
            }
            return 0;
        }

        public Dictionary<string, string> TcpList()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (Process != null)
            {
                foreach (var item in TcpTable)
                {
                    if (item.ProcessName == Process.ProcessName)
                    {
                        string key = item.RemoteIp + ":" + item.RemotePort;
                        if (!dict.ContainsKey(key))
                        {
                            dict.Add(key, item.LocalIp + ":" + item.LocalIp);
                        }
                    }
                }
            }
            return dict;
        }

        public Tcp TcpId(int processId)
        {
            foreach (var item in TcpTable)
            {
                if (item.ProcessId == processId)
                {
                    return item;
                }
            }
            return null;
        }

        #endregion
    }
}