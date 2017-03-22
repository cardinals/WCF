using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using HIS4.Schemas;
using System.Configuration;

namespace HIS4.Biz
{
    public class MENZHENJSMX : IMessage<MENZHENJSMX_IN, MENZHENJSMX_OUT>
    {

        public override void ProcessMessage()
        {
            this.OutObject = new MENZHENJSMX_OUT();

            string bingRenID = InObject.BINGRENID;//病人ID
            string shouFeiID = InObject.SHOUFEIID;//收费ID
            string kaiShiRQ = InObject.KAISHIRQ;//开始日期
            string jieShuRQ = InObject.JIESHURQ;//结束日期
            string yuanQuID = InObject.BASEINFO.FENYUANDM;//分院代码
            int menZhenSJFW = -1 * Convert.ToInt32(ConfigurationManager.AppSettings["FeiYongSJ"]);//门诊费用检索默认时间范围

            #region 基础入参判断
            if (string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("病人信息获取失败!");
            }

            if (string.IsNullOrEmpty(shouFeiID))
            {
                if (string.IsNullOrEmpty(jieShuRQ))
                {
                    jieShuRQ = DateTime.Now.ToString("yyyy-MM-dd");
                }

                if (string.IsNullOrEmpty(kaiShiRQ))
                {
                    kaiShiRQ = DateTime.Now.AddDays(menZhenSJFW).Date.ToString("yyyy-MM-dd");
                }

                string shouFeiIDSQL = "select shoufeiid,shoufeiren from mz_shoufei1 where bingrenid ='{0}' and shoufeirq between to_date('{1} 00:00:00', 'yyyy-mm-dd hh24:mi:ss') and  to_date('{2} 23:59:59', 'yyyy-mm-dd hh24:mi:ss') order by shoufeirq desc ";
                DataTable dtShouFeiID = DBVisitor.ExecuteTable(string.Format(shouFeiIDSQL, bingRenID, kaiShiRQ, jieShuRQ));
                if (dtShouFeiID.Rows.Count > 0)
                {
                    for (int i = 0; i < dtShouFeiID.Rows.Count; i++)
                    {
                        shouFeiID += "','" + dtShouFeiID.Rows[i]["shoufeiid"].ToString();
                    }
                    shouFeiID = shouFeiID.Trim('\'').ToString();
                    shouFeiID = shouFeiID.Trim(',').ToString();
                    shouFeiID = shouFeiID.Trim('\'').ToString();
                }
                else
                {
                    OutObject.FEIYONGMXTS = "0".ToString();
                }
            }
            #endregion

            //查询发票号码
            DataTable dt = DBVisitor.ExecuteTable(string.Format("select * from mz_fapiao1 where zuofeibz='0' and shoufeiid in ('{0}')", shouFeiID));

            #region 获取费用明细

            #region 获取处方数据
            StringBuilder sqlBufCF = new StringBuilder();
            sqlBufCF.Append("select a.shoufeiid shoufeiid, RPAD(nvl(a.kongzhisx,'0000'),4,'0') as kongzhisx, a.chufangid chufangxh,d.chufangmxid mingxixh,a.feiyonglb feiyonglx, l.yaopinid xiangmuxh,l.chandi xiangmucddm, ")
                .Append("l.yaopinmc xiangmumc,l.yaopinlx xiangmugl,m.DAIMAMC XIANGMUGLMC,l.yaopingg xiangmugg,l.jixing xiangmujx,l.jiliangdw xiangmudw, ")
                .Append("l.chandimc xiangmucdmc,l.baozhuangliang baozhuangsl,l.baozhuangdw,l.zuixiaodw zuixiaojldw,'' danciyl,'' yongliangdw, ")
                .Append("null meitiancs,d.yongyaots,l.fufangbz danfufbz,d.chufangts2 zhongcaoyts, d.jiesuanjia danjia, ")
                .Append("decode(a.chufanglx,'3',nvl(d.shuliang,0)*nvl(a.chufangts,0),d.shuliang) shuliang,'' SHENGPIBH , ")
                .Append("decode(a.chufanglx,'3',nvl(d.jiesuanje,0)*nvl(a.chufangts,0),d.jiesuanje) jine, ")
                .Append("d.yibaodj,d.yibaodm,d.yibaozfbl,d.xianjia xiangmuxj,d.zifeije,d.zilije,d.shenpibh,d.zifeibz,0 teshuyybz,d.yibaoxx yibaoxmfzxx,d.yicijl dancisl, ")
                .Append("d.pinci pinlvsz,a.kaidanks kaidanksdm,a.kaidanksmc kaidanksmc,a.kaidanys kaidanysdm,a.kaidanysxm kaidanysxm,d.zifubl,to_char(a.kaidanrq,'yyyy-mm-dd')kaidanrq, ")
                .Append("l.daguigid yaopindggxh,'' yaopindggcd,null yaopindggsl, ")
                .Append("(select  to_char(shoufeirq,'yyyy-mm-dd hh24:mi:ss') from mz_shoufei1 where shoufeiid = a.shoufeiid) as shoufeirq ")
                .Append("from mz_chufang1 a,gy_feiyonglb b,gy_feiyongxz c,mz_chufang2 d,gy_yaopincdjg2 l,gy_xiangmulx m ")
                .Append("where b.leibieid(+)=a.feiyonglb and c.xingzhiid(+)=a.feiyongxz and d.chufangid=a.chufangid and a.bingrenid = '{0}' ")
                .Append("and l.jiageid = d.jiageid and m.DAIMAID(+)=l.yaopinlx and a.shoufeibz=1 and a.jiaoyilx=1 and a.chongxiaobz = 0  ")
                .Append("and a.shoufeiid in ('{1}') ");
            DataTable dtChuFangMX = DBVisitor.ExecuteTable(string.Format(sqlBufCF.ToString(), bingRenID, shouFeiID));

            if (dtChuFangMX.Rows.Count > 0)
            {
                #region 费用明细拼装
                for (int i = 0; i < dtChuFangMX.Rows.Count; i++)
                {
                    var temp = new MENZHENFYXX();
                    temp.CHUFANGLX = "1"; //处方类型
                    temp.CHUFANGXH = dtChuFangMX.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                    temp.MINGXIXH = dtChuFangMX.Rows[i]["MINGXIXH"].ToString(); //明细序号
                    temp.FEIYONGLX = dtChuFangMX.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                    temp.XIANGMUXH = dtChuFangMX.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                    temp.XIANGMUCDDM = dtChuFangMX.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                    temp.XIANGMUMC = dtChuFangMX.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                    temp.XIANGMUGL = dtChuFangMX.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                    temp.XIANGMUGLMC = dtChuFangMX.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                    temp.XIANGMUGG = dtChuFangMX.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                    temp.XIANGMUJX = dtChuFangMX.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                    temp.XIANGMUDW = dtChuFangMX.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                    temp.XIANGMUCDMC = dtChuFangMX.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                    temp.BAOZHUANGSL = dtChuFangMX.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                    temp.BAOZHUANGDW = dtChuFangMX.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                    temp.ZUIXIAOJLDW = dtChuFangMX.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                    temp.DANCIYL = dtChuFangMX.Rows[i]["DANCIYL"].ToString(); //单次用量
                    temp.YONGLIANGDW = dtChuFangMX.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                    temp.MEITIANCS = dtChuFangMX.Rows[i]["MEITIANCS"].ToString(); //每天次数
                    temp.YONGYAOTS = dtChuFangMX.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                    temp.DANFUFBZ = dtChuFangMX.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                    temp.ZHONGCAOYTS = dtChuFangMX.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                    temp.DANJIA = dtChuFangMX.Rows[i]["DANJIA"].ToString(); //单价
                    temp.SHULIANG = dtChuFangMX.Rows[i]["SHULIANG"].ToString(); //数量
                    temp.JINE = dtChuFangMX.Rows[i]["JINE"].ToString(); //金额
                    temp.YIBAODJ = dtChuFangMX.Rows[i]["YIBAODJ"].ToString(); //医保等级
                    temp.YIBAODM = dtChuFangMX.Rows[i]["YIBAODM"].ToString(); //医保代码
                    temp.YIBAOZFBL = dtChuFangMX.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                    temp.XIANGMUXJ = dtChuFangMX.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                    temp.ZIFEIJE = dtChuFangMX.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                    temp.ZILIJE = dtChuFangMX.Rows[i]["ZILIJE"].ToString(); //自理金额
                    temp.SHENGPIBH = dtChuFangMX.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                    temp.ZIFEIBZ = dtChuFangMX.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                    temp.TESHUYYBZ = dtChuFangMX.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                    temp.YIBAOXMFZXX = dtChuFangMX.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                    temp.DANCISL = dtChuFangMX.Rows[i]["DANCISL"].ToString(); //单次数量
                    temp.PINLVSZ = dtChuFangMX.Rows[i]["PINLVSZ"].ToString(); //频率数值
                    temp.KAIDANKSDM = dtChuFangMX.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                    temp.KAIDANKSMC = dtChuFangMX.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                    temp.KAIDANYSDM = dtChuFangMX.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                    temp.KAIDANYSXM = dtChuFangMX.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                    temp.ZIFUBL = dtChuFangMX.Rows[i]["ZIFUBL"].ToString(); //自负比例
                    temp.KAIDANRQ = dtChuFangMX.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                    temp.YAOPINDGGXH = dtChuFangMX.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                    temp.YAOPINDGGCD = dtChuFangMX.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                    temp.YAOPINDGGSL = dtChuFangMX.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                    temp.SHOUFEIRQ = dtChuFangMX.Rows[i]["SHOUFEIRQ"].ToString();
                    temp.KONGZHISX = dtChuFangMX.Rows[i]["kongzhisx"].ToString();
                    switch (temp.KONGZHISX)
                    {
                        case "0100":
                            temp.KONGZHISX = "01";
                            temp.KONGZHISXMC = "特病";
                            break;
                        case "0010":
                            temp.KONGZHISX = "03";
                            temp.KONGZHISXMC = "生育";
                            break;
                        default:
                            temp.KONGZHISX = "00";
                            temp.KONGZHISXMC = "普通";
                            break;
                    }
                    temp.SHOUFEIID = dtChuFangMX.Rows[i]["shoufeiid"].ToString();
                    //发票号码/发票ID
                    DataRow[] dr = dt.Select("shoufeiid ='" + dtChuFangMX.Rows[i]["shoufeiid"].ToString() + "'");
                    if (dr.Length > 0)
                    {
                        temp.FAPIAOHM = dr[0]["fapiaohm"].ToString();
                        temp.FAPIAOID = dr[0]["fapiaoid"].ToString();
                    }
                    else
                    {
                        temp.FAPIAOHM = "";
                        temp.FAPIAOID = "";
                    }

                    string shouFeiR = "select  shoufeiren,jiesuanid,(feiyonghj -zifuje  - zifeije -  zilije) ybje from mz_shoufei1 where shoufeiid='" + dtChuFangMX.Rows[i]["shoufeiid"].ToString() + "'  ";
                    DataTable dtshouFeiR = DBVisitor.ExecuteTable(string.Format(shouFeiR, bingRenID, kaiShiRQ, jieShuRQ));
                    ///操作工号
                    temp.CAOZUOGH = dtshouFeiR.Rows[0]["shoufeiren"].ToString();
                    temp.YBJE = dtshouFeiR.Rows[0]["ybje"].ToString();
                    ///支付方式
                    DataTable dtZFFS = DBVisitor.ExecuteTable(string.Format("select * from mz_zhifu where jiesuanid='{0}'", dtshouFeiR.Rows[0]["jiesuanid"].ToString()));
                    if (dtZFFS.Rows.Count > 0)
                    {
                        temp.ZHIFUFS = dtZFFS.Rows[0]["zhifufs"].ToString();
                    }
                    else
                    {
                        temp.ZHIFUFS = "";
                    }

                    if (dr.Length > 0)
                    {
                        temp.FAPIAOHM = dr[0]["fapiaohm"].ToString();
                        temp.FAPIAOID = dr[0]["fapiaoid"].ToString();
                    }
                    else
                    {
                        temp.FAPIAOHM = "";
                        temp.FAPIAOID = "";
                    }


                    OutObject.FEIYONGMX.Add(temp);
                }
                #endregion
            }
            #endregion

            #region 获取医技数据
            StringBuilder sqlBufYJ = new StringBuilder();
            sqlBufYJ.Append("select  a.shoufeiid shoufeiid, 0 chufanglx,nvl(RPAD(nvl(a.kongzhisx,(select RPAD(nvl(cf.kongzhisx, '0000'), 4, '0') From mz_chufang1 cf where cf.chufangid = a.guanlianid)),4,'0'),'0000') as kongzhisx, a.yijiid chufangxh,d.yijimxid mingxixh,a.feiyonglb feiyonglx, l.shoufeixmid xiangmuxh,'' xiangmucddm, ")
                .Append("l.shoufeixmmc xiangmumc,'' xiangmugl,'' XIANGMUGLMC,'' xiangmugg,null xiangmujx,l.jijiadw xiangmudw, ")
                .Append("'' xiangmucdmc,null baozhuangsl,'' baozhuangdw,'' zuixiaojldw,'' danciyl,'' yongliangdw, '' SHENGPIBH , ")
                .Append("null meitiancs,null yongyaots,null danfufbz,null zhongcaoyts, d.jiesuanjia danjia,d.shuliang,d.jiesuanje jine,d.yibaodj, ")
                .Append("d.yibaodm,d.yibaozfbl,d.xianjia xiangmuxj,d.zifeije,d.zilije,d.shenpibh,d.zifeibz,0 teshuyybz,d.yibaoxx yibaoxmfzxx,d.shuliang dancisl, ")
                .Append("null pinlvsz,a.kaidanks kaidanksdm,a.kaidanksmc kaidanksmc,a.kaidanys kaidanysdm,a.kaidanysxm kaidanysxm,d.zifubl, ")
                .Append("to_char(a.kaidanrq,'yyyy-mm-dd')kaidanrq,'' yaopindggxh,'' yaopindggcd,null yaopindggsl, ")
                .Append("(select  to_char(shoufeirq,'yyyy-mm-dd hh24:mi:ss') from mz_shoufei1 where shoufeiid = a.shoufeiid) as shoufeirq ")
                .Append("from mz_yiji1 a,gy_feiyonglb b,gy_feiyongxz c,mz_yiji2 d,gy_shoufeixm l ")
                .Append("where b.leibieid(+)=a.feiyonglb and c.xingzhiid(+)=a.feiyongxz  ")
                .Append("and d.yijiid=a.yijiid and l.shoufeixmid = d.shoufeixm and a.bingrenid = '{0}' ")
                .Append("and a.shoufeibz=1 and a.jiaoyilx=1 and a.chongxiaobz = 0  ")
                .Append("and a.shoufeiid in ('{1}') ");
            DataTable dtYiJiMX = DBVisitor.ExecuteTable(string.Format(sqlBufYJ.ToString(), bingRenID, shouFeiID));

            if (dtYiJiMX.Rows.Count > 0)
            {
                #region 费用明细拼装
                for (int i = 0; i < dtYiJiMX.Rows.Count; i++)
                {
                    var temp = new MENZHENFYXX();
                    temp.CHUFANGLX = "0"; //处方类型
                    temp.CHUFANGXH = dtYiJiMX.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                    temp.MINGXIXH = dtYiJiMX.Rows[i]["MINGXIXH"].ToString(); //明细序号
                    temp.FEIYONGLX = dtYiJiMX.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                    temp.XIANGMUXH = dtYiJiMX.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                    temp.XIANGMUCDDM = dtYiJiMX.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                    temp.XIANGMUMC = dtYiJiMX.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                    temp.XIANGMUGL = dtYiJiMX.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                    temp.XIANGMUGLMC = dtYiJiMX.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                    temp.XIANGMUGG = dtYiJiMX.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                    temp.XIANGMUJX = dtYiJiMX.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                    temp.XIANGMUDW = dtYiJiMX.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                    temp.XIANGMUCDMC = dtYiJiMX.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                    temp.BAOZHUANGSL = dtYiJiMX.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                    temp.BAOZHUANGDW = dtYiJiMX.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                    temp.ZUIXIAOJLDW = dtYiJiMX.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                    temp.DANCIYL = dtYiJiMX.Rows[i]["DANCIYL"].ToString(); //单次用量
                    temp.YONGLIANGDW = dtYiJiMX.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                    temp.MEITIANCS = dtYiJiMX.Rows[i]["MEITIANCS"].ToString(); //每天次数
                    temp.YONGYAOTS = dtYiJiMX.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                    temp.DANFUFBZ = dtYiJiMX.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                    temp.ZHONGCAOYTS = dtYiJiMX.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                    temp.DANJIA = dtYiJiMX.Rows[i]["DANJIA"].ToString(); //单价
                    temp.SHULIANG = dtYiJiMX.Rows[i]["SHULIANG"].ToString(); //数量
                    temp.JINE = dtYiJiMX.Rows[i]["JINE"].ToString(); //金额
                    temp.YIBAODJ = dtYiJiMX.Rows[i]["YIBAODJ"].ToString(); //医保等级
                    temp.YIBAODM = dtYiJiMX.Rows[i]["YIBAODM"].ToString(); //医保代码
                    temp.YIBAOZFBL = dtYiJiMX.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                    temp.XIANGMUXJ = dtYiJiMX.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                    temp.ZIFEIJE = dtYiJiMX.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                    temp.ZILIJE = dtYiJiMX.Rows[i]["ZILIJE"].ToString(); //自理金额
                    temp.SHENGPIBH = dtYiJiMX.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                    temp.ZIFEIBZ = dtYiJiMX.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                    temp.TESHUYYBZ = dtYiJiMX.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                    temp.YIBAOXMFZXX = dtYiJiMX.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                    temp.DANCISL = dtYiJiMX.Rows[i]["DANCISL"].ToString(); //单次数量
                    temp.PINLVSZ = dtYiJiMX.Rows[i]["PINLVSZ"].ToString(); //频率数值
                    temp.KAIDANKSDM = dtYiJiMX.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                    temp.KAIDANKSMC = dtYiJiMX.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                    temp.KAIDANYSDM = dtYiJiMX.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                    temp.KAIDANYSXM = dtYiJiMX.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                    temp.ZIFUBL = dtYiJiMX.Rows[i]["ZIFUBL"].ToString(); //自负比例
                    temp.KAIDANRQ = dtYiJiMX.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                    temp.YAOPINDGGXH = dtYiJiMX.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                    temp.YAOPINDGGCD = dtYiJiMX.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                    temp.YAOPINDGGSL = dtYiJiMX.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                    temp.SHOUFEIRQ = dtYiJiMX.Rows[i]["SHOUFEIRQ"].ToString();
                    temp.KONGZHISX = dtYiJiMX.Rows[i]["kongzhisx"].ToString();
                    switch (temp.KONGZHISX)
                    {
                        case "0100":
                            temp.KONGZHISX = "01";
                            temp.KONGZHISXMC = "特病";
                            break;
                        case "0010":
                            temp.KONGZHISX = "03";
                            temp.KONGZHISXMC = "生育";
                            break;
                        default:
                            temp.KONGZHISX = "00";
                            temp.KONGZHISXMC = "普通";
                            break;
                    }

                    temp.SHOUFEIID = dtYiJiMX.Rows[i]["shoufeiid"].ToString();
                    //发票号码/发票ID
                    DataRow[] dr = dt.Select("shoufeiid ='" + dtYiJiMX.Rows[i]["shoufeiid"].ToString() + "'");
                    if (dr.Length > 0)
                    {
                        temp.FAPIAOHM = dr[0]["fapiaohm"].ToString();
                        temp.FAPIAOID = dr[0]["fapiaoid"].ToString();
                    }
                    else
                    {
                        temp.FAPIAOHM = "";
                        temp.FAPIAOID = "";
                    }

                    string shouFeiR = "select  shoufeiren,jiesuanid ,(feiyonghj -zifuje  - zifeije -  zilije) ybje from mz_shoufei1 where shoufeiid='" + dtYiJiMX.Rows[i]["shoufeiid"].ToString() + "'  ";
                    DataTable dtshouFeiR = DBVisitor.ExecuteTable(string.Format(shouFeiR, bingRenID, kaiShiRQ, jieShuRQ));
                    ///操作工号
                    temp.CAOZUOGH = dtshouFeiR.Rows[0]["shoufeiren"].ToString();
                    temp.YBJE = dtshouFeiR.Rows[0]["ybje"].ToString();
                    ///支付方式
                    DataTable dtZFFS = DBVisitor.ExecuteTable(string.Format("select * from mz_zhifu where jiesuanid='{0}'", dtshouFeiR.Rows[0]["jiesuanid"].ToString()));
                    if (dtZFFS.Rows.Count > 0)
                    {
                        temp.ZHIFUFS = dtZFFS.Rows[0]["zhifufs"].ToString();
                    }
                    else
                    {
                        temp.ZHIFUFS = "";
                    }


                    OutObject.FEIYONGMX.Add(temp);
                }
                #endregion
            }
            #endregion

            #region 获取挂号数据
            StringBuilder sqlBufGH = new StringBuilder();
            sqlBufGH.Append("select a.shoufeiid shoufeiid, 2 chufanglx,a.guahaoid chufangxh,d.guahaomxid mingxixh,a.feiyonglb feiyonglx,l.shoufeixmid xiangmuxh,")
                    .Append("'' xiangmucddm,l.shoufeixmmc xiangmumc,'' xiangmugl,'' XIANGMUGLMC,'' xiangmugg,null xiangmujx, ")
                    .Append("l.jijiadw xiangmudw,'' xiangmucdmc,null baozhuangsl,'' baozhuangdw,'' zuixiaojldw,'' danciyl, ")
                    .Append("'' yongliangdw,null meitiancs,null yongyaots,null danfufbz,null zhongcaoyts,d.jiesuanjia danjia, ")
                    .Append("d.shuliang,d.jiesuanje jine,d.yibaodj,d.yibaodm,d.yibaozfbl,d.xianjia xiangmuxj,d.zifeije,d.zilije, ")
                    .Append("d.zifeibz,0 teshuyybz,'' yibaoxmfzxx,d.shuliang dancisl,'' pinlvsz,'' kaidanksdm,'' kaidanksmc, ")
                    .Append("a.guahaoren kaidanysdm,a.guahaorxm kaidanysxm,d.zifubl,to_char(a.guahaorq, 'yyyy-mm-dd') kaidanrq, ")
                    .Append("'' yaopindggxh,'' yaopindggcd,'' yaopindggsl,'' SHENGPIBH, ")
                    .Append("(select  to_char(shoufeirq,'yyyy-mm-dd hh24:mi:ss') from mz_shoufei1 where shoufeiid = a.shoufeiid) as shoufeirq ")
                    .Append("from mz_guahao1 a,gy_feiyonglb b,gy_feiyongxz c,mz_guahao2 d,gy_shoufeixm l ")
                    .Append(" where b.leibieid(+) = a.feiyonglb and c.xingzhiid(+) = a.feiyongxz and d.guahaoid = a.guahaoid ")
                    .Append(" and l.shoufeixmid = d.shoufeixm and a.jiaoyilx = 1 and a.zuofeibz = 0 ")
                    .Append("   and a.bingrenid = '{0}' and a.shoufeiid in ('{1}') ");
            DataTable dtGuaHaoMX = DBVisitor.ExecuteTable(string.Format(sqlBufGH.ToString(), bingRenID, shouFeiID));

            if (dtGuaHaoMX.Rows.Count > 0)
            {
                #region 费用明细拼装
                for (int i = 0; i < dtGuaHaoMX.Rows.Count; i++)
                {
                    var temp = new MENZHENFYXX();
                    temp.CHUFANGLX = "2"; //处方类型
                    temp.CHUFANGXH = dtGuaHaoMX.Rows[i]["CHUFANGXH"].ToString(); //处方序号
                    temp.MINGXIXH = dtGuaHaoMX.Rows[i]["MINGXIXH"].ToString(); //明细序号
                    temp.FEIYONGLX = dtGuaHaoMX.Rows[i]["FEIYONGLX"].ToString(); //费用类型
                    temp.XIANGMUXH = dtGuaHaoMX.Rows[i]["XIANGMUXH"].ToString(); //项目序号
                    temp.XIANGMUCDDM = dtGuaHaoMX.Rows[i]["XIANGMUCDDM"].ToString(); //项目产品代码
                    temp.XIANGMUMC = dtGuaHaoMX.Rows[i]["XIANGMUMC"].ToString(); //项目名称
                    temp.XIANGMUGL = dtGuaHaoMX.Rows[i]["XIANGMUGL"].ToString(); //项目归类
                    temp.XIANGMUGLMC = dtGuaHaoMX.Rows[i]["XIANGMUGLMC"].ToString(); //项目归类名称
                    temp.XIANGMUGG = dtGuaHaoMX.Rows[i]["XIANGMUGG"].ToString(); //项目规格
                    temp.XIANGMUJX = dtGuaHaoMX.Rows[i]["XIANGMUJX"].ToString(); //项目剂型
                    temp.XIANGMUDW = dtGuaHaoMX.Rows[i]["XIANGMUDW"].ToString(); //项目单位
                    temp.XIANGMUCDMC = dtGuaHaoMX.Rows[i]["XIANGMUCDMC"].ToString(); //项目产地名称
                    temp.BAOZHUANGSL = dtGuaHaoMX.Rows[i]["BAOZHUANGSL"].ToString(); //包装数量
                    temp.BAOZHUANGDW = dtGuaHaoMX.Rows[i]["BAOZHUANGDW"].ToString(); //包装单位
                    temp.ZUIXIAOJLDW = dtGuaHaoMX.Rows[i]["ZUIXIAOJLDW"].ToString(); //最小剂量单位
                    temp.DANCIYL = dtGuaHaoMX.Rows[i]["DANCIYL"].ToString(); //单次用量
                    temp.YONGLIANGDW = dtGuaHaoMX.Rows[i]["YONGLIANGDW"].ToString(); //用量单位
                    temp.MEITIANCS = dtGuaHaoMX.Rows[i]["MEITIANCS"].ToString(); //每天次数
                    temp.YONGYAOTS = dtGuaHaoMX.Rows[i]["YONGYAOTS"].ToString(); //用药天数
                    temp.DANFUFBZ = dtGuaHaoMX.Rows[i]["DANFUFBZ"].ToString(); //单复方标志
                    temp.ZHONGCAOYTS = dtGuaHaoMX.Rows[i]["ZHONGCAOYTS"].ToString(); //中草药贴数
                    temp.DANJIA = dtGuaHaoMX.Rows[i]["DANJIA"].ToString(); //单价
                    temp.SHULIANG = dtGuaHaoMX.Rows[i]["SHULIANG"].ToString(); //数量
                    temp.JINE = dtGuaHaoMX.Rows[i]["JINE"].ToString(); //金额
                    temp.YIBAODJ = dtGuaHaoMX.Rows[i]["YIBAODJ"].ToString(); //医保等级
                    temp.YIBAODM = dtGuaHaoMX.Rows[i]["YIBAODM"].ToString(); //医保代码
                    temp.YIBAOZFBL = dtGuaHaoMX.Rows[i]["YIBAOZFBL"].ToString(); //医保自负比例
                    temp.XIANGMUXJ = dtGuaHaoMX.Rows[i]["XIANGMUXJ"].ToString(); //项目限价
                    temp.ZIFEIJE = dtGuaHaoMX.Rows[i]["ZIFEIJE"].ToString(); //自费金额
                    temp.ZILIJE = dtGuaHaoMX.Rows[i]["ZILIJE"].ToString(); //自理金额
                    temp.SHENGPIBH = dtGuaHaoMX.Rows[i]["SHENGPIBH"].ToString(); //审批编号
                    temp.ZIFEIBZ = dtGuaHaoMX.Rows[i]["ZIFEIBZ"].ToString(); //自费标志
                    temp.TESHUYYBZ = dtGuaHaoMX.Rows[i]["TESHUYYBZ"].ToString(); //特殊用药标志
                    temp.YIBAOXMFZXX = dtGuaHaoMX.Rows[i]["YIBAOXMFZXX"].ToString(); //医保项目辅助信息
                    temp.DANCISL = dtGuaHaoMX.Rows[i]["DANCISL"].ToString(); //单次数量
                    temp.PINLVSZ = dtGuaHaoMX.Rows[i]["PINLVSZ"].ToString(); //频率数值
                    temp.KAIDANKSDM = dtGuaHaoMX.Rows[i]["KAIDANKSDM"].ToString(); //开单科室代码
                    temp.KAIDANKSMC = dtGuaHaoMX.Rows[i]["KAIDANKSMC"].ToString(); //开单科室名称
                    temp.KAIDANYSDM = dtGuaHaoMX.Rows[i]["KAIDANYSDM"].ToString(); //开单医生代码
                    temp.KAIDANYSXM = dtGuaHaoMX.Rows[i]["KAIDANYSXM"].ToString(); //开单医生姓名
                    temp.ZIFUBL = dtGuaHaoMX.Rows[i]["ZIFUBL"].ToString(); //自负比例
                    temp.KAIDANRQ = dtGuaHaoMX.Rows[i]["KAIDANRQ"].ToString(); //开单日期
                    temp.YAOPINDGGXH = dtGuaHaoMX.Rows[i]["YAOPINDGGXH"].ToString(); //药品大规格序号
                    temp.YAOPINDGGCD = dtGuaHaoMX.Rows[i]["YAOPINDGGCD"].ToString(); //药品大规格产地
                    temp.YAOPINDGGSL = dtGuaHaoMX.Rows[i]["YAOPINDGGSL"].ToString(); //药品大规格数量
                    temp.SHOUFEIRQ = dtGuaHaoMX.Rows[i]["SHOUFEIRQ"].ToString();
                    temp.KONGZHISX = "00";
                    temp.KONGZHISXMC = "普通";

                    temp.SHOUFEIID = dtGuaHaoMX.Rows[i]["shoufeiid"].ToString();
                    //发票号码/发票ID
                    DataRow[] dr = dt.Select("shoufeiid ='" + dtGuaHaoMX.Rows[i]["shoufeiid"].ToString() + "'");
                    if (dr.Length > 0)
                    {
                        temp.FAPIAOHM = dr[0]["fapiaohm"].ToString();
                        temp.FAPIAOID = dr[0]["fapiaoid"].ToString();
                    }
                    else
                    {
                        temp.FAPIAOHM = "";
                        temp.FAPIAOID = "";
                    }

                    string shouFeiR = "select  shoufeiren,jiesuanid ,(feiyonghj -zifuje  - zifeije -  zilije) ybje from mz_shoufei1 where shoufeiid='" + dtGuaHaoMX.Rows[i]["shoufeiid"].ToString() + "'  ";
                    DataTable dtshouFeiR = DBVisitor.ExecuteTable(string.Format(shouFeiR, bingRenID, kaiShiRQ, jieShuRQ));
                    ///操作工号
                    temp.CAOZUOGH = dtshouFeiR.Rows[0]["shoufeiren"].ToString();
                    temp.YBJE = dtshouFeiR.Rows[0]["ybje"].ToString();
                    ///支付方式
                    DataTable dtZFFS = DBVisitor.ExecuteTable(string.Format("select * from mz_zhifu where jiesuanid='{0}'", dtshouFeiR.Rows[0]["jiesuanid"].ToString()));
                    if (dtZFFS.Rows.Count > 0)
                    {
                        temp.ZHIFUFS = dtZFFS.Rows[0]["zhifufs"].ToString();
                    }
                    else
                    {
                        temp.ZHIFUFS = "";
                    }

                    OutObject.FEIYONGMX.Add(temp);
                }
                #endregion
            }
            #endregion
            #endregion
        }
    }
}
