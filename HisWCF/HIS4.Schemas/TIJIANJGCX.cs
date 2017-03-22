using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class TIJIANJGCX_IN :MessageIn
    {
        /// <summary>
        /// 体检编码
        /// </summary>
        public string TIJIANBM { get; set; }

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

    public class TIJIANJGCX_OUT : MessageOUT
    {
        /// <summary>
        /// 体检结果明细
        /// </summary>
        public List<TIJIANJGXX> TIJIANJGMX { get; set; }

        public TIJIANJGCX_OUT() {
            this.TIJIANJGMX = new List<TIJIANJGXX>();
        }
        
    }

    /// <summary>
    /// 体检结果信息
    /// </summary>
    public class TIJIANJGXX { 
    
        /// <summary>
        /// 体检编码
        /// </summary>
        public string TIJIANBM { get; set; }
        /// <summary>
        /// 总检信息
        /// </summary>
        public ZONGJIANXX ZONGJIANXX { get; set; }
        /// <summary>
        /// 项目明细
        /// </summary>
        public List<TIJIANXMXX> TIJIANXMMX { get; set; }

        public TIJIANJGXX() {
            this.ZONGJIANXX = new ZONGJIANXX();
            this.TIJIANXMMX = new List<TIJIANXMXX>();
        }

    }
    /// <summary>
    /// 总检信息
    /// </summary>
    public class ZONGJIANXX {
        /// <summary>
        /// 总检小结
        /// </summary>
        public string ZONGJIANXJ { get; set; }
        /// <summary>
        /// 总检建议
        /// </summary>
        public string ZONGJIANJY { get; set; }
        /// <summary>
        /// 体检诊断
        /// </summary>
        public string TIJIANZD { get; set; }
    }
    /// <summary>
    /// 体检项目信息
    /// </summary>
    public class TIJIANXMXX {

        /// <summary>
        /// 项目组合名称
        /// </summary>
        public string XIANGMUZHMC { get; set; }
        /// <summary>
        /// 体检项目结果明细
        /// </summary>
        public List<TIJIANXMJGXX> TIJIANXMJGMX { get; set; }

        public TIJIANXMXX() {
            this.TIJIANXMJGMX = new List<TIJIANXMJGXX>();
        }
    }

    /// <summary>
    /// 体检项目结果信息
    /// </summary>
    public class TIJIANXMJGXX {
        /// <summary>
        /// 项目编码
        /// </summary>
        public string XIANGMUBM { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string XIANGMUMC { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public string JILIANGDW { get; set; }
        /// <summary>
        /// 执行科室
        /// </summary>
        public string ZHIXINGKS { get; set; }
        /// <summary>
        /// 执行科室名称
        /// </summary>
        public string ZHIXINGKSMC { get; set; }
        /// <summary>
        /// 操作人编码
        /// </summary>
        public string CAOZUORBM { get; set; }
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string CAOZUORXM { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CAOZUORQ { get; set; }
        /// <summary>
        /// 体检结果
        /// </summary>
        public string TIJIANJG { get; set; }
        /// <summary>
        /// 正常结果
        /// </summary>
        public string ZHENGCHANGJG { get; set; }
        /// <summary>
        /// 参考下限
        /// </summary>
        public string CANKAOXX { get; set; }
        /// <summary>
        /// 参考上限
        /// </summary>
        public string CANKAOSX { get; set; }
        /// <summary>
        /// 结论
        /// </summary>
        public string JIELUN { get; set; }
        /// <summary>
        /// 审核员
        /// </summary>
        public string SHENHEY { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string SHENHESJ { get; set; }
        /// <summary>
        /// 样本号
        /// </summary>
        public string YANGBENH { get; set; }
    }
}
