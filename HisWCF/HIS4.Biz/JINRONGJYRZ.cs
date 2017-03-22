using HIS4.Schemas;
using JYCS.Schemas;
using SWSoft.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS4.Biz
{
    /// <summary>
    /// 金融交易日志
    /// </summary>
    public class JINRONGJYRZ : IMessage<JINRONGJYRZ_IN, JINRONGJYRZ_OUT>
    {
        public override void ProcessMessage()
        {
            string jiuZhenKH = InObject.JIUZHENKH;//就诊卡号
            string bingAnHao = InObject.BINGANHAO;//病案号
            string caoZuoGH = InObject.BASEINFO.CAOZUOYDM;//操作员工号
            string jiaoYiSJ = InObject.BASEINFO.CAOZUORQ;//交易时间
            string jiaoYiFS = InObject.JIAOYIFS;//交易方式
            string jiaoYiLX = InObject.JIAOYILX;//交易类型
            string shangHuH = InObject.SHANGHUH;//商户号
            string zhongDuanH = InObject.ZHONGDUANH;//终端号
            string yinHangKH = InObject.YINHANGKH; //银行卡号
            string jiaoYiPCH = InObject.JIAOYIPCH;//交易批次号
            string jiaoYiLSH = InObject.JIAOYILSH;//交易流水号
            string jiaoYiCKH = InObject.JIAOYICKH;//交易参考号
            string dingDanH = InObject.DINGDANH;//订单号
            string jiaoYiJE = InObject.JIAOYIJE;//交易金额
            decimal zhiBi100 = Convert.ToDecimal(InObject.ZHIBIZS100);//100元纸币张数
            decimal zhiBi50 = Convert.ToDecimal(InObject.ZHIBIZS50);//50元纸币张数
            decimal zhiBi20 = Convert.ToDecimal(InObject.ZHIBIZS20);//20元纸币张数
            decimal zhiBi10 = Convert.ToDecimal(InObject.ZHIBIZS10);//10元纸币张数
            decimal zhiBi5 = Convert.ToDecimal(InObject.ZHIBIZS5);//5元纸币张数
            decimal zhiBi1 = Convert.ToDecimal(InObject.ZHIBIZS1);//1元纸币张数
            string jiaoYiZT = InObject.JIAOYIZT;//交易状态
            string GUANLIANJYID = InObject.GUANLIANJYID;//关联交易ID

            OutObject = new JINRONGJYRZ_OUT();

            #region 基础入参判断
            if (string.IsNullOrEmpty(jiaoYiZT))
            {
                jiaoYiZT = "1";
            }

            if (string.IsNullOrEmpty(jiuZhenKH) && string.IsNullOrEmpty(bingAnHao))
            {
                throw new Exception("就诊卡号和病案号不能同时为空！");
            }

            if (string.IsNullOrEmpty(caoZuoGH))
            {
                throw new Exception("操作工号不能为空！");
            }

            if (string.IsNullOrEmpty(jiaoYiSJ))
            {
                throw new Exception("交易时间不能为空！");
            }

            if (string.IsNullOrEmpty(jiaoYiFS))
            {
                throw new Exception("交易方式不能为空！");
            }

            if (string.IsNullOrEmpty(jiaoYiLX))
            {
                throw new Exception("交易类型不能为空！");
            }
            else if (true)
            {//交易类型和法值判断

            }

            if (string.IsNullOrEmpty(jiaoYiJE))
            {
                throw new Exception("交易金额不能为空！");
            }


            switch (jiaoYiLX)
            {
                case "1"://现金

                    if (zhiBi100 == 0 && zhiBi50 == 0 && zhiBi20 == 0 && zhiBi10 == 0 && zhiBi5 == 0 && zhiBi1 == 0)
                    {
                        throw new Exception("投币数量不能为0！");
                    }
                    break;
                case "10":
                case "4"://银行卡
                    if (string.IsNullOrEmpty(yinHangKH))
                    {
                        throw new Exception("银行卡号不能为空！");
                    }
                    if (string.IsNullOrEmpty(jiaoYiLSH))
                    {
                        throw new Exception("交易流水号不能为空！");
                    }
                    if (string.IsNullOrEmpty(jiaoYiCKH))
                    {
                        throw new Exception("交易参考号不能为空！");
                    }
                    break;
                case "23"://智慧医疗

                    if (string.IsNullOrEmpty(jiaoYiLSH))
                    {
                        throw new Exception("交易流水号不能为空！");
                    }
                    if (string.IsNullOrEmpty(jiaoYiCKH))
                    {
                        throw new Exception("交易参考号不能为空！");
                    }
                    break;
                case "16"://支付宝

                    if (string.IsNullOrEmpty(shangHuH))
                    {
                        throw new Exception("商户号不能为空！");
                    }
                    if (string.IsNullOrEmpty(dingDanH))
                    {
                        throw new Exception("订单号不能为空！");
                    }
                    break;
                case "17"://微信

                    if (string.IsNullOrEmpty(shangHuH))
                    {
                        throw new Exception("商户号不能为空！");
                    }
                    if (string.IsNullOrEmpty(dingDanH))
                    {
                        throw new Exception("订单号不能为空！");
                    }
                    break;
            }
            #endregion


            #region sql语句准备，数据初始  sqi-291
            string jieSuanID = "";//结算id 取序列
            var maxid = DBVisitor.ExecuteModel(SqlLoad.GetFormat(SQ.HIS00023, " seq_gy_jinrongjy_zzj.nextval "));
            if (maxid == null)
            {
                throw new Exception(string.Format("结算ID获取失败！"));
            }
            else
            {
                jieSuanID = maxid.Items["MAXID"].ToString();
            }

            if (string.IsNullOrEmpty(GUANLIANJYID))
            {
                /* SQI-HIS00291 插入金融交易日志
                 * jiaoyiid,jiuzhenkh,jiaoyisj,caozuoygh,jiaoyilx,
                 * jiaoyifs,shanghuh,zhongduanh,yinhangkh,jiaoyipch,
                 * jiaoyilsh,jiaoyickh,dingdanh,jiaoyije,zhibizs100,
                 * zhibizs50,zhibizs20,zhibizs10,zhibizs5,zhibizs1,
                 * bingAnHao,jiaoYiZT
                 */
                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00024,
                    jieSuanID, jiuZhenKH, jiaoYiSJ, caoZuoGH, jiaoYiLX,
                    jiaoYiFS, shangHuH, zhongDuanH, yinHangKH, jiaoYiPCH,
                    jiaoYiLSH, jiaoYiCKH, dingDanH, jiaoYiJE, zhiBi100,
                    zhiBi50, zhiBi20, zhiBi10, zhiBi5, zhiBi1,
                    bingAnHao, jiaoYiZT));
            }
            else {
                /* SQI-HIS00291 插入金融交易日志 有关联记录
                 * jiaoyiid,jiuzhenkh,jiaoyisj,caozuoygh,jiaoyilx,
                 * jiaoyifs,shanghuh,zhongduanh,yinhangkh,jiaoyipch,
                 * jiaoyilsh,jiaoyickh,dingdanh,jiaoyije,zhibizs100,
                 * zhibizs50,zhibizs20,zhibizs10,zhibizs5,zhibizs1,
                 * bingAnHao,jiaoYiZT,guanlianjyid
                 */
                DBVisitor.ExecuteNonQuery(SqlLoad.GetFormat(SQ.HIS00025,
                    jieSuanID, jiuZhenKH, jiaoYiSJ, caoZuoGH, jiaoYiLX,
                    jiaoYiFS, shangHuH, zhongDuanH, yinHangKH, jiaoYiPCH,
                    jiaoYiLSH, jiaoYiCKH, dingDanH, jiaoYiJE, zhiBi100,
                    zhiBi50, zhiBi20, zhiBi10, zhiBi5, zhiBi1, 
                    bingAnHao, jiaoYiZT, GUANLIANJYID));

            }
            #endregion

            OutObject.JIAOYIID = jieSuanID;
           
        }
    }
}
