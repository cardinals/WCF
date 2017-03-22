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
using HIS4.Schemas.DATAENTITY;


namespace HIS4.Biz
{
    public class DIANZIZHXX : IMessage<DIANZIZHXX_IN, DIANZIZHXX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new DIANZIZHXX_OUT();
            string jiuZhenKH = InObject.JIUZHENKH;//就诊卡号
            string shijianFW = InObject.SHIJIANFW;//时间范围（天）
            string defaultShiJianFW = ConfigurationManager.AppSettings["MORENGSJFW"];//默认时间范围

            if (string.IsNullOrEmpty(shijianFW) || (!string.IsNullOrEmpty(shijianFW) && shijianFW == "0"))
            {
                if (string.IsNullOrEmpty(defaultShiJianFW) || (!string.IsNullOrEmpty(defaultShiJianFW) && defaultShiJianFW == "0"))
                {
                    shijianFW = "30";
                }
                else
                {
                    shijianFW = defaultShiJianFW;
                }
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select bingrenid from gy_bingrenxx where jiuzhenkh = '" + jiuZhenKH + "'");
            DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
            if (dt.Rows.Count <= 0)//返回数据空
            {
                throw new Exception("该卡未建档，请前往人工服务台建档！");
            }

            string bingRenID = dt.Rows[0]["bingrenid"].ToString();
            StringBuilder sbFYMX = new StringBuilder();
            string yewulxx = "";
            if (InObject.YWLX == "1")
            { //1充值
                yewulxx = " and (a.yewulx='1' or a.yewulx='13') ";

            }
            else if (InObject.YWLX == "2")
            {//取现
                yewulxx = " and a.yewulx='2' ";
            }
            sbFYMX.Append("select to_char(a.caozuorq,'yyyy-mm-dd hh24:mi:ss') as CAOZUORQ,b.ZHIFUMC as ZHIFUMC,a.fashengje,a.bencije,a.shiyongje,a.caozuoyuan,a.jiaoyiid from " +
                         "gy_zijinmxz a ,gy_zhifufs b where a.fashengje != 0 and  a.zhifufs = b.ZHIFUFSID and " +
                         "a.caozuorq > sysdate - " + shijianFW + " and a.bingrenid = '" + bingRenID+"' " + yewulxx + " order by caozuorq desc,zijinmxzid desc");

            DataTable dtFYMX = DBVisitor.ExecuteTable(sbFYMX.ToString());
            if (dt.Rows.Count <= 0)//返回数据空
            {
                throw new Exception("近期内没有院内电子账户消费记录，请核实！");
            }
            else
            {
                OutObject.JIUZHENKH = jiuZhenKH;
                OutObject.TOTALCOUNT = dtFYMX.Rows.Count.ToString();

                for (int i = 0; i < dtFYMX.Rows.Count; i++)
                {
                    DIANZIZHXFMX xfmx = new DIANZIZHXFMX();
                    xfmx.CAOZUORQ = dtFYMX.Rows[i]["CAOZUORQ"].ToString();
                    xfmx.ZHIFUMC = dtFYMX.Rows[i]["ZHIFUMC"].ToString();
                    xfmx.FASHENGJE = dtFYMX.Rows[i]["FASHENGJE"].ToString();
                    xfmx.BENCIJE = dtFYMX.Rows[i]["BENCIJE"].ToString();
                    xfmx.SHIYONGJE = dtFYMX.Rows[i]["SHIYONGJE"].ToString();
                    xfmx.JIAOYIBH = dtFYMX.Rows[i]["jiaoyiid"].ToString();
                    xfmx.CAOZUOYUAN = dtFYMX.Rows[i]["caozuoyuan"].ToString();
                    OutObject.FEIYONGMX.Add(xfmx);
                }
            }

        }
    }
}
