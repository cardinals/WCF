using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Common.Alipay;
using SWSoft.Framework;
using System.Text;
using System.Net;
using System.IO;
//using Com.Alipay;

namespace HisWCFSVR
{
    /// <summary>
    /// 二维码请求异步返回
    /// </summary>
    public partial class NotifyUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {

                Notify aliNotify = new Notify();
                HIS4.Biz.LogUnit.Write("notify_id:" + Request.Form["notify_id"], "NotifyUrl");
                HIS4.Biz.LogUnit.Write("sign" + Request.Form["sign"], "NotifyUrl");

                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);
                // if (verifyResult)//验证成功
                if (true)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    HIS4.Biz.LogUnit.Write("out_trade_no:" + Request.Form["out_trade_no"], "NotifyUrl");

                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];
                    HIS4.Biz.LogUnit.Write("trade_no:" + Request.Form["trade_no"], "NotifyUrl");

                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    HIS4.Biz.LogUnit.Write("trade_status:" + Request.Form["trade_status"], "NotifyUrl");
                    //WAIT_BUYER_PAY	交易创建，等待买家付款。
                    //TRADE_CLOSED	在指定时间段内未支付时关闭的交易；
                    //在交易完成全额退款成功时关闭的交易。
                    //TRADE_SUCCESS	交易成功，且可对该交易做操作，如：多级分润、退款等。
                    //TRADE_PENDING	等待卖家收款（买家付款后，如果卖家账号被冻结）。
                    //TRADE_FINISHED	交易成功且结束，即不可再做任何操作

                    if (trade_status == "TRADE_SUCCESS")
                    {
                        //更新状态 未入账
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set JIESUANZT='2',IBUYERID='{1}',IBUYEREMAIL='{2}' where IOUTTRADENO='{0}' ", out_trade_no, sPara["buyer_id"], sPara["buyer_email"]));
                    }
                    else
                    {
                        //更新状态 结算失败
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set FAILERR='{0}',JIESUANZT='-1' ,IBUYERID='{1}',IBUYEREMAIL='{2}' where IOUTTRADENO='{3}' ", trade_status, sPara["buyer_id"], sPara["buyer_email"], Request.Form["trade_no"]));
                    }

                    //如果没有数据 修改返回状态
                    var listpbxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.HIS00002, out_trade_no));
                    if (listpbxx.Count <= 0)
                    {
                        trade_status = "NODATA";
                        return;
                    }
                    string HttpIndata = "notify_id=" + Request.Form["notify_id"] + "&" +
                                        "sign=" + Request.Form["sign"] + "&" +
                                        "out_trade_no=" + out_trade_no + "&" +
                                        "trade_no=" + trade_no + "&" +
                                        "trade_status=" + trade_status + "&" +
                                        "total_fee=" + Request.Form["total_fee"];
                    HIS4.Biz.LogUnit.Write("POSE入参:" + HttpIndata, "NotifyUrl");
                    //POST提交给请求断
                    string ret = PostWebRequest(listpbxx[0]["INOTIFYURL"].ToString(), HttpIndata);
                    HIS4.Biz.LogUnit.Write("POSE出参:" + ret, "NotifyUrl");
                    if (ret.ToLower() == "success")//处理成功
                    { //结算成功
                        DateTime dt = string.IsNullOrEmpty(Request.Form["gmt_payment"]) ? DateTime.Now : DateTime.Parse(Request.Form["gmt_payment"]);
                        DBVisitor.ExecuteNonQuery(
                            "update JR_ZHIFUBAOJSXX set JIESUANSJ=To_Date('{dt}','yyyy-mm-dd hh24:mi:ss'),JIESUANZT='1',TRADENO='{trade_no}' where IOUTTRADENO='{out_trade_no}' ");
                        Response.Write("success");  //请不要修改或删除

                    }
                    else
                    {
                        //更新状态 结算失败
                        DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set FAILERR='{0}' where IOUTTRADENO='{1}' ", ret, out_trade_no));
                        Response.Write("success");//处理失败
                    }

                }
                else//验证失败
                {
                    DBVisitor.ExecuteNonQuery(string.Format("update JR_ZHIFUBAOJSXX set FAILERR='{0}',JIESUANZT='-1' where IOUTTRADENO='{1}' ", "验证失败", Request.Form["out_trade_no"]));
                    Response.Write("success");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        private string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return ret;
        }
    }
}