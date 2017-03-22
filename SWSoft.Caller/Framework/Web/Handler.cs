using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace SWSoft.Framework.Web
{
    /// <summary>
    /// 通过实现 System.Web.IHttpHandler 接口的自定义 HttpHandler 启用 HTTP Web 请求的处理。
    ///   <para> $(function () {</para>
    ///   <para>  $.getJSON("cgi-bin/httpd.ashx", { op: "GetAllArea" }, function (json) {</para>
    ///   <para>     for (var i in json) {</para>
    ///   <para>          $("#files").append("<tr><td>" + json[i].AreaName + "</td></tr>");</para>
    ///   <para>             }</para>
    ///   <para>         });</para>
    ///   <para> });</para>   
    /// </summary>
    public class Handler : DBVisitor<Entry>, IHttpHandler, IRequiresSessionState
    {
        protected HttpRequest Request { get; set; }
        protected HttpResponse Response { get; set; }
        protected HttpSessionState Session { get; set; }
        protected string ContentType { get; set; }
        public Handler()
        {
            Request = HttpContext.Current.Request;
            Response = HttpContext.Current.Response;
            Session = HttpContext.Current.Session;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            Invoke();
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

        public string Invoke(NameValueCollection items)
        {
            string objName = items["obj"];
            string op = items["op"];
            //未指定名称时默认为当前对象
            objName = objName == null ? GetType().FullName : (GetType().Namespace + "." + objName);
            Type type = Assembly.GetAssembly(this.GetType()).GetType(objName, false, true);//获取对象类型
            object obj = Activator.CreateInstance(type);//创建对象            
            if (op != null)
            {
                MethodInfo method = type.GetMethod(op);
                if (method != null)
                {
                    List<object> list = new List<object>();
                    foreach (var item in method.GetParameters())
                    {
                        //将参数列表转换成方法的相应参数类型
                        MethodInfo parse = item.ParameterType.GetMethod("Parse", new Type[] { typeof(String) });//获取Type的Parse方法
                        if (parse == null)
                        {
                            list.Add(items[item.Name]);
                        }
                        else
                        {
                            //执行数据类型的Parse方法
                            list.Add(parse.Invoke(null, new object[] { items[item.Name] }));
                        }
                    }
                    Response.ContentType = ContentType;
                    try
                    {
                        return method.Invoke(obj, list.Count == 0 ? null : list.ToArray()).ToString();
                    }
                    catch (Exception e)
                    {
                        return string.Format("{{\r\n\"error\":\"{0}\",\r\n\"source\":\"{1}\"\r\n}}", e.InnerException.Message, e.InnerException.StackTrace);
                    }
                }
                return string.Format("Not find method {0} in {1}", op, type.FullName);
            }
            return string.Format("Not find object {0}", type.FullName);
        }

        protected string GetErrorString(string value)
        {
            switch (Response.ContentType)
            {
                case "text/xml": return "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" +
                              "<callresult>" +
                            HttpUtility.HtmlEncode(value) + "\r\n" +
                              "</callresult>";
                case "text/javascript": return "{'error':'" + HttpUtility.HtmlEncode(value) + "'}";
                case "text/html": return value;
                default: return value;
            }
        }
    }
}
