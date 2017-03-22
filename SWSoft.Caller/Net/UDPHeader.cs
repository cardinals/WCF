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
        /// ��Դ�˿�
        /// </summary>
        public ushort FromPort { get; set; }
        /// <summary>
        /// Ŀ��˿�
        /// </summary>
        public ushort ToPort { get; set; }
        /// <summary>
        /// ���ݰ�����
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// ���յ�������
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