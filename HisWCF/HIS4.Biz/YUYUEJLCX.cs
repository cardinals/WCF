using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIS4.Schemas;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using System.Configuration;

namespace HIS4.Biz
{
    public class YUYUEJLCX : IMessage<YUYUEJLCX_IN,YUYUEJLCX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new YUYUEJLCX_OUT();
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号

            string xingMing = InObject.XINGMING;//姓名
            string zhengjianHm = InObject.ZHENGJIANHM;//证件号码
            string lianxiDh = InObject.LIANXIDH;//联系电话
            string yuyuerq = InObject.YUYUERQ;//预约日期
            string quHaoZt = InObject.QUHAOZT;//取号状态

            string yuYueCZFS = ConfigurationManager.AppSettings["GUAHAOYYCZFS"];//挂号预约处理方式
             if (!string.IsNullOrEmpty(yuYueCZFS) && yuYueCZFS == "1")
             {
                 if (string.IsNullOrEmpty(lianxiDh)) {
                     throw new Exception("联系电话不能为空！");
                 }
                 string yyjlcx = @"  select xingming,jiuzhenyyid yuyueid,jiuzhenyyid quhaomm,yishengid yishengdm,(select zhigongxm from gy_zhigongxx where zhigongid = a.yishengid ) yishengxm,keshiid keshidm,(select keshimc from gy_keshi where keshiid = a.keshiid) keshimc,to_char(yuyuesj,'yyyy-mm-dd') riqi,
                            to_Char(yuyuesj,'hh24:mi:ss') jiuzhensj,guahaoyyxh guahaoxh,decode(shangxiawbz,0,1,2) guahaobc,DIANHUA,YUYUEZT,CAOZUORQ,paibanid,beizhu  from v_mz_guahaoyyxh_yshyh  a  where dianhua = '{0}' ";
                 DataTable dtYYJL = DBVisitor.ExecuteTable(string.Format(yyjlcx,lianxiDh));

                 if (dtYYJL.Rows.Count <= 0) {
                     throw new Exception("未找到预约记录！");
                 }

                 for (int i = 0; i < dtYYJL.Rows.Count; i++) {

                     YUYUEXX yuyuexx = new YUYUEXX();
                     yuyuexx.BINGRENXM = dtYYJL.Rows[i]["xingming"].ToString();//病人姓名
                     yuyuexx.YUYUEID = dtYYJL.Rows[i]["yuyueid"].ToString();//预约ID
                     yuyuexx.QUHAOMM = dtYYJL.Rows[i]["quhaomm"].ToString();//取号密码
                     yuyuexx.GUAHAOBC = dtYYJL.Rows[i]["guahaobc"].ToString();//挂号班次
                     yuyuexx.GUAHAOXH = dtYYJL.Rows[i]["guahaoxh"].ToString();//挂号序号
                     yuyuexx.JIUZHENSJ = dtYYJL.Rows[i]["jiuzhensj"].ToString();//就诊时间
                     yuyuexx.KESHIDM = dtYYJL.Rows[i]["keshidm"].ToString();//科室代码
                     yuyuexx.KESHIMC = dtYYJL.Rows[i]["keshimc"].ToString();//科室名称
                     yuyuexx.YISHENGDM = dtYYJL.Rows[i]["yishengdm"].ToString();//医生代码
                     yuyuexx.YISHENGXM = dtYYJL.Rows[i]["yishengxm"].ToString();//医生姓名
                     yuyuexx.RIQI = dtYYJL.Rows[i]["riqi"].ToString();//就诊日期
                     yuyuexx.YUYUESJ = dtYYJL.Rows[i]["caozuorq"].ToString();
                     yuyuexx.YUYUEZT = dtYYJL.Rows[i]["yuyuezt"].ToString();
                     yuyuexx.BEIZHU = dtYYJL.Rows[i]["beizhu"].ToString();
                     if (yuyuexx.YUYUEZT == "0")
                     {
                         yuyuexx.YUYUEZTSM = "未挂号";
                     }
                     else if (yuyuexx.YUYUEZT == "1")
                     {
                         yuyuexx.YUYUEZTSM = "已挂号";
                     }
                     else if (yuyuexx.YUYUEZT == "2")
                     {
                         yuyuexx.YUYUEZTSM = "预约取消";
                     }
                     else if (yuyuexx.YUYUEZT == "3")
                     {
                         yuyuexx.YUYUEZTSM = "预约过期";
                     }

                     string paiBanID = dtYYJL.Rows[i]["paibanid"].ToString();
                     OutObject.YUYUEMX.Add(yuyuexx);
                 }
             }
             else
             {
                 #region 标准his4业务流程
                 
                 #endregion
                 #region 入参判断
                 if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengjianHm))
                 {
                     throw new Exception("就诊卡号和证件号码不能同时为空");
                 }
                 //if (string.IsNullOrEmpty(lianxiDh))
                 //{
                 //    throw new Exception("联系电话不能为空");
                 //}
                 #endregion
                 StringBuilder sbSql = new StringBuilder("select a.yuyueid, a.yuyuezt,to_char(a.caozuorq,'yyyy-mm-dd hh24:mi:ss') caozuorq, a.yuyuehao quhaomm,decode(a.shangxiawbz,'0','上午','1','下午','其他') guahaobc,a.guahaoxh, ");
                 sbSql.Append("to_char(a.yuyuesj,'yyyy-mm-dd hh24:mi:ss') jiuzhensj,a.yuyueks keshidm,a.yuyueksmc keshimc,a.paibanid, ");//
                 sbSql.Append("a.yuyueys yishengdm,a.yuyueysxm yishengxm,to_char(a.yuyuesj,'yyyy-mm-dd') riqi from mz_guahaoyy a  WHERE 1=1 ");//          
                 if (!string.IsNullOrEmpty(jiuzhenKh))
                     sbSql.Append("and a.jiuzhenkh='" + jiuzhenKh + "' ");
                 //if (jiuzhenkLx != string.Empty)
                 //    sbSql.Append("and a.jiuzhenkLx='" + jiuzhenkLx + "' ");//？
                 if (!string.IsNullOrEmpty(xingMing))
                     sbSql.Append("and a.xingming ='" + xingMing + "' ");
                 if (!string.IsNullOrEmpty(zhengjianHm))
                     sbSql.Append("and a.shenfenzh ='" + zhengjianHm + "' ");
                 if (!string.IsNullOrEmpty(lianxiDh))
                     sbSql.Append("and a.lianxirdh ='" + lianxiDh + "' ");
                 if (!string.IsNullOrEmpty(yuyuerq))
                     sbSql.Append(" and to_char(a.yuyuesj,'yyyy-mm-dd') ='" + yuyuerq + "' ");
                 if (quHaoZt != "4")
                 {
                     if (!string.IsNullOrEmpty(quHaoZt))
                     {
                         sbSql.Append(" and a.yuyuezt =" + quHaoZt + " ");
                     }
                 }
                 sbSql.Append(" order by a.caozuorq desc ");
                 DataTable dt = DBVisitor.ExecuteTable(sbSql.ToString());
                 if (dt.Rows.Count == 0)
                 {
                     throw new Exception("未找到预约记录");
                 }
                 else
                 {
                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         YUYUEXX yuyuexx = new YUYUEXX();
                         yuyuexx.YUYUEID = dt.Rows[i]["yuyueid"].ToString();//预约ID
                         yuyuexx.QUHAOMM = dt.Rows[i]["quhaomm"].ToString();//取号密码
                         yuyuexx.GUAHAOBC = dt.Rows[i]["guahaobc"].ToString();//挂号班次
                         yuyuexx.GUAHAOXH = dt.Rows[i]["guahaoxh"].ToString();//挂号序号
                         yuyuexx.JIUZHENSJ = dt.Rows[i]["jiuzhensj"].ToString();//就诊时间
                         yuyuexx.KESHIDM = dt.Rows[i]["keshidm"].ToString();//科室代码
                         yuyuexx.KESHIMC = dt.Rows[i]["keshimc"].ToString();//科室名称
                         yuyuexx.YISHENGDM = dt.Rows[i]["yishengdm"].ToString();//医生代码
                         yuyuexx.YISHENGXM = dt.Rows[i]["yishengxm"].ToString();//医生姓名
                         yuyuexx.RIQI = dt.Rows[i]["riqi"].ToString();//就诊日期
                         yuyuexx.YUYUESJ = dt.Rows[i]["caozuorq"].ToString();
                         yuyuexx.YUYUEZT = dt.Rows[i]["yuyuezt"].ToString();
                         if (yuyuexx.YUYUEZT == "0")
                         {
                             yuyuexx.YUYUEZTSM = "未挂号";
                         }
                         else if (yuyuexx.YUYUEZT == "1")
                         {
                             yuyuexx.YUYUEZTSM = "已挂号";
                         }
                         else if (yuyuexx.YUYUEZT == "2")
                         {
                             yuyuexx.YUYUEZTSM = "预约取消";
                         }
                         else if (yuyuexx.YUYUEZT == "3")
                         {
                             yuyuexx.YUYUEZTSM = "预约过期";
                         }

                         string paiBanID = dt.Rows[i]["paibanid"].ToString();
                         string shouFeiXMSql = "select zhenliaofxm,guahaofxm from mz_v_guahaopb_ex_zzj where paibanid = '{0}' ";
                         DataTable dtShouFeiXM = DBVisitor.ExecuteTable(string.Format(shouFeiXMSql, paiBanID));
                         if (dtShouFeiXM.Rows.Count > 0)
                         {
                             string zhenLiaoFXM = dtShouFeiXM.Rows[0]["zhenliaofxm"].ToString();
                             string guaHaoFXM = dtShouFeiXM.Rows[0]["guahaofxm"].ToString();
                             yuyuexx.GUAHAOF = Unity.GetXiangMuFY(guaHaoFXM);
                             yuyuexx.ZHENLIAOF = Unity.GetXiangMuFY(zhenLiaoFXM);
                         }

                         OutObject.YUYUEMX.Add(yuyuexx);
                     }

                 }
             }
        }
    }
}
