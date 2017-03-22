using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Windows.Forms;
using System.Web.Hosting;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace HIS1.Biz
{
    public class BankAPI
    {
        /// <summary>
        /// 设置前置服务器的IP地址和端口
        /// </summary>
        /// <param name="nSvrPort">服务器端口号，长整型</param>
        /// <param name="szSvrAddr">服务器地址（医院前置机内网IP地址），字符串型</param>
        [DllImport(@"HiNetLib.dll", SetLastError = true, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = false)]
        public static extern void SetRemoteServerAddr(Int32 nSvrProt, string szSvrAddr);

        /// <summary>
        /// 向前置服务器发送交易
        /// </summary>
        /// <param name="sendbuf">要发送的数据包，字符串型, 长度必须在65535以内</param>
        /// <param name="sendlen">要发送的数据包长度，取sendbuf的长度，长整型</param>
        /// <param name="recvbuf">要接收的数据包，字符串长度必须在65535以内，必须预先分配地址空间。</param>
        /// <param name="recvlen">要接收的数据包长度，取recvbuf的长度，长整形。</param>
        /// <param name="waitsecs">超时限制（秒），请设置为200秒</param>
        /// <returns></returns>
        [DllImport("HiNetLib.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int SendRequestPack(string sendbuf, Int32 sendlen, [Out] StringBuilder recvbuf, ref int recvlen, Int32 waitsecs);

        static bool init = false;

        public static bool Call(BankParam callparam)
        {
            if (!init)
            {
                var path = Environment.GetEnvironmentVariable("path");
                Environment.SetEnvironmentVariable("path", HostingEnvironment.MapPath("~") + "\\bin\\His1;" + path);
                var strs = ConfigurationManager.AppSettings["yinyipt"].Split(':');
                SetRemoteServerAddr(int.Parse(strs[1]), strs[0]);
                init = true;
            }
            var recvbuff = new StringBuilder(2048);
            int recvlen = 2048;
            var retcode = SendRequestPack(callparam.sendbuf, callparam.sendbuf.Length, recvbuff, ref recvlen, 200);
            if (retcode != 0)
            {
                switch (retcode)
                {
                    case 1: throw new Exception("银医平台,连接医院前置机服务器失败");//
                    case 2: throw new Exception("银医平台,向中心服务器发送数据失败");
                    case 3: throw new Exception("银医平台,接收返回结果失败");
                    case 4: throw new Exception("银医平台,动态链接库不存在");
                    default: throw new Exception("银医平台,未知错误:" + retcode);
                }
            }
            else
            {
                callparam.SetOut(recvbuff.ToString());
                if (callparam.OutVal("FanHuiBH") == "-1")
                {
                    throw new Exception(ConfigurationManager.AppSettings["yinhangsbts"] + callparam.OutVal("CuoWuXX"));
                }
                return true;
            }
        }
    }

    public class BankParam
    {
        public List<KeyValuePair<string, object>> Items;
        public Dictionary<string, object> OutItems { get; set; }
        public object this[string name]
        {
            get
            {
                return "";
            }
            set
            {
                Items.Add(new KeyValuePair<string, object>(name, value));
            }
        }

        public BankParam()
        {
            Items = new List<KeyValuePair<string, object>>();
            OutItems = new Dictionary<string, object>();
            recvbuf = string.Empty.PadRight(1024);
        }

        public string recvbuf;

        public string sendbuf
        {
            get
            {
                var str = "";
                foreach (var item in Items)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                return str.TrimEnd('&');
            }
        }

        public void SetOut(string recvbuf)
        {
            //FanHuiBH=1&KeHuXm=严一三&YuE=62441.42&KeYongYE=62441.42
            foreach (var item in recvbuf.Split('&'))
            {
                OutItems.Add(item.Split('=')[0].ToUpper(), item.Split('=')[1]);
            }
        }

        public string OutVal(string name)
        {
            name = name.ToUpper();
            if (OutItems.ContainsKey(name))
            {
                return OutItems[name].ToString();
            }
            return string.Empty;
        }
    }
}
