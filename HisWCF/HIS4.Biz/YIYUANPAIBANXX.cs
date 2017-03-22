using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using System.Configuration;
using System.Data;
using SWSoft.Framework;

namespace HIS4.Biz
{
    public class YIYUANPAIBANXX : IMessage<YIYUANPAIBANXX_IN, YIYUANPAIBANXX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new YIYUANPAIBANXX_OUT();
            string guaHaoFS = InObject.GUAHAOFS;//1 挂号 2 预约
            string riQi = InObject.RIQI; //日期
            string guaHaoBC = InObject.GUAHAOBC;//挂号班次 0 全天 1上午 2下午
            string keShiDM = InObject.KESHIDM;//科室代码
            string yiShengDM = InObject.YISHENGDM;//医生代码
            string guaHaoLB = InObject.GUAHAOLB;//挂号类别
            string yuYueLX = InObject.YUYUELX;//预约类型

            #region 基本入参判断
            if (string.IsNullOrEmpty(guaHaoFS))
            {
                throw new Exception("挂号方式不能为空！");
            }
            else if(guaHaoFS != "1" && guaHaoFS != "2") {
                throw new Exception("请传入正确的挂号方式，1 当天挂号 2 挂号预约！");
            }

            if (string.IsNullOrEmpty(guaHaoBC)) {
                throw new Exception("挂号班次不能为空！");
            }else if(guaHaoBC != "0" && guaHaoBC != "1" && guaHaoBC != "2"){
                throw new Exception("请传入正确的挂号班次，0 全天 1 上午 2 下午");
            }

            if (guaHaoFS == "2" && string.IsNullOrEmpty(yuYueLX)) {
                yuYueLX = ConfigurationManager.AppSettings["GuaHaoYYLX"];
            }

            if (guaHaoFS == "1" && string.IsNullOrEmpty(riQi)) {
                riQi = DateTime.Now.ToString("yyyy-MM-dd");
            }
            #endregion
            StringBuilder sqlPaiBanXX = new StringBuilder();
            if (guaHaoFS == "1")
            {
                #region 当天挂号
                if (guaHaoBC == "0") {
                    sqlPaiBanXX.Append(" select * from ( ");
                }
                if (guaHaoBC == "1" || guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" select A.PAIBANID,to_Char(A.RIQI,'yyyy-mm-dd') as PAIBANRQ,a.GUAHAOLB,1 as GUAHAOBC,A.KESHIID,C.keshimc,NVL(A.WEIZHI,C.weizhism) as JIUZHENDD, ");
                    sqlPaiBanXX.Append(" c.keshizl as KESHIJS,nvl(a.YISHENGID,'*') as YISHENGDM,nvl(d.zhigongxm,'普通') as YISHENGXM , e.daimamc as YISHENGZC,a.ZHENLIAOFXM,a.GUAHAOFXM, ");
                    sqlPaiBanXX.Append(" NVL(a.SHANGWUXH,0) AS HAOYUANZS,NVL(a.SHANGWUXH,0)-NVL(a.SHANGWUYGH,0) AS SHENGYUHY ,'' as YUYUELX ");
                    sqlPaiBanXX.Append(" from mz_v_guahaopb_ex_zzj a ,v_gy_keshi c,v_gy_zhigongxx d , ");
                    sqlPaiBanXX.Append(" (select * from gy_daima where daimalb='0072') e  ");
                    sqlPaiBanXX.Append(" where a.keshiid=c.keshiid and d.zhigongid(+) = a.YISHENGID and e.daimaid(+) = d.zhicheng  ");
                    
                    sqlPaiBanXX.Append(" and to_char(a.riqi,'yyyy-mm-dd')='"+riQi+"' ");
                    sqlPaiBanXX.Append("  and a.shangwuxh > 0 ");
                    if (!string.IsNullOrEmpty(keShiDM)) {
                        sqlPaiBanXX.Append(" and nvl(a.keshiid,'*') = '" + keShiDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(yiShengDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.yishengid,'*') = '" + yiShengDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(guaHaoLB) && guaHaoLB != "0")
                    {
                        sqlPaiBanXX.Append(" and a.guahaolb = " + guaHaoLB + " ");
                    }
                }
                if (guaHaoBC == "0") {
                    sqlPaiBanXX.Append(" union all ");
                }
                if (guaHaoBC == "2" || guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" select A.PAIBANID,to_Char(A.RIQI,'yyyy-mm-dd') as PAIBANRQ,a.GUAHAOLB,2 as GUAHAOBC,A.KESHIID,C.keshimc,NVL(A.WEIZHI,C.weizhism) as JIUZHENDD, ");
                    sqlPaiBanXX.Append(" c.keshizl as KESHIJS,nvl(a.YISHENGID,'*') as YISHENGDM,nvl(d.zhigongxm,'普通') as YISHENGXM , e.daimamc as YISHENGZC,a.ZHENLIAOFXM,a.GUAHAOFXM, ");
                    sqlPaiBanXX.Append(" NVL(a.XIAWUXH,0) AS HAOYUANZS,NVL(a.XIAWUXH,0)-NVL(a.XIAWUYGH,0) AS SHENGYUHY, '' as YUYUELX ");
                    sqlPaiBanXX.Append(" from mz_v_guahaopb_ex_zzj a ,v_gy_keshi c,v_gy_zhigongxx d , ");
                    sqlPaiBanXX.Append(" (select * from gy_daima where daimalb='0072') e  ");
                    sqlPaiBanXX.Append(" where a.keshiid=c.keshiid and d.zhigongid(+) = a.YISHENGID and e.daimaid(+) = d.zhicheng  ");
                    
                    sqlPaiBanXX.Append(" and to_char(a.riqi,'yyyy-mm-dd')='" + riQi + "' ");
                    sqlPaiBanXX.Append("  and a.xiawuxh > 0 ");

                    if (!string.IsNullOrEmpty(keShiDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.keshiid,'*') = '" + keShiDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(yiShengDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.yishengid,'*') = '" + yiShengDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(guaHaoLB) && guaHaoLB != "0")
                    {
                        sqlPaiBanXX.Append(" and a.guahaolb = " + guaHaoLB + " ");
                    }
                }
                if (guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" ) ");
                }
                #endregion
            }

