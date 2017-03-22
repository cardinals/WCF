using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_Register : WSEntityRequest
    {
        public HISYY_Register()
        {
            StudiesExamine = new List<StudiesExamine>();
        }
        public string AdmissionSource { get; set; }//	病人类型
        public string HospitalCode { get; set; }//	申请医院代码
        public string HospitalName { get; set; }//	申请医院名称
        public string PatientName { get; set; }//	病人姓名
        public string IdNumber { get; set; }//	身份证号
        public string RequestNo { get; set; }//	申请单号
        public string AdmissionID { get; set; }//	门诊住院号
        public string ExaminePartTime { get; set; }//	项目耗时
        public string PatientSex { get; set; }//	病人性别
        public string PatientBorn { get; set; }//	病人出身时期
        public string PatientAge { get; set; }//	病人年龄
        public string PatientTel { get; set; }//	病人电话
        public string PatientAddress { get; set; }//	病人地址
        public string PatientCard	 { get; set; }//就诊卡号
        public string InPatientAreaName	 { get; set; }//登记病区
        public string InPatientAreaCode { get; set; }//	病区代码
        public string BedNum	 { get; set; }//登记床位
        public string DeviceCode { get; set; }//	设备代码
        public string DeviceName	 { get; set; }//设备名称
        public string ExamineCode	 { get; set; }//检查项目代码
        public string ExamineName	 { get; set; }//检查项目名称
        public string ReceiptNum	 { get; set; }//发票号码
        public string ZxDepartmentId { get; set; }//	执行科室代码
        public string ZxDepartmentName { get; set; }//	执行科室名称
        public string Sqrq { get; set; }//	申请日期
        public string ExamineFY { get; set; }//	检查费用
        public string YjsbCode { get; set; }//	医技设备代码
        public string Numbers	 { get; set; }//数量
        public string ExaminePrice { get; set; }//	单价
        public string BespeakDateTime { get; set; }//	预约时间
        public string PDH { get; set; }//	排队号
        public string JCH { get; set; }//	检查号
        public string YYH { get; set; }//	预约号
        public string RequestDepartmentId { get; set; }//	申请科室代码
        public string RequestDepartmentName	 { get; set; }//申请科室名称
        public string RequestDoctorId { get; set; }//	申请医生代码
        public string RequestDoctorName { get; set; }//申请医生名称
        public IList<StudiesExamine> StudiesExamine { get; set; }
    }
}
