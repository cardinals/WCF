using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Reflection;

namespace SWSoft.Framework.Web
{
    /// <summary>
    /// 经过改造的从 ASP.NET Web 应用程序的宿主服务器请求的 .aspx 文件。
    /// </summary>
    public class BasePage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Invoke())
            {
                base.OnLoad(e);
            }
        }

        /// <summary>
        /// 通过URL调用后台方法http://localhost/getjson
        /// </summary>
        protected virtual bool Invoke()
        {
            if (!string.IsNullOrEmpty(Request.PathInfo))
            {
                var strs = Request.PathInfo.Split('/');
                if (strs.Length < 2)
                {
                    return true;
                }
                object json = "";
                try
                {
                    var method = this.GetType().GetMethod(strs[1]);
                    var list = new List<object>();
                    foreach (var item in method.GetParameters())
                    {
                        list.Add(Request[item.Name]);
                    }
                    json = method.Invoke(this, list.ToArray()) ?? "";
                }
                catch (Exception ex)
                {
                    var exobj = ex.InnerException ?? ex;
                    json = GetErrorString(ex.Message);
                }
                if (Response.ContentType == "text/html")
                {
                    Response.ContentType = "txt/javascript;charset=utf-8;";
                }
                Response.Write(json.ToString().Trim().Replace("'", "\""));
                Response.End();
                return false;
            }
            return true;
        }

        protected string GetErrorString(string value)
        {
            switch (Response.ContentType)
            {
                case "text/xml": return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" +
                              "<callresult>" +
                            Server.HtmlEncode(value) + "\r\n" +
                              "</callresult>";
                case "text/javascript": return "{'error':'" + Server.HtmlEncode(value) + "'}";
                case "text/html": return value;
                default: return value;
            }
        }
    }
}