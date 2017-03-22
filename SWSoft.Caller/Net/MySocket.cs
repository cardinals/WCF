using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using SWSoft.Net;

namespace SWSoft.Net
{
    /// <summary>
    /// 实现自定义套接字接口
    /// </summary>
    public class MySocket
    {
        /// <summary>
        /// 表示处理事件 SWSoft.Net.ReceiveCompleted 的方法
        /// </summary>
        /// <param name="pack"></param>
        public delegate void ReceiveCompleted(Pack pack);
        /// <summary>
        /// 数据接收完时是发生
        /// </summary>
        public event ReceiveCompleted Received;
        /// <summary>
        /// 绑定的套接字接口
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// 与本地终结点相关联的IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 存储接收到的数据的位置
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 是否启动数据接收
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 操作完成是调用的方法
        /// </summary>
        public AsyncCallback CallBack { get; set; }
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public List<byte> Pack { get; set; }
        /// <summary>
        /// 过滤规则
        /// </summary>
        public SocketFilter Filter { get; set; }

        /// <summary>
        /// 创建一个套接字接口
        /// </summary>
        public void CreateSocket()
        {
            CreateSocket(SocketType.Raw);
        }

        /// <summary>
        /// 创建一个套接字访问对象
        /// </summary>
        /// <param name="ip">要绑定的本地IP地址</param>
        public MySocket(string ip)
        {
            IP = ip;
            CreateSocket();
        }

        /// <summary>
        /// 创建一个套接字接口
        /// </summary>
        /// <param name="socketType">套接字类型</param>
        public void CreateSocket(SocketType socketType)
        {
            Socket = new Socket(AddressFamily.InterNetwork, socketType, ProtocolType.IP);
            Socket.Bind(new IPEndPoint(IPAddress.Parse(IP), 0));
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            byte[] byteIn = new byte[4] { 1, 0, 0, 0 };
            byte[] byteOut = new byte[4];
            try
            {
                Socket.IOControl(IOControlCode.ReceiveAll, byteIn, byteOut);
            }
            catch (SocketException)
            {
                byteIn = new byte[] { 0, 0, 0, 0 };
                Socket.IOControl(IOControlCode.ReceiveAll, byteIn, byteOut);
            }
        }

        /// <summary>
        /// 启动接收数据
        /// </summary>
        public void Start()
        {
            Enable = true;
            Receive();
        }

        /// <summary>
        /// 开始异步接收数据
        /// </summary>
        public void Receive()
        {
            Data = new byte[4096];
            Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, OnReceive, null);
        }

        /// <summary>
        /// 停止异步接收数据
        /// </summary>
        public void Stop()
        {
            Enable = false;
        }

        /// <summary>
        /// 数据接收完成回调方法
        /// </summary>
        private void OnReceive(IAsyncResult ar)
        {
            if (Enable)
            {
                int buffReceived = Socket.EndReceive(ar);
                if (buffReceived > 0)
                {
                    SWSoft.Net.Pack pack = Convert(buffReceived);
                    Received(pack);
                }
                Receive();
            }
        }

        /// <summary>
        /// 将接收到的数据转换成SWSoft.Net.Pack
        /// </summary>
        /// <param name="buffReceived">接收到的数据</param>
        private Pack Convert(int buffReceived)
        {
            IPHeader ipHeader = new IPHeader(Data, buffReceived);
            Pack pack = new Pack { Header = ipHeader, FromIP = ipHeader.From, ToIP = ipHeader.To, ProtocolType = ipHeader.ProtocolType };
            switch (ipHeader.ProtocolType)
            {
                case ProtocolType.Tcp: return ToTcp(pack, ipHeader);
                case ProtocolType.Udp: return ToUdp(pack, ipHeader);
                default: return pack;
            }
        }

        public Pack ToTcp(Pack pack, IPHeader ipHeader)
        {
            TCPHeader tcpHeader = new TCPHeader(ipHeader.Data, ipHeader.Length);
            pack.FromPort = tcpHeader.FromPort;
            pack.ToPort = tcpHeader.ToPort;
            pack.Data = tcpHeader.Data;
            pack.Length = tcpHeader.Length;
            pack.TCP = tcpHeader;
            return pack;
        }

        public Pack ToUdp(Pack pack, IPHeader ipHeader)
        {
            UDPHeader udpHeader = new UDPHeader(ipHeader.Data, ipHeader.Length);
            pack.FromPort = udpHeader.FromPort;
            pack.ToPort = udpHeader.ToPort;
            pack.Data = udpHeader.Data;
            pack.Length = udpHeader.Length;
            return pack;
        }
    }

    public class SocketFilter
    {
        /// <summary>
        /// 来源端口
        /// </summary>
        public List<int> FromPort { get; set; }
        /// <summary>
        /// 目标端口
        /// </summary>
        public List<int> ToPort { get; set; }
        /// <summary>
        /// 来源IP
        /// </summary>
        public List<string> FromIp { get; set; }
        /// <summary>
        /// 目标IP
        /// </summary>
        public List<string> ToIp { get; set; }

        /// <summary>
        /// 是否通过过滤
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public bool Pass(Pack pack)
        {
            bool rs1, rs2, rs3, rs4;
            rs1 = FromPort == null ? true : FromPort.IndexOf(pack.FromPort) >= 0;
            rs2 = ToPort == null ? true : ToPort.IndexOf(pack.ToPort) >= 0;
            rs3 = FromIp == null ? true : FromIp.IndexOf(pack.FromIP) >= 0;
            rs4 = ToIp == null ? true : ToIp.IndexOf(pack.ToIP) >= 0;
            return rs1 || rs2 || rs3 || rs4;
        }
    }
}
