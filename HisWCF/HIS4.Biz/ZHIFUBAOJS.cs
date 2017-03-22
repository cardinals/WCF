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
using System.Xml;
using System.Runtime.Remoting.Contexts;
using System.Web.Hosting;
using System.Web;
using System.IO;

namespace HIS4.Biz
{
    public class ZHIFUBAOJS : IMessage<ZHIFUBAOJS_IN, ZHIFUBAOJS_OUT>
    {
        /// <summary>
        /// 支付宝结算
        /// </summary>
        public override void ProcessMessage()
        {

            var listpbxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00002, InObject.WIDOUTTRADENO));
            LogUnit.Write(listpbxx.Rows.Count.ToString());
            if (listpbxx.Rows.Count > 0)
            {
                throw new Exception("商户订单号不能重复!");
            }

            #region //手机网页支付
            if (InObject.JIESUANLX == "1")
            {
                //支付宝网关地址
                string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";
                ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

                //返回格式
                string format = "xml";
                //必填，不需要修改

                //返回格式
                string v = "2.0";
                //必填，不需要修改

                if (string.IsNullOrEmpty(InObject.QINGQIUDH))
                {
                    throw new Exception("请求单号不能为空！");
                }
                //请求号
                string req_id = InObject.QINGQIUDH; //DateTime.Now.ToString("yyyyMMddHHmmss");
                //必填，须保证每次请求都是唯一

                //req_data详细信息
                if (string.IsNullOrEmpty(InObject.NOTIFYURL))
                {
                    throw new Exception("服务器异步通知页面路径不能为空！");
                }
                //服务器异步通知页面路径
                string notify_url = InObject.NOTIFYURL;//"http://商户网关地址/WS_WAP_PAYWAP-CSHARP-UTF-8/notify_url.aspx";
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                if (string.IsNullOrEmpty(InObject.CALLBACKURL))
                {
                    throw new Exception("页面跳转同步通知页面路径不能为空！");
                }
                //页面跳转同步通知页面路径
                string call_back_url = InObject.CALLBACKURL;//"http://127.0.0.1:64704/WS_WAP_PAYWAP-CSHARP-UTF-8/call_back_url.aspx";
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                if (string.IsNullOrEmpty(InObject.MERCHANTURL))
                {
                    throw new Exception("操作中断返回地址不能为空！");
                }
                //操作中断返回地址
                string merchant_url = InObject.MERCHANTURL;// "http://127.0.0.1:64704/WS_WAP_PAYWAP-CSHARP-UTF-8/xxxxx.aspx";
                //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

                if (string.IsNullOrEmpty(InObject.WIDOUTTRADENO))
                {
                    throw new Exception("商户订单号不能为空！");
                }
                //商户订单号
                string out_trade_no = InObject.WIDOUTTRADENO;// WIDout_trade_no.Text.Trim();
                //商户网站订单系统中唯一订单号，必填

                if (string.IsNullOrEmpty(InObject.WIDSUBJECT))
                {
                    throw new Exception("订单名称不能为空！");
                }
                //订单名称
                string subject = InObject.WIDSUBJECT;//WIDsubject.Text.Trim();
                //必填

                if (string.IsNullOrEmpty(InObject.WIDTOTALFEE))
                {
                    throw new Exception("付款金额不能为空！");
                }
                //付款金额
                string total_fee = InObject.WIDTOTALFEE; //WIDtotal_fee.Text.Trim();
                //必填

                //请求业务参数详细
                string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url +
                                       "</notify_url><call_back_url>" + call_back_url +
                                       "</call_back_url><seller_account_name>" + Common.Alipay.Config.Seller_email +
                                       "</seller_account_name><out_trade_no>" + out_trade_no +
                                       "</out_trade_no><subject>" + subject +
                                       "</subject><total_fee>" + total_fee +
                                       "</total_fee><merchant_url>" + merchant_url +
                                       "</merchant_url></direct_trade_create_req>";
                //必填

                //把请求参数打包成数组
                Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
                sParaTempToken.Add("partner", Config.Partner);
                sParaTempToken.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTempToken.Add("sec_id", Config.Sign_type.ToUpper());
                sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
                sParaTempToken.Add("format", format);
                sParaTempToken.Add("v", v);
                sParaTempToken.Add("req_id", req_id);
                sParaTempToken.Add("req_data", req_dataToken);

                //建立请求
                string sHtmlTextToken = Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
                //URLDECODE返回的信息
                Encoding code = Encoding.GetEncoding(Config.Input_charset);
                sHtmlTextToken = System.Web.HttpUtility.UrlDecode(sHtmlTextToken, code);

                //解析远程模拟提交后返回的信息
                Dictionary<string, string> dicHtmlTextToken = Submit.ParseResponse(sHtmlTextToken);

                //获取token
                string request_token = dicHtmlTextToken["request_token"];


                ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////

                //业务详细
                string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
                //必填

                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00001,
                 InObject.QINGQIUDH,        //请求单号
                 '1',                      //结算类型 1手机网页支付 2二维码扫描支付 3商户代扣
                 InObject.NOTIFYURL,       //服务器异步通知页面路径
                 InObject.CALLBACKURL,     //页面跳转同步通知页面路径
                 InObject.MERCHANTURL,     //操作中断返回地址)
                 InObject.WIDOUTTRADENO,   //商户订单号
                 InObject.WIDSUBJECT,      //订单名称
                 InObject.WIDTOTALFEE,     //订单金额
                 InObject.WIDBODY,         //订单描述
                 InObject.OUTTIME,         //超时时间取值范围
                 "",                       //买家支付宝用户号
                 "",                       //买家支付宝帐号
                 "",                       //授权号
                 "",                        //协议号
                 "0",                      //结算状态 0未结算 1结算成功 2结算失败
                 request_token));

                //把请求参数打包成数组
                Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("sec_id", Config.Sign_type.ToUpper());
                sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
                sParaTemp.Add("format", format);
                sParaTemp.Add("v", v);
                sParaTemp.Add("req_data", req_data);

                //建立请求
                string sHtmlText = Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
                OutObject = new ZHIFUBAOJS_OUT();
                OutObject.JIESUANURL = sHtmlText;
            }
            #endregion
            #region 二维码支付
            else if (InObject.JIESUANLX == "2")
            {
                if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
                {
                    throw new Exception("医院代码不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.BASEINFO.CAOZUOYDM))
                {
                    throw new Exception("操作员代码不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.BASEINFO.CAOZUOYXM))
                {
                    throw new Exception("操作员姓名不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.JIUZHENKH))
                {
                    throw new Exception("就诊卡号不能为空!");
                }
                DataTable dt = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00006, InObject.BASEINFO.FENYUANDM));
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("查询不到医院代码为【" + InObject.BASEINFO.FENYUANDM + "】的医院信息");
                }
                var ddxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00002, InObject.WIDOUTTRADENO));
                if (ddxx.Rows.Count > 0)
                {
                    throw new Exception("已存在相同订单号的结算信息!");
                }
                if (string.IsNullOrEmpty(InObject.NOTIFYURL))
                {
                    throw new Exception("异步请求地址不能为空!");
                }
                //服务器异步通知页面路径
                string notify_url = ConfigurationManager.AppSettings["FWPTURL"] + "NotifyUrl2.aspx";//InObject.NOTIFYURL;
                //需http://格式的完整路径，不能加?id=123这类自定义参数
                //商户订单号
                string out_trade_no = InObject.WIDOUTTRADENO;// WIDout_trade_no.Text.Trim();
                //商户网站订单系统中唯一订单号，必填

                //订单名称
                string subject = InObject.WIDSUBJECT;// WIDsubject.Text.Trim();
                //必填

                //订单业务类型
                string product_code = "QR_CODE_OFFLINE";// WIDproduct_code.Text.Trim();
                //目前只支持QR_CODE_OFFLINE（二维码支付），必填

                //付款金额
                string total_fee = InObject.WIDTOTALFEE;// WIDtotal_fee.Text.Trim();
                //必填

                //卖家支付宝帐户
                string seller_email = dt.Rows[0]["SELLEREMAIL"].ToString(); //WIDseller_email.Text.Trim();
                //必填

                //订单描述
                string body = InObject.WIDBODY;

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "alipay.acquire.precreate");
                sParaTemp.Add("notify_url", notify_url);
                sParaTemp.Add("out_trade_no", out_trade_no);
                sParaTemp.Add("subject", subject);
                sParaTemp.Add("product_code", product_code);
                sParaTemp.Add("total_fee", total_fee);
                sParaTemp.Add("seller_email", seller_email);
                sParaTemp.Add("body", body);
                sParaTemp.Add("it_b_pay", InObject.OUTTIME);
                sParaTemp.Add("key", "rfk3sw6dlq28k2tijjj54le6lfhw73s4");//取余杭卫生局的key
                //将请求数据 插入数据库
                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00001,
               InObject.WIDOUTTRADENO,        //商户订单号
               '2',                      //结算类型 1手机网页支付 2二维码扫描支付 3商户代扣
               InObject.NOTIFYURL,       //服务器异步通知页面路径
                "",     //页面跳转同步通知页面路径
               "",     //操作中断返回地址)
               InObject.WIDSUBJECT,      //订单名称
               InObject.WIDTOTALFEE,     //订单金额
               InObject.WIDBODY,         //订单描述
               InObject.OUTTIME,         //超时时间取值范围
               InObject.BUYERID,                       //买家支付宝用户号
               InObject.BUYEREMAIL,                     //买家支付宝帐号
               InObject.AUTHNO,                       //授权号
               InObject.AGREENNO,                        //协议号 
               "0",                      //结算状态 0未结算 1结算成功 2结算失败
               "",
               InObject.BASEINFO.FENYUANDM,//分院代码
               dt.Rows[0]["SELLEREMAIL"].ToString(),//收款帐号
               InObject.BASEINFO.CAOZUOYXM,
               InObject.BASEINFO.CAOZUOYDM,
               "",//身份证号
               InObject.JIUZHENKH//就诊卡号
               ));

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);
                //记录日志
                LogUnit.Write("JIESUANLX == 2###" + sHtmlText, "ZHIFUBAOJS");
                OutObject = new ZHIFUBAOJS_OUT();
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    string PicUrl = "";
                    xmlDoc.LoadXml(sHtmlText);
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("alipay").ChildNodes;
                    foreach (XmlNode xn in nodeList)
                    {
                        //判断是否异常,如果返回异常 则抛出
                        //if (xn.Name == "is_success" && xn.InnerText == "F")
                        //{
                        //    throw new Exception("支付宝请求生成二维码失败!");
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
                                        if (mxnode.Name == "big_pic_url")
                                        {
                                            //   OutObject.PICURL = mxnode.InnerText;
                                            PicUrl = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "out_trade_no")
                                        {
                                            OutObject.WIDOUTTRADENO = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "qr_code")
                                        {
                                            OutObject.QRCODE = mxnode.InnerText;
                                        }

                                    }
                                    try
                                    {//将二维码图片下载到服务平台本地
                                        var path = HostingEnvironment.ApplicationPhysicalPath + "ZFBImg\\";
                                        DeletePic(path);
                                        System.Net.WebClient webClient = new System.Net.WebClient();
                                        webClient.DownloadFile(PicUrl, path + OutObject.WIDOUTTRADENO + ".jpg");
                                        OutObject.PICLOCAL = OutObject.WIDOUTTRADENO + ".jpg";
                                    }
                                    catch
                                    {

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
            #endregion
            #region 签约账户支付 商户代扣
            else if (InObject.JIESUANLX == "3")
            {

                if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
                {
                    throw new Exception("医院代码不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.BASEINFO.CAOZUOYDM))
                {
                    throw new Exception("操作员代码不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.BASEINFO.CAOZUOYXM))
                {
                    throw new Exception("操作员姓名不能为空!");
                }
                if (string.IsNullOrEmpty(InObject.JIUZHENKH))
                {
                    throw new Exception("就诊卡号不能为空!");
                }
                DataTable dt = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00006, InObject.BASEINFO.FENYUANDM));
                if (dt.Rows.Count <= 0)
                {
                    throw new Exception("查询不到医院代码为【" + InObject.BASEINFO.FENYUANDM + "】的医院信息");
                }

                var ddxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00002, InObject.WIDOUTTRADENO));
                if (ddxx.Rows.Count > 0)
                {
                    throw new Exception("已存在相同订单号的结算信息!");
                }

                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "alipay.acquire.createandpay");
                sParaTemp.Add("notify_url", InObject.NOTIFYURL);
                sParaTemp.Add("seller_email", dt.Rows[0]["SELLEREMAIL"].ToString());//Config.Seller_email);
                //sParaTemp.Add("seller_id", "2088121042783039");
                sParaTemp.Add("out_trade_no", InObject.WIDOUTTRADENO);//InObject.WIDOUTTRADENO
                sParaTemp.Add("subject", InObject.WIDSUBJECT);
                sParaTemp.Add("product_code", "GENERAL_WITHHOLDING");
                sParaTemp.Add("total_fee", InObject.WIDTOTALFEE);
                sParaTemp.Add("agreement_info", "{\"agreement_no\":\"" + InObject.AGREENNO + "\"}");//协议号 必传 签约后传回
                sParaTemp.Add("key", "rfk3sw6dlq28k2tijjj54le6lfhw73s4");//取余杭卫生局的key
                // sParaTemp.Add("royalty_type", "ROYALTY");
                // sParaTemp.Add("buyer_id", InObject.BUYERID);
                // sParaTemp.Add("buyer_email", InObject.BUYEREMAIL);
                // sParaTemp.Add("body", InObject.WIDBODY);
                //sParaTemp.Add("it_b_pay", InObject.OUTTIME);
                //sParaTemp.Add("dynamic_id_type", "bar_code");//动态ID类型
                //sParaTemp.Add("dynamic_id", "kff3hjwqzxrbvrrkd0");//动态ID
                //  sParaTemp.Add("auth_no", InObject.AUTHNO);

                string sfzh = "";
                DataTable qyxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00021, InObject.AGREENNO));
                if (qyxx.Rows.Count >= 0)
                {
                    sfzh = qyxx.Rows[0]["SHENFENZH"].ToString();
                }
                //将请求数据 插入数据库
                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00001,
               InObject.WIDOUTTRADENO,        //商户订单号
               '3',                      //结算类型 1手机网页支付 2二维码扫描支付 3商户代扣
               "",       //服务器异步通知页面路径
                "",     //页面跳转同步通知页面路径
               "",     //操作中断返回地址)
               InObject.WIDSUBJECT,      //订单名称
               InObject.WIDTOTALFEE,     //订单金额
               InObject.WIDBODY,         //订单描述
               InObject.OUTTIME,         //超时时间取值范围
               InObject.BUYERID,                       //买家支付宝用户号
               InObject.BUYEREMAIL,                     //买家支付宝帐号
               InObject.AUTHNO,                       //授权号
               InObject.AGREENNO,                        //协议号 
               "0",                      //结算状态 0未结算 1结算成功 2结算失败
               "",
               InObject.BASEINFO.FENYUANDM,//分院代码
               dt.Rows[0]["SELLEREMAIL"].ToString(),//收款帐号
               InObject.BASEINFO.CAOZUOYXM,
               InObject.BASEINFO.CAOZUOYDM,
               sfzh,
               InObject.JIUZHENKH
               ));

                LogUnit.Write("开始请求支付宝!");
                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);
                //记录日志
                LogUnit.Write("JIESUANLX == 3###" + sHtmlText, "ZHIFUBAOJS");
                OutObject = new ZHIFUBAOJS_OUT();
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(sHtmlText);
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("alipay").ChildNodes;
                    foreach (XmlNode xn in nodeList)
                    {
                        if (xn.Name == "error")
                        {
                            DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set FAILERR='{0}',JIESUANZT='-1' where IOUTTRADENO='{1}' ", xn.InnerText, InObject.WIDOUTTRADENO));
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
                                            throw new Exception(mxnode.InnerText);
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
                                            OutObject.WIDOUTTRADENO = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "buyer_user_id")
                                        {
                                            OutObject.BUYERID = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "buyer_logon_id")
                                        {
                                            OutObject.BUYEREMAIL = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "total_fee")
                                        {
                                            OutObject.TOTALFREE = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "gmt_payment")
                                        {
                                            OutObject.PAYMENTTIME = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "extend_info")
                                        {
                                            OutObject.EXTENDINFO = mxnode.InnerText;
                                        }
                                        if (mxnode.Name == "fund_bill_list")
                                        {
                                            OutObject.FUNDBILLLIST = mxnode.InnerText;
                                        }
                                    }

                                }

                            }

                        }
                    }
                    if (OutObject.RESULTCODE.ToUpper() == "ORDER_SUCCESS_PAY_SUCCESS")
                    {
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set TRADENO='{0}',JIESUANZT='1',JIESUANSJ= sysdate,IBUYERID='{1}',IBUYEREMAIL='{2}'  where IOUTTRADENO='{3}' ", OutObject.TRADENO, OutObject.BUYERID, OutObject.BUYEREMAIL, InObject.WIDOUTTRADENO));

                    }
                    else
                    {
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set FAILERR='{0}',JIESUANZT='-1',IBUYERID='{1}',IBUYEREMAIL='{2}'  where IOUTTRADENO='{3}' ", OutObject.ERRORDES, OutObject.BUYERID, OutObject.BUYEREMAIL, InObject.WIDOUTTRADENO));

                    }

                }
                catch (Exception ep)
                {
                    LogUnit.Write(ep.Message);
                    throw new Exception(ep.Message);
                }

            }
            #endregion
            else
            {
                throw new Exception("支付类型不正确!");
            }
        }
        public void DeletePic(string path)
        {
            string[] list = this.recursed(path, new string[] { "*.jpg", "*.png", "*.gif" });
            foreach (string str in list)
            {
                FileInfo file = new FileInfo(str);

                TimeSpan ts1 = new TimeSpan(file.CreationTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts3 = ts1.Subtract(ts2).Duration();
                //删除大于7天的图片
                int ss = ts3.Days;
                if (ss > 7)
                {
                    //判断文件是不是存在
                    if (File.Exists(str))
                    {
                        //如果存在则删除
                        File.Delete(str);
                    }
                }

            }
        }
        public string[] recursed(string path, string[] patterns)
        {
            string[] arrList = new string[0];

            foreach (string str in patterns)
            {
                string[] list = Directory.GetFiles(path, str, SearchOption.AllDirectories);

                if (list != null)
                {
                    string[] temp = arrList;
                    arrList = new string[arrList.Length + list.Length];

                    Array.Copy(temp, 0, arrList, 0, temp.Length);
                    Array.Copy(list, 0, arrList, temp.Length, list.Length);
                }
            }

            return arrList;
        }
    }

}