using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using SWSoft.Framework;

namespace HisWCFSVR
{
    public class ToolUnity
    {
        static Assembly Assemb { get; set; }
        static FileVersionInfo FileVer { get; set; }
        public static object LoadAssembly(string tradetype)
        {
            var tt = tradetype.Split('.');
            var dllpath = HostingEnvironment.MapPath("~") + "\\bin\\" + tt[0] + "\\" + tt[0] + ".Biz.dll";
            if (Assemb == null || FileVer.FileVersion != FileVersionInfo.GetVersionInfo(dllpath).FileVersion)
            {
                FileVer = FileVersionInfo.GetVersionInfo(dllpath);
                if (ConfigurationManager.AppSettings["enabledymc"] == "0")
                {
                    Assemb = Assembly.LoadFile(dllpath);
                }
                else
                {
                    Assemb = Assembly.LoadFile(dllpath);
                    //Assemb = Assembly.Load(File.ReadAllBytes(dllpath));
                }
            }
            return Assemb.CreateInstance(tradetype, true);
        }

        public static string ServiceERR(string tradetype, Exception ex)
        {
            var tt = tradetype.Split('.');
            var outxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<" + tt[1] + "_OUT>" +
            "<OUTMSG>" +
            "<ERRNO>-1</ERRNO>" +
            "<ERRMSG>" + ex.Message + "</ERRMSG>" +
            "<ERRMSGEX>" + (ex.InnerException ?? ex).StackTrace.Trim() + "</ERRMSGEX>" +
            "</" + tt[1] + "_OUT>";
            return outxml;
        }
        public static string ServiceERR(string tradetype, string ex)
        {
            var tt = tradetype.Split('.');
            var outxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<" + tt[1] + "_OUT>" +
            "<OUTMSG>" +
            "<ERRNO>-1</ERRNO>" +
            "<ERRMSG>" + ex + "</ERRMSG>" +
            "</" + tt[1] + "_OUT>";
            return outxml;
        }
        /// 星期转日期
        /// </summary>
        /// <param name="xq"></param>
        /// <returns></returns>
        public static DateTime RqtoXq(string xq)
        {
            var weekdays = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            var time = DateTime.Now;
            int Tdy = (int)time.DayOfWeek;
            if (Tdy == 0)
            {
                Tdy = 7;
            }
            int i = weekdays[Tdy - 1];
            int DicNum = 0;
            if (int.Parse(xq) - i < 0)
            {
                DicNum = 7 - System.Math.Abs(i - int.Parse(xq));
            }
            else if (int.Parse(xq) - i > 0)
            {
                DicNum = System.Math.Abs(i - int.Parse(xq));
            }
            else
            {
                return DateTime.Now.AddDays(7);
            }


            return DateTime.Now.AddDays(DicNum);
        }
        /// <summary>
        /// 判断是否节假日
        /// </summary>
        /// <param name="regDate"></param>
        /// <returns></returns>
        public static bool GetHoliday(DateTime regDate)
        {
            if ((int)regDate.DayOfWeek == 6)
            {
                string dynamicSql = "Select Decode(2,1,Csz1,Csz2) Value From Gy_Xtcs Where Csmc = 'SAT' And Xtxh = 23";
                DataTable dt = DBVisitor.ExecuteTable(dynamicSql);
                if (dt.Rows.Count > 0 && dt.Rows[0]["Value"].ToString() == "1")
                {
                    return true;

                }

            }
            else if ((int)regDate.DayOfWeek == 0)
            {
                string dynamicSql = "Select Decode(2,1,Csz1,Csz2) Value From Gy_Xtcs Where Csmc = 'SUN' And Xtxh = 23";
                DataTable dt = DBVisitor.ExecuteTable(dynamicSql);
                if (dt.Rows.Count > 0 && dt.Rows[0]["Value"].ToString() == "1")
                {
                    return true;

                }

            }
            else
            {
                string dynamicSql = $"SELECT COUNT(*) FROM gh_jjr WHERE RQ ='{regDate.ToString("MMdd")}'";
                DataTable dt = DBVisitor.ExecuteTable(dynamicSql);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }


            }
            return false;
        }
    }
}