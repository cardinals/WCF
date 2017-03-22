using System.Net.Sockets;

namespace SWSoft.Net
{
    public class Tcp
    {
        public ProtocolType ProtocolType { get { return ProtocolType.Tcp; } }
        /// <summary>
        /// 进程标志符
        /// </summary>
        public int ProcessId { get; set; }
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 本地地址
        /// </summary>
        public string LocalIp { get; set; }
        /// <summary>
        /// 本地端口
        /// </summary>
        public int LocalPort { get; set; }
        /// <summary>
        /// 远程地址
        /// </summary>
        public string RemoteIp { get; set; }
        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public string State { get; set; }
    }

    public class Udp
    { }
}
