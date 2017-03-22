using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JYCS.Schemas
{
    /// <summary>
    /// 预约信息查询入参
    /// </summary>
    public class YUYUEXXCX_IN : MessageIn
    {
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }
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
        /// 身份证号
        /// </summary>
        public string SHENFENZH { get; set; }
        /// <summary>
        /// 检查申请单编号
        /// </summary>
        public string JIANCHASQDBH { get; set; }
        /// <summary>
        /// 预约开始日期
        /// </summary>
        public string YUYUEKSRQ { get; set; }
        /// <summary>
        /// 查询类型
        /// </summary>
        public string CHAXUNLX { get; set; }
    }

    /// <summary>
    /// 预约信息查询出参
    /// </summary>
    public class YUYUEXXCX_OUT : MessageOUT
    {
        /// <summary>
        /// 预约信息
        /// </summary>
        public List<YUYUEXX> YUYUEXXXX { get; set; }
        public YUYUEXXCX_OUT()
        {
            YUYUEXXXX = new List<YUYUEXX>();
        }
    }

    /// <summary>
    /// 预约信息
    /// </summary>
    public class YUYUEXX
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
        /// 检查设备代码
        /// </summary>
        public int JIANCHASBDM { get; set; }
        /// <summary>
        /// 检查设备名称
        /// </summary>
        public string JIANCHASBMC { get; set; }
        /// <summary>
        /// 检查设备地点
        /// </summary>
        public string JIANCHASBDD { get; set; }
        /// <summary>
        /// 预约号
        /// </summary>
        public string YUYUEH { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string SHENFENZH { get; set; }
        /// <summary>
        /// 预约收费
        /// </summary>
        public int YUYUESF { get; set; }
        /// <summary>
        /// 预约申请单编号
        /// </summary>
        public string YUYUESQDBH { get; set; }
        /// <summary>
        /// 预约申请单状态
        /// </summary>
        public int YUYUESQDZT { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public string SHENQINGSJ { get; set; }
        /// <summary>
        /// 检查号
        /// </summary>
        public string JIANCHAH { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string YUYUERQ { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string YUYUESJ { get; set; }
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
        public string YUYUSJD { get; set; }
    }
}
