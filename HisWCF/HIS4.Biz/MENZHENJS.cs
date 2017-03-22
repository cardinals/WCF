using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using HIS4.Schemas;
using log4net;

namespace HIS4.Biz
{
    public class MENZHENJS : IMessage<MENZHENJS_IN,MENZHENJS_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {
            string jiuzhenkLx = InObject.JIUZHENKLX;//就诊卡类型
            string jiuzhenKh = InObject.JIUZHENKH;//就诊卡号
            string bingrenLb = InObject.BINGRENLB;//病人类别
            string bingrenXz = InObject.BINGRENXZ;//病人性质
            string yibaokLx = InObject.YIBAOKLX;//医保卡类型
            string yibaokMm = InObject.YIBAOKMM;//医保卡密码
            string yibaokXx = InObject.YIBAOKXX;//医保卡信息
            string yibaobrXx = InObject.YIBAOBRXX;//医保病人信息
            string yiliaoLb = InObject.YILIAOLB;//医疗类别
            string jiesuanLb = InObject.JIESUANLB;//结算类别
            string jiesuanId = InObject.JIESUANID;//结算ID
            string hisbrXx = InObject.HISBRXX;//his病人信息
            //string zhifuLx = BaseCommon.GetNoteValue(tradeMsg, tradeType, "ZHIFULX");
            string zhifuJe = InObject.ZONGJE;//总金额
            //string yinhangkXx = BaseCommon.GetNoteValue(tradeMsg, tradeType, "YINHANGKXX");
            string caozuoyDm = InObject.BASEINFO.CAOZUOYDM;
            string caozuoyXm = InObject.BASEINFO.CAOZUOYXM;
            string caozuoRq = InObject.BASEINFO.CAOZUORQ;
            string jiaoyiLsh = InObject.BASEINFO.ZHONGDUANLSH;//终端流水号即交易流水号？？ 

            #region 基础入参判断
 
            //就诊卡号
            if (string.IsNullOrEmpty(jiuzhenKh))
            {
                throw new Exception("就诊卡号获取失败");
            }

            //就诊卡类型
            if (string.IsNullOrEmpty(jiuzhenkLx))
            {
                throw new Exception("就诊卡类型获取失败");
            }

            //病人类别
            if (string.IsNullOrEmpty(bingrenLb))
            {
                throw new Exception("病人类别获取失败");
            }

            //病人性质
            if (string.IsNullOrEmpty(bingrenXz))
            {
                throw new Exception("病人性质获取失败");
            }
            #endregion

            string bingrenId = DBVisitor.ExecuteScalar(string.Format("select bingrenid from gy_bingrenxx where jiuzhenkh='{0}'", jiuzhenKh)).ToString();
            string fapiaoId = string.Empty;
            string zhifuLx = string.Empty;
            string zhifuFs = "1";//支付方式？
            string jiesuanIds = jiesuanId;//结算单字符串？
            string yhjiaoyiLsh = string.Empty;
            string yibaojsId = string.Empty;
            string yinhangkXx = string.Empty;
            string jiaoyiMsg = bingrenId + "|" + caozuoyDm + "|" + zhifuFs + "|"
               + jiesuanIds + "|" + zhifuJe + "|" + jiaoyiLsh + "|" + fapiaoId + "|"
               + caozuoRq + "|" + jiuzhenKh + "|" + zhifuLx + "|" + yinhangkXx + "|" + yibaojsId + "|";

            //门诊收费-------------------------------------------------------------------------------------------

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

            log.InfoFormat("{0}", "执行存储过程：PKG_GY_YINYIJK.PRC_SHOUFEIJS \r\nPRM_MSG：" + jiaoyiMsg + "\r\n");

            DbTransaction transaction = null;
            DbConnection conn = DBVisitor.Connection;
            try
            {
                transaction = conn.BeginTransaction();
                DBVisitor.ExecuteProcedure("PKG_GY_YINYIJK.PRC_SHOUFEIJS", paramJiaoYi, transaction);     
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();//回滚
                    conn.Close();
                }
                throw new Exception(ex.Message);
            }
            //--------------------------------------------------------------------------------------
            string returnValue = paramJiaoYi[1].Value.ToString();
            string returnMsg = paramJiaoYi[2].Value.ToString();
            //LogHelper.WriteLog(typeof(GG_JiaoYiBLL), "门诊收费存储过程返回值：" + returnValue + "|" + returnMsg);
            if (returnValue == "1")//交易成功
            {
                transaction.Commit();//提交
                conn.Close();
                string[] list = returnMsg.Split('|');
                DataTable dtTarget = new DataTable();
                dtTarget.Columns.Add("SHIFUJE", Type.GetType("System.String"));
                dtTarget.Columns.Add("TIAOXINGM", Type.GetType("System.String"));
                dtTarget.Columns.Add("QUYAOCK", Type.GetType("System.String"));
                dtTarget.Columns.Add("BINGRENID", Type.GetType("System.String"));
                DataRow nRow = dtTarget.NewRow();
                nRow["SHIFUJE"] = list[0]; ;
                nRow["TIAOXINGM"] = list[1];
                nRow["QUYAOCK"] = list[2];
                nRow["BINGRENID"] = bingrenId;
                dtTarget.Rows.Add(nRow);
            }
            else
            {
                transaction.Rollback();//回滚
                conn.Close();
                throw new Exception(returnMsg);
            }
        }
    }
}
