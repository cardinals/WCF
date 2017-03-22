using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace HIS4.Biz
{
    public class JIUZHENXXXZ : IMessage<JIUZHENXXXZ_IN, JIUZHENXXXZ_OUT>
    {

        public override void ProcessMessage()
        {
            string shengFengZH = InObject.IDENTITYCARDID;//身份证号
            string APPOINTMENTDATE = string.Empty;// InObject.APPOINTMENTDATE;//预约日期
            string JiGouDM = InObject.APPOINTMENTHOSCODE;
            string inMsg =string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                             <MedicalInfo_In>
  	                            <IdentityCardID>{0}</IdentityCardID>
  	                            <AppointmentDate>{1}</AppointmentDate>
                                <AppointmentHosCode>{2}</AppointmentHosCode>
                             </MedicalInfo_In>", shengFengZH, APPOINTMENTDATE,JiGouDM);
            BangTaiJK.BonsTechClient btjk = new BangTaiJK.BonsTechClient();

            string outMsg = btjk.GetMessage("MedicalInfo", inMsg);

            OutObject = MessageParse.ToXmlObject<JIUZHENXXXZ_OUT>(outMsg.ToUpper().Replace("MEDICALINFO_OUT", "JIUZHENXXXZ_OUT").Replace("ERRCODE", "ERRNO"), true);


        }
    }
}
