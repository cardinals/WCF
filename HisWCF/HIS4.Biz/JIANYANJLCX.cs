using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class JIANYANJLCX : IMessage<JIANYANJLCX_IN, JIANYANJLCX_OUT>
    {

        public override void ProcessMessage()
        {
            this.OutObject = new JIANYANJLCX_OUT();

            string bingRenID = InObject.BINGRENID;//病人ID
            string kaiShiRQ = InObject.KAISHIRQ;//开始日期
            string jieShuRQ = InObject.JIESHURQ;//结束日期
            string jiuZhenLY = InObject.JIUZHENLY;//就诊来源 0 门诊 1 住院 空全部

            #region 基本入参判断
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("获取病人信息失败！");
            }

            if (string.IsNullOrEmpty(kaiShiRQ)) {
                kaiShiRQ = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(jieShuRQ)) {
                jieShuRQ = DateTime.Now.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrEmpty(jiuZhenLY))
            {
                jiuZhenLY = "0,1";
            }
            else {
                if (jiuZhenLY != "0" && jiuZhenLY != "1") {
                    jiuZhenLY = "0,1";
                }
            }
            #endregion
             DataTable dtJianYanJL;
             if (string.IsNullOrEmpty(bingRenID))
             {
                 String jianYanJLSql = " select TIAOMAH from v_bp_lis_patientinfo where  kaidanrq between to_date( '{0} 00:00:00' ,'yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss')   and jiuzhenly in ({2}) group by tiaomah ";
                 dtJianYanJL = DBVisitor.ExecuteTable(string.Format(jianYanJLSql, kaiShiRQ, jieShuRQ, jiuZhenLY));
             }
             else
             {
                 String jianYanJLSql = " select TIAOMAH from v_bp_lis_patientinfo where  kaidanrq between to_date( '{1} 00:00:00' ,'yyyy-mm-dd hh24:mi:ss') and to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss')   and bingrenid = '{0}' and jiuzhenly in ({3}) group by tiaomah ";
                 dtJianYanJL = DBVisitor.ExecuteTable(string.Format(jianYanJLSql, bingRenID, kaiShiRQ, jieShuRQ, jiuZhenLY));
             }
            if (dtJianYanJL.Rows.Count <= 0)
            {
                throw new Exception("未找到相关的检验记录，请确认！");
            }
            else {
                OutObject.JIANYANJLTS = dtJianYanJL.Rows.Count.ToString();
                for (int i = 0; i < dtJianYanJL.Rows.Count; i++) {
                    String jianYanJLMXSql = " select * from v_bp_lis_patientinfo where tiaomah ={0}";
                    DataTable dtJianYanMXJL = DBVisitor.ExecuteTable(string.Format(jianYanJLMXSql, dtJianYanJL.Rows[i]["TIAOMAH"].ToString()));
                    if (dtJianYanMXJL.Rows.Count > 0)
                    {
                        JIANYANJLXX jyjlxx = new JIANYANJLXX();
                        jyjlxx.BINGRENID = dtJianYanMXJL.Rows[0]["BINGRENID"].ToString();//病人ID
                        jyjlxx.JIUZHENLY = dtJianYanMXJL.Rows[0]["JIUZHENLY"].ToString();//就诊来源
                        jyjlxx.KAIDANSJ = dtJianYanMXJL.Rows[0]["KAIDANRQ"].ToString();//开单时间
                        jyjlxx.YIZHUID = dtJianYanMXJL.Rows[0]["YIZHUID"].ToString();//医嘱ID
                        jyjlxx.SHENQINGDID = dtJianYanMXJL.Rows[0]["SHENQINGDID"].ToString();//申请单ID
                        jyjlxx.MENZHENID = dtJianYanMXJL.Rows[0]["MENZHENID"].ToString();//门诊ID
                        jyjlxx.ZHUYUANID = dtJianYanMXJL.Rows[0]["ZHUYUANID"].ToString();//住院ID
                        jyjlxx.BINGRENCW = dtJianYanMXJL.Rows[0]["BINGRENCW"].ToString();//病人床位
                        jyjlxx.BINGRENBQ = dtJianYanMXJL.Rows[0]["BINGRENBQ"].ToString();//病人病区
                        jyjlxx.BINGRENKS = dtJianYanMXJL.Rows[0]["BINGRENKS"].ToString();//病人科室
                        jyjlxx.KAIDANKSDM = dtJianYanMXJL.Rows[0]["KAIDANKSDM"].ToString();//开单科室代码
                        jyjlxx.KAIDANYSDM = dtJianYanMXJL.Rows[0]["KAIDANYSDM"].ToString();//开单医生代码
                        jyjlxx.KAIDANKSMC = dtJianYanMXJL.Rows[0]["KAIDANKSMC"].ToString();//开单科室名称
                        jyjlxx.KAIDANYSXM = dtJianYanMXJL.Rows[0]["KAIDANYSXM"].ToString();//开单医生姓名
                        for (int j = 0; j < dtJianYanMXJL.Rows.Count; j++)//检验项目信息
                        {
                            jyjlxx.JIANYANXMMX.Add(new JIANYANXMXX(dtJianYanMXJL.Rows[j]["JIANYANXMID"].ToString(),dtJianYanMXJL.Rows[j]["JIANYANXMMC"].ToString()));
                        }
                        jyjlxx.CAIJIYSDM = dtJianYanMXJL.Rows[0]["CAIJIYSDM"].ToString();//采集医生代码
                        jyjlxx.CAIJIYSXM = dtJianYanMXJL.Rows[0]["CAIJIYSXM"].ToString();//采集医生姓名
                        jyjlxx.CAIJIKSDM = dtJianYanMXJL.Rows[0]["CAIJIKSDM"].ToString();//采集科室代码
                        jyjlxx.CAIJIKSMC = dtJianYanMXJL.Rows[0]["CAIJIKSMC"].ToString();//采集科室名称
                        jyjlxx.TIAOMAH = dtJianYanMXJL.Rows[0]["TIAOMAH"].ToString();//条码号
                        jyjlxx.ZHENDUAN = dtJianYanMXJL.Rows[0]["ZHENDUAN"].ToString();//诊断
                        jyjlxx.SHENHEBZ = dtJianYanMXJL.Rows[0]["SHENHEBZ"].ToString();//审核标识
                        jyjlxx.SHENHEREN = dtJianYanMXJL.Rows[0]["SHENHEREN"].ToString();//审核人
                        jyjlxx.SHENHERXM = dtJianYanMXJL.Rows[0]["SHENHERXM"].ToString();//审核人姓名
                        jyjlxx.SHENHERQ = dtJianYanMXJL.Rows[0]["SHENHERQ"].ToString();//审核日期

                        OutObject.JIANYANJLMX.Add(jyjlxx);
                    }
                }
            }
        }
    }
}
