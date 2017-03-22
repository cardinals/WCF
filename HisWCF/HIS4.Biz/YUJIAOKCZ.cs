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
    public class YUJIAOKCZ : IMessage<YUJIAOKCZ_IN, YUJIAOKCZ_OUT>
    {

        public override void ProcessMessage()
        {
            string bingrenId = InObject.BINGRENID;//病人ID
            string zhifuLx = InObject.ZHIFUMX[0].ZHIFULX;//支付类型
            string zhifuSm =  InObject.ZHIFUMX[0].ZHIFUSM;//支付说明
            string zhifuJe =InObject.ZHIFUMX[0].ZHIFUJE;;//支付金额
            string yinhangKh = InObject.ZHIFUMX[0].YINHANGKH;//银行卡号
            string yinhangMm = InObject.ZHIFUMX[0].YINHANGMM;//银行密码
            string zhongduanBh = InObject.ZHIFUMX[0].ZHONGDUANBH;//终端编号
            string ShanghuH = InObject.ZHIFUMX[0].SHANGHUH;//商户号
            string yinhangkXx = InObject.ZHIFUMX[0].YINHANGKXX;//银行卡信息
            string piaojuHm = InObject.ZHIFUMX[0].PIAOJUHM;//票据号码
            string shoukuanRen = InObject.BASEINFO.CAOZUOYDM;//收款人
            string shoukuanRq = InObject.BASEINFO.CAOZUORQ;//收款日期

            #region 基本入参判断
            if (string.IsNullOrEmpty(bingrenId))
            {
                throw new Exception("病人唯一号不能为空");
            }

            if (string.IsNullOrEmpty(zhifuLx)) {
                throw new Exception("支付类型不能为空");
            }

            if (string.IsNullOrEmpty(zhifuJe)) {
                throw new Exception("支付金额不能为空");
            }

            try {
                zhifuJe = Convert.ToDecimal(zhifuJe).ToString();
            }
            catch (Exception ex){
                throw new Exception(ex.Message);
            }
            #endregion
            //通过bingrenID获取病人住院ID
            string brSql = "select bingrenzyid from zy_bingrenxx where bingrenid='{0}' and zaiyuanzt in('0','1') ";
            string bingrenZyId = (string)DBVisitor.ExecuteScalar (string.Format(brSql,bingrenId));
            //拼装交易字符串
            string jiaoyiZfc = bingrenZyId + "|" + shoukuanRen + "|" + zhifuJe + "|" + zhifuLx + "|"//病人住院ID|收款人|交款金额|支付方式
                  + shoukuanRq + "|" + zhongduanBh + "|" + yinhangKh + "|" + "" + "|" + "0" + "|";//交易日期|银行交易流水号|卡号|交易类型|重发标志           


            OracleParameter[] paramJiaoYi = new OracleParameter[3];
            paramJiaoYi[0] = new OracleParameter("PRM_MSG", OracleType.VarChar);
            paramJiaoYi[0].Value = jiaoyiZfc;
            paramJiaoYi[0].Direction = ParameterDirection.Input;
            paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
            paramJiaoYi[1].Value = null;
            paramJiaoYi[1].Direction = ParameterDirection.Output;
            paramJiaoYi[2] = new OracleParameter("PRM_DATABUFFER", OracleType.VarChar);
            paramJiaoYi[2].Value = null;
            paramJiaoYi[2].Size = 2000;
            paramJiaoYi[2].Direction = ParameterDirection.Output;

            string returnValue = string.Empty;
            DbTransaction transaction = null;
            DbConnection conn = DBVisitor.Connection;
            try
            {
                transaction = conn.BeginTransaction();
                DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_ZY_YuJiaoKUAN", paramJiaoYi,transaction);
            }
            catch (Exception ex)
            {
                if (transaction != null) {
                    transaction.Rollback();
                    conn.Close();
                }
                throw new Exception(ex.Message);
            }
            //--------------------------------------------------------------------------------------------------  
            returnValue = paramJiaoYi[1].Value.ToString();
            string returnMsg = paramJiaoYi[2].Value.ToString();
            if (returnValue != "1")
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                }
                throw new Exception(returnMsg);
            }
            else { 
            //交易成功
                if (transaction != null)
                {
                    transaction.Commit();
                    conn.Close();
                }
            }
        }
    }
}
