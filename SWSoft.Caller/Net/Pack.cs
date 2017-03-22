using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SWSoft.Net
{
    public class Pack
    {
        /// <summary>
        /// IP头
        /// </summary>
        public IPHeader Header { get; set; }
        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string FromIP { get; set; }
        /// <summary>
        /// 目标地址
        /// </summary>
        public string ToIP { get; set; }
        /// <summary>
        /// 来源端口
        /// </summary>
        public int FromPort { get; set; }
        /// <summary>
        /// 目标端口
        /// </summary>
        public int ToPort { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Data { get; set; }

        public string HexData { get; set; }
        /// <summary>
        /// TCP头
        /// </summary>
        public TCPHeader TCP { get; set; }
    }
}
