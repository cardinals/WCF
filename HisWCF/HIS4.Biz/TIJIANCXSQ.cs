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
    public class TIJIANCXSQ : IMessage<TIJIANCXSQ_IN, TIJIANCXSQ_OUT>
    {
        public override void ProcessMessage()
        {

            OutObject = new TIJIANCXSQ_OUT();
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

            #region 报告状态确认
            string tjbgdzt = ConfigurationManager.AppSettings["TJBGFFZT"].ToString();
            if (string.IsNullOrEmpty(tjbgdzt))
            {
                tjbgdzt = "6";
            }
            string tiJianBaoGaoZTSql = "select * from tj_dengjixx_view where tijianbm = '{0}' and tijiandzt in ({1})";
            DataTable dtTiJianBaoGaoZT = DBVisitorTiJian.ExecuteTable(string.Format(tiJianBaoGaoZTSql, tiJianBM, tjbgdzt,zhengJianHM));
            if (dtTiJianBaoGaoZT == null || dtTiJianBaoGaoZT.Rows.Count <= 0)
            {
                throw new Exception("该体检还未出报告！");
            }
            #endregion

            #region 检查是否已经提交申请
            string tiJianChaXunSQSql = "select * from TJ_JK_SHENQINGDAN_view  where tijianbm = '{0}' and ZHENGJIANBM = '{1}'  and shenqingdlx = '{2}' ";
            DataTable dtTiJianChaXunSQ = DBVisitorTiJian.ExecuteTable(string.Format(tiJianChaXunSQSql, tiJianBM, zhengJianHM ,shengQingLX));
            if (dtTiJianChaXunSQ != null && dtTiJianChaXunSQ.Rows.Count > 0) {
                throw new Exception("该体检单已提交了查询申请，请等待审核！");
            }
            #endregion

            #region 写入申请信息
            string shenQingDID = DBVisitorTiJian.ExecuteScalar("select JK_SHENQINGDAN.Nextval from dual").ToString();
            string tiJiaoTiJianSQSql = "insert into tj_jk_shenqingdan(shenqingdid,shenqingdlx,zhengjianbm,tijianbm,zhuangtai,shengqingren,shengqingrq) values ('{0}','{1}','{2}','{3}',0,'{4}',sysdate)";
            DBVisitorTiJian.ExecuteNonQuery(string.Format(tiJiaoTiJianSQSql, shenQingDID, shengQingLX, zhengJianHM, tiJianBM, InObject.BASEINFO.CAOZUOYDM));
            #endregion
        }
    }
}