            if (guaHaoFS == "2")
            {
                #region 预约挂号
                if (guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" select * from ( ");
                }
                if (guaHaoBC == "1" || guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" select A.PAIBANID,to_Char(A.RIQI,'yyyy-mm-dd') as PAIBANRQ,a.GUAHAOLB,1 as GUAHAOBC,A.KESHIID,C.keshimc,NVL(A.WEIZHI,C.weizhism) as JIUZHENDD, ");
                    sqlPaiBanXX.Append(" c.keshizl as KESHIJS,nvl(a.YISHENGID,'*') as YISHENGDM,nvl(d.zhigongxm,'普通') as YISHENGXM , e.daimamc as YISHENGZC,a.ZHENLIAOFXM,a.GUAHAOFXM, ");
                    sqlPaiBanXX.Append(" NVL(f.shangwuyyxh,0) AS HAOYUANZS,NVL(f.shangwuyyxh,0)-NVL(f.shangwuyyyh,0) AS SHENGYUHY ,f.yuyuelx as YUYUELX ");
                    sqlPaiBanXX.Append(" from mz_v_guahaopb_ex_zzj a ,v_gy_keshi c,v_gy_zhigongxx d , ");
                    sqlPaiBanXX.Append(" (select * from gy_daima where daimalb='0072') e ,v_mz_guahaoyyxh f  ");
                    sqlPaiBanXX.Append(" where a.keshiid=c.keshiid and d.zhigongid(+) = a.YISHENGID and e.daimaid(+) = d.zhicheng and a.PAIBANID = f.paibanid  ");

