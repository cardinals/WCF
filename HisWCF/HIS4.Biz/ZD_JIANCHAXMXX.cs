using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using System.Configuration;
using SWSoft.Framework;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using HIS4.Schemas;


namespace HIS4.Biz
{
    public class ZD_JIANCHAXMXX : IMessage<ZD_JIANCHAXMXX_IN, ZD_JIANCHAXMXX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new ZD_JIANCHAXMXX_OUT();
            DataTable dtJianChaXMXX = DBVisitor.ExecuteTable("SELECT A.JIANCHAXMID AS JIANCHAXMDM,A.JIANCHAXMMC AS JIANCHAXMMC,A.JIANCHALX AS JIANCHAXMLX,D.DAIMAMC AS JIANCHAXMLXMC,C.JIANCHABWID AS JIANCHABWDM,B.DAIMAMC AS JIANCHABWMC,'' AS JIANCHAXMFX,A.JIANCHASM AS JIANCHAXMBZ FROM GY_JIANCHAXM A, GY_JIANCHABW B, GY_JIANCHAXMBWDY C, GY_JIANCHALX D WHERE A.JIANCHAXMID = C.JIANCHAXMID AND A.JIANCHALX = D.DAIMAID AND B.DAIMAID = C.JIANCHABWID AND A.JIANCHALX = D.DAIMAID AND A.ZUOFEIBZ = 0 AND A.MOJIBZ = 1 AND B.ZUOFEIBZ = 0 AND D.ZUOFEIBZ = 0 and substr(a.yuanqusy,1,1) ='1' ");

            for (int i = 0; i < dtJianChaXMXX.Rows.Count; i++) {
                JIANCHAXMXX temp = new JIANCHAXMXX();
                temp.JIANCHAXMDM = dtJianChaXMXX.Rows[i]["JIANCHAXMDM"].ToString();
                temp.JIANCHABWDM = dtJianChaXMXX.Rows[i]["JIANCHABWDM"].ToString();
                temp.JIANCHABWMC = dtJianChaXMXX.Rows[i]["JIANCHABWMC"].ToString();
                temp.JIANCHAXMBZ = dtJianChaXMXX.Rows[i]["JIANCHAXMBZ"].ToString();
                temp.JIANCHAXMFX = dtJianChaXMXX.Rows[i]["JIANCHAXMFX"].ToString();
                temp.JIANCHAXMLX = dtJianChaXMXX.Rows[i]["JIANCHAXMLX"].ToString();
                temp.JIANCHAXMLXMC = dtJianChaXMXX.Rows[i]["JIANCHAXMLXMC"].ToString();
                temp.JIANCHAXMMC = dtJianChaXMXX.Rows[i]["JIANCHAXMMC"].ToString();
                OutObject.JIANCHAXMMX.Add(temp);
            }
        } 
    }
}
