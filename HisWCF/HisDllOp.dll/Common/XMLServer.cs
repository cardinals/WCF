using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MEDI.SIIM.SelfServiceWeb.Entity;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class XMLServer
    {
        static string top = "<?xml version='1.0' encoding='utf-8'?>";
        public static Entity XMLtoEntity<Entity>(string xml)
            where Entity : BaseEntity, new()
        {
            Entity entity = new Entity();
            Type type = typeof(Entity);
            string[] xmls = xml.Replace("\r\n", string.Empty).Split(new char[] { '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries);
            IList<string> newXMls = new List<string>(64);
            foreach (var x in xmls)
            {
                if (!string.IsNullOrEmpty(x.Trim()))
                {
                    newXMls.Add(x);
                }
            }
            xmls = newXMls.ToArray();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(IList<>)))
                {
                    int startindex = 0;
                    while (startindex < xmls.Length)
                    {
                        if (xmls[startindex].ToUpper() == property.Name.ToUpper() && xmls[startindex + 1].ToUpper() != property.Name.ToUpper())
                        {
                            BuildEntity(entity, xmls, property, ref startindex);
                        }
                        startindex++;
                    }
                }
                else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    for (int i = 0; i < xmls.Length; i++)
                    {
                        if (xmls[i].ToUpper() == property.Name.ToUpper() && xmls[i + 1].ToUpper() != property.Name.ToUpper())
                        {
                            NullableConverter nullableConverter = new NullableConverter(property.PropertyType);
                            property.SetValue(entity, Convert.ChangeType(xmls[i + 1], nullableConverter.UnderlyingType), null);
                            break;
                        }
                    }
                }
                else if (property.PropertyType.BaseType == typeof(BaseGroup))
                {
                    BuildGroup(xmls, 0, entity, property);
                }
                else
                {
                    for (int i = 0; i < xmls.Length; i++)
                    {
                        if (xmls[i].ToUpper() == property.Name.ToUpper() && xmls[i + 1].ToUpper() != property.Name.ToUpper())
                        {
                            property.SetValue(entity, Convert.ChangeType(xmls[i + 1], property.PropertyType), null);
                            break;
                        }
                    }
                }
            }
            return entity;
        }

        private static void BuildEntity(dynamic entity, string[] xmls, System.Reflection.PropertyInfo property, ref int index)
        {
            var groupType = property.PropertyType.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(groupType);
            dynamic list = listType.GetConstructor(new Type[0]).Invoke(new object[0]);
            string groupsName = string.Empty;
            for (int i = index; i < xmls.Length; i++)
            {
                if (string.IsNullOrEmpty(groupsName))
                {
                    groupsName = xmls[i];
                }
                else
                {
                    if (groupsName == xmls[i])
                    {
                        index = i;
                        break;
                    }
                }
                if (xmls[i].ToUpper() == groupType.Name.ToUpper())
                {
                    Type gtype = property.PropertyType.GetGenericArguments()[0];
                    var newgroup = gtype.GetConstructor(new Type[0]).Invoke(new object[0]);
                    int twinsNameIndex = 0;
                    while (xmls[++i].ToUpper() != groupType.Name.ToUpper())
                    {
                        foreach (var gproperty in newgroup.GetType().GetProperties())
                        {
                            if (xmls[i].ToUpper() == gproperty.Name.ToUpper())
                            {
                                if (twinsNameIndex % 2 == 0)
                                {
                                    if (gproperty.PropertyType.IsGenericType && gproperty.PropertyType.GetGenericTypeDefinition().Equals(typeof(IList<>)))
                                    {
                                        if (xmls[i + 1].ToUpper() != gproperty.Name.ToUpper())
                                        {
                                            //进入递归
                                            BuildEntity(newgroup, xmls, gproperty, ref i);
                                        }
                                    }
                                    else if (gproperty.PropertyType.IsGenericType && gproperty.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                                    { 
                                        NullableConverter nullableConverter = new NullableConverter(gproperty.PropertyType);
                                        gproperty.SetValue(newgroup, Convert.ChangeType(xmls[i + 1], nullableConverter.UnderlyingType), null);
                                    }
                                    else if (gproperty.PropertyType.BaseType == typeof(BaseGroup))
                                    {
                                        BuildGroup(xmls, i, newgroup, gproperty);
                                    }
                                    else
                                    {
                                        if (xmls[i + 1].ToUpper() != gproperty.Name.ToUpper())
                                        {
                                            gproperty.SetValue(newgroup, Convert.ChangeType(xmls[i + 1], gproperty.PropertyType), null);
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

        private static void BuildGroup(string[] xmls, int i, object newgroup, System.Reflection.PropertyInfo gproperty)
        {
            int startindex = i;
            object group = null;
            while (startindex < xmls.Length)
            {
                if (xmls[startindex].ToUpper() == gproperty.PropertyType.Name.ToUpper() && xmls[startindex + 1].ToUpper() != gproperty.PropertyType.Name.ToUpper())
                {
                    group = gproperty.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]);
                    gproperty.SetValue(newgroup, group, null);
                    break;
                }
                startindex++;
            }
            if (startindex != xmls.Length)
            {
                foreach (var Gproperty in gproperty.PropertyType.GetProperties())
                {
                    for (int j = startindex; j < xmls.Length; j++)
                    {
                        if (xmls[j].ToUpper() == Gproperty.Name.ToUpper() && xmls[j + 1].ToUpper() != Gproperty.Name.ToUpper())
                        {
                            if (Gproperty.PropertyType.IsGenericType && Gproperty.PropertyType.GetGenericTypeDefinition().Equals(typeof(IList<>)))
                            {
                                if (xmls[j + 1].ToUpper() != Gproperty.Name.ToUpper())
                                {
                                    //进入递归
                                    BuildEntity(group, xmls, Gproperty, ref j);
                                }
                            }
                            else if (Gproperty.PropertyType.IsGenericType && Gproperty.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter nullableConverter = new NullableConverter(Gproperty.PropertyType);
                                Gproperty.SetValue(group, Convert.ChangeType(xmls[j + 1], nullableConverter.UnderlyingType), null);
                            }
                            else if (Gproperty.PropertyType.BaseType == typeof(BaseGroup))
                            {
                                BuildGroup(xmls, j, group, Gproperty);
                            }
                            else
                            {
                                Gproperty.SetValue(group, Convert.ChangeType(xmls[j + 1], Gproperty.PropertyType), null);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static string EntitytoXML<Entity>(Entity entity)
                    where Entity : BaseEntity
        {
            StringBuilder body = new StringBuilder(512);
            body.Append(top);
            Type type = entity.GetType();
            body.Append('<');
            body.Append(type.Name);
            body.Append('>');
            foreach (var property in type.GetProperties())
            {
                BuildXML(entity, body, property);
            }
            body.Append("</");
            body.Append(type.Name);
            body.Append('>');
            return body.ToString();
        }

        private static void BuildXML(dynamic entity, StringBuilder body, System.Reflection.PropertyInfo property)
        {
            if(property.GetValue(entity, null) == null)
            {
                return;
            }
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(IList<>)))
            {
                body.Append('<');
                body.Append(property.Name);
                body.Append('>');
                //LIST中的元素循环
                foreach (var value in (IEnumerable)property.GetValue(entity, null))
                {
                    var groupName = value.GetType().Name;
                    body.Append('<');
                    body.Append(groupName);
                    body.Append('>');
                    foreach (var g_property in value.GetType().GetProperties())
                    {
                        BuildXML(value, body, g_property);
                    }
                    body.Append("</");
                    body.Append(groupName);
                    body.Append('>');
                }
                body.Append("</");
                body.Append(property.Name);
                body.Append('>');
            }
            else if(property.PropertyType.BaseType == typeof(BaseGroup))
            {
                body.Append('<');
                body.Append(property.Name);
                body.Append('>');
                foreach(var g_property in property.PropertyType.GetProperties())
                {
                    BuildXML(property.GetValue(entity, null), body, g_property);
                }
                body.Append("</");
                body.Append(property.Name);
                body.Append('>');
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                body.Append('<');
                body.Append(property.Name);
                body.Append('>');
                if (property.PropertyType == typeof(Nullable<DateTime>))
                {
                    string time = ((DateTime)property.GetValue(entity, null)).ToString("yyyy-MM-dd HH:mm:ss");
                    if (time.IndexOf(" 00:00:00") > 1)
                    {
                        body.Append(time.Substring(0, 10));
                    }
                    else
                    {
                        body.Append(time);
                    }
                }
                else
                {
                    body.Append(property.GetValue(entity, null).ToString());
                }
                body.Append("</");
                body.Append(property.Name);
                body.Append('>');
            }
            else
            {
                body.Append('<');
                body.Append(property.Name);
                body.Append('>');
                if (property.PropertyType == typeof(DateTime))
                {
                    string time = ((DateTime)property.GetValue(entity, null)).ToString("yyyy-MM-dd HH:mm:ss");
                    if (time.IndexOf(" 00:00:00") > 1)
                    {
                        body.Append(time.Substring(0, 10));
                    }
                    else
                    {
                        body.Append(time);
                    }
                }
                else
                {
                    body.Append(property.GetValue(entity, null).ToString());
                }
                body.Append("</");
                body.Append(property.Name);
                body.Append('>');
            }
        }
    }
}
