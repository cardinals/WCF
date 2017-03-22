using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data.SqlClient;
using System.Data.Common;
using HIS4.Schemas;
using System.Data.OracleClient;

namespace HIS4.Biz
{
    public class ZHANGHUKT : IMessage<ZHANGHUKT_IN,ZHANGHUKT_OUT>
    {
        public override void ProcessMessage()
        {
            string jiuzhenKh = InObject.JIUZHENKH;
            string jiuzhenkLx = InObject.JIUZHENKLX;
            string xingMing = InObject.XINGMING;
            string zhengjianHm = InObject.ZHENGJIANHM;
            string bingRenID = InObject.BINGRENID;
            string lianxiDh = InObject.LIANXIDH;
            string jiandangRen = InObject.BASEINFO.CAOZUOYDM;//建档人工号         
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;//操作日期

            #region 基本入参判断
            //就诊卡类型
            //if (string.IsNullOrEmpty(jiuzhenkLx))
            //{
            //    throw new Exception("就诊卡类型获取失败");
            //}
            //就诊卡号
            if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(bingRenID))
            {
                throw new Exception("就诊卡号获取失败");
            }

            if (string.IsNullOrEmpty(jiuzhenKh)) {
                DataTable dtBingRenXX = DBVisitor.ExecuteTable(string.Format("select * from gy_v_bingrenxx where bingrenid = '{0}' "),bingRenID);
                if (dtBingRenXX.Rows.Count > 0) {
                    jiuzhenKh = dtBingRenXX.Rows[0]["jiuzhenkh"].ToString();
                }
            }
            ////证件号码
            //if (string.IsNullOrEmpty(zhengjianHm))
            //{
            //    throw new Exception(证件号码获取失败");
            //}
            ////联系电话
            //if (string.IsNullOrEmpty(lianxiDh))
            //{
            //    throw new Exception(联系电话获取失败");
            //}
            //建档人
            if (string.IsNullOrEmpty(jiandangRen))
            {
                throw new Exception("建档人获取失败");
            }
            //操作日期
            if (string.IsNullOrEmpty(caozuoRq))
            {
                throw new Exception("操作日期获取失败");
            }
            #endregion

            //就诊卡号|就诊卡类型|操作日期|操作员工号       
            string jiaoyiMsg = jiuzhenKh + "||" + caozuoRq + "|" + jiandangRen + "|";
            //WcfCommon.writeLog(WcfCommon.LOGTYPE_SQLLOG, OutObject.GetType().Name.ToString(), "院内账户开通存储过程入参：" + jiaoyiMsg, messageId);

            //开通账户-------------------------------------------------------------------------------------------
            //SqlParameter param1 = new SqlParameter("PRM_MSG", jiaoyiMsg);
            //param1.SqlDbType = SqlDbType.VarChar;
            //param1.Direction = ParameterDirection.Input;
            //SqlParameter param2 = new SqlParameter("PRM_APPCODE", SqlDbType.Int);
            //param2.Direction = ParameterDirection.Output;
            //SqlParameter param3 = new SqlParameter("PRM_DATABUFFER", "");
            //param3.SqlDbType = SqlDbType.VarChar;
            //param3.Direction = ParameterDirection.Output;
            //SqlParameter[] paramJiaoYi = { param1, param2, param3 };

            //DBVisitor.CreateOracleParameter("PRM_MSG", OracleType.VarChar, jiaoyiMsg);
            //OracleParameter param2 = DBVisitor.CreateOracleParameter("PRM_APPCODE", OracleType.Number, 0, ParameterDirection.Output);
            //OracleParameter param3 = DBVisitor.CreateOracleParameter("PRM_DATABUFFER", OracleType.VarChar, "", ParameterDirection.Output);

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
                transaction = DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_KAITONGZH", paramJiaoYi, transaction);//院内账户开通
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                }
                throw new Exception(ex.Message);
            }
            //--------------------------------------------------------------------------------------------------
            string returnValue = paramJiaoYi[1].Value.ToString();
            string returnMsg = paramJiaoYi[2].Value.ToString();
            if (returnValue != "1")//
            {
                transaction.Rollback();
                conn.Close();
                throw new Exception(returnMsg);
            }
            else
            {
                transaction.Commit();
                conn.Close();
            }

        }
    }
}
