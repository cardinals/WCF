using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;

namespace HIS4.Schemas
{
    public class JIUZHENXXXZ_IN : MessageIn
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDENTITYCARDID { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public string APPOINTMENTDATE { get; set; }
        /// <summary>
        /// 医院机构代码（9位）
        /// </summary>
        public string APPOINTMENTHOSCODE { get; set; }
    }

    public class JIUZHENXXXZ_OUT : MessageOUT
    {
        public MEDICALINFO MEDICALINFO { get; set; }
    }


    public class MEDICALINFO
    {
        public string BINGANID { get; set; }
        public string PATIENTBEDNUM { get; set; }
        public string IDENTITYCARDID { get; set; }
        public string PATIENTNAME { get; set; }
        public string PATIENTSEX { get; set; }
        public string PATIENTNATION { get; set; }
        public string PATIENTPROFESSION { get; set; }
        public string PATIENTBIRTHDAY { get; set; }
        public string PATIENTAGE { get; set; }
        public string PATIENTPHONE { get; set; }
        public string PATIENTADDRESS { get; set; }
        public string SENDHOSID { get; set; }
        public string SENDHOSNAME { get; set; }
        public string SENDHOSPHONE { get; set; }
        public string SENDSECNAME { get; set; }
        public string SENDSECPHONE { get; set; }
        public string SENDDOCNAME { get; set; }
        public string SENDDOCPHONE { get; set; }
        public string ENTERHOSDATE { get; set; }
        public string SUBMITDATE { get; set; }
        public string FIRSTDIAGNOSIS { get; set; }
        public string EXAMINATIONINFO { get; set; }
        public string INSPECTIONINFO { get; set; }
        public string DRUGDOSESINFO { get; set; }
        public string PATIENTCONDITION { get; set; }
        public string REMARK { get; set; }
        public string RESERVATIONNUM { get; set; }
        public string RESERVATIONVISITDATE { get; set; }
        public string RESERVATIONVISITNUM { get; set; }
        public string RESERVATIONHOSID { get; set; }
        public string RESERVATIONHOSNAME { get; set; }
        public string RESERVATIONSECID { get; set; }
        public string RESERVATIONSECNAME { get; set; }
        public string RESERVARIONDOCID { get; set; }
        public string RESERVATIONDOCNAME { get; set; }
    }


}
