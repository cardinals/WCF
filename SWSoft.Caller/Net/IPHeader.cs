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
    /// IP����ͷ
    /// </summary>
    public class IPHeader
    {
        /// <summary>
        /// IP�汾��
        /// </summary>
        public byte Version { get; set; }
        /// <summary>
        /// ͷ������
        /// </summary>
        public byte HeaderLength { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public byte TOS { get; set; }
        /// <summary>
        /// ���ݳ���
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// ��ʶ
        /// </summary>
        public ushort Identification { get; set; }
        /// <summary>
        /// �ֶα�־
        /// </summary>
        public int[] Fragment { get; set; }
        /// <summary>
        /// �ֶ�ƫ����
        /// </summary>
        public ushort OffSet { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public byte TTL { get; set; }
        /// <summary>
        /// Э��
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// У���
        /// </summary>
        public short CheckSum { get; set; }
        /// <summary>
        /// ��Դ��ַ
        /// </summary>                                                      
        public string From { get; set; }
        /// <summary>
        /// Ŀ���ַ
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// ���յ�������
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
                Debug.Write("��־: " + Identification);
                Debug.WriteLine("�ֶΣ�" + bc[0] + bc[1] + bc[2]);
            }
            Data = new byte[Length];
            Array.Copy(buffer, HeaderLength, Data, 0, Length);
        }
    }
}