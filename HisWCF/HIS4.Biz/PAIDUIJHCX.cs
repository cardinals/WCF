using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Data;
using SWSoft.Framework;
using System.Configuration;

namespace HIS4.Biz
{
    public class PAIDUIJHCX : IMessage<PAIDUIJHCX_IN, PAIDUIJHCX_OUT>
    {

        public override void ProcessMessage()
        {
            
            OutObject = new PAIDUIJHCX_OUT();
            string keShiDM = InObject.KESHIID;
            string paiDuiJHLSJL = ConfigurationManager.AppSettings["PaiDuiJHLSJL"];//排队叫号是否获取历史队列记录
            //KESHIID 不为空时 获取科室名称信息，进行队列检索
            //检索队列为科室类别的数据，是否有可以匹配的内容
            //检索队列为医生类别的数据，是否有可以匹配的内容
            //获取队列号
            //获取队列当前就诊号码（当前就诊号或最后一个就诊号）
            //KESHIID 为空时 获取所有的队列的信息
            //获取队列号 列表
            //循环获取队列当前就诊循环

            if (string.IsNullOrEmpty(paiDuiJHLSJL))
            {
                paiDuiJHLSJL = "0";
            }


            if (!string.IsNullOrEmpty(keShiDM))
            {
                //科室信息
                KESHIXX ksxx = new KESHIXX();
                string keShiXXSql = "select keshimc,keshiid from v_gy_keshi where keshiid = {0}";
                DataTable dtKSXX = DBVisitor.ExecuteTable(string.Format(keShiXXSql, keShiDM));//科室信息
                if (dtKSXX.Rows.Count > 0)
                {
                    ksxx.KESHIDM = dtKSXX.Rows[0]["keshiid"].ToString();
                    ksxx.KESHIMC = dtKSXX.Rows[0]["keshimc"].ToString();
                }
                //队列
                string duiLieKSSql = "select distinct duilieid from jh_duiliexx where to_char(xitongsj,'yyyy-mm-dd') = '{0}' "
                    + " and zuofeibz = 0 and duiliebh != 'DBA' and ((duilielbid = '01' and duiliebh ='{1}') or (duilielbid='02' and fuduilm ='{2}'))";
                DataTable dtDuiLieKS = DBVisitor.ExecuteTable(string.Format(duiLieKSSql, DateTime.Now.ToString("yyyy-MM-dd"), keShiDM, ksxx.KESHIMC));
                for (int i = 0; i < dtDuiLieKS.Rows.Count; i++)
                {
                    //队列详细信息
                    string duiLieID = dtDuiLieKS.Rows[i]["duilieid"].ToString();
                    string paiDuiXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz)  as shangxiawbz from jh_paiduidl where duilieid = '{0}'  and zhuangtaiid = '06' ";
                    DataTable dtPaiDuiXX = DBVisitor.ExecuteTable(string.Format(paiDuiXXSql, duiLieID));
                    if (dtPaiDuiXX.Rows.Count > 0)
                    {
                        PAIDUIJHXX pdjhxx = new PAIDUIJHXX();
                        string duiliexx = "select * from gy_weizhi a ,jh_duiliexx b where a.weizhiid=b.suozaizj and b.duilieid= '{0}'";
                        DataTable duiliexxtb = DBVisitor.ExecuteTable(string.Format(duiliexx, duiLieID));
                        if (duiliexxtb.Rows.Count > 0)//就诊位置
                        {
                            pdjhxx.SUOZAIZJ = duiliexxtb.Rows[0]["bieming"].ToString();
                        }
                        else
                        {
                            pdjhxx.SUOZAIZJ = "";

                        }
                        pdjhxx.PAIDUIID = dtPaiDuiXX.Rows[0]["duilieid"].ToString();//排队队列唯一编号
                        pdjhxx.DANGQIANHM = dtPaiDuiXX.Rows[0]["yewuxh"].ToString();//业务序号（挂号序号）
                        string GuaHaoId = (dtPaiDuiXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                        pdjhxx.SHANGXIAWBZ = dtPaiDuiXX.Rows[0]["shangxiawbz"].ToString();
                        pdjhxx.KESHIDM = ksxx.KESHIDM;//科室代码

                        pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                        pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", ksxx.KESHIDM)).ToString();
                        if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                        {
                            pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                        }
                        OutObject.PAIDUIJHMX.Add(pdjhxx);
                    }
                    else
                    {
                        if (paiDuiJHLSJL == "0")
                        {
                            //结束队列信息
                            string paiDuiJSXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz) as shangxiawbz from jh_jieshudl where duilieid = '{0}'";
                            DataTable dtPaiDuiJSXX = DBVisitor.ExecuteTable(string.Format(paiDuiJSXXSql, duiLieID));
                            if (dtPaiDuiJSXX.Rows.Count > 0)
                            {
                                PAIDUIJHXX pdjhxx = new PAIDUIJHXX();
                                string duiliexx = "select * from gy_weizhi a ,jh_duiliexx b where a.weizhiid=b.suozaizj and b.duilieid= '{0}'";
                                DataTable duiliexxtb = DBVisitor.ExecuteTable(string.Format(duiliexx, duiLieID));
                                if (duiliexxtb.Rows.Count > 0)//就诊位置
                                {
                                    pdjhxx.SUOZAIZJ = duiliexxtb.Rows[0]["bieming"].ToString();
                                }
                                else
                                {
                                    pdjhxx.SUOZAIZJ = "";

                                }
                                pdjhxx.PAIDUIID = dtPaiDuiJSXX.Rows[0]["duilieid"].ToString();//排队队列唯一编号
                                pdjhxx.DANGQIANHM = dtPaiDuiJSXX.Rows[0]["yewuxh"].ToString();//业务序号（挂号序号）
                                string GuaHaoId = (dtPaiDuiJSXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                                pdjhxx.SHANGXIAWBZ = dtPaiDuiJSXX.Rows[0]["shangxiawbz"].ToString();
                                pdjhxx.KESHIDM = ksxx.KESHIDM;
                                pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                                pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", ksxx.KESHIDM)).ToString();
                                if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                                {
                                    pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                                }
                                OutObject.PAIDUIJHMX.Add(pdjhxx);
                            }
                        }
                    }
                }
            }
            else
            {
                string duiLieKSSql = "select distinct duilieid from ( select duilieid ,ruduisj as riqi from jh_paiduidl union all select duilieid ,chuduisj as riqi from jh_jieshudl ) where to_char(riqi,'yyyy-mm-dd') = '{0}'";
                DataTable dtDuiLieXX = DBVisitor.ExecuteTable(string.Format(duiLieKSSql, DateTime.Now.ToString("yyyy-MM-dd")));

                for (int i = 0; i < dtDuiLieXX.Rows.Count; i++)
                {
                    //队列详细信息
                    string duiLieID = dtDuiLieXX.Rows[i]["duilieid"].ToString();
                    string paiDuiXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz) as shangxiawbz from jh_paiduidl where duilieid = '{0}'  and zhuangtaiid = '06' ";
                    DataTable dtPaiDuiXX = DBVisitor.ExecuteTable(string.Format(paiDuiXXSql, duiLieID));
                    if (dtPaiDuiXX.Rows.Count > 0)
                    {
                        PAIDUIJHXX pdjhxx = new PAIDUIJHXX();
                        string duiliexx = "select * from gy_weizhi a ,jh_duiliexx b where a.weizhiid=b.suozaizj and b.duilieid= '{0}'";
                        DataTable duiliexxtb = DBVisitor.ExecuteTable(string.Format(duiliexx, duiLieID));
                        if (duiliexxtb.Rows.Count > 0)//就诊位置
                        {
                            pdjhxx.SUOZAIZJ = duiliexxtb.Rows[0]["bieming"].ToString();
                        }
                        else
                        {
                            pdjhxx.SUOZAIZJ = "";

                        }
                        pdjhxx.PAIDUIID = dtPaiDuiXX.Rows[0]["duilieid"].ToString();//排队队列唯一编号
                        pdjhxx.DANGQIANHM = dtPaiDuiXX.Rows[0]["yewuxh"].ToString();//业务序号（挂号序号）
                        string GuaHaoId = (dtPaiDuiXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                        pdjhxx.SHANGXIAWBZ = dtPaiDuiXX.Rows[0]["shangxiawbz"].ToString();
                        pdjhxx.KESHIDM = DBVisitor.ExecuteScalar(string.Format("select guahaoks from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                        pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();

                        pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", pdjhxx.KESHIDM)).ToString();
                        if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                        {
                            pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                        }
                        OutObject.PAIDUIJHMX.Add(pdjhxx);
                    }
                    else
                    {
                        if (paiDuiJHLSJL == "0")
                        {
                            //结束队列信息
                            string paiDuiJSXXSql = "select distinct duilieid,yewuid,yewuxh,decode(shangxiawbz,0,1,1,2,shangxiawbz) as shangxiawbz from jh_jieshudl where duilieid = '{0}' ";
                            DataTable dtPaiDuiJSXX = DBVisitor.ExecuteTable(string.Format(paiDuiJSXXSql, duiLieID));
                            if (dtPaiDuiJSXX.Rows.Count > 0)
                            {
                                PAIDUIJHXX pdjhxx = new PAIDUIJHXX();
                                string duiliexx = "select * from gy_weizhi a ,jh_duiliexx b where a.weizhiid=b.suozaizj and b.duilieid= '{0}'";
                                DataTable duiliexxtb = DBVisitor.ExecuteTable(string.Format(duiliexx, duiLieID));
                                if (duiliexxtb.Rows.Count > 0)//就诊位置
                                {
                                    pdjhxx.SUOZAIZJ = duiliexxtb.Rows[0]["bieming"].ToString();
                                }
                                else
                                {
                                    pdjhxx.SUOZAIZJ = "";

                                }
                                pdjhxx.PAIDUIID = dtPaiDuiJSXX.Rows[0]["duilieid"].ToString();//排队队列唯一编号
                                pdjhxx.DANGQIANHM = dtPaiDuiJSXX.Rows[0]["yewuxh"].ToString();//业务序号（挂号序号）
                                string GuaHaoId = (dtPaiDuiJSXX.Rows[0]["yewuid"].ToString()).Split('|')[1].ToString();//获取挂号ID
                                pdjhxx.SHANGXIAWBZ = dtPaiDuiJSXX.Rows[0]["shangxiawbz"].ToString();
                                pdjhxx.KESHIDM = DBVisitor.ExecuteScalar(string.Format("select guahaoks from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();
                                pdjhxx.YISHENGDM = DBVisitor.ExecuteScalar(string.Format("select guahaoys from mz_guahao1 where guahaoid = '{0}'", GuaHaoId)).ToString();

                                pdjhxx.KESHIMC = DBVisitor.ExecuteScalar(string.Format("select keshimc from v_gy_keshi where keshiid ='{0}'", pdjhxx.KESHIDM)).ToString();
                                if (!string.IsNullOrEmpty(pdjhxx.YISHENGDM) && pdjhxx.YISHENGDM != "*")
                                {
                                    pdjhxx.YISHENGMC = DBVisitor.ExecuteScalar(string.Format("select zhigongxm from v_gy_zhigongxx where zhigongid = '{0}'", pdjhxx.YISHENGDM)).ToString();
                                }
                                OutObject.PAIDUIJHMX.Add(pdjhxx);
                            }
                        }
                    }
                }
            }
        }
    }
}
