using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace Common
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogUnit
    {
        private static string GetLogFileName(string Filename)
        {
            try
            {
                if (string.IsNullOrEmpty(Filename))
                {
                    return string.Format(@"{0}\LOGS\{1}\{1:yyyyMMdd}.log", HostingEnvironment.ApplicationPhysicalPath, DateTime.Now.ToString("yyyyMMdd"));

                }
                else
                {
                    return string.Format(@"{0}\LOGS\{1}\{2}\{1:yyyyMMdd}.log", HostingEnvironment.ApplicationPhysicalPath, DateTime.Now.ToString("yyyyMMdd"), Filename);
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 记录日志方法
        /// </summary>
        /// <param name="Content">日志内容</param>
        /// <param name="JIEKOUMC">接口名称</param>
        public static void Write(string Content, string JIEKOUMC = "")
        {
            try
            {
                string filename = GetLogFileName(JIEKOUMC);
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
                using (StreamWriter m_streamWriter = File.AppendText(filename))
                {
                    m_streamWriter.Flush();
                    m_streamWriter.WriteLine("***************************************************************");
                    m_streamWriter.WriteLine(DateTime.Now.ToString() + "###" + Content);
                    m_streamWriter.Flush();
                    m_streamWriter.Dispose();
                    m_streamWriter.Close();
                }
            }
            catch
            {

            }
        }
    }
}
