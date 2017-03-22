using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 设备预约入参
    /// </summary>
    public class SHEBEIYY_IN : MessageIn
    {
        /// <summary>
        /// 检查科室代码
        /// </summary>
        public string JIANCHAKSDM { get; set; }
        /// <summary>
        /// 检查科室名称
        /// </summary>
        public string JIANCHAKSMC { get; set; }
        /// <summary>
        /// 病人发票号
        /// </summary>
        public string BINGRENFPH { get; set; }
        /// <summary>
        /// 病人类型
        /// </summary>
        public int BINGRENLX { get; set; }
        /// <summary>
        /// 病人类型名称
        /// </summary>
        public string BINGRENLXMC { get; set; }
        /// <summary>
        /// 病人卡号
        /// </summary>
        public string BINGRENKH { get; set; }
        /// <summary>
        /// 病人门诊号
        /// </summary>
        public string BINGRENMZH { get; set; }
        /// <summary>
        /// 病人住院号
        /// </summary>
        public string BINGRENZYH { get; set; }
        /// <summary>
        /// 病人病区代码
        /// </summary>
        public string BINGRENBQDM { get; set; }
        /// <summary>
        /// 病人病区名称
        /// </summary>
        public string BINGRENBQMC { get; set; }
        /// <summary>
        /// 病人床位号
        /// </summary>
        public string BINGRENCWH { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string BINGRENXM { get; set; }
        /// <summary>
        /// 病人性别
        /// </summary>
        public int BINGRENXB { get; set; }
        /// <summary>
        /// 病人年龄
        /// </summary>
        public string BINGRENNL { get; set; }
        /// <summary>
        /// 病人出生日期
        /// </summary>
        public string BINGRENCSRQ { get; set; }
        /// <summary>
        /// 病人联系地址
        /// </summary>
        public string BINGRENLXDZ { get; set; }
        /// <summary>
        /// 病人联系电话
        /// </summary>
        public string BINGRENLXDH { get; set; }
        /// <summary>
        /// 申请医生工号
        /// </summary>
        public string SHENQINGYSGH { get; set; }
        /// <summary>
        /// 申请医生姓名
        /// </summary>
        public string SHENQINGYSMC { get; set; }
        /// <summary>
        /// 申请医院代码
        /// </summary>
        public string SHENQINGYYDM { get; set; }
        /// <summary>
        /// 申请医院名称
        /// </summary>
        public string SHENQINGYYMC { get; set; }
        /// <summary>
        /// 检查项目代码
        /// </summary>
        public string JIANCHAXMDM { get; set; }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        public string JIANCHAXMMC { get; set; }
        /// <summary>
        /// 检查项目类型
        /// </summary>
        public string JIANCHAXMLX { get; set; }
        /// <summary>
        /// 检查部位代码
        /// </summary>
        public string JIANCHABWDM { get; set; }
        /// <summary>
        /// 检查部位名称
        /// </summary>
        public string JIANCHABWMC { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string YUYUESJ { get; set; }
        /// <summary>
        /// 预约号
        /// </summary>
        public string YUYUEH { get; set; }
        /// <summary>
        /// 检查设备代码
        /// </summary>
        public string JIANCHASBDM { get; set; }
        /// <summary>
        /// 检查设备名称
        /// </summary>
        public string JIANCHASBMC { get; set; }
        /// <summary>
        /// 检查设备地点
        /// </summary>
        public string JIANCHASBDD { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string SHENFENZH { get; set; }
        /// <summary>
        /// 预约收费
        /// </summary>
        public int YUYUESF { get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public int YUYUEZT { get; set; }
        /// <summary>
        /// 检查申请单编号
        /// </summary>
        public string JIANCHASQDBH { get; set; }
        /// <summary>
        /// 影像方向
        /// </summary>
        public string YINGXIANGFX { get; set; }
        /// <summary>
        /// 预约时间段
        /// </summary>
        public string YUYUESJD { get; set; }
        /// <summary>
        /// 详细安排时间
        /// </summary>
        public string XIANGXIAPSJ { get; set; }
        /// <summary>
        /// 业务来源
        /// </summary>
        public string YEWULY { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string YEWULX { get; set; }
        /// <summary>
        /// 是否增强
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
        /// <summary>
        /// 项目耗时
        /// </summary>
        public string XIANGMUHS { get; set; }
        /// <summary>
        /// 就诊卡类型
        /// </summary>
        public string JIUZHENKLX { get; set; }
        /// <summary>
        /// 就诊卡号
        /// </summary>
        public string JIUZHENKH { get; set; }
        /// <summary>
        /// 病情描述
        /// </summary>
        public string BINGQINGMS { get; set; }
        /// <summary>
        /// 诊断
        /// </summary>
        public string ZHENDUAN { get; set; }
        /// <summary>
        /// 病人体征
        /// </summary>
        public string BINGRENTZ { get; set; }
        /// <summary>
        /// 其它检查
        /// </summary>
        public string QITAJC { get; set; }
        /// <summary>
        /// 病人主诉
        /// </summary>
        public string BINGRENZS { get; set; }
        /// <summary>
        /// 检查明细
        /// </summary>
        public List<JIANCHAXX> JIANCHALB { get; set; }
        /// <summary>
        /// 诊断信息
        /// </summary>
        public List<ZHENDUANXX> ZHENDUANLB { get; set; }

        public SHEBEIYY_IN()
        {
            JIANCHALB = new List<JIANCHAXX>();
            ZHENDUANLB = new List<ZHENDUANXX>();
        }
    }

    /// <summary>
    /// 设备预约出参
    /// </summary>
    public class SHEBEIYY_OUT : MessageOUT
    {
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }
        /// <summary>
        /// 预约号
        /// </summary>
        public string YUYUEH { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string YUYUESJ { get; set; }

        /// <summary>
        /// 检查号
        /// </summary>
        public string JIANCHAH { get; set; }
    }

    public class ZHENDUANXX
    {
        /// <summary>
        /// ICD10
        /// </summary>
        public string ICD10 { get; set; }
        /// <summary>
        /// 诊断名称
        /// </summary>
        public string ZHENDUANMC { get; set; }
    }

    public class JIANCHAXX
    {
        /// <summary>
        /// 检查项目编号
        /// </summary>
        public string JIANCHAXMBH { get; set; }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        public string JIANCHAXMMC { get; set; }
        /// <summary>
        /// 检查分类编码
        /// </summary>
        public string JIANCHAFLBM { get; set; }
        /// <summary>
        /// 检查身体部位
        /// </summary>
        public string JIANCHASTBW { get; set; }
        /// <summary>
        /// 检查方向代码
        /// </summary>
        public string JIANCHAFXDM { get; set; }
        /// <summary>
        /// 检查肢位代码
        /// </summary>
        public string JIANCHAZYDM { get; set; }
        /// <summary>
        /// 检查提示
        /// </summary>
        public string JIANCHATS { get; set; }
    }
}
