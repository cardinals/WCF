using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JYCS.Schemas
{
    /// <summary>
    /// 入参消息基类
    /// </summary>
    public class MessageIn
    {
        /// <summary>
        /// 基本入参
        /// </summary>
        public BASEINFO BASEINFO { get; set; }
        public MessageIn()
        {
            BASEINFO = new BASEINFO();
        }
    }
}
