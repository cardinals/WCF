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
    public class JIANCHAJLCX : IMessage<JIANCHAJLCX_IN,JIANCHAJLCX_OUT>
    {
        public override void ProcessMessage()
        {
            this.OutObject = new JIANCHAJLCX_OUT();

            string bingRenID = InObject.BINGRENID;//病人ID
            string kaiShiRQ = InObject.KAISHIRQ;//开始日期
            string jieShuRQ = InObject.JIESHURQ;//结束日期
            string jiuZhenLY = InObject.JIUZHENLY;//就诊来源


            #region 基础入参判断
            //病人ID
            //if (string.IsNullOrEmpty(bingRenID)) {
            //    throw new Exception("病人编号获取失败！");
            //}
            //开始时间
            if (string.IsNullOrEmpty(kaiShiRQ))
            {
                kaiShiRQ = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
            }
            //结束时间
            if (string.IsNullOrEmpty(jieShuRQ))
            {
                jieShuRQ = DateTime.Now.ToString("yyyy-MM-dd");
            }
            //就诊来院
            if (string.IsNullOrEmpty(jiuZhenLY))
            {
                jiuZhenLY = "0,1";
            }
            else
            {
                if (jiuZhenLY != "0" && jiuZhenLY != "1")
                {
                    jiuZhenLY = "0,1";
                }
            }
            #endregion
            DataTable dtJianChaJL;
            if (string.IsNullOrEmpty(bingRenID))
            {
                string jianChaJLSQL = "select a.shenqindanid,a.yizhuid,a.yizhuxmid,a.yizhumc,a.jiuzhenid,a.bingrenzyid,a.bingrenid,a.bingrenxm,"
               + "a.shururen,a.shurusj,a.kaidanks,a.kaidanrq,a.jianchaks,a.jiancharq,a.menzhenzybz,a.dangqianzt,decode(a.dangqianzt,'9','已撤销','10','已撤销','7','已报告','8','已报告','未报告') as dangqianztmc,a.zhusu,a.jianyaobs,"
               + "a.jianchabw,a.jianchamd,a.jianchalx,a.tuidanren,a.tuidanrq,a.tuidanrxm  from yj_shenqingdan a where shurusj between to_date('{0} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{1} 23:59:59','yyyy-mm-dd hh24:mi:ss')  and a.menzhenzybz in ({2})"
               + " order by  a.kaidanrq desc ";
                dtJianChaJL = DBVisitor.ExecuteTable(string.Format(jianChaJLSQL, kaiShiRQ, jieShuRQ, jiuZhenLY));
            }
            else
            {
                string jianChaJLSQL = "select a.shenqindanid,a.yizhuid,a.yizhuxmid,a.yizhumc,a.jiuzhenid,a.bingrenzyid,a.bingrenid,a.bingrenxm,"
               + "a.shururen,a.shurusj,a.kaidanks,a.kaidanrq,a.jianchaks,a.jiancharq,a.menzhenzybz,a.dangqianzt,decode(a.dangqianzt,'9','已撤销','10','已撤销','7','已报告','8','已报告','未报告') as dangqianztmc,a.zhusu,a.jianyaobs,"
               + "a.jianchabw,a.jianchamd,a.jianchalx,a.tuidanren,a.tuidanrq,a.tuidanrxm  from yj_shenqingdan a where shurusj between to_date('{1} 00:00:00','yyyy-mm-dd hh24:mi:ss') and to_date('{2} 23:59:59','yyyy-mm-dd hh24:mi:ss') and bingrenid = '{0}' and a.menzhenzybz in ({3}) "
               + " order by  a.kaidanrq desc ";
               dtJianChaJL = DBVisitor.ExecuteTable(string.Format(jianChaJLSQL, bingRenID, kaiShiRQ, jieShuRQ, jiuZhenLY));
            }
           

            if (dtJianChaJL.Rows.Count > 0)
            {
                OutObject.JIANCHAJLTS = dtJianChaJL.Rows.Count.ToString();
                for (int i = 0; i < dtJianChaJL.Rows.Count; i++)
                {
                    JIANCHAJLXX jcjlxx = new JIANCHAJLXX();
                    jcjlxx.BINGRENID = dtJianChaJL.Rows[i]["bingrenid"].ToString();//病人ID
                    jcjlxx.SHENQINDANID = dtJianChaJL.Rows[i]["shenqindanid"].ToString();//申请单ID
                    jcjlxx.YIZHUID = dtJianChaJL.Rows[i]["yizhuid"].ToString();//医嘱ID
                    jcjlxx.YIZHUXMID = dtJianChaJL.Rows[i]["yizhuxmid"].ToString();//医嘱项目ID
                    jcjlxx.YIZHUMC = dtJianChaJL.Rows[i]["yizhumc"].ToString();//医嘱名称
                    jcjlxx.JIUZHENID = dtJianChaJL.Rows[i]["jiuzhenid"].ToString();//就诊ID
                    jcjlxx.BINGRENZYID = dtJianChaJL.Rows[i]["bingrenzyid"].ToString();//病人住院ID
                    jcjlxx.BINGRENXM = dtJianChaJL.Rows[i]["bingrenxm"].ToString();//病人姓名
                    jcjlxx.SHURUREN = dtJianChaJL.Rows[i]["shururen"].ToString();//输入人
                    jcjlxx.SHURUSJ = dtJianChaJL.Rows[i]["shurusj"].ToString();//输入时间
                    jcjlxx.KAIDANKS = dtJianChaJL.Rows[i]["kaidanks"].ToString();//开单科室
                    jcjlxx.KAIDANRQ = dtJianChaJL.Rows[i]["kaidanrq"].ToString();//开单日期
                    jcjlxx.MENZHENZYBZ = dtJianChaJL.Rows[i]["menzhenzybz"].ToString();//门诊住院标识
                    jcjlxx.DANGQIANZT = dtJianChaJL.Rows[i]["dangqianzt"].ToString();//当前状态
                    jcjlxx.DANGQIANZTMC = dtJianChaJL.Rows[i]["dangqianztmc"].ToString();//当前状态名称
                    jcjlxx.ZHUSU = dtJianChaJL.Rows[i]["zhusu"].ToString();//主诉
                    jcjlxx.JIANYAOBS = dtJianChaJL.Rows[i]["jianyaobs"].ToString();//简要病史
                    jcjlxx.JIANCHABW = dtJianChaJL.Rows[i]["jianchabw"].ToString();//检查部位
                    jcjlxx.JIANCHAMD = dtJianChaJL.Rows[i]["jianchamd"].ToString();//检查目的
                    jcjlxx.JIANCHALX = dtJianChaJL.Rows[i]["jianchalx"].ToString();//检查类型
                    jcjlxx.TUIDANREN = dtJianChaJL.Rows[i]["tuidanren"].ToString();//退单人
                    jcjlxx.TUIDANRQ = dtJianChaJL.Rows[i]["tuidanrq"].ToString();//退单日期
                    jcjlxx.TUIDANRXM = dtJianChaJL.Rows[i]["tuidanrxm"].ToString();//退单人姓名
                    OutObject.JIANCHAJLMX.Add(jcjlxx);
                }
            }
            else {
                throw new Exception("未找到相关的检查信息记录");
            }

        }
    }
}
