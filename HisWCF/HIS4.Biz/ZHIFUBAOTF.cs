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
    /// 支付宝退费
    /// </summary>
    public class ZHIFUBAOTF : IMessage<ZHIFUBAOTF_IN, ZHIFUBAOTF_OUT>
    {
        public override void ProcessMessage()
        {
            #region 手机网页退费
            if (InObject.TUIFEILX == "1")
            {

                ////////////////////////////////////////////请求参数////////////////////////////////////////////


                //服务器异步通知页面路径
                string notify_url = InObject.NOTIFYURL;
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //卖家支付宝帐户
                string seller_email = Common.Alipay.Config.Seller_email;
                //必填

                if (string.IsNullOrEmpty(InObject.REFUNDDATA))
                {
                    throw new Exception("退款请求时间不能为空!");
                }

                try
                {
                    string dt = DateTime.Parse(InObject.REFUNDDATA).ToString("yyyy-MM-dd HH:mm:ss");

                }
                catch
                {
                    throw new Exception("退款时间格式不正确!");
                }
                //退款当天日期
                string refund_date = InObject.REFUNDDATA;
                //必填，格式：年[4位]-月[2位]-日[2位] 小时[2位 24小时制]:分[2位]:秒[2位]，如：2007-10-01 13:13:13

                if (string.IsNullOrEmpty(InObject.BATCHNO))
                {
                    throw new Exception("批次号不能为空!");
                }
                //批次号
                string batch_no = InObject.BATCHNO;// WIDbatch_no.Text.Trim();
                //必填，格式：当天日期[8位]+序列号[3至24位]，如：201008010000001

                if (string.IsNullOrEmpty(InObject.BATCHNUM))
                {
                    throw new Exception("退款笔数不能为空!");
                }
                //退款笔数
                string batch_num = InObject.BATCHNUM;
                //必填，参数detail_data的值中，“#”字符出现的数量加1，最大支持1000笔（即“#”字符出现的数量999个）

                //退款用trade_no 异步通知回调的时候你们可以拿到
                if (string.IsNullOrEmpty(InObject.DETAILDATA))
                {
                    throw new Exception("退款详细数据不能为空！");
                }
                //退款详细数据  结算时的trade_no
                string detail_data = InObject.DETAILDATA;// WIDdetail_data.Text.Trim();
                //必填，具体格式请参见接口技术文档


                ////////////////////////////////////////////////////////////////////////////////////////////////

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "refund_fastpay_by_platform_pwd");
                sParaTemp.Add("notify_url", notify_url);
                sParaTemp.Add("seller_email", seller_email);
                sParaTemp.Add("refund_date", refund_date);
                sParaTemp.Add("batch_no", batch_no);
                sParaTemp.Add("batch_num", batch_num);
                sParaTemp.Add("detail_data", detail_data);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                OutObject = new ZHIFUBAOTF_OUT();
                OutObject.TUIFEIURL = sHtmlText;
            }
            #endregion
            #region 商户代扣、二维码支付退费，两个方式一样
            else if (InObject.TUIFEILX == "2" || InObject.TUIFEILX == "3")
            {
                var listpbxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00002, InObject.WIDOUTTRADENO));
                if (listpbxx.Rows.Count <= 0)
                {
                    //if(string.IsNullOrEmpty(InObject.WIDTOTALFEE)&&)
                    throw new Exception("订单号为【" + InObject.WIDOUTTRADENO + "】的结算信息不存在！");
                }
                if (listpbxx.Rows[0]["JIESUANZT"].ToString() == "0")
                {
                    throw new Exception("订单【" + InObject.WIDOUTTRADENO + "】未支付!");
                }
                //-1支付失败 2未入账 3 退费成功 4 退费中 5 退费失败
                if (listpbxx.Rows[0]["JIESUANZT"].ToString() == "-1")
                {
                    throw new Exception("订单【" + InObject.WIDOUTTRADENO + "】支付未成功!");
                }
                //正记录的冲销标志=1代表 已经退费
                if (listpbxx.Rows[0]["CXBZ"].ToString() == "1")
                {
                    throw new Exception("订单【" + InObject.WIDOUTTRADENO + "】已退费!");
                }
                if (double.Parse(InObject.WIDTOTALFEE) != double.Parse(listpbxx.Rows[0]["ITOTALFEE"].ToString()))
                {
                    throw new Exception("传入的金额与结算的金额不一致!");
                }

                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "alipay.acquire.refund");
                sParaTemp.Add("out_trade_no", InObject.WIDOUTTRADENO);   //out_trade_no	商户网站唯一订单号	String(64)	支付宝合作商户网站唯一订单号。	不可空	HZ0120131127001           
                sParaTemp.Add("refund_amount", listpbxx.Rows[0]["ITOTALFEE"].ToString());    //refund_amount	退款金额	number(9,2)	退款金额不能大于订单金额，全额退款必须与订单金额一致。	不可空	200.00
                sParaTemp.Add("trade_no", listpbxx.Rows[0]["TRADENO"].ToString());//trade_no	支付宝交易号	String(64)	该交易在支付宝系统中的交易流水号。最短16位，最长64位。如果同时传了out_trade_no和trade_no，则以trade_no为准。	可空	2013112611001004680073956707
                sParaTemp.Add("out_request_no", InObject.WIDOUTREQUESTNO);//out_request_no	商户退款请求单号	String(64)	商户退款请求单号，用以标识本次交易的退款请求。如果不传入本参数，则以out_trade_no填充本参数的值。同时，认为本次请求为全额退款，要求退款金额和交易支付金额一致。	可空	HZ01RF001
                sParaTemp.Add("operator_id", InObject.BASEINFO.CAOZUOYDM);//operator_id	操作员号	String(28)	卖家的操作员ID。	可空	OP001
                sParaTemp.Add("refund_reason", InObject.REFUNDREASON);//refund_reason	退款原因	String(128)	退款原因说明。	可空	正常退款
                sParaTemp.Add("key", "rfk3sw6dlq28k2tijjj54le6lfhw73s4");//取余杭卫生局的key
                //sParaTemp.Add("it_b_pay", InObject.OUTTIME);
                //ref_ids	业务关联ID集合	String(256)	业务关联ID集合，用于放置商户的退款单号、退款流水号等信息，json格式，具体请参见“4.3  业务关联ID集合说明”。	可空	[{"id_type":"orig_out_request_no","id":"HZ0001"},{"id_type":"orig_out_order_no","id":"HZ0001"}]



                string sHtmlText = Submit.BuildRequest(sParaTemp);
                OutObject = new ZHIFUBAOTF_OUT();
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(sHtmlText);
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("alipay").ChildNodes;
                    foreach (XmlNode xn in nodeList)
                    {
                        //判断是否异常,如果返回异常 则抛出
                        //if (xn.Name == "is_success" && xn.InnerText == "F")
                        //{

                        //    throw new Exception("支付宝商户代扣失败!");
                        //}
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
                                        if (mxnode.Name == "fund_change")
                                        {
                                            OutObject.FUNDCHANGE = mxnode.InnerText;
                                        }

                                        if (mxnode.Name == "refund_fee")
                                        {
                                            OutObject.REFUNDFEE = mxnode.InnerText;
                                        }
                                    }

                                }

                            }

                        }
                    }
                    if (OutObject.RESULTCODE.ToUpper() == "SUCCESS")
                    {
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00015, "T" + InObject.WIDOUTTRADENO, "3",
                                   InObject.BASEINFO.CAOZUOYXM, InObject.BASEINFO.CAOZUOYDM, InObject.WIDOUTTRADENO));
                        //更新付记录状态 退费成功
                        // DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set JIESUANZT='3' where IOUTTRADENO='{0}' ", InObject.WIDOUTREQUESTNO));
                        //更新正记录 冲销标志
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set CXBZ='1' where IOUTTRADENO='{0}' ", InObject.WIDOUTTRADENO));

                    }
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message.ToString());
                }

            }
            #endregion
            #region 支付宝中心多了结算数据，冲正
            else if (InObject.TUIFEILX == "4")
            {
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "alipay.acquire.refund");
                sParaTemp.Add("out_trade_no", InObject.WIDOUTTRADENO);   //out_trade_no	商户网站唯一订单号	String(64)	支付宝合作商户网站唯一订单号。	不可空	HZ0120131127001           
                sParaTemp.Add("refund_amount", InObject.WIDTOTALFEE);    //refund_amount	退款金额	number(9,2)	退款金额不能大于订单金额，全额退款必须与订单金额一致。	不可空	200.00
                sParaTemp.Add("trade_no", InObject.TRADENO);//trade_no	支付宝交易号	String(64)	该交易在支付宝系统中的交易流水号。最短16位，最长64位。如果同时传了out_trade_no和trade_no，则以trade_no为准。	可空	2013112611001004680073956707
                sParaTemp.Add("out_request_no", InObject.WIDOUTREQUESTNO);//out_request_no	商户退款请求单号	String(64)	商户退款请求单号，用以标识本次交易的退款请求。如果不传入本参数，则以out_trade_no填充本参数的值。同时，认为本次请求为全额退款，要求退款金额和交易支付金额一致。	可空	HZ01RF001
                sParaTemp.Add("operator_id", InObject.BASEINFO.CAOZUOYDM);//operator_id	操作员号	String(28)	卖家的操作员ID。	可空	OP001
                sParaTemp.Add("refund_reason", InObject.REFUNDREASON);//refund_reason	退款原因	String(128)	退款原因说明。	可空	正常退款
                sParaTemp.Add("key", "rfk3sw6dlq28k2tijjj54le6lfhw73s4");//取余杭卫生局的key

                string sHtmlText = Submit.BuildRequest(sParaTemp);
                OutObject = new ZHIFUBAOTF_OUT();
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
                                        if (mxnode.Name == "fund_change")
                                        {
                                            OutObject.FUNDCHANGE = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "refund_fee")
                                        {
                                            OutObject.REFUNDFEE = mxnode.InnerText;
                                        }
                                    }

                                }

                            }

                        }
                    }
                    //if (OutObject.RESULTCODE.ToUpper() == "SUCCESS")
                    //{
                    //    DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00015, InObject.WIDOUTREQUESTNO, "3",
                    //               InObject.BASEINFO.CAOZUOYXM, InObject.BASEINFO.CAOZUOYDM, InObject.WIDOUTTRADENO));
                    //    //更新付记录状态 退费成功
                    //    // DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set JIESUANZT='3' where IOUTTRADENO='{0}' ", InObject.WIDOUTREQUESTNO));
                    //    //更新正记录 冲销标志
                    //    DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set CXBZ='1' where IOUTTRADENO='{0}' ", InObject.WIDOUTTRADENO));

                    //}
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message.ToString());
                }

            }
            #endregion
            else
            {
                throw new Exception("退费类型不正确!");
            }
        }
    }
}
