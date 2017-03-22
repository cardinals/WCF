using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIS4.Schemas;
using JYCS.Schemas;
using System.Data;
using SWSoft.Framework;
using System.Data.OracleClient;
using System.Data.Common;

namespace HIS4.Biz
{
    public class ZHUYUANFYMX : IMessage<ZHUYUANFYMX_IN, ZHUYUANFYMX_OUT>
    {

        public override void ProcessMessage()
        {
            string bingrenZYId = InObject.BINGRENZYID;//病人ID
            string kaishiRq = InObject.KAISHIRQ;//开始日期
            string jieshuRq = InObject.JIESHURQ;//结束日期
            string jiaoyiMm = InObject.JIAOYANMM;//交易密码
            string tongjiFs = InObject.TONGJIFS;//统计方式
            string tongJiFL = InObject.TONGJIFL;//统计分类

            OutObject = new ZHUYUANFYMX_OUT();

            #region 基本入参判断
            if (string.IsNullOrEmpty(bingrenZYId))
            {
                throw new Exception("病人ID不能为空");
            }

            //if (string.IsNullOrEmpty(jiaoyiMm)) {
            //    throw new Exception("交易密码不能为空");
            //}

            if (string.IsNullOrEmpty(kaishiRq))
            {
                string sqlRYRQ = "select to_char(ruyuanrq,'yyyy-mm-dd') from zy_bingrenxx where bingrenzyid = '{0}'";
                kaishiRq = DBVisitor.ExecuteScalar(string.Format(sqlRYRQ, bingrenZYId)).ToString();
            }

            if (string.IsNullOrEmpty(jieshuRq))
            {

                string sqlJSRQ = "select to_char(sysdate,'yyyy-mm-dd') from dual";
                jieshuRq = DBVisitor.ExecuteScalar(sqlJSRQ).ToString();
            }

            if (string.IsNullOrEmpty(tongjiFs))
            {
                tongjiFs = "2";
            }
            else if (tongjiFs != "1" && tongjiFs != "2")
            {
                throw new Exception("统计方式必须为：1：日清单；2：汇总清单，请选择正确的统计方式");
            }
            #endregion


            if (tongjiFs == "1")
            {
                if (string.IsNullOrEmpty(kaishiRq)) {
                    throw new Exception("请确认需查询日期！");
                }

                StringBuilder sbSql = new StringBuilder("SELECT to_char(a.jifeirq,'yyyy-mm-dd') jifeirq,b.hesuanxmmc as feiyonglx,a.xiangmuid as xiangmuxh, ");
                sbSql.Append("a.xiangmuid as xiangmucddm,a.xiangmumc, b.hesuanxmid as xiangmugl,b.hesuanxmmc as xiangmuglmc,a.yaopingg as xiangmugg,'' xiangmujx, ");
                sbSql.Append("a.jijiadw xiangmudw,'' xiangmucdmc,to_char(a.jiesuanjia,'FM9999999999999990.0099') as danjia,a.shuliang,to_char(round(a.jiesuanje,2),'FM9999999999999990.00') as jine,a.yibaodj, ");
                sbSql.Append("a.yibaozfbl,a.xianjia as xiangmuxj,a.zifeije,a.zilije,a.kaidanks as kaidanksdm,a.kaidanksmc, ");
                sbSql.Append("a.kaidanys as kaidanysdm,a.kaidanysxm ");
                sbSql.Append("FROM ZY_FEIYONG1 a,gy_hesuanxm b,zy_bingrenxx c,gy_xiangmulx d ");
                sbSql.Append("WHERE b.hesuanxmid(+)=a.hesuanxm and c.BINGRENZYID = a.bingrenzyid and d.daimaid(+) = a.xiangmulx ");
                sbSql.Append(" and a.shuliang > 0 and a.feiyongid not in (select yuanfeiyid from zy_feiyong1 where bingrenzyid = '" + bingrenZYId + "' and shuliang < 0) ");
                sbSql.Append("and  (c.bingrenzyid='" + bingrenZYId + "' or a.MUQINZYID ='" + bingrenZYId + "') " );
                sbSql.Append("and a.jifeirq>=to_date('" + kaishiRq + " 00:00:00','yyyy-mm-dd hh24:mi:ss') ");
                sbSql.Append("and a.jifeirq<=to_date('" + kaishiRq + " 23:59:59','yyyy-mm-dd hh24:mi:ss') ");
                sbSql.Append(" order by a.hesuanxm,a.jifeirq desc ");
                DataTable dtFYXX = DBVisitor.ExecuteTable(sbSql.ToString());

                if (dtFYXX.Rows.Count > 0)
                {
                    OutObject.FEIYONGMXTS = dtFYXX.Rows.Count.ToString();//费用明细条数

                    for (int i = 0; i < dtFYXX.Rows.Count; i++)
                    {
                        FEIYONGXX_ZY zyFyxx = new FEIYONGXX_ZY();
                        zyFyxx.JIFEIRQ = dtFYXX.Rows[i]["JIFEIRQ"].ToString();//计费日期
                        zyFyxx.FEIYONGLX = dtFYXX.Rows[i]["FEIYONGLX"].ToString();//费用类型
                        zyFyxx.XIANGMUXH = dtFYXX.Rows[i]["XIANGMUXH"].ToString();//项目序号
                        zyFyxx.XIANGMUCDDM = dtFYXX.Rows[i]["XIANGMUCDDM"].ToString();//项目产品代码
                        zyFyxx.XIANGMUMC = dtFYXX.Rows[i]["XIANGMUMC"].ToString();//项目名称
                        zyFyxx.XIANGMUGL = dtFYXX.Rows[i]["XIANGMUGL"].ToString();//项目归类
                        zyFyxx.XIANGMUGLMC = dtFYXX.Rows[i]["XIANGMUGLMC"].ToString();//项目归类名称
                        zyFyxx.XIANGMUGG = dtFYXX.Rows[i]["XIANGMUGG"].ToString();//项目规格
                        zyFyxx.XIANGMUJX = dtFYXX.Rows[i]["XIANGMUJX"].ToString();//项目剂型
                        zyFyxx.XIANGMUDW = dtFYXX.Rows[i]["XIANGMUDW"].ToString();//项目单位
                        zyFyxx.XIANGMUCDMC = dtFYXX.Rows[i]["XIANGMUCDMC"].ToString();//项目产地名称
                        zyFyxx.DANJIA = dtFYXX.Rows[i]["DANJIA"].ToString();//单价
                        zyFyxx.SHULIANG = dtFYXX.Rows[i]["SHULIANG"].ToString();//数量
                        zyFyxx.JINE = dtFYXX.Rows[i]["JINE"].ToString();//金额
                        zyFyxx.YIBAODJ = dtFYXX.Rows[i]["YIBAODJ"].ToString();//医保等级
                        zyFyxx.YIBAOZFBL = dtFYXX.Rows[i]["YIBAOZFBL"].ToString();//医保自负比例
                        zyFyxx.XIANGMUXJ = dtFYXX.Rows[i]["XIANGMUXJ"].ToString();//项目限价
                        zyFyxx.ZIFEIJE = dtFYXX.Rows[i]["ZIFEIJE"].ToString();//自费金额
                        zyFyxx.ZILIJE = dtFYXX.Rows[i]["ZILIJE"].ToString();//自理金额
                        zyFyxx.KAIDANKSDM = dtFYXX.Rows[i]["KAIDANKSDM"].ToString();//开单科室代码
                        zyFyxx.KAIDANKSMC = dtFYXX.Rows[i]["KAIDANKSMC"].ToString();//开单科室名称
                        zyFyxx.KAIDANYSDM = dtFYXX.Rows[i]["KAIDANYSDM"].ToString();//开单医生代码
                        zyFyxx.KAIDANYSXM = dtFYXX.Rows[i]["KAIDANYSXM"].ToString();//开单医生姓名

                        OutObject.FEIYONGMX_ZY.Add(zyFyxx);
                    }

                }
            }
            else if (tongjiFs == "2")
            {
                StringBuilder sbSql = new StringBuilder("SELECT to_char(a.jifeirq,'yyyy-mm-dd') jifeirq,b.hesuanxmmc as feiyonglx,a.xiangmuid as xiangmuxh, ");
                sbSql.Append("a.xiangmuid as xiangmucddm,a.xiangmumc, b.hesuanxmid as xiangmugl,b.hesuanxmmc as xiangmuglmc,a.yaopingg as xiangmugg,'' xiangmujx, ");
                sbSql.Append("a.jijiadw xiangmudw,'' xiangmucdmc,to_char(a.jiesuanjia,'FM9999999999999990.0099') as danjia,a.shuliang,to_char(round(a.jiesuanje,2),'FM9999999999999990.00') as jine,a.yibaodj, ");
                sbSql.Append("a.yibaozfbl,a.xianjia as xiangmuxj,a.zifeije,a.zilije,a.kaidanks as kaidanksdm,a.kaidanksmc, ");
                sbSql.Append("a.kaidanys as kaidanysdm,a.kaidanysxm ");
                sbSql.Append("FROM ZY_FEIYONG1 a,gy_hesuanxm b,zy_bingrenxx c,gy_xiangmulx d ");
                sbSql.Append("WHERE b.hesuanxmid(+)=a.hesuanxm and c.BINGRENZYID = a.bingrenzyid and d.daimaid(+) = a.xiangmulx ");
                sbSql.Append(" and a.shuliang > 0 and a.feiyongid not in (select yuanfeiyid from zy_feiyong1 where bingrenzyid = '" + bingrenZYId + "' and shuliang < 0) ");
                sbSql.Append("and c.bingrenzyid='" + bingrenZYId + "' ");

                if (kaishiRq != string.Empty)
                    sbSql.Append("and a.jifeirq>=to_date('" + kaishiRq + " 00:00:00','yyyy-mm-dd hh24:mi:ss') ");
                if (jieshuRq != string.Empty)
                    sbSql.Append("and a.jifeirq<=to_date('" + jieshuRq + " 23:59:59','yyyy-mm-dd hh24:mi:ss') ");
                //if (jiaoyiMm != string.Empty)
                //    sbSql.Append("and substr(c.shenfenzh,length(c.shenfenzh)-5,6)='" + jiaoyiMm + "' ");
                if (!string.IsNullOrEmpty(tongJiFL))
                {
                    sbSql.Append(" and a.hesuanxm = '" + tongJiFL + "'  ");
                }

                sbSql.Append(" order by jifeirq desc ");

                DataTable dtFYXX = DBVisitor.ExecuteTable(sbSql.ToString());
                if (dtFYXX.Rows.Count > 0)
                {
                    OutObject.FEIYONGMXTS = dtFYXX.Rows.Count.ToString();//费用明细条数

                    for (int i = 0; i < dtFYXX.Rows.Count; i++)
                    {

                        FEIYONGXX_ZY zyFyxx = new FEIYONGXX_ZY();
                        zyFyxx.JIFEIRQ = dtFYXX.Rows[i]["JIFEIRQ"].ToString();//计费日期
                        zyFyxx.FEIYONGLX = dtFYXX.Rows[i]["FEIYONGLX"].ToString();//费用类型
                        zyFyxx.XIANGMUXH = dtFYXX.Rows[i]["XIANGMUXH"].ToString();//项目序号
                        zyFyxx.XIANGMUCDDM = dtFYXX.Rows[i]["XIANGMUCDDM"].ToString();//项目产品代码
                        zyFyxx.XIANGMUMC = dtFYXX.Rows[i]["XIANGMUMC"].ToString();//项目名称
                        zyFyxx.XIANGMUGL = dtFYXX.Rows[i]["XIANGMUGL"].ToString();//项目归类
                        zyFyxx.XIANGMUGLMC = dtFYXX.Rows[i]["XIANGMUGLMC"].ToString();//项目归类名称
                        zyFyxx.XIANGMUGG = dtFYXX.Rows[i]["XIANGMUGG"].ToString();//项目规格
                        zyFyxx.XIANGMUJX = dtFYXX.Rows[i]["XIANGMUJX"].ToString();//项目剂型
                        zyFyxx.XIANGMUDW = dtFYXX.Rows[i]["XIANGMUDW"].ToString();//项目单位
                        zyFyxx.XIANGMUCDMC = dtFYXX.Rows[i]["XIANGMUCDMC"].ToString();//项目产地名称
                        zyFyxx.DANJIA = dtFYXX.Rows[i]["DANJIA"].ToString();//单价
                        zyFyxx.SHULIANG = dtFYXX.Rows[i]["SHULIANG"].ToString();//数量
                        zyFyxx.JINE = dtFYXX.Rows[i]["JINE"].ToString();//金额
                        zyFyxx.YIBAODJ = dtFYXX.Rows[i]["YIBAODJ"].ToString();//医保等级
                        zyFyxx.YIBAOZFBL = dtFYXX.Rows[i]["YIBAOZFBL"].ToString();//医保自负比例
                        zyFyxx.XIANGMUXJ = dtFYXX.Rows[i]["XIANGMUXJ"].ToString();//项目限价
                        zyFyxx.ZIFEIJE = dtFYXX.Rows[i]["ZIFEIJE"].ToString();//自费金额
                        zyFyxx.ZILIJE = dtFYXX.Rows[i]["ZILIJE"].ToString();//自理金额
                        zyFyxx.KAIDANKSDM = dtFYXX.Rows[i]["KAIDANKSDM"].ToString();//开单科室代码
                        zyFyxx.KAIDANKSMC = dtFYXX.Rows[i]["KAIDANKSMC"].ToString();//开单科室名称
                        zyFyxx.KAIDANYSDM = dtFYXX.Rows[i]["KAIDANYSDM"].ToString();//开单医生代码
                        zyFyxx.KAIDANYSXM = dtFYXX.Rows[i]["KAIDANYSXM"].ToString();//开单医生姓名

                        OutObject.FEIYONGMX_ZY.Add(zyFyxx);
                    }

                }
            }
        }
    }
}