                    if (string.IsNullOrEmpty(riQi))
                    {
                        //挂号预约可预约当天号源 0 不启用 1 启用
                        if (ConfigurationManager.AppSettings["GuaHaoYYDTHY"] == "1")
                        {
                            sqlPaiBanXX.Append(" and a.riqi >= trunc(sysdate) ");
                        }
                        else
                        {
                            sqlPaiBanXX.Append(" and a.riqi > trunc(sysdate) ");
                        }
                    }
                    else
                    {
                        sqlPaiBanXX.Append(" and to_char(a.riqi,'yyyy-mm-dd')='" + riQi + "' ");
                    }
                    sqlPaiBanXX.Append(" and f.yuyuelx in (" + yuYueLX + ") ");
                    sqlPaiBanXX.Append("  and a.shangwuxh > 0 and f.shangwuyyxh > 0 ");
                    if (!string.IsNullOrEmpty(keShiDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.keshiid,'*') = '" + keShiDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(yiShengDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.yishengid,'*') = '" + yiShengDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(guaHaoLB) && guaHaoLB != "0")
                    {
                        sqlPaiBanXX.Append(" and a.guahaolb = " + guaHaoLB + " ");
                    }
                }
                if (guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" union all ");
                }
                if (guaHaoBC == "2" || guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" select A.PAIBANID,to_Char(A.RIQI,'yyyy-mm-dd') as PAIBANRQ,a.GUAHAOLB,2 as GUAHAOBC,A.KESHIID,C.keshimc,NVL(A.WEIZHI,C.weizhism) as JIUZHENDD, ");
                    sqlPaiBanXX.Append(" c.keshizl as KESHIJS,nvl(a.YISHENGID,'*') as YISHENGDM,nvl(d.zhigongxm,'普通') as YISHENGXM , e.daimamc as YISHENGZC,a.ZHENLIAOFXM,a.GUAHAOFXM, ");
                    sqlPaiBanXX.Append(" NVL(f.xiawuyyxh,0) AS HAOYUANZS,NVL(f.xiawuyyxh,0)-NVL(f.xiawuyyyh,0) AS SHENGYUHY ,f.yuyuelx as YUYUELX ");
                    sqlPaiBanXX.Append(" from mz_v_guahaopb_ex_zzj a ,v_gy_keshi c,v_gy_zhigongxx d , ");
                    sqlPaiBanXX.Append(" (select * from gy_daima where daimalb='0072') e ,v_mz_guahaoyyxh f  ");
                    sqlPaiBanXX.Append(" where a.keshiid=c.keshiid and d.zhigongid(+) = a.YISHENGID and e.daimaid(+) = d.zhicheng and a.PAIBANID = f.paibanid  ");

                    if (string.IsNullOrEmpty(riQi))
                    {
                        sqlPaiBanXX.Append(" and a.riqi > sysdate ");
                    }
                    else
                    {
                        sqlPaiBanXX.Append(" and to_char(a.riqi,'yyyy-mm-dd')='" + riQi + "' ");
                    }
                    sqlPaiBanXX.Append(" and f.yuyuelx in (" + yuYueLX + ") ");
                    sqlPaiBanXX.Append("  and a.xiawuxh > 0  and f.xiawuyyxh > 0 ");
                    if (!string.IsNullOrEmpty(keShiDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.keshiid,'*') = '" + keShiDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(yiShengDM))
                    {
                        sqlPaiBanXX.Append(" and nvl(a.yishengid,'*') = '" + yiShengDM + "' ");
                    }
                    if (!string.IsNullOrEmpty(guaHaoLB) && guaHaoLB != "0")
                    {
                        sqlPaiBanXX.Append(" and a.guahaolb = " + guaHaoLB + " ");
                    }
                }
                if (guaHaoBC == "0")
                {
                    sqlPaiBanXX.Append(" ) ");
                }
                sqlPaiBanXX.Append(" order by paibanrq,paibanid,guahaobc ");
                #endregion
            }

            DataTable dtPaiBanXX = DBVisitor.ExecuteTable(sqlPaiBanXX.ToString());

            for (int i = 0; i < dtPaiBanXX.Rows.Count;i++ )
            {
                PAIBANXX pbxx = new PAIBANXX();
                pbxx.PAIBANID = dtPaiBanXX.Rows[i]["PAIBANID"].ToString();//排班ID
                pbxx.PAIBANRQ = dtPaiBanXX.Rows[i]["PAIBANRQ"].ToString();//排班日期
                pbxx.GUAHAOLB = dtPaiBanXX.Rows[i]["GUAHAOLB"].ToString();//挂号类别
                pbxx.GUAHAOBC = dtPaiBanXX.Rows[i]["GUAHAOBC"].ToString();//挂号班次
                pbxx.KESHIDM =dtPaiBanXX.Rows[i]["KESHIID"].ToString();//科室代码
                pbxx.KESHIMC = dtPaiBanXX.Rows[i]["keshimc"].ToString();//科室名称
                pbxx.JIUZHENDD = dtPaiBanXX.Rows[i]["JIUZHENDD"].ToString();//就诊地点（科室位置）
                pbxx.KESHIJS = dtPaiBanXX.Rows[i]["KESHIJS"].ToString();//科室介绍
                pbxx.YISHENGDM =dtPaiBanXX.Rows[i]["YISHENGDM"].ToString();//医生代码
                pbxx.YISHENGXM =dtPaiBanXX.Rows[i]["YISHENGXM"].ToString();//医生姓名
                pbxx.YISHENGZC =dtPaiBanXX.Rows[i]["YISHENGZC"].ToString();//医生职称
                string zlfxm = dtPaiBanXX.Rows[i]["ZHENLIAOFXM"].ToString();//诊疗费
                pbxx.ZHENLIAOF = Unity.GetXiangMuFY(zlfxm);
                string ghfxm = dtPaiBanXX.Rows[i]["GUAHAOFXM"].ToString();//挂号费
                pbxx.GUAHAOF = Unity.GetXiangMuFY(ghfxm);
                pbxx.HAOYUANZS = dtPaiBanXX.Rows[i]["HAOYUANZS"].ToString();//号源总数
                pbxx.SHENGYUHY = dtPaiBanXX.Rows[i]["SHENGYUHY"].ToString();//剩余号源
                if (Convert.ToInt32(pbxx.HAOYUANZS) <= 0) {
                    pbxx.SHENGYUHY = "-1";
                }
               
                pbxx.HAOYUANZS = Convert.ToInt32(pbxx.HAOYUANZS) < 0 ? "0" : pbxx.HAOYUANZS;
                pbxx.SHENGYUHY = Convert.ToInt32(pbxx.SHENGYUHY) < 0 ? "0" : pbxx.SHENGYUHY;
                pbxx.GUAHAOFS = guaHaoFS;
                pbxx.YUYUELX = dtPaiBanXX.Rows[i]["YUYUELX"].ToString();//预约类型
                OutObject.PAIBANMX.Add(pbxx);
            }

            if (OutObject.PAIBANMX.Count <= 0) {
                throw new Exception("未找到排班信息！");
            }
        
        }
    }
}
