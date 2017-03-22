using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class TIJIANJLCX_IN :MessageIn
    {
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 单位体检编码
        /// </summary>
        public string DANWEITJDBM { get; set; }
        /// <summary>
        /// 单位编码
        /// </summary>
        public string DANWEIBM { get; set; }
        
    }

    public class TIJIANJLCX_OUT : MessageOUT
    {
        /// <summary>
        /// 体检记录明细
        /// </summary>
        public List<TIJIANXX> TIJIANMX { get; set; }


        public TIJIANJLCX_OUT() {
            this.TIJIANMX = new List<TIJIANXX>();
        }
        
    }

    public class TIJIANXX { 
    
        /// <summary>
        /// 体检编码
        /// </summary>
        public string TIJIANBM { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZHENGJIANHM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XINGMING { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string CHUSHENGRQ { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string GONGZUODW { get; set; }
        /// <summary>
        /// 体检日期
        /// </summary>
        public string TIJIANRQ { get; set; }
        /// <summary>
        /// 操作员编码
        /// </summary>
        public string CAOZUOYBM { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string CAOZUOYMC { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CAOZUORQ { get; set; }
        /// <summary>
        /// 体检类别编码
        /// </summary>
        public string TIJIANLBBM { get; set; }
        /// <summary>
        /// 体检类别名称
        /// </summary>
        public string TIJIANLBMC { get; set; }
        /// <summary>
        /// 体检单状态
        /// </summary>
        public string TIJIANDZT { get; set; }
        /// <summary>
        /// 体检开始日期
        /// </summary>
        public string TIJIANKSRQ { get; set; }
        /// <summary>
        /// 体检结束日期
        /// </summary>
        public string TIJIANJSRQ { get; set; }
        /// <summary>
        /// 单位体检单编码
        /// </summary>
        public string DANWEITJDBM { get; set; }
        /// <summary>
        /// 单位编码
        /// </summary>
        public string DANWEIBM { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public string JIESUANZT { get; set; }
        /// <summary>
        /// 归档标识
        /// </summary>
        public string GUIDANGBZ { get; set; }
        /// <summary>
        /// 归档日期
        /// </summary>
        public string GUIDANGRQ { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string TAOCANMC { get; set; }
    }
}
