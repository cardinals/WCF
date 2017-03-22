using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using log4net;

namespace HIS4.Biz
{
    public class JIANCHAJGXX : IMessage<JIANCHAJGXX_IN, JIANCHAJGXX_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {
            this.OutObject = new JIANCHAJGXX_OUT();

            string kaishiRq =InObject.KAISHIRQ;//开始日期
            string jieshuRq = InObject.JIESHURQ;//结束日期
            string jiuzhenLy = InObject.JIUZHENLY;//就诊来源
            string bingrenId = InObject.BINGRENID;//病人id
            string xingMing = InObject.XINGMING;//姓名
            string jianChaDH = InObject.JIANCHADH;//检查单号
            #region 基本入参判断
            if (string.IsNullOrEmpty(bingrenId))
            {
                throw new Exception("未接收到病人唯一号!");
            }
            #endregion
            #region 语句拼装
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbSql_MZ = new StringBuilder();
            StringBuilder sbSql_ZY = new StringBuilder();
            sbSql_MZ.Append("select b.jiuzhenkh JIUZHENKH, ");
            sbSql_MZ.Append("a.jiuzhenid   YILIAOXH, ");
            sbSql_MZ.Append("a.jianchabwmc YILIAOMC, ");
            sbSql_MZ.Append("e.zhigonggh   SONGJIANYSGH, ");
            sbSql_MZ.Append("e.zhigongxm   SONGJIANYSXM, ");
            sbSql_MZ.Append("d.yuyuesqrq   SONGJIANSJ, ");
            sbSql_MZ.Append("NVL(D.JIANCHARQ,NVL(D.YUYUEQRRQ,D.KAIDANRQ)) JIANCHARQ, ");
            sbSql_MZ.Append("d.jiancharen  BIANGENGYSGH, ");
            sbSql_MZ.Append("f.zhigongxm   BIANGENGYSXM, ");
            sbSql_MZ.Append("a.baogaosj    BIANGENGRQ, ");
            sbSql_MZ.Append("d.yuyuesqr    SHENHEYSGH, ");
            sbSql_MZ.Append("h.zhigongxm   SHENHEYSXM, ");
            sbSql_MZ.Append("decode(d.dangqianzt,1,'新开单',9,'已撤销',8,'已打印',7,'已报告',6,'已完成',5,'已安排',4,'已预约',3,'待登记',2,'待划价',11,'已发送''未接收',10,'已退单')  JIANCHAZT, ");
            sbSql_MZ.Append("a.jianchasj   JIANCHAMS, ");
            sbSql_MZ.Append("a.zhenduanjg  JIANCHAJG, ");
            sbSql_MZ.Append("d.yuyueqrrq     SHENHERQ, ");
            sbSql_MZ.Append("a.SHENQINGDID JIANCHAH, ");
            sbSql_MZ.Append("b.binganhao   BINGANH, ");
            sbSql_MZ.Append("''            BINGQUMC, ");
            sbSql_MZ.Append("''            BINGQUCH, ");
            sbSql_MZ.Append("b.xingbie     BINGRENXB, ");
            sbSql_MZ.Append("c.nianling||c.nianlingdw BINGRENNL, ");
            sbSql_MZ.Append("g.keshimc       SONGJIANKSMC, ");
            sbSql_MZ.Append("d.yuyuesqrq     YUYUERQ, ");
            sbSql_MZ.Append("a.jianchabwid   JIANCHABWDM ");
            sbSql_MZ.Append("From yj_jianchabg a, gy_bingrenxx b ,zj_jiuzhenxx c,yj_shenqingdan d , ");
            sbSql_MZ.Append("gy_zhigongxx e , gy_zhigongxx f,gy_keshi g,gy_zhigongxx h, gy_yuanqu i ");
            sbSql_MZ.Append("where a.bingrenid = b.bingrenid  ");
            sbSql_MZ.Append("and a.jiuzhenid = c.jiuzhenid  ");
            sbSql_MZ.Append("and a.shenqingdid = d.shenqindanid ");
            sbSql_MZ.Append("and d.kaidanren = e.zhigongid(+) ");
            sbSql_MZ.Append("and d.jiancharen = f.zhigongid(+) ");
            sbSql_MZ.Append("and d.yuyuesqr = h.zhigongid(+) ");
            sbSql_MZ.Append("and g.keshiid=d.kaidanks ");
            sbSql_MZ.Append("and i.yuanquid = g.yuanquid ");
            if (!string.IsNullOrEmpty(kaishiRq))
                sbSql_MZ.Append(" and a.baogaosj >= to_date('" + kaishiRq + "','yyyy-mm-dd') ");
            if (!string.IsNullOrEmpty(jieshuRq))
                sbSql_MZ.Append(" and a.baogaosj <= to_date('" + Convert.ToDateTime(jieshuRq).ToString("yyyy-MM-dd") + " 23:59:59" + "','yyyy-mm-dd hh24:mi:ss') ");
            if (!string.IsNullOrEmpty(bingrenId) )
                sbSql_MZ.Append(" and b.bingrenid ='" + bingrenId + "' ");
            if (!string.IsNullOrEmpty(xingMing) )
                sbSql_MZ.Append(" and b.xingming ='" + xingMing + "' ");
            if (!string.IsNullOrEmpty(jianChaDH))
                sbSql_MZ.Append(" and a.shenqingdid ='" + jianChaDH + "' ");

