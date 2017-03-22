using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 设备预约状态查询入参
    /// </summary>
    public class SHEBEIYYZTCX_IN : MessageIn
    {
        /// <summary>
        /// 检查项目代码
        /// </summary>
        public string JIANCHAXMDM { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string YUYUESJ { get; set; }
        /// <summary>
        /// 查询类型
        /// </summary>
        public string CHAXUNLX { get; set; }
        /// <summary>
        /// 业务来源
        /// </summary>
        public string YEWULY { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string YEWULX { get; set; }
        /// <summary>
        /// 死否增强
        /// </summary>
        public string ZENGQIANG { get; set; }
        /// <summary>
        /// 是否急诊
        /// </summary>
        public string JIZHEN { get; set; }
        /// <summary>
        /// 是否临时
        /// </summary>
        public string LINSHI { get; set; }
    }

    /// <summary>
    /// 设备预约状态查询出参
    /// </summary>
    public class SHEBEIYYZTCX_OUT : MessageOUT
    {
        /// <summary>
        /// 设备预约信息
        /// </summary>
        public List<SHEBEIYYXX> SHEBEIYYXXXX { get; set; }
        public SHEBEIYYZTCX_OUT()
        {
            SHEBEIYYXXXX = new List<SHEBEIYYXX>();
        }
    }

    /// <summary>
    /// 设备预约信息
    /// </summary>
    public class SHEBEIYYXX
    {
        /// <summary>
        /// 检查设备代码
        /// </summary>
        public string JIANCHASBDM{ get; set; }
        /// <summary>
        /// 检查设备名称
        /// </summary>
        public string JIANCHASBMC{ get; set; }
        /// <summary>
        /// 检查设备地点
        /// </summary>
        public string JIANCHASBDD{ get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ{ get; set; }
        /// <summary>
        /// 预约开始时间
        /// </summary>
        public string YUYUEKSSJ{ get; set; }
        /// <summary>
        /// 预约结束时间
        /// </summary>
        public string YUYUEJSSJ{ get; set; }
        /// <summary>
        /// 预约检查部位
        /// </summary>
        public string YUYUEJCBW{ get; set; }
        /// <summary>
        /// 检查预约类型
        /// </summary>
        public int JIANCHAYYLX{ get; set; }
        /// <summary>
        /// 预约号总数
        /// </summary>
        public int YUYUEHZS { get; set; }
        /// <summary>
        /// 已预约数
        /// </summary>
        public int YIYUYUES { get; set; }
        /// <summary>
        /// 项目耗时
        /// </summary>
        public int XIANGMUHS { get; set; }
        ///// <summary>
        ///// 预约状态信息
        ///// </summary>
        //public List<YUYUEZTXX> YUYUEZTXX { get; set; }
        /// <summary>
        /// 预约号信息
        /// </summary>
        public List<YUYUEHXX> YUYUEHXXXX { get; set; }
    }

    ///// <summary>
    ///// 预约状态信息
    ///// </summary>
    //public class YUYUEZTXX
    //{
    //    /// <summary>
    //    /// 预约号总数
    //    /// </summary>
    //    public int YUYUEHZS { get; set; }
    //    /// <summary>
    //    /// 已预约数
    //    /// </summary>
    //    public int YIYUYUES { get; set; }
    //}

    /// <summary>
    /// 预约号信息
    /// </summary>
    public class YUYUEHXX
    {
        /// <summary>
        /// 预约号
        /// </summary>
        public string YUYUEH{ get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public int YUYUEZT { get; set; }
    }
}
