using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class WorkFlowConfigurationSection : IConfigurationSectionHandler
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="parent"></param> 
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            Dictionary<string, IList<Step>> wolkflows = new Dictionary<string, IList<Step>>();  
            foreach (XmlNode node in section.ChildNodes)
            {
                string name = node.Attributes["name"].Value;
                string className = node.Attributes["classname"].Value;
                wolkflows.Add(name, new List<Step>());
                foreach (XmlNode vnode in node.ChildNodes)
                {
                    wolkflows[name].Add(new Step() { WolkflowDiscription = node.Attributes["discription"].Value, Discription = vnode.Attributes["discription"].Value, URL = vnode.InnerText.Replace("\r\n", string.Empty).Trim(), Number = vnode.Attributes["number"].Value, ClassName = className, Properties = vnode.Attributes["properties"].Value.Split('|') });
                }
            }
            return wolkflows;
        }
    }
}