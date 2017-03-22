using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Web;

namespace SWSoft.Net
{
    public class Req
    {
        private CookieContainer ReqCookie { get; set; }
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }
        public HtmlDocument docu { get; set; }
        private string cookie;
        public Req()
        {

        }

        //public Req(HtmlDocument document)
        //{
        //    //ReqCookie = new CookieContainer();
        //    //foreach (var item in document.Cookie.Split(';'))
        //    //{
        //    //    string[] keyvalue = item.Split('=');
        //    //    Cookie ck = new Cookie(keyvalue[0].Trim(), HttpUtility.UrlEncode(keyvalue[1].Trim()));
        //    //    ck.Domain = document.Domain;
        //    //    ReqCookie.Add(ck);
        //    //}
        //    cookie = document.Cookie;
        //}

        public Req(CookieContainer container, HtmlDocument document)
        {
            ReqCookie = container;
            docu = document;
        }

        public string Post(string url, string args, string encoding = "utf-8")
        {
            Request = HttpWebRequest.Create(url) as HttpWebRequest;
            Request.CookieContainer = ReqCookie;
            //Request.Headers["cookie"] = cookie;
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            //byte[] buffer = Encoding.GetEncoding(encoding).GetBytes(args);
            using (StreamWriter sr = new StreamWriter(Request.GetRequestStream()))
            {
                sr.Write(args);
            }
            using (StreamReader sr = new StreamReader(Request.GetResponse().GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        public string Get(string url, string args, string encoding = "utf-8")
        {
            Request = HttpWebRequest.Create(url + "?" + args) as HttpWebRequest;
            Request.CookieContainer = ReqCookie;
            Request.Method = "GET";
            Request.Referer = docu.Url.AbsolutePath;
            Response = Request.GetResponse() as HttpWebResponse;
            List<byte> list = new List<byte>();
            using (StreamReader stream = new StreamReader(Response.GetResponseStream()))
            {
                while (!stream.EndOfStream)
                {
                    list.Add((byte)stream.Read());
                }
            }
            return Encoding.GetEncoding(Response.CharacterSet).GetString(list.ToArray());
        }
    }
}
