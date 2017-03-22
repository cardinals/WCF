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
    public class CHUYUANXJ : IMessage<CHUYUANXJ_IN, CHUYUANXJ_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new CHUYUANXJ_OUT();
            string bingRenId = InObject.BINGRENID;//病人住院ID

            #region 基本入参判断
            if (string.IsNullOrEmpty(bingRenId))
            {
                throw new Exception("病人ID不能为空！");
            }
            #endregion

            StringBuilder sbSql = new StringBuilder("select T.BINGANHAO BIGNANH,ZAIYUANZT,T.XINGMING BINGRENXM, ");
            sbSql.Append("JIUZHENKH,SHENFENZH,ZHIYE,XINGBIE,T.CHUSHENGDI CHUSHENGGD,NIANLING,T.LIANXIREN LIANXIR, ");
            sbSql.Append("LIANXIRDH,T.JIATINGDZ XIANZHUZ,HUNYIN,MINZU,to_char(t.RUYUANRQ,'yyyy-mm-dd hh24:mi:ss') RUYUANRQ, ");
            sbSql.Append("to_char(t.chuyuanrq,'yyyy-mm-dd hh24:mi:ss') CHUYUANRQ,T.DANGQIANCW CHUANWEI,T.ZHUYUANYSXM ZHUZHIYS, ");
            sbSql.Append("T.RUYUANKSMC KESHIMC,'' BINGLIZD,'' RUYUNAQK,'' CHUYUANQK,T.RUYUANZDMC RUYUANZD, ");
            sbSql.Append("T.CHUYUANZDMC CHUYUANZD,'' CHUYUANYZ ,extract(XMLTYPE(XMLVALUE), '//入院情况//child::text()') .getstringval() RUYUANQK, ");
            sbSql.Append("extract(XMLTYPE(XMLVALUE), '//出院情况//child::text()') .getstringval() CHUYUANQK, ");
            sbSql.Append("nvl((select JIBINGMC from EMR3.V_BL_BINGRENZDQK WHERE zhenduanlx =10 AND bingrenid = '" + bingRenId + "' and ROWNUM = 1),'') AS BINGLIZD ");
            sbSql.Append("FROM ZY_BINGRENXX T ,emr3.zy_doc_binglijlxml_v4 A, emr3.ZY_DOC_BINGLIJL_V4 B ");
            //sbSql.Append("FROM ZY_BINGRENXX T ");
            sbSql.Append("WHERE T.BINGRENZYID = B.BINGRENID AND A.BINGLIJLID = B.BINGLIJLID and B.mobanlx = 'CYJL' ");
            sbSql.Append("AND T.BINGRENZYID = '" + bingRenId + "'");


            DataTable dtChuYuanXX = DBVisitor.ExecuteTable(sbSql.ToString());

            if (dtChuYuanXX != null && dtChuYuanXX.Rows.Count > 0)
            {
                for (int i = 0; i < dtChuYuanXX.Rows.Count; i++)
                {

                    BINGRENCYXX brzyxx = new BINGRENCYXX();
                    brzyxx.BIGNANH = dtChuYuanXX.Rows[i]["BIGNANH"].ToString();//病案号
                    brzyxx.ZAIYUANZT = dtChuYuanXX.Rows[i]["ZAIYUANZT"].ToString();//在院状态
                    brzyxx.BINGRENXM = dtChuYuanXX.Rows[i]["BINGRENXM"].ToString();//病人姓名
                    brzyxx.JIUZHENKH = dtChuYuanXX.Rows[i]["JIUZHENKH"].ToString();//就诊卡号
                    brzyxx.SHENFENZH = dtChuYuanXX.Rows[i]["SHENFENZH"].ToString();//身份证号
                    brzyxx.ZHIYE = dtChuYuanXX.Rows[i]["ZHIYE"].ToString();//职业
                    brzyxx.XINGBIE = dtChuYuanXX.Rows[i]["XINGBIE"].ToString();//性别
                    brzyxx.CHUSHENGGD = dtChuYuanXX.Rows[i]["CHUSHENGGD"].ToString();//出生地
                    brzyxx.NIANLING = dtChuYuanXX.Rows[i]["NIANLING"].ToString();//年龄
                    brzyxx.LIANXIR = dtChuYuanXX.Rows[i]["LIANXIR"].ToString();//联系人
                    brzyxx.LIANXIRDH = dtChuYuanXX.Rows[i]["LIANXIRDH"].ToString();//联系人电话
                    brzyxx.XIANZHUZ = dtChuYuanXX.Rows[i]["XIANZHUZ"].ToString();//现住址
                    brzyxx.HUNYIN = dtChuYuanXX.Rows[i]["HUNYIN"].ToString();//婚姻
                    brzyxx.MINZU = dtChuYuanXX.Rows[i]["MINZU"].ToString();//民族
                    brzyxx.RUYUANRQ = dtChuYuanXX.Rows[i]["RUYUANRQ"].ToString();//入院日期
                    brzyxx.CHUYUANRQ = dtChuYuanXX.Rows[i]["CHUYUANRQ"].ToString();//出院日期
                    brzyxx.CHUANWEI = dtChuYuanXX.Rows[i]["CHUANWEI"].ToString();//床位
                    brzyxx.ZHUZHIYS = dtChuYuanXX.Rows[i]["ZHUZHIYS"].ToString();//主治医生
                    brzyxx.KESHIMC = dtChuYuanXX.Rows[i]["KESHIMC"].ToString();//科室名称
                    brzyxx.BINGLIZD = dtChuYuanXX.Rows[i]["BINGLIZD"].ToString();//病理诊断
                    brzyxx.RUYUNAQK = dtChuYuanXX.Rows[i]["RUYUNAQK"].ToString();//入院情况
                    brzyxx.CHUYUANQK = dtChuYuanXX.Rows[i]["CHUYUANQK"].ToString();//出院情况
                    brzyxx.RUYUANZD = dtChuYuanXX.Rows[i]["RUYUANZD"].ToString();//入院诊断
                    brzyxx.CHUYUANZD = dtChuYuanXX.Rows[i]["CHUYUANZD"].ToString();//出院诊断
                    brzyxx.CHUYUANYZ = dtChuYuanXX.Rows[i]["CHUYUANYZ"].ToString();//出院医嘱
                    OutObject.CHUYUANXJXX.Add(brzyxx);
                }
            }

        }
    }
}
