using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using HIS4.Schemas;
using SWSoft.Framework;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using log4net;
using System.Configuration;

namespace HIS4.Biz
{
    public class GUAHAOYYTH :IMessage<GUAHAOYYTH_IN,GUAHAOYYTH_OUT>
    {
        ILog log = log4net.LogManager.GetLogger("SqlInfo");
        public override void ProcessMessage()
        {
            OutObject = new GUAHAOYYTH_OUT();
            string quhaoMm = InObject.QUHAOMM;//取号密码即预约号？？？？         

            #region 基本入参判断

            //if (string.IsNullOrEmpty(jiuzhenKh) && string.IsNullOrEmpty(zhengjianHm)) {
            //    throw new Exception("就诊卡号和证件号码不能同时为空！");
            //}

            if (string.IsNullOrEmpty(quhaoMm))
            {
                throw new Exception("取号密码不能为空");
            }
            #endregion

            string yuYueCZFS = ConfigurationManager.AppSettings["GUAHAOYYCZFS"];//挂号预约处理方式
            if (!string.IsNullOrEmpty(yuYueCZFS) && yuYueCZFS == "1")
            {
                #region 嘉兴悦城妇儿业务流程
                string yuYueJL = @"select * from gy_jiuzhenyy where jiuzhenyyid = '{0}'";
                DataTable dtYuYueJL = DBVisitor.ExecuteTable(string.Format(yuYueJL, quhaoMm));
                if (dtYuYueJL == null || dtYuYueJL.Rows.Count <= 0) {
                    throw new Exception("未找到预约记录！");
                }

                string jiuZhenBZ = dtYuYueJL.Rows[0]["jiuzhenbz"].ToString();
                if (string.IsNullOrEmpty(jiuZhenBZ)) {
                    jiuZhenBZ = "0";
                }
                if (jiuZhenBZ != "0") {
                    throw new Exception("该病人已就诊，无法取消预约！");
                }

                string quXiaoYY = "update gy_jiuzhenyy set zuofeibz = 1 where jiuzhenyyid = '{0}'";
                DBVisitor.ExecuteNonQuery(string.Format(quXiaoYY, quhaoMm));

                #endregion
            }
            else
            {
                #region 标准his4版本业务流程
                
               

                OracleParameter[] paramJiaoYi = new OracleParameter[3];
                paramJiaoYi[0] = new OracleParameter("PRM_YUYUEHAO", OracleType.VarChar);
                paramJiaoYi[0].Value = quhaoMm;
                paramJiaoYi[0].Direction = ParameterDirection.Input;
                paramJiaoYi[1] = new OracleParameter("PRM_APPCODE", OracleType.Number);
                paramJiaoYi[1].Value = null;
                paramJiaoYi[1].Direction = ParameterDirection.Output;
                paramJiaoYi[2] = new OracleParameter("PRM_DATABUFFER", OracleType.VarChar);
                paramJiaoYi[2].Value = null;
                paramJiaoYi[2].Size = 2000;
                paramJiaoYi[2].Direction = ParameterDirection.Output;

                log.InfoFormat("{0}", "执行存储过程：PKG_MZ_YUYUE.PRC_GY_QUXIAOYY \r\nPRM_YUYUEHAO：" + quhaoMm + "\r\n");

                DbTransaction transaction = null;
                DbConnection conn = DBVisitor.Connection;
                try
                {
                    transaction = conn.BeginTransaction();
                    DBVisitor.ExecuteProcedure("PKG_MZ_YUYUE.PRC_GY_QUXIAOYY", paramJiaoYi, transaction);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conn.Close();
                    throw new Exception(ex.Message);
                }
                //--------------------------------------------------------------------------------------------------
                string returnValue = paramJiaoYi[2].Value.ToString();
                if (returnValue.Length > 2 && returnValue.Substring(0, 2).ToUpper() == "OK")//取消预约成功
                {
                    transaction.Commit();
                    conn.Close();
                    OutObject.OUTMSG.ERRMSG = "预约取消成功！";
                    //tradeOut = WcfCommon.GetXmlString(tradeHead, tradeType, tradeDetail, new DataTable(), 0, "挂号预约退号成功");
                    //return 0;
                }
                else
                {
                    transaction.Rollback();
                    conn.Close();
                    throw new Exception(returnValue.Substring(returnValue.IndexOf('|') + 1));
                }

                #endregion
            }
        }
    }
}
