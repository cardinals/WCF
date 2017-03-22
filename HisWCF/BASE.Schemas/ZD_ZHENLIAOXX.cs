using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    public class ZD_ZHENLIAOXX_IN : MessageIn
    {
        /// <summary>
        ///项目归类
        /// </summary>
        public string XIANGMUGL { get; set; }
        /// <summary>
        /// 项目类型（输入码）
        /// </summary>
        public string XIANGMULX { get; set; }
        /// <summary>
        ///输入码类型
        /// </summary>
        public string SHURUMLX { get; set; }
        /// <summary>
        ///输入码
        /// </summary>
        public string SHURUM { get; set; }
        /// <summary>
        /// 查询套餐明细时传入大项的项目序号
        /// </summary>
        public string XIANGMUXH { get; set; }

    }
    public class ZD_ZHENLIAOXX_OUT : MessageOUT
    {
        /// <summary>
        /// 诊疗信息列表
        /// </summary>
        public List<ZHENLIAOXX> ZHENLIAOMX { get; set; }
        public ZD_ZHENLIAOXX_OUT()
        {
            ZHENLIAOMX = new List<ZHENLIAOXX>();
        }
    }

    public class ZHENLIAOXX
    {
        /// <summary>
        ///项目归类
        /// </summary>
        public string XIANGMUGL { get; set; }
        /// <summary>
        ///项目序号
        /// </summary>
        public string XIANGMUXH { get; set; }
        /// <summary>
        ///项目名称
        /// </summary>
        public string XIANGMUMC { get; set; }
        /// <summary>
        ///项目归类名称
        /// </summary>
        public string XIANGMUGLMC { get; set; }
        /// <summary>
        ///项目单位
        /// </summary>
        public string XIANGMUDW { get; set; }
        /// <summary>
        ///单价
        /// </summary>
        public string DANJIA { get; set; }
        /// <summary>
        /// 医保等级
        /// </summary>
        public string YIBAODJ { get; set; }
    }
}
