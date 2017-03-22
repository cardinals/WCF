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
    public class ZD_JIANCHAXMBW : IMessage<ZD_JIANCHAXMBW_IN, ZD_JIANCHAXMBW_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new ZD_JIANCHAXMBW_OUT();
            DataTable dtJianChaBWXMXX = DBVisitor.ExecuteTable("SELECT DAIMAID AS JIANCHAXMBWDM, DAIMAMC AS JIANCHAXMBWMC,JIANCHALX AS JIANCHAXMLX  FROM GY_JIANCHABW WHERE ZUOFEIBZ = 0 ");

            for (int i = 0; i < dtJianChaBWXMXX.Rows.Count; i++)
            {
                JIANCHAXMBWXX temp = new JIANCHAXMBWXX();

                temp.JIANCHAXMBWDM = dtJianChaBWXMXX.Rows[i]["JIANCHAXMBWDM"].ToString();
                temp.JIANCHAXMBWMC = dtJianChaBWXMXX.Rows[i]["JIANCHAXMBWMC"].ToString();
                temp.JIANCHAXMLX = dtJianChaBWXMXX.Rows[i]["JIANCHAXMLX"].ToString();

                OutObject.JIANCHAXMBWMX.Add(temp);
            }
        } 
    }
}
