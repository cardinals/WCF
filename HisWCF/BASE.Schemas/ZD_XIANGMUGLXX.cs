using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_XIANGMUGLXX_IN : MessageIn
    {

    }
    public class ZD_XIANGMUGLXX_OUT : MessageOUT
    {
        /// <summary>
        /// 项目归类列表
        /// </summary>
        public List<XIANGMUGLXX> XIANGMUGLMX { get; set; }
        public ZD_XIANGMUGLXX_OUT()
        {
            XIANGMUGLMX = new List<XIANGMUGLXX>();
        }
    }

    public class XIANGMUGLXX
    {
        /// <summary>
        /// 项目归类代码
        /// </summary>
        public string XIANGMUGL { get; set; }
        /// <summary>
        /// 项目归类名称
        /// </summary>
        public string XIANGMUGLMC { get; set; }

    }
}
