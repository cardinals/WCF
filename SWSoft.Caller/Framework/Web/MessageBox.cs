using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Net.Mail;
using System.IO;

namespace SWSoft.Framework
{
    /// <summary>
    /// 消息提示的各种方法
    /// </summary>
    public class HtmlBox
    {
        /// <summary>
        /// 让页面执行一段JS
        /// </summary>
        /// <param name="strCode">要执行的JS</param>
        public static void CallJavascript(string strCode)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append(strCode.Trim());
            sb.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), sb.ToString());
        }

        /// <summary>
        /// 页面加载完成后提示脚本信息
        /// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="str">要显示的信息</param>
        public static void ShowAfter(System.Web.UI.Page page, string str)
        {
            CallJavascript(string.Format("window.onload=function(){{alert(\"{0}\");}}", str));
        }

        /// <summary>
        /// 页面加载完成后提示脚本信息,然后跳转到指定页
        /// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="str">要显示的信息</param>
        /// <param name="url">跳转页路径</param>
        public static void ShowHref(System.Web.UI.Page page, string str, string url)
        {
            CallJavascript(string.Format("window.onload=function(){{alert(\"{0}\");location.href='{1}'}}", str, url));
        }

        /// <summary>
        /// 显示一个弹出窗口，并转向目标页
        /// </summary>
        /// <param name="str">要显示的信息</param>
        /// <param name="url">要跳转的页面</param>
        public static void ShowGoBack(string str, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\"> \n");
            sb.Append("alert(\"" + str.Trim() + "\"); \n");
            sb.Append("window.location.href=\"" + url.Trim() + "\";\n");
            sb.Append("</script>");

            System.Web.HttpContext.Current.Response.Write(sb.ToString());
            System.Web.HttpContext.Current.Response.End();
        }

        public static void ShowConfirmHref(string msg, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if(confirm(\"" + msg + "\"))");
            sb.Append("{");
            sb.Append("location.href=\"" + url + "\";");
            sb.Append("}");
            CallJavascript(sb.ToString());
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">接收人Email</param>
        /// <param name="title">标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="error">返回错误信息</param>
        /// <returns></returns>
        public static bool SendMail(string to, string title, string body, out string error)
        {
            MailMessage mailMessage = new MailMessage();
            foreach (var item in to.Split(';'))
            {
                mailMessage.To.Add(item);
            }
            mailMessage.From = new System.Net.Mail.MailAddress("service@fenglaijun.com", "清风网络客服中心");//系统以哪个Email发送邮件
            mailMessage.Subject = title;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            //test
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, "12fenglaijun");//设置发件人身份的票据  
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Host = "smtp.exmail.qq.com";
            try
            {
                smtpClient.Send(mailMessage);
                error = string.Empty;
                return true;
            }
            catch (SmtpException smtp)
            {
                error = smtp.Message;
                return false;
            }
        }
    }
}
