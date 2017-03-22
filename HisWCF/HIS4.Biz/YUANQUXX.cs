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
    public class YUANQUXX : IMessage<YUANQUXX_IN, YUANQUXX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new YUANQUXX_OUT();

            string sqlBuf = "SELECT yuanquid,yuanqumc FROM v_GY_YUANQU ";
            DataTable dtYuanQuXX = DBVisitor.ExecuteTable(sqlBuf);
            for (int i = 0; i < dtYuanQuXX.Rows.Count; i++) {

                Schemas.YUANQUXX yqxx = new Schemas.YUANQUXX();
                yqxx.YUANQUID = dtYuanQuXX.Rows[i]["yuanquid"].ToString();
                yqxx.YUANQUMC = dtYuanQuXX.Rows[i]["yuanqumc"].ToString();
                OutObject.YUANQUMX.Add(yqxx);
            }
        } 
    }
}
