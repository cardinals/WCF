using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WSEntity;
using System.Collections;

namespace Common.WSCall
{
    public class XMLHandle
    {
        static string top = "<?xml version='1.0' encoding='utf-8'?>";
        public static Entity XMLtoEntity<Entity>(string xml)
            where Entity : WSEntity.WSEntity, new()
        {
            Entity entity = new Entity();
            Type type = typeof(Entity);
            string[] xmls = xml.Replace("\r\n", string.Empty).Split(new char[]{'<', '>', '/',' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsGenericType)
                {
                    BuildEntity(entity, xmls, property);
                }
                else
                {
                    for(int i = 0; i < xmls.Length; i++)
                    {
                        if (xmls[i].ToUpper() == property.Name.ToUpper() && xmls[i + 1].ToUpper() != property.Name.ToUpper())
                        {
                            property.SetValue(entity, xmls[i + 1].Replace("\n", string.Empty), null);
                            break;
                        }
                    }
                }
            }
            return entity;
        }

        private static void BuildEntity(dynamic entity, string[] xmls, System.Reflection.PropertyInfo property)
        {
            var listType = typeof(List<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]);
            dynamic list = listType.GetConstructor(new Type[0]).Invoke(new object[0]);
            for (int i = 0; i < xmls.Length; i++)
            {
                if (xmls[i].ToUpper() == property.Name.ToUpper())
                {
                    Type gtype = property.PropertyType.GetGenericArguments()[0];
                    var newgroup = gtype.GetConstructor(new Type[0]).Invoke(new object[0]);
                    int twinsNameIndex = 0;
                    while (xmls[++i].ToUpper() != property.Name.ToUpper())
                    {
                        foreach (var gproperty in newgroup.GetType().GetProperties())
                        {
                            if (xmls[i].ToUpper() == gproperty.Name.ToUpper())
                            {
                                if (twinsNameIndex % 2 == 0)
                                {
                                    if (gproperty.PropertyType.IsGenericType)
                                    {
                                        //进入递归
                                        BuildEntity(newgroup, xmls, gproperty);
                                    }
                                    else
                                    {
                                        if (xmls[i + 1].ToUpper() != gproperty.Name.ToUpper())
                                        {
                                            gproperty.SetValue(newgroup, xmls[i + 1], null);
                                        }
                                    }
                                }
                                twinsNameIndex++;
                                break;
                            }
                        }
                    }
                    list.GetType().GetMethod("Add").Invoke(list, new object[] { newgroup });
                }
            }
            property.SetValue(entity, list, null);
        }

        public static string EntitytoXML<Entity>(Entity entity)
                    where Entity : WSEntity.WSEntity
        {
            StringBuilder body = new StringBuilder(512);
            body.AppendLine(top);
            Type type = entity.GetType();
            string title = type.BaseType.GetProperties()[0].Name;
            body.Append('<');
            body.Append(title);
            body.AppendLine(">");
            foreach (var property in type.GetProperties())
            {
                if (property.Name == title)
                    continue;
                BuildXML(entity, body, property);
            }
            body.Append("</");
            body.Append(title);
            body.Append(">");
            return body.ToString();
        }

        private static void BuildXML(dynamic entity, StringBuilder body, System.Reflection.PropertyInfo property)
        {
            if (property.PropertyType.IsGenericType)
            {
                foreach (var value in (IEnumerable)property.GetValue(entity, null))
                {
                    var groupName = value.GetType().Name;
                    body.Append('<');
                    body.Append(groupName);
                    body.AppendLine(">");
                    foreach (var g_property in value.GetType().GetProperties())
                    {
                        if (g_property.PropertyType.IsGenericType)
                        {
                            BuildXML(value, body, g_property);
                        }
                        else
                        {
                            body.Append('<');
                            body.Append(g_property.Name);
                            body.Append('>');
                            body.Append(g_property.GetValue(value, null) == null ? string.Empty : g_property.GetValue(value, null).ToString());
                            body.Append("</");
                            body.Append(g_property.Name);
                            body.AppendLine(">");
                        }
                    }
                    body.Append("</");
                    body.Append(groupName);
                    body.AppendLine(">");
                }
            }
            else
            {
                body.Append('<');
                body.Append(property.Name);
                body.Append('>');
                body.Append(property.GetValue(entity, null) == null ? string.Empty : property.GetValue(entity, null).ToString());
                body.Append("</");
                body.Append(property.Name);
                body.AppendLine(">");
            }
        }
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
    }
}
