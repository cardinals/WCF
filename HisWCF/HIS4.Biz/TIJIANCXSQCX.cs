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
using Common.Alipay;
using System.Net;
using System.IO;
using System.Xml;
using SWSoft.Caller.Framework;

namespace HIS4.Biz
{
    /// <summary>
    /// 体检记录查询
    /// </summary>
    public class TIJIANCXSQCX : IMessage<TIJIANCXSQCX_IN, TIJIANCXSQCX_OUT>
    {
        public override void ProcessMessage()
        {

            OutObject = new TIJIANCXSQCX_OUT();
            string zhengJianHM = InObject.ZHENGJIANHM.ToUpper();//证件号码
            string tiJianBM = InObject.TIJIANBM;//体检编码
            string shengQingLX = InObject.SHENQINGLX;//申请类型

            #region 基础入参判断

            if (string.IsNullOrEmpty(zhengJianHM)) {
                throw new Exception("证件号码不能为空！");
            }
            if (string.IsNullOrEmpty(tiJianBM))
            {
                throw new Exception("体检编码不能为空！");
            }
            if (string.IsNullOrEmpty(shengQingLX))
            {
                shengQingLX = "1";
            }
            #endregion

            #region 检查是否已经提交申请
            string tiJianChaXunSQSql = "select * from TJ_JK_SHENQINGDAN_view  where tijianbm = '{0}' and ZHENGJIANBM = '{1}' and shenqingdlx = '{2}' ";
            DataTable dtTiJianChaXunSQ = DBVisitorTiJian.ExecuteTable(string.Format(tiJianChaXunSQSql, tiJianBM, zhengJianHM,shengQingLX));
            if (dtTiJianChaXunSQ != null && dtTiJianChaXunSQ.Rows.Count <= 0) {
                throw new Exception("未找到该体检的操作申请！");
            }
            #endregion

            OutObject.SHENQINGDID = dtTiJianChaXunSQ.Rows[0]["shenqingdid"].ToString();
            OutObject.SHENQINGZT = dtTiJianChaXunSQ.Rows[0]["zhuangtai"].ToString();

           
        }
    }
}
