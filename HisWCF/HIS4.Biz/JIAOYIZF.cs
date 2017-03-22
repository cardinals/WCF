using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWSoft.Framework;
using JYCS.Schemas;
using System.Xml;
using Common.WSEntity;
using Common.WSCall;
using System.IO;
using HIS4.Schemas;
using System.Net;
using System.IO.Compression;
using System.Security.Cryptography;

namespace HIS4.Biz
{
    public class JIAOYIZF : IMessage<HIS4.Schemas.JIAOYIZF.JIAOYIZF_IN, HIS4.Schemas.JIAOYIZF.JIAOYIZF_OUT>
    {
        public override void ProcessMessage()
        {   //接口类型
            if (string.IsNullOrEmpty(InObject.JIEKOULX)) throw new Exception("传入参数[JIEKOULX]为空");
            //交易类型
            if (string.IsNullOrEmpty(InObject.JIAOYILX)) throw new Exception("传入参数[JIAOYILX]为空");
            //交易入参
            if (string.IsNullOrEmpty(InObject.JIAOYIRC)) throw new Exception("传入参数[JIAOYIRC]为空");
            //区分转发那个接口交易
            string outVar = "";
            switch (InObject.JIEKOULX)
            {
                case "1":
                    string Indata = InObject.JIAOYIRC.Replace("[", "<").Replace("]", ">");
                    outVar = testgethospital(Indata, InObject.JIAOYILX);
                    outVar.Replace("<", "[").Replace(">","]");
                    break;
                default:
                    throw new Exception("接口类型未实现!");
                    break;
            }

        }
        private static string testgethospital(string Message, string JiaoYLX)
        {
            string URL = "http://localhost:9000/";//服务器地址
            string partner_key = "xxxxxxx"; //合作密钥
            string partner = "20110010"; //合作id
            switch (JiaoYLX)
            {
                case "hospital"://1.批量推送医院：
                    URL += "hospital/batch_push";
                    break;
                case "department"://批量推送科室
                    URL += "department/batch_push";
                    break;
                case "expert"://3.批量推送专家
                    URL += "expert/batch_push";
                    break;
                case "shiftcase"://批量推送排班（带日期）
                    URL += "shiftcase/batch_push";
                    break;

                default:
                    throw new Exception("交易类型未实现!");

            }
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(URL);
            wreq.Method = "POST";
            //报文内容
            string message = Message;// "<customer> <name>jack</name></customer>";
            // ASCIIEncoding encoding = new ASCIIEncoding();
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
            byte[] data = encoding.GetBytes(message);


            //头部参数设置
            wreq.Headers.Add("partner", partner); //加入合作id
            wreq.Headers.Add("sign", getmd5(partner_key + message).ToLower()); //加入签名，md5（32位）加密后小写，加密字符串：合作密钥+报文内容
            //设置发送的内容长度
            wreq.ContentLength = data.Length;

            Stream newstream = wreq.GetRequestStream();
            // send the data.
            newstream.Write(data, 0, data.Length);
            newstream.Close();

            HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse();
            StreamReader sr = new StreamReader(wres.GetResponseStream(), encoding);

            //获取返回数据，xml格式
            string result = sr.ReadToEnd();

            sr.Close();
            wres.Close();
            return result;

            //解析xml数据，并做相应的处理            

        }
        #region md5加密
        /// <summary>
        ///  md5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getmd5(string value)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string secstr = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
            secstr = secstr.Replace("-", "");
            return secstr;
        }
        #endregion
    }
}
