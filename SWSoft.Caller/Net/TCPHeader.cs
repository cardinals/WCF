using System.Net;
using System.Text;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SWSoft.Net
{
    public class TCPHeader
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
        /// 序列号
        /// </summary>
        public uint SequenceNumber { get; set; }
        /// <summary>
        /// 确认号
        /// </summary>
        public uint AcknowledgementNumber { get; set; }
        /// <summary>
        /// 标志位
        /// </summary>
        public ushort DataOffsetAndFlags { get; set; }
        /// <summary>
        /// 窗口大小
        /// </summary>
        public ushort WindowSize { get; set; }
        /// <summary>
        /// 校验和
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// 紧急数据偏移量
        /// </summary>
        public ushort UrgentPointer { get; set; }
        /// <summary>
        /// 首部长度
        /// </summary>
        public byte HeaderLength { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public ushort Length { get; set; }
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data { get; set; }

        public TCPHeader(byte[] buffer, int received)
        {
            MemoryStream memoryStream = new MemoryStream(buffer, 0, received);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            FromPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            ToPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            SequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            AcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            DataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            WindowSize = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            CheckSum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            HeaderLength = (byte)(DataOffsetAndFlags >> 12);
            HeaderLength *= 4;
            Length = (ushort)(received - HeaderLength);
            Data = new byte[Length];
            Array.Copy(buffer, HeaderLength, Data, 0, Length);
        }
    }
}