using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JYCS.Schemas
{
    public interface IBaseMessage
    {
        void ProcessMessage();
        void ParseInXml(string xml);
        string ParseOutXml(Exception ex = null, bool detial = false);
        /// <summary>
        /// 消息跟踪ID，采用GUID全部大写
        /// </summary>
        string MessageID { get; set; }
    }

    public class IMessage<TIN, TOUT> : IBaseMessage
    {
        public TIN InObject;
        public TOUT OutObject;
        public virtual void ProcessMessage()
        {
        }

        public void ParseInXml(string xml)
        {
            InObject = MessageParse.ToXmlObject<TIN>(xml, true);
        }

        /// <summary>
        /// 是否包含传入人员类别
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string decode(string value, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (value == args[i].ToString())
                {
                    return args[i + 1].ToString();
                }
                i++;
            }
            return string.Empty;
        }

        public virtual string ParseOutXml(Exception ex = null, bool detial = false)
        {
            if (ex != null)
            {
                ex = ex.InnerException ?? ex;
            }
            if (OutObject == null)
            {
                dynamic obj = Activator.CreateInstance(this.GetType().BaseType.GetGenericArguments()[1]);
                obj.OUTMSG.ERRMSG = "";
                obj.OUTMSG.ERRNO = "0";
                OutObject = obj;
            }
            else
            {
                dynamic obj = (dynamic)OutObject;
                obj.OUTMSG.ERRNO = obj.OUTMSG.ERRNO ?? "0";
                obj.OUTMSG.ERRMSG = obj.OUTMSG.ERRMSG ?? "";
                obj.OUTMSG.ERRMSGEX = obj.OUTMSG.ERRMSGEX ?? "";
            }
			if (InObject != null)
			{
				((dynamic)OutObject).OUTMSG.ZHONGDUANLSH = ((dynamic)InObject).BASEINFO.ZHONGDUANLSH;
				((dynamic)OutObject).OUTMSG.ZHONGDUANJBH = ((dynamic)InObject).BASEINFO.ZHONGDUANJBH;
				((dynamic)OutObject).OUTMSG.MSGNO = ((dynamic)InObject).BASEINFO.MSGNO;
			}
            ((dynamic)OutObject).OUTMSG.MessageID = MessageID;
            if (ex != null)
            {
                if (((dynamic)OutObject).OUTMSG.ERRNO == "-2")
                {
                    if (detial)
                    {
                        ((dynamic)OutObject).OUTMSG.ERRMSGEX = ex.StackTrace;
                    }
                    ((dynamic)OutObject).OUTMSG.ERRMSG = ex.Message;
                    ((dynamic)OutObject).OUTMSG.ERRNO = "-2";
                }
                else
                {
                    if (detial)
                    {
                        ((dynamic)OutObject).OUTMSG.ERRMSGEX = ex.StackTrace;
                    }
                    ((dynamic)OutObject).OUTMSG.ERRMSG = ex.Message;
                    ((dynamic)OutObject).OUTMSG.ERRNO = "-1";
                }
            }
            return MessageParse.GetXml(OutObject, true);
        }

        /// <summary>
        /// 获得医保名称，返回空时为自费
        /// </summary>
        /// <param name="bingrenlb"></param>
        /// <returns></returns>
        public string GetYBJK(string bingrenlb)
        {
            switch (bingrenlb)
            {
                case "1": return "ZIFEI";//自费
                case "15": return "ZheJiangSZYB";//省医保
                case "16": return "HangZhouSYB";//市医保
                case "55": return "ZheJiangSYKT";//省一卡通
                case "56": return "HangZhouSYKT";//市一卡通
                case "40": return "JiaXingSYB";//嘉兴市医保
                case "53": return "JiaXingSYKT";//嘉兴市异地医保
                case "84": return "HangZhouSYB2";//新杭州市医保
                default: return "";
            }
        }

        /// <summary>
        /// 将字符串转换成十进制数 参数为String.Empty和null时返回0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defval">转换失败是返回的默认值</param>
        /// <returns></returns>
        public decimal de(object value, decimal defval = 0M)
        {
            value = value ?? defval;
            return string.IsNullOrWhiteSpace(value.ToString()) ? defval : decimal.Parse(value.ToString());
        }

        public void RollBack(System.Data.Common.DbTransaction tran, bool p = false)
        {
            try
            {
                tran.Rollback();
            }
            catch
            {
                if (p)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 消息ID (GUID)
        /// </summary>
        public string MessageID { get; set; }
    }

    /// <summary>
    /// 北京华康医疗科技 药房自动化系统接口
    /// </summary>
    /// <typeparam name="TIN"></typeparam>
    /// <typeparam name="TOUT"></typeparam>
    public class HKCMessage<TIN, TOUT> : IBaseMessage {
        public TIN InObject;
        public TOUT OutObject;
        /// <summary>
        /// 消息ID (GUID)
        /// </summary>
        public string MessageID { get; set; }

        public virtual void ProcessMessage()
        {
        }

        public void ParseInXml(string xml)
        {
            InObject = MessageParse.ToXmlObject<TIN>(xml, true);
        }

        public virtual string ParseOutXml(Exception ex = null, bool detial = false)
        {
            if (ex != null)
            {
                ex = ex.InnerException ?? ex;
            }
            if (OutObject == null)
            {
                dynamic obj = Activator.CreateInstance(this.GetType().BaseType.GetGenericArguments()[1]);
                obj.ResultCode ="0";
                obj.ResultContent = "成功";
                OutObject = obj;
            }
            else
            {
                dynamic obj = (dynamic)OutObject;
            }
            if (ex != null)
            {
                if (detial)
                {
                    ((dynamic)OutObject).ResultContent = ex.StackTrace;
                }
                ((dynamic)OutObject).ResultContent = ex.Message;
                ((dynamic)OutObject).ResultCode = "-1";
            }
            return MessageParse.GetXml(OutObject, true);
        }
    }
}
