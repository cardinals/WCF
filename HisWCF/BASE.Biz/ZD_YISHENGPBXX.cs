using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{

    public class ZD_YISHENGPBXX : IMessage<ZD_YISHENGPBXX_IN, ZD_YISHENGPBXX_OUT>
    {
        public override void ProcessMessage()
        {
            string where = "";
            if (!string.IsNullOrEmpty(InObject.YSDM)) {
                where = " and YSGH='" + InObject.YSDM + "'";
            }
            #region sql查询
            var listbqxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00014, where));

            if (listbqxx.Count == 0)
            {
                throw new Exception(string.Format("无医生排班信息！"));
            }
            else
            {
                OutObject = new ZD_YISHENGPBXX_OUT();

                foreach (var bqxx in listbqxx)
                {
                    var pbxx = new PAIBANXX();
                    pbxx.JLXH = bqxx.Get("JLXH");
                    pbxx.XQ = bqxx.Get("XQ");
                    pbxx.KSMC = bqxx.Get("KSMC");
                    pbxx.KSDM = bqxx.Get("KSDM");
                    pbxx.MZLBMC = bqxx.Get("MZLBMC");
                    pbxx.SWXH = bqxx.Get("SWJSH");
                    pbxx.SWZGH = bqxx.Get("SWZGXH");
                    pbxx.XWXH = bqxx.Get("XWJSH");
                    pbxx.XWZGH = bqxx.Get("XWZGXH");
                    pbxx.YSGH = bqxx.Get("YSGH");
                    pbxx.YSXM = bqxx.Get("YSXM");
                    pbxx.SWYYXH = bqxx.Get("SWYYXH");
                    pbxx.XWYYXH = bqxx.Get("XWYYXH");
                    pbxx.YYF = bqxx.Get("YYF");
                    pbxx.YSMS = bqxx.Get("YSMS");
                    OutObject.PAIBANXX.Add(pbxx);
                }
            }
            #endregion
        }
    }
}
