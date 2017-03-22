using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class HUANZHEJHCX : IMessage<HUANZHEJHCX_IN, HUANZHEJHCX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new HUANZHEJHCX_OUT();

            string BingRenID = InObject.BINGRENID;

            //病人编号不为空时，获取病人所在队列的信息
            //获取指定队列号
            //获取当前就诊号码
            //获取病人队列号码
            if (!string.IsNullOrEmpty(BingRenID))
            {
                DataTable dt = DBVisitor.ExecuteTable(string.Format("select duilieid from jh_paiduidl where bingrenid={0} "
                    + " union all select duilieid from jh_jieshudl where bingrenid={0} ", BingRenID));
                if (dt.Rows.Count > 0)
                {
                    for (int dl = 0; dl < dt.Rows.Count; dl++)
                    {
                        PAIDUIJHXX pdjhxx = new PAIDUIJHXX();
                        #region 取病人队列信息
                        string duiLieSql = "select duilieid,yewuxh,zhuangtaiid from jh_paiduidl where bingrenid = {0}";
                        DataTable dtDuiLie = DBVisitor.ExecuteTable(string.Format(duiLieSql, BingRenID));
                        if (dtDuiLie.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtDuiLie.Rows.Count; i++)
                            {
                                pdjhxx.PAIDUIID = dtDuiLie.Rows[i]["duilieid"].ToString();
                                pdjhxx.BINGRENXH = dtDuiLie.Rows[i]["yewuxh"].ToString();
                                string ztxx = dtDuiLie.Rows[i]["zhuangtaiid"].ToString();
                                if (ztxx == "06")
                                {
                                    pdjhxx.BINGRENZT = "就诊中";
                                }
                                else if (ztxx == "02")
                                {
                                    pdjhxx.BINGRENZT = "已过号";
                                }
                                else
                                {
                                    pdjhxx.BINGRENZT = "等待就诊";
                                }
                            }
                        }
                        else
                        {
                            string jieShuDuiLieSql = "select duilieid,yewuxh,zhuangtaiid from jh_jieshudl where bingrenid = {0}";
                            DataTable jieShuDuiLie = DBVisitor.ExecuteTable(string.Format(jieShuDuiLieSql, BingRenID));
                            for (int i = 0; i < jieShuDuiLie.Rows.Count; i++)
                            {
                                pdjhxx.PAIDUIID = jieShuDuiLie.Rows[i]["duilieid"].ToString();
                                pdjhxx.BINGRENXH = jieShuDuiLie.Rows[i]["yewuxh"].ToString();
                                pdjhxx.BINGRENZT = "已就诊";
                            }
                        }
                        #endregion
                        #region 取队列信息
                        string duiLieID = pdjhxx.PAIDUIID;
                        string paiDuiXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz) as shangxiawbz from jh_paiduidl where duilieid = '{0}'  and zhuangtaiid = '06' ";
                        DataTable dtPaiDuiXX = DBVisitor.ExecuteTable(string.Format(paiDuiXXSql, duiLieID));
                        if (dtPaiDuiXX.Rows.Count > 0)
                        {
                            pdjhxx.PAIDUIID = dtPaiDuiXX.Rows[0]["duilieid"].ToString();
                            pdjhxx.DANGQIANHM = dtPaiDuiXX.Rows[0]["yewuxh"].ToString();
                            string GuaHaoId = (dtPaiDuiXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                            pdjhxx.SHANGXIAWBZ = dtPaiDuiXX.Rows[0]["shangxiawbz"].ToString();
                            pdjhxx.KESHIDM = DBVisitor.ExecuteScalar(string.Format("select guahaoks from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                            pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                            pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", pdjhxx.KESHIDM)).ToString();
                            if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                            {
                                pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                            }
                        }
                        else
                        {
                            //结束队列信息
                            string paiDuiJSXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz) as shangxiawbz from jh_jieshudl where duilieid = '{0}' ";
                            DataTable dtPaiDuiJSXX = DBVisitor.ExecuteTable(string.Format(paiDuiJSXXSql, duiLieID));
                            if (dtPaiDuiJSXX.Rows.Count > 0)
                            {
                                pdjhxx.PAIDUIID = dtPaiDuiJSXX.Rows[0]["duilieid"].ToString();
                                pdjhxx.DANGQIANHM = dtPaiDuiJSXX.Rows[0]["yewuxh"].ToString();
                                string GuaHaoId = (dtPaiDuiJSXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                                pdjhxx.SHANGXIAWBZ = dtPaiDuiJSXX.Rows[0]["shangxiawbz"].ToString();
                                pdjhxx.KESHIDM = DBVisitor.ExecuteScalar(string.Format("select guahaoks from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                                pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                                pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", pdjhxx.KESHIDM)).ToString();
                                if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                                {
                                    pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                                }
                            }
                        }
                        #endregion
                        OutObject.HUANZHEJHMX.Add(pdjhxx);
                    }
                }
                else
                {
                    throw new Exception("未找到病人的排队信息！");
                }
            }
        }
    }
}
