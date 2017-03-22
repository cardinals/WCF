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
    public class PAIBANTZXX : IMessage<PAIBANTZXX_IN, PAIBANTZXX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new PAIBANTZXX_OUT();

            string kaiShiRq = InObject.KAISHIRQ;//开始日期
            string jieShuRq = InObject.JIESHURQ;//结束日期
            #region 基本入参判断
            if (string.IsNullOrEmpty(kaiShiRq)) {
                throw new Exception("开始日期不能为空！");
            }

            if (string.IsNullOrEmpty(jieShuRq))
            {
                throw new Exception("结束日期不能为空！");
            }
            #endregion
           
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select to_char(a.riqi,'yyyy-mm-dd') TINGZHENKSRQ, ");
            sbSql.Append("to_char(a.riqi,'yyyy-mm-dd') TINGZHENJSRQ, ");
            sbSql.Append("a.paibanid YIZHOUPBID,'' DANGTIANPBID, ");
            sbSql.Append("b.shangwuyyxh SHANGWUTZZT, ");
            sbSql.Append("b.xiawuyyxh XIAWUTZZT, ");
            sbSql.Append("a.guahaolb GUAHAOLB, ");
            sbSql.Append("a.keshiid KESHIDM, ");
            sbSql.Append("a.yishengid YISHENGDM, ");
             sbSql.Append("c.keshimc  KESHIMC, ");
             sbSql.Append("d.zhigongxm YISHENGXM ");
            sbSql.Append("from mz_guahaopb_ex a,mz_guahaoyyxh b ,gy_keshi c,gy_zhigongxx d where b.paibanid=a.paibanid and a.keshiid = c.keshiid and a.yishengid = d.zhigongid ");
            sbSql.Append("and (b.shangwuyyxh<0 or b.xiawuyyxh<0) ");
           
            if (kaiShiRq != "")
            {
                sbSql.Append("and a.riqi >= to_date('" + kaiShiRq + "','yyyy-mm-dd') ");
            }
            if (jieShuRq != "")
            {
                sbSql.Append("and a.riqi <= to_date ('" + Convert.ToDateTime(jieShuRq).ToShortDateString() + " 23:59:59" + "','yyyy-mm-dd hh24:mi:ss') ");
            }
            sbSql.Append(" order by a.riqi,a.keshiid ");
            DataTable dtPaiBanTzXX = DBVisitor.ExecuteTable(sbSql.ToString());

            if (dtPaiBanTzXX != null && dtPaiBanTzXX.Rows.Count > 0) {
                for (int i = 0; i < dtPaiBanTzXX.Rows.Count; i++)
                {
                    TINGZHENXX pbtzxx = new TINGZHENXX();
                    pbtzxx.TINGZHENKSRQ = dtPaiBanTzXX.Rows[i]["TINGZHENKSRQ"].ToString();
                    pbtzxx.TINGZHENJSRQ = dtPaiBanTzXX.Rows[i]["TINGZHENJSRQ"].ToString();
                    pbtzxx.DANGTIANPBID = dtPaiBanTzXX.Rows[i]["DANGTIANPBID"].ToString();
                    pbtzxx.SHANGWUTZZT = dtPaiBanTzXX.Rows[i]["SHANGWUTZZT"].ToString();
                    pbtzxx.XIAWUTZZT = dtPaiBanTzXX.Rows[i]["XIAWUTZZT"].ToString();
                    pbtzxx.GUAHAOLB = dtPaiBanTzXX.Rows[i]["GUAHAOLB"].ToString();
                    pbtzxx.KESHIDM = dtPaiBanTzXX.Rows[i]["KESHIDM"].ToString();
                    pbtzxx.YISHENGDM = dtPaiBanTzXX.Rows[i]["YISHENGDM"].ToString();
                    pbtzxx.KESHIMC = dtPaiBanTzXX.Rows[i]["KESHIMC"].ToString(); ;
                    pbtzxx.YISHENGXM = dtPaiBanTzXX.Rows[i]["YISHENGXM"].ToString(); ;
                    OutObject.TINGZHENMX.Add(pbtzxx);
                }
            }
        }
    }
}
