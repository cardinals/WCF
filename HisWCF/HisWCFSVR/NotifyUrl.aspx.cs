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
                if (verifyResult)//验证成功
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

                    //判断是否在商户网站中已经做过了这次通知返回的处理
                    //如果没有做过处理，那么执行商户的业务程序
                    //如果有做过处理，那么不执行商户的业务程序
                    Response.Write("success");  //请不要修改或删除
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
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
    }
}