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
    public class TIJIANJLCX : IMessage<TIJIANJLCX_IN, TIJIANJLCX_OUT>
    {
        public override void ProcessMessage()
        {

            OutObject = new TIJIANJLCX_OUT();
            string zhengJianHM = InObject.ZHENGJIANHM;//证件号码
            string danWeiBM = InObject.DANWEIBM;//单位编码
            string danWeiTiJianDBM = InObject.DANWEITJDBM;//单位体检单编码

            #region 基础入参判断

            if (!string.IsNullOrEmpty(zhengJianHM) || (!string.IsNullOrEmpty(danWeiBM) && !string.IsNullOrEmpty(danWeiTiJianDBM)))
            {
                if (string.IsNullOrEmpty(zhengJianHM))
                {
                    zhengJianHM = string.Empty;
                }
                else
                {
                    zhengJianHM = InObject.ZHENGJIANHM.ToUpper();
                }
                
                if (string.IsNullOrEmpty(danWeiBM))
                {
                    danWeiBM = string.Empty;
                }

                if (string.IsNullOrEmpty(danWeiTiJianDBM))
                {
                    danWeiTiJianDBM = string.Empty;
                }
                
            }
            else {
                throw new Exception("请传入正确的证件号码或单位体检信息！");
            }

            #endregion

            string tiJianXXSql = "select * from tj_dengjixx_view where zhengjianbm = '{0}' or ( nvl(danweibm,'*') = '{1}' and nvl(danweitjdbm,'*') = '{2}' )";

            DataTable dtTiJianXX = DBVisitorTiJian.ExecuteTable(string.Format(tiJianXXSql, zhengJianHM, danWeiBM, danWeiTiJianDBM));

            if (dtTiJianXX == null || dtTiJianXX.Rows.Count <= 0)
            {

                throw new Exception("未找到相关的体检信息！");
            }
            else {
                for (int i = 0; i < dtTiJianXX.Rows.Count; i++) {
                    TIJIANXX tjxx = new TIJIANXX();
                    tjxx.TIJIANBM = dtTiJianXX.Rows[i]["tijianbm"].ToString();//体检编码 唯一号
                    tjxx.ZHENGJIANHM = dtTiJianXX.Rows[i]["zhengjianbm"].ToString();//诊间号码
                    tjxx.XINGMING = dtTiJianXX.Rows[i]["xingming"].ToString();//姓名
                    tjxx.CHUSHENGRQ = dtTiJianXX.Rows[i]["chushengrq"].ToString();//出生日期
                    tjxx.GONGZUODW = dtTiJianXX.Rows[i]["gongzuodw"].ToString();//工作单位
                    tjxx.TIJIANRQ = dtTiJianXX.Rows[i]["tijianrq"].ToString();//体检日期
                    tjxx.CAOZUOYBM = dtTiJianXX.Rows[i]["caozuoybm"].ToString();//操作员编码
                    tjxx.CAOZUOYMC = dtTiJianXX.Rows[i]["caozuoymc"].ToString();//操作员名称
                    tjxx.CAOZUORQ = dtTiJianXX.Rows[i]["caozuorq"].ToString();//操作日期
                    tjxx.TIJIANLBBM = dtTiJianXX.Rows[i]["tijianlbbm"].ToString();//体检类别编码
                    tjxx.TIJIANLBMC = dtTiJianXX.Rows[i]["tijianlbmc"].ToString();//体检类别名称
                    tjxx.TIJIANDZT = dtTiJianXX.Rows[i]["tijiandzt"].ToString();//体检单状态  0:登记 1：科室结束 2：总检结束 3：审核结束 4：报告已发 5：结算结束 6:发放报告
                    tjxx.TIJIANKSRQ = dtTiJianXX.Rows[i]["tijianksrq"].ToString();//体检开始日期
                    tjxx.TIJIANJSRQ = dtTiJianXX.Rows[i]["tijianjsrq"].ToString();//体检结束日期
                    tjxx.DANWEITJDBM = dtTiJianXX.Rows[i]["danweitjdbm"].ToString();//单位体检单代码
                    tjxx.DANWEIBM = dtTiJianXX.Rows[i]["danweibm"].ToString();//单位编码
                    tjxx.JIESUANZT = dtTiJianXX.Rows[i]["jszt"].ToString();//结算状态  
                    tjxx.GUIDANGBZ = dtTiJianXX.Rows[i]["guidangbz"].ToString();//归档标识
                    tjxx.GUIDANGRQ = dtTiJianXX.Rows[i]["guidangrq"].ToString();//归档日期
                    tjxx.TAOCANMC = dtTiJianXX.Rows[i]["taocanmc"].ToString();//套餐名称
                    OutObject.TIJIANMX.Add(tjxx);
                }
            
            }
        }
    }
}
