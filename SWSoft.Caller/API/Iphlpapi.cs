using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SWSoft.Reflector
{
    public class Iphlpapi
    {
        /// <summary>
        /// 发送ARP包
        /// </summary>
        /// <param name="DestIP">目标IP</param>
        /// <param name="SrcIP">来源IP</param>
        /// <param name="MacAddr">MAC地址</param>
        /// <param name="MacLength">MAC地址长度</param>
        /// <returns></returns>
        [DllImport("Iphlpapi.dll")]
        public static extern int SendARP(int DestIP, int SrcIP, ref long MacAddr, ref int PhyAddrLen);
        [DllImport("Ws2_32.dll")]
        public static extern Int32 inet_addr(string ipaddr);
    }
}