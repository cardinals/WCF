using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using SWSoft.Framework;
using HIS4.Schemas;

namespace HIS4.Biz
{
    public class GUAHAOYYQH : IMessage<GUAHAOYYQH_IN, GUAHAOYYQH_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new GUAHAOYYQH_OUT();
            string jiuzhenkLx = InObject.JIUZHENKLX;
            string jiuzhenKh = InObject.JIUZHENKH;
            string quhaomm = InObject.QUHAOMM;
            string caozuoyDm = InObject.BASEINFO.CAOZUOYDM;
            string caozuoyXm = InObject.BASEINFO.CAOZUOYXM;
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;
            string daiShouFY = InObject.DAISHOUFY;
            string yuYueLY = InObject.YUYUELY;

            #region 基本信息有效性判断
            if (string.IsNullOrEmpty(daiShouFY))
            {//默认不代收
                daiShouFY = "0";
            }

            if (string.IsNullOrEmpty(jiuzhenkLx))
            {
                throw new Exception("卡类型获取失败，请重新尝试挂号！");
            }
            if (string.IsNullOrEmpty(jiuzhenKh))
            {
                throw new Exception("就诊卡号获取失败，请重新尝试挂号！");
            }
            if (string.IsNullOrEmpty(quhaomm))
            {
                throw new Exception("取号密码获取失败，请重新尝试挂号！");
            }
            #endregion


            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" select b.guahaolb guahaolb ,c.daimamc guahaolbmc,b.keshiid,a.yuyueksmc keshimc ,b.yishengid,to_char(a.yuyuesj,'yyyy-mm-dd')  riqi,a.shangxiawbz guahaobc, ");
            sbSql.Append(" a.guahaoxh guahaoxh ,b.weizhi jiuzhendd,b.paibanid,a.yuyuehao ,a.yuyuelx,a.yuyueid,nvl(b.zhenliaofxm,'*') as zhenliaofxm ,nvl(b.guahaofxm,'*') as guahaofxm ");
            sbSql.Append(" from mz_guahaoyy a,mz_v_guahaopb_ex_zzj b,(select * from gy_daima where daimalb='0025' and zuofeibz='0') c  where a.paibanid = b.paibanid  and b.guahaolb = c.daimaid ");
            sbSql.Append(" and a.yuyuehao = '" + quhaomm + "' and ((a.xingming in( select xingming from gy_bingrenxx where jiuzhenkh = '" + jiuzhenKh + "')) or (a.jiuzhenkh = '" + jiuzhenKh + "'))  ");
            DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
            if (dt.Rows.Count > 0)
            {
                OutObject.GUAHAOLB = dt.Rows[0]["GUAHAOLB"].ToString();
                OutObject.GUAHAOLBMC = dt.Rows[0]["GUAHAOLBMC"].ToString();
                OutObject.KESHIDM = dt.Rows[0]["KESHIID"].ToString();
                OutObject.KESHIMC = dt.Rows[0]["KESHIMC"].ToString();
                OutObject.DANGTIANPBID = dt.Rows[0]["PAIBANID"].ToString();
                OutObject.YUYUEID = dt.Rows[0]["YUYUEID"].ToString();
                OutObject.YISHENGDM = dt.Rows[0]["YISHENGID"].ToString();
                OutObject.RIQI = dt.Rows[0]["RIQI"].ToString();
                OutObject.GUAHAOBC = dt.Rows[0]["guahaobc"].ToString();
                OutObject.JIUZHENDD = dt.Rows[0]["jiuzhendd"].ToString();
                OutObject.GUAHAOXH = dt.Rows[0]["guahaoxh"].ToString();
                OutObject.YUYUEHAO = dt.Rows[0]["YUYUEHAO"].ToString();
                string zhenLiaoXmXh = dt.Rows[0]["zhenliaofxm"].ToString();
                string guaHaoXmXh = dt.Rows[0]["guahaofxm"].ToString();
                if (!string.IsNullOrEmpty(zhenLiaoXmXh) && zhenLiaoXmXh != "*") 
                {//诊疗项目信息 
                    MENZHENFYXX fyxx = getXiangMuXX(zhenLiaoXmXh);
                    if (fyxx != null) {
                        OutObject.FEIYONGMX.Add(fyxx);
                    }
                }
                if (!string.IsNullOrEmpty(guaHaoXmXh) && guaHaoXmXh != "*")
                {//挂号项目信息 
                    MENZHENFYXX fyxx = getXiangMuXX(guaHaoXmXh);
                    if (fyxx != null)
                    {
                        OutObject.FEIYONGMX.Add(fyxx);
                    }
                }
                string ysdm = string.IsNullOrEmpty(OutObject.YISHENGDM)?"*":OutObject.YISHENGDM;
                OutObject.JIUZHENSJ = Unity.getJiuZhenSJD(ysdm, OutObject.KESHIDM, OutObject.GUAHAOXH, dt.Rows[0]["YUYUELX"].ToString(), ((OutObject.GUAHAOBC == "1") ? 0 : 1), OutObject.GUAHAOLB, OutObject.RIQI);

                //获取医生姓名
                if (!string.IsNullOrEmpty(OutObject.YISHENGDM)&&OutObject.YISHENGDM!="*")
                {
                    DataTable dtYs = DBVisitor.ExecuteTable("select xm from gy_zgxx where zgid ='" + OutObject.YISHENGDM + "' ");
                    //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "挂号信息查询：" + OutObject.YISHENGDM + ":医生信息：" + "select xm from gy_zgxx where zgid ='" + OutObject.YISHENGDM + "' ", messageId);
                    if (dtYs.Rows.Count > 0)
                    {
                        OutObject.YISHENGXM = dtYs.Rows[0]["XM"].ToString();
                    }
                }
            }
            else
            {
                throw new Exception("取号密码错误或没有该预约记录！");
            }
        }

        public MENZHENFYXX getXiangMuXX(string xiangMuXH) {
            MENZHENFYXX mzfyxx = null;
            string mzfyxxSql = "select a.shoufeixmid, a.shoufeixmmc, a.hesuanxm,a.jijiadw,a.danjia1,a.yibaodj,b.hesuanxmmc from gy_shoufeixm a,gy_hesuanxm b where a.hesuanxm = b.hesuanxmid and a.zuofeibz ='0' and shoufeixmid = '{0}' ";
            if (!string.IsNullOrEmpty(xiangMuXH) && xiangMuXH != "*")
            {
                DataTable dtFYXX = DBVisitor.ExecuteTable(string.Format(mzfyxxSql, xiangMuXH));
                if (dtFYXX.Rows.Count > 0)
                {
                    mzfyxx = new MENZHENFYXX();
                    mzfyxx.XIANGMUXH = dtFYXX.Rows[0]["shoufeixmid"].ToString();//项目序号
                    mzfyxx.XIANGMUMC = dtFYXX.Rows[0]["shoufeixmmc"].ToString();//项目名称
                    mzfyxx.XIANGMUGL = dtFYXX.Rows[0]["hesuanxm"].ToString();//项目归类
                    mzfyxx.XIANGMUGLMC = dtFYXX.Rows[0]["hesuanxmmc"].ToString();//项目归类名称
                    mzfyxx.XIANGMUDW = dtFYXX.Rows[0]["jijiadw"].ToString();//项目单位
                    mzfyxx.DANJIA = dtFYXX.Rows[0]["danjia1"].ToString();//单价
                    mzfyxx.SHULIANG = "1";//数量
                    mzfyxx.FEIYONGLX = "0";
                    mzfyxx.YIBAODJ = dtFYXX.Rows[0]["yibaodj"].ToString();//医保等级
                }
            }
            return mzfyxx;
        }
    }
}