            sbSql_ZY.Append("select b.jiuzhenkh JIUZHENKH, ");
            sbSql_ZY.Append("a.jiuzhenid   YILIAOXH, ");
            sbSql_ZY.Append("a.jianchabwmc YILIAOMC, ");
            sbSql_ZY.Append("e.zhigonggh   SONGJIANYSGH, ");
            sbSql_ZY.Append("e.zhigongxm   SONGJIANYSXM, ");
            sbSql_ZY.Append("d.yuyuesqrq   SONGJIANSJ, ");
            sbSql_ZY.Append("NVL(D.JIANCHARQ,NVL(D.YUYUEQRRQ,D.KAIDANRQ)) JIANCHARQ, ");
            sbSql_ZY.Append("d.jiancharen  BIANGENGYSGH, ");
            sbSql_ZY.Append("f.zhigongxm   BIANGENGYSXM, ");
            sbSql_ZY.Append("a.baogaosj    BIANGENGRQ, ");
            sbSql_ZY.Append("d.yuyuesqr    SHENHEYSGH, ");
            sbSql_ZY.Append("h.zhigongxm   SHENHEYSXM, ");
            sbSql_ZY.Append("decode(d.dangqianzt,1,'新开单',9,'已撤销',8,'已打印',7,'已报告',6,'已完成',5,'已安排',4,'已预约',3,'待登记',2,'待划价',11,'已发送''未接收',10,'已退单')  JIANCHAZT, ");
            sbSql_ZY.Append("a.jianchasj   JIANCHAMS, ");
            sbSql_ZY.Append("a.zhenduanjg  JIANCHAJG, ");
            sbSql_ZY.Append("d.yuyueqrrq     SHENHERQ, ");
            sbSql_ZY.Append("a.SHENQINGDID JIANCHAH, ");
            sbSql_ZY.Append("b.binganhao   BINGANH, ");
            sbSql_ZY.Append("c.dangqianbqmc BINGQUMC, ");
            sbSql_ZY.Append("c.dangqiancw   BINGQUCH, ");
            sbSql_ZY.Append("b.xingbie     BINGRENXB, ");
            sbSql_ZY.Append("c.nianling||c.nianlingdw BINGRENNL, ");
            sbSql_ZY.Append("g.keshimc       SONGJIANKSMC, ");
            sbSql_ZY.Append("d.yuyuesqrq     YUYUERQ, ");
            sbSql_ZY.Append("a.jianchabwid   JIANCHABWDM ");
            sbSql_ZY.Append("From yj_jianchabg a, gy_bingrenxx b ,zy_bingrenxx c,yj_shenqingdan d , ");
            sbSql_ZY.Append("gy_zhigongxx e , gy_zhigongxx f,gy_keshi g,gy_zhigongxx h, gy_yuanqu i ");
            sbSql_ZY.Append("where a.bingrenid = b.bingrenid  ");
            sbSql_ZY.Append("and a.bingrenzyid = c.bingrenzyid  ");
            sbSql_ZY.Append("and a.shenqingdid = d.shenqindanid ");
            sbSql_ZY.Append("and d.kaidanren = e.zhigongid(+) ");
            sbSql_ZY.Append("and d.jiancharen = f.zhigongid(+) ");
            sbSql_ZY.Append("and d.yuyuesqr = h.zhigongid(+) ");
            sbSql_ZY.Append("and g.keshiid=d.kaidanks ");
            sbSql_ZY.Append("and i.yuanquid = g.yuanquid ");
            if (!string.IsNullOrEmpty(kaishiRq) )
                sbSql_ZY.Append(" and a.baogaosj >= to_date('" + kaishiRq + "','yyyy-mm-dd') ");
            if (!string.IsNullOrEmpty(jieshuRq) )
                sbSql_ZY.Append(" and a.baogaosj <= to_date('" + Convert.ToDateTime(jieshuRq).ToString("yyyy-MM-dd") + " 23:59:59" + "','yyyy-mm-dd hh24:mi:ss') ");
            if (!string.IsNullOrEmpty(bingrenId))
                sbSql_ZY.Append(" and c.bingrenzyid ='" + bingrenId + "' ");
            if (!string.IsNullOrEmpty(xingMing) )
                sbSql_ZY.Append(" and b.xingming ='" + xingMing + "' ");
            if (!string.IsNullOrEmpty(jianChaDH) )
                sbSql_ZY.Append(" and a.shenqingdid ='" + jianChaDH + "' ");

