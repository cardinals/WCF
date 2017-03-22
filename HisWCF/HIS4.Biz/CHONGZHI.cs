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
    public class CHONGZHI : IMessage<CHONGZHI_IN, CHONGZHI_OUT>
    {

        public override void ProcessMessage()
        {
            OutObject = new CHONGZHI_OUT();
            string jiuzhenKh = InObject.JIUZHENKH;
            string jiuzhenkLx = InObject.JIUZHENKLX;
            string xingMing = InObject.XINGMING;
            string zhengjianHm = InObject.ZHENGJIANHM;
            string lianxiDh = InObject.LIANXIDH;
            string chongzhiJe = InObject.CHONGZHIJE;
            string zhifuFs = InObject.ZHIFUFS;
            string jiandangRen = InObject.BASEINFO.CAOZUOYDM;//建档人工号         
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;//操作日期 
            string yuanQuDM = InObject.BASEINFO.FENYUANDM;//分院代码
            string YINHANGKXX = InObject.YINGHANGKXX;
            int JIUZHENKCD = Convert.ToInt32(ConfigurationManager.AppSettings["JIUZHENKCD"]);//就诊卡默认长度

            #region 基本入参判断
            if (string.IsNullOrEmpty(yuanQuDM))
            {
                throw new Exception("院区代码不能为空！");
            }
            //就诊卡号
            if (string.IsNullOrEmpty(jiuzhenKh))
            {
                throw new Exception(string.Format("就诊卡号获取失败！"));
            }
            //支付方式
            if (string.IsNullOrEmpty(zhifuFs))
            {
                throw new Exception(string.Format("支付方式获取失败！"));
            }
            //充值金额
            if (string.IsNullOrEmpty(zhifuFs))
            {
                throw new Exception(string.Format("充值金额获取失败！"));
            }

            if (string.IsNullOrEmpty(caozuoRq) || caozuoRq.Length != 19)
            {
                caozuoRq = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
            #endregion

            if (JIUZHENKCD > 0)
            {
                if (jiuzhenKh.Length < JIUZHENKCD)
                {
                    jiuzhenKh = jiuzhenKh.PadLeft(JIUZHENKCD, '0');
                }
            }

            string brxxSql = "select bingrenid, ZIJINZHQYBZ,xingMing,nvl(shenfenzh,'') as zhengjianHm,"
                + "nvl(lianxirdh,'') as lianxiDh from gy_bingrenxx where jiuzhenkh='" + jiuzhenKh + "'";
            DataTable dtBrxx = DBVisitor.ExecuteTable(brxxSql);
            string qiyongBz = string.Empty;
            string bingrenId = string.Empty;
            string jiaoyiMa = string.Empty;//交易码
            if (dtBrxx.Rows.Count > 0)
            {
                qiyongBz = dtBrxx.Rows[0]["ZIJINZHQYBZ"].ToString();
                bingrenId = dtBrxx.Rows[0]["bingrenid"].ToString();
                zhengjianHm = dtBrxx.Rows[0]["zhengjianHm"].ToString();
                xingMing = dtBrxx.Rows[0]["xingMing"].ToString();
                lianxiDh = dtBrxx.Rows[0]["lianxiDh"].ToString();
            }



            jiaoyiMa = Unity.GetMD5(bingrenId + Convert.ToSingle(chongzhiJe).ToString("f4") + caozuoRq + "1" + jiandangRen).ToLower();
            if (qiyongBz == "1")
            {
                if (zhifuFs == "10" || zhifuFs == "15")
                {
                    DataTable dtXLH = DBVisitor.ExecuteTable("Select seq_gy_posjiaoyixx.nextval POSXH From Dual");
                    if (dtXLH.Rows.Count > 0)
                    {
                        string POSJIAOYIXXID = dtXLH.Rows[0]["POSXH"].ToString();
                        YINHANGKXX += "#" + POSJIAOYIXXID;
                    }
                }
                else if (zhifuFs == "21")
                {
                    //DataTable dtXLH = DBVisitor.ExecuteTable("Select S_ZFB_JYLSH.nextval ZFBXH From Dual");
                    //if (dtXLH.Rows.Count > 0)
                    //{
                    string ZFBJIAOYIXXID = "808";//dtXLH.Rows[0]["ZFBXH"].ToString();
                        YINHANGKXX += "#" + ZFBJIAOYIXXID + "#" + InObject.BASEINFO.FENYUANDM;
                    //}
                }
                string jiaoyiMsg = jiuzhenKh + "|" + jiuzhenkLx + "|" + xingMing + "|" + zhengjianHm + "|" + lianxiDh + "|" +
                     chongzhiJe + "|" + zhifuFs + "|" + caozuoRq + "|" + jiandangRen + "|" + jiaoyiMa + "|";
                if (!string.IsNullOrEmpty(YINHANGKXX))
                {
                    jiaoyiMsg += YINHANGKXX + "|";
                }
                else
                {
                    jiaoyiMsg += "|";
                }
                jiaoyiMsg += yuanQuDM + "|";
                //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "院内账户充值存储过程入参：" + jiaoyiMsg, messageId);
                //账户充值-------------------------------------------------------------------------------------------

                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_MSG", OracleType.VarChar);
                paramJiaoYi[0].Value = jiaoyiMsg;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = null;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_OUTBUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_ZZSF_DIANZIZHCZ", paramJiaoYi, transaction);//院内账户充值
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        conn.Close();
                    }
                    throw new Exception(string.Format(ex.Message));
                }
                //--------------------------------------------------------------------------------------------------
                string returnValue = paramJiaoYi[1].Value.ToString();
                string returnMsg = paramJiaoYi[2].Value.ToString();
                if (returnValue == "1")//
                {
                    transaction.Commit();
                    conn.Close();
                    OutObject.ZHANGHUYE = returnMsg;
                }
                else
                {
                    transaction.Rollback();
                    conn.Close();
                    throw new Exception(returnMsg);
                }
            }
            else
            {
                throw new Exception("请先开通院内账户");
            }
        }
    }
}
