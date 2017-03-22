using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JYCS.Schemas;
using SWSoft.Framework;
using System.Configuration;

namespace BASE.Biz
{
    public class ZD_KESHIXX : IMessage<ZD_KESHIXX_IN, ZD_KESHIXX_OUT>
    {
        public override void ProcessMessage()
        {
            #region sql查询
            var listksxx = DBVisitor.ExecuteModels(SqlLoad.GetFormat(SQ.BASE00008));

            if (listksxx.Count == 0)
            {
                throw new Exception(string.Format("无科室信息！"));
            }
            else
            {
                OutObject = new ZD_KESHIXX_OUT();

                foreach (var bqxx in listksxx)
                {
                    var bingqulb = new KESHIXX();
                    bingqulb.KESHIDM = bqxx.Get("KESHIDM");
                    bingqulb.KESHIMC = bqxx.Get("KESHIMC");
                    OutObject.KESHIMX.Add(bingqulb);
                }
            }
            #endregion
        }
    }
}
