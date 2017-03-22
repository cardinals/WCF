using System.Net;
using System.Text;
using System;
using System.IO;
using System.Windows.Forms;

namespace SWSoft.Net
{
    public class UDPHeader
    {
        /// <summary>
        /// 来源端口
        /// </summary>
        public ushort FromPort { get; set; }
        /// <summary>
        /// 目标端口
        /// </summary>
        public ushort ToPort { get; set; }
        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 检验和
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data { get; set; }

        public UDPHeader(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            FromPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            ToPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            Length = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16()) - 8;
            CheckSum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            Data = new byte[Length];
            Array.Copy(byBuffer, 8, Data, 0, Length);
        }
    }
}