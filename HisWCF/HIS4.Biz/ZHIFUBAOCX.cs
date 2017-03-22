using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Configuration;
using SWSoft.Framework;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using HIS4.Schemas;
using Common.Alipay;
using System.Net;
using System.IO;
using System.Xml;
namespace HIS4.Biz
{
    /// <summary>
    /// 支付宝查询 商户可查询交易的状态等信息。
    /// </summary>
    public class ZHIFUBAOCX : IMessage<ZHIFUBAOCX_IN, ZHIFUBAOCX_OUT>
    {
        public override void ProcessMessage()
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.acquire.query");
            sParaTemp.Add("out_trade_no", InObject.WIDOUTTRADENO);   //out_trade_no	商户网站唯一订单号	String(64)	支付宝合作商户网站唯一订单号。	不可空	HZ0120131127001           
            sParaTemp.Add("trade_no", InObject.TRADENO);//trade_no	支付宝交易号	String(64)	该交易在支付宝系统中的交易流水号。最短16位，最长64位。如果同时传了out_trade_no和trade_no，则以trade_no为准。	可空	2013112611001004680073956707
            string sHtmlText = Submit.BuildRequest(sParaTemp);
            LogUnit.Write("sHtmlText", "ZHIFUBAOCX");
            OutObject = new ZHIFUBAOCX_OUT();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode("alipay").ChildNodes;
                foreach (XmlNode xn in nodeList)
                {
                    if (xn.Name == "error")
                    {
                        throw new Exception(xn.InnerText);

                    }
                    if (xn.Name == "response")
                    {
                        XmlElement xe = (XmlElement)xn;
                        XmlNodeList subList = xe.ChildNodes;
                        foreach (XmlNode xmlNode in subList)
                        {
                            if (xmlNode.Name == "alipay")
                            {
                                XmlElement xemx = (XmlElement)xmlNode;
                                XmlNodeList submxList = xemx.ChildNodes;

                                foreach (XmlNode mxnode in submxList)
                                {
                                    if (mxnode.Name == "detail_error_code")
                                    {
                                        OutObject.ERRORCODE = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "detail_error_des")
                                    {
                                        OutObject.ERRORDES = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "result_code")
                                    {
                                        OutObject.RESULTCODE = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "trade_no")
                                    {
                                        OutObject.TRADENO = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "out_trade_no")
                                    {
                                        OutObject.WIDOUTREQUESTNO = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "buyer_user_id")
                                    {
                                        OutObject.BUYERUSERID = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "buyer_logon_id")
                                    {
                                        OutObject.BUYEREMAIL = mxnode.InnerText;
                                    }

                                    if (mxnode.Name == "partner")
                                    {
                                        OutObject.PARTNER = mxnode.InnerText;
                                    }
                                    if (mxnode.Name == "trade_status")
                                    {
                                        OutObject.TRADESTATUS = mxnode.InnerText;
                                    }
                                }

                            }

                        }

                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
    }
}
