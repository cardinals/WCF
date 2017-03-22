using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class MACmappingConfigurationSection : IConfigurationSectionHandler
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
            Dictionary<string, string> mappings = new Dictionary<string, string>();
            foreach (XmlNode node in section.ChildNodes)
            {
                mappings.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
            }
            return mappings;
        }
    }
}