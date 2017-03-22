using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WSEntity
{
    public class HISYY_Submit : WSEntityRequest
    {
        public HISYY_Submit()
        {
            StudiesExamine = new List<StudiesExamine>();
        }
        public string AdmissionSource { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string PatientName { get; set; }
        public string IdNumber { get; set; }
        public string RequestNo { get; set; }
        public string AdmissionID { get; set; }
        public string ExaminePartTime { get; set; }
        public string PatientSex { get; set; }
        public string PatientBorn { get; set; }
        public string PatientAge { get; set; }
        public string PatientTel { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCard { get; set; }
        public string InPatientAreaName { get; set; }
        public string InPatientAreaCode { get; set; }
        public string BedNum { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
        public IList<StudiesExamine> StudiesExamine { get; set; }
        public string ExamineFY { get; set; }
        public string ReceiptNum { get; set; }
        public string ZxDepartmentId { get; set; }
        public string ZxDepartmentName { get; set; }
        public string Sqrq { get; set; }
        public string Jcdxhs { get; set; }
        public string YjsbCode { get; set; }
        public string BespeakDateTime { get; set; }
        public string JZ { get; set; }
        public string PF { get; set; }
        public string ZQ { get; set; }
        public string LS { get; set; }
        public string UScount { get; set; }
    }
}
