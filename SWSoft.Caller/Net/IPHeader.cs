using System.Net;
using System.Text;
using System;
using System.IO;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;

namespace SWSoft.Net
{
    /// <summary>
    /// IP数据头
    /// </summary>
    public class IPHeader
    {
        /// <summary>
        /// IP版本号
        /// </summary>
        public byte Version { get; set; }
        /// <summary>
        /// 头部长度
        /// </summary>
        public byte HeaderLength { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public byte TOS { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public ushort Identification { get; set; }
        /// <summary>
        /// 分段标志
        /// </summary>
        public int[] Fragment { get; set; }
        /// <summary>
        /// 分段偏移量
        /// </summary>
        public ushort OffSet { get; set; }
        /// <summary>
        /// 生存时间
        /// </summary>
        public byte TTL { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// 校验和
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>                                                      
        public string From { get; set; }
        /// <summary>
        /// 目标地址
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data { get; set; }


        public IPHeader(byte[] buffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(buffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            Version = binaryReader.ReadByte();
            HeaderLength = Version;
            Version >>= 4;
            HeaderLength <<= 4;
            HeaderLength >>= 4;
            HeaderLength *= 4;
            TOS = binaryReader.ReadByte();
            Length = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16()) - HeaderLength;
            Identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            OffSet = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            char[] bc = Convert.ToString(OffSet >> 13, 2).PadLeft(3, '0').ToCharArray();

            Fragment = new int[3];
            for (int i = 0; i < bc.Length; i++)
            {
                Fragment[i] = int.Parse(bc[i].ToString());
            }
            OffSet <<= 3;
            TTL = binaryReader.ReadByte();
            int x = binaryReader.ReadByte();
            ProtocolType = (ProtocolType)Enum.ToObject(typeof(ProtocolType), x);
            CheckSum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            From = new IPAddress((uint)binaryReader.ReadInt32()).ToString();
            To = new IPAddress((uint)binaryReader.ReadInt32()).ToString();
            if (From == "202.91.246.17")
            {
                Debug.Write("标志: " + Identification);
                Debug.WriteLine("分段：" + bc[0] + bc[1] + bc[2]);
            }
            Data = new byte[Length];
            Array.Copy(buffer, HeaderLength, Data, 0, Length);
        }
    }
}