            if (!string.IsNullOrEmpty(jiuzhenLy))
            {
                if (jiuzhenLy == "1") //门诊
                {
                    sbSql = sbSql_MZ;
                }
                else if (jiuzhenLy == "2")//住院
                {
                    sbSql = sbSql_ZY;
                }
            }
            else //所有
            {
                sbSql = sbSql_MZ;
                sbSql.Append("Union all ");
                sbSql.Append(sbSql_ZY);
            }
            #endregion
            DataTable dtJianChaXX = DBVisitor.ExecuteTable(sbSql.ToString());

            if (dtJianChaXX != null && dtJianChaXX.Rows.Count > 0)
            {
                for (int i = 0; i < dtJianChaXX.Rows.Count; i++)
                {
                    JIANCHAXXX jcxx = new JIANCHAXXX();
                    jcxx.JIUZHENKH = dtJianChaXX.Rows[i]["JIUZHENKH"].ToString();                    //就诊卡号    
                    jcxx.YILIAOXH = dtJianChaXX.Rows[i]["YILIAOXH"].ToString();                      //医疗序号    
                    jcxx.YILIAOMC = dtJianChaXX.Rows[i]["YILIAOMC"].ToString();                      //医疗名称    
                    jcxx.SONGJIANYSGH = dtJianChaXX.Rows[i]["SONGJIANYSGH"].ToString();              //送检医生工号
                    jcxx.SONGJIANYSXM = dtJianChaXX.Rows[i]["SONGJIANYSXM"].ToString();              //送检医生姓名
                    jcxx.SONGJIANSJ = dtJianChaXX.Rows[i]["SONGJIANSJ"].ToString();                  //送检时间    
                    jcxx.JIANCHARQ = dtJianChaXX.Rows[i]["JIANCHARQ"].ToString();                    //检查日期    
                    jcxx.BIANGENGYSGH = dtJianChaXX.Rows[i]["BIANGENGYSGH"].ToString();              //变更医生工号
                    jcxx.BIANGENGYSXM = dtJianChaXX.Rows[i]["BIANGENGYSXM"].ToString();              //变更医生姓名
                    jcxx.BIANGENGRQ = dtJianChaXX.Rows[i]["BIANGENGRQ"].ToString();                  //变更日期    
                    jcxx.SHENHEYSGH = dtJianChaXX.Rows[i]["SHENHEYSGH"].ToString();                  //审核医生工号
                    jcxx.SHENHEYSXM = dtJianChaXX.Rows[i]["SHENHEYSXM"].ToString();                  //审核医生姓名
                    jcxx.JIANCHAZT = dtJianChaXX.Rows[i]["JIANCHAZT"].ToString();                    //检查状态    
                    jcxx.JIANCHAMS = dtJianChaXX.Rows[i]["JIANCHAMS"].ToString();                    //检查描述    
                    jcxx.JIANCHAJG = dtJianChaXX.Rows[i]["JIANCHAJG"].ToString();                    //检查结果    
                    jcxx.SHENHERQ = dtJianChaXX.Rows[i]["SHENHERQ"].ToString();                      //审核日期    
                    //jcxx.STUDYUID = dtJianChaXX.Rows[i]["STUDYUID"].ToString();                      //STUDYUID    
                    jcxx.JIANCHAH = dtJianChaXX.Rows[i]["JIANCHAH"].ToString();                      //检查号码    
                    jcxx.BINGANH = dtJianChaXX.Rows[i]["BINGANH"].ToString();                        //病案号      
                    jcxx.BINGQUMC = dtJianChaXX.Rows[i]["BINGQUMC"].ToString();                      //病区名称    
                    jcxx.BINGQUCH = dtJianChaXX.Rows[i]["BINGQUCH"].ToString();                      //病区床号    
                    jcxx.BINGRENXB = dtJianChaXX.Rows[i]["BINGRENXB"].ToString();                    //病人性别    
                    jcxx.BINGRENNL = dtJianChaXX.Rows[i]["BINGRENNL"].ToString();                    //病人年龄    
                    jcxx.SONGJIANKSMC = dtJianChaXX.Rows[i]["SONGJIANKSMC"].ToString();              //送检科室名称
                    jcxx.YUYUERQ = dtJianChaXX.Rows[i]["YUYUERQ"].ToString();                        //预约日期    
                    jcxx.JIANCHABWDM = dtJianChaXX.Rows[i]["JIANCHABWDM"].ToString();                //检查部位代码
                    OutObject.JIANCHAXMX.Add(jcxx);
                }
            }
            else {
                throw new Exception("报告制作中,请耐心等待！");
            }
        }
    }
}
