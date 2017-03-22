using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web.Hosting;
using log4net;
using System.Diagnostics;
using SWSoft.Framework;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml;
using Common.Alipay;
using HisWCFSVR.Entity;
using HIS4.Schemas;
using JYCS.Schemas;

namespace HisWCFSVR
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    public class MediInfoHis : IHisApplay
    {
        static ILog log = log4net.LogManager.GetLogger("XmlLog");
        public int RunService(string TradeType, string TradeMsg, ref string TradeMsgOut)
        {

            int i = 0;
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            //log.Info("[" + guid + "]");
            log.InfoFormat("[{2}][{0}].IN  {1}", TradeType, LogUnity.I.ShowXml(TradeMsg), guid);
            //string outxml = "";
            IBaseMessage objtype = null;
            try
            {
                objtype = (IBaseMessage)ToolUnity.LoadAssembly(TradeType);
            }
            catch (Exception ex)
            {
                //outxml = ToolUnity.ServiceERR(TradeType, ex);
                TradeMsgOut = ToolUnity.ServiceERR(TradeType, ex);
                i = -1;
                goto last;
            }
            #region 验证数字签名
            //为适应老版本,这个参数设置成0，以后新上线的 都必须设置成1
            string Signature = ConfigurationManager.AppSettings["Signature"];
            if (Signature == "1")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(TradeMsg);
                string key = "";
                string mac = "";
                string sign = "";
                if (doc.DocumentElement != null)
                {
                    string head = doc.DocumentElement.Name;

                    XmlNode nodemac = doc.SelectSingleNode(head + "/BASEINFO/MACDZ");
                    if (nodemac == null)
                    {
                        TradeMsgOut = ToolUnity.ServiceERR(TradeType, "MACDZ未分配!");
                        return -1;
                    }
                    mac = nodemac.InnerText;
                    XmlNode nodekey = doc.SelectSingleNode(head + "/BASEINFO/KEY");
                    if (nodekey == null)
                    {
                        TradeMsgOut = ToolUnity.ServiceERR(TradeType, "校验码未分配!");
                        return -1;
                    }
                    key = nodekey.InnerText;
                    XmlNode nodesign = doc.SelectSingleNode(head + "/BASEINFO/SIGN");
                    if (nodesign == null)
                    {
                        TradeMsgOut = ToolUnity.ServiceERR(TradeType, "数字验证未分配!");
                        return -1;
                    }
                    sign = nodesign.InnerText;
                    XmlNode BASEINFO = doc.SelectSingleNode(head + "/BASEINFO");
                    BASEINFO.RemoveChild(nodesign);
                }
                string inxml = doc.InnerXml;

                DataTable dt = DBVisitor.ExecuteTable($"select * from GY_REGISTERED where mac='{mac}'");
                if (dt.Rows.Count <= 0)
                {
                    TradeMsgOut = ToolUnity.ServiceERR(TradeType, "未注册!");
                    i = -1;
                    return i;
                }
                if (dt.Rows[0]["KEY"].ToString() != key)
                {
                    TradeMsgOut = ToolUnity.ServiceERR(TradeType, "校验码验证不通过!");
                    i = -1;
                    return i;

                }
                string mysign = AlipayMD5.Sign(inxml, dt.Rows[0]["KEY"].ToString(), "utf-8");
                if (mysign != sign)
                {

                    TradeMsgOut = ToolUnity.ServiceERR(TradeType, "签名验证错误!");
                    i = -1;
                    return i;

                }
            }
            #endregion
            try
            {
                objtype.MessageID = guid;
                objtype.ParseInXml(TradeMsg);
                objtype.ProcessMessage();
                //outxml = objtype.ParseOutXml();
                TradeMsgOut = objtype.ParseOutXml();
                //LogUnity.I.Info(0, TradeType, TradeMsg, outxml, "", guid);
            }
            catch (Exception ex)
            {
                //outxml = objtype.ParseOutXml(ex, ConfigurationManager.AppSettings["enabledymc"] == "0");
                TradeMsgOut = objtype.ParseOutXml(ex, ConfigurationManager.AppSettings["enabledymc"] == "0");
                //LogUnity.I.Insert(-1, TradeType, TradeMsg, outxml, ex.StackTrace.Trim(), guid);
                //LogUnity.I.Insert(-1, TradeType, TradeMsg, TradeMsgOut, ex.StackTrace.Trim(), guid);
                i = -1;
            }
            last:
            {
                //log.InfoFormat("[{0}].OUT {1}", TradeType, outxml + "\r\n");
                log.InfoFormat("[{2}][{0}].OUT {1}", TradeType, TradeMsgOut, guid);
                return i;
            }
        }

        /// <summary>
        /// 注册获取交易安全校验码
        /// </summary>
        /// <param name="MAC"></param>
        /// <param name="key"></param>
        public int Signature(string MAC, ref string key, ref string Erro)
        {
            DataTable dt = DBVisitor.ExecuteTable($"select * from GY_REGISTERED where mac='{MAC}'");
            int i = 0;
            key = GetRandomString(12);
            if (dt.Rows.Count > 0)
            {
                i = DBVisitor.ExecuteNonQuery($"update GY_REGISTERED set KEY='{key}' where MAC='{MAC}'");

            }
            else
            {
                i = DBVisitor.ExecuteNonQuery($"insert into GY_REGISTERED(MAC,KEY) values('{MAC}','{key}')");
            }
            if (i > 0)
            {
                return 0;
            }
            else
            {
                Erro = "注册失败!";
                return -1;
            }

        }
        /// <summary>
        /// 生成随机数的种子
        /// </summary>
        /// <returns></returns>
        private static int getNewSeed()
        {
            byte[] rndBytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// 生成秘钥
        /// </summary>
        /// <param name="length"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private string GetRandomString(int len)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(getNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }


        /// <summary>
        /// 转诊预约接口转发(桐庐) 和区域卫生做的对接 _______胡谦
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <param name="ehrXml"></param>
        public void DoBusiness(string header, string body, ref string ehrXml)
        {
            log.InfoFormat("DoBusiness##" + header + "###" + body);
            string ZHUANZHENYYLXDM = ConfigurationManager.AppSettings["ZHUANZHENYYLXDM"];
            int i = 0;
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            log.InfoFormat("[{2}][{0}].IN  {1}", header, LogUnity.I.ShowXml(body), guid);

            Header head = new Header();
            head = MessageParse.ToXmlObject<Header>(header);

            Body inobject = MessageParse.ToXmlObject<Body>(body);

            BASEINFO baseinfo = new BASEINFO();
            baseinfo.CAOZUORQ = head.RequestTime;
            baseinfo.CAOZUOYDM = head.UserId;
            baseinfo.JIGOUDM = head.OrganizationId;
            baseinfo.MACDZ = head.lient_Mac;

            Result result = new Result();
            result.Code = "0";
            result.Msg = "OK";

            //modify by 沈宝
            try
            {
                switch (head.DocumentID)
                {
                    #region 挂号科室信息
                    case "GUAHAOKSXX":
                        string ksdm = inobject.KESHIDM;
                        string sql = "select * from v_ksxx_mcp";
                        if (!string.IsNullOrEmpty(ksdm))
                        {
                            sql += $" where  KESHIDM='{ksdm}'";
                        }
                        DataTable dt = DBVisitor.ExecuteTable(sql);

                        Body_GUAHAOKSXX outObj = new Body_GUAHAOKSXX
                        {
                            Result = new Result(),
                            KESHIMX = new List<Entity.KESHIXX>()
                        };
                        outObj.Result = result;
                        foreach (DataRow dr in dt.Rows)
                        {
                            Entity.KESHIXX ksxx = new Entity.KESHIXX
                            {
                                JIUZHENDD = dr["WEIZHI"].ToString(),
                                KESHIDM = dr["KESHIDM"].ToString(),
                                KESHIJS = dr["KESHIJS"].ToString(),
                                KESHIMC = dr["KESHIMC"].ToString(),
                                KESHIPX = ""
                            };
                            outObj.KESHIMX.Add(ksxx);
                        }
                        ehrXml = MessageParse.GetXml(outObj);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);

                        return;
                    #endregion

                    #region 医生信息
                    case "YISHENGJSXX":

                        string ysdm = inobject.YISHENGDM;
                        sql = "select * from v_zgxx_mcp ";
                        if (!string.IsNullOrEmpty(ysdm))
                        {
                            sql += $" where ZHIGONGID='{ysdm}'";

                        }
                        dt = DBVisitor.ExecuteTable(sql);

                        Body_YISHENGJSXX jsxxObj = new Body_YISHENGJSXX
                        {
                            Result = new Result(),
                            YISHENGMX = new List<Entity.YISHENGXX>()
                        };
                        jsxxObj.Result = result;
                        foreach (DataRow dr in dt.Rows)
                        {
                            Entity.YISHENGXX ysxx = new Entity.YISHENGXX
                            {
                                YISHENGDM = dr["ZHIGONGID"].ToString(),
                                YISHENGXM = dr["ZHIGONGXM"].ToString(),
                                YISHENGXB = dr["XB"].ToString(),
                                ZHENGJIANLX = "1",
                                ZHENGJIANHM = dr["shenfenzh"].ToString(),
                                YISHENGPX = "",
                                YISHENGZC = dr["ZHICHENG"].ToString(),
                                YISHENGTC = dr["YISHENGSC"].ToString(),
                                YISHENGJS = dr["DISCRIPTION"].ToString(),
                                ZHAOPIAN = "",

                            };
                            jsxxObj.YISHENGMX.Add(ysxx);
                        }
                        ehrXml = MessageParse.GetXml(jsxxObj);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);

                        return;
                    #endregion

                    #region 排班信息 排班 上午和下午要分两条转 排班ID 要不一样
                    case "YIYUANPBXX":
                        //mody by 沈宝2017/1/9
                        string ysdmpb = inobject.YISHENGDM == "" ? "1" : "2";
                        if (ysdmpb == "1")
                        {
                            string sql1 = (inobject.KESHIDM == "" ? "" : "and YIZHOUKSDM = " + inobject.KESHIDM);//过滤条件可是代码为空时传全部;
                            sql = $" select * from V_YIYUANYZPBXX_MCP where YIZHOUPNLBDM='1' and YUYUELX='{ZHUANZHENYYLXDM}'{sql1}";//YUYUELX 10是分配给转诊预约的，测试时用2
                        }
                        else
                        {
                            string sql1 = (inobject.KESHIDM == "" ? "" : "and YIZHOUKSDM = " + inobject.KESHIDM);
                            sql = $" select * from V_YIYUANYZPBXX_MCP where YUYUELX='{ZHUANZHENYYLXDM}'{sql1}";
                        }
                        dt = DBVisitor.ExecuteTable(sql);

                        Body_YIYUANPBXX pbxxObj = new Body_YIYUANPBXX
                        {
                            Result = new Result(),
                            PAIBANLB = new List<Entity.PAIBANMX>()
                        };
                        pbxxObj.Result = result;
                        int pd = Convert.ToInt32(inobject.GUAHAOBC);
                        //modify by 沈宝
                        foreach (DataRow dr in dt.Rows)
                        {
                            DateTime schedulingDate = ToolUnity.RqtoXq(dr["YIZHOUXQ"].ToString());
                            #region 上午的排班
                            if (pd == 1)
                            {
                                Entity.PAIBANMX pbmx1 = new Entity.PAIBANMX
                                {
                                    HAOYUANFPLX = "0",
                                    KESHIDM = dr["YIZHOUKSDM"].ToString(),
                                    KESHIMC = dr["YIZHOUKSMC"].ToString(),
                                    JIUZHENDD = dr["GUAHAOKSWZMC"].ToString(),
                                    KESHIJS = "",
                                    KESHIPX = "",
                                    YISHENGDM = dr["YIZHOUYSDM"].ToString(),
                                    YISHENGXM = dr["YIZHOUPBLBDM"].ToString() == "1" ? dr["YIZHOUPBLBMC"].ToString() : dr["YIZHOUYSXM"].ToString(),//dr["YIZHOUPNLBDM"]=="1"?dr[]: dr["YIZHOUYSXM"].ToString(),
                                    ZHENLIAOF = dr["ZHENLIAOFEI"].ToString(),
                                    ZHENLIAOJSF = "0",
                                    PAIBANRQ = schedulingDate.ToString("yyyy-MM-dd"),
                                    TINGZHENBZ = "0",
                                    GUAHAOBC = "1",
                                    GUAHAOLB = dr[" "].ToString(),
                                    YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|1",
                                    DANGTIANPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|1"
                                };
                                if (int.Parse(dr["YIZHOUSWZGXH"].ToString()) > 0)
                                {
                                    pbxxObj.PAIBANLB.Add(pbmx1);
                                }
                            }
                            else if (pd == 2)
                            {
                                #endregion
                                #region 下午的排班


                                Entity.PAIBANMX pbmx2 = new Entity.PAIBANMX
                                {
                                    HAOYUANFPLX = "0",
                                    KESHIDM = dr["YIZHOUKSDM"].ToString(),
                                    KESHIMC = dr["YIZHOUKSMC"].ToString(),
                                    JIUZHENDD = dr["GUAHAOKSWZMC"].ToString(),
                                    KESHIJS = "",
                                    KESHIPX = "",
                                    YISHENGDM = dr["YIZHOUYSDM"].ToString(),
                                    YISHENGXM = dr["YIZHOUPBLBDM"].ToString() == "1" ? dr["YIZHOUPBLBMC"].ToString() : dr["YIZHOUYSXM"].ToString(),// dr["YIZHOUYSXM"].ToString(),
                                    ZHENLIAOF = dr["ZHENLIAOFEI"].ToString(),
                                    ZHENLIAOJSF = "0",
                                    PAIBANRQ = schedulingDate.ToString("yyyy-MM-dd"),
                                    TINGZHENBZ = "0",
                                    GUAHAOBC = "2",
                                    GUAHAOLB = dr["YIZHOUPBLBDM"].ToString(),
                                    YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2",
                                    DANGTIANPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2"
                                };
                                //pbmx2.YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2";
                                if (int.Parse(dr["YIZHOUXWZGXH"].ToString()) > 0)
                                {
                                    pbxxObj.PAIBANLB.Add(pbmx2);
                                }
                            }
                            else if (pd == 4 || pd == 0)
                            {
                                #endregion
                                #region 全天的排班
                                Entity.PAIBANMX pbmx1 = new Entity.PAIBANMX
                                {
                                    HAOYUANFPLX = "0",
                                    KESHIDM = dr["YIZHOUKSDM"].ToString(),
                                    KESHIMC = dr["YIZHOUKSMC"].ToString(),
                                    JIUZHENDD = dr["GUAHAOKSWZMC"].ToString(),
                                    KESHIJS = "",
                                    KESHIPX = "",
                                    YISHENGDM = dr["YIZHOUYSDM"].ToString(),
                                    YISHENGXM = dr["YIZHOUPBLBDM"].ToString() == "1" ? dr["YIZHOUPBLBMC"].ToString() : dr["YIZHOUYSXM"].ToString(),// dr["YIZHOUYSXM"].ToString(),
                                    ZHENLIAOF = dr["ZHENLIAOFEI"].ToString(),
                                    ZHENLIAOJSF = "0",
                                    PAIBANRQ = schedulingDate.ToString("yyyy-MM-dd"),
                                    TINGZHENBZ = "0",
                                    GUAHAOBC = pd.ToString(),
                                    GUAHAOLB = dr["YIZHOUPBLBDM"].ToString(),
                                    YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|1",
                                    DANGTIANPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|1"
                                };
                                if (int.Parse(dr["YIZHOUSWZGXH"].ToString()) > 0)
                                {
                                    pbxxObj.PAIBANLB.Add(pbmx1);
                                }

                                PAIBANMX pbmx2 = new Entity.PAIBANMX
                                {
                                    HAOYUANFPLX = "0",
                                    KESHIDM = dr["YIZHOUKSDM"].ToString(),
                                    KESHIMC = dr["YIZHOUKSMC"].ToString(),
                                    JIUZHENDD = dr["GUAHAOKSWZMC"].ToString(),
                                    KESHIJS = "",
                                    KESHIPX = "",
                                    YISHENGDM = dr["YIZHOUYSDM"].ToString(),
                                    YISHENGXM = dr["YIZHOUPBLBDM"].ToString() == "1" ? dr["YIZHOUPBLBMC"].ToString() : dr["YIZHOUYSXM"].ToString(),//dr["YIZHOUYSXM"].ToString(),
                                    ZHENLIAOF = dr["ZHENLIAOFEI"].ToString(),
                                    ZHENLIAOJSF = "0",
                                    PAIBANRQ = schedulingDate.ToString("yyyy-MM-dd"),
                                    TINGZHENBZ = "0",
                                    GUAHAOBC = pd.ToString(),
                                    GUAHAOLB = dr["YIZHOUPBLBDM"].ToString(),
                                    YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2",
                                    DANGTIANPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2"
                                };
                                //pbmx2.YIZHOUPBID = dr["YIZHOUPBID"].ToString() + "|" + dr["YIZHOUYSDM"].ToString() + "|2";
                                if (int.Parse(dr["YIZHOUXWZGXH"].ToString()) > 0)
                                {
                                    pbxxObj.PAIBANLB.Add(pbmx2);
                                }
                                #endregion

                            }
                            else
                            {
                                //inobject.GUAHAOBC = '3';晚上排班;
                                pbxxObj.Result.Msg = "无晚上排班";
                            }


                        }
                        ehrXml = MessageParse.GetXml(pbxxObj);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);

                        return;
                    #endregion

                    #region 号源信息
                    case "GUAHAOHYXX":
                        //已挂号号源信息
                        sql = "SELECT  * from V_GUAHAOYSYHYXX_MCP A where 1=1 ";
                        if (!string.IsNullOrEmpty(inobject.KESHIDM))
                        {
                            sql += $" and PAIBANKSDM='{inobject.KESHIDM}'";
                        }
                        if (!string.IsNullOrEmpty(inobject.YISHENGDM))
                        {
                            sql += $" and PAIBANYSGH='{inobject.YISHENGDM}'";

                        }
                        DataTable YghDt = DBVisitor.ExecuteTable(sql);

                        //挂号号源信息 预约类型代码10开放给转诊预约---测试的时候用2 正式使用的时候是10
                        sql = $"SELECT* FROM V_YIYUANGHHYXX_MCP WHERE   YUYUELXDM= '{ZHUANZHENYYLXDM}'";
                        if (!string.IsNullOrEmpty(inobject.KESHIDM))
                        {
                            sql += $" and YIZHOUKSDM='{inobject.KESHIDM}'";
                        }
                        if (!string.IsNullOrEmpty(inobject.YISHENGDM))
                        {
                            sql += $" and YIZHOUYSDM='{inobject.YISHENGDM}'";

                        }
                        if (!string.IsNullOrEmpty(inobject.GUAHAOBC))
                        {
                            if (inobject.GUAHAOBC != "0" && inobject.GUAHAOBC != "4")
                            {
                                sql += $" and PAIBANLX='{inobject.GUAHAOBC}'";
                            }
                        }
                        DataTable ghhyDt = DBVisitor.ExecuteTable(sql);
                        if (ghhyDt.Rows.Count < 1) { result.Msg = "无所要查询号源信息"; }

                        DataTable HouZhenSj = DBVisitor.ExecuteTable("select * from mz_houzhensj a");

                        Body_GUAHAOHYXX hyxxOut = new Body_GUAHAOHYXX { Result = new Result() };
                        hyxxOut.Result = result;
                        hyxxOut.HAOYUANMX = new List<Entity.HAOYUANXX>();
                        foreach (DataRow variable in ghhyDt.Rows)
                        {
                            #region  //上午号源
                            if (int.Parse(variable["SHANGWUYYXH"].ToString()) > 0)
                            {
                                string[] guaHaoSwXh = variable["SHANGWUYYXHMX"].ToString().Split('^');
                                foreach (var SwXhmx in guaHaoSwXh)
                                {
                                    if (!string.IsNullOrEmpty(SwXhmx))
                                    {
                                        //判断号源是否被占用
                                        DataRow[] dr =
                                            YghDt.Select(
                                                $"PAIBANID='{variable["YIZHOUPBID"].ToString()}' and PAIBANKSDM='{variable["YIZHOUKSDM"].ToString()}'" +
                                                $" and PAIBANYSGH='{variable["YIZHOUYSDM"].ToString()}' and PAIBANGHLB='{variable["YIZHOUPBLBDM"].ToString()}'" +
                                                $" and HAOYUANSYSJ='1' and HAOYUANSYXH='{SwXhmx}'");
                                        if (dr.Length == 0)
                                        {

                                            Entity.HAOYUANXX hymx1 = new Entity.HAOYUANXX
                                            {
                                                PAIBANRQ = variable["schedulingdate"].ToString().Substring(0, 10),
                                                GUAHAOBC = "1",
                                                GUAHAOLB = variable["YIZHOUPBLBDM"].ToString(),
                                                YISHENGDM = variable["YIZHOUYSDM"].ToString(),
                                                KESHIDM = variable["YIZHOUKSDM"].ToString(),
                                                YIZHOUPBID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|1",
                                                DANGTIANPBID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|1",
                                                HAOYUANLB = "1",
                                                GUAHAOXH = SwXhmx,
                                                HAOYUANID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|1|" + SwXhmx
                                            };
                                            DataRow[] HzSjDr =
                                                HouZhenSj.Select(
                                                    $"qishighxh<={SwXhmx} and jieshughxh>={SwXhmx} and shangxiawbz=0 and yishengid='{variable["YIZHOUYSDM"].ToString()}' and KESHIID='{variable["YIZHOUKSDM"].ToString()}'");
                                            if (HzSjDr.Length > 0)
                                            {
                                                hymx1.JIUZHENSJ = HzSjDr[0]["KAISHISJ"].ToString() + "-" +
                                                                  HzSjDr[0]["JIESHUSJ"].ToString();
                                            }
                                            else
                                            {
                                                hymx1.JIUZHENSJ = "07:30-12:00";
                                            }
                                            hyxxOut.HAOYUANMX.Add(hymx1);

                                        }
                                    }
                                }

                            }
                            #endregion

                            #region  下午号源
                            if (int.Parse(variable["XIAWUYYXH"].ToString()) > 0)
                            {
                                string[] guaHaoXwXh = variable["xiawuyyyhmx"].ToString().Split('^');
                                foreach (var XwXhmx in guaHaoXwXh)
                                {
                                    if (!string.IsNullOrEmpty(XwXhmx))
                                    {
                                        //判断号源是否被占用
                                        DataRow[] dr =
                                            YghDt.Select(
                                                $"PAIBANID='{variable["YIZHOUPBID"].ToString()}' and PAIBANKSDM='{variable["YIZHOUKSDM"].ToString()}'" +
                                                $" and PAIBANYSGH='{variable["YIZHOUYSDM"].ToString()}' and PAIBANGHLB='{variable["YIZHOUPBLBDM"].ToString()}'" +
                                                $" and HAOYUANSYSJ='1' and HAOYUANSYXH='{XwXhmx}'");
                                        if (dr.Length == 0)
                                        {

                                            Entity.HAOYUANXX hymx2 = new Entity.HAOYUANXX
                                            {
                                                PAIBANRQ = variable["schedulingdate"].ToString().Substring(0, 10),
                                                GUAHAOBC = "1",
                                                GUAHAOLB = variable["YIZHOUPBLBDM"].ToString(),
                                                YISHENGDM = variable["YIZHOUYSDM"].ToString(),
                                                KESHIDM = variable["YIZHOUKSDM"].ToString(),
                                                YIZHOUPBID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|2",
                                                DANGTIANPBID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|2",
                                                HAOYUANLB = "1",
                                                GUAHAOXH = XwXhmx,
                                                HAOYUANID =
                                                    variable["YIZHOUPBID"].ToString() + "|" +
                                                    variable["YIZHOUYSDM"].ToString() + "|2|" + XwXhmx
                                            };
                                            //候诊时间
                                            DataRow[] HzSjDr =
                                                HouZhenSj.Select(
                                                    $"qishighxh<={XwXhmx} and jieshughxh>={XwXhmx} and shangxiawbz=1 and yishengid='{variable["YIZHOUYSDM"].ToString()}' and KESHIID='{variable["YIZHOUKSDM"].ToString()}'");
                                            if (HzSjDr.Length > 0)
                                            {
                                                hymx2.JIUZHENSJ = HzSjDr[0]["KAISHISJ"].ToString() + "-" +
                                                                  HzSjDr[0]["JIESHUSJ"].ToString();
                                            }
                                            else
                                            {
                                                hymx2.JIUZHENSJ = "13:30-17:00";
                                            }
                                            hyxxOut.HAOYUANMX.Add(hymx2);

                                        }
                                    }
                                }

                            }
                            #endregion
                        }
                        ehrXml = MessageParse.GetXml(hyxxOut);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);

                        return;
                    #endregion

                    #region 预约挂号
                    case "YUYUEGH":
                        inobject.YUYUEXX.YIZHOUPBID = inobject.YUYUEXX.YIZHOUPBID.Split('|')[0] == "" ? inobject.YUYUEXX.DANGTIANPBID.Split('|')[0] : inobject.YUYUEXX.YIZHOUPBID.Split('|')[0];

                        Body_YUYUEGH yyghOut = new Body_YUYUEGH();
                        yyghOut.Result = new Result();
                        yyghOut.Result = result;
                        sql = $"select * from V_YIYUANYZPBXX_MCP where YIZHOUPBID='{inobject.YUYUEXX.YIZHOUPBID}'";//YUYUELX 10是分配给转诊预约的，测试时用2
                        dt = DBVisitor.ExecuteTable(sql);
                        if (dt.Rows.Count == 0)
                        {
                            result.Code = "-1";
                            result.Msg = $"找不到排班信息";
                            yyghOut.Result = result;
                            ehrXml = MessageParse.GetXml(yyghOut);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        GUAHAOYY_IN ghyy = new GUAHAOYY_IN();
                        ghyy.BASEINFO = new BASEINFO();
                        ghyy.BASEINFO = baseinfo;
                        ghyy.JIUZHENKLX = inobject.YUYUEXX.JIUZHENKLX == "2" ? "3" : "2";
                        ghyy.JIUZHENKH = inobject.YUYUEXX.JIUZHENKH;
                        ghyy.ZHENGJIANLX = "1";
                        ghyy.ZHENGJIANHM = inobject.YUYUEXX.ZHENGJIANHM;
                        ghyy.XINGMING = inobject.YUYUEXX.XINGMING;
                        ghyy.YIZHOUPBID = inobject.YUYUEXX.YIZHOUPBID;
                        ghyy.DANGTIANPBID = inobject.YUYUEXX.YIZHOUPBID;
                        ghyy.RIQI = inobject.YUYUEXX.PAIBANRQ;
                        ghyy.GUAHAOBC = inobject.YUYUEXX.GUAHAOBC;
                        ghyy.GUAHAOLB = inobject.YUYUEXX.GUAHAOLB;
                        ghyy.KESHIDM = inobject.YUYUEXX.KESHIDM;
                        ghyy.YISHENGDM = inobject.YUYUEXX.YISHENGDM;
                        ghyy.GUAHAOXH = inobject.YUYUEXX.GUAHAOXH;
                        ghyy.LIANXIDH = inobject.YUYUEXX.LIANXIDH;
                        ghyy.YUYUELY = "10";
                        string TradeMsg = MessageParse.GetXml(ghyy);
                        string TradeMsgOut = "";
                        int j = RunService("HIS4.Biz.GUAHAOYY", TradeMsg, ref TradeMsgOut);
                        //   ehrXml = TradeMsgOut;
                        GUAHAOYY_OUT yyout = new GUAHAOYY_OUT();
                        yyout = MessageParse.ToXmlObject<GUAHAOYY_OUT>(TradeMsgOut);
                        if (yyout.OUTMSG.ERRNO != "0")
                        {
                            result.Code = "-1";
                            result.Msg = yyout.OUTMSG.ERRMSG;
                            yyghOut.Result = result;
                            ehrXml = MessageParse.GetXml(yyghOut);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }

                        yyghOut.GUAHAOYY = new GUAHAOYY();
                        GUAHAOYY Outghyy = new GUAHAOYY
                        {
                            YUYUEID = yyout.QUHAOMM,
                            QUHAOMM = yyout.QUHAOMM,
                            JIUZHENSJ = yyout.JIUZHENSJ,
                            GUAHAOXH = yyout.GUAHAOXH
                        };
                        yyghOut.GUAHAOYY = Outghyy;

                        yyghOut.FEIYONGMX = new List<Entity.FEIYONGXX>();

                        Entity.FEIYONGXX fyxx1 = new Entity.FEIYONGXX
                        {
                            XIANGMUXH = dt.Rows[0]["ZHENLIAOFEIDM"].ToString(),
                            XIANGMUMC = dt.Rows[0]["ZHENLIAOFEIMC"].ToString(),
                            FEIYONGLX = "04",
                            DANJIA = dt.Rows[0]["ZHENLIAOFEI"].ToString(),
                            XIANGMUDW = "次",
                            SHULIANG = "1",
                            JINE = dt.Rows[0]["ZHENLIAOFEI"].ToString(),
                            YIBAODJ = "",
                            YIBAODM = "",
                            YIBAOZFBL = ""
                        };
                        yyghOut.FEIYONGMX.Add(fyxx1);

                        Entity.FEIYONGXX fyxx2 = new Entity.FEIYONGXX
                        {
                            XIANGMUXH = dt.Rows[0]["ZHENLIAOJSFDM"].ToString(),
                            XIANGMUMC = dt.Rows[0]["ZHENLIAOJSFMC"].ToString(),
                            FEIYONGLX = "14",
                            DANJIA = dt.Rows[0]["ZHENLIAOJSF"].ToString(),
                            XIANGMUDW = "次",
                            SHULIANG = "1",
                            JINE = dt.Rows[0]["ZHENLIAOJSF"].ToString(),
                            YIBAODJ = "",
                            YIBAODM = "",
                            YIBAOZFBL = ""
                        };
                        yyghOut.FEIYONGMX.Add(fyxx2);
                        ehrXml = MessageParse.GetXml(yyghOut);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                        break;
                    #endregion
                    #region 预约退号
                    case "YUYUETH":
                        Body_YUYUETH yyth = new Body_YUYUETH();
                        yyth.Result = result;
                        GUAHAOYYTH_IN yythIn = new GUAHAOYYTH_IN();
                        yythIn.BASEINFO = new BASEINFO();
                        yythIn.BASEINFO = baseinfo;
                        yythIn.JIUZHENKLX = inobject.JIUZHENKLX == "2" ? "3" : "2";
                        yythIn.JIUZHENKH = inobject.JIUZHENKH;
                        yythIn.ZHENGJIANLX = "1";
                        yythIn.ZHENGJIANHM = inobject.ZHENGJIANHM;
                        yythIn.XINGMING = inobject.XINGMING;
                        yythIn.YUYUELY = "10";
                        yythIn.QUHAOMM = inobject.QUHAOMM;
                        TradeMsg = MessageParse.GetXml(yythIn);
                        TradeMsgOut = "";
                        j = RunService("HIS4.Biz.GUAHAOYYTH", TradeMsg, ref TradeMsgOut);
                        GUAHAOYYTH_OUT yythout = new GUAHAOYYTH_OUT();
                        yythout = MessageParse.ToXmlObject<GUAHAOYYTH_OUT>(TradeMsgOut);
                        if (yythout.OUTMSG.ERRNO != "0")
                        {
                            result.Code = "-1";
                            result.Msg = yythout.OUTMSG.ERRMSG;
                            yyth.Result = result;
                            ehrXml = MessageParse.GetXml(yyth);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        ehrXml = MessageParse.GetXml(yyth);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                        break;
                    #endregion
                    #region  病情自述
                    //modify by 沈宝
                    case "BINGQINGZS":
                        Body_BINGQINGZS bqzs = new Body_BINGQINGZS();
                        bqzs.Result = result;
                        BINGQINGZS_IN bqzs_in = new BINGQINGZS_IN();
                        bqzs_in.BASEINFO = new BASEINFO();
                        bqzs_in.BASEINFO = baseinfo;
                        bqzs_in.YILIAOJGDM = inobject.YILIAOJGDM;
                        if (string.IsNullOrEmpty(bqzs_in.YILIAOJGDM))
                        {
                            result.Code = "-1";
                            result.Msg = "医疗机构不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.JIUZHENKLX = inobject.JIUZHENKLX;
                        bqzs_in.JIUZHENKH = inobject.JIUZHENKH;
                        bqzs_in.ZHENGJIANLX = inobject.ZHENGJIANLX;
                        if (string.IsNullOrEmpty(bqzs_in.ZHENGJIANLX))
                        {
                            result.Code = "-1";
                            result.Msg = "证件类型不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.ZHENGJIANHM = inobject.ZHENGJIANHM;
                        if (string.IsNullOrEmpty(bqzs_in.ZHENGJIANHM))
                        {
                            result.Code = "-1";
                            result.Msg = "证件号码不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;

                        }
                        bqzs_in.XINGMING = inobject.XINGMING;
                        if (string.IsNullOrEmpty(bqzs_in.XINGMING))
                        {
                            result.Code = "-1";
                            result.Msg = "姓名不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.YUYUEID = inobject.YUYUEID;  //预约ID 是返回的信息的唯一标示；
                        if (string.IsNullOrEmpty(bqzs_in.YUYUEID))
                        {
                            result.Code = "-1";
                            result.Msg = "预约ID不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.GUAHAOXH = inobject.GUAHAOXH;
                        if (string.IsNullOrEmpty(bqzs_in.GUAHAOXH))
                        {
                            result.Code = "-1";
                            result.Msg = "挂号序号不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.RIQI = inobject.RIQI;
                        if (string.IsNullOrEmpty(bqzs_in.RIQI))
                        {
                            result.Code = "-1";
                            result.Msg = "日期不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.YUYUELY = inobject.YUYUELY;
                        if (string.IsNullOrEmpty(bqzs_in.YUYUELY))
                        {
                            result.Code = "-1";
                            result.Msg = "预约来源不能为空！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        bqzs_in.GUOMINYW = inobject.GUOMINYW;
                        bqzs_in.BINGQINGMS = inobject.BINGQINGMS;
                        string yuYuesj = @"select * from mz_guahaoyy where yuyueid = '{0}'and yuyuezt='0'";   //预约状态 0 未取，为1 取过，为2 取消；取过挂号单不在病情描述；
                        DataTable dtYuYueJL = DBVisitor.ExecuteTable(string.Format(yuYuesj, bqzs_in.YUYUEID));
                        if (dtYuYueJL == null || dtYuYueJL.Rows.Count <= 0)
                        {
                            result.Code = "-1";
                            result.Msg = "未找到预约记录！";
                            bqzs.Result = result;
                            ehrXml = MessageParse.GetXml(bqzs);
                            log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                            return;
                        }
                        else
                        {
                            string bingqingmsgx = @"update mz_guahaoyy set JIWANGSHI='{0}',GUOMINSHI='{1}' where yuyueid = '{2}'";
                            try
                            {
                                DBVisitor.ExecuteNonQuery(string.Format(bingqingmsgx, bqzs_in.BINGQINGMS, bqzs_in.GUOMINYW, bqzs_in.YUYUEID));
                            }
                            catch (Exception e)
                            {
                                result.Code = "-1";
                                result.Msg = e.Message;
                                bqzs.Result = result;
                                ehrXml = MessageParse.GetXml(bqzs);
                                log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                                return;
                            }
                        }
                        ehrXml = MessageParse.GetXml(bqzs);
                        log.InfoFormat("[{2}][{0}].OUT  {1}", head.DocumentID, LogUnity.I.ShowXml(ehrXml), guid);
                        break;
                    #endregion

                    case "PAIBANHYXX":

                        break;
                }
            }
            catch (Exception e)
            {
                log.InfoFormat("{0}", e.Message);
            }
        }
        /// <summary>
        /// 测试网络
        /// </summary>
        /// <returns></returns>
        public DateTime PCall()
        {
            return DateTime.Now;
        }

        public string sendToHis(string code)
        {

            code = code.Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&apos;", "'")
                .Replace("&quot;", "\"");

            string TradeMsgOut = string.Empty;
            string TradeType = "HKC.Biz.";
            string TradeMsg = string.Empty;
            string TradeT = string.Empty;
            if (code.Contains("DISP_READY"))//完成配药
            {
                TradeT = "DISP_READY";
            }
            if (code.Contains("DISP_START"))//开始配药
            {
                TradeT = "DISP_START";
            }
            if (code.Contains("DISP_PREPARE"))//配药状态查询
            {
                TradeT = "DISP_PREPARE";
            }
            if (code.Contains("DISP_FINISH"))//完成发药
            {
                TradeT = "DISP_FINISH";
            }
            if (!string.IsNullOrEmpty(TradeT))
            {
                TradeType = TradeType + TradeT;
                TradeMsg = code.Replace("<code>", "<" + TradeT + "_IN>").Replace("</code>", "</" + TradeT + "_IN>");
                RunService(TradeType, TradeMsg, ref TradeMsgOut);
                TradeMsgOut = TradeMsgOut.Replace("<" + TradeT + "_OUT>", "<Response>").Replace("</" + TradeT + "_OUT>", "</Response>");
            }
            else
            {
                TradeMsgOut = "<Response><ResultCode>-1</ResultCode><ResultContent>未定义的接口</ResultContent></Response>";
            }
            return TradeMsgOut;
        }
    }
}
