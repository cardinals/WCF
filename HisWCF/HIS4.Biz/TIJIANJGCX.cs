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
    public class TIJIANJGCX : IMessage<TIJIANJGCX_IN, TIJIANJGCX_OUT>
    {
        public override void ProcessMessage()
        {
            OutObject = new TIJIANJGCX_OUT();
            string tiJianBM = InObject.TIJIANBM;//体检编码

            string zhengJianHM = InObject.ZHENGJIANHM.ToUpper();//证件号码
            string danWeiBM = InObject.DANWEIBM;//单位编码
            string danWeiTiJianDBM = InObject.DANWEITJDBM;//单位体检单编码

            #region 基本入参判断
            if (string.IsNullOrEmpty(tiJianBM))
            {
                throw new Exception("体检编码不能为空！");
            }
            #endregion

            #region 报告状态确认
            string tjbgdzt = ConfigurationManager.AppSettings["TJBGFFZT"].ToString();
            if (string.IsNullOrEmpty(tjbgdzt)) {
                tjbgdzt = "6";
            }
            string tiJianBaoGaoZTSql = "select * from tj_dengjixx_view where tijianbm = '{0}' and tijiandzt in ({1})";
            DataTable dtTiJianBaoGaoZT = DBVisitorTiJian.ExecuteTable(string.Format(tiJianBaoGaoZTSql, tiJianBM,tjbgdzt));
            if (dtTiJianBaoGaoZT == null || dtTiJianBaoGaoZT.Rows.Count <= 0) {
                throw new Exception("该体检还未出报告！");
            }
            #endregion

            #region 信息查询授权判断
            if (ConfigurationManager.AppSettings["TJBGRGSHBZ"] == "1")
            {
                string tiJianChaXunSQSql = "select * from TJ_JK_SHENQINGDAN_view  where tijianbm = '{0}' and ZHENGJIANBM = '{1}'  and shenqingdlx = '1' and zhuangtai = 1 ";
                DataTable dtTiJianChaXunSQ = DBVisitorTiJian.ExecuteTable(string.Format(tiJianChaXunSQSql, tiJianBM, zhengJianHM));
                if (dtTiJianChaXunSQ != null && dtTiJianChaXunSQ.Rows.Count <= 0)
                {
                    throw new Exception("该体检单已提交了查询申请，还未审核通过，请等待审核！");
                }
            }
            #endregion

            #region 体检结果数据检索

            TIJIANJGXX tjjgxx = new TIJIANJGXX();
            tjjgxx.TIJIANBM = tiJianBM;//体检编码  唯一号

            #region 总检信息
            string zongJianXXSql = "select * from Tj_Gerenzjd_view where tijianbm = '{0}' ";
            DataTable dtZongJianXX = DBVisitorTiJian.ExecuteTable(string.Format(zongJianXXSql, tjjgxx.TIJIANBM));
            if (dtZongJianXX != null && dtZongJianXX.Rows.Count > 0) {
                tjjgxx.ZONGJIANXX.TIJIANZD = dtZongJianXX.Rows[0]["TIJIANZD"].ToString();
                tjjgxx.ZONGJIANXX.ZONGJIANJY = dtZongJianXX.Rows[0]["ZONGJIANJY"].ToString();
                tjjgxx.ZONGJIANXX.ZONGJIANXJ = dtZongJianXX.Rows[0]["ZONGJIANXJ"].ToString();
            }
            #endregion

            #region 体检组合信息
            string tiJianZuHeXXSql = "select distinct nvl((select zuhexmmc from tj_zd_zuhexm where zuhexmbm = a.zuhexmbm),a.zuhexmbm) as zuhexmbm from tj_tijiandtjxm_view a  where tijianbm ='{0}'";
            DataTable dtTiJianZuHeXX = DBVisitorTiJian.ExecuteTable(string.Format(tiJianZuHeXXSql, tjjgxx.TIJIANBM));
            if (dtTiJianZuHeXX != null && dtTiJianZuHeXX.Rows.Count > 0) {
                for (int i = 0; i < dtTiJianZuHeXX.Rows.Count; i++) {
                    TIJIANXMXX tjxmxx = new TIJIANXMXX();
                    tjxmxx.XIANGMUZHMC = dtTiJianZuHeXX.Rows[i]["zuhexmbm"].ToString();
                    string tiJianJieGuoMXXXSql = "select * from tj_tijiandtjxm_view where tijianbm ='{0}' and zuhexmbm ='{1}' ";
                    DataTable dtTiJianJieGuoMXXX = DBVisitorTiJian.ExecuteTable(string.Format(tiJianJieGuoMXXXSql, tjjgxx.TIJIANBM, tjxmxx.XIANGMUZHMC));
                    if (dtTiJianJieGuoMXXX != null && dtTiJianJieGuoMXXX.Rows.Count > 0) {
                        for (int j = 0; j < dtTiJianJieGuoMXXX.Rows.Count; j++)
                        {
                            TIJIANXMJGXX tjxmjgxx = new TIJIANXMJGXX();
                            tjxmjgxx.XIANGMUBM = dtTiJianJieGuoMXXX.Rows[j]["xiangmubm"].ToString();
                            tjxmjgxx.XIANGMUMC = dtTiJianJieGuoMXXX.Rows[j]["xiangmumc"].ToString();
                            tjxmjgxx.JILIANGDW = dtTiJianJieGuoMXXX.Rows[j]["jiliangdwbm"].ToString();
                            tjxmjgxx.ZHIXINGKS = dtTiJianJieGuoMXXX.Rows[j]["zhixingksbm"].ToString();
                            tjxmjgxx.CAOZUORBM = dtTiJianJieGuoMXXX.Rows[j]["caozuorbm"].ToString();
                            tjxmjgxx.CAOZUORQ = dtTiJianJieGuoMXXX.Rows[j]["caozuorq"].ToString();
                            tjxmjgxx.TIJIANJG = dtTiJianJieGuoMXXX.Rows[j]["tijianjg"].ToString();
                            tjxmjgxx.ZHENGCHANGJG = dtTiJianJieGuoMXXX.Rows[j]["zhengchangjg"].ToString();
                            tjxmjgxx.CANKAOXX = dtTiJianJieGuoMXXX.Rows[j]["cankaoxx"].ToString();
                            tjxmjgxx.CANKAOSX = dtTiJianJieGuoMXXX.Rows[j]["cankaosx"].ToString();
                            tjxmjgxx.JIELUN = dtTiJianJieGuoMXXX.Rows[j]["jielun"].ToString();
                            tjxmjgxx.SHENHEY = dtTiJianJieGuoMXXX.Rows[j]["shenhey"].ToString();
                            tjxmjgxx.SHENHESJ = dtTiJianJieGuoMXXX.Rows[j]["shenhesj"].ToString();
                            tjxmjgxx.YANGBENH = dtTiJianJieGuoMXXX.Rows[j]["yangbenh"].ToString();
                            tjxmxx.TIJIANXMJGMX.Add(tjxmjgxx);
                        }
                    }
                    tjjgxx.TIJIANXMMX.Add(tjxmxx);
                }
            }

            OutObject.TIJIANJGMX.Add(tjjgxx);
            #endregion
            #endregion

            if (tjjgxx.ZONGJIANXX == null
                || (string.IsNullOrEmpty(tjjgxx.ZONGJIANXX.ZONGJIANJY) && string.IsNullOrEmpty(tjjgxx.ZONGJIANXX.TIJIANZD) && string.IsNullOrEmpty(tjjgxx.ZONGJIANXX.ZONGJIANXJ))
                || tjjgxx.TIJIANXMMX.Count == 0) {
                    throw new Exception("未能获取完整的体检结果信息！");
            }
        }
    }
}
