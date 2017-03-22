using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Runtime.Remoting.Messaging;
using SWSoft.Framework;

namespace FSDYY.Biz
{
    public class Unity
    {

        /** 
         * 替换一个字符串中的某些指定字符 
         * @param strData String 原始字符串 
         * @param regex String 要替换的字符串 
         * @param replacement String 替代字符串 
         * @return String 替换后的字符串 
         */
        public static String replaceString(String strData, String regex, String replacement)
        {
            if (strData == null)
            {
                return null;
            }
            int index;
            index = strData.IndexOf(regex);
            String strNew = "";
            if (index >= 0)
            {
                while (index >= 0)
                {
                    strNew += strData.Substring(0, index) + replacement;
                    strData = strData.Substring(index + regex.Length);
                    index = strData.IndexOf(regex);
                }
                strNew += strData;
                return strNew;
            }
            return strData;
        }

        /** 
         * 替换字符串中特殊字符 
         */
        public static String encodeString(String strData)
        {
            if (strData == null)
            {
                return "";
            }
            strData = replaceString(strData, "&", "＆");
            strData = replaceString(strData, "<", "＜;");
            strData = replaceString(strData, ">", "＞;");
            strData = replaceString(strData, "';", "＇");
            strData = replaceString(strData, "\"", "＂;");
            return strData;
        }

        /** 
         * 还原字符串中特殊字符 
         */
        public static String decodeString(String strData)
        {
            strData = replaceString(strData, "&lt;", "<");
            strData = replaceString(strData, "&gt;", ">");
            strData = replaceString(strData, "&apos;", "&apos;");
            strData = replaceString(strData, "&quot;", "\"");
            strData = replaceString(strData, "&amp;", "&");
            return strData;
        }
    }
}
