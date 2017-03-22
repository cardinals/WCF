using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;

namespace HisDllOp
{
    public class Unity
    {
        /**/
        /// <summary>
        /// 将Xml内容字符串转换成DataSet对象
        /// </summary>
        /// <param name="xmlStr">Xml内容字符串</param>
        /// <returns>DataSet对象</returns>
        public static DataSet CXmlToDataSet(string xmlStr)
        {

            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                        StrStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 卡类型
        /// </summary>
        /// <param name="klx"></param>
        /// <returns></returns>
        public string KLXZH(string klx)
        {
            string reklx = "1";
            switch (klx)
            {
                case "0"://院内诊疗卡
                    reklx = "";
                    break;
                case "1"://身份证
                    reklx = "";
                    break;
                case "2"://社保卡
                    reklx = "";
                    break;
                case "3"://银行卡
                    reklx = "";
                    break;
                case "4"://居民健康卡
                    reklx = "";
                    break;

            }
            return reklx;
        }
        /// <summary>
        /// 病人类别转化
        /// </summary>
        /// <param name="lb"></param>
        /// <returns></returns>
        public static int BingRenLb1(string lb)
        {
            int relb = 1;
            switch (lb)
            {
                case "0":
                    relb = 1;
                    break;
                case "1":
                    relb = 56;
                    break;
                case "2":
                    relb = 84;
                    break;
            }


            return relb;
        }
        public static int BingRenLb2(string lb)
        {
            int relb = 1;
            switch (lb)
            {
                case "1"://自费
                    relb = 0;
                    break;
                case "56"://农保
                    relb = 1;
                    break;
                case "84"://医保
                    relb = 2;
                    break;
            }


            return relb;
        }
        ///挂号类别
        ///
        public static int GUAHAOLBZH(string guahaolb)
        {
            int ghlb = 0;
            switch (guahaolb)
            {
                case ""://全部
                    ghlb = 0;
                    break;
                case "0"://普通
                    ghlb = 1;
                    break;
                case "1"://专家
                    ghlb = 3;
                    break;
            }
            return ghlb;
        }
        ///卡状态
        /*
        public static int KZTZH(string kzt)
        {
 
        }
        */
        ///性别置换
        public static int XINGBIEZH(string xingbie)
        {
            int xb = 0;
            switch (xingbie)
            {
                case "男"://
                    xb = 1;
                    break;
                case "女":
                    xb = 2;
                    break;
                case "未知":
                    xb = 9;
                    break;
            }
            return xb;

        }
        public static string XINGBIEFCCFZH(string xingbie)
        {
            string xb = "";
            switch (xingbie)
            {
                case "1"://
                    xb = "男";
                    break;
                case "2":
                    xb = "女";
                    break;
                case "0":
                    xb = "未知";
                    break;
            }
            return xb;

        }
        /*
        public static int GUAHAOBCZH(string guahaobc)
        {
 
        }
        */


        internal static object XINGBIEFZH(string p)
        {
            throw new NotImplementedException();
        }
    }
}
