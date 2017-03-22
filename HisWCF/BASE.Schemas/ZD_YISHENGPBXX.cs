using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{

    public class ZD_YISHENGPBXX_IN : MessageIn
    {
        /// <summary>
        /// 医生工号
        /// </summary>
        public string YSDM { get; set; }

    }
    public class ZD_YISHENGPBXX_OUT : MessageOUT
    {
        /// <summary>
        /// 医生排班信息
        /// </summary>
        public List<PAIBANXX> PAIBANXX { get; set; }
        public ZD_YISHENGPBXX_OUT()
        {
            PAIBANXX = new List<PAIBANXX>();
        }
    }
    public class PAIBANXX
    {
        /// <summary>
        /// 记录序号
        /// </summary>
        public string JLXH { get; set; }
        /// <summary>
        /// 星期
        /// </summary>
        public string XQ { get; set; }//  ---星期
        /// <summary>
        /// --科室名称
        /// </summary>
        public string KSMC { get; set; }
        /// <summary>
        /// //-- 科室代码
        /// </summary>
        public string KSDM { get; set; }
        /// <summary>
        /// ,--门诊类别名称
        /// </summary>
        public string MZLBMC { get; set; }
        /// <summary>
        /// ,--上午限号
        /// </summary>
        public string SWXH { get; set; }
        /// <summary>
        /// 上午最高号
        /// </summary>
        public string SWZGH { get; set; }
        /// <summary>
        /// ,--下午限号
        /// </summary>
        public string XWXH { get; set; }
        /// <summary>
        /// 下午最高号
        /// </summary>
        public string XWZGH { get; set; }
        /// <summary>
        /// -医生工号
        /// </summary>
        public string YSGH { get; set; }
        /// <summary>
        ///  --'医生姓名';
        /// </summary>
        public string YSXM { get; set; }
        /// <summary>
        /// 上午预约限号
        /// </summary>
        public string SWYYXH { get; set; }
        /// <summary>
        /// 下午预约限号
        /// </summary>
        public string XWYYXH { get; set; }
        /// <summary>
        /// 预约费
        /// </summary>
        public string YYF { get; set; }
        /// <summary>
        /// 医生描述
        /// </summary>
        public string YSMS { get; set; }


    }
}
