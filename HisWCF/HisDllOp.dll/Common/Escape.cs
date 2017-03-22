using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class Escape
    {
        public static string Do(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string Undo(string s)
        {
            string str = s.Remove(0, 2);//删除最前面两个＂%u＂
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码
            return str;
        }
    }
}