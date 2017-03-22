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
    /// ��Ϣ��ʾ�ĸ��ַ���
    /// </summary>
    public class HtmlBox
    {
        /// <summary>
        /// ��ҳ��ִ��һ��JS
        /// </summary>
        /// <param name="strCode">Ҫִ�е�JS</param>
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
        /// ҳ�������ɺ���ʾ�ű���Ϣ
        /// </summary>
        /// <param name="page">ҳ�����</param>
        /// <param name="str">Ҫ��ʾ����Ϣ</param>
        public static void ShowAfter(System.Web.UI.Page page, string str)
        {
            CallJavascript(string.Format("window.onload=function(){{alert(\"{0}\");}}", str));
        }

        /// <summary>
        /// ҳ�������ɺ���ʾ�ű���Ϣ,Ȼ����ת��ָ��ҳ
        /// </summary>
        /// <param name="page">ҳ�����</param>
        /// <param name="str">Ҫ��ʾ����Ϣ</param>
        /// <param name="url">��תҳ·��</param>
        public static void ShowHref(System.Web.UI.Page page, string str, string url)
        {
            CallJavascript(string.Format("window.onload=function(){{alert(\"{0}\");location.href='{1}'}}", str, url));
        }

        /// <summary>
        /// ��ʾһ���������ڣ���ת��Ŀ��ҳ
        /// </summary>
        /// <param name="str">Ҫ��ʾ����Ϣ</param>
        /// <param name="url">Ҫ��ת��ҳ��</param>
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
        /// �����ʼ�
        /// </summary>
        /// <param name="to">������Email</param>
        /// <param name="title">����</param>
        /// <param name="body">�ʼ�����</param>
        /// <param name="error">���ش�����Ϣ</param>
        /// <returns></returns>
        public static bool SendMail(string to, string title, string body, out string error)
        {
            MailMessage mailMessage = new MailMessage();
            foreach (var item in to.Split(';'))
            {
                mailMessage.To.Add(item);
            }
            mailMessage.From = new System.Net.Mail.MailAddress("service@fenglaijun.com", "�������ͷ�����");//ϵͳ���ĸ�Email�����ʼ�
            mailMessage.Subject = title;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            //test
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, "12fenglaijun");//���÷�������ݵ�Ʊ��  
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
