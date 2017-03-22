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
    public class JIUZHENXXCX : IMessage<JIUZHENXXCX_IN, JIUZHENXXCX_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new JIUZHENXXCX_OUT();
            string zhengjianLx = InObject.ZHENGJIANLX;
            string zhengjianHm = InObject.ZHENGJIANHM; 
            string kaishiRq = InObject.KAISHIRQ;
            string jieshuRq = InObject.JIESHURQ;
            string chaxunLx = InObject.CHAXUNLX;//1门诊，2住院
            string shujuLy = InObject.SHUJULY;//1 his，2 邦泰

            #region 基本入参判断
            if (string.IsNullOrEmpty(zhengjianLx))
            {
                throw new Exception("证件类型不能为空！");
            }
            if (string.IsNullOrEmpty(zhengjianHm))
            {
                throw new Exception("证件号码不能为空！");
            }
            if (string.IsNullOrEmpty(kaishiRq))
            {
                throw new Exception("开始日期不能为空！");
            }
            if (string.IsNullOrEmpty(jieshuRq))
            {
                throw new Exception("结束日期不能为空！");
            }
            if (string.IsNullOrEmpty(chaxunLx))
            {
                throw new Exception("查询类型不能为空！");
            }
            if (string.IsNullOrEmpty(shujuLy))
            {
                throw new Exception("数据来源不能为空！");
            }

            #endregion

            StringBuilder sbStr = new StringBuilder();
            if (chaxunLx == "1")//门诊
            {
                sbStr.Append("select a.jiuzhenkh,a.feiyonglb bingrenlb,a.feiyongxz bingrenxz,a.yibaokh,a.gerenbh,  ");
                sbStr.Append("a.binganhao binglibh,a.xingming,a.xingbie,a.minzu,a.hunyin, to_char(a.chushengrq,'yyyy-mm-dd hh24:mi:ss') chushengrq, ");
                sbStr.Append("a.zhengjianlx,a.shenfenzh zhengjianhm,''danweilx,''danweibh, c.gongzuodw danweimc, ");
                sbStr.Append("a.jiatingdz,a.binganhao binganh, b.xingzhimc bingrenxzmc, '' dangqianbq,c.jiuzhenks dangqianks, ");
                sbStr.Append("c.jiuzhenys zhuzhiyisheng, c.chuangweiid chuangweih, c.jiezhensj ruyuanrq, c.jiuzhenrq chuyuanrq, ");
                sbStr.Append("a.dianhua lianxidh,a.lianxiren lianxir,'' zhiliaojg,''chuyuanqk, ''chuyuanzt,  ");
                sbStr.Append("c.guahaoys shuxieys, c.linchuangzd binglizd, d.yuanqumc yuyueyy,c.jiuzhenid ");
                sbStr.Append("from zj_jiuzhenxx c, gy_bingrenxx a, gy_feiyongxz b, gy_yuanqu d  ");
                sbStr.Append("where b.XINGZHIID(+)=a.feiyongxz and a.bingrenid=c.bingrenid  ");
                sbStr.Append("and d.yuanquid(+) = c.yuanquid and c.zuofeibz = '0' ");
                if (kaishiRq != "")
                    sbStr.Append("and c.jiuzhenrq>=to_date('" + kaishiRq + "','yyyy-mm-dd hh24:mi:ss') ");
                if (jieshuRq != "")
                    sbStr.Append("and (c.jiuzhenrq<=to_date('" + jieshuRq + " 23:59:59','yyyy-mm-dd hh24:mi:ss') or c.jiuzhenrq is null) ");
            }
            else //住院，二期再考虑
            {
                sbStr.Append("select a.jiuzhenkh,a.feiyonglb bingrenlb,a.feiyongxz bingrenxz,a.yibaokh,a.gerenbh, ");
                sbStr.Append("a.binganhao binglibh,a.xingming,a.xingbie,a.minzu,a.hunyin, ");
                sbStr.Append("to_char(a.chushengrq,'yyyy-mm-dd hh24:mi:ss') chushengrq,'' zhengjianlx,a.shenfenzh zhengjianhm,''danweilx,''danweibh, ");
                sbStr.Append("'' danweimc,a.jiatingdz,a.binganhao binganh, b.xingzhimc bingrenxzmc,a.dangqianbq,a1.dangqianks,a.zhuzhiys zhuzhiyisheng, ");
                sbStr.Append("a.dangqiancw chuangweih, a.ruyuanrq,a.chuyuanrq, a.lianxirdh lianxidh,a.lianxiren lianxir,'' zhiliaojg, ");
                sbStr.Append("a.chuyuanzdmc chuyuanqk,'' chuyuanzt, ''shuxieys, '' binglizd, '' yuyueyy ");
                sbStr.Append("from zy_bingrenxx a, gy_feiyongxz b where b.XINGZHIID(+)=a.feiyongxz ");
                if (kaishiRq != "")
                    sbStr.Append("and a.ruyuanrq>=to_date('" + kaishiRq + "','yyyy-mm-dd hh24:mi:ss') ");
                if (jieshuRq != "")
                    sbStr.Append("and a.chuyuanrq<=to_date('" + jieshuRq + " 23:59:59','yyyy-mm-dd hh24:mi:ss') ");
            }
            //if (zhengjianLx != "")
            //    sbStr.Append("and a.zhengjianlx='" + zhengjianLx + "' ");
            if (zhengjianHm != "")
                sbStr.Append("and a.shenfenzh='" + zhengjianHm + "' ");
            if (shujuLy != "1")//非his
            {
                sbStr.Append("and 1<>1 ");//取空数据
            }


            DataTable dtJiuZhenXX = DBVisitor.ExecuteTable(sbStr.ToString());

            if (dtJiuZhenXX != null && dtJiuZhenXX.Rows.Count > 0)
            {
                OutObject.FEIYONGMXTS = dtJiuZhenXX.Rows.Count.ToString();
                for (int i = 0; i < dtJiuZhenXX.Rows.Count; i++)
                {
                    JIUZHENXX jzxx = new JIUZHENXX();
                    jzxx.JIUZHENKH = dtJiuZhenXX.Rows[i]["JIUZHENKH"].ToString();
                    jzxx.BINGRENLB = dtJiuZhenXX.Rows[i]["BINGRENLB"].ToString();
                    jzxx.BINGRENXZ = dtJiuZhenXX.Rows[i]["BINGRENXZ"].ToString();
                    jzxx.YIBAOKH = dtJiuZhenXX.Rows[i]["YIBAOKH"].ToString();
                    jzxx.GERENBH = dtJiuZhenXX.Rows[i]["GERENBH"].ToString();
                    jzxx.BINGLIBH = dtJiuZhenXX.Rows[i]["BINGLIBH"].ToString();
                    jzxx.XINGMING = dtJiuZhenXX.Rows[i]["XINGMING"].ToString();
                    jzxx.XINGBIE = dtJiuZhenXX.Rows[i]["XINGBIE"].ToString();
                    jzxx.MINZU = dtJiuZhenXX.Rows[i]["MINZU"].ToString();
                    jzxx.HUNYIN = dtJiuZhenXX.Rows[i]["HUNYIN"].ToString();
                    jzxx.CHUSHENGRQ = dtJiuZhenXX.Rows[i]["CHUSHENGRQ"].ToString();
                    jzxx.ZHENGJIANLX = dtJiuZhenXX.Rows[i]["ZHENGJIANLX"].ToString();
                    jzxx.ZHENGJIANHM = dtJiuZhenXX.Rows[i]["ZHENGJIANHM"].ToString();
                    jzxx.DANWEILX = dtJiuZhenXX.Rows[i]["DANWEILX"].ToString();
                    jzxx.DANWEIBH = dtJiuZhenXX.Rows[i]["DANWEIBH"].ToString();
                    jzxx.DANWEIMC = dtJiuZhenXX.Rows[i]["DANWEIMC"].ToString();
                    //jzxx.JIATINGZZ = dtJiuZhenXX.Rows[i]["JIATINGZZ"].ToString();
                    jzxx.BINGANH = dtJiuZhenXX.Rows[i]["BINGANH"].ToString();
                    jzxx.BINGRENXZMC = dtJiuZhenXX.Rows[i]["BINGRENXZMC"].ToString();
                    jzxx.DANGQIANBQ = dtJiuZhenXX.Rows[i]["DANGQIANBQ"].ToString();
                    jzxx.DANGQIANKS = dtJiuZhenXX.Rows[i]["DANGQIANKS"].ToString();
                    jzxx.ZHUZHIYISHENG = dtJiuZhenXX.Rows[i]["ZHUZHIYISHENG"].ToString();
                    jzxx.CHUANGWEIH = dtJiuZhenXX.Rows[i]["CHUANGWEIH"].ToString();
                    jzxx.RUYUANRQ = dtJiuZhenXX.Rows[i]["RUYUANRQ"].ToString();
                    jzxx.CHUYUANRQ = dtJiuZhenXX.Rows[i]["CHUYUANRQ"].ToString();
                    jzxx.LIANXIDH = dtJiuZhenXX.Rows[i]["LIANXIDH"].ToString();
                    jzxx.LIANXIR = dtJiuZhenXX.Rows[i]["LIANXIR"].ToString();
                    jzxx.ZHILIAOJG = dtJiuZhenXX.Rows[i]["ZHILIAOJG"].ToString();
                    jzxx.CHUYUANQK = dtJiuZhenXX.Rows[i]["CHUYUANQK"].ToString();
                    jzxx.CHUYUANZT = dtJiuZhenXX.Rows[i]["CHUYUANZT"].ToString();
                    jzxx.SHUXIEYS = dtJiuZhenXX.Rows[i]["SHUXIEYS"].ToString();
                    jzxx.BINGLIZD = dtJiuZhenXX.Rows[i]["BINGLIZD"].ToString();
                    jzxx.YUYUEYY = dtJiuZhenXX.Rows[i]["YUYUEYY"].ToString();
                    var jiuZhenID = dtJiuZhenXX.Rows[i]["jiuzhenid"].ToString();
                    string SqlYongYaoXX = "select a.yaopinmc,a.pinci,a.zuixiaodw,a.yaopingg from mz_chufang2 a,mz_chufang1 b where a.chufangid=b.chufangid and b.jiuzhenid = '{0}'";
                    DataTable dtYongYaoXX = DBVisitor.ExecuteTable(string.Format(SqlYongYaoXX, jiuZhenID));
                    if (dtYongYaoXX!= null &&dtYongYaoXX.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtYongYaoXX.Rows.Count; j++)
                        {
                            MENZHENYONGYAOXX MZYYXX = new MENZHENYONGYAOXX();
                            MZYYXX.YAOPINDW = dtYongYaoXX.Rows[j]["zuixiaodw"].ToString();
                            MZYYXX.YAOPINGG = dtYongYaoXX.Rows[j]["yaopingg"].ToString();
                            MZYYXX.YAOPINMC = dtYongYaoXX.Rows[j]["yaopinmc"].ToString();
                            MZYYXX.YONGYAOPC = dtYongYaoXX.Rows[j]["pinci"].ToString();
                            jzxx.MENZHENYONGYAOMX.Add(MZYYXX);
                        }
                    }

                    OutObject.JIUZHENMX.Add(jzxx);
                }
            }
            else {
                OutObject.FEIYONGMXTS = "0";
            }

        }
    }
}
