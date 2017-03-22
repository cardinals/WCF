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
    public class JIESHOUSZSQ : IMessage<JIESHOUSZSQ_IN, JIESHOUSZSQ_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new JIESHOUSZSQ_OUT();
            string jiuzhenKh = InObject.JIUZHENKH;
            string jiuzhenkLx = InObject.JIUZHENKLX;

            int JIUZHENKCD = Convert.ToInt32(ConfigurationManager.AppSettings["JIUZHENKCD"]);//就诊卡默认长度

            #region 基本入参判断
            if (InObject.YEWULX == null || InObject.YEWULX == "")
            {
                throw new Exception(string.Format("业务类型不能为空！"));
            }
            if (InObject.BINGRENXM == null || InObject.BINGRENXM == "")
            {
                throw new Exception(string.Format("病人姓名不能为空！"));
            }
            if (InObject.BINGRENXB == null || InObject.BINGRENXB == "")
            {
                throw new Exception(string.Format("病人性别不能为空！"));
            }
            if (InObject.BINGRENCSRQ == null || InObject.BINGRENCSRQ == "")
            {
                throw new Exception(string.Format("病人出生日期不能为空！"));
            }
            if (InObject.BINGRENSFZH == null || InObject.BINGRENSFZH == "")
            {
                throw new Exception(string.Format("病人身份证号不能为空！"));
            }
            if (InObject.BINGRENLXDH == null || InObject.BINGRENLXDH == "")
            {
                throw new Exception(string.Format("病人联系电话不能为空！"));
            }
            if (InObject.BINGRENLXDZ == null || InObject.BINGRENLXDZ == "")
            {
                throw new Exception(string.Format("病人联系地址不能为空！"));
            }
            if (InObject.SHENQINGJGDM == null || InObject.SHENQINGJGDM == "")
            {
                throw new Exception(string.Format("申请机构代码不能为空！"));
            }
            if (InObject.SHENQINGJGMC == null || InObject.SHENQINGJGMC == "")
            {
                throw new Exception(string.Format("申请机构名称不能为空！"));
            }
            if (InObject.SHENQINGYS == null || InObject.SHENQINGYS == "")
            {
                throw new Exception(string.Format("申请医生不能为空！"));
            }
            if (InObject.SHENQINGYSDH == null || InObject.SHENQINGYSDH == "")
            {
                throw new Exception(string.Format("申请医生电话不能为空！"));
            }
            if (InObject.SHENQINGRQ == null || InObject.SHENQINGRQ == "")
            {
                throw new Exception(string.Format("申请日期不能为空！"));
            }
            if (InObject.BINQINGMS == null || InObject.BINQINGMS == "")
            {
                throw new Exception(string.Format("病情描述不能为空！"));
            }
            if (InObject.ZHUANZHENZYSX == null || InObject.ZHUANZHENZYSX == "")
            {
                throw new Exception(string.Format("转诊注意事项不能为空！"));
            }
            #endregion

            if (JIUZHENKCD > 0)
            {
                if (jiuzhenKh.Length < JIUZHENKCD)
                {
                    jiuzhenKh = jiuzhenKh.PadLeft(JIUZHENKCD, '0');
                }
            }

            //转诊申请单号
            var zzsqdbh = DBVisitor.ExecuteScalar(SqlLoad.GetFormat("select seq_sxzz_zzsqd.nextval zzsqd from dual")).ToString();

            var tran = DBVisitor.Connection.BeginTransaction();
            try
            {
                #region//申请单信息
                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00007,
                    zzsqdbh, InObject.JIUZHENKLX, InObject.JIUZHENKH, InObject.YEWULX,
                    InObject.BINGRENXM, InObject.BINGRENXB, InObject.BINGRENCSRQ,
                    InObject.BINGRENNL, InObject.BINGRENSFZH, InObject.BINGRENLXDH,
                    InObject.BINGRENLXDZ, InObject.BINGRENFYLB, InObject.SHENQINGJGDM,
                    InObject.SHENQINGJGMC, InObject.SHENQINGJGLXDH, InObject.SHENQINGYS,
                    InObject.SHENQINGYSDH, InObject.SHENQINGRQ, InObject.ZHUANZHENYY,
                    InObject.BINQINGMS, InObject.ZHUANZHENZYSX, InObject.ZHUANZHENDH,
                    InObject.SZJSLXR, InObject.SZJSLXRDH, InObject.ZHUANRUKSDM,
                    InObject.ZHUANRUKSMC), tran);
                //申请单状态
                //DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00077, zzsqdbh), tran);
                #endregion
                #region //门诊处方信息
                foreach (var item in InObject.CHUFANGMX)
                {
                    int i = 0;
                    if (item.CHUFANGXXMX.Count > 0)
                    {
                        if (string.IsNullOrEmpty(item.CHUFANGID))
                        {
                            throw new Exception(string.Format("处方ID不能为空！"));
                        }
                        /*zzsqdh,cfid,cfly,cflx,kfrq,bz*/
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00008,
                        zzsqdbh, item.CHUFANGID, item.CHUFANGLY, item.CHUFANGLX, item.KAIFANGRQ, item.BEIZHU), tran);
                        foreach (var itemmx in item.CHUFANGXXMX)
                        {
                            if (string.IsNullOrEmpty(itemmx.XIANGMUMC))
                            {
                                throw new Exception(string.Format("药品项目名称不能为空！"));
                            }
                            //门诊处方明细
                            /*xh,cfid,fylx,xmmc,yptym,ypspm,
                                  cdmc,ypgg,dw,sl,pl,gytj,
                                  yyts,dcyl,yldw,psjg,zcyts,
                                  fyrq*/
                            ++i;
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00009,
                                i, item.CHUFANGID, itemmx.FEIYONGLX, itemmx.XIANGMUMC, itemmx.YAOPINTYM, itemmx.YAOPINSPM,
                                itemmx.CHANGDIMC, itemmx.YAOPINGG, itemmx.DANGWEI, itemmx.SHULIANG, itemmx.PINLV, itemmx.GEIYAOTJ,
                                itemmx.YONGYAOTS, itemmx.DANCIYL, itemmx.YONGLIANGDW, itemmx.PISHIJG, itemmx.ZHONGCHAOYTS,
                                itemmx.FAYAORQ, zzsqdbh), tran);
                        }
                    }
                }
                #endregion
                #region//检验处方信息
                foreach (var item in InObject.JIANYANMX)
                {
                    int i = 0;
                    if (item.JIANYANXXMX.Count > 0)
                    {
                        if (string.IsNullOrEmpty(item.JIANYANID))
                        {
                            throw new Exception(string.Format("检验ID不能为空！"));
                        }
                        /*zzsqdh,jyid,xmmc,kdrq,bz,jyjg,
                        jcff,wjzbz,jczd,jcrq,yblxmc,ybh,
                        shys,jyrq*/
                        DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00010,
                        zzsqdbh, item.JIANYANID, item.XIANGMUMC, item.KAIDANRQ, item.BEIZHU, item.JIANYANJG,
                        item.JIANCEFF, item.WEIJIZBZ, item.JIANCHAZD, item.JIANCHARQ, item.YANGBENLXMC, item.YANGBENGH,
                        item.SHENGHEYS, item.JIANYANRQ), tran);
                        foreach (var itemmx in item.JIANYANXXMX)
                        {
                            if (string.IsNullOrEmpty(itemmx.XIANGMUMC))
                            {
                                throw new Exception(string.Format("检验项目名称不能为空！"));
                            }
                            //检验处方明细
                            /*xh,jyid,xmmc,jyz,dyxh,dx,
                                  fw,dw*/
                            ++i;
                            DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00011,
                                i, item.JIANYANID, itemmx.XIANGMUMC, itemmx.JIANYANZ, itemmx.DAYINXH, itemmx.DINGXING,
                                itemmx.FANWEI, itemmx.DANWEI, zzsqdbh), tran);
                        }
                    }
                }
                #endregion
                #region 检查信息
                foreach (var item in InObject.JIANCHAMX)
                {
                    /*zzsqdh,jcid,jclxmc,kdrq,xmmc,yxsj,
                     zdjg,bz,bgdz,kdys*/
                    if (string.IsNullOrEmpty(item.JIANCHAID))
                    {
                        throw new Exception(string.Format("检查ID不能为空！"));
                    }
                    DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00012,
                    zzsqdbh, item.JIANCHAID, item.JIANCHALX, item.KAIDANRQ, item.XIANGMUMC, item.YIXIANGSJ,
                    item.ZHENDUAMJG, item.BEIZHU, item.BAOGAODZ, item.KAIDANYS), tran);
                }
                #endregion
                #region//住院医嘱信息
                foreach (var item in InObject.ZHUYUANYZMX)
                {
                    /*zzsqdh,yzid,yzlx,yzmc,yzzh,kssj,
                    jssj,ycsl,yldw,zxrq,pl,yzlb,
                    kdys,fyzid,psjg,gytj*/
                    if (string.IsNullOrEmpty(item.YIZHUXH))
                    {
                        throw new Exception(string.Format("医嘱序号不能为空！"));
                    }
                    string sql = SqlLoad.GetFormat(SQ.HIS00013,
                    zzsqdbh, item.YIZHUXH, item.YIZHULX, item.YIZHUMC, item.YIZHUZH, item.KAISHISJ,
                    item.TINGZHISJ, item.YICISL, item.YONGLIANGDW, item.ZHIXINGRQ, item.PINGLV, item.YIZHULB,
                    item.KAIDANYS, item.FUYIZXH, item.PISHIJG, item.GEIYAOTJ);
                    DBVisitor.ExecuteNonQuery(sql, tran);
                }
                #endregion
                tran.Commit();
                OutObject = new JIESHOUSZSQ_OUT();
                OutObject.ZHUANZHENDH = zzsqdbh;
               

            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }


        } 
    }
}
