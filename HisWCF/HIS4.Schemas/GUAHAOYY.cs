using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class GUAHAOYY_IN : MessageIn
    {
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZHENGJIANLX { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 性别  1 男 2 女 0 未知 9 其他
        /// </summary>
        public string XINGBIE { get; set; }
        /// <summary>
        /// 一周排班ID
        /// </summary>
        public string YIZHOUPBID { get; set; }
        /// <summary>
        /// 当天排班ID
        /// </summary>
        public string DANGTIANPBID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string RIQI { get; set; }
        /// <summary>
        /// 挂号班次
        /// </summary>
        public string GUAHAOBC { get; set; }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GUAHAOLB { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string KESHIDM { get; set; }
        /// <summary>
        /// 医生代码
        /// </summary>
        public string YISHENGDM { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 预约来源
        /// </summary>
        public string YUYUELY { get; set; }
        /// <summary>
        /// 预约类型
        /// </summary>
        public string YUYUELX { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LIANXIDH { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string BINGRENID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string BEIZHU { get; set; }
        
    }

    public class GUAHAOYY_OUT : MessageOUT {
        /// <summary>
        /// 取号密码
        /// </summary>
        public string QUHAOMM { get; set; }
        /// <summary>
        /// 挂号序号
        /// </summary>
        public string GUAHAOXH { get; set; }
        /// <summary>
        /// 就诊时间
        /// </summary>
        public string JIUZHENSJ { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<MENZHENFYXX> FEIYONGMX { get; set; }

        public GUAHAOYY_OUT() {
            this.FEIYONGMX = new List<MENZHENFYXX>();
            this.QUHAOMM = string.Empty;
            this.GUAHAOXH = string.Empty;
            this.JIUZHENSJ = string.Empty;
        }
    }
}
