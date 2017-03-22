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
using System.Xml;
using System.Runtime.Remoting.Contexts;
using System.Web.Hosting;
using System.Web;

namespace HIS4.Biz
{
    public class ZHIFUBAODZ : IMessage<ZHIFUBAODZ_IN, ZHIFUBAODZ_OUT>
    {
        /// <summary>
        /// 支付宝对账
        /// </summary>
        public override void ProcessMessage()
        {
            if (string.IsNullOrEmpty(InObject.BASEINFO.FENYUANDM))
            {
                throw new Exception("分院代码不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.DZLX))
            {
                throw new Exception("对账类型不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.JIESUANZE))
            {
                throw new Exception("结算总额不能为空!");
            }
            if (string.IsNullOrEmpty(InObject.DZRQ))
            {
                throw new Exception("对账日期不能为空!");
            }

            OutObject = new ZHIFUBAODZ_OUT();
            //操作员日对账 需传入操作工号
            if (InObject.DZLX == "1")
            {
                if (string.IsNullOrEmpty(InObject.CZYDM))
                {
                    throw new Exception("要对账的操作员代码不能为空!");
                }
                string CZDM = string.Format(" and CAOZUOYDM='{0}'", InObject.CZYDM);
                DataTable ddxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00014, InObject.DZRQ, InObject.BASEINFO.FENYUANDM, CZDM));
                double ze = double.Parse(ddxx.Rows[0]["FYZE"].ToString());
                if (double.Parse(InObject.JIESUANZE) == ze)
                {
                    OutObject.DZJG = "0";
                }
                else if (double.Parse(InObject.JIESUANZE) > ze)
                {
                    OutObject.DZJG = "2";
                    OutObject.BZXX = string.Format("平台总额:{0}元,HIS总额{1}元,HIS多", ze, InObject.JIESUANZE);
                }
                else if (double.Parse(InObject.JIESUANZE) < ze)
                {
                    OutObject.DZJG = "1";
                    OutObject.BZXX = string.Format("平台总额:{0}元,HIS总额{1}元,平台多", ze, InObject.JIESUANZE);
                }
            }
            //日对账
            else if (InObject.DZLX == "2")
            {
                DataTable ddxx = DBVisitor.ExecuteTable(SqlLoad.GetFormat(SQ.HIS00014, InObject.DZRQ, InObject.BASEINFO.FENYUANDM, ""));
                double ze = double.Parse(ddxx.Rows[0]["FYZE"].ToString());
                if (double.Parse(InObject.JIESUANZE) == ze)
                {
                    OutObject.DZJG = "0";
                }
                else if (double.Parse(InObject.JIESUANZE) > ze)
                {
                    OutObject.DZJG = "2";
                    OutObject.BZXX = string.Format("平台总额:{0}元,HIS总额{1}元,HIS多", ze, InObject.JIESUANZE);
                }
                else if (double.Parse(InObject.JIESUANZE) < ze)
                {
                    OutObject.DZJG = "1";
                    OutObject.BZXX = string.Format("平台总额:{0}元,HIS总额{1}元,平台多", ze, InObject.JIESUANZE);
                }
            }
            else
            {
                throw new Exception("对账类型不正确!");
            }
        }
    }
}
