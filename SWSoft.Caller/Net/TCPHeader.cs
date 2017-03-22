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
        /// ��Դ�˿�
        /// </summary>
        public ushort FromPort { get; set; }
        /// <summary>
        /// Ŀ��˿�
        /// </summary>
        public ushort ToPort { get; set; }
        /// <summary>
        /// ���к�
        /// </summary>
        public uint SequenceNumber { get; set; }
        /// <summary>
        /// ȷ�Ϻ�
        /// </summary>
        public uint AcknowledgementNumber { get; set; }
        /// <summary>
        /// ��־λ
        /// </summary>
        public ushort DataOffsetAndFlags { get; set; }
        /// <summary>
        /// ���ڴ�С
        /// </summary>
        public ushort WindowSize { get; set; }
        /// <summary>
        /// У���
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// ��������ƫ����
        /// </summary>
        public ushort UrgentPointer { get; set; }
        /// <summary>
        /// �ײ�����
        /// </summary>
        public byte HeaderLength { get; set; }
        /// <summary>
        /// ���ݳ���
        /// </summary>
        public ushort Length { get; set; }
        /// <summary>
        /// ���յ�������
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